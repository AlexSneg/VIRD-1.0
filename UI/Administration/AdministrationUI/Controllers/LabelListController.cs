using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using Domain.Administration.AdministrationClient;
using Syncfusion.Windows.Forms;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using UI.Administration.AdministrationUI.Forms;
using UI.PresentationDesign.DesignUI.Classes.Helpers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using Label=TechnicalServices.Persistence.SystemPersistence.Configuration.Label;

namespace UI.Administration.AdministrationUI.Controllers
{
    public class LabelListController : CommonAdministationController
    {
        private static LabelListController _instance;
        
        public string AddedLabelCode;
        public static LabelListController Instance
        {
            get
            {
                return _instance;
            }
        }

        public LabelListController(AdministrationForm form, DataGridView dataGridView)
        {
            view = form;
            labelDataGridView = dataGridView;
            _instance = this;
            //identity = Thread.CurrentPrincipal as UserIdentity;//TODO
        }


        AdministrationForm view;
        DataGridView labelDataGridView;
        //SortableBindingList<Label> pList;
        public static SortableBindingList<LabelView> listView = new SortableBindingList<LabelView>();

        public Label[] List
        {
            get
            {
                return AdministrationClient.Instance.GetLabelStorage();
            }
        }

        public void LoadLabelList()
        {
            
            listView.Clear();
            foreach (Label label in List)
            {
                
                    listView.Add(new LabelView()
                    {
                        Id = label.Id,
                        Name = label.Name,
                        
                        Type = (label.IsSystem ? "Конфигурация" : "Пользовательская") 
                    });
                
            }
            view.LabelDataSource = listView;
        }

        
        

        public LabelError AddLabel(Label labelInfo)
        {
            return AdministrationClient.Instance.AddLabel(labelInfo);
        }

        public LabelError DeleteLabel(Label labelInfo)
        {
            return AdministrationClient.Instance.DeleteLabel(labelInfo);
        }

        public LabelError UpdateLabel(Label labelInfo)
        {
            return AdministrationClient.Instance.UpdateLabel(labelInfo);
        }


        public void EditLabelForm(Label labelInfo)
        {
            LabelError error;
            error = AdministrationClient.Instance.LockLabel(labelInfo);

            if ((error & LabelError.NoError) == error)
            {
                try
                {
                    using (LabelForm frm = new LabelForm(labelInfo))
                        frm.ShowDialog();
                }
                finally
                {
                    error = AdministrationClient.Instance.UnlockLabel(labelInfo);
                    if ((error & LabelError.NoError) != error) // Если возникла ошибка при разблокировке
                    {
                        MessageBoxAdv.Show(GetErrorMessage(error, labelInfo), "Ошибка");
                    }
                }
            }
            else // Если возникла ошибка при блокировке
            {
                MessageBoxAdv.Show(GetErrorMessage(error, labelInfo), "Ошибка");
            }
        }

        public string GetErrorMessage(LabelError error, Label labelInfo)
        {
            string errorMessage = "";

            if ((error & LabelError.LockedAlready) == LabelError.LockedAlready)
                errorMessage += "- Метка с названием \"" + labelInfo.Name + "\" заблокирована\n";

            if ((error & LabelError.NoName) == LabelError.NoName)
                errorMessage += "- У метки отсутствует название\n";

            if ((error & LabelError.SystemLabel) == LabelError.SystemLabel)
                errorMessage += "-  Это системная  метка, с которой запрещены какие - либо операции\n";

            if ((error & LabelError.DeletedAlready) == LabelError.DeletedAlready)
                errorMessage += "- Метка " + (labelInfo == null ? "" : "\"" + labelInfo.Name + "\" ") +
                                "уже удалена\n";

            if ((error & LabelError.LabelAlreadyExist) == LabelError.LabelAlreadyExist)
                errorMessage += "- Метка с названием " + (labelInfo == null ? "" : "\"" + labelInfo.Name + "\" ") +
                                "уже учтена в Системе\n";
            
            if ((error & LabelError.NoDeleted) == LabelError.NoDeleted)
                errorMessage += "- Метку не удалось удален\n";

            if ((error & LabelError.UnlockedAlready) == LabelError.UnlockedAlready)
                errorMessage += "- Метка уже разблокирован\n";

            //if ((error & LabelError.WorkInSystem) == LabelError.WorkInSystem)
            //    errorMessage += "- Запись недоступна для изменения: пользователь работает в Системе\n";

            return errorMessage;
        }

    }
}
