using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Timers;
using System.Linq;

using Domain.PresentationShow.ShowCommon;
using DomainServices.EquipmentManagement.AgentCommon;

using TechnicalServices.Common;
using TechnicalServices.Configuration.Agent;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Interfaces.ConfigModule.Visualizator;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;
using Timer = System.Timers.Timer;

namespace Domain.PresentationShow.ShowAgent
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Single,
        IncludeExceptionDetailInFaults = true)]
    public class ShowAgent : IShowAgent, IDisposable
    {
        private const string InfoMessage = "Список загруженных визуализаторов:";
        private readonly IAgentConfiguration _config;
        //private readonly ISourceDAL _sourceDAL;
        //private readonly Dictionary<IVisualizatorModule, List<IntPtr>>
        //    _activeWindowHandleMapping = new Dictionary<IVisualizatorModule, List<IntPtr>>();

        private readonly IAgentManager _manager;
        private readonly IResourceManager _resourceManager;
        private readonly IModule _currentModule;

        private readonly Dictionary<Type, IVisualizatorModule> _sourceTypeMapping =
            new Dictionary<Type, IVisualizatorModule>();

        private readonly Dictionary<string, IVisualizatorModule> _sourceIdVisualizatorMapping = 
            new Dictionary<string, IVisualizatorModule>();

        private const int _interval = 5000;
        private Timer _restoringTimer = new Timer(_interval);

        public ShowAgent(IAgentConfiguration config, IAgentManager manager, IResourceManager resourceManager)
        {
            Debug.Assert(config != null, "IAgentConfiguration не может быть null");
            _config = config;
            _resourceManager = resourceManager;
            //TODO
            // пока создается здесь - если нужен будет еще где то на компе, то нужно будет передавать извне
            //_sourceDAL = new SourceDAL(_config);
            _manager = manager;

            StringBuilder text = new StringBuilder(InfoMessage, 1024);

            CurrentDisplay = GetDisplayType(_config);
            if (CurrentDisplay == null)
                throw new WrongAgentUIDException();
            _currentModule = GetModuleByDisplayType(config, CurrentDisplay);
            foreach (IModule module in _config.ModuleList)
            {
                try
                {
                    if (module.VisualizatorModule.Init(config.EventLog))
                    {
                        foreach (Type type in module.SystemModule.Presentation.GetSource())
                        {
                            _sourceTypeMapping[type] = module.VisualizatorModule.IsSupportView ? module.VisualizatorModule : _currentModule.VisualizatorModule;
                        }
                        text.AppendLine(module.Name);
                    }
                }
                catch (Exception ex)
                {
                    _config.EventLog.WriteError(String.Format("Модуль {0}, не был загружен, ошибка: {1}", module.Name,
                                                              ex.Message));
                }
            }
            _config.EventLog.WriteInformation(text.ToString());
            _restoringTimer.Elapsed += new ElapsedEventHandler(_restoringTimer_Elapsed);
            _restoringTimer.Start();
        }

        #region Nested

        private class VisualizatorWindow
        {
            private readonly IVisualizatorModule _visualizator;
            private readonly Window _window;

            public VisualizatorWindow(IVisualizatorModule visualizator, Window window)
            {
                _visualizator = visualizator;
                _window = window;
            }

            public IVisualizatorModule Visualizator
            {
                get { return _visualizator; }
            }

            public Window Window
            {
                get { return _window; }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_restoringTimer != null)
            {
                _restoringTimer.Stop();
                _restoringTimer = null;
            }
            //DestroyPreviousWindow();
            foreach (IModule module in _config.ModuleList)
            {
                module.VisualizatorModule.Done();
            }
        }

        #endregion

        private static DisplayType GetDisplayType(IAgentConfiguration config)
        {
                        // TODO - это правильный вариант
                        if (string.IsNullOrEmpty(config.AgentUID)) return null;
                        foreach (DisplayType item in config.ModuleConfiguration.DisplayList)
                        {
                            if (!(item is DisplayTypeUriCapture)) continue;
                            DisplayTypeUriCapture display = (DisplayTypeUriCapture)item;
                            if (String.Compare(config.AgentUID, display.AgentUID, StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                return display;
                            }
                        }
                        return null;
            // TODO - это лажа, исключительно для тестирования, надо объяснить тестировщикам как настраивать
            //string name = Environment.MachineName;
            //foreach (DisplayType item in config.ModuleConfiguration.DisplayList)
            //{
            //    if (!(item is DisplayTypeUriCapture)) continue;
            //    DisplayTypeUriCapture display = (DisplayTypeUriCapture)item;
            //    if (String.Compare(name, display.AgentUID, StringComparison.InvariantCultureIgnoreCase) == 0)
            //    {
            //        return display;
            //    }
            //}
            //foreach (DisplayType item in config.ModuleConfiguration.DisplayList)
            //{
            //    if (!(item is DisplayTypeUriCapture)) continue;
            //    DisplayTypeUriCapture display = (DisplayTypeUriCapture)item;
            //    if (String.Compare("localhost", display.AgentUID, StringComparison.InvariantCultureIgnoreCase) == 0)
            //    {
            //        return display;
            //    }
            //}
            //return null;
        }

        private static IModule GetModuleByDisplayType(IConfiguration config, DisplayType display)
        {
            foreach (IModule module in config.ModuleList)
            {
                foreach (Type type in module.SystemModule.Configuration.GetDisplay())
                {
                    if (display.GetType().FullName == type.FullName)
                        return module;
                }
            }
            return null;
        }

        public DisplayType CurrentDisplay { get; private set; }

        #region IShowAgent Members

        public MemoryStream GetScreenShort(Guid imageFormat)
        {
            return CaptureScreen.GetScreenShort(_config.EventLog, imageFormat);
        }

        public void CloseWindows()
        {
            foreach (IModule module in _config.ModuleList)
            {
                if (module.VisualizatorModule.IsSupportView)
                    module.VisualizatorModule.HideAll();
            }
        }

        //private class WindowPtr
        //{
        //    internal WindowPtr(Window window, IntPtr handle)
        //    {
        //        Window = window;
        //        Handle = handle;
        //    }
        //    internal Window Window { get; set; }
        //    internal IntPtr Handle { get; set; }
        //}

        //private readonly List<WindowPtr> _currentWindowList = new List<WindowPtr>();
        public void ShowWindow(Window[] windows,
                               BackgroundImageDescriptor backgroundImageDescriptor)
        {
            if (backgroundImageDescriptor != null)
            {
                string fileName = _resourceManager.GetRealResourceFileName(backgroundImageDescriptor);
                _manager.SetBackgroudImage(fileName);
            }
            else
            {
                _manager.SetBackgroudImage(null);
            }
            // рассортируем окошки по визуализаторам
            Dictionary<IVisualizatorModule, List<Window>> _visWindow =
                new Dictionary<IVisualizatorModule, List<Window>>(_config.ModuleList.Length);
            foreach (IModule module in _config.ModuleList)
            {
                _visWindow[module.VisualizatorModule] = new List<Window>();
            }
            foreach (Window window in windows)
            {
                IVisualizatorModule visualizatorModule;
                if (_sourceTypeMapping.TryGetValue(window.Source.GetType(), out visualizatorModule))
                {
                    List<Window> windowList;
                    if (!_visWindow.TryGetValue(visualizatorModule, out windowList))
                    {
                        _visWindow[visualizatorModule] = windowList = new List<Window>();
                    }
                    windowList.Add(window);
                }
            }
            // заполняем маппинг SourceId - IVisualizator - для дальнейшей пересылки команд визуализаторам
            _sourceIdVisualizatorMapping.Clear();
            foreach (KeyValuePair<IVisualizatorModule, List<Window>> pair in _visWindow)
            {
                IVisualizatorModule visualizatorModule = pair.Key;
                foreach (Window window in pair.Value)
                {
                    if (window != null && window.Source != null)
                    {
                        _sourceIdVisualizatorMapping[window.Source.Id] = visualizatorModule;
                    }
                }
            }

            // команда визуализаторам- всем ассинхронно
            List<IAsyncResult> resultList = new List<IAsyncResult>(_visWindow.Count);
            foreach (KeyValuePair<IVisualizatorModule, List<Window>> keyValuePair in _visWindow)
                resultList.Add(AsyncCaller.BeginCall(ShowWindow, keyValuePair.Key, keyValuePair.Value.ToArray()));

            foreach (IAsyncResult aResult in resultList)
                AsyncCaller.EndCall<IVisualizatorModule, Window[]>(aResult);

            //foreach (KeyValuePair<IVisualizatorModule, List<Window>> keyValuePair in _visWindow)
            //    ShowWindow(keyValuePair.Key, keyValuePair.Value.ToArray());

            //DestroyPreviousWindow();
            //_currentWindowList.Clear();

            //List<IAsyncResult> resultList = new List<IAsyncResult>(windows.Length);
            //foreach (Window window in windows)
            //    resultList.Add(AsyncCallerResult.BeginCall<Window, WindowPtr>(ShowWindow, window));

            //foreach (IAsyncResult aResult in resultList)
            //    _currentWindowList.AddNotNull<WindowPtr>(AsyncCallerResult.EndCall<Window, WindowPtr>(aResult));

            // учет ZOrder

            List<VisualizatorWindow> listToOrder = new List<VisualizatorWindow>();
            foreach (KeyValuePair<IVisualizatorModule, List<Window>> pair in _visWindow)
            {
                foreach (Window window in pair.Value)
                {
                    listToOrder.Add(new VisualizatorWindow(pair.Key, window));
                }
            }
            foreach (VisualizatorWindow visualizatorWindow in listToOrder.OrderBy(vw=>vw.Window.ZOrder))
            {
                visualizatorWindow.Visualizator.BringToFront(visualizatorWindow.Window);
            }
            //_currentWindowList.Sort((wptr1, wptr2) => wptr1.Window.ZOrder.CompareTo(wptr2.Window.ZOrder));
            //foreach (WindowPtr windowPtr in _currentWindowList)
            //{
            //    IVisualizatorModule visualizatorModule;
            //    if (_sourceTypeMapping.TryGetValue(windowPtr.Window.Source.GetType(), out visualizatorModule))
            //    {
            //        visualizatorModule.BringToFront(windowPtr.Handle);
            //    }
            //}
        }

        public ResourceDescriptor[] GetResourcesForUpload(ResourceDescriptor[] resourceDescriptors,
                                                          out bool isEnoughFreeSpace)
        {
            return _resourceManager.GetResourcesForUpload(resourceDescriptors,
                                                          out isEnoughFreeSpace);
        }

        public void DeleteAllResources()
        {
            _resourceManager.DeleteAllSource();
        }
        
        public void DeleteResourcesUploaded(ResourceDescriptor[] resourceDescriptors)
        {
            _resourceManager.DeleteResourcesUploaded(resourceDescriptors);
        }


        public string DoSourceCommand(string sourceId, string command)
        {
            IVisualizatorModule visualizatorModule;
            //if (_sourceTypeMapping.TryGetValue(source.GetType(), out visualizatorModule))
            //{
            //    return visualizatorModule.DoCommand(source.Id, command);
            //    //// находим нужное окно
            //    //WindowPtr windowPtr = _currentWindowList.Find(wptr => wptr.Window.Source.Id == source.Id);
            //    //if (windowPtr != null)
            //    //{
            //    //    return visualizatorModule.DoCommand(windowPtr.Handle, command);
            //    //}
            //}
            if (_sourceIdVisualizatorMapping.TryGetValue(sourceId, out visualizatorModule))
            {
                return visualizatorModule.DoCommand(sourceId, command);
            }
            return null;
        }

        public void Pause()
        {
            List<IAsyncResult> asyncResults = new List<IAsyncResult>(_config.ModuleList.Length);
            foreach (IModule module in _config.ModuleList)
            {
                asyncResults.Add(AsyncCaller.BeginCall<IVisualizatorModule>(Pause, module.VisualizatorModule));
            }
            foreach (IAsyncResult asyncResult in asyncResults)
            {
                AsyncCaller.EndCall<IVisualizatorModule>(asyncResult);
            }
        }

        private void Pause(IVisualizatorModule visualizator)
        {
            try
            {
                visualizator.Pause();
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowAgent.Pause: \n{0}\n{1}",
                                                          visualizator, ex));
            }
        }

        #endregion

        //private void DestroyPreviousWindow()
        //{
        //    foreach (KeyValuePair<IVisualizatorModule, List<IntPtr>> pair in _activeWindowHandleMapping)
        //    {
        //        foreach (IntPtr ptr in pair.Value)
        //        {
        //            pair.Key.Destroy(ptr);
        //        }
        //    }
        //    _activeWindowHandleMapping.Clear();
        //}

        private void ShowWindow(IVisualizatorModule visualizator, Window[] windows)
        {
            try
            {
                foreach (Window window in windows)
                {
                    if (window.Source.ResourceDescriptor != null)
                        _resourceManager.CorrectResourceFileName(window.Source.ResourceDescriptor);
                }
                visualizator.Show(CurrentDisplay, windows);
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ShowAgent.ShowWindow: \n{0}\n{1}",
                                                          visualizator, ex));
            }
            //IVisualizatorModule visualizatorModule;
            //if (_sourceTypeMapping.TryGetValue(window.Source.GetType(), out visualizatorModule))
            //{
            //    // если сорс с файлом то для него нужно прописать правильный путь, где файл лежит
            //    if (window.Source.ResourceDescriptor != null)
            //        _resourceManager.CorrectResourceFileName(window.Source.ResourceDescriptor);
            //    IntPtr handle = visualizatorModule.Show(window);
            //    AddHandleMapping(visualizatorModule, handle);
            //    return new WindowPtr(window, handle);
            //}
            //return null;
        }

        #region Image при сбое в ядре

        private CommunicationState _previouseState = CommunicationState.Closed;
        private ICommunicationObject _communicationObject = null;

        void _restoringTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(this))
            {
                try
                {
                    if (_communicationObject == null) return;
                    CommunicationState state = _communicationObject.State;
                    try
                    {
                        if ((state == CommunicationState.Closed || state == CommunicationState.Faulted)
                            && state != _previouseState)
                        {
                            string restoreImage = Path.Combine(_config.RestoreImagePath,
                                                               _config.LoadSystemParameters().ReloadImage);
                            if (!File.Exists(restoreImage)) return;
                            // закроем все открытые окошки
                            HashSet<IVisualizatorModule> visualizatorModules = new HashSet<IVisualizatorModule>(_sourceIdVisualizatorMapping.Values);
                            foreach (IVisualizatorModule visualizator in visualizatorModules)
                            {
                                visualizator.HideAll();
                            }
                            _manager.SetBackgroudImage(restoreImage);
                        }
                    }
                    finally
                    {
                        _previouseState = state;
                    }
                }
                catch (Exception ex)
                {
                    _config.EventLog.WriteError(string.Format("ShowAgent._restoringTimer_Elapsed:\n {0}", ex));
                }
                finally
                {
                    Monitor.Exit(this);
                }
            }
        }

        #endregion

        #region Implementation of IPing

        public void Ping(UserIdentity identity)
        {
            _communicationObject = OperationContext.Current.Channel;
        }

        #endregion

        #region Implementation of IFileTransfer

        public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        {
            return _resourceManager.Send(userIdentity, obj);
        }

        public FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        {
            return _resourceManager.Receive(userIdentity, resourceId);
        }

        public void Terminate(UserIdentity userIdentity)
        {
            _resourceManager.Terminate(userIdentity);
        }

        #endregion

        //private WindowPtr ShowWindow(Window window)
        //{
        //    IVisualizatorModule visualizatorModule;
        //    if (_sourceTypeMapping.TryGetValue(window.Source.GetType(), out visualizatorModule))
        //    {
        //        // если сорс с файлом то для него нужно прописать правильный путь, где файл лежит
        //        if (window.Source.ResourceDescriptor != null)
        //            _resourceManager.CorrectResourceFileName(window.Source.ResourceDescriptor);
        //        IntPtr handle = visualizatorModule.Show(window);
        //        AddHandleMapping(visualizatorModule, handle);
        //        return new WindowPtr(window, handle);
        //    }
        //    return null;
        //}

        //private void AddHandleMapping(IVisualizatorModule visualizatorModule, IntPtr handle)
        //{
        //    lock (((ICollection)_activeWindowHandleMapping).SyncRoot)
        //    {
        //        List<IntPtr> intPtrs;
        //        if (!_activeWindowHandleMapping.TryGetValue(visualizatorModule, out intPtrs))
        //        {
        //            _activeWindowHandleMapping[visualizatorModule] = intPtrs = new List<IntPtr>();
        //        }
        //        intPtrs.Add(handle);
        //    }
        //}

        #region Implementation of ISourceTransferCRUD

        public FileSaveStatus InitSourceUpload(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string otherResourceId)
        {
            return _resourceManager.InitSourceUpload(userIdentity, resourceDescriptor, status, out otherResourceId);
        }

        public FileSaveStatus SaveSource(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string otherResourceId)
        {
            otherResourceId = null;
            return null == _resourceManager.SaveSource(userIdentity, resourceDescriptor) ? FileSaveStatus.Abort : FileSaveStatus.Ok;
        }

        public void DoneSourceTransfer(UserIdentity userIdentity)
        {
            _resourceManager.DoneSourceTransfer(userIdentity);
        }

        public ResourceDescriptor InitSourceDownload(UserIdentity identity, ResourceDescriptor resourceDescriptor)
        {
            throw new NotImplementedException("Этот метод не примаеним для агента");
            //return _resourceManager.InitSourceDownload(identity, resourceDescriptor);
        }
        public int ForwardMoveNeeded()
        {
            return _resourceManager.ForwardMoveNeeded();
        }
        
        public double GetCurrentSpeed()
        {
            return _resourceManager.GetCurrentSpeed();
        }

        public string GetCurrentFile()
        {
            return _resourceManager.GetCurrentFile();
        }


        #endregion
    }
}