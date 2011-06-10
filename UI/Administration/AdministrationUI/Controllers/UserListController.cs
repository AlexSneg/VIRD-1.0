using System.Windows.Forms;

using Domain.Administration.AdministrationClient;

using Syncfusion.Windows.Forms;

using TechnicalServices.Entity;

using UI.Administration.AdministrationUI.Forms;
using UI.PresentationDesign.DesignUI.Classes.Helpers;

namespace UI.Administration.AdministrationUI.Controllers
{
    public class UserListController : CommonAdministationController
    {
        private static UserListController _instance;
        public static SortableBindingList<UserInfoView> listView = new SortableBindingList<UserInfoView>();

        private readonly AdministrationForm view;
        //private SortableBindingList<UserInfo> pList;
        public string AddedUserCode;
        private DataGridView userDataGridView;

        //UserIdentity identity;
        public UserListController(AdministrationForm form, DataGridView dataGridView)
        {
            view = form;
            userDataGridView = dataGridView;
            _instance = this;
            //identity = Thread.CurrentPrincipal as UserIdentity;//TODO если понадобится
        }

        public UserInfo[] List
        {
            get { return AdministrationClient.Instance.GetUserStorage(); }
        }

        public static UserListController Instance
        {
            get { return _instance; }
        }

        public void LoadUserList()
        {
            UserInfo systemUser = AdministrationClient.Instance.FindSystemUser();
            listView.Clear();

            foreach (UserInfo info in List)
            {
                if (systemUser != null && systemUser.Id != info.Id)
                {
                    listView.Add(new UserInfoView
                                     {
                                         Id = info.Id,
                                         Login = info.Name,
                                         FullName = info.FullName == null ? "" : info.FullName,
                                         Role = (
                                                    (info.IsAdmin ? "Администратор" : "") +
                                                    (info.IsOperator ? (info.IsAdmin ? ", Оператор" : "Оператор") : "")
                                                ),
                                         Comment = info.Comment == null ? "" : info.Comment
                                     });
                }
            }
            view.UserDataSource = listView;
        }

        public UserError AddUser(UserInfo userInfo)
        {
            return AdministrationClient.Instance.AddUser(userInfo);
        }

        public UserError DeleteUser(UserInfo userInfo)
        {
            return AdministrationClient.Instance.DeleteUser(userInfo);
        }

        public UserError UpdateUser(UserInfo userInfo)
        {
            return AdministrationClient.Instance.UpdateUser(userInfo);
        }

        public void EditUserForm(UserInfo userInfo)
        {
            UserError error;
            error = AdministrationClient.Instance.LockUser(userInfo);

            if ((error & UserError.NoError) == error)
            {
                try
                {
                    using (UserForm frm = new UserForm(userInfo))
                        frm.ShowDialog();
                }
                finally
                {
                    error = AdministrationClient.Instance.UnlockUser(userInfo);
                    if ((error & UserError.NoError) != error) // Если возникла ошибка при разблокировке
                    {
                        MessageBoxAdv.Show(GetErrorMessage(error, userInfo), "Ошибка");
                    }
                }
            }
            else // Если возникла ошибка при блокировке
            {
                MessageBoxAdv.Show(GetErrorMessage(error, userInfo), "Ошибка");
            }
        }

        public string GetErrorMessage(UserError error, UserInfo userInfo)
        {
            string errorMessage = "";

            if ((error & UserError.LockedAlready) == UserError.LockedAlready)
                errorMessage += "- Пользователь с логином \"" + userInfo.Name + "\" заблокирован\n";

            if ((error & UserError.NoLogin) == UserError.NoLogin)
                errorMessage += "- У пользователя отсутствует логин\n";

            if ((error & UserError.NoPassword) == UserError.NoPassword)
                errorMessage += "- Не заполнено поле Пароль\n";

            if ((error & UserError.DeletedAlready) == UserError.DeletedAlready)
                errorMessage += "- Пользователь " + (userInfo == null ? "" : "\"" + userInfo.Name + "\" ") +
                                "уже удален\n";

            if ((error & UserError.LoginAlreadyExist) == UserError.LoginAlreadyExist)
                errorMessage += "- Пользователь с логином " + (userInfo == null ? "" : "\"" + userInfo.Name + "\" ") +
                                "уже учтен в Системе\n";

            if ((error & UserError.FIOAlreadyExist) == UserError.FIOAlreadyExist)
                errorMessage += "- Пользователь с таким ФИО " +
                                (userInfo == null ? "" : "\"" + userInfo.FullName + "\" ") + "уже учтен в Системе\n";

            if ((error & UserError.NoRole) == UserError.NoRole)
                errorMessage += "- Пользователю не назначена ни одна роль\n";

            if ((error & UserError.NoFIO) == UserError.NoFIO)
                errorMessage += "- У пользователя отсутствует ФИО\n";

            if ((error & UserError.NoDeleted) == UserError.NoDeleted)
                errorMessage += "- Пользователь не удален\n";

            if ((error & UserError.UnlockedAlready) == UserError.UnlockedAlready)
                errorMessage += "- Пользователь уже разблокирован\n";

            if ((error & UserError.NoMoreAdminAfterDelete) == UserError.NoMoreAdminAfterDelete)
                errorMessage += "- В Системе должен остаться хотя бы один пользователь с ролью Администратор\n";

            if ((error & UserError.WorkInSystem) == UserError.WorkInSystem)
                errorMessage += "- Запись недоступна для изменения: пользователь работает в Системе\n";

            if ((error & UserError.LastUserWithAdminRole) == UserError.LastUserWithAdminRole)
                errorMessage += "- В Системе должен остаться хотя бы один пользователь с ролью Администратор\n";


            return errorMessage;
        }
    }
}