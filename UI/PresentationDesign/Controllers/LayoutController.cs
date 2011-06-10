using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Domain.PresentationShow.ShowCommon;
using Microsoft.Win32;

using Syncfusion.Windows.Forms.Diagram;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using UI.Common.CommonUI.Helpers;
using UI.PresentationDesign.DesignUI.Controls.SourceTree;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Domain.PresentationDesign.Client;
using UI.PresentationDesign.DesignUI.Controls;
using System.Drawing;
using Syncfusion.Windows.Forms;
using System;
using System.Windows.Forms;
using UI.PresentationDesign.DesignUI.Controllers;
using TechnicalServices.Common.Utils;
using System.Collections.Generic;
using Domain.PresentationShow.ShowClient;
using UI.PresentationDesign.DesignUI.Services;
using UI.PresentationDesign.DesignUI.Views;
using System.IO;
using UI.PresentationDesign.DesignUI.Model;
using UI.PresentationDesign.DesignUI.Forms;
using TechnicalServices.Interfaces;
using TechnicalServices.Util.FileTransfer;
using UI.PresentationDesign.DesignUI.Helpers;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using System.Threading;
using TechnicalServices.Common.Classes;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    public enum CreateWindowStatus
    {
        WindowCreated,
        WindowReplaced,
        InvalidDisplay,
        InvalidResource,
        NotSupportMultiWindow,
        PluginError
    }

    public class LayoutController : DiagramController, ISelectionController, IDisposable, IDesignServiceProvider
    {
        #region fields and properties
        private static LayoutController _instance;
        Presentation m_presentation;
        PresentationInfo m_presentationInfo;
        SlideLayoutControl _view;

        SlideLayout CurrentLayout;
        BackgroundProvider backProvider;

        public event Action<DisplayType> OnNotEnoughSpace;
        public event Action<bool> OnShownStatusChanged;

        public bool InternalChanging
        {
            get
            {
                return this.internalChanging;
            }
        }

        public bool Readonly
        {
            set
            {
                _view.ReadOnly = value;
            }
        }

        public static LayoutController Instance
        {
            get
            {
                return _instance;
            }
        }

        public SourceWindow SelectedWindow
        {
            get
            {
                var hit = NodesHit.OfType<SourceWindow>();
                if (hit.Count() > 0)
                    return hit.First();
                else
                {
                    hit = SelectionList.OfType<SourceWindow>();
                    if (hit.Count() > 0)
                        return hit.First();
                    return null;
                }
            }
        }

        bool _slideLocked = false;

        public bool CanEdit
        {
            get
            {
                return _slideLocked | DesignerClient.Instance.IsStandAlone;
            }
        }

        Dictionary<string, BackgroundImageDescriptor> backgrounds = new Dictionary<string, BackgroundImageDescriptor>();

        #endregion

        #region ctor and factories
        public LayoutController()
            : base()
        {
            _instance = this;
            Tool tmp = this.GetTool(ToolDescriptor.PanTool);
            this.UnRegisterTool(tmp);
            this.RegisterTool(new PresentationPanTool(this));
            ShowClient.Instance.OnPreparationFinished += new Action(Instance_OnPreparationFinished);
            ShowClient.Instance.OnProgressChanged += new Action<int, int, string>(Instance_OnProgressChanged);
            ShowClient.Instance.OnNotEnoughSpace += new Action<DisplayType>(Instance_OnNotEnoughSpace);
            ShowClient.Instance.OnShownStatusChanged += new Action<bool>(Instance_OnShownStatusChanged);
        }


        public override void UpdateServiceReferences(IServiceReferenceProvider provider)
        {
            base.UpdateServiceReferences(provider);
        }

        void HistoryManager_CommandCompleted(object sender, EventArgs e)
        {
            if (!internalChanging)
                PresentationController.Instance.PresentationChanged = true;
        }

        #endregion

        #region ISelectionController Members
        public void CheckSelection()
        {
            SourceWindow selected = null;
            if (this.SelectionList.Count > 0 && this.SelectionList.OfType<SourceWindow>().Count() > 0)
            {
                selected = this.SelectionList.OfType<SourceWindow>().First();
            }

            PresentationController.Instance.SendOnSelectedResourceChanged(selected);
        }

        public void CheckHitSelection()
        {
            CheckSelection();
        }
        #endregion

        #region init controller

        public void AssignView(SlideLayoutControl AView)
        {
            _view = AView;
        }

        public void InitLayoutController()
        {
            m_presentation = PresentationController.Instance.Presentation;
            m_presentationInfo = PresentationController.Instance.PresentationInfo;
            PresentationController.Instance.OnSlideLockChanged += new SlideLockChanged(OnSlideLockChanged);
            PresentationController.Instance.OnSlideLayoutChanged += new SlideLayoutChanged(Instance_OnSlideLayoutChanged);
            PresentationController.Instance.OnSavePresentation += new SavePresentation(Instance_OnSavePresentation);
            UndoService.Instance.OnHistoryChanged += new HistoryChanged(OnHistoryChanged);
            this.Model.HistoryManager.RecordComplete += new EventHandler(HistoryManager_CommandCompleted);
            DesignerClient.Instance.PresentationNotifier.OnResourceAdded += new EventHandler<NotifierEventArg<ResourceDescriptor>>(PresentationNotifier_OnResourceAdded);
            DesignerClient.Instance.PresentationNotifier.OnResourceDeleted += new EventHandler<NotifierEventArg<ResourceDescriptor>>(PresentationNotifier_OnResourceDeleted);
            backProvider = new BackgroundProvider();
            //load background sources
            Dictionary<string, IList<ResourceDescriptor>> _rd = DesignerClient.Instance.PresentationWorker.GetLocalSources(m_presentation.UniqueName);
            foreach (var resource in _rd)
            {
                foreach (ResourceDescriptor r in resource.Value)
                    if (r is BackgroundImageDescriptor)
                    {
                        backgrounds.Add(r.ResourceInfo.Id, (BackgroundImageDescriptor)r);
                    }
            }

            Instance_OnSlideLayoutChanged(PresentationController.Instance.CurrentSlideLayout);
        }

        #endregion

        #region event handlers

        void Instance_OnSavePresentation()
        {
            SaveWindows();
        }

        void OnHistoryChanged(object Target)
        {
            if (this.SelectedWindow != null && Target == this.SelectedWindow.Window)
            {
                Display disp = PresentationController.Instance.CurrentSlideLayout.Display;
                PresentationController.Instance.PresentationChanged = true;

                if (disp is PassiveDisplay)
                {
                    //nop
                }
                else
                {
                    this.SelectedWindow.Refresh();
                }
                this.Viewer.UpdateView();
                this.InvalidateView();
            }
        }


        void OnSlideLockChanged(Slide slide, bool IsLocked, LockingInfo info)
        {
            if (CurrentLayout != null)
            {
                if (slide == CurrentLayout.Slide)
                {
                    _slideLocked = IsLocked && PresentationController.Instance.CanUnlockSlide(slide);
                    _view.EnableEdit(CanEdit);
                }
            }
        }

        bool internalChanging = false;
        protected override void Document_NodeCollectionChanged(CollectionExEventArgs evtArgs)
        {
            base.Document_NodeCollectionChanged(evtArgs);

            if (evtArgs.ChangeType == CollectionExChangeType.Insert && !internalChanging)
            {
                ActivateTool(ToolDescriptor.SelectTool);
            }
        }

        void Instance_OnSlideLayoutChanged(SlideLayout NewSlideLayout)
        {
            SaveWindows();

            MethodInvoker i = new MethodInvoker(() =>
                {
                    if (NewSlideLayout != null)
                        _slideLocked = PresentationController.Instance.CanUnlockSlide(NewSlideLayout.Slide);
                    bool _isEmpty = NewSlideLayout == null || NewSlideLayout.IsEmpty;
                    if (CanEdit)
                        _view.ReadOnly = _isEmpty;
                    else
                        _view.ReadOnly = true;

                    LoadLayout(NewSlideLayout);
                    CurrentLayout = NewSlideLayout;
                });

            if (_view.IsHandleCreated)
            {
                _view.Invoke(i);
            }
            else
            {
                i.Invoke();
            }
        }

        #endregion

        #region Work with windows
        void PresentationNotifier_OnResourceDeleted(object sender, NotifierEventArg<ResourceDescriptor> e)
        {
            if (e.Data is BackgroundImageDescriptor && e.Data.PresentationUniqueName == m_presentation.UniqueName)
            {
                if (backgrounds.ContainsKey(e.Data.ResourceInfo.Id))
                    backgrounds.Remove(e.Data.ResourceInfo.Id);
            }
        }

        void PresentationNotifier_OnResourceAdded(object sender, NotifierEventArg<ResourceDescriptor> e)
        {
            if (e.Data is BackgroundImageDescriptor && e.Data.PresentationUniqueName == m_presentation.UniqueName)
            {
                if (!backgrounds.ContainsKey(e.Data.ResourceInfo.Id))
                    backgrounds.Add(e.Data.ResourceInfo.Id, (BackgroundImageDescriptor)e.Data);
            }
        }

        private void LoadBackground(SlideLayout layout)
        {
            string imId = null;
            int x = -1; int y = -1;
            //download the background image
            if (layout != null)
            {
                if (layout.Display is ActiveDisplay)
                {
                    string id = ((ActiveDisplay)layout.Display).BackgroundImage;
                    if (!String.IsNullOrEmpty(id))
                    {
                        x = layout.Display.Size.X;
                        y = layout.Display.Size.Y;
                        if (backgrounds.ContainsKey(id))
                        {
                            BackgroundImageDescriptor bid = backgrounds[id];
                            imId = GetFile(bid);
                        }
                    }
                }
            }

            backProvider.SetBackgroundImage(imId, this.View as IBackgroundSupport, x, y);
            ((Control)(this.Viewer)).Invalidate();

        }


        private string GetFile(BackgroundImageDescriptor bid)
        {
            if (ClientResourceCRUD.GetSource(bid, true))
            {
                ResourceDescriptor rd = DesignerClient.Instance.PresentationWorker.SourceDAL.GetStoredSource(bid);
                return ((ResourceFileInfo)rd.ResourceInfo).ResourceFullFileName;
            }
            else
            {
                return String.Empty;
            }
        }


        void LoadLayout(SlideLayout layout)
        {
            internalChanging = true;
            Model.HistoryManager.Pause();
            Model.Clear();
            View.Origin = new PointF(0, 0);
            LoadBackground(layout);
            if (layout != null && !layout.IsEmpty)
            {
                if (layout.Slide.SourceList.Count > 0)
                    PresentationController.Instance.SourceID = new Identity(layout.Slide.SourceList.Max(s => Int32.Parse(s.Id)));
                else
                    PresentationController.Instance.SourceID = new Identity(0);

                Display display = layout.Display;
                this.Model.DocumentSize = new PageSize(display.Width, MeasureUnits.Pixel, display.Height, MeasureUnits.Pixel);

                float width = ((Control)Viewer).Width;
                float height = ((Control)Viewer).Height;

                this.View.Magnification = Math.Min((width - 40f) / (float)display.Width, (height - 40f) / (float)display.Height) * 100f;

                Model.BeginUpdate();
                SourceWindow last = null;
                foreach (Window w in display.WindowList.OrderBy(wnd => wnd.ZOrder))
                {
                    SourceWindow window;

                    if (w.Source != null && w.Source.ResourceDescriptor != null)
                    {
                        window = new SourceWindow(w);
                        // Для пассивного односгемнтного дисплея нельзя изменить положение окна источника на раскладке
                        ISegmentationSupport segmentSupport = display as ISegmentationSupport;
                        bool allowResize = true;
                        if ((display is PassiveDisplay) &&
                            (segmentSupport == null || (segmentSupport.SegmentColumns == 1 && segmentSupport.SegmentRows == 1)))
                            allowResize = false;

                        window.EditStyle.AllowChangeHeight = allowResize;
                        window.EditStyle.AllowChangeWidth = allowResize;
                        window.EditStyle.AllowMoveX = allowResize;
                        window.EditStyle.AllowMoveY = allowResize;

                        Model.SetZOrder(window, w.ZOrder);
                    }
                    else
                    {
                        continue;
                    }
                    Model.AppendChild(window);
                    last = window;
                    if (last != null) ProvideReferenceFor(last);
                }
                Model.EndUpdate();
                PresentationController.Instance.SendOnSelectedResourceChanged(last);
            }

            internalChanging = false;
            Model.HistoryManager.Resume();
        }

        private void ProvideReferenceFor(SourceWindow window)
        {
            if (window.Window != null && window.Window.Source != null)
            {
                IDesignInteractionSupport support = window.Window.Source as IDesignInteractionSupport;
                if (support != null)
                    support.UpdateServiceReference(this);
            }
        }


        /// <summary>
        /// Выполняет сохранение позиций окон
        /// </summary>
        public void SaveWindows()
        {
            foreach (SourceWindow wnd in Model.Nodes.OfType<SourceWindow>())
            {
                wnd.Window.Left = wnd.GetPosition().X;
                wnd.Window.Top = wnd.GetPosition().Y;
                wnd.Window.Height = (int)wnd.GetSize().Height;
                wnd.Window.Width = (int)wnd.GetSize().Width;
                wnd.Window.ZOrder = (byte)wnd.ZOrder;
            }
        }

        /// <summary>
        /// Выполняет создание окна на раскладке с проверкой перекрытий
        /// </summary>
        public bool CreateWindow(Node node, PointF position, out CreateWindowStatus status)
        {
            SourceWindow wnd = node as SourceWindow;
            if (wnd != null && CurrentLayout != null && !CurrentLayout.IsEmpty)
            {
                Display currentDisplay = CurrentLayout.Display;
                ISegmentationSupport segmentSupport = currentDisplay as ISegmentationSupport;

                // не понял зачем идет проверка дальше, ведь у каждого дисплея есть маппинг тут сразу проверяем
                //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-939
                if (!currentDisplay.Type.MappingList.Any(map => map.Source.Equals(wnd.SourceType)))
                {
                    status = CreateWindowStatus.InvalidDisplay;
                    return false;
                }

                if (currentDisplay is PassiveDisplay)
                {
                    if (!wnd.Mapping.ResourceInfo.IsHardware)
                    {
                        status = CreateWindowStatus.InvalidDisplay;
                        return false;
                    }

                }

                if (currentDisplay is ActiveDisplay)
                {
                    if ((segmentSupport != null && (segmentSupport.SegmentColumns == 1 && segmentSupport.SegmentRows == 1)) || segmentSupport == null)
                    {
                        //активный односегментный дисплей
                        if (wnd.Mapping.ResourceInfo.IsHardware)
                        {
                            status = CreateWindowStatus.InvalidDisplay;
                            return false;
                        }
                    }
                    else if (segmentSupport != null && (segmentSupport.SegmentColumns > 1 || segmentSupport.SegmentRows > 1))
                    //активный многосегментный дисплей
                    {
                        if (wnd.Mapping.ResourceInfo.IsHardware)
                        {
                            //nop
                        }
                    }
                }

                bool createResult = false;
                try
                {
                    createResult = wnd.CreateSourceWindow(PresentationController.Instance.SourceID, currentDisplay);
                }
                catch (Exception ex)
                {
                    status = CreateWindowStatus.PluginError;
                    MessageBoxExt.Show(ex.Message, "Создание окна", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (createResult)
                {
                    if (wnd.Window.Source.ResourceDescriptor == null)
                    {
                        status = CreateWindowStatus.InvalidResource;
                        return false;
                    }

                    SizeF size = wnd.GetWindowSize();
                    bool allowResize = true;
                    bool onlyOne = false;

                    if (currentDisplay is PassiveDisplay)
                    {
                        if (segmentSupport == null || (segmentSupport != null && (segmentSupport.SegmentColumns == 1 && segmentSupport.SegmentRows == 1)))
                        {
                            //пассивный односегментный дисплей
                            position = new PointF(0, 0);
                            size = new SizeF(currentDisplay.Width, currentDisplay.Height);
                            allowResize = false;
                            onlyOne = true;
                        }
                    }

                    if (currentDisplay is ActiveDisplay)
                    {
                        if (segmentSupport != null && (segmentSupport.SegmentColumns > 1 || segmentSupport.SegmentRows > 1))
                        //активный многосегментный дисплей
                        {
                            if (wnd.Window.Source.ResourceDescriptor.ResourceInfo.IsHardware)
                            {
                                //nop
                            }
                        }

                        if (!currentDisplay.Type.SupportsMultiWindow)
                            onlyOne = true;
                    }


                    RectangleF destRect = new RectangleF(position, size);
                    wnd.SetBoundsInfo(destRect);

                    #region проверка на перекрытие другого ресурса
                    NodeCollection childs = new NodeCollection();

                    int n = Model.GetChildrenIntersecting(childs, destRect);
                    if (n > 0)
                    {
                        var windows = childs.OfType<SourceWindow>().Except(new[] { wnd });
                        if (windows.Count() > 0)
                        {
                            SourceWindow sourceWnd = windows.Last();

                            if (MessageBoxExt.Show(String.Format("Заменить источник?", sourceWnd.Name), Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }) == DialogResult.OK)
                            {
                                CurrentLayout.ReplaceWindows(sourceWnd.Window, wnd.Window);

                                //sourceWnd.Mapping = wnd.Mapping;
                                sourceWnd.Name = wnd.Mapping.ResourceInfo.Name;
                                sourceWnd.Window = wnd.Window;
                                sourceWnd.UpdateWindowInfo();
                                sourceWnd.Refresh();

                                ProvideReferenceFor(sourceWnd);

                                PresentationController.Instance.SendOnSelectedResourceChanged(sourceWnd);
                                status = CreateWindowStatus.WindowReplaced;
                                PresentationController.Instance.PresentationChanged = true;
                                return false;
                            }
                            else
                            {
                                if (onlyOne)
                                {
                                    status = CreateWindowStatus.NotSupportMultiWindow;
                                    return false;
                                }
                            }
                        }
                    }

                    #endregion

                    CurrentLayout.AppendToLayout(wnd.Window);
                    wnd.EditStyle.AllowChangeHeight = allowResize;
                    wnd.EditStyle.AllowChangeWidth = allowResize;
                    //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1492
                    wnd.EditStyle.AllowMoveX = allowResize;
                    wnd.EditStyle.AllowMoveY = allowResize;

                    //PresentationController.Instance.PresentationChanged = true;
                    PresentationController.Instance.SendOnSelectedResourceChanged(wnd);
                    status = CreateWindowStatus.WindowCreated;

                    ProvideReferenceFor(wnd);

                    return true;
                }
                else
                    MessageBoxExt.Show(UI.PresentationDesign.DesignUI.Properties.Resources.UnableCreateSource, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            status = CreateWindowStatus.InvalidDisplay;
            return false;
        }

        /// <summary>
        /// Развернуть текущее окно на весь экран.
        /// </summary>
        public void SetWindowFullSize()
        {
            if (CurrentLayout != null && !CurrentLayout.IsEmpty)
            {
                Display currentDisplay = CurrentLayout.Display;
                if (SelectedWindow == null) return;
                if (SelectedWindow.Window.Source is IAspectLock)
                {
                    ((IAspectLock)SelectedWindow.Window.Source).AspectLock = false;
                }

                SelectedWindow.SetBoundsInfo(new RectangleF(0, 0, currentDisplay.Width, currentDisplay.Height));
            }

        }

        public void Set1x1Size()
        {
            SetWindowKxKSize(1);
        }

        public void Set2x2Size()
        {
            SetWindowKxKSize(2);
        }

        public void Set3x3Size()
        {
            SetWindowKxKSize(3);
        }

        /// <summary>
        ///  Установить
        /// </summary>
        /// <param name="k"></param>
        private void SetWindowKxKSize(int k)
        {
            if (CurrentLayout != null && !CurrentLayout.IsEmpty)
            {
                Display currentDisplay = CurrentLayout.Display;
                if (SelectedWindow == null) return;
                if (SelectedWindow.Window.Source is ISourceSize)
                {
                    ((ISourceSize)SelectedWindow.Window.Source).AspectLock = false;
                }
                ISegmentationSupport segments = (currentDisplay as ISegmentationSupport);
                Point point = SelectedWindow.GetPosition();
                SelectedWindow.SetBoundsInfo(new RectangleF(point.X, point.Y, segments.SegmentWidth * k, segments.SegmentHeight * k));
            }
        }

        /// <summary>
        /// Проверяет, является ли многосегментным текущий дисплей.
        /// </summary>
        public bool IsSegmentedDisplay()
        {
            if (CurrentLayout != null && !CurrentLayout.IsEmpty)
            {
                Display currentDisplay = CurrentLayout.Display;
                if (currentDisplay is ISegmentationSupport)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Удаление окна
        /// </summary>
        public void RemoveWindow()
        {
            if (SelectionList.OfType<SourceWindow>().Count() > 0 && CurrentLayout != null && !CurrentLayout.IsEmpty)
            {
                if (MessageBoxExt.Show("Вы действительно хотите удалить окно?", Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }) == DialogResult.OK)
                {
                    internalChanging = true;
                    Model.HistoryManager.StartAtomicAction("Remove windows");
                    IEnumerable<SourceWindow> windows = SelectionList.OfType<SourceWindow>();
                    foreach (SourceWindow window in windows)
                    {
                        window.EditStyle.AllowDelete = true;
                        CurrentLayout.RemoveWindow(window.Window);
                    }

                    Delete();
                    Model.HistoryManager.EndAtomicAction();
                    internalChanging = false;
                    PresentationController.Instance.PresentationChanged = true;
                    PresentationController.Instance.SendOnSelectedResourceChanged(null);
                }
            }
        }

        #endregion

        #region preview

        SplashForm splash = null;
        ManualResetEvent loadSync = new ManualResetEvent(false);

        void Instance_OnProgressChanged(int arg1, int arg2, string displayName)
        {
            if (splash != null)
            {
                splash.Invoke(new MethodInvoker(() =>
                {
                    splash.Progress = (int)((float)arg2 / (float)arg1 * 100f);
                }));
            }
        }

        public Image GetPreview()
        {
            if (LoadAndShowSlide())
                return (new DisplayViewer(CurrentLayout.Display, false)).getSceenshot();
            return null;
            //loadSync.Reset();
            //if (ShowClient.Instance.Load(PresentationController.Instance.PresentationInfo, CurrentLayout.Slide.Id))
            //{
            //    SplashForm splash = SplashForm.CreateAndShowForm(true, true);
            //    splash.Status = "Получение снимка";
            //    splash.Progress = 0;
            //    splash.OnCancel += frm_OnCancel;
            //    loadSync.WaitOne();
            //    splash.OnCancel -= frm_OnCancel;
            //    splash.HideForm();
            //    return (new DisplayViewer(CurrentLayout.Display, false)).getSceenshot();
            //}

            //return null;
        }

        public bool IsShownByPlayer()
        {
            return ShowClient.Instance.IsShownByPlayer();
        }

        /// <summary>
        /// Подготовить презентацию.
        /// </summary>
        private bool Prepare()
        {
            PresentationInfo info = PresentationController.Instance.PresentationInfo;
            PreparePresentationController controller = new PreparePresentationController(new PresentationInfo(info), CurrentLayout.Slide.Id);
            PreparePresentationForm form = new PreparePresentationForm(controller);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
            bool prepared = controller.PreparationStatus != ShowClient.PreparationStatus.Error;
            return prepared;
        }

        public bool LoadAndShowSlide()
        {
            loadSync.Reset();
            if(Prepare())
            //if (ShowClient.Instance.Load(PresentationController.Instance.PresentationInfo, CurrentLayout.Slide.Id))
            {
                SplashForm splash = SplashForm.CreateAndShowForm(true, true);
                splash.Status = "Получение снимка";
                splash.Progress = 0;
                splash.OnCancel += frm_OnCancel;
                loadSync.WaitOne();
                splash.OnCancel -= frm_OnCancel;
                splash.HideForm();
                String error = String.Empty;
                String warning = string.Empty;
                if (ShowClient.PreparationStatus.Error == ShowClient.Instance.HasError(PresentationController.Instance.PresentationInfo, out error, out warning))
                {
                    MessageBoxExt.Show("Ошибка при подготовке сцены\r\n" + error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                bool isPrevEnable, isNextEnable;
                ShowClient.Instance.CloseWindows();
                return ShowClient.Instance.ShowSlide(PresentationController.Instance.Presentation, CurrentLayout.Slide,
                                                     out isPrevEnable, out isNextEnable).IsSuccess;
                //return true;
            }
            return false;
        }

        void Instance_OnNotEnoughSpace(DisplayType obj)
        {
            if (OnNotEnoughSpace != null)
                OnNotEnoughSpace(obj);
        }

        void Instance_OnShownStatusChanged(bool isShownStatusChange)
        {
            if (OnShownStatusChanged != null)
                OnShownStatusChanged(isShownStatusChange);
        }

        void frm_OnCancel(object sender, EventArgs e)
        {
            ShowClient.Instance.TerminateLoad(Domain.PresentationShow.ShowCommon.TerminateLoadCommand.StopAll, null);
            loadSync.Set();
        }


        void Instance_OnPreparationFinished()
        {
            // обработка ошибок перехала в LoadAndShowSlide
            //String error = String.Empty;
            //if (ShowClient.Instance.HasError(PresentationController.Instance.PresentationInfo, out error))
            //{
            //    MessageBoxExt.Show("Ошибка при подготовке сцены\r\n" + error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            loadSync.Set();
            ////TODO
            //bool isPrevEnable, isNextEnable;
            //ShowClient.Instance.ShowSlide(PresentationController.Instance.Presentation, CurrentLayout.Slide,
            //    out isPrevEnable, out isNextEnable);
        }


        #endregion

        #region background

        //ManualResetEvent resourceLoading = new ManualResetEvent(false);
        FileSaveStatus fileStatus = FileSaveStatus.Abort;
        private IClientResourceCRUD<ResourceDescriptor> clientResourceCRUD = null;
        public IClientResourceCRUD<ResourceDescriptor> ClientResourceCRUD
        {
            get
            {
                if (clientResourceCRUD == null)
                {
                    clientResourceCRUD = DesignerClient.Instance.PresentationWorker.GetResourceCrud();
                    clientResourceCRUD.OnPartTransmit += clientResourceCRUD_OnPartTransmit;
                    //clientResourceCRUD.OnTerminate += clientResourceCRUD_OnTerminate;
                    clientResourceCRUD.OnComplete += clientResourceCRUD_OnComplete;

                    //clientResourceCRUD = ClientSourceTransferFactory.CreateClientFileTransfer(
                    //    DesignerClient.Instance.IsStandAlone,
                    //    DesignerClient.Instance.PresentationWorker,
                    //    DesignerClient.Instance.SourceDAL);
                }
                return clientResourceCRUD;
            }
        }

        void clientResourceCRUD_OnPartTransmit(object sender, PartSendEventArgs e)
        {
            if (e.NumberOfParts <= 1) return;

            if (splash == null)
            {
                splash = SplashForm.CreateAndShowForm(true, true);
                splash.OnCancel += new EventHandler(splash_OnCancel);
            }

            splash.Progress = (int)(e.Part * 1f / e.NumberOfParts * 100f);
            Application.DoEvents();
        }

        void clientResourceCRUD_OnComplete(object sender, OperationStatusEventArgs<ResourceDescriptor> e)
        {
            CloseSplash();
            fileStatus = e.Status;
            //resourceLoading.Set();
        }

        //void clientResourceCRUD_OnTerminate(object sender, EventArgs e)
        //{
        //    CloseSplash();
        //}

        void CloseSplash()
        {
            if (splash != null && !splash.IsDisposed)
            {
                splash.Invoke(new MethodInvoker(() => splash.HideForm()));
                splash = null;
            }
        }

        void splash_OnCancel(object sender, EventArgs e)
        {
            if (ClientResourceCRUD != null)
                ClientResourceCRUD.Terminate();
        }

        Thread createSourceThread;

        private bool CreateBackgroundResource(BackgroundImageDescriptor descr)
        {
            //resourceLoading.Reset();
            fileStatus = FileSaveStatus.Abort;
            string otherResourceId;
            ClientResourceCRUD.CreateSource(descr, out otherResourceId);


            //createSourceThread = new Thread(new ThreadStart(new MethodInvoker(() =>
            //{
            //    string otherResourceId;
            //    ClientResourceCRUD.CreateSource(descr, out otherResourceId);
            //})));

            //createSourceThread.IsBackground = true;
            //createSourceThread.SetApartmentState(ApartmentState.STA);
            //createSourceThread.Start();

            //resourceLoading.WaitOne();
            return fileStatus == FileSaveStatus.Ok;
        }

        public void ShowBackgroundProps()
        {
            if (CurrentLayout.Display is ActiveDisplay)
            {
                Guid dsoGuid = new Guid("00460182-9E5E-11d5-B7C8-B8269041DD57");
                string keyName = @"CLSID\{" + dsoGuid + "}";
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(keyName))
                    if (key == null)
                    {
                        MessageBoxExt.Show(
                            "В системе не установлен DSO компонент необходимый для просмотра",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                string b_id = ((ActiveDisplay)(CurrentLayout.Display)).BackgroundImage;
                try
                {
                    using (PowerPointForm frm = new PowerPointForm(CurrentLayout.Display.Width, CurrentLayout.Display.Height))
                    {
                        if (String.IsNullOrEmpty(b_id) || backgrounds.ContainsKey(b_id) == false)
                            frm.CreateDocument();
                        else
                        {
                            // прежде чем брать путь - обязательно закачаем ресурс
                            //string fName = DesignerClient.Instance.PresentationWorker.SourceDAL.GetResourceFileName(backgrounds[b_id]);
                            string fName = GetFile(backgrounds[b_id]);
                            if (string.IsNullOrEmpty(fName))
                            {
                                MessageBoxExt.Show(
                                    "Фоновый файл не удалось закачать с сервера",
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            frm.AssignDocument(fName);
                        }

                        frm.ShowDialog();

                        if (frm.Changed)
                        {
                            DialogResult result = DialogResult.None;
                            if (!DesignerClient.Instance.IsStandAlone)
                            {
                                string userName = string.IsNullOrEmpty(PresentationController.Instance.UserIdentity.User.FullName) ?
                                    PresentationController.Instance.UserIdentity.User.Name :
                                    PresentationController.Instance.UserIdentity.User.FullName;
                                result = MessageBoxExt.Show
                                    (null, String.Format("Применить изменения фона для всех раскладок дисплея \"{0}\" на всех сценах сценария, заблокированных пользователем {1}?", CurrentLayout.Display.Type.Name, userName),
                                    Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }, false);
                            }
                            else
                            {
                                result = MessageBoxExt.Show(null, String.Format("Применить изменения фона для всех раскладок дисплея \"{0}\" на всех сценах сценария?", CurrentLayout.Display.Type.Name), Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }, false);
                            }

                            BackgroundImageDescriptor descr = new BackgroundImageDescriptor(frm.DocumentPath, m_presentation.UniqueName);
                            descr.IsLocal = true;

                            createSourceThread = new Thread(new ThreadStart(new MethodInvoker(() =>
                            {
                                if (CreateBackgroundResource(descr))
                                {
                                    string id = descr.ResourceInfo.Id;
                                    backgrounds.Add(id, descr);
                                    List<Display> selected = new List<Display>();
                                    if (PresentationController.Instance.SelectedDisplay != null)
                                        selected.Add(PresentationController.Instance.SelectedDisplay);
                                    else
                                        selected.AddRange(PresentationController.Instance.SelectedDisplayGroup);
                                    Size<int> sizeDisplayes = selected[0].Size;
                                    if (result != DialogResult.OK)
                                    {
                                        foreach (Display item in selected)
                                            CurrentLayout.ApplyDisplayBackground(id, item);
                                    }
                                    else
                                    {
                                        foreach (Slide slide in PresentationController.Instance.GetAllSlides())
                                            if (DesignerClient.Instance.IsStandAlone || (slide.IsLocked && PresentationController.Instance.CanUnlockSlide(slide)))
                                            {
                                                //SV: если блокировку проводили до выделения дисплея, то он попал только в последний текущий слайд
                                                //а нам надо добавить его во все заблокированнные
                                                foreach (Display item in selected)
                                                {
                                                    if (!slide.DisplayList.Contains(item))
                                                    {
                                                        SlideLayout sltemp = new SlideLayout(slide.DisplayList, slide);
                                                        sltemp.AppendToLayout(item, true);
                                                    }
                                                    //SV: End
                                                    SlideLayout.ApplyDisplayBackground(slide, id, item);
                                                }
                                            }
                                    }

                                    ((Control)this.Viewer).Invoke(new MethodInvoker(() => LoadBackground(CurrentLayout)));
                                    PresentationController.Instance.PresentationChanged = true;
                                    // удаляем временный файл
                                    //string tempPath = ((ResourceFileInfo) descr.ResourceInfo).ResourceFullFileName;
                                    //try
                                    //{
                                    //    File.Delete(tempPath);
                                    //}
                                    //catch{}
                                }
                            })));

                            createSourceThread.IsBackground = true;
                            createSourceThread.SetApartmentState(ApartmentState.STA);
                            createSourceThread.Start();

                            //if (CreateBackgroundResource(descr))
                            //{
                            //    string id = descr.ResourceInfo.Id;
                            //    backgrounds.Add(id, descr);
                            //    List<Display> selected = new List<Display>();
                            //    if (PresentationController.Instance.SelectedDisplay != null)
                            //        selected.Add(PresentationController.Instance.SelectedDisplay);
                            //    else
                            //        selected.AddRange(PresentationController.Instance.SelectedDisplayGroup);
                            //    Size<int> sizeDisplayes = selected[0].Size;
                            //    if (result != DialogResult.OK)
                            //    {
                            //        foreach (Display item in selected)
                            //            CurrentLayout.ApplyDisplayBackground(id, item);
                            //    }
                            //    else
                            //    {
                            //        foreach (Slide slide in PresentationController.Instance.GetAllSlides())
                            //            if (DesignerClient.Instance.IsStandAlone || (slide.IsLocked && PresentationController.Instance.CanUnlockSlide(slide)))
                            //            {
                            //                //SV: если блокировку проводили до выделения дисплея, то он попал только в последний текущий слайд
                            //                //а нам надо добавить его во все заблокированнные
                            //                foreach (Display item in selected)
                            //                {
                            //                    if (!slide.DisplayList.Contains(item))
                            //                    {
                            //                        SlideLayout sltemp = new SlideLayout(slide.DisplayList, slide);
                            //                        sltemp.AppendToLayout(item, true);
                            //                    }
                            //                    //SV: End
                            //                    SlideLayout.ApplyDisplayBackground(slide, id, item);
                            //                }
                            //            }
                            //    }

                            //    ((Control)this.Viewer).Invoke(new MethodInvoker(() =>
                            //    {
                            //        backProvider.SetBackgroundImage(frm.DocumentPath, this.View as IBackgroundSupport, sizeDisplayes.X, sizeDisplayes.Y);
                            //        ((Control)(this.Viewer)).Invalidate();
                            //    }));
                            //    PresentationController.Instance.PresentationChanged = true;
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    //if (ex is COMException || 
                    //    ((ex is TargetInvocationException) && (ex.InnerException is COMException)))
                    MessageBoxExt.Show(String.Format("Произошла ошибка при открытии формы. Возможно подложка была создана в другой версии PowerPoint{0}Детальная информация:{0}{1}", Environment.NewLine, ex.Message),
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBoxExt.Show("Тип выбранного дисплея не поддерживает редактирование подложки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            DesignerClient.Instance.PresentationNotifier.OnResourceAdded -= new EventHandler<NotifierEventArg<ResourceDescriptor>>(PresentationNotifier_OnResourceAdded);
            DesignerClient.Instance.PresentationNotifier.OnResourceDeleted -= new EventHandler<NotifierEventArg<ResourceDescriptor>>(PresentationNotifier_OnResourceDeleted);
            PresentationController.Instance.OnSlideLockChanged -= new SlideLockChanged(OnSlideLockChanged);
            PresentationController.Instance.OnSlideLayoutChanged -= new SlideLayoutChanged(Instance_OnSlideLayoutChanged);
            PresentationController.Instance.OnSavePresentation -= new SavePresentation(Instance_OnSavePresentation);
            UndoService.Instance.OnHistoryChanged -= new HistoryChanged(OnHistoryChanged);
            ShowClient.Instance.OnPreparationFinished -= Instance_OnPreparationFinished;
            ShowClient.Instance.OnProgressChanged -= Instance_OnProgressChanged;
            ShowClient.Instance.OnNotEnoughSpace -= Instance_OnNotEnoughSpace;
            ShowClient.Instance.OnShownStatusChanged -= Instance_OnShownStatusChanged;
            backProvider.Dispose();
            _instance = null;
        }

        #endregion

        #region Interaction

        public override void OnKeyDown(KeyEventArgs evtArgs)
        {
            base.OnKeyDown(evtArgs);

            if (CanEdit)
            {
                switch (evtArgs.KeyCode)
                {
                    case Keys.Add:
                        BringForward();
                        break;
                    case Keys.Subtract:
                        SendBackward();
                        break;
                }
            }

        }

        public override void OnDoubleClick(EventArgs evtArgs)
        {
            if (this.SelectedWindow != null && this.SelectedWindow.Window != null && this.SelectedWindow.Window.Source != null)
            {
                IDesignInteractionSupport interaction = this.SelectedWindow.Window.Source as IDesignInteractionSupport;
                if (interaction != null && interaction.SupportInteraction)
                {
                    interaction.InteractiveAction();
                    return;
                }
            }

            base.OnDoubleClick(evtArgs);
        }


        #endregion

        #region IDesignServiceProvider Members

        public object GetService(Type type)
        {
            if (type == typeof(Slide))
                return PresentationController.Instance.SelectedSlide;

            if (type == typeof(IClientResourceCRUD<ResourceDescriptor>))
                return ClientResourceCRUD;

            if (type == typeof(IPresentationNotifier))
                return DesignerClient.Instance.PresentationNotifier;

            if (type == typeof(IPresentationClient))
                return DesignerClient.Instance.PresentationWorker;

            if (type == typeof(IConfiguration))
                return DesignerClient.Instance.ClientConfiguration;

            return null;
        }

        public void InvalidateView()
        {
            ((Control)this.Viewer).Invalidate();
        }

        public bool IsActive()
        {
            return true;
        }

        #endregion
    }
}
