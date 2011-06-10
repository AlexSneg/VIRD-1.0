using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain.PresentationDesign.Client;
using Domain.PresentationDesign.DesignCommon;
using DomainServices.ImportExportClientManagement.Presentation;
using DomainServices.ImportExportCommon;
using Syncfusion.Windows.Forms;
using TechnicalServices.Communication.Communication.Client;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.ImportExport.ImportExportUI.Forms;

namespace UI.ImportExport.ImportExportUI.Controllers
{
    public class ExportPresentationController : IExportPresentationController
    {
        private static readonly ExportPresentationController _instance = new ExportPresentationController();
        public static ExportPresentationController Instanse { get { return _instance; } }

        public void Export(PresentationInfo[] presentationInfos)
        {
            ExportPresentation exportPresentation = new ExportPresentation(
                DesignerClient.Instance.ClientConfiguration,
                DesignerClient.Instance.PresentationWorker,
                DesignerClient.Instance.StandalonePresentationWorker,
                this);
            using (SimpleClient<IDesignerService> client = new SimpleClient<IDesignerService>())
            {
                client.Open();
                exportPresentation.Export(presentationInfos, client.Channel);
            }
        }

        #region IExportPresentationController

        public bool ConfirmExport(string directory, string filter, IEnumerable<string> presentationNames, out string newPresentationFileName)
        {
            newPresentationFileName = null;
            using (ExportPresentationForm form = new ExportPresentationForm(directory, filter, presentationNames.ToArray()))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    newPresentationFileName = presentationNames.Count() == 1 ? form.SelectedFile : null;
                    return true;
                }
                return false;
            }
        }

        public IContinue GetUserInteractive(bool onlyOnePresentation)
        {
            return new ImportExportContinue(onlyOnePresentation);
        }

        public void ErrorMessage(string errorMessage)
        {
            MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SuccessMessage(string message)
        {
            MessageBoxAdv.Show(message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}
