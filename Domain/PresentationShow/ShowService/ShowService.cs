using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;

using Domain.PresentationShow.ShowCommon;
using TechnicalServices.Common.Locking;
using TechnicalServices.Common.Notification;
using TechnicalServices.Common.Utils;
using TechnicalServices.Communication.EquipmentController;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Interfaces.ConfigModule.Server;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;
using Timer = System.Timers.Timer;

namespace Domain.PresentationShow.ShowService
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Single,
        IncludeExceptionDetailInFaults = true)]
    public class ShowService : IShowCommon, IShowDisplayAndDeviceCommand, IDisposable
    {
        private readonly IServerConfiguration _config;
        private readonly IControllerChannel _controller;
        private readonly IExternalSystemCommand _externalSystemController;
        private readonly DisplayAndEquipmentMonitor _monitor;
        private readonly PresentationShowPreparator _showPreparator;
        private readonly BackgroundPresentationManager _backgroundPresentationManager;
        private readonly Dictionary<string, List<DisplayType>> _sourceIdDisplayMapping = new Dictionary<string, List<DisplayType>>();
        private readonly object _sourceIdDisplayMappingSyncObject = null;
        private readonly UserIdentity _systemUser;
        private readonly IPresentationWorker _worker;
        private readonly Scheduler _scheduler;
        //private readonly BlockedPresentationMonitor _blockedPresentationMonitor;
        private readonly Dictionary<Type, IModule> _mappingList = new Dictionary<Type, IModule>();
        private readonly FreezedEquipmentCollection _freezedEquipment = new FreezedEquipmentCollection();

        private readonly Stack<int> _slideShowHistory = new Stack<int>();
        private IShowNotifier _callback;
        private IShowNotifier _playerCallback;

        private PresentationInfo _currentInfo;
        private Slide _currentSlide;
        private int _isPlayerOpen;
        private IContextChannel _lastPlayerChannel;
        //private int _nowInPreparationProgress;
        private bool _presentationShowByPlayer = false;
        private bool _isPaused;
        private string _playerInstanceRemoteAddress = string.Empty;

        private readonly NotificationManager<IShowCommon>.NotificationStore<UserIdentity, IShowNotifier> _globalNotifier;

        private readonly Timer _playerMonitoringTimer = new Timer();
        private object _playerMonitoringTimerSync = new object();

        public ShowService(IServerConfiguration config, IPresentationWorker worker, UserIdentity identity)
        {
            Debug.Assert(config != null, "IServerConfiguration не может быть null");
            Debug.Assert(worker != null, "IPresentationWorker не может быть null");
            Debug.Assert(identity != null, "UserIdentity не может быть null");

            _sourceIdDisplayMappingSyncObject = ((ICollection)_sourceIdDisplayMapping).SyncRoot;
            _config = config;
            _worker = worker;
            _systemUser = identity;

            _globalNotifier = NotificationManager<IShowCommon>.Instance.
                RegisterDuplexService<UserIdentity, IShowNotifier>(NotifierBehaviour.OneInstance);

            _controller = ControllerFactory.CreateController(config.EventLog, config.ControllerLibrary, config.ControllerURI, config.ControllerReceiveTimeout, config.ControllerCheckTimeout);
            _externalSystemController = ExternalSystemControllerFactory.CreateController(
                config.ExternalSystemControllerLibrary, config.ExternalSystemControllerUri, config.EventLog);
            _scheduler = new Scheduler(_config);
            _scheduler.OnTick += new Action<int>(_scheduler_OnTick);
            _monitor = new DisplayAndEquipmentMonitor(_controller, _config);
            _monitor.OnStateChange += _monitor_OnStateChange;
            _externalSystemController.OnGoToLabel += _externalSystemController_OnGotoLabelReceive;
            _externalSystemController.OnGoToSlideById += _externalSystemController_OnGoToSlideById;
            _externalSystemController.OnGoToNextSlide += _externalSystemController_OnGoToNextSlide;
            _externalSystemController.OnGoToPrevSlide += _externalSystemController_OnGoToPrevSlide;
            //_controller.OnCheck += new EventHandler<DeviceCheckResultEventArgs>(_controller_OnCheck);
            foreach (IModule module in _config.ModuleList)
            {
                foreach (Type type in module.SystemModule.Presentation.GetDevice())
                    _mappingList.Add(type, module);
                foreach (Type type in module.SystemModule.Presentation.GetDisplay())
                    _mappingList.Add(type, module);
                foreach (Type type in module.SystemModule.Presentation.GetSource())
                    _mappingList.Add(type, module);

                // так же сделаем мапинг для типов из конфигурации чтобы мягко перейти везде где возможно на конфигурационные типы
                foreach (Type type in module.SystemModule.Configuration.GetDevice())
                    _mappingList.Add(type, module);
                foreach (Type type in module.SystemModule.Configuration.GetDisplay())
                    _mappingList.Add(type, module);
                foreach (Type type in module.SystemModule.Configuration.GetSource())
                    _mappingList.Add(type, module);

                module.ServerModule.Init(_config, module, _controller);
                WatchDog.WatchDogAction(_config.EventLog.WriteError, module.ServerModule.CheckLicense);
            }
            _worker.OnPresentationChanged += new EventHandler<PresentationChangedEventArgs>(_worker_OnPresentationChanged);
            _worker.OnSlideChanged += new EventHandler<SlideChangedEventArgs>(_worker_OnSlideChanged);

            _showPreparator = new PresentationShowPreparator(_config, _worker, _monitor, _mappingList);
            //_showPreparator.OnPreparationFinish += _showPreparator_OnPreparationFinish;
            //_showPreparator.OnResourceTransmit += _showPreparator_OnResourceTransmit;
            _backgroundPresentationManager = new BackgroundPresentationManager(_config,
                _worker, _showPreparator, _monitor, this, _systemUser);
            _backgroundPresentationManager.StartShow();

            _playerMonitoringTimer.Elapsed += new System.Timers.ElapsedEventHandler(_playerMonitoringTimer_Elapsed);
            _playerMonitoringTimer.Interval = TimeSpan.FromSeconds(_config.LoadSystemParameters().BackgroundPresentationRestoreTimeout).TotalMilliseconds;
            _playerMonitoringTimer.Start();
            //_blockedPresentationMonitor = new BlockedPresentationMonitor(_systemUser, _worker, _config);
        }

        #region IDisposable Members

        public void Dispose()
        {
            _currentSlide = null;
            _mappingList.Clear();
            if (_monitor != null) _monitor.Dispose();
            if (_scheduler != null) _scheduler.Dispose();
            foreach (IModule module in _config.ModuleList)
                module.ServerModule.Done();
            if (_controller != null) _controller.Dispose();
            if (_externalSystemController != null) _externalSystemController.Dispose();
            if (_backgroundPresentationManager != null) _backgroundPresentationManager.Dispose();
            if (_playerMonitoringTimer != null) _playerMonitoringTimer.Dispose();
            //if (_blockedPresentationMonitor != null) _blockedPresentationMonitor.Dispose();
        }

        #endregion

        //void _controller_OnCheck(object sender, DeviceCheckResultEventArgs e)
        //{
        //    string result = e.Result;
        //}

        #region IShowCommon Members

        public string DoEquipmentCommand(CommandDescriptor cmd)
        {
            //_callback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();

            if (cmd.EquipmentId < 0) return null;
            try
            {
                return _controller.Send(cmd);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService.DoEquipmentCommand: \n{0}", ex));
                return null;
            }
        }

        public void FreezeEquipmentSetting(EquipmentType equipmentType, FreezeStatus status)
        {
            _freezedEquipment.Set(equipmentType, status);
        }

        public EquipmentType[] GetFreezedEquipment()
        {
            return _freezedEquipment.GetFreezedEquipment();
        }

        public bool IsOnLine(EquipmentType equipmentType)
        {
            //_callback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();
            try
            {
                return _monitor.IsOnLine(equipmentType);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService.IsOnLine: \n{0}", ex));
            }
            return false;
        }

        public bool StartPlayer()
        {
            bool retValue = Interlocked.CompareExchange(ref _isPlayerOpen, 1, 0) == 0;
            if (!retValue)
            {
                if (_lastPlayerChannel != null
                    && _lastPlayerChannel.State != CommunicationState.Closed
                    && _lastPlayerChannel.State != CommunicationState.Closing
                    && _lastPlayerChannel.State != CommunicationState.Faulted)
                {
                    try
                    {
                        if (_playerCallback != null)
                            _playerCallback.Ping();
                    }
                    catch (Exception ex)
                    {
                        _config.EventLog.WriteWarning(string.Format("ShowService.StartPlayer: Мертвый канал ранне запущенного плеера, новый запуск разрешен\n {0}", ex));
                        _lastPlayerChannel = OperationContext.Current.Channel;
                        _playerInstanceRemoteAddress = GetRemoteAddress(OperationContext.Current);
                        _playerCallback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();
                        return true;
                    }
                    _config.EventLog.WriteInformation(string.Format("ShowService.StartPlayer: Запущен другой экземпляр плеера по адресу: {0}", _playerInstanceRemoteAddress));
                    return false;
                }
                else
                {
                    _lastPlayerChannel = OperationContext.Current.Channel;
                    _playerInstanceRemoteAddress = GetRemoteAddress(OperationContext.Current);
                    _playerCallback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();
                    return true;
                }
            }
            else
            {
                _lastPlayerChannel = OperationContext.Current.Channel;
                _playerInstanceRemoteAddress = GetRemoteAddress(OperationContext.Current);
                _playerCallback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();
                return true;
            }
        }

        public void StopPlayer()
        {
            Interlocked.CompareExchange(ref _isPlayerOpen, 0, 1);
            _lastPlayerChannel = null;
            _playerCallback = null;
        }

        public bool IsShownByPlayer()
        {
            return PresentationShowByPlayer;
        }

        public void SubscribeForNotification(UserIdentity identity)
        {
            _globalNotifier.Subscribe(this, identity);
        }

        public void UnSubscribeForNotification(UserIdentity identity)
        {
            _globalNotifier.Unsubscribe(this, identity);
        }

        public MemoryStream[] CaptureScreen(DisplayType[] displayList)
        {
            Debug.Assert(displayList != null, "displayList не может быть null");

            //_callback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();

            List<MemoryStream> result = new List<MemoryStream>(displayList.Length);
            foreach (DisplayType displayType in displayList)
            {
                MemoryStream screen = null;
                if (displayType.SupportsCaptureScreen)
                {
                    Type type = displayType.GetType();
                    if (_mappingList.ContainsKey(type))
                    {
                        IModule module = _mappingList[type];
                        screen = module.ServerModule.CaptureScreen(displayType);
                    }
                }
                result.Add(screen);
            }
            return result.ToArray();
        }

        public PresentationShowPrepareStatus CheckStatus(UserIdentity userIdentity, PresentationInfo info)
        {
            return CheckStatus(userIdentity, info.UniqueName);
        }

        public PresentationShowPrepareStatus CheckStatus(UserIdentity userIdentity, string uniqueName)
        {
            //_callback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();

            if (_showPreparator.IsPreparationInProgress)
                return PresentationShowPrepareStatus.ResourceLoading;
            else
                return PresentationShowPrepareStatus.ReadyToShow;
        }

        //public bool Load(UserIdentity userIdentity, PresentationInfo info)
        //{
        //    return Load(userIdentity, info.UniqueName);
        //}

        public bool Load(UserIdentity userIdentity, string uniqueName)
        {
            return Load(userIdentity, uniqueName, -1, false);
        }

        public bool Load(UserIdentity userIdentity, string uniqueName, int slideId, bool autoPrepare)
        {
            // если кэширование в процессе - отлуп
            //if (Interlocked.CompareExchange(ref _nowInPreparationProgress, 1, 0) == 1) return false;
            _showPreparator.AutoPrepare = autoPrepare; // Пусть подготовится автоматически, если надо.
            bool isSuccess = _showPreparator.StartPreparation(userIdentity, uniqueName, slideId,
                                             AskClientForNotEnoughFreeSpace,
                                             _showPreparator_OnPreparationFinish,
                                             _showPreparator_OnResourceTransmit,
                                             _showPreparator_OnReceiveAgentResourcesList,
                                             _showPreparator_OnUploadSpeed,
                                             _showPreparator_OnPreparationForDisplayEnded,
                                             _showPreparator_OnLogMessage);
            if (isSuccess)
            {
                _callback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();
            }
            return isSuccess;
        }

        public bool TerminateLoad(TerminateLoadCommand command, string display)
        {
            try
            {
                _showPreparator.Terminate(command, display);
                //Interlocked.CompareExchange(ref _nowInPreparationProgress, 0, 1);
                return true;
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(String.Format("ShowService.TerminateLoad: Возникла ошибка \n {0}", ex));
                return false;
            }
        }

        public void ResponseForNotEnoughFreeSpaceRequest(DisplayType displayType, AgentAction agentAction)
        {
            _showPreparator.ResponseForNotEnoughFreeSpaceRequest(displayType, agentAction);
        }

        public PreparationResult GetPreparationResult()
        {
            //_callback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();
            return _showPreparator.GetPreparationResult();
        }

        public bool Start(UserIdentity identity, PresentationInfo info)
        {
            _slideShowHistory.Clear();
            _freezedEquipment.Clear();
            _currentInfo = info;
            _currentSlide = null;
            // вместо лока - проверка на лок
            //bool isSuccess = _worker.AcquireLockForPresentation(_systemUser, _currentInfo.UniqueName,
            //                                                    RequireLock.ForShow);
            LockingInfo[] lockingInfos = _worker.GetLockingInfo(new ObjectKey[] { ObjectKeyCreator.CreatePresentationKey(info.UniqueName) });
            if (lockingInfos.Length == 0) return false;
            //if (!isSuccess) return false;
            _backgroundPresentationManager.StopShow();
            _controller.Start();
            PresentationShowByPlayer = true;
            //_sourceIdDisplayMapping.Clear();
            _scheduler.Start(info);
            //_blockedPresentationMonitor.AddForWatch(identity, _lastPlayerChannel, info);

            StartController(info);
            return true;
        }

        public bool Stop(UserIdentity identity, PresentationInfo info)
        {
            _slideShowHistory.Clear();
            _freezedEquipment.Clear();
            _controller.Stop();
            _scheduler.Stop();
            if (_currentSlide != null)
                _worker.ReleaseLockForSlide(_systemUser, info.UniqueName,
                                            _currentSlide.Id);
            //_blockedPresentationMonitor.RemoveFromWatch(identity);
            _backgroundPresentationManager.StartShow();
            //_worker.ReleaseLockForPresentation(_systemUser, _currentInfo.UniqueName);

            StopController();
            _currentInfo = null;
            _currentSlide = null;
            PresentationShowByPlayer = false;
            return true;
        }

        private string GetRemoteAddress(OperationContext context)
        {
            RemoteEndpointMessageProperty endpointMessageProperty =
                context.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as
                RemoteEndpointMessageProperty;
            return endpointMessageProperty == null ? string.Empty : endpointMessageProperty.Address;
        }

        private bool StartController(PresentationInfo info)
        {
            string result;
            CommandDescriptor cmd;

            result = _controller.Send(new CommandDescriptor(0, "GeneralScenarioStart"));
            //if (result != "OK") throw new ApplicationException();

            GeneralFullSceneListAndGeneralMarkedSceneListCommand(info);
            return true;
        }

        private void StopController()
        {
            _controller.Send(new CommandDescriptor(0, "GeneralScenarioStop"));
        }

        private Label getLabel(Slide current)
        {
            Slide item = current;
            Label label = _config.ModuleConfiguration.LabelList.FirstOrDefault(l => l.Id == item.LabelId);
            return label;
        }

        private Label getLabel(SlideInfo current)
        {
            SlideInfo item = current;
            Label label = _config.ModuleConfiguration.LabelList.FirstOrDefault(l => l.Id == item.LabelId);
            return label;
        }


        public bool Pause(string presentationUniqueName)
        {
            if (_currentInfo == null ||
                _currentSlide == null ||
                !_currentInfo.UniqueName.Equals(presentationUniqueName, StringComparison.InvariantCultureIgnoreCase))
                return false;
            // стопим только для текущих дисплеев
            HashSet<IModule> moduleList = new HashSet<IModule>();
            foreach (Display display in _currentSlide.DisplayList)
            {
                IModule module;
                if (!_mappingList.TryGetValue(display.GetType(), out module)) continue;
                moduleList.Add(module);
            }
            List<IAsyncResult> asyncResults = new List<IAsyncResult>(moduleList.Count);
            foreach (IModule module in moduleList)
            {
                asyncResults.Add(AsyncCaller.BeginCall<IModule>(Pause, module));
            }
            foreach (IAsyncResult asyncResult in asyncResults)
            {
                AsyncCaller.EndCall<IModule>(asyncResult);
            }
            _isPaused = true;
            StopController();
            return true;
        }

        private void Pause(IModule module)
        {
            try
            {
                module.ServerModule.Pause();
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowServer.Pause: Ошибка при выполнении команды Pause для модуля {0}\n{1}",
                    module.Name, ex));
            }
        }

        public ShowSlideResult ShowSlideBySlideInfo(PresentationInfo info, SlideInfo slide, out bool isPrevEnable, out bool isNextEnable)
        {
            return ShowSlide(info.UniqueName, slide.Id, out isPrevEnable, out isNextEnable);
        }

        //public bool ShowSlideBySlideId(PresentationInfo info, int slideId, out bool isPrevEnable, out bool isNextEnable)
        //{
        //    return ShowSlide(info.UniqueName, slideId, out isPrevEnable, out isNextEnable);
        //}

        public ShowSlideResult ShowSlide(string uname, SlideInfo slide, out bool isPrevEnable, out bool isNextEnable)
        {
            return ShowSlide(uname, slide.Id, out isPrevEnable, out isNextEnable);
        }

        public ShowSlideResult ShowSlideByLabelId(PresentationInfo info, int labelId, out bool isPrevEnable, out bool isNextEnable)
        {
            GetPrevNextSlideStatus(info, _currentSlide == null ? (int?)null : _currentSlide.Id,
                out isPrevEnable, out isNextEnable);
            // находим по label id сладйа
            SlideInfo slideInfo =
                info.SlideInfoList.SingleOrDefault(
                    sl => sl.LabelId == labelId);
            if (null == slideInfo)
            {
                string error = string.Format(
                    "В сценарии {0} не найдено сцены с меткой '{1}'", info.Name, labelId);
                _config.EventLog.WriteWarning(error);
                return new ShowSlideResult(false, error);
            }
            return ShowSlide(info.UniqueName, slideInfo.Id, out isPrevEnable, out isNextEnable);
        }

        public ShowSlideResult GoToPrevSlide(out int slideId, out bool isPrevEnable, out bool isNextEnable)
        {
            isPrevEnable = false;
            isNextEnable = false;
            slideId = _currentSlide == null ? -1 : _currentSlide.Id;
            if (!PresentationShowByPlayer || _isPaused || _currentInfo == null)
                return new ShowSlideResult(false, "Мы не в режиме показа");
            PresentationInfo info = _worker.GetPresentationInfo(_currentInfo.UniqueName);
            GetPrevNextSlideStatus(info, _currentSlide == null ? (int?)null : _currentSlide.Id,
                out isPrevEnable, out isNextEnable);
            if (!isPrevEnable) return new ShowSlideResult(false, "Предыдущий слайд не доступен");
            int prevSlideId = _slideShowHistory.Pop();
            ShowSlideResult isSuccess = ShowSlide(info.UniqueName, prevSlideId, false,
                out isPrevEnable, out isNextEnable);
            if (isSuccess.IsSuccess)
            {
                slideId = prevSlideId;
            }
            return isSuccess;
        }

        public ShowSlideResult GoToNextSlide(out int slideId, out bool isPrevEnable, out bool isNextEnable)
        {
            isPrevEnable = false;
            isNextEnable = false;
            slideId = -1;
            if (!PresentationShowByPlayer || _isPaused || _currentInfo == null || _currentSlide == null)
                return new ShowSlideResult(false, "Мы не в режиме показа");
            PresentationInfo info = _worker.GetPresentationInfo(_currentInfo.UniqueName);
            GetPrevNextSlideStatus(info, _currentSlide.Id,
                out isPrevEnable, out isNextEnable);
            if (isNextEnable)
            {
                IList<LinkInfo> list;
                if (info.SlideLinkInfoList.TryGetValue(_currentSlide.Id, out list))
                {
                    LinkInfo linkInfo = list.OrderByDescending(li => li.IsDefault).FirstOrDefault();
                    if (linkInfo != null)
                    {
                        int nextSlideId = linkInfo.NextSlideId;
                        return GoToNextSlide(nextSlideId, out slideId, out isPrevEnable, out isNextEnable);
                    }
                }
            }
            return new ShowSlideResult(false, "Следующий слайд недоступен");
        }

        public ShowSlideResult GoToNextSlide(int nextSlideId, out int slideId,
            out bool isPrevEnable, out bool isNextEnable)
        {
            isPrevEnable = false;
            isNextEnable = false;
            slideId = _currentSlide == null ? -1 : _currentSlide.Id;
            if (!PresentationShowByPlayer || _isPaused || _currentInfo == null)
                return new ShowSlideResult(false, "Мы не в режиме показа");
            ShowSlideResult isSuccess = ShowSlide(_currentInfo.UniqueName, nextSlideId, out isPrevEnable, out isNextEnable);
            if (isSuccess.IsSuccess)
                slideId = _currentSlide == null ? -1 : _currentSlide.Id;
            return isSuccess;
        }

        public String DoSourceCommand(string sourceId, String command)
        {
            //_callback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();

            IModule module;
            List<DisplayType> displays;
            if (!_sourceIdDisplayMapping.TryGetValue(sourceId, out displays)) return null;
            List<string> responses = new List<string>();
            foreach (DisplayType displayType in displays)
            {
                if (_mappingList.TryGetValue(displayType.GetType(), out module))
                {
                    responses.Add(module.ServerModule.DoSourceCommand(displayType, sourceId, command));
                }
            }
            return responses.FirstOrDefault(str => !string.IsNullOrEmpty(str));
        }

        #endregion

        #region Implementation of IPing

        public void Ping(UserIdentity identity)
        {
            _globalNotifier.RefreshSubscribers(identity);
            //_callback = OperationContext.Current.GetCallbackChannel<IShowNotifier>();
        }

        #endregion

        #region IShowDisplayAndDeviceCommand

        public void ShowDisplays(string presentationUniqueName, Slide slide)
        {
            _sourceIdDisplayMapping.Clear();
            List<IAsyncResult> alist = new List<IAsyncResult>(_config.ModuleConfiguration.DisplayList.Count);
            foreach (DisplayType displayType in _config.ModuleConfiguration.DisplayList)
            // IsClient ModuleConfiguration.DisplayList)
            {
                Display display = null;
                if ((display = slide.DisplayList.Find(dis => dis.Type.Equals(displayType))) == null)
                {
                    display = displayType.CreateNewDisplay();
                }
                alist.Add(AsyncCaller.BeginCall<Display, string>(ShowDisplay, display, presentationUniqueName));
                //ShowDisplay(display, presentationUniqueName);

                //foreach (Display display in slide.DisplayList)
                //{
                //    IModule module;
                //    if (_mappingList.TryGetValue(display.GetType(), out module))
                //    {
                //        module.ServerModule.ShowDisplay(display, presentationUniqueName);
                //    }
                //    foreach (Window window in display.WindowList)
                //    {
                //        _sourceIdDisplayMapping[window.Source.Id] = display;
                //    }
                //}
            }
            foreach (IAsyncResult result in alist)
            {
                AsyncCaller.EndCall<Display, string>(result);
            }
        }

        public void ShowDisplay(Display display, string presentationUniqueName)
        {
            IModule module;
            if (_mappingList.TryGetValue(display.GetType(), out module))
            {
                BackgroundImageDescriptor backgroundImageDescriptor = null;
                ActiveDisplay activeDisplay = display as ActiveDisplay;
                if (activeDisplay != null && !string.IsNullOrEmpty(activeDisplay.BackgroundImage))
                {
                    ResourceDescriptor descriptor = _worker.SourceDAL.GetStoredSource(
                        activeDisplay.BackgroundImage, Constants.BackGroundImage, presentationUniqueName);
                    backgroundImageDescriptor = descriptor as BackgroundImageDescriptor;
                }
                module.ServerModule.ShowDisplay(display, backgroundImageDescriptor);
            }
            foreach (Window window in display.WindowList)
            {
                lock (_sourceIdDisplayMappingSyncObject)
                {
                    List<DisplayType> list;
                    if (!_sourceIdDisplayMapping.TryGetValue(window.Source.Id, out list))
                    {
                        _sourceIdDisplayMapping[window.Source.Id] = list = new List<DisplayType>();
                    }
                    if (!list.Contains(display.Type))
                        list.Add(display.Type);
                }
            }
        }

        public void ComposeCommandAndSendToControllerForDevice(Device device, Slide prevSlide, Slide currentSlide)
        {
            if (!device.Type.IsHardware || device.Type.UID <= 0) return;
            IModule module;
            if (!_mappingList.TryGetValue(device.GetType(), out module)) return;
            try
            {
                foreach (CommandDescriptor commandDescriptor in module.ServerModule.GetCommand(prevSlide, currentSlide, _freezedEquipment.GetFreezedEquipment()))
                {
                    DoEquipmentCommand(commandDescriptor);
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteWarning(string.Format("ShowService.ComposeCommandAndSendToControllerForDevice\n {0}", ex));
            }
        }


        internal class LogicSetTieParameter
        {
            public readonly int Input;
            public readonly int Output;
            public readonly int State;

            internal LogicSetTieParameter(int input, int output, int state)
            {
                Input = input;
                Output = output;
                State = state;
            }
        }

        protected List<CommandDescriptor> PreprocessingCommandList(List<CommandDescriptor> list)
        {
            List<LogicSetTieParameter> paramList = new List<LogicSetTieParameter>();
            List<CommandDescriptor> result = new List<CommandDescriptor>();
            foreach (CommandDescriptor commandDescriptor in list)
            {
                if (commandDescriptor.EquipmentId == 0  && 
                    commandDescriptor.CommandName == "LogicSetTie" && 
                    commandDescriptor.Parameters.Count == 3)
                {
                    paramList.Add(new LogicSetTieParameter(
                        (int)commandDescriptor.Parameters[0],
                        (int)commandDescriptor.Parameters[1],
                        (int)commandDescriptor.Parameters[2]));
                }
                else result.Add(commandDescriptor);
            }
            if (paramList.Count > 0)
            {
                CommandDescriptor cmd = new CommandDescriptor(0, "LogicSetTies", paramList.Count);
                foreach (LogicSetTieParameter parameter in paramList)
                {
                    cmd.Parameters.Add(parameter.Input);
                    cmd.Parameters.Add(parameter.Output);
                    cmd.Parameters.Add(parameter.State);
                }
                result.Add(cmd);
            }
            return result;
        }

        public void ComposeCommandAndSendToControllerForAllDevice(Slide prevSlide, Slide currentSlide)
        {
            try
            {
                List<CommandDescriptor> commandDescriptors = new List<CommandDescriptor>(100);
                foreach (IModule module in _config.ModuleList)
                {
                    commandDescriptors.AddRange(module.ServerModule.GetCommand(prevSlide, currentSlide, _freezedEquipment.GetFreezedEquipment()));
                }

                commandDescriptors = PreprocessingCommandList(commandDescriptors);

                foreach (CommandDescriptor commandDescriptor in commandDescriptors)
                {
                    DoEquipmentCommand(commandDescriptor);
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService.ComposeCommandAndSendToControllerForAllDevice: {0}", ex));
            }
        }

        #endregion

        #region команды контроллеру

        private void GeneralFullSceneListAndGeneralMarkedSceneListCommand(PresentationInfo info)
        {
            try
            {
                //GeneralFullSceneList
                string result;
                List<IConvertible> paramList = new List<IConvertible>(info.SlideCount << 1);
                paramList.Add(info.SlideCount);
                foreach (SlideInfo slideInfo in info.SlideInfoList)
                {
                    paramList.Add(slideInfo.Id);
                    paramList.Add(slideInfo.Name);
                    paramList.Add(String.IsNullOrEmpty(slideInfo.Comment) ? "" : slideInfo.Comment);
                }
                CommandDescriptor cmd = new CommandDescriptor(0, "GeneralFullSceneList", paramList.ToArray());
                result = _controller.Send(cmd);

                //GeneralMarkedSceneList
                paramList.Clear();
                int count = 0;
                foreach (SlideInfo slideInfo in info.SlideInfoList)
                {
                    if (slideInfo.LabelId == 0) continue;
                    // Копия чтобы решарпер не ругался
                    SlideInfo item = slideInfo;
                    Label label = _config.ModuleConfiguration.LabelList.FirstOrDefault(l => l.Id == item.LabelId);
                    if (label != null)
                    {
                        paramList.Add(label.Name);
                        paramList.Add(slideInfo.Name);
                        count++;
                    }
                }
                paramList.Insert(0, count);
                cmd = new CommandDescriptor(0, "GeneralMarkedSceneList", paramList.ToArray());
                result = _controller.Send(cmd);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService.GeneralFullSceneListAndGeneralMarkedSceneListCommand:\n {0}", ex));
            }
        }

        private void ChangeSlideController(PresentationInfo info, SlideInfo prev, SlideInfo current)
        {
            try
            {
                List<IConvertible> paramList = new List<IConvertible>();
                Label label = getLabel(current);

                paramList.Add(current.Id);
                paramList.Add((label == null) ? "" : label.Name);
                paramList.Add(String.IsNullOrEmpty(current.Name) ? "" : current.Name);
                paramList.Add(String.IsNullOrEmpty(current.Comment) ? "" : current.Comment);
                paramList.Add((prev == null) ? 0 : prev.Id);
                paramList.Add((prev == null) ? "" : String.IsNullOrEmpty(prev.Name) ? "" : prev.Name);
                paramList.Add((prev == null) ? "" : String.IsNullOrEmpty(prev.Comment) ? "" : prev.Comment);
                _controller.Send(new CommandDescriptor(0, "GeneralSceneChange", paramList.ToArray()));
                paramList.Clear();


                IList<LinkInfo> linkList;
                if (info.SlideLinkInfoList.TryGetValue(current.Id, out linkList))
                {
                    paramList.Add(linkList.Count);
                    foreach (LinkInfo item in linkList)
                    {
                        LinkInfo link = item;
                        SlideInfo slideInfo = info.SlideInfoList.First(x => x.Id == link.NextSlideId);
                        paramList.Add(slideInfo.Id);
                        paramList.Add(slideInfo.Name);
                    }
                }
                else paramList.Add(0);
                _controller.Send(new CommandDescriptor(0, "GeneralNextSceneList", paramList.ToArray()));
                paramList.Clear();

                IEnumerable<int> result = info.SlideLinkInfoList.Where(li => li.Value.Where(l => l.NextSlideId == current.Id).Count() > 0).Select(x => x.Key);
                foreach (int item in result)
                {
                    int id = item;
                    SlideInfo slideInfo = info.SlideInfoList.First(x => x.Id == id);
                    paramList.Add(slideInfo.Id);
                    paramList.Add(slideInfo.Name);
                }
                paramList.Insert(0, result.Count());
                _controller.Send(new CommandDescriptor(0, "GeneralPreviousSceneList", paramList.ToArray()));
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService.ChangeSlideController\n {0}", ex));
            }
        }

        #endregion

        public void CloseWindows()
        {
            try
            {
                foreach (DisplayType display in _config.ModuleConfiguration.DisplayList)
                {
                    IModule module;
                    if (_mappingList.TryGetValue(display.GetType(), out module))
                        module.ServerModule.CloseWindows();
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService.CloseWindows: {0}", ex));
            }
        }

        public ShowSlideResult ShowSlide(string presentationUniqueName, int slideId, out bool isPrevEnable, out bool isNextEnable)
        {
            return ShowSlide(presentationUniqueName, slideId, true, out isPrevEnable, out isNextEnable);
        }

        private ShowSlideResult ShowSlide(string presentationUniqueName, int slideId, bool addToHistory, out bool isPrevEnable, out bool isNextEnable)
        {
            lock (this)
            {
                _backgroundPresentationManager.PauseShow();
                PresentationInfo info = _worker.GetPresentationInfo(presentationUniqueName);
                GetPrevNextSlideStatus(info, _currentSlide == null ? (int?)null : _currentSlide.Id,
                    out isPrevEnable, out isNextEnable);
                //if (_currentSlide != null && _currentSlide.Id == slideId) return false;
                try
                {
                    // если кэширование сцены в процессе - то возвращаем отлуп
                    if (_showPreparator.IsSlidePreparationInProgress(presentationUniqueName, slideId))
                        return new ShowSlideResult(false, "Выполняется копирование источников. До окончания кеширования показ сцены невозможен.");
                    // достаем слайд
                    Slide slide = _worker.LoadSlides(presentationUniqueName, new[] { slideId }).SingleOrDefault();
                    if (slide == null) return new ShowSlideResult(false, string.Format("Слайд с индексом {0} не найден в сценарии {1}", slideId, presentationUniqueName));
                    if (!ReleaseLockForPreviouseSlideAndAcquireForCurrent(presentationUniqueName, _currentSlide, slide)) return new ShowSlideResult(false, "Не удалось снять блокировку с предыдущего слайда и залочить следующий");

                    //команды которые нужно отправить контроллеру
                    IAsyncResult controllerAr = AsyncCaller.BeginCall<PresentationInfo, Slide, Slide>(ControllerActions, info, _currentSlide, slide);
                    //ControllerActions(info, _currentSlide, slide);

                    // пробегаем по всем дисплеям и отправляем в соответсвующие плагины команды на показ
                    IAsyncResult showDisplayAr = AsyncCaller.BeginCall<string, Slide>(ShowDisplays, presentationUniqueName, slide);
                    //ShowDisplays(presentationUniqueName, slide);

                    if (controllerAr != null)
                        AsyncCaller.EndCall<PresentationInfo, Slide, Slide>(controllerAr);
                    if (showDisplayAr != null)
                        AsyncCaller.EndCall<string, Slide>(showDisplayAr);

                    Slide prevSlide = _currentSlide;
                    _currentSlide = slide;
                    if (PresentationShowByPlayer) _isPaused = false;
                    if (PresentationShowByPlayer && addToHistory && prevSlide != null)
                    {
                        if ((_slideShowHistory.Count > 0 ? _slideShowHistory.Peek() : -1) != prevSlide.Id)
                            _slideShowHistory.Push(prevSlide.Id);
                    }
                    GetPrevNextSlideStatus(info, _currentSlide == null ? (int?)null : _currentSlide.Id,
                        out isPrevEnable, out isNextEnable);
                    return ShowSlideResult.Ok;
                }
                catch (Exception ex)
                {
                    if (_currentSlide != null)
                    {
                        // надо разлочить предыдущий слайд
                        _worker.ReleaseLockForSlide(_systemUser, presentationUniqueName, _currentSlide.Id);
                    }
                    _config.EventLog.WriteError(
                        string.Format("ShowService.ShowSlide: presentationUniqueName: {0}, slideId: {1}\n{2} ",
                                      presentationUniqueName, slideId, ex));
                    return new ShowSlideResult(false, ex.ToString());
                }
            }
        }

        private void ControllerActions(PresentationInfo info, Slide prevSlide, Slide nextSlide)
        {
            try
            {
                // Отправляем контроллеру информацию о слайде и вариантах перехода
                if (_isPaused) StartController(info);
                ChangeSlideController(info,
                                      prevSlide == null ? null : new SlideInfo(info, prevSlide),
                                      new SlideInfo(info, nextSlide));

                // формируем соответсующие команды хардварному оборудованию и посылаем команды контроллеру
                ComposeCommandAndSendToControllerForAllDevice(prevSlide, nextSlide);
            }
            catch(Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService.ControllerActions:\n{0}",ex));
            }
        }

        private bool ReleaseLockForPreviouseSlideAndAcquireForCurrent(string presentationUniqueName, Slide prevSlide, Slide nextSlide)
        {
            if (PresentationShowByPlayer)
            {
                if (prevSlide != null)
                {
                    // надо разлочить предыдущий слайд
                    if (prevSlide.Id != nextSlide.Id)
                    {
                        bool isSuccess = _worker.ReleaseLockForSlide(_systemUser, presentationUniqueName, prevSlide.Id);
                        if (!isSuccess)
                            _config.EventLog.WriteWarning(string.Format("Не удалось разлочить слайд {0}", prevSlide.Id));
                    }
                }
                if ((prevSlide == null) || (prevSlide.Id != nextSlide.Id))
                    return _worker.AcquireLockForSlide(_lastPlayerChannel, _systemUser, presentationUniqueName,
                                                             nextSlide.Id, RequireLock.ForShow);
            }
            return true;
        }

        private void GetPrevNextSlideStatus(PresentationInfo info, int? slideId, out bool isPrevEnable, out bool isNextEnable)
        {
            isPrevEnable = false;
            if ((_slideShowHistory.Count != 0) && (slideId.HasValue) && 
                (_slideShowHistory.Peek() != slideId.Value))
                isPrevEnable = true;
            isNextEnable = false;
            if (slideId.HasValue && info != null)
            {
                IList<LinkInfo> list;
                if (info.SlideLinkInfoList.TryGetValue(slideId.Value, out list))
                {
                    isNextEnable = list.Count != 0;
                }
            }
        }

        internal void AskClientForNotEnoughFreeSpace(DisplayType displayType)
        {
            _callback.NotEnoughFreeSpaceRequest(displayType);
        }


        private void _showPreparator_OnResourceTransmit(/*object sender,*/ PartSendEventArgs e)
        {
            if (_callback != null)
            {
                try
                {
                    _callback.UploadProgress(e.Part, e.NumberOfParts, e.DisplayName);
                }
                catch (Exception ex)
                {
                    _config.EventLog.WriteError(string.Format("ShowService:OnResourceTransmit \n {0}", ex));
                }
            }
        }

        private void _showPreparator_OnReceiveAgentResourcesList(DisplayType[] agents, int[] resources)
        {
            if (_callback != null)
            {
                try
                {
                    _callback.ReceiveAgentResourcesList(agents, resources);
                }
                catch (Exception ex)
                {
                    _config.EventLog.WriteError(string.Format("ShowService:OnReceiveAgentResourcesList \n {0}", ex));
                }
            }
        }

        private void _showPreparator_OnUploadSpeed(double speed, string display, string file)
        {
            if (_callback != null)
            {
                try
                {
                    _callback.UploadSpeed(speed, display, file);
                }
                catch (Exception ex)
                {
                    _config.EventLog.WriteError(string.Format("ShowService:OnUploadSpeed \n {0}", ex));
                }
            }
        }

        private void _showPreparator_OnLogMessage(string message)
        {
            if (_callback != null)
            {
                try
                {
                    _callback.LogMessage(message);
                }
                catch (Exception ex)
                {
                    _config.EventLog.WriteError(string.Format("ShowService:nLogMessage \n {0}", ex));
                }
            }
        }

        private void _showPreparator_OnPreparationForDisplayEnded(string display, bool allOk)
        {
            if (_callback != null)
            {
                try
                {
                    _callback.PreparationForDisplayEnded(display, allOk);
                }
                catch (Exception ex)
                {
                    _config.EventLog.WriteError(string.Format("ShowService:OnPreparationForDisplayEnded \n {0}", ex));
                }
            }
        }


        private void _showPreparator_OnPreparationFinish(/*object sender, EventArgs e*/)
        {
            if (_callback != null)
            {
                try
                {
                    _callback.PreparationFinished();
                }
                catch (Exception ex)
                {
                    _config.EventLog.WriteError(string.Format("ShowService:StartPreparation \n {0}", ex));
                }
            }
            //_showPreparator.OnResourceTransmit -= _showPreparator_OnResourceTransmit;
            //Interlocked.CompareExchange(ref _nowInPreparationProgress, 0, 1);
        }

        private static void CheckLicense(int agentCount)
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckAgentCount(agentCount);
        }

        private void _monitor_OnStateChange(object sender, EqiupmentStateChangeEventArgs e)
        {
            try
            {
                // проверка на число подключенных активных дисплеев
                DisplayType displayType = e.EquipmentType as DisplayType;
                if (displayType != null && !displayType.IsHardware && e.IsOnLine)
                {
                    WatchDog.WatchDogAction(_config.EventLog.WriteError, CheckLicense, _monitor.GetActiveDisplayNumber());
                }
                _globalNotifier.Notify(null, this, "EquipmentStateChange", e.EquipmentType, e.IsOnLine);
                //if (_callback != null) _callback.EquipmentStateChange(e.EquipmentType, e.IsOnLine);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService._monitor_OnStateChange \n {0}", ex));
            }
        }

        #region External System Command Processing
        private PresentationInfo ExternalSystemCommandPreprocessing()
        {
            if (!PresentationShowByPlayer || _isPaused)
                throw new ApplicationException(
                    "Получена команда от внешней системы, но мы не находимся в режиме показа");
            if (null == _currentInfo)
                throw new ApplicationException(
                    "Получена команда от внешней системы, но неизвестна текущий сценарий");
            // сценарий мог изменится - получим текущую версию
            PresentationInfo info = _worker.GetPresentationInfo(_currentInfo.UniqueName);
            if (null == info)
                throw new ApplicationException(string.Format(
                                                   "Получена команда от внешней системы, но сценарий {0} видимо уже удалена", _currentInfo.Name));
            return info;
        }

        private bool _externalSystemController_OnGotoLabelReceive(string labelName)
        {
            try
            {
                PresentationInfo info = ExternalSystemCommandPreprocessing();
                // найдем метку
                Label label = _config.LabelStorageAdapter.GetLabelStorage().SingleOrDefault(
                    lb => lb.Name.Equals(labelName, StringComparison.InvariantCultureIgnoreCase));
                if (null == label)
                    throw new ApplicationException(string.Format(
                        "Получена команда от внешней системы, но такой мекти '{0}' не найдено", labelName));
                SlideInfo slideInfo = info.SlideInfoList.SingleOrDefault(
                        sl => sl.LabelId == label.Id);
                if (null == slideInfo)
                    throw new ApplicationException(string.Format(
                        "Получена команда от внешней системы, но сцены с меткой '{0}' не найдено в сценарии {1}", labelName, info.Name));
                bool isPrevEnable, isNextEnable;
                if (ShowSlide(info.UniqueName, slideInfo.Id, out isPrevEnable, out isNextEnable).IsSuccess)
                {
                    _globalNotifier.Notify(null, this, "GotoSlideByExternalSystem", slideInfo.Id, isPrevEnable, isNextEnable);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService._externalSystemController_OnGotoLabelReceive: LabelName '{0}' \n {1}", labelName, ex));
            }
            return false;
        }

        bool _externalSystemController_OnGoToSlideById(string slideId)
        {
            try
            {
                int _slideId = Int32.Parse(slideId);
                PresentationInfo info = ExternalSystemCommandPreprocessing();
                bool isPrevEnable, isNextEnable;
                if (ShowSlide(info.UniqueName, _slideId, out isPrevEnable, out isNextEnable).IsSuccess)
                {
                    _globalNotifier.Notify(null, this, "GotoSlideByExternalSystem", _slideId, isPrevEnable, isNextEnable);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService._externalSystemController_OnGoToSlideById: SlideId '{0}' \n {1}", slideId, ex));
            }
            return false;
        }

        bool _externalSystemController_OnGoToPrevSlide()
        {
            try
            {
                int slideId;
                bool isPrevEnable;
                bool isNextEnable;
                if (GoToPrevSlide(out slideId, out isPrevEnable, out isNextEnable).IsSuccess)
                {
                    _globalNotifier.Notify(null, this, "GotoSlideByExternalSystem", slideId, isPrevEnable, isNextEnable);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService._externalSystemController_OnGoToSlideById: \n{0}", ex));
            }
            return false;
        }

        bool _externalSystemController_OnGoToNextSlide()
        {
            try
            {
                int slideId;
                bool isPrevEnable;
                bool isNextEnable;
                if (GoToNextSlide(out slideId, out isPrevEnable, out isNextEnable).IsSuccess)
                {
                    _globalNotifier.Notify(null, this, "GotoSlideByExternalSystem", slideId, isPrevEnable, isNextEnable);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService._externalSystemController_OnGoToSlideById: \n {0}", ex));
            }
            return false;
        }

        #endregion

        private bool PresentationShowByPlayer
        {
            get
            {
                return _presentationShowByPlayer;
            }
            set
            {
                if (_presentationShowByPlayer != value)
                {
                    _presentationShowByPlayer = value;
                    try
                    {
                        _globalNotifier.Notify(_systemUser, this, "ShownStatusChanged", _presentationShowByPlayer);
                    }
                    catch(Exception ex)
                    {
                        _config.EventLog.WriteError(string.Format("ShowService.set_PresentationShowByPlayer \n {0}", ex));
                    }
                }
            }
        }

        void _worker_OnPresentationChanged(object sender, PresentationChangedEventArgs e)
        {
            if (_scheduler != null && PresentationShowByPlayer && _currentInfo != null
                && _currentInfo.Equals(e.PresentationInfo))
            {
                _scheduler.UpdateSchedule(e.PresentationInfo);
            }
        }

        void _worker_OnSlideChanged(object sender, SlideChangedEventArgs e)
        {
            try
            {
                if (PresentationShowByPlayer && _currentInfo != null &&
                    e.UniquePresentationName.Equals(_currentInfo.UniqueName))
                {
                    PresentationInfo info = _worker.GetPresentationInfo(e.UniquePresentationName);
                    //признак что команда которая посылается в случае изменений НЕ в соседних сценах послана. в этом случае посылать еще один раз эту команду не нужно
                    bool commandForAnySlideChangeSent = false;
                    SlideInfo[] neighboringSlides = null;
                    SlideInfo currentSlideInfo = null;
                    if (_currentSlide != null)
                    {
                        currentSlideInfo = info.SlideInfoList.SingleOrDefault(si => si.Id == _currentSlide.Id);
                        neighboringSlides =
                            info.GetNeighboringSlides(currentSlideInfo);
                    }
                    foreach (int slideId in e.SlideIds)
                    {
                        SlideInfo neighboringSlideInfo = null;
                        if (neighboringSlides != null)
                        {
                            neighboringSlideInfo = neighboringSlides.SingleOrDefault(si => si.Id == slideId);
                        }
                        if (!commandForAnySlideChangeSent)
                        {
                            GeneralFullSceneListAndGeneralMarkedSceneListCommand(info);
                            commandForAnySlideChangeSent = true;
                        }

                        if (neighboringSlideInfo != null)
                        {
                            // соседний
                            ChangeSlideController(info, currentSlideInfo, neighboringSlideInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowService._worker_OnSlideChanged: \n {0}", ex));
            }
        }

        void _scheduler_OnTick(int slideId)
        {
            if (PresentationShowByPlayer && _currentInfo != null && !_isPaused)
            {
                bool isPrevEnable, isNextEnable;
                if (ShowSlide(_currentInfo.UniqueName, slideId, out isPrevEnable, out isNextEnable).IsSuccess)
                {
                    _globalNotifier.Notify(null, this, "GotoSlideByExternalSystem", slideId, isPrevEnable, isNextEnable);
                }
            }
        }

        void _playerMonitoringTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(_playerMonitoringTimerSync))
            {
                try
                {
                    if (PresentationShowByPlayer && _playerCallback != null && _currentInfo != null)
                    {
                        try
                        {
                            _playerCallback.Ping();
                        }
                        catch
                        {
                            Stop(_systemUser, _currentInfo);
                            StopPlayer();
                        }
                    }
                }
                catch(Exception ex)
                {
                    _config.EventLog.WriteError(string.Format("ShowService._playerMonitoringTimer_Elapsed \n {0}", ex));
                }
                finally
                {
                    Monitor.Exit(_playerMonitoringTimerSync);
                }
            }
        }

    }
}
