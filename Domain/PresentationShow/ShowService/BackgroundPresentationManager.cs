using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Domain.PresentationShow.ShowCommon;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule.Server;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Domain.PresentationShow.ShowService
{
    internal class BackgroundPresentationManager : IDisposable
    {
        private readonly PresentationShowPreparator _showPreparator;
        private readonly DisplayAndEquipmentMonitor _monitor;
        private readonly IServerConfiguration _config;
        private readonly IPresentationWorker _worker;
        private readonly IShowDisplayAndDeviceCommand _showDisplayAndDeviceCommand;
        private readonly UserIdentity _systemUser;

        private Thread _initThread = null;
        private Timer _restoreShowTimer;
        private Timer _backgroundPresentationChange;
        private int _isWorked = 0;
        private int _isInit = 0;
        private int _isPaused = 0;
        //private int _interval = 60000;
        private readonly AutoResetEvent _waitingForPrepareFinish = new AutoResetEvent(false);

        private string _currentPresentationUniqueName = null;
        private Slide _currentSlide = null;

        public BackgroundPresentationManager(IServerConfiguration config,
            IPresentationWorker worker,
            PresentationShowPreparator preparator,
            DisplayAndEquipmentMonitor monitor,
            IShowDisplayAndDeviceCommand showDisplayAndDeviceCommand,
            UserIdentity systemUser)
        {
            _config = config;
            _showPreparator = preparator;
            _monitor = monitor;
            _worker = worker;
            _showDisplayAndDeviceCommand = showDisplayAndDeviceCommand;
            _systemUser = systemUser;
            _restoreShowTimer = new Timer(RestoreShow);
            TimeSpan interval =TimeSpan.FromSeconds(BackgroundPresentationRestoreTimeout);
            _backgroundPresentationChange = new Timer(MonitorForBackgroundPresentation, null, interval, interval);
            // мониторинг оборудования
            _monitor.OnStateChange += new EventHandler<TechnicalServices.Interfaces.ConfigModule.Server.EqiupmentStateChangeEventArgs>(_monitor_OnStateChange);
            // мониторинг изменения в фоновом сценарии
            _worker.OnPresentationChanged += new EventHandler<PresentationChangedEventArgs>(_worker_OnPresentationChanged);
        }

        #region private

        private int BackgroundPresentationRestoreTimeout
        {
            get
            {
                return _config.LoadSystemParameters().BackgroundPresentationRestoreTimeout;
            }
        }

        private string BackgroundPresentationUniqueName
        {
            get
            {
                return _config.LoadSystemParameters().BackgroundPresentationUniqueName;
            }
        }

        private void NotEnoughSpaceRequest(DisplayType displayType)
        {
            // запишем в лог - ответ продолжить
            _config.EventLog.WriteWarning(string.Format("На агенте {0} закончилось свободное место", displayType.Name));
            if (_showPreparator != null)
            {
                _showPreparator.ResponseForNotEnoughFreeSpaceRequest(displayType, AgentAction.Continue);
            }
        }

        private void PreparationFinished()
        {
            _waitingForPrepareFinish.Set();
        }

        private void Init()
        {
            if (Interlocked.CompareExchange(ref _isInit, 1, 0) == 0)
                try
                {
                    // достанем первый слайд фонового сценария - остальные нам по фигу
                    _currentPresentationUniqueName = BackgroundPresentationUniqueName;
                    if (string.IsNullOrEmpty(_currentPresentationUniqueName))
                    {
                        _config.EventLog.WriteInformation(string.Format("Фоновый сценарий не задан"));
                        return;
                    }
                    PresentationInfo info = _worker.GetPresentationInfo(_currentPresentationUniqueName);
                    if (info == null)
                    {
                        _config.EventLog.WriteWarning(string.Format("Фоновый сценарий не найден"));
                        return;
                    }
                    _currentSlide =
                        _worker.LoadSlides(_currentPresentationUniqueName, new int[] { info.StartSlideId }).
                            FirstOrDefault();
                    if (_currentSlide == null)
                    {
                        _config.EventLog.WriteWarning(string.Format("Не задан стартовый слайд у фонового сценария {0}",
                                                                    info.Name));
                        return;
                    }

                    _showPreparator.AutoPrepare = true;
                    // подготовим к показу
                    bool isSuccess = _showPreparator.StartPreparation(_systemUser,
                                                                      _currentPresentationUniqueName,
                                                                      _currentSlide.Id, NotEnoughSpaceRequest,
                                                                      PreparationFinished, null, null, null, null, null);
                    if (isSuccess)
                    {
                        _waitingForPrepareFinish.WaitOne();

                        // команды оборудованию и дисплеям
                        SendCommandForDeviceAndDisplay(_currentPresentationUniqueName, _currentSlide);
                    }
                    else
                    {
                        _config.EventLog.WriteWarning(
                            string.Format("Подготовка к показу фонового сценария невозможна - загрузка в процессе"));
                    }
                }
                catch (Exception ex)
                {
                    _config.EventLog.WriteError(string.Format("BackgroundPresentationManager.Init\n{0}", ex));
                }
                finally
                {
                    Interlocked.Exchange(ref _isInit, 0);
                }
        }

        private void MonitorForBackgroundPresentation(object state)
        {
            if (_isWorked == 0 || _isPaused == 1 || _isInit == 1) return;
            string backgroundPresentationUniqueName = BackgroundPresentationUniqueName;
            if (backgroundPresentationUniqueName != _currentPresentationUniqueName)
                Init();
        }

        void _worker_OnPresentationChanged(object sender, PresentationChangedEventArgs e)
        {
            if (_isWorked == 0 || _isPaused == 1 || _isInit == 1) return;
            if (e.PresentationInfo.UniqueName.Equals(BackgroundPresentationUniqueName))
            {
                new Action(Init).BeginInvoke(null, null);
            }
        }

        void _monitor_OnStateChange(object sender, EqiupmentStateChangeEventArgs e)
        {
            if (_isWorked == 0 || _isPaused == 1 || _isInit == 1) return;
            if (e.IsOnLine)
                new Action<EquipmentType>(SendCommandToEquipment).BeginInvoke(
                    e.EquipmentType, null, null);
        }

        private void SendCommandToEquipment(EquipmentType equipmentType)
        {
            DisplayType displayType = equipmentType as DisplayType;
            if (displayType != null)
            {
                // команда дисплею
                Display display = FindDisplay(displayType);
                if (display == null) return;
                SendCommandToDisplay(display);
                return;
            }
            if (equipmentType.IsHardware && equipmentType.UID > 0)
            {
                Device device = FindDevice(equipmentType);
                if (device == null) return;
                SendCommandToHardwareDevice(device);
                return;
            }
        }


        private void SendCommandToHardwareDevice(Device device)
        {
            try
            {
                // послать команду оборудованию
                _showDisplayAndDeviceCommand.ComposeCommandAndSendToControllerForDevice(device, null, _currentSlide);
            }
            catch(Exception ex)
            {
                _config.EventLog.WriteError(string.Format("BackgroundPresentationManager.SendCommandToHardwareDevice\n {0}", ex));
            }
        }

        private void SendCommandToDisplay(Display display)
        {
            try
            {
                _showPreparator.AutoPrepare = true;
                // загрузить источники
                if (_showPreparator.StartPreparation(_systemUser, _currentPresentationUniqueName,
                    _currentSlide.Id, display.Type, NotEnoughSpaceRequest, PreparationFinished, null, null,null, null, null))
                {
                    _waitingForPrepareFinish.WaitOne();
                    // послать команду на показ
                    _showDisplayAndDeviceCommand.ShowDisplay(display, _currentPresentationUniqueName);
                }
                else
                {
                    _config.EventLog.WriteWarning(string.Format(
                        "BackgroundPresentationManager.SendCommandToDisplay\n невозможно загрузить источники на дисплей {0}", display.Name));
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("BackgroundPresentationManager.SendCommandToHardwareDevice\n {0}", ex));
            }

        }

        private Display FindDisplay(DisplayType displayType)
        {
            if (_currentSlide == null) return null;
            return
                _currentSlide.DisplayList.Find(
                    dis => dis.Type.Type.Equals(displayType.Type, StringComparison.InvariantCultureIgnoreCase)
                           && dis.Type.Name.Equals(displayType.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        private Device FindDevice(EquipmentType equipmentType)
        {
            if (_currentSlide == null) return null;
            return _currentSlide.DeviceList.Find(dev => dev.Equals(equipmentType));
        }

        private void SendCommandForDeviceAndDisplay(string presentationUniqueName, Slide slide)
        {
            _showDisplayAndDeviceCommand.ShowDisplays(presentationUniqueName, slide);
            _showDisplayAndDeviceCommand.ComposeCommandAndSendToControllerForAllDevice(null, slide);
        }

        private void RestoreShow(object state)
        {
            if (_isWorked == 0) return;
            Init();
            Interlocked.Exchange(ref _isPaused, 0);
        }

        private void Interrupt()
        {
            try
            {
                if (_isInit == 1)
                {
                    _showPreparator.Terminate(Domain.PresentationShow.ShowCommon.TerminateLoadCommand.StopAll, null);
                    if (_initThread != null && _initThread.IsAlive)
                        _initThread.Join();
                }
                _initThread = null;
            }
            catch(Exception ex)
            {
                _config.EventLog.WriteError(string.Format("BackgroundPresentationManager.Interrupt\n {0}", ex));
            }
        }

        #endregion

        #region public

        public void StartShow()
        {
            if (Interlocked.CompareExchange(ref _isWorked, 1, 0) == 1) return;
            _initThread = new Thread(Init) { IsBackground = true };
            _initThread.Start();
        }

        public void StopShow()
        {
            if (Interlocked.CompareExchange(ref _isWorked, 0, 1) == 0) return;
            Interrupt();
        }

        public void PauseShow()
        {
            if (_isWorked == 0) return;
            Interrupt();
            Interlocked.Exchange(ref _isPaused, 1);
            _restoreShowTimer.Change(TimeSpan.FromSeconds(BackgroundPresentationRestoreTimeout),
                                     TimeSpan.FromMilliseconds(-1));
        }


        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            StopShow();
            if (_restoreShowTimer != null)
            {
                _restoreShowTimer.Dispose();
                _restoreShowTimer = null;
            }
            if (_backgroundPresentationChange != null)
            {
                _backgroundPresentationChange.Dispose();
                _backgroundPresentationChange = null;
            }
        }

        #endregion
    }
}
