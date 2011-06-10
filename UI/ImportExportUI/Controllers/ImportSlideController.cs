using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain.PresentationDesign.Client;
using DomainServices.ImportExportClientManagement.Slide;
using DomainServices.ImportExportCommon;
using Syncfusion.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using UI.ImportExport.ImportExportUI.Forms;

namespace UI.ImportExport.ImportExportUI.Controllers
{
    public class ImportSlideController : IImportSlideController
    {
        private static readonly ImportSlideController _instance = new ImportSlideController();
        public static ImportSlideController Instanse { get { return _instance; } }

        private Func<Slide, Slide> _addSlideDelegate;
        private Func<IEnumerable<Slide>, bool> _addLinkDelegate;
        private Func<string, string, bool> _isSlideUniqueName;


        public void Import(Presentation presentation, ResourceDescriptor[] resourceDescriptors,
            DeviceResourceDescriptor[] deviceResourceDescriptors, 
            Func<Slide, Slide> addSlideDelegate, Func<IEnumerable<Slide>, bool> addLinkDelegate,
            Func<string, string, bool> isSlideUniqueName,
            int indent, int height)
        {
            _addSlideDelegate = addSlideDelegate;
            _addLinkDelegate = addLinkDelegate;
            _isSlideUniqueName = isSlideUniqueName;
            string selectedFile = null;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "XML Files (*.xml) | *.xml";
                openFileDialog.Multiselect = false;
                if (DialogResult.OK == openFileDialog.ShowDialog())
                {
                    selectedFile = openFileDialog.FileName;
                }
            }
            if (string.IsNullOrEmpty(selectedFile)) return;
            ImportSlide importSlide = new ImportSlide(DesignerClient.Instance.StandalonePresentationWorker,
                this);

            importSlide.Import(selectedFile, presentation, resourceDescriptors, deviceResourceDescriptors, indent, height);
        }

        #region Implementation of IImportSlideController

        public void ErrorMessage(string errorMessage)
        {
            MessageBoxAdv.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SuccessMessage(string successMessage)
        {
            MessageBoxAdv.Show(successMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //public void AddSlide(Slide slide, PointF point)
        //{
        //    if (_addSlideDelegate != null)
        //        _addSlideDelegate.Invoke(slide);
        //}

        public void AddSlide(Slide slide)
        {
            if (_addSlideDelegate != null)
                _addSlideDelegate.Invoke(slide);
        }

        public void AddLink(Slide slideFrom, Slide slideTo)
        {
            if (_addLinkDelegate != null)
            {
                if (!_addLinkDelegate.Invoke(new[] { slideFrom, slideTo }))
                    throw new ApplicationException(string.Format("Не удалось создать линк между сценами: {0} и {1}",
                        slideFrom.Name, slideTo.Name));
            }
        }

        public bool GetNewName(string message, out string newName)
        {
            newName = string.Empty;
            DialogResult result;
            using (WrongSlideName wrongSlideName = new WrongSlideName(message))
            {
                if (DialogResult.OK == (result = wrongSlideName.ShowDialog()))
                {
                    newName = wrongSlideName.SlideName;
                }
            }
            return result == DialogResult.OK;
        }

        public bool IsSlideUniqueName(string name, string exceptOne)
        {
            if (_isSlideUniqueName != null)
            {
                return _isSlideUniqueName.Invoke(name, exceptOne);
            }
            return true;
        }

        #endregion
    }
}
