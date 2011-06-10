using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using Syncfusion.Windows.Forms.Diagram;
using UI.PresentationDesign.DesignUI.Classes.View;
using Presentation = TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Diagram.Controls;
using UI.PresentationDesign.DesignUI.Controls;
using UI.PresentationDesign.DesignUI.Controls.Utils;
using TechnicalServices.Entity;
using Domain.PresentationDesign.Client;
using UI.PresentationDesign.DesignUI.Controllers;
using System.IO;

namespace UI.PresentationDesign.DesignUI
{
    public partial class SlideDiagramControl : UserControl
    {
        #region Fields and Properties
        private ContextMenuStrip activeMenu;
        SlideGraphController m_controller;
        PresentationSelectionTool selectTool;
        SlideCreationTool creationTool;
        SlideLinkTool linkTool;
        PresentationPanTool panTool;
        ZoomTool zoomTool;

        private bool isPaused = false;

        private bool isPlayerMode = false;

        /// <summary>
        /// Ассоциированная диаграмма сценария
        /// </summary>
        public PresentationDiagram Diagram
        {
            get
            {
                return this.diagram;
            }
        }


        /// <summary>
        /// Ассоциированная модель диаграммы
        /// </summary>
        public Syncfusion.Windows.Forms.Diagram.Model Model
        {
            get { return model; }
        }

        /// <summary>
        /// Ассоциированный вид сценария
        /// </summary>
        public PresentationView PresentationView
        {
            get
            {
                return diagram.View as PresentationView;
            }
        }


        bool _isEnabled = true;

        public bool IsSlideLocked
        {
            get
            {
                return m_controller.IsSelectedSlideLocked();
            }
        }

        public int SelectedSlideCount
        {
            get
            {
                return m_controller.SelectionList.OfType<SlideView>().Count();
            }
        }

        public bool CanPaste
        {
            get
            {
                return (_isEnabled || DesignerClient.Instance.IsStandAlone) & m_controller.CanPaste;
            }
        }

        public bool CanCopy
        {
            get
            {
                return m_controller.CanCopy;
            }
        }

        public bool CanRemove
        {
            get
            {
                return (_isEnabled || DesignerClient.Instance.IsStandAlone) && m_controller.SelectionList.Count > 0;
            }
        }

        #endregion

        #region ctor

        public SlideDiagramControl()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.diagram.AllowNodesDrop = false;
                propertiesStripButton.Enabled = false;
                this.zoomCombo.ConnectToView(this.diagram.View);
                this.zoomCombo.Text="100%";
                pasteStripButton.Enabled = false;

                unlockMenuItem.Visible = !DesignerClient.Instance.IsStandAlone;
                lockMenuItem.Visible = !DesignerClient.Instance.IsStandAlone;
                
                AssignController(SlideGraphController.CreateSlideGraphController());
                m_controller.InitController();

                //refreshStripButton.Visible = !DesignerClient.Instance.IsStandAlone;
            }
        }
               
        #endregion

        #region Initialization
        public void AssignController(SlideGraphController controller)
        {
            diagram.Controller = m_controller = controller;
            if (m_controller == null)
                throw new ApplicationException("Unknown type");

            //register tools for controller
            linkTool = new SlideLinkTool(m_controller);
            linkTool.OnToolDeactivate += new ToolDeactivate(linkTool_OnToolDeactivate);
            m_controller.RegisterTool(linkTool);
            Tool tmptool = m_controller.GetTool(ToolDescriptor.SelectTool);
            m_controller.UnRegisterTool(tmptool);

            selectTool = new PresentationSelectionTool(m_controller);
            selectTool.OnShowSlideContextMenu += new ShowSelectionContextMenu(selectTool_OnShowNodeContextMenu);
            selectTool.OnShowModelContextMenu += new ShowSelectionContextMenu(selectTool_OnShowModelContextMenu);
            m_controller.RegisterTool(selectTool);

            creationTool = new SlideCreationTool(m_controller, diagram);
            creationTool.ToolCursor = creationTool.ActionCursor = new Cursor(new MemoryStream(Properties.Resources.createSlide));
            creationTool.OnToolDeactivate += new ToolDeactivate(creationTool_OnToolDeactivate);
            m_controller.RegisterTool(creationTool);

            tmptool = m_controller.GetTool(ToolDescriptor.PanTool);
            m_controller.UnRegisterTool(tmptool);

            panTool = new PresentationPanTool(m_controller);
            m_controller.RegisterTool(panTool);

            panTool.OnToolDeactivate += new ToolDeactivate(panTool_OnToolDeactivate);

            zoomTool = (ZoomTool)m_controller.GetTool(ToolDescriptor.ZoomTool);

            PresentationController.Instance.OnSlideLockChanged += new SlideLockChanged(m_controller_OnSlideLockChanged);
            PresentationController.Instance.OnPresentationLockChanged += new PresentationLockChanged(m_controller_OnPresentationLockChanged);
            m_controller.OnCheckSelection += new ControllerCheckSelection(CheckSelection);
            m_controller.OnCurrentSlideChanged += new CurrentSlideChanged(m_controller_OnCurrentSlideChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                if (this.m_controller != null)
                    this.m_controller.CheckSelection();
            }
        }

        private void SlideDiagramControl_Load(object sender, EventArgs e)
        {
            applyButtonStates();
        }

        #endregion

        #region Lock handlers
        void m_controller_OnSlideLockChanged(Slide slide, bool IsLocked, LockingInfo info)
        {
            if (slide != null)
            {
                if (m_controller.IsSlideSelected(slide))
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(new MethodInvoker(() =>
                            {
                                lockStripButton.Enabled = !IsLocked;
                                unlockStripButton.Enabled = IsLocked;
                                //removeStripButton.Enabled = !IsLocked;
                            }));
                    }
                }
            }
            else
            {
                this.Invoke(new MethodInvoker(() =>
                        {
                            lockStripButton.Enabled = false;
                            unlockStripButton.Enabled = false;
                            //removeStripButton.Enabled = false;
                        }));
            }
            SetLockButtonsState(slide);
        }

        /// <summary>
        /// Установить состояние кнопок блокировки всех сцен.
        /// </summary>
        private void SetLockButtonsState(Slide slide)
        {


            if (this.IsHandleCreated)
            {
                if (slide == null)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        unlockAllStripButton.Enabled = m_controller.IsAnySlidesLocked(slide);
                        lockAllStripButton.Enabled = m_controller.IsAnySlidesUnlocked(slide);
                    }));
                    return;
                }

                this.Invoke(new MethodInvoker(() =>
                {
                    if (slide.IsLocked)
                    {
                        unlockAllStripButton.Enabled = m_controller.IsAnySlidesLocked(slide);
                        lockAllStripButton.Enabled = true;
                    }
                    else
                    {
                        unlockAllStripButton.Enabled = true;
                        lockAllStripButton.Enabled = m_controller.IsAnySlidesUnlocked(slide);
                    }
                }));
            }
        }

        void m_controller_OnPresentationLockChanged(bool IsLocked)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new MethodInvoker(() => SetEditMode(IsLocked)));
            }
            else
                SetEditMode(IsLocked);
        }

        private void SetEditMode(bool IsLocked)
        {
            _isEnabled = IsLocked;
            Diagram.ReadOnly = !_isEnabled;
            this.createSlideButton.Enabled = _isEnabled;
            this.linkSlideButton.Enabled = _isEnabled;

            bool slideSelected = m_controller.SelectionList.OfType<SlideView>().Count() > 0;

            this.copyStripButton.Enabled = _isEnabled && slideSelected;
            this.pasteStripButton.Enabled = CanPaste;
        }
        #endregion

        #region Tool handlers

        void creationTool_OnToolDeactivate()
        {
            createSlideButton.Checked = false;
        }

        void linkTool_OnToolDeactivate()
        {
            linkSlideButton.Checked = false;
            slideLinkToolMenuItem.Checked = false;
        }

        void panTool_OnToolDeactivate()
        {
            panButton.Checked = false;
        }

        #endregion

        #region Commands

        public void SwitchPlayerMode(bool playerMode)
        {
            isPlayerMode = playerMode;
            foreach (ToolStripItem i in this.diagramToolStrip.Items)
            {
                if (i.Tag == null)
                    continue;
                if (i.Tag.ToString() == "designer")
                    i.Visible = !isPlayerMode;
                if (i.Tag.ToString() == "player")
                    i.Visible = isPlayerMode;
            }

            if (isPlayerMode)
            {
                playStripButton.Visible = false;
                nextSlideButton.Text = "Следующая сцена";
                prevSlideButton.Text = "Предыдущая сцена";
            }

            selectTool.AllowMultiSelect = !isPlayerMode;

            lockStripButton.Visible = !DesignerClient.Instance.IsStandAlone && !isPlayerMode;
            unlockStripButton.Visible = !DesignerClient.Instance.IsStandAlone && !isPlayerMode;
        }

        public void CreateSlideToolActivate()
        {
            if (m_controller.ActiveTool != creationTool)
            {
                createSlideButton.Checked = true;
                m_controller.ActivateTool(creationTool);
            }
            else
                m_controller.ActivateTool(selectTool);
        }

        public void LinkToolActivate()
        {
            if (m_controller.ActiveTool != linkTool)
            {
                linkSlideButton.Checked = true;
                slideLinkToolMenuItem.Checked = true;
                m_controller.ActivateTool(linkTool);
            }
            else
                m_controller.ActivateTool(selectTool);
        }


        public void PanToolActivate()
        {
            if (m_controller.ActiveTool != panTool)
            {
                panButton.Checked = true;
                m_controller.ActivateTool(panTool);
            }
            else
                m_controller.ActivateTool(selectTool);
        }

        public void SelectAll()
        {
            m_controller.SelectAll();
        }

        public void Copy()
        {
            m_controller.Copy();
            this.pasteStripButton.Enabled = this.CanPaste;
        }

        public void Paste()
        {
            m_controller.Paste();
        }


        public void RemoveSelected()
        {
            m_controller.RemoveSelected();
        }

        public void Undo()
        {
            m_controller.Undo();
        }

        public void Redo()
        {
            m_controller.Redo();
        }

        public void GoPrev()
        {
            m_controller.GoPrevSlide();
        }

        public void GoNext()
        {
            m_controller.GoNextSlide();
        }

        public void LockSlide()
        {
            m_controller.LockSlides();
        }

        public void UnLockSlide()
        {
            m_controller.UnlockSlides();
        }

        public void ShowSlideProperties()
        {
            if (m_controller.SelectedSlide != null)
                using (SlidePropertiesForm pf = new SlidePropertiesForm(m_controller))
                    pf.ShowDialog();
        }

        public void SlideDoubleClick()
        {
            if (isPlayerMode)
            {
                if (this.playStripButton.Enabled && this.diagram.AllowSelect)
                    this.playStripButton_Click(null, EventArgs.Empty);
            }
            else
                ShowSlideProperties();
        }

        public void GoFirst()
        {
            m_controller.GoHome(true);
        }

        public void GoToSlide(int slideId)
        {
            m_controller.GoToSlide(slideId);
        }

        public void GoLast()
        {
            m_controller.GoLastSlide();
        }

        #endregion

        #region User action handlers

        public List<Slide> GetSelectedSlides()
        {
            var s = m_controller.SelectionList.OfType<SlideView>();
            if (s.Count() > 0)
                return s.Select(v => v.Slide).ToList();

            return new List<Slide>();
        }

        void m_controller_OnCurrentSlideChanged(Slide slide)
        {
            prevSlideButton.Enabled = m_controller.CanSelectPrevSlide(slide);
            nextSlideButton.Enabled = m_controller.CanSelectNextSlide(slide);
        }

        public void CheckSelection(NodeCollection nodes)
        {
            var slideNodes = nodes.OfType<SlideView>();
            List<Slide> slides = new List<Slide>();

            if (slideNodes.Count() > 0)
                slides = slideNodes.Select(s => s.Slide).ToList();

            goLastSlideButton.Enabled = m_controller.CanSelectLastSlide();

            if (slides.Count > 0)
            {
                prevSlideButton.Enabled = PresentationController.Instance.CanSelectPrevSlide;
                nextSlideButton.Enabled = PresentationController.Instance.CanSelectNextSlide;

                propertiesStripButton.Enabled = slides.Count() == 1;

                var linked_slides = m_controller.GetLinksFromCurrent();
                nextSlideButton.DropDown.Items.Clear();
                if (linked_slides.Count > 1)
                {
                    //show dropdown
                    nextSlideButton.DropDownButtonWidth = 10;
                    foreach (var slide in linked_slides)
                    {
                        ToolStripMenuItem mi = new ToolStripMenuItem(slide.SlideName);
                        mi.Tag = slide;
                        mi.Click += new EventHandler(mi_Click);
                        nextSlideButton.DropDown.Items.Add(mi);
                    }
                }
                else
                {
                    //hide dropdown
                    nextSlideButton.DropDownButtonWidth = 0;
                }
            }
            else
            {
                propertiesStripButton.Enabled = false;
                propertiesMenuItem.Enabled = false;
                prevSlideButton.Enabled = nextSlideButton.Enabled = false;
            }

            lockStripButton.Enabled = PresentationController.Instance.IsLockGUICommandEnabled(slides);
            unlockStripButton.Enabled = PresentationController.Instance.IsUnlockGUICommandEnabled(slides);

            if (_isEnabled)
            {
                bool locked = PresentationController.Instance.IsSlidesLocked(slides);
                removeStripButton.Enabled = copyStripButton.Enabled = (locked && PresentationController.Instance.CanUnlockSlides(slides)) || !locked;
                pasteStripButton.Enabled = this.CanPaste;
            }
            else
            {
                removeStripButton.Enabled = false;
                copyStripButton.Enabled = pasteStripButton.Enabled = false;
            }
            SetLockButtonsState(null);            
        }

        private void createSlideButton_DoubleClick(object sender, EventArgs e)
        {
            m_controller.CreateSlide();
        }

        private void createSlideButton_Click(object sender, EventArgs e)
        {
            this.CreateSlideToolActivate();
        }

        private void linkSlideButton_Click(object sender, EventArgs e)
        {
            this.LinkToolActivate();
        }

        private void addNewSlideMenuItem_Click(object sender, EventArgs e)
        {
            Point p = diagram.PointToClient(((ToolStripDropDown)diagramContextMenuStrip).Location);
            p = m_controller.ConvertToModelCoordinates(p);
            m_controller.CreateSlide(p);
        }

        private void slideLinkToolMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<SlideView> slides = m_controller.SelectionList.OfType<SlideView>();
            if (slides.Count() == 2)
            {
                m_controller.CreateLink(slides);
            }
            else
            {
                this.LinkToolActivate();
            }
        }

        private void panButton_Click(object sender, EventArgs e)
        {
            this.PanToolActivate();
        }

        private void selectAllMenuItem_Click(object sender, EventArgs e)
        {
            this.SelectAll();
        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            this.Copy();
        }

        private void pasteMenuItem_Click(object sender, EventArgs e)
        {
            Point p = diagram.PointToClient(((ToolStripDropDown)diagramContextMenuStrip).Location);
            p = m_controller.ConvertToModelCoordinates(p);
            m_controller.Paste(p);
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            this.RemoveSelected();
        }

        //private void undoMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.Undo();
        //}

        //private void redoMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.Redo();
        //}

        private void defaultLinkMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<SlideView> views = m_controller.SelectionList.OfType<SlideView>();
            if (views.Count() > 0)
            {
                SlideView v = views.First();
                m_controller.MakeDefaultLinksFor(v);
            }
        }

        private void prevSlideButton_Click(object sender, EventArgs e)
        {
            this.GoPrev();
            this.applyButtonStates();
        }

        private void nextSlideButton_Click(object sender, EventArgs e)
        {
            this.GoNext();
            this.applyButtonStates();
        }


        private void editMenuItem_Click(object sender, EventArgs e)
        {
            this.LockSlide();
        }

        private void saveSlideMenuItem_Click(object sender, EventArgs e)
        {
            this.UnLockSlide();
        }

        private void propertiesMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowSlideProperties();
        }

        private void selectToolButton_Click(object sender, EventArgs e)
        {
            m_controller.ActivateTool(selectTool);
        }

        private void goFirstSlideButton_Click(object sender, EventArgs e)
        {
            this.GoFirst();
        }

        private void goLastSlideButton_Click(object sender, EventArgs e)
        {
            this.GoLast();
        }


        private void diagram_ScrollTipFeedback(object sender, Syncfusion.Windows.Forms.ScrollTipFeedbackEventArgs e)
        {
            //nop
        }

        private void pasteStripButton_Click(object sender, EventArgs e)
        {
            this.Paste();
        }

        private void diagram_KeyUp(object sender, KeyEventArgs e)
        {
            //nop
        }
        #endregion

        #region Context menus

        void mi_Click(object sender, EventArgs e)
        {
            m_controller.SelectSlideViewFromControl((SlideView)((ToolStripMenuItem)sender).Tag);
        }

        void selectTool_OnShowModelContextMenu(Point p)
        {
            if (isPlayerMode)
                return;
            pasteMenuItem.Enabled = CanPaste;

            //EnableUndoAction(m_controller.CanHistoryUndo() && _isEnabled);
            //EnableRedoActon(m_controller.CanHistoryRedo() && _isEnabled);

            slideLinkToolMenuItem.Enabled = m_controller.SelectionList.OfType<SlideView>().Count() <= 2 && _isEnabled;
            addNewSlideMenuItem.Enabled = _isEnabled;
            (activeMenu = diagramContextMenuStrip).Show(this.diagram, p);
        }

        void selectTool_OnShowNodeContextMenu(Point p)
        {
            if (isPlayerMode)
            {
                this.demoToolStripMenuItem.Enabled = m_controller.CanPlaySlide(this.GetSelectedSlides().FirstOrDefault());
                this.editToolStripMenuItem.Enabled = !m_controller.IsAnySlidesLocked(new Slide[] {this.GetSelectedSlides().FirstOrDefault()});
                this.playerNodeContextMenuStrip.Show(this.diagram, p);
                return;
            }

            //bool enable = m_controller.SelectionList.Count > 0 && (_isEnabled || DesignerClient.Instance.IsStandAlone);
            //copyMenuItem.Enabled = enable;

            IEnumerable<SlideView> selectedSlides = m_controller.SelectionList.OfType<SlideView>();
            IEnumerable<SlideLink> selectedLinks = m_controller.SelectionList.OfType<SlideLink>();
            var slides = selectedSlides.Select(s => s.Slide);

            if (selectedSlides.Count() > 0 && _isEnabled)
            {
                //linkslideItemsSeparator.Visible = true;
                defaultLinkMenuItem.Enabled = true;
                defaultLinkMenuItem.Enabled = ((selectedSlides.First() as SlideView).GetIncomingSlideLinks().Any(l => !l.IsDefault));
                bool locked = PresentationController.Instance.IsSlidesLocked(slides);
                deleteMenuItem.Enabled = copyMenuItem.Enabled = (locked && PresentationController.Instance.CanUnlockSlides(slides)) || (!locked);
            }
            else
            {
                defaultLinkMenuItem.Enabled = false;
                //linkslideItemsSeparator.Visible = false;
                copyMenuItem.Enabled = false;
                deleteMenuItem.Enabled = false;
            }

            if (selectedSlides.Count() > 0)
            {
                lockMenuItem.Enabled  = PresentationController.Instance.IsLockGUICommandEnabled(slides);
                unlockMenuItem.Enabled = PresentationController.Instance.IsUnlockGUICommandEnabled(slides);
            }
            else
            {
                lockMenuItem.Enabled = false;
                unlockMenuItem.Enabled = false;

                //lockMenuItem.Visible = selectedLinks.Count() == 0;
                //unlockMenuItem.Visible = selectedLinks.Count() == 0;
            }

            propertiesMenuItem.Enabled = selectedSlides.Count() == 1;

            if (_isEnabled)
            {
                //if (m_controller.SelectionList.Count > 0)
                //{
                //    deleteMenuItem.Enabled = true;
                //}
                //else
                //    deleteMenuItem.Enabled = false;
            }
            else
            {
                deleteMenuItem.Enabled = false;
                copyMenuItem.Enabled = false;
                pasteMenuItem.Enabled = false;
            }

            (activeMenu = nodeContextMenuStrip).Show(this.diagram, p);
            diagram.Refresh();
        }


        #endregion

        #region Player

        private void applyButtonStates()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime && m_controller != null)
            {
                this.prepareStripButton.Enabled = m_controller.CanPrepare;
                this.showStripButton.Enabled = m_controller.CanStart;
                this.playStripButton.Enabled = this.pauseStripButton.Enabled = m_controller.CanPlay;
                this.demoToolStripMenuItem.Enabled = m_controller.CanPlay;
                if (this.isPlayerMode && m_controller.CanPlay)
                {
                    this.prevSlideButton.Enabled = m_controller.CanSelectPrevSlide(null);
                    this.nextSlideButton.Enabled = m_controller.CanSelectNextSlide(null);
                }
                else
                {
                    this.prevSlideButton.Enabled = PresentationController.Instance.CanSelectPrevSlide;
                    this.nextSlideButton.Enabled = PresentationController.Instance.CanSelectNextSlide;
                }
            }
        }

        private void prepareStripButton_Click(object sender, EventArgs e)
        {
            this.m_controller.PreparePresentation();
            applyButtonStates();
        }

        private void showStripButton_Click(object sender, EventArgs e)
        {
            this.m_controller.StartPresentation();
            applyButtonStates();
        }

        private void pauseStripButton_Click(object sender, EventArgs e)
        {
            this.m_controller.PausePresentation();
            isPaused = true;
            pauseStripButton.Visible = false;
            playStripButton.Visible = true;
            applyButtonStates();
            this.nextSlideButton.Enabled = this.prevSlideButton.Enabled = false;
            this.diagram.AllowSelect = false;
        }

        private void playStripButton_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;
            isPaused = false;
            pauseStripButton.Visible = true;
            playStripButton.Visible = false;

            this.m_controller.PlaySlide();
            this.Cursor = Cursors.Default;
            this.UseWaitCursor = false;
            applyButtonStates();

            //this.nextSlideButton.Enabled = this.prevSlideButton.Enabled = this.diagram.AllowSelect = true;
            this.diagram.AllowSelect = true;
        }

        private void refreshStripButton_Click(object sender, EventArgs e)
        {
            applyButtonStates();
        }

        private void demoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_controller.PlaySlide();
            applyButtonStates();
        }

        private void propsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowSlideProperties();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_controller.EditSlide();
        }
        #endregion

        #region search
        private void searchStripButton_Click(object sender, EventArgs e)
        {
            FindItemController.Instance.ShowSearchForm(ItemToSearch.Slide);
        }
        #endregion

        public event EventHandler RefreshRequest;
 
        private void refreshScreenshotsButton_Click(object sender, EventArgs e)
        {
            if (RefreshRequest != null) RefreshRequest(this, null);
        }

        /// <summary>
        /// Установить видимость и статус кнопки обновления окон.
        /// </summary>
        /// <param name="visible">Видимость кнопки.</param>
        /// <param name="enabled">Признак доступности кнопки.</param>
        public void SetRefreshScreenshotsButtonVisibility(bool visible, bool enabled)
        {
            refreshScreenshotsButton.Visible = visible;
            refreshScreenshotsButton.Enabled = enabled;
        }

        private void lockAllStripButton_Click(object sender, EventArgs e)
        {
            SetLockForAllSlides(true);
        }

        private void unlockAllStripButton_Click(object sender, EventArgs e)
        {
            SetLockForAllSlides(false);
        }

        /// <summary>
        /// Установить блокировку для всех сцен.
        /// </summary>
        /// <param name="locked">Устанавливаемое состояние блокировки.</param>
        public void SetLockForAllSlides(bool locked)
        {
            this.SuspendLayout();
            // Список выделенных сцен для восстановления выделения
            SlideView[] selected = m_controller.SelectionList.OfType<SlideView>().ToArray();
            m_controller.SelectAll();
            if (locked) this.LockSlide();
            else this.UnLockSlide();
            m_controller.SelectionList.Clear();
            m_controller.SelectionList.AddRange(selected);  // Восстанавливаем сохраненное
            this.ResumeLayout();
        }
    }
}

