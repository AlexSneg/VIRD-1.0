using System;
using System.Windows.Forms;

using Syncfusion.Windows.Forms;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace UI.Common.CommonUI
{
    public partial class LoginForm : Office2007Form, IUserCredential
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        #region IUserCredential Members

        public bool GetUserCredential(out string loginName, out string password)
        {
            loginName = password = string.Empty;
            textBoxPassword.Clear();
            DialogResult result = ShowDialog();

            if (result != DialogResult.OK) return false;

            loginName = Login;
            password = Password;
            return true;
        }

        public void FailedRole(string role)
        {
            MessageBoxAdv.Show(String.Format("Вход в систему не разрешен: пользователь не обладает ролью {0}", role), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void FailedLogin()
        {
            MessageBoxAdv.Show("Вход в систему не разрешен: неверный логин/пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        #endregion

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            string loginName = Login;
            string password = Password;
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                MessageBoxAdv.Show("Логин и пароль должны быть заполнены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                DialogResult = DialogResult.OK;

        }

        private string Login
        {
            get { return textBoxName.Text.Trim(); }
        }

        private string Password
        {
            get { return textBoxPassword.Text; }
        }

    }
}