using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Domain.Administration.AdministrationClient;
using TechnicalServices.Configuration.Common;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using UI.Administration.AdministrationUI.Controllers;
using UI.Common.CommonUI.Forms;
using UI.PresentationDesign.DesignUI.Helpers;
using Label = TechnicalServices.Persistence.SystemPersistence.Configuration.Label;


namespace UI.Administration.AdministrationUI.Forms
{

    internal enum Grid
    {
        UserGrid = 0,
        LabelGrid = 1,
        SystemGrid = 2
    }

    
    
    //public delegate void Method();

    public partial class AdministrationForm : RibbonForm
    {
        private Grid SelectedTable { set; get; }

        private readonly UserListController _controller;
        private readonly LabelListController _labelController;
        private readonly SystemParametersController _systemParametersController;
        
        
        private int selectedUserRowIndex = 0;
        private int selectedLabelRowIndex = 0;
        //private int selectedSystemParamsRowIndex = 0;

        DataGridViewColumn sortColumnUserGrid = null;
        ListSortDirection sortOrderUserGrid;

        DataGridViewColumn sortColumnLabelGrid = null;
        ListSortDirection sortOrderLabelGrid;

        public void _resourceCRUD_OnTerminate(object sender, LabelEventArg e)
        {
            MessageBoxAdv.Show( e.Label.Name);
        }

        public AdministrationForm(IEventLogging logging) : this()
        {
            ILabelStorageAdapter labelStorageAdapter = new LabelStorageAdapter(null, logging);
            this._controller = new UserListController(this, gridUserParametrs);
            this._labelController = new LabelListController(this, gridUserParametrs);
            this._systemParametersController = new SystemParametersController(this, gridSystemParameters);
            SystemParametersController.Instance.OnSystemParametersCellChenged += new SystemParametersCellChenged(AdministrationForm_OnSystemParametersCellChenged);
            UserGridVisibility();
        }

        public AdministrationForm()
        {
            InitializeComponent();
        }

        private void AdministrationForm_OnSystemParametersCellChenged(bool changed)
        {
            btnCancel.Enabled = btm1Cancel.Enabled = btnSave.Enabled = btm1Save.Enabled = changed;
        }

        public Object UserDataSource
        {
            get
            {
                return gridUserParametrs.DataSource;
                
            }
            set
            {
                
                gridUserParametrs.DataSource = value;
            }
        }

        public Object LabelDataSource
        {
            get
            {
                return gridLabelParameters.DataSource;
            }
            set
            {
                
                gridLabelParameters.DataSource = value;
            }
        }

        public Object SystemDataSource
        {
            get
            {
                return gridLabelParameters.DataSource;
            }
            set
            {
                gridLabelParameters.DataSource = value;
            }
        }

        private void UserGridVisibility()
        {
            SaveSelectedRowIndex();
            statusStripSelectedRow.Text = "";
            btnSave.Enabled = false;
            btm1Save.Enabled = false;
            btnCancel.Enabled = false;
            btm1Cancel.Enabled = false;

            btnAdd.Enabled = true;
            btnChange.Enabled = true;
            btnDelete.Enabled = true;
            btm3Change.Enabled = true;
            btm3Delete.Enabled = true;
            btmCommand.Enabled = true;

            gridUserParametrs.Visible = true;
            gridLabelParameters.Visible = false;
            gridSystemParameters.Visible = false;
            SelectedTable = Grid.UserGrid;
            UserListController.Instance.CRUD(UserListController.Instance.LoadUserList);

            if (sortColumnUserGrid != null && sortColumnUserGrid.Index!=0)
            {
                gridUserParametrs.Sort(sortColumnUserGrid, sortOrderUserGrid);
            }
            else
            {
                gridUserParametrs.Sort(gridUserParametrs.Columns[2], sortOrderUserGrid);
            }
            statusStripCount.Text = "Всего записей: " + gridUserParametrs.Rows.Count.ToString();
            UserRowSelect();
        }

        void UserRowSelect()
        {
            if (gridUserParametrs.Rows.Count > selectedUserRowIndex)
                gridUserParametrs.Rows[selectedUserRowIndex].Selected = true;
            else if (gridUserParametrs.Rows.Count > 0)
                gridUserParametrs.Rows[gridUserParametrs.Rows.Count - 1].Selected = true;
        
        }

         void LabelRowSelect()
         {
             if (gridLabelParameters.Rows.Count > selectedLabelRowIndex)
                 gridLabelParameters.Rows[selectedLabelRowIndex].Selected = true;
             else if (gridLabelParameters.Rows.Count > 0)
                 gridLabelParameters.Rows[gridLabelParameters.Rows.Count - 1].Selected = true;
         }

        private void LabelGridVisibility()
        {
            SaveSelectedRowIndex();
            statusStripSelectedRow.Text = "";
            btnSave.Enabled = false;
            btm1Save.Enabled = false;
            btnCancel.Enabled = false;
            btm1Cancel.Enabled = false;

            btnAdd.Enabled = true;
            btnChange.Enabled = true;
            btnDelete.Enabled = true;

            btm3Change.Enabled = true;
            btm3Delete.Enabled = true;

            btmCommand.Enabled = true;

            gridLabelParameters.Visible = true;
            gridUserParametrs.Visible = false;
            gridSystemParameters.Visible = false;
            SelectedTable = Grid.LabelGrid;
            LabelListController.Instance.CRUD(LabelListController.Instance.LoadLabelList);

            if (sortColumnLabelGrid != null && sortColumnLabelGrid.Index != 0)
            {
                gridLabelParameters.Sort(sortColumnLabelGrid, sortOrderLabelGrid);
            }
            else
            {
                gridLabelParameters.Sort(gridLabelParameters.Columns[1], sortOrderLabelGrid);
            }
            statusStripCount.Text = "Всего записей: " + gridLabelParameters.Rows.Count.ToString();
            LabelRowSelect();
        }


        private void SystemParametersGridVisibility()
        {
            SaveSelectedRowIndex();
            statusStripSelectedRow.Text = "";
            btnSave.Enabled = btm1Save.Enabled = false;
            btnCancel.Enabled = btm1Cancel.Enabled = false;
            btnAdd.Enabled = false;
            btnChange.Enabled = false;
            btnDelete.Enabled = false;
            btm3Change.Enabled = false;
            btm3Delete.Enabled = false;
            btmCommand.Enabled = false;

            gridSystemParameters.Visible = true;
            gridUserParametrs.Visible = false;
            gridLabelParameters.Visible = false;
            SelectedTable = Grid.SystemGrid;

            statusStripCount.Text = "";
            

            LabelListController.Instance.CRUD(SystemParametersController.Instance.LoadSystemParameters);
            
        }

        void SaveSelectedRowIndex()
        {
            //Запомнить индекс выбранной строки UserTable
            if (gridUserParametrs.SelectedRows.Count == 1)
                selectedUserRowIndex = gridUserParametrs.SelectedRows[0].Index;
            //Запомнить индекс выбранной строки LabelTable
            if (gridLabelParameters.SelectedRows.Count == 1)
                selectedLabelRowIndex = gridLabelParameters.SelectedRows[0].Index;
        }


        private void groupView1_GroupViewItemSelected(object sender, EventArgs e)
        {
            SystemParametersController.Instance.CRUD(CheckForChangesInSystemParameters);
            int index = ((Syncfusion.Windows.Forms.Tools.GroupView)(sender)).SelectedItem;
            switch (index)
            {
                case 0:
                    UserGridVisibility();
                    break;
                case 1:
                    LabelGridVisibility();
                    break;
                case 2:
                    SystemParametersGridVisibility();
                    break;
                default:
                    break;
            }
        }

        
        #region Menu Click Events

        private void btm2User_Click(object sender, EventArgs e)
        {
            SystemParametersController.Instance.CRUD(CheckForChangesInSystemParameters);
            UserGridVisibility();
            groupView1.SelectedItem = 0;
        }

        private void btm2Label_Click(object sender, EventArgs e)
        {
            SystemParametersController.Instance.CRUD(CheckForChangesInSystemParameters);
            LabelGridVisibility();
            groupView1.SelectedItem = 1;
        }

        private void btm2Sysparam_Click(object sender, EventArgs e)
        {
            SystemParametersGridVisibility();
            groupView1.SelectedItem = 2;
        }

        private void btm1Exit_Click(object sender, EventArgs e)
        {
            SystemParametersController.Instance.CRUD(CheckForChangesInSystemParameters);
            Close();
        }

        private void btm1Save_Click(object sender, EventArgs e)
        {
            SystemParametersController.Instance.CRUD(SystemParametersController.Instance.SaveSystemParameters);
        }

        private void btm1Cancel_Click(object sender, EventArgs e)
        {
            gridSystemParameters.ConfirmChanges();
            SystemParametersGridVisibility();
            SystemParametersController.Instance.Changed = false;
        }

        private void  CheckForChangesInSystemParameters()
        {
            if (SystemParametersController.Instance.Changed)
            {
                switch (MessageBoxAdv.Show("Системные параметры были изменены. Сохранить изменения?", "Подтверждение",
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        SystemParametersController.Instance.SaveSystemParameters();
                        break;
                    case DialogResult.No:
                        
                        break;
                    //case DialogResult.Cancel:
                        //((Syncfusion.Windows.Forms.Tools.GroupView)(sender)).SelectedItem = 0;
                        //return;
                }
                SystemParametersController.Instance.Changed = false;
            }
        }

        private void Delete()
        {
            
            int index;
            switch (SelectedTable)
            {
                case Grid.UserGrid:

                    index = gridUserParametrs.SelectedRows.Count == 1 ? gridUserParametrs.SelectedRows[0].Index : -1;
                    if (index != -1)
                    {
                        UserError resultError = UserError.NoError;
                        UserInfo userInfoResult;
                        switch (
                            MessageBoxExt.Show("Вы действительно хотите удалить пользователя?", "Удаление пользователя", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }))
                        {
                            case DialogResult.OK:
                                int Id = (int)gridUserParametrs.Rows[index].Cells[0].Value;
                                //userInfoResult = UserListController.Instance.List.Find(x => x.Id == Id);
                                userInfoResult = Array.Find(UserListController.Instance.List, x => x.Id == Id);
                                resultError = UserListController.Instance.DeleteUser(userInfoResult);
                                UserGridVisibility();

                                if ((resultError & UserError.NoError) != resultError)
                                {
                                    string errorMessage = UserListController.Instance.GetErrorMessage(resultError, userInfoResult);
                                    MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                break;
                            case DialogResult.Cancel:
                                break;
                        }
                    }
                    break;
                case Grid.LabelGrid:
                    index = gridLabelParameters.SelectedRows.Count == 1 ? gridLabelParameters.SelectedRows[0].Index : -1;
                    if (index != -1)
                    {
                        LabelError resultError = LabelError.NoError;
                        Label labelResult;
                        switch (
                            MessageBoxExt.Show("Вы действительно хотите удалить метку?", "Удаление метки", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }))
                           // MessageBoxAdv.Show("Вы действительно хотите удалить метку?", "Удаление метки",
                             //               MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                        {
                            case DialogResult.OK:
                                int Id = (int)gridLabelParameters.Rows[index].Cells[0].Value;
                                labelResult = Array.Find(LabelListController.Instance.List, x => x.Id == Id);
                                resultError = LabelListController.Instance.DeleteLabel(labelResult);
                                LabelGridVisibility();

                                if ((resultError & LabelError.NoError) != resultError)
                                {
                                    string errorMessage = LabelListController.Instance.GetErrorMessage(resultError, labelResult);
                                    MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                break;
                            case DialogResult.Cancel:
                                break;
                        }
                    }

                    break;
                case Grid.SystemGrid:
                    break;
            }
        }

        private void Change()
        {
            switch (SelectedTable)
            {
                case Grid.UserGrid:
                    int index = gridUserParametrs.SelectedRows.Count == 1 ? gridUserParametrs.SelectedRows[0].Index :-1 ;
                    if (index != -1)
                    {   //TODO: измение после изменений 
                        int Id = (int)gridUserParametrs.Rows[index].Cells[0].Value;
                        UserInfo userInfo = Array.Find(UserListController.Instance.List,x => x.Id == Id);
                        UserListController.Instance.EditUserForm(userInfo);
                    }
                    UserGridVisibility();

                    foreach (DataGridViewRow row in gridUserParametrs.Rows)
                    {
                        if ((string)row.Cells[1].Value == UserListController.Instance.AddedUserCode)
                        {
                            selectedUserRowIndex = row.Index;
                            UserRowSelect();
                        }
                    }

                    break;
                case Grid.LabelGrid:
                    index = gridLabelParameters.SelectedRows.Count == 1 ? gridLabelParameters.SelectedRows[0].Index : -1;
                    if (index != -1)
                    {   //TODO: измение после изменений 
                        int Id = (int)gridLabelParameters.Rows[index].Cells[0].Value;
                        Label labelInfo = Array.Find(LabelListController.Instance.List, x => x.Id == Id);
                        LabelListController.Instance.EditLabelForm(labelInfo);
                    }
                    LabelGridVisibility();
                    foreach (DataGridViewRow row in gridLabelParameters.Rows)
                    {
                        if ((string)row.Cells[1].Value == LabelListController.Instance.AddedLabelCode)
                        {
                            selectedLabelRowIndex = row.Index;
                            LabelRowSelect();
                        }
                    }
                    break;
            }

        }

        private void Add()
        {
            switch (SelectedTable)
            {
                case Grid.UserGrid:
                    using (UserForm frm = new UserForm())
                        frm.ShowDialog();
                    UserGridVisibility();
                    gridUserParametrs.Refresh();
                    //Установка курсора таблицы на добавленное значение

                    foreach (DataGridViewRow row in gridUserParametrs.Rows)
                    {
                        if ((string) row.Cells[1].Value == UserListController.Instance.AddedUserCode)
                        {
                            selectedUserRowIndex = row.Index;
                            UserRowSelect();
                        }
                    }

                    break;
                case Grid.LabelGrid:
                    using (LabelForm frm = new LabelForm())
                        frm.ShowDialog();
                    LabelGridVisibility();
                    gridLabelParameters.Refresh();
                    foreach (DataGridViewRow row in gridLabelParameters.Rows)
                    {
                        if ((string)row.Cells[1].Value == LabelListController.Instance.AddedLabelCode)
                        {
                            selectedLabelRowIndex = row.Index;
                            LabelRowSelect();
                        }
                    }
                    break;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            UserListController.Instance.CRUD(Delete);
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            UserListController.Instance.CRUD(Change);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserListController.Instance.CRUD(Add);
        }

        #endregion

        private void gridUserParametrs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void gridSystemParameters_CellClick(object sender, Syncfusion.Windows.Forms.Grid.GridCellClickEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColIndex;
            Control control = gridSystemParameters[rowIndex, colIndex].Control;
            if (control == null) return;
            //вычислим координаты внутри контрола
            Point point = new Point(e.MouseEventArgs.X - control.Location.X, e.MouseEventArgs.Y - control.Location.Y);
            Button button = FindButton(control.Controls, point);
            if (button != null)
            {
                button.PerformClick();
            }

        }
        private Button FindButton(Control.ControlCollection collection, Point point)

        {
            foreach (Control control in collection)
            {
                if (!control.Bounds.Contains(point)) continue;
                if (control is Button) return (Button)control;
            }
            return null;
        }

        private void gridSystemParameters_Resize(object sender, EventArgs e)
        {
            int a = gridSystemParameters.Width;
            int columnA = Convert.ToInt32(a*0.3);
            int columnB = Convert.ToInt32(a*0.3);
            int columnC = a - (columnA + columnB)-1;
            this.gridSystemParameters.ColWidthEntries.Clear();
            this.gridSystemParameters.ColWidthEntries.AddRange(new Syncfusion.Windows.Forms.Grid.GridColWidth[] {
            new Syncfusion.Windows.Forms.Grid.GridColWidth(0, 0),
            new Syncfusion.Windows.Forms.Grid.GridColWidth(1, columnA),
            new Syncfusion.Windows.Forms.Grid.GridColWidth(2, columnB),
            new Syncfusion.Windows.Forms.Grid.GridColWidth(3, columnC)});
            
        }

        private void AdministrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemParametersController.Instance.CRUD(CheckForChangesInSystemParameters);
        }


        private void gridLabelParameters_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = null;
            statusStripSelectedRow.Text = ""; 
            if (((DataGridView)sender).SelectedRows.Count==1)
             row = ((DataGridView) sender).SelectedRows[0];

            if (row!=null)
            {
                int Id = (int)gridLabelParameters.Rows[row.Index].Cells[0].Value;
                Label labelResult = Array.Find(LabelListController.Instance.List, x => x.Id == Id);

                if (labelResult == null)
                {
                    MessageBoxAdv.Show(this, "Метка уже удалена", "Внимание", MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);
                    gridLabelParameters.Rows.Remove(row);
                    return;
                }
                if (labelResult.IsSystem)
                {
                    btnChange.Enabled = false;
                    btnDelete.Enabled = false;
                    btm3Change.Enabled = false;
                    btm3Delete.Enabled = false;
                }
                else
                {
                    btnChange.Enabled = true;
                    btnDelete.Enabled = true;
                    btm3Change.Enabled = true;
                    btm3Delete.Enabled = true;
                }
                statusStripSelectedRow.Text = "Текущая запись: " + labelResult.Name;

            }
        }

        private void gridLabelParameters_Sorted(object sender, EventArgs e)
        {
            sortColumnLabelGrid = gridLabelParameters.SortedColumn;
            if (gridLabelParameters.SortOrder == SortOrder.Descending)
            {
                sortOrderLabelGrid = ListSortDirection.Descending;
            }
            else
            {
                sortOrderLabelGrid = ListSortDirection.Ascending;
            }
        }

        private void gridUserParametrs_Sorted(object sender, EventArgs e)
        {
            sortColumnUserGrid = gridUserParametrs.SortedColumn;
            if (gridUserParametrs.SortOrder == SortOrder.Descending)
            {
                sortOrderUserGrid = ListSortDirection.Descending;
            }
            else
            {
                sortOrderUserGrid = ListSortDirection.Ascending;
            }
        }

        private void ribbonControlAdv1_Click(object sender, EventArgs e)
        {

        }

        private void btnExitForm_Click(object sender, EventArgs e)
        {
            SystemParametersController.Instance.CRUD(CheckForChangesInSystemParameters);
            Close();
        }

        private void gridUserParametrs_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = null;
            statusStripSelectedRow.Text = "";
            if (((DataGridView)sender).SelectedRows.Count == 1)
                row = ((DataGridView)sender).SelectedRows[0];

            if (row != null)
            {
                int Id = (int)gridUserParametrs.Rows[row.Index].Cells[0].Value;
                UserInfo userResult = Array.Find(UserListController.Instance.List, x => x.Id == Id);
                if (userResult == null)
                {
                    MessageBoxAdv.Show(this, "Пользователь уже удален", "Внимание", MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);
                    gridUserParametrs.Rows.Remove(row);
                    return;
                }
                statusStripSelectedRow.Text = "Текущая запись: " + userResult.FullName;
            }
        }

        private void menuButtonAbout_Click(object sender, EventArgs e)
        {
            AboutDialog.Execute(this);
        }

    }
}
