using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Domain.PresentationDesign.DesignCommon;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using TechnicalServices.Communication.Communication.Client;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

using UI.Common.CommonUI.Forms;
using UI.ImportExport.ImportExportUI.Controllers;
using UI.PresentationDesign.DesignUI.Classes.Helpers;
using Domain.PresentationDesign.Client;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPresentation;
using TechnicalServices.Entity;
using System.Threading;
using System.Text;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class PresentationListForm : RibbonForm
    {
        #region fields
        public PresentationListController Controller
        {
            get;
            set;
        }

        public Object DataSource
        {
            get
            {
                return presentationGridView.DataSource;
            }
            set
            {
                presentationGridView.DataSource = value;
            }
        }

        public DataGridView GridView
        {
            get
            {
                return this.presentationGridView;
            }
        }

        #endregion

        #region OnLoad & constructor

        public PresentationListForm()
            : this(true)
        {
        }

        public PresentationListForm(bool isDesigner)
        {
            InitializeComponent();

            this.statusStripEx1.ContextMenuStrip = null;
            this.WindowState = FormWindowState.Maximized;

            this.Controller = new PresentationListController(this);
            presentationGridView.AutoGenerateColumns = false;

            m_IsDesigner = isDesigner;
            setDesignerMode(m_IsDesigner);

            presentationFilterControl.OnSwitchToPrev += new UI.PresentationDesign.DesignUI.Controls.SwitchToNext(presentationFilterControl_OnSwitchToPrev);
            presentationFilterControl.OnSwitchToNext += new UI.PresentationDesign.DesignUI.Controls.SwitchToNext(presentationFilterControl_OnSwitchToNext);
            slideFilterControl.OnSwitchToNext += new UI.PresentationDesign.DesignUI.Controls.SwitchToNext(slideFilterControl_OnSwitchToNext);
            slideFilterControl.OnSwitchToPrev += new UI.PresentationDesign.DesignUI.Controls.SwitchToNext(slideFilterControl_OnSwitchToPrev);
            toolStripEx4.OnSwitchToNext += new UI.PresentationDesign.DesignUI.Controls.SwitchToNext(toolStripEx4_OnSwitchToNext);
            toolStripEx4.OnSwitchToPrev += new UI.PresentationDesign.DesignUI.Controls.SwitchToNext(toolStripEx4_OnSwitchToPrev);
            presentationGridView.OnSwitchToNext += new UI.PresentationDesign.DesignUI.Controls.SwitchToNext(presentationGridView_OnSwitchToNext);
            presentationGridView.OnSwitchToPrev += new UI.PresentationDesign.DesignUI.Controls.SwitchToNext(presentationGridView_OnSwitchToPrev);
            modeStripLabel.Text = Description.GetModeDescription(DesignerClient.Instance.IsStandAlone);
        }

        #region Tab switch
        void presentationFilterControl_OnSwitchToPrev()
        {
            this.presentationGridView.Focus();
        }

        void presentationGridView_OnSwitchToPrev()
        {
            toolStripEx4.SelectLastItem();
        }

        void presentationGridView_OnSwitchToNext()
        {
            presentationFilterControl.FocusFirstControl();
        }

        void toolStripEx4_OnSwitchToPrev()
        {
            slideFilterControl.FocusLastControl();
        }


        void slideFilterControl_OnSwitchToPrev()
        {
            presentationFilterControl.FocusLastControl();
        }

        void slideFilterControl_OnSwitchToNext()
        {
            toolStripEx4.Focus();
            toolStripEx4.SelectFirstItem();
        }

        void toolStripEx4_OnSwitchToNext()
        {
            this.presentationGridView.Focus();
        }

        void presentationFilterControl_OnSwitchToNext()
        {
            slideFilterControl.FocusTextBox();
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.toolStripEx2.Items.Add(new ToolStripControlHost(presentationFilterControl));
            this.toolStripEx3.Items.Add(new ToolStripControlHost(slideFilterControl));
            this.fromXmlButton.Enabled = 
                this.toXmlButton.Enabled = 
                this.configToXmlButton.Enabled = 
                this.importToolButton.Enabled = 
                this.exportToolButton.Enabled =
                !DesignerClient.Instance.IsStandAlone;
            if (exportToolButton.Enabled) exportToolButton.Enabled = presentationGridView.Rows.Count > 0;
        }
        #endregion

        #region Player/Designer difference
        private readonly bool m_IsDesigner;
        private const String designerTag = "Designer";
        private const String playerTag = "Player";

        private void setDesignerMode(bool isDesigner)
        {
            foreach (ToolStripItem item in this.toolStripEx1.Items)
                item.Visible = !((item.Tag.Equals(designerTag) && !isDesigner) || (item.Tag.Equals(playerTag) && isDesigner));

            foreach (ToolStripItem item in this.mainTopStrip.OfficeMenu.Items)
                item.Visible = !((item.Tag.Equals(designerTag) && !isDesigner) || (item.Tag.Equals(playerTag) && isDesigner));
        }
        #endregion

        #region Click handlers
        private void filterStripButton_Click(object sender, EventArgs e)
        {
            Controller.Filter();
            filterStripButton.Checked = true;

        }

        private void openPresentation(object sender, EventArgs e)
        {
            if (presentationGridView.SelectedRows.Count > 0)
            {
                PresentationInfo info = ((PresentationInfo)presentationGridView.SelectedRows[0].DataBoundItem);
                Controller.OpenPresentation(info, m_IsDesigner);
            }
        }

        private void viewPresentation(object sender, EventArgs e)
        {
            if (presentationGridView.SelectedRows.Count > 0)
            {
                PresentationInfo info = ((PresentationInfo)presentationGridView.SelectedRows[0].DataBoundItem);
                Controller.OpenPresentation(info, false);
            }
        }

        private void presentationGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex <= presentationGridView.RowCount)
            {
                PresentationInfo info = ((PresentationInfo)presentationGridView.Rows[e.RowIndex].DataBoundItem);
                Controller.OpenPresentation(info, m_IsDesigner);
            }
        }

        private void createToolButton_Click(object sender, EventArgs e)
        {
            Controller.CreatePresentation();
        }

        private void createFromPPTToolButton_Click(object sender, EventArgs e)
        {
            Controller.CreatePresentationFromPpt();
        }

        private void unfilterStripButton_Click(object sender, EventArgs e)
        {
            Controller.Unfilter();
            filterStripButton.Checked = false;
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            filterTabItem.PerformClick();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void removePresentation(object sender, EventArgs e)
        {
            RemovePresentation();
        }

        private void RemovePresentation()
        {
            if (MessageBoxExt.Show("Подтвердите удаление выделенных записей", Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] {"Да", "Нет"}) == DialogResult.OK)
            {
                int count = presentationGridView.SelectedRows.Count;
                var list = presentationGridView.SelectedRows.Cast<DataGridViewRow>().Select(r => r.DataBoundItem).Cast<PresentationInfo>().OrderBy(o => o.Name).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    UserIdentity id;
                    PresentationInfo info = list[i];
                    bool last = (i < list.Count - 1);
                    PresentationStatus status = DesignerClient.Instance.PresentationWorker.GetPresentationStatus(info.UniqueName, out id);
                    if (Controller.RemovePresentation(info, ref status))
                    {
                        //nop
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Сценарий {0} не может быть удален\r\nПричина: {1}", info.Name, PresentationStatusInfo.GetPresentationStatusDescr(info, status, id));
                        //DialogResult r = MessageBoxExt.Show(sb.ToString(), "Внимание", count > 1 ? MessageBoxButtons.RetryCancel : MessageBoxButtons.OK, MessageBoxIcon.Warning, new string[] { (status == PresentationStatus.LockedForEdit)? "OK": "Продолжить", "Отмена" });
                        string[] buttonsText =
                            last ? new string[] { "Продолжить", "Отмена" } : new string[] { "ОК" };

                        DialogResult r = MessageBoxExt.Show(sb.ToString(),
                            "Внимание",
                            last ? MessageBoxButtons.RetryCancel : MessageBoxButtons.OK,
                            MessageBoxIcon.Warning,
                            buttonsText);
                        if (r == DialogResult.Cancel)
                            break;
                    }
                }
            }
        }


        private void aboutButton_Click(object sender, EventArgs e)
        {
            //TODO : Показать окно О программе
            AboutDialog.Execute(this);
        }

        private void presentationGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.presentationGridView.Columns[e.ColumnIndex].Name == "StatusColumn")
            {
                e.FormattingApplied = true;
                if (false == DesignerClient.Instance.IsStandAlone)
                {
                    LockingInfo info = Controller.GetPresentationStatus((PresentationInfo)this.presentationGridView.Rows[e.RowIndex].DataBoundItem);
                    if (info == null)
                    {
                        e.Value = imageList1.Images["unlocked"];
                    }
                    else
                        if (info.RequireLock == RequireLock.ForEdit)
                        {
                            e.Value = imageList1.Images["locked"];
                        }
                        else
                            e.Value = imageList1.Images["show"];
                }
                else
                {
                    e.Value = imageList1.Images["standalone"];
                }
            }

            if (this.presentationGridView.Columns[e.ColumnIndex].Name == "DateModifiedColumn")
            {
                e.Value = ((PresentationInfo)this.presentationGridView.Rows[e.RowIndex].DataBoundItem).LastChangeDate.ToString("dd MMM yyyy, HH:mm:ss");
            }
        }

        internal void RefreshView()
        {
            Sort();
            presentationGridView.Refresh();
        }

        private void paramsButton_Click(object sender, EventArgs e)
        {
            using (ConfigForm frm = new ConfigForm(PresentationController.Configuration))
                frm.ShowDialog();
        }

        internal void Sort()
        {
            DataGridViewColumn c = presentationGridView.SortedColumn;
            SortOrder o = presentationGridView.SortOrder;
            presentationGridView.Sort(c, o == SortOrder.Ascending ? System.ComponentModel.ListSortDirection.Ascending : System.ComponentModel.ListSortDirection.Descending);
        }

        private void presentationGridView_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && m_IsDesigner)
            {
                RemovePresentation();
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (this.GridView.SelectedRows.Count > 0)
            {
                PresentationInfo info = ((PresentationInfo)presentationGridView.Rows[this.GridView.SelectedRows[0].Index].DataBoundItem);
                Controller.OpenPresentation(info, m_IsDesigner);
            }
        }

        private void configToXmlButton_Click(object sender, EventArgs e)
        {
            ExportConfigurationController.Instanse.Export();
        }

        private void toXmlButton_Click(object sender, EventArgs e)
        {
            if (presentationGridView.SelectedRows.Count == 0) return;
            List<PresentationInfo> presentationInfos = new List<PresentationInfo>(presentationGridView.SelectedRows.Count);
            foreach (DataGridViewRow row in presentationGridView.SelectedRows)
            {
                presentationInfos.Add((PresentationInfo)row.DataBoundItem);
            }
            ExportPresentationController.Instanse.Export(presentationInfos.ToArray());
        }

        private void fromXmlButton_Click(object sender, EventArgs e)
        {
            ImportPresentationController.Instanse.Import(ResourceExistsDialog.Show);
        }

        #endregion

        #region Selection

        private void presentationGridView_SelectionChanged(object sender, EventArgs e)
        {
            bool IsEnabled = presentationGridView.SelectedRows.Count > 0;
            EnableOpenOrRemoveCmd(IsEnabled);

            if (IsEnabled)
            {
                PresentationInfoExt info = ((PresentationInfoExt)presentationGridView.SelectedRows[0].DataBoundItem);
                statusStripLabel.Text = info.Name;
                if (info.LockingInfo != null)
                {
                    string reason = info.LockingInfo.RequireLock == RequireLock.ForEdit ? "для редактирования" : "для показа";
                    lockedStripLabel.Text = string.Format("Заблокирован: {0}, {1}",
                        string.IsNullOrEmpty(info.LockingInfo.UserIdentity.User.FullName) 
                            ? info.LockingInfo.UserIdentity.User.Name 
                            : info.LockingInfo.UserIdentity.User.FullName, 
                        reason);
                }
                else lockedStripLabel.Text = String.Empty;
            }
            else
            {
                statusStripLabel.Text = String.Empty;
                lockedStripLabel.Text = String.Empty;
            }
        }

        private void EnableOpenOrRemoveCmd(bool IsEnabled)
        {
            openButton.Enabled = openToolButton.Enabled = presentationGridView.SelectedRows.Count == 1;
            exportToolButton.Enabled = IsEnabled && !DesignerClient.Instance.IsStandAlone;
            removeButton.Enabled = removeToolButton.Enabled = IsEnabled;
        }

        #endregion

        #region Commands

        private void InitAndShowDesigner(PresentationDesignerForm designer, bool Editing)
        {
            Cursor = Cursors.WaitCursor;
            designer.Init(Editing);
            this.Hide();
            Cursor = Cursors.Default;
            designer.ShowDialog();
        }

        private void InitAndShowPlayer(PlayerForm player)
        {
            Cursor = Cursors.WaitCursor;
            player.InitPresentation();
            this.Hide();
            Cursor = Cursors.Default;
            player.ShowDialog();
        }

        public void ShowDesigner(PresentationInfo info, bool Editing)
        {
            ShowDesigner(info, Editing, false);
        }

        public void ShowDesigner(PresentationInfo info, bool Editing, bool JustCreatedPresentation)
        {
            using (PresentationDesignerForm designer = new PresentationDesignerForm(info))
            {
                designer.JustCreatedPresentation = JustCreatedPresentation; // Признак только что созданной презентации
                InitAndShowDesigner(designer, Editing);
            }

            if (!this.IsDisposed)
                this.Show();
        }

        public void ShowPlayer(PresentationInfo info)
        {
            using (PlayerForm player = new PlayerForm(info))
            {
                InitAndShowPlayer(player);
            }
            this.Show();
        }

        private void EnableFilterCmd(bool IsEnabled)
        {
            filterTabItem.Enabled = IsEnabled;
            filterToolButton.Enabled = IsEnabled;
            filterStripButton.Enabled = IsEnabled;
        }

        #endregion

        private void mainTopStrip_OfficeMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.toXmlButton.Enabled =
                !DesignerClient.Instance.IsStandAlone 
                && (presentationGridView.Rows.Count > 0);
        }

    }
}
