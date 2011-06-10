using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;

using Domain.PresentationShow.ShowCommon;

using TechnicalServices.Communication.Communication.Client;
using TechnicalServices.Configuration.Client;
using TechnicalServices.Configuration.Player;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Designer;

namespace Domain.PresentationShow.ShowClient
{
    public class ShowClient : IPlayerCommand
    {
        private const int _pingInterval = 60;
        private static readonly ShowClient _instance = new ShowClient();
        private IConfiguration _configuration;
        private Dictionary<Type, IModule> _moduleMapping;
        private ShowNotifier _showNotifier;
        private DuplexClient<IShowCommon> _svc;

        private FreezedEquipmentCollection _freezedEquipment = new FreezedEquipmentCollection();

        private bool _created = true;

        private ShowClient()
        {
        }

        public static ShowClient Instance
        {
            get { return _instance; }
        }

        public event Action OnPreparationFinished;
        public event Action<int, int, string> OnProgressChanged;
        public event Action<DisplayType[], int[]> OnReceiveAgentResourcesList;
        public event Action<double, string, string> OnUploadSpeed;
        public event Action<string, bool> OnPreparationForDisplayEnded;
        public event Action<EquipmentType, bool?> OnEquipmentStateChanged;
        public event Action<EquipmentType, FreezeStatus> OnEquipmentFreezeChanged;
        public event Action<DisplayType> OnNotEnoughSpace;
        public event Action<int> OnGoToSlide;
        public event Action OnSlidePlay;
        public event Action<bool> OnShownStatusChanged;
        public event Action<string> OnLogMessage;

        public bool IsShow { get; protected set; }

        public void Done()
        {
            if (_svc != null)
            {
                try
                {
                    _svc.Channel.UnSubscribeForNotification(Thread.CurrentPrincipal as UserIdentity);
                }
                catch (Exception ex)
                {
                    _configuration.EventLog.WriteError(String.Format("ShowClient.Done(): {0}", ex));
                }
                finally
                {
                    _svc.Dispose();
                    _svc = null;
                }
            }
        }


        //!! Метод Вызывается из Дизайнера!!
        public void InitializeFromDisigner(IClientConfiguration configuration)
        {
            try
            {
                _configuration = configuration;
                _showNotifier = new ShowNotifier(_configuration);
                _showNotifier.OnPreparationFinished += _showNotifier_OnPreparationFinished;
                _showNotifier.OnProgressChanged += _showNotifier_OnProgressChanged;
                _showNotifier.OnEquipmentStateChanged += _showNotifier_OnEquipmentStateChanged;
                _showNotifier.OnNotEnoughSpace += _showNotifier_OnNotEnoughSpace;
                _showNotifier.OnShownStatusChanged += (_showNotifier_OnShownStatusChanged);
                _showNotifier.OnReceiveAgentResourcesList += _showNotifier_OnReceiveAgentResourcesList;
                _showNotifier.OnUploadSpeed += new Action<double, string, string>(_showNotifier_OnUploadSpeed);
                _showNotifier.OnPreparationForDisplayEnded += new Action<string, bool>(_showNotifier_OnPreparationForDisplayEnded);
                _showNotifier.OnLogMessage+=new Action<string>(_showNotifier_OnLogMessage);
                _svc = new DuplexClient<IShowCommon>(new InstanceContext(_showNotifier), _pingInterval);
                _svc.Open();
                _created = true;
            }
            catch (CommunicationException)
            {
                _svc.Dispose();
                _svc = null;
                _created = false;
            }
        }


        public bool InitializeFromPlayer(IPlayerConfiguration configuration)
        {
            _configuration = configuration;
            _showNotifier = new ShowNotifier(_configuration);
            _showNotifier.OnPreparationFinished += _showNotifier_OnPreparationFinished;
            _showNotifier.OnProgressChanged += _showNotifier_OnProgressChanged;
            _showNotifier.OnReceiveAgentResourcesList += _showNotifier_OnReceiveAgentResourcesList;
            _showNotifier.OnUploadSpeed +=new Action<double,string, string>(_showNotifier_OnUploadSpeed);
            _showNotifier.OnPreparationForDisplayEnded += new Action<string, bool>(_showNotifier_OnPreparationForDisplayEnded);
            _showNotifier.OnEquipmentStateChanged += _showNotifier_OnEquipmentStateChanged;
            _showNotifier.OnNotEnoughSpace += _showNotifier_OnNotEnoughSpace;
            _showNotifier.OnLogMessage += new Action<string>(_showNotifier_OnLogMessage);
            _showNotifier.OnExternalSystemCommand += _showNotifier_OnExternalSystemCommand;
            _svc = new DuplexClient<IShowCommon>(new InstanceContext(_showNotifier), configuration.PingInterval);
            try
            {
                _svc.Open();
            }
            catch (Exception /*ex*/)
            {
                _svc.Dispose();
                _svc = null;
                return false;
            }
            return true;
        }

        void _showNotifier_OnExternalSystemCommand(int obj, bool isPrevEnable, bool isNextEnable)
        {
            if (OnGoToSlide != null)
                OnGoToSlide(obj);
        }

        void _showNotifier_OnNotEnoughSpace(DisplayType obj)
        {
            if (OnNotEnoughSpace != null)
                OnNotEnoughSpace(obj);
        }

        public void SubscribeForNotification()
        {
            if (_svc != null)
                _svc.Channel.SubscribeForNotification(Thread.CurrentPrincipal as UserIdentity);
        }

        private void _showNotifier_OnEquipmentStateChanged(EquipmentType equipmentType, bool isOnLine)
        {
            if (OnEquipmentStateChanged != null)
                OnEquipmentStateChanged(equipmentType, isOnLine);
        }

        private void _showNotifier_OnProgressChanged(int processed, int total, string displayName)
        {
            if (OnProgressChanged != null)
                OnProgressChanged(processed, total, displayName);
        }
        private void _showNotifier_OnReceiveAgentResourcesList(DisplayType[] agents, int[] resources)
        {
            if (OnReceiveAgentResourcesList != null)
                OnReceiveAgentResourcesList(agents, resources);
        }

        void _showNotifier_OnUploadSpeed(double arg1, string arg2, string arg3)
        {
            if(OnUploadSpeed!=null)
                OnUploadSpeed(arg1, arg2, arg3);
        }

        void _showNotifier_OnLogMessage(string obj)
        {
            if(OnLogMessage!=null)
                OnLogMessage(obj);
        }

        void _showNotifier_OnPreparationForDisplayEnded(string arg1, bool allOk)
        {
            if (OnPreparationForDisplayEnded != null)
                OnPreparationForDisplayEnded(arg1, allOk);
        }


        private void _showNotifier_OnPreparationFinished()
        {
            if (OnPreparationFinished != null)
                OnPreparationFinished();
        }

        void _showNotifier_OnShownStatusChanged(bool isShownByPlayer)
        {
            if (OnShownStatusChanged != null)
                OnShownStatusChanged(isShownByPlayer);
        }


        public MemoryStream[] getScreenshot(DisplayType[] displayList)
        {
            if (!_created) return new MemoryStream[] { };
            try
            {
                return _svc.Channel.CaptureScreen(displayList);
            }
            catch (Exception)
            {
                return new MemoryStream[] { };
            }
        }

        public void NotEnoughSpaceResponce(DisplayType displayType, bool needContinue)
        {
            _svc.Channel.ResponseForNotEnoughFreeSpaceRequest(displayType,
                                                              needContinue
                                                                  ? AgentAction.Continue
                                                                  : AgentAction.Delete);
        }

        private IModule getModule(Type sourceType)
        {
            if (_moduleMapping == null)
            {
                if (_configuration == null)
                    return null;

                _moduleMapping = new Dictionary<Type, IModule>();
                foreach (IModule module in _configuration.ModuleList)
                    foreach (
                        Type type in
                            module.SystemModule.Presentation.GetSource().Concat(
                                module.SystemModule.Configuration.GetDevice()))
                        _moduleMapping.Add(type, module);
            }

            IModule mod = null;
            if (_moduleMapping.TryGetValue(sourceType, out mod))
                return mod;
            return null;
        }

        public IPlayerModule GetPlayerModule(Type sourceType)
        {
            IModule module = getModule(sourceType);
            if (module != null)
                return module.PlayerModule;
            return null;
        }

        public IDesignerModule GetDesignerModule(Type sourceType)
        {
            IModule module = getModule(sourceType);
            if (module != null)
                return module.DesignerModule;
            return null;
        }

        public bool Pause(Presentation presentation)
        {
            if (_svc != null)
            {
                bool result = _svc.Channel.Pause(presentation.UniqueName);
                IsShow = !result;
                return result;
            }
            return false;
        }

        public void CloseWindows()
        {
            if (_created) _svc.Channel.CloseWindows();
        }
        
        public ShowSlideResult ShowSlide(Presentation presentation, Slide slide, out bool isPrevEnable, out bool isNextEnable)
        {
            isPrevEnable = false;
            isNextEnable = false;
            try
            {
                IsShow = true;
                if (!_created) return new ShowSlideResult(false, "Нет соединения с сервером");
                ShowSlideResult retValue = _svc.Channel.ShowSlide(presentation.UniqueName, slide.Id, out isPrevEnable,
                                                                  out isNextEnable);
                if (OnSlidePlay != null)
                    OnSlidePlay();
                return retValue;
            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format("ShowClient.ShowSlide: \n{0}", ex));
                return new ShowSlideResult(false, ex.ToString());
            }
        }

        public ShowSlideResult GoToPrevSlide(out int slideId, out bool isPrevEnable, out bool isNextEnable)
        {
            isPrevEnable = false;
            isNextEnable = false;
            slideId = -1;
            try
            {
                if (!_created) return new ShowSlideResult(false, "Нет соединения с сервером");
                ShowSlideResult retValue = _svc.Channel.GoToPrevSlide(out slideId, out isPrevEnable, out isNextEnable);
                if (OnSlidePlay != null)
                    OnSlidePlay();
                return retValue;
            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format("ShowClient.GoToPrevSlide: \n{0}", ex));
                return new ShowSlideResult(false, ex.ToString());
            }
        }

        public bool Load(PresentationInfo info)
        {
            if (!_created) return false;
            return _svc.Channel.Load(Thread.CurrentPrincipal as UserIdentity,
                                     info.UniqueName);
        }

        public bool Load(PresentationInfo info, int slideId, bool autoPrepare)
        {
            if (!_created) return false;
            return _svc.Channel.Load(Thread.CurrentPrincipal as UserIdentity, info.UniqueName, slideId, autoPrepare);
        }

        public bool Start(PresentationInfo info)
        {
            _freezedEquipment.Reset();
            return _svc.Channel.Start(Thread.CurrentPrincipal as UserIdentity, info);
        }

        public bool StartPlayer()
        {
            return _svc.Channel.StartPlayer();
        }

        public void StopPlayer()
        {
            _svc.Channel.StopPlayer();
        }

        public bool Stop(PresentationInfo info)
        {
            _freezedEquipment.Reset();
            IsShow = false;
            return _svc.Channel.Stop(Thread.CurrentPrincipal as UserIdentity, info);
        }

        public bool TerminateLoad(TerminateLoadCommand command, string display)
        {
            return _svc.Channel.TerminateLoad(command, display);
        }

        public bool CheckStatus(UserIdentity identity, PresentationInfo info)
        {
            if (!_created) return false;
            PresentationShowPrepareStatus status = _svc.Channel.CheckStatus(identity, info);
            return status == PresentationShowPrepareStatus.ReadyToShow;
        }

        public enum PreparationStatus
        {
            Error,
            Warning,
            Ok
        }

        public PreparationStatus HasError(PresentationInfo info, out String error, out String warning)
        {
            warning = string.Empty;
            if (!_created)
            {
                error = "В автономном режиме не поддерживается";
                return PreparationStatus.Error;
            }
            error = String.Empty;
            PreparationResult result = _svc.Channel.GetPreparationResult();
            if (result.WithError)
            {
                foreach (String errLine in result.ErrorLog)
                    error = error + errLine + Environment.NewLine;
                return PreparationStatus.Error;
            }
            if (result.WithWarning)
            {
                foreach (string warningLine in result.WarningLog)
                {
                    warning = warning + warningLine + Environment.NewLine;
                }
                return PreparationStatus.Warning;
            }
            return PreparationStatus.Ok;
        }

        #region IPlayerCommand

        public String DoSourceCommand(Source source, String command)
        {
            try
            {
                if (!_created) return null;
                return _svc.Channel.DoSourceCommand(source.Id, command);
            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format("ShowClient.DoSourceCommand: \n{0}", ex));
                return null;
            }
        }

        public string DoEquipmentCommand(CommandDescriptor cmd)
        {
            try
            {
                if (!_created) return null;
                return _svc.Channel.DoEquipmentCommand(cmd);
            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format("ShowClient.DoEquipmentCommand: \n{0}", ex));
                return null;
            }
        }

        public bool? IsOnLine(EquipmentType equipmentType)
        {
            if (!_created) return null;
            // для некоторых устройств в принципе невохзможно определить статус
            if (equipmentType is PassiveDisplayType) return null;
            SourceType sourceType = equipmentType as SourceType;
            if (sourceType != null && !(sourceType is HardwareSourceType)) return null;
            return _svc.Channel.IsOnLine(equipmentType);
        }

        public void FreezeEquipmentSetting(EquipmentType equipmentType, FreezeStatus status)
        {
            if (!_created) return;
            _svc.Channel.FreezeEquipmentSetting(equipmentType, status);
            _freezedEquipment.Set(equipmentType, status);
            if (OnEquipmentFreezeChanged != null)
                OnEquipmentFreezeChanged(equipmentType, status);
        }

        public FreezeStatus GetFreezedEquipment(EquipmentType equipmentType)
        {
            if (!_created) return FreezeStatus.UnFreeze;
            if (!_freezedEquipment.IsInit)
                _freezedEquipment.Init(_svc.Channel.GetFreezedEquipment());
            return _freezedEquipment.Exists(equipmentType);
        }


        #endregion

        public bool IsShownByPlayer()
        {
            if (!_created) return false;
            return _svc.Channel.IsShownByPlayer();
        }
    }

    [CallbackBehavior(UseSynchronizationContext = false,
        IncludeExceptionDetailInFaults = true,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class ShowNotifier : IShowNotifier
    {
        private readonly IConfiguration _config;
        public ShowNotifier(IConfiguration config)
        {
            _config = config;
        }
        public bool IsAlive { get; set; }

        #region Implementation of IShowNotifier

        public void ShownStatusChanged(bool isShownByPlayer)
        {
            try
            {
                if (OnShownStatusChanged != null)
                    OnShownStatusChanged(isShownByPlayer);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.ShownStatusChanged: {0}", ex));
            }
        }

        public void Ping()
        { }

        public void PreparationFinished()
        {
            try
            {
                if (OnPreparationFinished != null)
                    OnPreparationFinished();
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.PreparationFinished: {0}", ex));
            }
        }

        public void UploadProgress(int currentResource, int totalResources, string displayName)
        {
            try
            {
                if (OnProgressChanged != null)
                    OnProgressChanged(currentResource, totalResources, displayName);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.UploadProgress: {0}", ex));
            }

        }

        public void ReceiveAgentResourcesList(DisplayType[] agents, int[] resources)
        {
            try
            {
                if (OnReceiveAgentResourcesList != null)
                    OnReceiveAgentResourcesList(agents, resources);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.ReceiveAgentResourcesList: {0}", ex));
            }
        }

        public void UploadSpeed(double speed, string display, string file)
        {
            try
            {
                if (OnUploadSpeed != null)
                    OnUploadSpeed(speed, display, file);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.UploadSpeed: {0}", ex));
            }
        }

        public void LogMessage(string messge)
        {
            try
            {
                if (OnLogMessage != null)
                    OnLogMessage(messge);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.LogMessage: {0}", ex));
            }
        }

        public void PreparationForDisplayEnded(string display, bool allOk)
        {
            try
            {
                if (OnPreparationForDisplayEnded != null)
                    OnPreparationForDisplayEnded(display, allOk);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.PreparationForDisplayEnded: {0}", ex));
            }
        }


        public void NotEnoughFreeSpaceRequest(DisplayType displayType)
        {
            try
            {
                if (OnNotEnoughSpace != null)
                    OnNotEnoughSpace(displayType);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.NotEnoughFreeSpaceRequest: {0}", ex));
            }
        }

        public void EquipmentStateChange(EquipmentType equipmentType, bool isOnLine)
        {
            try
            {
                if (OnEquipmentStateChanged != null)
                    OnEquipmentStateChanged(equipmentType, isOnLine);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.EquipmentStateChange: {0}", ex));
            }
        }

        public void GotoSlideByExternalSystem(int slideId, bool isPrevEnable, bool isNextEnable)
        {
            try
            {
                if (OnExternalSystemCommand != null)
                    OnExternalSystemCommand(slideId, isPrevEnable, isNextEnable);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowNotifier.GotoSlideByExternalSystem: {0}", ex));
            }
        }

        #endregion

        public event Action OnPreparationFinished;
        public event Action<int, int, string> OnProgressChanged;
        public event Action<DisplayType[], int[]> OnReceiveAgentResourcesList;
        public event Action<double, string, string> OnUploadSpeed;
        public event Action<string, bool> OnPreparationForDisplayEnded;
        public event Action<EquipmentType, bool> OnEquipmentStateChanged;
        public event Action<DisplayType> OnNotEnoughSpace;
        public event Action<int, bool, bool> OnExternalSystemCommand;
        public event Action<bool> OnShownStatusChanged;
        public event Action<string> OnLogMessage;
    }
}
