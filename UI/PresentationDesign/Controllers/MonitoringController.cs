using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UI.PresentationDesign.DesignUI.Controllers.Interfaces;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Controls.DisplayList;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.IO;
using UI.PresentationDesign.DesignUI.Controls.DisplayMonitor;
using Domain.PresentationShow.ShowClient;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    /// <summary>
    /// Обработчик события включения/выключения галочки в списке дисплеев
    /// </summary>
    /// <param name="viewer"></param>
    public delegate void ViewerChanged(IDisplayViewer viewer);
    /// <summary>
    /// Обработчик смены текущего слайда в графе сцен
    /// </summary>
    /// <param name="newCollection"></param>
    public delegate void ViewerCollectionChanged();

    /// <summary>
    /// Контроллер для области мониторинга
    /// </summary>
    public class MonitoringController : IDisposable
    {
        /// <summary>
        /// Срабатывает, когда в списке дисплеев поставили галочку. На это подписан DisplayMonitorControl
        /// </summary>
        public event ViewerChanged OnViewerAdded;
        /// <summary>
        /// Срабатывает, когда в списке дисплеев убрали галочку. На это подписан DisplayMonitorControl
        /// </summary>
        public event ViewerChanged OnViewerRemoved;
        /// <summary>
        /// Срабатывает, когда в графе сцен выбрали другой слайд. На это подписан DisplayMonitorControl
        /// </summary>
        public event ViewerCollectionChanged OnCollectionChanged;
        /// <summary>
        /// Срабатывает, rогда в списке дисплеев выделили дисплей
        /// </summary>
        public event ViewerChanged OnViewerSelected;

        private static System.Drawing.Rectangle defaultPos = new System.Drawing.Rectangle(0, 0, 200, 150);
        private Dictionary<String, DisplayViewer> m_Viewers = new Dictionary<string, DisplayViewer>();

        public float MaxWidth
        {
            get { return DisplayController.Instance.UngrouppedDisplays().Select(x => x.Width).Max(); }
        }

        private static MonitoringController _instance;

        public static MonitoringController Instance
        {
            get { return _instance; }
        }

        public static void CreateController()
        {
            _instance = new MonitoringController();
        }

        private MonitoringController()
        {
            DisplayController.Instance.OnDisplayChecked += Instance_OnDisplayChecked;
            ShowClient.Instance.OnGoToSlide += Instance_OnGoToSlide;
            PresentationController.Instance.OnPlaySelectionChanged += Instance_OnPlaySelectionChanged;
            ShowClient.Instance.OnSlidePlay += Instance_OnSlidePlay;
            PresentationController.Instance.OnSlideChangedExternally += Instance_OnSlideChangedExternally;
            DisplayController.Instance.OnSelectedDisplayChanged += Instance_OnSelectedDisplayChanged;
        }

        private void Instance_OnSelectedDisplayChanged(Display obj)
        {
            IEnumerable<KeyValuePair<string, DisplayViewer>> views = m_Viewers.Where(v => v.Key.Equals(obj.Type.Name));
            foreach (KeyValuePair<string, DisplayViewer> item in views)
            {
                if (OnViewerSelected != null)
                    OnViewerSelected(item.Value);
            }
        }

        public void SelectDisplayView(IDisplayViewer view)
        {
            DisplayController.Instance.OnSelectedDisplayChanged -= Instance_OnSelectedDisplayChanged;
            DisplayController.Instance.ChangeSelectedDisplay(view.Display);
            DisplayController.Instance.OnSelectedDisplayChanged += Instance_OnSelectedDisplayChanged;
        }

        private void Instance_OnPlaySelectionChanged(Slide slide)
        {
            Instance_OnSlidePlay();
        }

        private void Instance_OnGoToSlide(int obj)
        {
            Instance_OnSlidePlay();
        }

        private void Instance_OnSlideChangedExternally(Slide slide)
        {
            Instance_OnSlidePlay();
        }

        /// <summary>
        /// Обработчик смены текущего слайда
        /// </summary>
        /// <param name="NewSelection">Новый набор выделенных слайдов</param>
        void Instance_OnSlideSelectionChanged(IEnumerable<TechnicalServices.Persistence.SystemPersistence.Presentation.Slide> NewSelection)
        {
            Instance_OnSlidePlay();
        }

        private void Instance_OnSlidePlay()
        {
            foreach (var pair in m_Viewers)
                pair.Value.ReloadImage();
            if (OnCollectionChanged != null)
                OnCollectionChanged();
        }

        /// <summary>
        /// Обрабочик включения/выключения галочки в списке дисплеев
        /// </summary>
        /// <param name="disp">Измененный дисплей</param>
        /// <param name="isChecked">Новое значение галочки</param>
        /// <returns>true, если данный дисплей можно включить</returns>
        bool Instance_OnDisplayChecked(TechnicalServices.Persistence.SystemPersistence.Presentation.Display disp, bool isChecked)
        {
            if (isChecked)
            {
                DisplayViewer viewer = new DisplayViewer(disp) { Pos = getPosition(disp) };
                m_Viewers.Add(disp.Type.Name, viewer);
                if (OnViewerAdded != null)
                    OnViewerAdded(viewer);
            }
            else if (OnViewerRemoved != null)
            {
                m_Viewers.Remove(disp.Type.Name);
                OnViewerRemoved(new DisplayViewer(disp) { Pos = defaultPos });
            }
            PresentationController.Instance.NotifyMonitorListChanged(from pair in m_Viewers select pair.Value.Display);
            return true;
        }

        [Serializable]
        public struct MonitoringSaver
        {
            public String Name { get; set; }
            public System.Drawing.Rectangle Pos { get; set; }
        }

        public void SavePositions(String path)
        {
            try
            {
                MonitoringSaver[] rects = m_Viewers.Select(x => new MonitoringSaver { Name = x.Key, Pos = x.Value.Pos }).ToArray();
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(rects.GetType());
                ser.Serialize(new FileStream(path, FileMode.Create), rects);
            }
            catch (Exception /*ex*/)
            {
                // не палимся и рушим программу из-за мелкой ошибки
            }
        }

        public void LoadPositions(String path)
        {
            if (!File.Exists(path))
                return;
            try
            {
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(MonitoringSaver[]));
                MonitoringSaver[] rects = (MonitoringSaver[])ser.Deserialize(new FileStream(path, FileMode.Open));
                foreach (var r in rects)
                {
                    Display disp = DisplayController.Instance.UngrouppedDisplays().Where(x => x.Type.Name == r.Name).FirstOrDefault();
                    if (disp == null)
                        disp = (from gr in DisplayController.Instance.GrouppedDisplays() from d in gr.Value where d.Type.Name == r.Name select d).FirstOrDefault();
                    if (disp == null)
                        continue;
                    DisplayViewer viewer = new DisplayViewer(disp) { Pos = r.Pos, IsTransformed = true };
                    m_Viewers.Add(r.Name, viewer);
                    DisplayController.Instance.ForceCheckDisplay(r.Name, true);
                    if (OnViewerAdded != null)
                        OnViewerAdded(viewer);
                }
                PresentationController.Instance.NotifyMonitorListChanged(from pair in m_Viewers select pair.Value.Display);
            }
            catch (Exception /*ex*/)
            {
                // не ломаем ничего если не получилось загрузить позиции
            }
        }

        private System.Drawing.Rectangle getPosition(Display disp)
        {
            return new System.Drawing.Rectangle(0, 0, disp.Width, disp.Height);
        }

        public System.Drawing.Size getDefaultSize(IDisplayViewer viewer)
        {
            return new System.Drawing.Size((viewer as DisplayViewer).Display.Width, (viewer as DisplayViewer).Display.Height + MonitorRectangle.HEADER_HEIGHT);
        }

        /// <summary>
        /// Создать новый DisplayViewer при переносе мышкой дисплея на область мониторинга
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Interfaces.IDisplayViewer addViewer(UI.PresentationDesign.DesignUI.Controls.DisplayList.DisplayNode node)
        {
            if (m_Viewers.ContainsKey(node.Display.Type.Name))
                return null;
            DisplayViewer viewer = new DisplayViewer(node.Display) { Pos = getPosition(node.Display) };
            m_Viewers.Add(node.Display.Type.Name, viewer);
            DisplayController.Instance.ForceCheckDisplay(viewer.Name, true);
            PresentationController.Instance.NotifyMonitorListChanged(from pair in m_Viewers select pair.Value.Display);
            return viewer;
        }

        public List<Interfaces.IDisplayViewer> addGroup(DisplayGroupNode node)
        {
            List<Interfaces.IDisplayViewer> retValue = new List<IDisplayViewer>();
            List<Display> displays = DisplayController.Instance.ForceCheckGroup(node);
            foreach (Display disp in displays)
            {
                if (!m_Viewers.ContainsKey(disp.Type.Name))
                {
                    DisplayViewer viewer = new DisplayViewer(disp) { Pos = getPosition(disp) };
                    m_Viewers.Add(disp.Type.Name, viewer);
                    retValue.Add(viewer);
                }
            }
            PresentationController.Instance.NotifyMonitorListChanged(from pair in m_Viewers select pair.Value.Display);
            return retValue;
        }
        /// <summary>
        /// Удалить из внутренней коллекции DisplayViewer, связанный удаленным окошком
        /// </summary>
        /// <param name="viewer"></param>
        public void removeViewer(IDisplayViewer viewer)
        {
            m_Viewers.Remove(viewer.Name);
            DisplayController.Instance.ForceCheckDisplay(viewer.Name, false);
            PresentationController.Instance.NotifyMonitorListChanged(from pair in m_Viewers select pair.Value.Display);
        }
        /// <summary>
        /// Проверяет можно ли добавить дисплей на область мониторинга (разрешить grag-n-drop)
        /// </summary>
        /// <param name="node">Перетаскиваемая нода</param>
        /// <returns>true, если можно</returns>
        public bool canAddViewer(DisplayNode node)
        {
            return !m_Viewers.ContainsKey(node.Display.Type.Name);
        }

        /// <summary>
        /// Проверяет, можно ли добавить группу дисплеев на область мониторинга (разрешить drag-n-drop)
        /// </summary>
        /// <param name="node">Нода группы дисплеев</param>
        /// <returns>true, если можно</returns>
        public bool canAddGroup(DisplayGroupNode node)
        {
            foreach(String name in node.DisplayGroup.DisplayNameList)
                if(!m_Viewers.ContainsKey(name))
                    return true;
            return false;
        }

        public void Dispose()
        {
            if (DisplayController.Instance != null)
                DisplayController.Instance.OnDisplayChecked -= Instance_OnDisplayChecked;
            if (ShowClient.Instance != null)
            {
                ShowClient.Instance.OnGoToSlide -= Instance_OnGoToSlide;
                ShowClient.Instance.OnSlidePlay -= Instance_OnSlidePlay;
            }
            if (PresentationController.Instance != null)
            {
                PresentationController.Instance.OnSlideChangedExternally -= Instance_OnSlideChangedExternally;
                PresentationController.Instance.OnPlaySelectionChanged -= Instance_OnPlaySelectionChanged;
            }
            if (DisplayController.Instance != null)
                DisplayController.Instance.OnSelectedDisplayChanged -= Instance_OnSelectedDisplayChanged;
            _instance = null;
        }

        /// <summary>
        /// Обновить все скриншоты.
        /// </summary>
        public void RefreshScreenShots()
        {
            foreach(DisplayViewer viewer in m_Viewers.Values)
            {
               viewer.ReloadImage();
            }
        }

        /// <summary>
        /// Признак наличия окон для мониторинга.
        /// </summary>
        public bool HasMonitoredWindow
        {
            get
            {
                return m_Viewers.Count > 0;
            }
        }

    }
}
