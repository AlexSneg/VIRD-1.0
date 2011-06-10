using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;
using System.Threading;
using System;

using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Controllers;
using UI.PresentationDesign.DesignUI.Services;
using Domain.PresentationDesign.Client;

using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Syncfusion.Runtime.Serialization;
using TechnicalServices.Entity;
using UI.PresentationDesign.DesignUI.Classes.Helpers;

namespace UI.PresentationDesign.DesignUI
{
    /// <summary>
    /// Экранная форма проигрывателя
    /// </summary>
    public partial class PlayerForm : RibbonForm
    {
        const String Title = "ВИРД - Показ сценариев: {0}";
        Presentation m_Presentation;
        PresentationInfo m_PresentationInfo;

        public PlayerForm()
        {
            InitializeComponent();
        }       

        public PlayerForm(PresentationInfo APresentationInfo)
        {
            DisplayController.IsPlayerMode = true;
            m_Presentation = APresentationInfo.CreatePresentationStub();
            m_PresentationInfo = APresentationInfo;

            UndoService.CreateUndoService();
            PresentationController.CreatePresentationController();
            PlayerController.CreatePlayerController();
            PresentationController.Instance.AssignPresentation(m_Presentation, m_PresentationInfo);
            PlayerSourcesController.CreateController();
            PlayerEquipmentController.CreateController();
            SourceCommandListController.CreateSourceCommandListController();
            DeviceCommandListController.CreateController();
            InitializeComponent();
            this.Text = String.Format(Title, m_Presentation.Name);
            this.WindowState = FormWindowState.Maximized;

            MonitoringController.CreateController();
            this.displayMonitorControl1.AssingController(MonitoringController.Instance);
            this.slideDiagramControl.SwitchPlayerMode(true);

            PresentationController.Instance.PresentationLocked = true;
            SlideGraphController.Instance.AssignPlayerController(PlayerController.Instance);

            SlideGraphController.Instance.OnSlideHover += new EventHandler<SlideEventArgs>(Instance_OnSlideHover);

            PresentationController.Instance.OnMonitorListChanged += new MonitorListChanged(Instance_OnMonitorListChanged);
            slideDiagramControl.RefreshRequest += new EventHandler(slideDiagramControl_RefreshRequest);
        }

        void Instance_OnMonitorListChanged(System.Collections.Generic.IEnumerable<Display> newList)
        {
            UpdateRefreshScreenshotButtonVisibility();
        }

        private void UpdateRefreshScreenshotButtonVisibility()
        {
            slideDiagramControl.SetRefreshScreenshotsButtonVisibility(true, MonitoringController.Instance.HasMonitoredWindow);
        }

        void slideDiagramControl_RefreshRequest(object sender, EventArgs e)
        {
            MonitoringController.Instance.RefreshScreenShots();
        }

        void Instance_OnSlideHover(object sender, SlideEventArgs e)
        {
            if (e.Slide != null)
            {
                LockingInfo li = PresentationController.Instance.GetSlideLockingInfo(e.Slide);
                if (li != null)
                {
                    SlideLockingStatus.Text = PresentationStatusInfo.GetSlideLockingInfoDescr(li);
                    SlideLockingStatus.Visible = true;
                    return;
                }
            }
            else
                SlideLockingStatus.Visible = false;
        }

        private void closeMenuButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public void InitPresentation()
        {
            DisplayController.Instance.EnableCheckboxes(true);

            PlayerSourcesController.Instance.Initialize();
            playerSourcesControl1.AssignController(PlayerSourcesController.Instance);
            equipmentControl.AssignController(PlayerEquipmentController.Instance);

            this.SourceManagementControl.AssignController(SourceCommandListController.Instance);
            this.equipmentCommandControl.AssignController(DeviceCommandListController.Instance);

            PresentationController.Instance.PresentationLocked = true;
            SlideGraphController.Instance.GoHome(true);
        }

        private bool reflectOnMenuCheck = true;

        private void displaysMenuButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if(!reflectOnMenuCheck)
                return;
            this.dockingManager.SetDockVisibility(this.displayListControl, this.displaysMenuButton.Checked);
            this.dockingManager.SetDockVisibility(this.playerSourcesControl1, this.sourcesMenuButton.Checked);
            this.dockingManager.SetDockVisibility(this.SourceManagementControl, this.sourcesControlMenuButton.Checked);
            this.dockingManager.SetDockVisibility(this.equipmentControl, this.equipmentMenuButton.Checked);
            this.dockingManager.SetDockVisibility(this.equipmentCommandControl, this.equipmentControlMenuButton.Checked);
        }

        private void setMenuChecks()
        {
            reflectOnMenuCheck = false;
            this.displaysMenuButton.Checked = this.dockingManager.GetDockVisibility(this.displayListControl);
            this.sourcesMenuButton.Checked = this.dockingManager.GetDockVisibility(this.playerSourcesControl1);
            this.sourcesControlMenuButton.Checked = this.dockingManager.GetDockVisibility(this.SourceManagementControl);
            this.equipmentMenuButton.Checked = this.dockingManager.GetDockVisibility(this.equipmentControl);
            this.equipmentControlMenuButton.Checked = this.dockingManager.GetDockVisibility(this.equipmentCommandControl);
            reflectOnMenuCheck = true;
        }

        private void cascadeMenuButton_Click(object sender, EventArgs e)
        {
            this.displayMonitorControl1.ArrangeCascade();
        }

        private void arrange2x2MenuButton_Click(object sender, EventArgs e)
        {
            this.displayMonitorControl1.Arrange2x2();
        }

        private void arrange3x3MenuButton_Click(object sender, EventArgs e)
        {
            this.displayMonitorControl1.Arrange3x3();
        }

        private void arrange4x4MenuButton_Click(object sender, EventArgs e)
        {
            this.displayMonitorControl1.Arrange4x4();
        }

        private void defaultSizeMenuButton_Click(object sender, EventArgs e)
        {
            this.displayMonitorControl1.ArrangeDefault();
        }

        private void closeAllMenuButton_Click(object sender, EventArgs e)
        {
            this.displayMonitorControl1.CloseAllWindows();
        }

        private void PlayerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.displayMonitorControl1.SavePositions();
            MonitoringController.Instance.SavePositions(Application.StartupPath + "\\monitoring.xml");

            SlideGraphController.Instance.Dispose();
            DisplayController.Instance.Dispose();
            PresentationController.Instance.Dispose();

            PlayerController.Instance.Stop(this.m_PresentationInfo);
            PlayerController.Instance.Dispose();
            MonitoringController.Instance.Dispose();
            PlayerSourcesController.Instance.Dispose();
            PlayerEquipmentController.Instance.Dispose();
            SourceCommandListController.Instance.Dispose();
            DeviceCommandListController.Instance.Dispose();
            
            AppStateSerializer serializer = new AppStateSerializer(SerializeMode.XMLFile, Application.StartupPath + "\\dockstate.xml");
            this.dockingManager.SaveDockState(serializer);
            serializer.PersistNow();
        }

        private void PlayerForm_Load(object sender, EventArgs e)
        {
            AppStateSerializer serializer = new AppStateSerializer(SerializeMode.XMLFile, Application.StartupPath + "\\dockstate.xml");
            this.dockingManager.LoadDockState(serializer);
            setMenuChecks();
            MonitoringController.Instance.LoadPositions(Application.StartupPath + "\\monitoring.xml");
        }

        private void windowsMenuButton_Click(object sender, EventArgs e)
        {

        }
    }
}
