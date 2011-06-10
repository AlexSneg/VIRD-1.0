using System;
using System.Windows.Forms;

using Syncfusion.Windows.Forms;

using TechnicalServices.Entity;
using TechnicalServices.Security.SecurityCommon;

using UI.Administration.AdministrationUI.Controllers;
using UI.PresentationDesign.DesignUI.Forms;

namespace UI.Administration.AdministrationUI.Forms
{
    public partial class UserForm : PropertyDialog
    {
        //public delegate UserError MethodExt(UserInfo userInfo);

        private readonly UserInfo _userInfoEditor;
        private readonly bool NewUser;

        public UserForm()
        {
            InitializeComponent();
            NewUser = true;
            Text = "Новый пользователь";
            btnSave.Enabled = false;
        }

        public UserForm(UserInfo userInfoEditor)
        {
            _userInfoEditor = userInfoEditor;
            InitializeComponent();
            InitializeControls();
            NewUser = false;
            Text = "Пользователь " + _userInfoEditor.FullName;
            btnSave.Enabled = false;
        }

        private void InitializeControls()
        {
            txtLogin.Text = _userInfoEditor.Name;
            txtPassword.Text = "*************";
            txtUserName.Text = _userInfoEditor.FullName;
            txtComment.Text = _userInfoEditor.Comment;
            chbAdministrator.Checked = _userInfoEditor.IsAdmin;
            chbOperator.Checked = _userInfoEditor.IsOperator;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AcceptChanges();
        }

        private UserInfo GetUserInfo()
        {
            if (!NewUser)
            {
                return _userInfoEditor;
            }
            UserInfo userInfo = new UserInfo();
            userInfo.Enable = true;
            userInfo.Date = DateTime.Today;
            userInfo.Priority = 0;
            return userInfo;
        }

        private void FillUserInfo(ref UserInfo userInfo)
        {
            userInfo.FullName = txtUserName.Text.Trim();
            userInfo.Name = txtLogin.Text.Trim();
            userInfo.Comment = txtComment.Text;
            userInfo.IsAdmin = chbAdministrator.Checked;
            userInfo.IsOperator = chbOperator.Checked;
            if (txtPassword.Modified)
            {
                byte[] hash = SecurityUtils.PasswordToHash(txtPassword.Text);
                userInfo.Hash = hash;
            }
        }

        public override bool AcceptChanges()
        {
            UserError resultError;
            UserInfo userInfo;

            userInfo = GetUserInfo();

            FillUserInfo(ref userInfo);

            if (txtPassword.Text.Length == 0)
            {
                resultError = UserError.NoPassword;
            }
            else if (NewUser)
            {
                resultError = UserListController.Instance.CRUD(UserListController.Instance.AddUser, userInfo);
            }
            else
            {
                resultError = UserListController.Instance.CRUD(UserListController.Instance.UpdateUser, userInfo);
            }

            if ((resultError & UserError.NoError) != resultError) //TODO вынести получение строки в более общий класс
            {
                string errorMessage = UserListController.Instance.GetErrorMessage(resultError, userInfo);
                MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CanClose = false;
                return false;
            }
            UserListController.Instance.AddedUserCode = userInfo.Name;
            DialogResult = DialogResult.OK;
            //userInfo.Id
            CanClose = true;
            CloseMe();
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelChanges();
        }

        public override void CancelChanges()
        {
            DialogResult = DialogResult.Cancel;
            CloseMe();
        }

        public override bool Changed()
        {
            return txtUserName.Modified || txtLogin.Modified || txtComment.Modified || txtPassword.Modified;
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }
    }
}