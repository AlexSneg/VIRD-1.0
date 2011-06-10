using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;

using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Interfaces.ConfigModule.Visualizator;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Util;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    public abstract class DomainVisualizatorModule<TExecute, TWindow> : IVisualizatorModule
        where TExecute : MarshalByRefObject, IExecute, new()
        where TWindow : Window
    {
        #region private

        private AppDomain _domain;
        private IExecute _execute;
        private IEventLogging _log;

        private readonly List<WindowPtr<TWindow>> _currentWindowList = new List<WindowPtr<TWindow>>();

        private readonly IEqualityComparer<TWindow> _comparer = new BaseComparer();

        #region Nested

        private class BaseComparer : BaseWindowEqualityComparer, IEqualityComparer<TWindow>
        {
            public bool Equals(TWindow x, TWindow y)
            {
                return base.Equals(x, y);
            }

            public int GetHashCode(TWindow window)
            {
                return base.GetHashCode(window);
            }
        }

        private class WindowPtr<TWindow> where TWindow : Window
        {
            internal WindowPtr(TWindow window, IntPtr handle)
            {
                Window = window;
                Handle = handle;
            }
            internal TWindow Window { get; set; }
            internal IntPtr Handle { get; set; }
        }

        #endregion

        private void RemoveDeadWindows()
        {
            _currentWindowList.RemoveAll(win => !_execute.IsAlive(win.Handle));
        }

        private void CloseOldWindow(IEnumerable windows)
        {
            IEnumerable<WindowPtr<TWindow>> windowToClose = _currentWindowList.Where(wptr => !windows.Cast<TWindow>().Contains(wptr.Window, EquatableComparer));
            List<IAsyncResult> asyncResults = new List<IAsyncResult>();
            foreach (WindowPtr<TWindow> windowPtr in windowToClose)
                asyncResults.Add(AsyncCaller.BeginCall<IntPtr>(CloseWindow, windowPtr.Handle));
            foreach (IAsyncResult aResult in asyncResults)
                AsyncCaller.EndCall<IntPtr>(aResult);
            //IEnumerable<TWindow> toClose = windowToClose.Select(wptr => wptr.Window);
            _currentWindowList.RemoveAll(wptr => windowToClose.Any(wtc => object.ReferenceEquals(wptr, wtc)));  //toClose.Contains(wptr.Window, EquatableComparer)
        }

        private void CloseWindow(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;
            try
            {
                _execute.DestroyForm(handle);
            }
            catch (Exception ex)
            {
                _log.WriteError(string.Format("DomainVisualizatorModule.CloseWindow: {0}", ex));
            }
        }

        private void ProcessSourceStateChange(IEnumerable windows)
        {
            try
            {
                IEnumerable<TWindow> windowIntersected =
                    windows.Cast<TWindow>().Intersect(_currentWindowList.Select(wptr => wptr.Window), EquatableComparer);
                Dictionary<IntPtr, List<string>> diffCommand =
                    new Dictionary<IntPtr, List<string>>(windowIntersected.Count());
                foreach (TWindow tWindow in windowIntersected)
                {
                    WindowPtr<TWindow> windowPtr =
                        _currentWindowList.Single(wptr => EquatableComparer.Equals(wptr.Window, tWindow));
                    diffCommand[windowPtr.Handle] =
                        new List<string>(GetChangedState(windowPtr.Window.Source, tWindow.Source));
                }
                foreach (KeyValuePair<IntPtr, List<string>> keyValuePair in diffCommand)
                {
                    foreach (string command in keyValuePair.Value)
                    {
                        _execute.DoCommand(keyValuePair.Key, command);
                    }
                }
            }
            catch(Exception ex)
            {
                _log.WriteError(string.Format("DomainVisualizatorModule.ProcessSourceStateChange: \n{0}", ex));
            }
        }

        private void ShowNewWindow(DisplayType display, IEnumerable windows)
        {
            IEnumerable<TWindow> windowToShow = windows.Cast<TWindow>().Except(_currentWindowList.Select(wptr=>wptr.Window).Cast<TWindow>(), EquatableComparer);
            List<IAsyncResult> asyncResults = new List<IAsyncResult>();
            foreach (TWindow tWindow in windowToShow)
                asyncResults.Add(AsyncCallerResult.BeginCall<DisplayType, TWindow, WindowPtr<TWindow>>(ShowWindow, display, tWindow));
            foreach (IAsyncResult aResult in asyncResults)
            {
                WindowPtr<TWindow> wptr = AsyncCallerResult.EndCall<DisplayType, TWindow, WindowPtr<TWindow>>(aResult);
                if (wptr.Handle != IntPtr.Zero) _currentWindowList.Add(wptr);
            }
        }

        private WindowPtr<TWindow> ShowWindow(DisplayType display, TWindow tWindow)
        {
            try
            {
                return new WindowPtr<TWindow>(tWindow,_execute.ShowForm(display, tWindow));
            }
            catch(Exception ex)
            {
                _log.WriteError(string.Format("DomainVisualizatorModule.ShowWindow: {0}", ex));
            }
            return new WindowPtr<TWindow>(tWindow,IntPtr.Zero);
        }

        private void SyncWindow(IEnumerable windows)
        {
            for (int i = 0; i < _currentWindowList.Count; i++ )
            {
                TWindow window = _currentWindowList[i].Window;
                IEnumerable<TWindow> windowArr = windows.Cast<TWindow>().Where(win => EquatableComparer.Equals((TWindow)win, window));
                if (windowArr.Count() == 1) _currentWindowList[i].Window = windowArr.First();
            }
        }

        private void SortByZOrder()
        {
            _currentWindowList.Sort((wptr1,wptr2)=>wptr1.Window.ZOrder.CompareTo(wptr2.Window.ZOrder));
            foreach (WindowPtr<TWindow> windowPtr in _currentWindowList)
            {
                _execute.BringToFront(windowPtr.Handle);
            }
        }

        private void CloseAllWindows()
        {
            _currentWindowList.ForEach(wptr=>CloseWindow(wptr.Handle));
            _currentWindowList.Clear();
        }

        void _domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.WriteError(string.Format("_domain_UnhandledException sender: {0}, Exception: {1}", sender, e.ExceptionObject));
        }


        #endregion

        #region protected

        /// <summary>
        /// хитрый компарер - должен учитывать то что важно для визуализации, т.е. к примеру параметры окна + файл, который показывается
        /// </summary>
        protected virtual IEqualityComparer<TWindow> EquatableComparer { get { return _comparer; } }

        /// <summary>
        /// метод используется при изменеии стайта сорсов при переходе к другому слайду, если раскладка окна не имзенилось
        /// возвращаемый списорк команд должен быть понятен плагинам, так как они будут в дальнейшем посыласться этим же плагинам
        /// </summary>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <returns></returns>
        protected virtual IEnumerable<string> GetChangedState(Source source1, Source source2)
        {
            return new string[] {};
        }

        /// <summary>
        /// получение команды пауза
        /// </summary>
        /// <returns></returns>
        protected virtual string GetPauseCommand()
        {
            return null;
        }

        /// <summary>
        /// установка для сорса состояния пауза, если конечно такое состояние есть
        /// </summary>
        /// <param name="source"></param>
        protected virtual void SetStateToPause(Source source)
        {}

        #endregion

        #region IVisualizatorModule Members

        public bool IsSupportView
        {
            get { return true; }
        }

        public bool Init(IEventLogging log)
        {
            Debug.Assert(log != null, "IEventLogging не может быть null");
            _currentWindowList.Clear();
            _log = log;
            try
            {
                AppDomainSetup ads = new AppDomainSetup();
                ads.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                ads.PrivateBinPath = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).CodeBase); // "Module";
                ads.DisallowBindingRedirects = false;
                ads.DisallowCodeDownload = true;
                ads.ConfigurationFile =
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

                _domain = AppDomain.CreateDomain(typeof(TExecute).Name, null, ads);
                //_domain.UnhandledException += new UnhandledExceptionEventHandler(_domain_UnhandledException);
                _execute =
                    (IExecute)
                    _domain.CreateInstanceFromAndUnwrap(typeof(TExecute).Assembly.CodeBase, typeof(TExecute).FullName);
                return _execute.Init();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex.ToString());
                return false;
            }
        }

        public void Show(DisplayType display, Window[] windows)
        {
            if (_execute == null) return;
            // некоторые окна могли быть закрыты ручками - очистим список
            RemoveDeadWindows();
            // расьираем окошки - которые уже есть и новые. если есть совпадения их оставляем, старые закрываем, новые открываем
            // закрываем те окошки которых нет
            CloseOldWindow(windows);
            // для совпадающих окошек - надо понять изменилось ли состояние источников (к примеру было Play, стало Stop)
            // без переоткрытия окон - нужно отправить соответсвующие команды сорсам
            ProcessSourceStateChange(windows);
            // открываем новые
            ShowNewWindow(display, windows);
            // синхронизуем внутренний массив текущих окон с актуальными - чтобы были теже самые объкты
            SyncWindow(windows);
            // упорядочим в соответсивии с ZOrder - это упорядочивание делается в ShowAgent, так как должно выполняться для всех визуализаторогв
            //SortByZOrder();
        }


        public void Done()
        {
            _currentWindowList.Clear();
            try
            {
                if (_execute != null) _execute.Done();
                if (_domain != null) AppDomain.Unload(_domain);
            }
            catch (Exception ex)
            {
                _log.WriteError(string.Format("DomainVisualizatorModule.Done: \n{0}",ex));
            }
        }

        public string DoCommand(string sourceId, string command)
        {
            if (_execute == null) return null;
            try
            {
                WindowPtr<TWindow> windowPtr =
                    _currentWindowList.SingleOrDefault(wptr => wptr.Window.Source.Id == sourceId);
                if (windowPtr == null) return null;
                return _execute.DoCommand(windowPtr.Handle, command);
            }
            catch(Exception ex)
            {
                _log.WriteError(string.Format("DomainVisualizatorModule.DoCommand: \n{0}", ex));
            }
            return null;
        }

        public void Pause()
        {
            if (_execute == null) return;
            try
            {
                string pauseCommand = GetPauseCommand();
                if (!string.IsNullOrEmpty(pauseCommand))
                {
                    foreach (WindowPtr<TWindow> windowPtr in _currentWindowList)
                    {
                        SetStateToPause(windowPtr.Window.Source);
                        _execute.DoCommand(windowPtr.Handle, pauseCommand);
                    }
                }
            }
            catch(Exception ex)
            {
                _log.WriteError(string.Format("DomainVisualizatorModule.Pause: \n{0}", ex));
            }
        }

        public void BringToFront(Window window)
        {
            if (_execute == null) return;
            try
            {
                WindowPtr<TWindow> windowPtr =
                    _currentWindowList.SingleOrDefault(wptr => wptr.Window.Source.Id == window.Source.Id);
                if (windowPtr != null)
                {
                    _execute.BringToFront(windowPtr.Handle);
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(string.Format("DomainVisualizatorModule.Pause: \n{0}", ex));
            }
        }

        public void HideAll()
        {
            CloseAllWindows();
        }

        #endregion
    }
}