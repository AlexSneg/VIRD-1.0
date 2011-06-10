using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using Label = TechnicalServices.Persistence.SystemPersistence.Configuration.Label;

namespace UI.Administration.AdministrationUI.Controllers
{
    public class CommonAdministationController
    {
        public void CRUD(Action proc)
        {
                try
                {
                    proc();
                }
                catch (NoConnectionException /*ex*/)
                {

                    MessageBoxAdv.Show("Связь с сервером потеряна.\r\nПриложение будет закрыто.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
                    Process.GetCurrentProcess().Kill();
                }
                catch (LabelUsedInPresentationException faultException)
                {
                    //нельзя удалить/редактировать используему метку
                    MessageBoxAdv.Show(
                        String.Format("{0}", faultException.Message),
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch(SystemParametersSaveException saveException)
                {
                    MessageBoxAdv.Show(
                            String.Format("Неудалось сохранить системные параметры. \n  Описание ошибки: {0}", saveException.Message),
                            "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

        }

        public LabelError CRUD(Func<Label, LabelError> proc, Label labelInfo)
        {
            try
            {
                return proc(labelInfo);
            }
            catch (NoConnectionException /*ex*/)
            {
                MessageBoxAdv.Show("Связь с сервером потеряна.\r\nПриложение будет закрыто.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Process.GetCurrentProcess().Kill();
            }
         
            return LabelError.NoError;
        }

        public UserError CRUD(Func<UserInfo, UserError> proc, UserInfo userInfo)
        {
            try
            {
                return proc(userInfo);
            }
            catch (NoConnectionException /*ex*/)
            {

                MessageBoxAdv.Show("Связь с сервером потеряна.\r\nПриложение будет закрыто.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Process.GetCurrentProcess().Kill();
            }

            return UserError.NoError;
        }

    }
}
