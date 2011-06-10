using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Domain.PresentationShow.ShowCommon;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.PowerPointLib;
using TechnicalServices.Util;

namespace Domain.PresentationShow.ShowService
{
    internal class PresentationShowPreparator
    {
        #region Nested

        #region Nested type: FreeSpaceNotEnough

        private class FreeSpaceNotEnough : IEnumerable<DisplayType>
        {
            private readonly Dictionary<DisplayType, bool> _displayList = new Dictionary<DisplayType, bool>();
            private readonly object _sync;

            public FreeSpaceNotEnough()
            {
                _sync = ((ICollection) _displayList).SyncRoot;
            }

            public int CountOfNotEnoughFreeSpaceDisplay
            {
                get { return _displayList.Values.Count(miss => !miss); }
            }

            public void Add(DisplayType displayType)
            {
                lock (_sync)
                {
                    _displayList.Add(displayType, false);
                }
            }

            public void Remove(DisplayType displayType)
            {
                lock (_sync)
                {
                    _displayList.Remove(displayType);
                }
            }

            public void MarkDisplayForMissingUpload(DisplayType displayType)
            {
                lock (_sync)
                {
                    _displayList[displayType] = true;
                }
            }

            public bool IsDisplayMissedForUpload(DisplayType displayType)
            {
                lock (_sync)
                {
                    bool isMissed;
                    _displayList.TryGetValue(displayType, out isMissed);
                    return isMissed;
                }
            }

            public void Clear()
            {
                _displayList.Clear();
            }

            #region Implementation of IEnumerable

            public IEnumerator<DisplayType> GetEnumerator()
            {
                List<DisplayType> displays = new List<DisplayType>();
                foreach (KeyValuePair<DisplayType, bool> pair in _displayList)
                {
                    if (!pair.Value) displays.Add(pair.Key);
                }
                return displays.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

        #region Nested type: ObjectData

        private class ObjectData
        {
            private readonly string _presentationUniqueName;
            private readonly int _slideId = -1;
            private readonly UserIdentity _userIdentity;
            private readonly DisplayType _displayType = null;

            public ObjectData(UserIdentity userIdentity, string presentationUniqueName)
            {
                _userIdentity = userIdentity;
                _presentationUniqueName = presentationUniqueName;
            }

            public ObjectData(UserIdentity userIdentity, string presentationUniqueName,
                              int slideId)
                : this(userIdentity, presentationUniqueName)
            {
                _slideId = slideId;
            }

            public ObjectData(UserIdentity userIdentity, string presentationUniqueName,
                  int slideId, DisplayType displayType)
                : this(userIdentity, presentationUniqueName, slideId)
            {
                _displayType = displayType;
            }


            public UserIdentity UserIdentity
            {
                get { return _userIdentity; }
            }

            public string PresentationUniqueName
            {
                get { return _presentationUniqueName; }
            }

            public int SlideId
            {
                get { return _slideId; }
            }

            public DisplayType DisplayType
            {
                get { return _displayType; }
            }
        }

        #endregion

        #region Nested type: UploadlistForDisplay

        //private class UploadlistForDisplay
        //{
        //    private readonly List<ResourceForUpload> _list = new List<ResourceForUpload>();

        //    public UploadlistForDisplay(Display display, IEnumerable<ResourceForUpload> uploadList)
        //    {
        //        Display = display;
        //        ResourceForUploadList.AddRange(uploadList);
        //    }

        //    public Display Display { get; set; }

        //    public List<ResourceForUpload> ResourceForUploadList
        //    {
        //        get { return _list; }
        //    }
        //}

        private class UploadlistForDisplay
        {
            private readonly List<ResourceDescriptor> _list = new List<ResourceDescriptor>();

            public UploadlistForDisplay(DisplayType displayType, IEnumerable<ResourceDescriptor> uploadList)
            {
                DisplayType = displayType;
                ResourceForUploadList.AddRange(uploadList);
            }

            public DisplayType DisplayType { get; set; }

            public List<ResourceDescriptor> ResourceForUploadList
            {
                get { return _list; }
            }
        }


        #endregion

        #endregion

        private readonly IConfiguration _config;
        private readonly FreeSpaceNotEnough _freeSpaceNotEnough = new FreeSpaceNotEnough();
        private readonly Dictionary<Type, IModule> _mappingList;
        private readonly DisplayAndEquipmentMonitor _monitor;
        private readonly PreparationLog _preparationLog;

        private readonly Dictionary<DisplayType, List<ResourceDescriptor>> _resourceForUpload =
            new Dictionary<DisplayType, List<ResourceDescriptor>>();

        private readonly object _resourceUploadSyncObject;
        private readonly ManualResetEvent _terminateEvent = new ManualResetEvent(false);
        private int _preparationInProgress = 0;
        private readonly IPresentationWorker _worker;

        private Action<DisplayType> _askDelegate;
        private Action _preparationFinishDelegate;
        private Action<PartSendEventArgs> _resourceTransmitDelegate;
        private Action<DisplayType[], int[]> _resourceListDelegate;
        private Action<double, string, string> _uploadSpeedDelegate;
        private Action<string, bool> _preparationForDisplayEndedDelegate;
        private Action<string> _logMessageDelegate;
        private int _currentLoadedResource;


        private Thread _preparationThread;
        private int _totalResources;

        private string _presentationUniqueName = string.Empty;
        private int _slideId = -1;

        private bool _autoPrepare = false;

        public bool AutoPrepare
        {
            get { return _autoPrepare; }
            set { _autoPrepare = value; }
        }


        private readonly ManualResetEvent _endSourceUploadEvent = new ManualResetEvent(false);
        internal PresentationShowPreparator(IConfiguration configuration,
                                            IPresentationWorker presentationWorker, DisplayAndEquipmentMonitor monitor,
                                            Dictionary<Type, IModule> mappingList)
        {
            _config = configuration;
            _worker = presentationWorker;
            _monitor = monitor;
            _mappingList = mappingList;
            _preparationLog = new PreparationLog(configuration.EventLog);
            _preparationLog.OnLogMessage += new Action<string>(LogMessage);
            _resourceUploadSyncObject = ((ICollection) _resourceForUpload).SyncRoot;
            Init();
        }

        internal bool StartPreparation(UserIdentity userIdentity, string uniqueName, int slideId,
                                       Action<DisplayType> askDelegate, Action preparationFinishDelegate,
                                        Action<PartSendEventArgs> resourceTransmitDelegate,
                                        Action<DisplayType[], int[]> resourceListDelegate,
                                        Action<double, string, string> uploadSpeedDelegate,
                                        Action<string, bool> preparationForDisplayEndedDelegate,
                                        Action<string> logMessageDelegate)
        {
            return StartPreparation(userIdentity, uniqueName, slideId, null, askDelegate,
                preparationFinishDelegate, resourceTransmitDelegate, resourceListDelegate, uploadSpeedDelegate, preparationForDisplayEndedDelegate, logMessageDelegate);
        }


        internal bool StartPreparation(UserIdentity userIdentity, string uniqueName, int slideId,
                               DisplayType displayType,
                               Action<DisplayType> askDelegate, Action preparationFinishDelegate,
                                Action<PartSendEventArgs> resourceTransmitDelegate,
                                Action<DisplayType[], int[]> resourceListDelegate,
                                Action<double, string, string> uploadSpeedDelegate,
                                Action<string, bool> preparationForDisplayEndedDelegate,
                                Action<string> logMessageDelegate)
        {
            if (Interlocked.CompareExchange(ref _preparationInProgress, 1, 0) == 1) return false;
            _preparationThread = new Thread(Prepare) {IsBackground = true};
            _terminateEvent.Reset();
            _freeSpaceNotEnough.Clear();
            _askDelegate = askDelegate;
            _preparationFinishDelegate = preparationFinishDelegate;
            _resourceTransmitDelegate = resourceTransmitDelegate;
            _resourceListDelegate = resourceListDelegate;
            _uploadSpeedDelegate = uploadSpeedDelegate;
            _preparationForDisplayEndedDelegate = preparationForDisplayEndedDelegate;
            _logMessageDelegate = logMessageDelegate;
            _preparationThread.Start(new ObjectData(userIdentity, uniqueName, slideId, displayType));
            return true;
        }

        internal bool StartPreparation(UserIdentity userIdentity, string presentationUniqueName,
                                       Action<DisplayType> askDelegate, Action preparationFinishDelegate,
                                       Action<PartSendEventArgs> resourceTransmitDelegate,
                                       Action<DisplayType[], int[]> resourceListDelegate,
                                       Action<double, string, string> uploadSpeedDelegate,
                                       Action<string, bool> preparationForDisplayEndedDelegate)
        {
            return StartPreparation(userIdentity, presentationUniqueName, -1, askDelegate,
                preparationFinishDelegate, resourceTransmitDelegate, resourceListDelegate, uploadSpeedDelegate, preparationForDisplayEndedDelegate, null);
        }

        internal PreparationResult GetPreparationResult()
        {
            return _preparationLog.GetPreparationResult();
        }

        internal void Terminate(TerminateLoadCommand command, string display)
        {
            if (displayThreads == null) return;
            if (command == TerminateLoadCommand.StopAll)
            {
                if (_preparationThread != null && _preparationThread.IsAlive)
                {
                    if (_resourceForUpload != null && displayThreads != null)
                    {
                        foreach (DisplayType displayType in _resourceForUpload.Keys)
                        {
                            if (displayThreads.ContainsKey(displayType.Name))
                            {
                                var item = displayThreads[displayType.Name];

                                //Так работает неверно, так как нити в состоянии паузы не очищаются, будем очищать все, чтобы не усложнять
                                //if (item.Thread.ThreadState != ThreadState.WaitSleepJoin) // Если загрузка не завершена
                                {
                                    StopLoadForDisplay(displayType.Name);      // Останавливаем загрузку
                                    TerminateUpload(displayType);
                                    DeleteResourcesUploaded(displayType.Name);
                                }
                            }
                            _preparationLog.AddLog(LogEntry.Terminated, false, displayType.Name);
                        }
                    }
                    _endSourceUploadEvent.Set(); // Надо это делать в конце, иначе displayThreads=null
                }
            }
            else
            {
                //Команда для какого-то конкретного дисплея.
                switch (command)
                {
                    case TerminateLoadCommand.StopLoad:
                    {
                        if (displayThreads != null)
                        {
                            
                            if (displayThreads.ContainsKey(display))
                            {
                                DisplayType type = GetDisplayTypeByName(display);
                                if (type != null)
                                {
                                    DeleteResourcesUploaded(type.Name);
                                    TerminateUpload(type);
                                    StopLoadForDisplay(display);
                                }
                                _preparationLog.AddLog(LogEntry.Terminated, false, display);
                            }
                        }
                        break;
                    }
                    case TerminateLoadCommand.PauseLoad:
                    {
                        if (displayThreads != null)
                        {
                            if (displayThreads.ContainsKey(display))
                            {
                                DisplayType type = GetDisplayTypeByName(display);
                                if (type != null)
                                {
                                    TerminateUpload(type);
                                    StopLoadForDisplay(display);
                                }
                            }
                            _preparationLog.AddLog(LogEntry.Paused, false, display);
                        }
                        break;
                    }
                    case TerminateLoadCommand.ResumeLoad:
                    {
                        if (displayThreads != null)
                        {
                            if (displayThreads.ContainsKey(display))
                            {

                                if (ResanFreeSpace(GetDisplayTypeByName(display))) // Если места достаточно
                                {
                                    Thread thread = new Thread(UploadForDisplay) { IsBackground = true };
                                    Thread.CurrentPrincipal = displayThreads[display].UploadUser;
                                    thread.Name = display;
                                    displayThreads[display].Thread = thread;
                                    thread.Start(displayThreads[display].ThreadParameters);
                                }
                            }
                        }
                        break;
                    }
                    case TerminateLoadCommand.EndSourceUpload:
                    {
                        _endSourceUploadEvent.Set();
                        break;
                    }

                }
            }
        }

        /// <summary>
        /// Получить тип дисплея по имени агента.
        /// </summary>
        /// <param name="display">Имя.</param>
        /// <returns>Тип дисплея.</returns>
        private DisplayType GetDisplayTypeByName(string display)
        {
            DisplayType type = null;
            if (_resourceForUpload == null) return null;
            foreach (DisplayType displayType in _resourceForUpload.Keys)
            {
                if (displayType.Name == display)
                {
                    type = displayType;
                    break;
                }
            }
            return type;
        }

        private void StopLoadForDisplay(string display)
        {
            displayThreads[display].Thread = null;
        }

        internal void ResponseForNotEnoughFreeSpaceRequest(DisplayType displayType, AgentAction agentAction)
        {
                switch (agentAction)
                {
                    case AgentAction.Delete:
                        _preparationLog.AddLog(LogEntry.ClearSpace, false, displayType.Name);
                        DeleteAllResourcesOnAgent(displayType);
                        TerminateUpload(displayType);
                        RescanAgentSources(displayType); // После очистки места возможно требуется пересканирование источников.
                        _freeSpaceNotEnough.Remove(displayType);
                        break;
                    case AgentAction.Continue:
                        _preparationLog.AddLog(LogEntry.NotUploadResource, true, displayType.Name);
                        _freeSpaceNotEnough.MarkDisplayForMissingUpload(displayType);
                        break;
                }
                if (_clientResponseEvent != null)
                    _clientResponseEvent.Set();

        }

        private void MarkResourcesAsNew(DisplayType displayType)
        {
            if (displayThreads != null && displayThreads.ContainsKey(displayType.Name))
            {
                foreach (var r in displayThreads[displayType.Name].ThreadParameters.Resources)
                {
                    var info = r.ResourceInfo as ResourceFileInfo;
                    if (info != null)
                    {
                        foreach (var file in info.ResourceFileList)
                        {
                            file.Newly = true;
                        }
                    }
                }
            }
        }

        internal bool IsPreparationInProgress
        {
            get
            {
                return _preparationInProgress == 1;
            }
        }

        internal bool IsSlidePreparationInProgress(string presentationUniqueName, int slideId)
        {
            if (_preparationInProgress == 1 && _presentationUniqueName.Equals(presentationUniqueName)
                && (_slideId <= 0 || _slideId == slideId))
                return true;
            return false;
        }

        #region private

        private ManualResetEvent _clientResponseEvent;
        private void Prepare(object objectData)
        {
            _endSourceUploadEvent.Reset();
            try
            {
                ObjectData od = (ObjectData) objectData;
                _presentationUniqueName = od.PresentationUniqueName;
                _slideId = od.SlideId;
                Thread.CurrentPrincipal = od.UserIdentity;
                _preparationLog.Clear();
                _resourceForUpload.Clear();
                _freeSpaceNotEnough.Clear();
                Slide[] slideList = null;
                if (_slideId > 0)
                {
                    slideList = _worker.LoadSlides(_presentationUniqueName, new[] {_slideId});
                }
                else
                {
                    byte[] pres = _worker.GetPresentation(_presentationUniqueName);
                    Presentation presentation = BinarySerializer.Deserialize<Presentation>(pres);
                    slideList = presentation.SlideList.ToArray();
                }
                // формируем список дисплеев и ресурсов для них
                if (_terminateEvent.WaitOne(0)) return;
                Dictionary<DisplayType, List<ResourceDescriptor>> displayResourceDictionary =
                    GetDisplayResourceDictionary(_presentationUniqueName, slideList, od.DisplayType);

                if (_terminateEvent.WaitOne(0)) return;
                // проверка на коннекшн дисплеев и аппаратных источников
                CheckForOnLine(slideList);
                // теперь определим какие источники нужно загружать
                bool isSuccess;
                //do
                //{
                    GetForUpload(displayResourceDictionary);
                    if (_terminateEvent.WaitOne(0)) return;
                    isSuccess = ProccesAgentWhichHasNoFreeSpace();
                //} while (!isSuccess);

                if (_terminateEvent.WaitOne(0)) return;

                // определим общее число сорсо для аплоада
                _totalResources = 0;
                foreach (KeyValuePair<DisplayType, List<ResourceDescriptor>> pair in _resourceForUpload)
                {
                    _totalResources += pair.Value.Sum(
                        rd=>((ResourceFileInfo)rd.ResourceInfo).ResourceFileList.Count(
                            rfp=>rfp.Newly));
                }
                StartUpload(_resourceForUpload);
            }
            catch (Exception ex)
            {
                _preparationLog.AddLog(LogEntry.Unknown, true, ex.ToString());
                _config.EventLog.WriteError(string.Format("PresentationShowPreparator.Prepare: \n{0}", ex));
            }
            finally
            {
                Finish();
            }
        }

        private void CheckForOnLine(IEnumerable<Slide> slideList)
        {
            IEnumerable<EquipmentType> equipmentTypes = new List<EquipmentType>();
            foreach (Slide slide in slideList)
            {
                // для дисплеев отбираем только софтварные для которых есть раскладка!!!
                equipmentTypes = equipmentTypes.
                    Union(slide.DeviceList.ConvertAll<EquipmentType>(dev => dev.Type)).
                    Union(
                    slide.DisplayList.Where(dis => (dis is ActiveDisplay) && dis.WindowList.Count > 0).Select((dis => dis.Type)).Cast
                        <EquipmentType>()).
                    Union(slide.SourceList.ConvertAll<EquipmentType>(s => s.Type).Where(et => et.IsHardware));
            }
            // если есть хотя бы один харварный девайс, то надо проверить контроллер
            if (equipmentTypes.Any(et => et.IsHardware))
            {
                if (!_monitor.IsControllerOnLine) _preparationLog.AddLog(LogEntry.NotConneted, false, "Контроллер");
            }
            foreach (EquipmentType equipmentType in equipmentTypes)
            {
                if (!_monitor.IsOnLine(equipmentType))
                    _preparationLog.AddLog(LogEntry.NotConneted, false, equipmentType.Name);
            }
        }

        private void DeleteAllResourcesOnAgent(DisplayType displayType)
        {
            _mappingList[displayType.GetType()].ServerModule.DeleteAllResources(displayType);
            // Пометим все файлы как новые, раз их все снесли.
            MarkResourcesAsNew(displayType);
        }

        private bool ProccesAgentWhichHasNoFreeSpace()
        {
            if (_freeSpaceNotEnough.CountOfNotEnoughFreeSpaceDisplay == 0) return true;
            foreach (DisplayType displayType in _freeSpaceNotEnough)
            {
                AskClientAndWaitResponse(displayType);
            }
            return false;
        }

        private void AskClientAndWaitResponse(DisplayType displayType)
        {
            try
            {
                _preparationLog.AddLog(LogEntry.NotEnoughSpace, false, displayType.Name);
                _askDelegate.Invoke(displayType);
                // Очистка места у нас теперь по другому принципу делается.
                //bool isClientResponded;
                //using (_clientResponseEvent = new ManualResetEvent(false))
                //{
                //    //ждем час, если юзер не ответил - лесом
                //    isClientResponded = _clientResponseEvent.WaitOne(TimeSpan.FromMinutes(1));
                //}
                //_clientResponseEvent = null;
                //if (!isClientResponded)
                //    ResponseForNotEnoughFreeSpaceRequest(displayType,
                //                                         AgentAction.Continue);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(String.Format(
                                                "PresentationShowPreparator.AskClientAndWaitResponse: \n {0}", ex));
                ResponseForNotEnoughFreeSpaceRequest(displayType, AgentAction.Continue);
            }
        }

        private void GetForUpload(Dictionary<DisplayType, List<ResourceDescriptor>> displayResourceDictionary)
        {
            _resourceForUpload.Clear();
            List<IAsyncResult> resultList = new List<IAsyncResult>();

            foreach (KeyValuePair<DisplayType, List<ResourceDescriptor>> pair in displayResourceDictionary)
            {
                if (_freeSpaceNotEnough.IsDisplayMissedForUpload(pair.Key)) continue;
                resultList.Add(AsyncCallerResult.BeginCall<DisplayType, ResourceDescriptor[], UploadlistForDisplay>(GetResourcesForUpload, pair.Key, pair.Value.ToArray()));
            }

            foreach (IAsyncResult result in resultList)
            {
                UploadlistForDisplay uploadList = AsyncCallerResult.EndCall<DisplayType, ResourceDescriptor[], UploadlistForDisplay>(result);
                if (_terminateEvent.WaitOne(0)) continue;
                lock (_resourceUploadSyncObject)
                {
                    _resourceForUpload[uploadList.DisplayType] = uploadList.ResourceForUploadList;
                }
            }
        }

        private Dictionary<DisplayType, List<ResourceDescriptor>> GetDisplayResourceDictionary(
            string presentationUniqueName, IEnumerable<Slide> slideList, DisplayType displayType)
        {
            Dictionary<DisplayType, List<ResourceDescriptor>> displayResourceDictionary =
                new Dictionary<DisplayType, List<ResourceDescriptor>>();
            foreach (Slide slide in slideList)
            {
                foreach (Display dis in slide.DisplayList)
                {
                    if (displayType != null && !dis.Type.Equals(displayType)) continue;
                    // загрузку делаем ТОЛЬКО для активных дисплеев - фильтр безопасности
                    ActiveDisplay activeDisplay = dis as ActiveDisplay;
                    if (activeDisplay == null) continue;
                    foreach (Window window in activeDisplay.WindowList)
                    {
                        Source source = window.Source;
                        ResourceDescriptor resourceDescriptor = source.ResourceDescriptor;
                        if (resourceDescriptor == null) continue;
                        ResourceFileInfo resourceFileInfo = resourceDescriptor.ResourceInfo as ResourceFileInfo;
                        if (resourceFileInfo == null) continue;
                        AddToDictionary(displayResourceDictionary, activeDisplay.Type, resourceDescriptor);
                    }
                    if (!string.IsNullOrEmpty(activeDisplay.BackgroundImage))
                    {
                        ResourceDescriptor backgroundImageDescriptor = _worker.SourceDAL.GetStoredSource(activeDisplay.BackgroundImage, Constants.BackGroundImage, presentationUniqueName);
                        if (backgroundImageDescriptor != null)
                            AddToDictionary(displayResourceDictionary,
                                            activeDisplay.Type, backgroundImageDescriptor);
                    }
                }
            }
            return displayResourceDictionary;
        }

        private UploadlistForDisplay GetResourcesForUpload(
    DisplayType displayType,
    ResourceDescriptor[] resourceDescriptors)
        {
            bool isEnoughFreeSpace;
            ResourceDescriptor[] resourcesForUpload = _mappingList[displayType.GetType()].ServerModule.
                GetResourcesForUpload(displayType, resourceDescriptors, out isEnoughFreeSpace);
            if (!isEnoughFreeSpace)
                _freeSpaceNotEnough.Add(displayType);
            return new UploadlistForDisplay(displayType, resourcesForUpload);
        }

        /// <summary>
        /// Повторная проверка наличия нужного количества свободного пространства.
        /// </summary>
        /// <param name="displayType">Дисплей, на котором идет проверка</param>
        /// <returns>Истина, если свободного места достаточно.</returns>
        private bool ResanFreeSpace(DisplayType displayType)
        {
            if (!displayThreads.ContainsKey(displayType.Name)) return true;
            ResourceDescriptor[] descriptors = displayThreads[displayType.Name].ThreadParameters.Resources;
            bool isEnoughFreeSpace;
            ResourceDescriptor[] resourcesForUpload = _mappingList[displayType.GetType()].ServerModule.
                GetResourcesForUpload(displayType, descriptors, out isEnoughFreeSpace);
            if (!isEnoughFreeSpace)
            {
                AskClientAndWaitResponse(displayType);
                return false;
            }
            return true;
        }


        private bool UploadResources(DisplayType displayType, ResourceDescriptor[] resourcesForUpload)
        {
            try
            {
                // для бэкграундов - ход конем, подменяем файл ppt на имидж извлеченный из первого сладйа - и его подсовываем в качестве ресурса
                // делается здесь, только один раз, и только для передачи на агента, иначе хрен знает сколько перелапачивать
                List<string> tempFiles = new List<string>(resourcesForUpload.Length);
                // для всех ресурсов изменим имя ресурса чтобы оно указывало на реальное имя в хранилище
                foreach (ResourceDescriptor descriptor in resourcesForUpload)
                {
                    ResourceFileInfo resourceFileInfo = descriptor.ResourceInfo as ResourceFileInfo;
                    if (resourceFileInfo == null) continue;
                    IEnumerable<ResourceFileProperty> resourceFileProperties =
                        resourceFileInfo.ResourceFileList.Where(rfp => rfp.Newly);
                    foreach (ResourceFileProperty property in resourceFileProperties)
                    {
                        BackgroundImageDescriptor backgroundImageDescriptor = descriptor as BackgroundImageDescriptor;
                        if (backgroundImageDescriptor != null)
                        {
                            try
                            {
                                // для бэкграунда извлекаем слайд как имидж!!!
                                string fullFileName = _worker.SourceDAL.GetResourceFileName(descriptor, property.Id);
                                string tempFile = Path.ChangeExtension(Path.GetFullPath(fullFileName), "._temppng");
                                using (PowerPoint powerPoint = PowerPoint.OpenFile(fullFileName))
                                {
                                    powerPoint.SaveSlideToFile(1, tempFile, displayType.Width, displayType.Height);
                                }
                                tempFiles.Add(tempFile);
                                property.ResourceFullFileName = tempFile;
                                property.Length = new FileInfo(tempFile).Length;
                            }
                            catch(Exception ex)
                            {
                                _config.EventLog.WriteError(string.Format("PresentationShowPreparator.UploadResources: Exception для дисплея {0}\n{1}", displayType.Name, ex));
                                // если вышел облом с бэкграундом, то занесем эту ошибку в лог, кэширование ПРОДОЛЖИМ!!!
                                _preparationLog.AddLog(LogEntry.NotUploadBackground, false, displayType.Name);
                                property.Newly = false;
                            }
                        }
                        else
                        {
                            property.ResourceFullFileName =
                                _worker.SourceDAL.GetResourceFileName(descriptor, property.Id);
                        }
                    }
                }
                ResourceDescriptor[] notUploaded =
                    _mappingList[displayType.GetType()].ServerModule.UploadResources(displayType,
                                                                                     resourcesForUpload,
                                                                                     _worker.SourceDAL);
                
                foreach (string file in tempFiles)
                {
                    File.Delete(file);
                }
                if (notUploaded != null && notUploaded.Length != 0)
                {
                    string equipmentName = displayType.Name;
                    if (!string.IsNullOrEmpty(equipmentName))
                    {
                        object thread=null;
                        if(displayThreads !=null && displayThreads.ContainsKey(equipmentName)) thread= displayThreads[equipmentName].Thread;
                        if (thread != null)
                        {
                            _preparationLog.AddLog(LogEntry.NotUploadResource, false, equipmentName);
                            return false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                if (!(ex is ThreadAbortException)) // Отмена выполнения потока -- это всего лишь прерывание закачки
                {
                    _preparationLog.AddLog(LogEntry.NotUploadResource, false, displayType.Name);
                    _config.EventLog.WriteError(string.Format("PresentationShowPreparator.UploadResources: Exception для дисплея {0}\n{1}", displayType.Name, ex));
                }
                return false;
            }
            return true;
        }

        private void DeleteResourcesUploaded(string display)
        {
            DisplayType displayType = GetDisplayTypeByName(display);
            if (displayType != null && displayThreads != null && displayThreads.ContainsKey(display))
            {
                var res = displayThreads[display].ThreadParameters.Resources;
                _mappingList[displayType.GetType()].ServerModule.DeleteResourcesUploaded(displayType,
                                                                                         res);
                MarkResourcesAsNew(displayType);
            }
        }

        private void TerminateUpload(DisplayType displayType)
        {
            IModule module;
            if (!_mappingList.TryGetValue(displayType.GetType(), out module)) return;
            module.ServerModule.TerminateUpload(displayType.Name);
        }

        private void AddToDictionary(
            IDictionary<DisplayType, List<ResourceDescriptor>> dictionary,
            DisplayType displayType,
            ResourceDescriptor resourceDescriptor)
        {
            List<ResourceDescriptor> resourceDescriptors;
            if (!dictionary.TryGetValue(displayType, out resourceDescriptors))
            {
                dictionary[displayType] = resourceDescriptors = new List<ResourceDescriptor>();
            }
            if (!resourceDescriptors.Contains(resourceDescriptor))
            {
                resourceDescriptors.Add(resourceDescriptor);
            }
        }

        private void Init()
        {
            foreach (KeyValuePair<Type, IModule> pair in _mappingList)
            {
                pair.Value.ServerModule.OnResourceTransmit += ServerModule_OnResourceTransmit;
                pair.Value.ServerModule.OnUploadSpeed += new Action<double, string, string>(ServerModule_OnUploadSpeed);
            }
        }

        void ServerModule_OnUploadSpeed(double arg1, string arg2, string arg3)
        {
            this.UploadSpeed(arg1, arg2, arg3);
        }

        private Dictionary<string,UploadThreadItem> displayThreads;
        private void StartUpload(Dictionary<DisplayType, List<ResourceDescriptor>> forUpload)
        {
            DisplayType[] agents = new DisplayType[forUpload.Values.Count];
            int[] resources=new int[agents.Length];
            int i = 0;
            foreach (var pair in forUpload)
            {
                agents[i] = pair.Key;
                resources[i] = pair.Value.Count;
                i++;
            }
            ResourceList(agents, resources);
            List<IAsyncResult> resultList = new List<IAsyncResult>();
            displayThreads = new Dictionary<string, UploadThreadItem>();
            foreach (KeyValuePair<DisplayType, List<ResourceDescriptor>> pair in forUpload)
            {
                if (_terminateEvent.WaitOne(0)) break;
                UploadForDisplayObject param = new UploadForDisplayObject(pair.Key, pair.Value.ToArray());
                Thread thread = null;
                if (_autoPrepare)
                {
                    // Если подготовка идет в автоматическом режиме, то заполняем параметры нити прямо здесь
                    thread = new Thread(UploadForDisplay) { IsBackground = true };
                    thread.Name = pair.Key.Name;
                }
                UploadThreadItem item = new UploadThreadItem(thread, param, Thread.CurrentPrincipal as UserIdentity);
                displayThreads.Add(pair.Key.Name, item);
            }
            if (_autoPrepare)
            {
                _autoPrepare = false;
                //Автоматом стартуем закачку, дожидаемся ее и все.
                foreach (var thread in displayThreads)
                {
                    thread.Value.Thread.Start(displayThreads[thread.Key].ThreadParameters);
                    thread.Value.Thread.Join();
                }
                _endSourceUploadEvent.Set();
            }
            else
            {
                if (displayThreads.Count > 0)
                {
                    _endSourceUploadEvent.WaitOne();
                }
            }
            displayThreads = null;
        }

        /// <summary>
        /// Заново получить ресурсы для закачки.
        /// </summary>
        /// <param name="displayType">Агент, для которого заново выполняется сканирование.</param>
        private void RescanAgentSources(DisplayType displayType)
        {
            Slide[] slideList = null;
            if (_slideId > 0)
            {
                slideList = _worker.LoadSlides(_presentationUniqueName, new[] { _slideId });
            }
            else
            {
                byte[] pres = _worker.GetPresentation(_presentationUniqueName);
                Presentation presentation = BinarySerializer.Deserialize<Presentation>(pres);
                slideList = presentation.SlideList.ToArray();
            }

            Dictionary<DisplayType, List<ResourceDescriptor>> displayResourceDictionary =
            GetDisplayResourceDictionary(_presentationUniqueName, slideList, displayType);
            GetForUpload(displayResourceDictionary);
            List<IAsyncResult> resultList = new List<IAsyncResult>();
            List<ResourceDescriptor> descriptors = null;
            foreach (var v in _resourceForUpload)
            {
                if (v.Key.Equals(displayType)) descriptors = v.Value;
            }
            UploadForDisplayObject param = new UploadForDisplayObject(displayType, descriptors.ToArray());
            Thread thread = null;
            UploadThreadItem item = new UploadThreadItem(thread, param, Thread.CurrentPrincipal as UserIdentity);
            if (displayThreads.ContainsKey(displayType.Name))
                displayThreads[displayType.Name] = item;
            else
                displayThreads.Add(displayType.Name, item);
        }

        /// <summary>
        /// Объект для хранения информации о запущенной нити.
        /// </summary>
        class UploadThreadItem
        {
            public enum UploadState
            {
                InProgress,
                Paused,
                Stopped,
                End
            }
            private Thread _thread;
            /// <summary>
            /// Нить обработки.
            /// </summary>
            public Thread Thread
            {
                get { return _thread; }
                set { _thread = value; }
            }
            private UploadForDisplayObject _threadParameters;
            /// <summary>
            /// Параметры запуска нити обработки.
            /// </summary>
            public UploadForDisplayObject ThreadParameters
            {
                get { return _threadParameters; }
            }

            private UserIdentity _uploadUser;
            public UserIdentity UploadUser
            {
                get { return _uploadUser; }
            }

            UploadState _state = UploadState.Stopped;
            public UploadState State
            {
                get { return _state; }
                set { _state = value; }
            }

            public UploadThreadItem(Thread thread, UploadForDisplayObject threadParameters, UserIdentity uploadUser)
            {
                this._thread = thread;
                this._threadParameters = threadParameters;
                this._uploadUser = uploadUser;
            }

        }
        /// <summary>
        /// Объект для передачи параметров в нить загрузки ресурсов дисплея.
        /// </summary>
        class UploadForDisplayObject
        {
            private DisplayType _display;
            /// <summary>
            /// Дисплей.
            /// </summary>
            public DisplayType Display
            {
                get { return _display; }
            }

            private ResourceDescriptor[] _resources;
            /// <summary>
            /// Список ресурсов.
            /// </summary>
            public ResourceDescriptor[] Resources
            {
                get { return _resources; }
            }

            public UploadForDisplayObject(DisplayType display, ResourceDescriptor[] resources)
            {
                this._display = display;
                this._resources = resources;
            }
        }

        /// <summary>
        /// Точка входа нити загрузки ресурсов для дисплея.
        /// </summary>
        /// <param name="ob">Параметры типа UploadForDisplayObject.</param>
        private void UploadForDisplay(object ob)
        {
            UploadForDisplayObject par = (UploadForDisplayObject)ob;
            bool result = UploadResources(par.Display, par.Resources);
            PreparationForDisplayEnded(par.Display.Name, result);
        }

        private void ServerModule_OnResourceTransmit(object sender, EventArgs e)
        {
            lock (this)
            {
                ResourceTransmit(sender, new PartSendEventArgs(++_currentLoadedResource, _totalResources, (string)sender));
            }
        }

        private void ResourceTransmit(object sender, PartSendEventArgs args)
        {
            if (_resourceTransmitDelegate != null)
            {
                _resourceTransmitDelegate.Invoke(args);
            }
        }

        private void ResourceList(DisplayType[] agents, int[] resources)
        {
            if(_resourceListDelegate != null)
            {
                _resourceListDelegate.Invoke(agents, resources);
            }
        }

        private void UploadSpeed(double speed, string display, string file)
        {
            if (_uploadSpeedDelegate != null)
            {
                _uploadSpeedDelegate.Invoke(speed, display, file);
            }
        }

        private void PreparationForDisplayEnded(string display, bool allOk)
        {
            if (_preparationForDisplayEndedDelegate != null)
            {
                _preparationForDisplayEndedDelegate.Invoke(display, allOk);
            }
        }

        private void LogMessage(string message)
        {
            if (_logMessageDelegate != null)
            {
                _logMessageDelegate.Invoke(message);
            }
        }

        private void Finish()
        {
            try
            {
                _totalResources = 0;
                _currentLoadedResource = 0;
                if (_preparationFinishDelegate != null)
                {
                    _preparationFinishDelegate.Invoke();
                }
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("PresentationShowPreparator.Finish\n{0}", ex));
            }
            finally
            {
                Interlocked.Exchange(ref _preparationInProgress, 0);
            }
        }
        #endregion

    }
}