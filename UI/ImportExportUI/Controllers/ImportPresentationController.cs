using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Domain.PresentationDesign.Client;
using DomainServices.ImportExportClientManagement.Presentation;
using DomainServices.ImportExportCommon;
using Syncfusion.Windows.Forms;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.ImportExport.ImportExportUI.Forms;

namespace UI.ImportExport.ImportExportUI.Controllers
{
    public class ImportPresentationController : IImportPresentationController
    {
        private static readonly ImportPresentationController _instance = new ImportPresentationController();
        public static ImportPresentationController Instanse { get { return _instance; } }

        private Func<string, DialogResult> _resourceExistsDialogDelegate;

        public void Import(Func<string, DialogResult> resourceExistsDialogDelegate)
        {
            _resourceExistsDialogDelegate = resourceExistsDialogDelegate;
            string[] selectedFiles = null;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "XML Files (*.xml) | *.xml";
                openFileDialog.InitialDirectory = DesignerClient.Instance.ClientConfiguration.ScenarioFolder;
                openFileDialog.Multiselect = true;
                if (DialogResult.OK == openFileDialog.ShowDialog())
                {
                    selectedFiles = openFileDialog.FileNames;
                }
            }
            if (selectedFiles != null && selectedFiles.Length != 0)
            {
                ImportPresentation importPresentation = new ImportPresentation(
                    DesignerClient.Instance.ClientConfiguration,
                    DesignerClient.Instance.PresentationWorker,
                    DesignerClient.Instance.StandalonePresentationWorker,
                    this
                    //GetUserInteractive,
                    //ErrorMessage,
                    //SuccessMessage,
                    //GetHowImport
                    );
                importPresentation.OnPresentationAdded += importPresentation_OnPresentationAdded;
                importPresentation.OnPresentationDeleted += importPresentation_OnPresentationDeleted;
                importPresentation.Import(selectedFiles);
            }
        }

        static void importPresentation_OnPresentationDeleted(PresentationInfo presentationInfo)
        {
            DesignerClient.Instance.PresentationNotifier.PresentationDeleted(
                Thread.CurrentPrincipal as UserIdentity,
                new PresentationInfoExt(presentationInfo, null));
        }

        static void importPresentation_OnPresentationAdded(TechnicalServices.Persistence.SystemPersistence.Presentation.PresentationInfo presentationInfo)
        {
            DesignerClient.Instance.PresentationNotifier.PresentationAdded(
                Thread.CurrentPrincipal as UserIdentity,
                new PresentationInfoExt(presentationInfo, null));
        }

        public IContinue GetUserInterActive(bool onlyOnePresentation)
        {
            return new ImportExportContinue(onlyOnePresentation);
        }

        public bool ExistsSameSource(string message, out SameSourceAction action)
        {
            action = SameSourceAction.Replace;
            DialogResult dialogResult = _resourceExistsDialogDelegate.Invoke(message);
            if (DialogResult.Cancel == dialogResult) return false;
            else if (DialogResult.Yes == dialogResult)
            {
                action = SameSourceAction.Copy;
            }
            else if (DialogResult.No == dialogResult)
            {
                action = SameSourceAction.Replace;
            }
            return true;
        }

        public void ErrorMessage(string errorMessage)
        {
            MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SuccessMessage(string message)
        {
            MessageBoxAdv.Show(message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public HowImport HowToImportRequest(string message)
        {
            DialogResult dialogResult = PresentationExistsDialog.Show(message);
            if (DialogResult.Yes == dialogResult)
            {
                return HowImport.New;
            }
            else if (DialogResult.No == dialogResult)
            {
                return HowImport.Replace;
            }
            return HowImport.Cancel;
        }

    }
}
