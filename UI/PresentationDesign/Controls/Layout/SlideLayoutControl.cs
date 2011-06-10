using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

using Syncfusion.Windows.Forms.Diagram;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Domain.PresentationDesign.Client;
using UI.PresentationDesign.DesignUI.Controllers;
using UI.PresentationDesign.DesignUI.Forms;
using Syncfusion.Windows.Forms;
using UI.PresentationDesign.DesignUI.Controls.Utils;
using UI.PresentationDesign.DesignUI.Helpers;
using Domain.PresentationShow.ShowClient;

namespace UI.PresentationDesign.DesignUI.Controls
{
    public partial class SlideLayoutControl : UserControl
    {
        #region fields and properties
        LayoutController layoutController;
        PresentationSelectionTool selectTool;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReadOnly
        {
            get
            {
                return slideLayoutDiagram.ReadOnly;
            }
            set
            {
                if (this.IsHandleCreated)
                    this.Invoke(new MethodInvoker(() =>
                    {
                        SetReadOnly(value);
                    }));
                else
                {
                    SetReadOnly(value);
                }
            }
        }

        void SetReadOnly(bool value)
        {
            slideLayoutDiagram.ReadOnly = value;
            try
            {
                slideLayoutDiagram.AllowDrop = !value;
                //needLockLabel.Visible = value && !DesignerClient.Instance.IsStandAlone;
            }
            catch
            {
                //hack the invalid dragdrop...
                slideLayoutDiagram.AllowDrop = !value;
            }
            //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1240
            //slideLayoutDiagram.AllowSelect = !value;

            //if (value)
            //    PresentationController.Instance.SendOnSelectedResourceChanged(null);

            // Кнопка редактирования подложки
            this.backgroundPropsButton.Enabled = !value;
        }

        #endregion

        #region ctor, factories, initializers
        public SlideLayoutControl()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                model.BoundaryConstraintsEnabled = false;
                layoutController = this.slideLayoutDiagram.Controller as LayoutController;
                zoomCombo.ConnectToView(this.slideLayoutDiagram.View);
                layoutController.AssignView(this);

                layoutController.OnNotEnoughSpace += new Action<DisplayType>(layoutController_OnNotEnoughSpace);
                selectTool = (PresentationSelectionTool)PresentationSelectionTool.GetInstance(layoutController);
                selectTool.OnShowSlideContextMenu += new ShowSelectionContextMenu(selectTool_OnShowSlideContextMenu);

                AssignController(PresentationController.Instance);
                layoutController.InitLayoutController();
                EnableEdit(DesignerClient.Instance.IsStandAlone);

                PresentationPanTool tool = (PresentationPanTool)layoutController.GetTool(ToolDescriptor.PanTool);
                tool.OnToolDeactivate += new ToolDeactivate(tool_OnToolDeactivate);

                slideLayoutDiagram.MinimumSize = new Size(200, 150);

            }
        }

        bool resizeUpdated = false;
        protected override void OnResize(EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime && PresentationController.Instance != null)
            {
                base.OnResize(e);

                if (!resizeUpdated)
                {
                    float width = this.slideLayoutDiagram.Width;
                    float height = this.slideLayoutDiagram.Height;
                    if (PresentationController.Instance.CurrentSlideLayout != null)
                    {
                        Display display = PresentationController.Instance.CurrentSlideLayout.Display;
                        slideLayoutDiagram.View.Magnification =
                            Math.Min((width - 40f) / (float)display.Width, (height - 40f) / (float)display.Height) * 100f;
                    }
                }

                resizeUpdated = !resizeUpdated;
            }
        }

        void tool_OnToolDeactivate()
        {
            moveButton.Checked = false;
        }

        void selectTool_OnShowSlideContextMenu(Point p)
        {
            windowContextMenu.Show(slideLayoutDiagram, p);
        }

        void AssignController(PresentationController AController)
        {
            PresentationController pc = AController;
            if (pc != null)
            {
                pc.OnSlideSelectionChanged += new SlideSelectionChanged(m_controller_OnSlideSelectionChanged);
            }
        }

        #endregion

        #region controller event handlers
        void m_controller_OnSlideSelectionChanged(IEnumerable<Slide> NewSelection)
        {
            if (NewSelection.Count() > 0)
            {
                this.toolStripLabel1.Text = String.Format("Выбрана сцена: {0}", NewSelection.First().Name);
                this.slideLayoutDiagram.Visible = true;
            }
            else
            {
                this.toolStripLabel1.Text = "Не выбрано";
                this.slideLayoutDiagram.Visible = false;
                PresentationController.Instance.SendOnSelectedResourceChanged(null);
            }
        }


        void layoutController_OnNotEnoughSpace(DisplayType obj)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (MessageBoxExt.Show(
                    "На контроллере дисплея «"
                    + obj.Name +
                    "» нет места для копирования источников. Очистить дисковое пространство контроллера?", "Подготовка",
                     MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new[] { "Очистить", "Продолжить" }) == DialogResult.OK)
                    ShowClient.Instance.NotEnoughSpaceResponce(obj, false);
                else
                    ShowClient.Instance.NotEnoughSpaceResponce(obj, true);
            }));

        }

        #endregion

        #region Commands

        public void ShowWindowProps()
        {
            //nop
        }

        public void RemoveWindow()
        {
            layoutController.RemoveWindow();
        }

        public void WindowBringToFront()
        {
            layoutController.BringToFront();
        }

        public void WindowSendToBack()
        {
            layoutController.SendToBack();
        }

        public void WindowMoveForward()
        {
            layoutController.BringForward();
        }

        public void WindowMoveBackward()
        {
            layoutController.SendBackward();
        }

        public void BackgroundProps()
        {
            layoutController.ShowBackgroundProps();
        }

        public void ShowPreview()
        {
            if (PresentationController.Instance.CurrentSlideLayout == null || PresentationController.Instance.CurrentSlideLayout.IsEmpty)
            {
                return;
            }

            SlideInfo info = PresentationController.Instance.PresentationInfo.SlideInfoList.FirstOrDefault(s => s.Id == PresentationController.Instance.CurrentSlideLayout.Slide.Id);
            if (info != null)
            {
                if (info.LockingInfo != null && info.LockingInfo.RequireLock == TechnicalServices.Entity.RequireLock.ForShow)
                {
                    MessageBoxExt.Show("Выбранная сцена уже заблокирована для показа.\r\nСначала снимите блокировку для показа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // сохранять нужно перед каждым предварительным просмотром!!!!
            if (PresentationController.Instance.PresentationChanged && MessageBoxExt.Show("Перед предварительным просмотром будет выполнено сохранение сценария.\r\nПродолжить?", Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }) == DialogResult.Cancel)
            {
                return;
            }

            bool flag = true;
            if (PresentationController.Instance.PresentationChanged)
                flag = PresentationController.Instance.SavePresentation();

            if (flag)
            {
                if (!PresentationController.Instance.CurrentSlideLayout.Display.Type.SupportsCaptureScreen)
                {
                    //MessageBoxExt.Show("Выбранный дисплей не поддерживает предварительный просмотр", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    layoutController.LoadAndShowSlide();
                    return;
                }

                //Show preview
                Image preview = layoutController.GetPreview();
                if (preview != null)
                {
                    using (LayoutPreviewForm frm = new LayoutPreviewForm(preview, PresentationController.Instance.CurrentSlideLayout.Display, slideLayoutDiagram.View.Magnification))
                        frm.ShowDialog();
                }
                else
                    MessageBoxExt.Show("Невозможно получить изображение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EnableEdit(bool enabled)
        {
            this.ReadOnly = !enabled;
        }

        #endregion

        #region User handlers

        private void contextMenuStripEx1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = !(layoutController.SelectedWindow != null && layoutController.CanEdit);
            x1ToolStripMenuItem.Visible = x2ToolStripMenuItem.Visible = x3ToolStripMenuItem.Visible = layoutController.IsSegmentedDisplay();
        }

        private void propsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindowProps();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveWindow();
        }

        private void bringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowBringToFront();
        }

        private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowSendToBack();
        }

        private void moveForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowMoveForward();
        }

        private void moveBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowMoveBackward();
        }


        private void fullsizeMenuItem_Click(object sender, EventArgs e)
        {
            SetWindowFullSize();
        }

        private void x1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            layoutController.Set1x1Size();
        }

        private void x2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            layoutController.Set2x2Size();
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            layoutController.Set3x3Size();
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            if (!moveButton.Checked)
            {
                this.slideLayoutDiagram.Controller.ActivateTool(ToolDescriptor.PanTool);
                moveButton.Checked = true;
            }
            else
            {
                this.slideLayoutDiagram.Controller.ActivateTool(ToolDescriptor.SelectTool);
                moveButton.Checked = false;
            }

        }

        /// <summary>
        /// Отобразить настройки фона сцены.
        /// </summary>
        private void backgroundPropsButton_Click(object sender, EventArgs e)
        {
            this.BackgroundProps();
        }

        private void Scrolling(object sender, ScrollEventArgs e)
        {
            this.slideLayoutDiagram.Invalidate();
        }

        private void slideLayoutDiagram_Resize(object sender, EventArgs e)
        {
            this.slideLayoutDiagram.Invalidate();
        }

        #endregion

        public void SetWindowFullSize()
        {
            layoutController.SetWindowFullSize();
        }

        public void IsSegmentedDisplay()
        {
            layoutController.Set3x3Size();
        }


    }
}
