using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain.PresentationDesign.Client;
using DomainServices.ImportExportClientManagement.Slide;
using DomainServices.ImportExportCommon;
using Syncfusion.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace UI.ImportExport.ImportExportUI.Controllers
{
    public class ExportSlideController : IExportSlideController
    {
        private static readonly ExportSlideController _instance = new ExportSlideController();
        public static ExportSlideController Instanse { get { return _instance; } }

        public void Export(Presentation presentation, Slide[] slides)
        {
            string selectedFile = null;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = "XML Files (*.xml) | *.xml";
                if (DialogResult.OK == saveFileDialog.ShowDialog())
                {
                    selectedFile = saveFileDialog.FileName;
                }
            }
            if (string.IsNullOrEmpty(selectedFile)) return;

            ExportSlide exportSlide = new ExportSlide(DesignerClient.Instance.PresentationWorker,
                DesignerClient.Instance.StandalonePresentationWorker, this);

            exportSlide.Export(selectedFile, new PresentationInfo(presentation), slides);
        }

        public IContinue GetUserInterActive(bool onlyOneSlide)
        {
            return new ImportExportContinue(onlyOneSlide);
        }

        public void ErrorMessage(string errorMessage)
        {
            MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SuccessMessage(string message)
        {
            MessageBoxAdv.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
