using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain.PresentationDesign.Client;
using Domain.PresentationDesign.DesignCommon;
using DomainServices.ImportExportClientManagement.Configuration;
using Syncfusion.Windows.Forms;
using TechnicalServices.Communication.Communication.Client;
using UI.ImportExport.ImportExportUI.Forms;

namespace UI.ImportExport.ImportExportUI.Controllers
{
    public class ExportConfigurationController
    {
        private static readonly ExportConfigurationController _instance = new ExportConfigurationController();
        public static ExportConfigurationController Instanse { get { return _instance; } }

        public void Export()
        {
            ExportConfiguration exportConfiguration = new ExportConfiguration(
                DesignerClient.Instance.ClientConfiguration,
                DesignerClient.Instance.StandalonePresentationWorker,
                DesignerClient.Instance.PresentationWorker,
                SuccessMessage, ErrorMessage,
                GetFileNameForConfiguration);
            using (SimpleClient<IDesignerService> client = new SimpleClient<IDesignerService>())
            {
                client.Open();
                exportConfiguration.Export(client.Channel);
            }
        }

        private static string GetFileNameForConfiguration(string directory, string filter)
        {
            string fileName = null;
            using (ExportConfigurationForm form = new ExportConfigurationForm(directory, filter, "LabelStorage.xml"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    fileName = form.SelectedFile;
                }
            }
            return fileName;
        }

        private static void ErrorMessage(string errorMessage)
        {
            MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void SuccessMessage(string message)
        {
            MessageBoxAdv.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
