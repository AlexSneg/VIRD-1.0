using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using UI.PresentationDesign.DesignUI.Helpers;
using System.IO;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class PowerPointForm : Office2007Form
    {
        private DateTime _lastModifyTime = DateTime.MinValue;
        //private const double _default_width = 720.16d;

        string _docPath;
        public string DocumentPath
        {
            get { return _docPath; }
        }

        public bool Changed { get; set; }

        string currPath;

        public PowerPointForm()
        {
            InitializeComponent();
        }
        
        int displayWidth;
        int displayHeight;

        public PowerPointForm(int width, int height)
        {
            InitializeComponent();

            displayWidth = width;
            displayHeight = height;

            currPath = Environment.CurrentDirectory;
            framerControl.ActivationPolicy = DSOFramer.dsoActivationPolicy.dsoKeepUIActiveOnAppDeactive;
            framerControl.BorderStyle = DSOFramer.dsoBorderStyle.dsoBorderNone;
            framerControl.set_EnableFileCommand(DSOFramer.dsoFileCommandType.dsoFileSaveAs, false);
            framerControl.set_EnableFileCommand(DSOFramer.dsoFileCommandType.dsoFileNew, false);
            framerControl.set_EnableFileCommand(DSOFramer.dsoFileCommandType.dsoFileClose, false);
            //framerControl.set_EnableFileCommand(DSOFramer.dsoFileCommandType.dsoFilePageSetup, false);
            framerControl.set_EnableFileCommand(DSOFramer.dsoFileCommandType.dsoFilePrint, false);
            framerControl.set_EnableFileCommand(DSOFramer.dsoFileCommandType.dsoFilePrintPreview, false);
            framerControl.EventsEnabled = true;
            framerControl.Select();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Invalidate();
        }

        public void AssignDocument(string path)
        {
            _docPath = Path.ChangeExtension(Path.GetTempFileName(), ".ppt");     //path;
            File.Copy(path, _docPath, true);
            _lastModifyTime = File.GetLastWriteTime(_docPath);
            framerControl.Open(_docPath);
        }

        public void CreateDocument()
        {
            string appProgID = "PowerPoint.Show";
            framerControl.CreateNew(appProgID);
            //PowerPoint.Presentation pres = ((PowerPoint.Presentation)framerControl.ActiveDocument);
            //pres.PageSetup.SlideSize = PowerPoint.PpSlideSizeType.ppSlideSizeCustom;
            //pres.PageSetup.SlideWidth = 1;
            //pres.PageSetup.SlideHeight = 1000;
            object obj = framerControl.ActiveDocument;
            object pageSetup = obj.GetType().InvokeMember("PageSetup", System.Reflection.BindingFlags.GetProperty, null, obj, new object[0]);
            float defaultWidth = (float)pageSetup.GetType().InvokeMember("SlideWidth",
                                                                     System.Reflection.BindingFlags.GetProperty, null,
                                                                     pageSetup, new object[0]);
            float newHeight = displayHeight / (displayWidth * 1.0f) * defaultWidth;

            pageSetup.GetType().InvokeMember("SlideWidth", System.Reflection.BindingFlags.SetProperty, null, pageSetup, new object[] { defaultWidth });
            pageSetup.GetType().InvokeMember("SlideHeight", System.Reflection.BindingFlags.SetProperty, null, pageSetup, new object[] { newHeight });   //displayHeight
        }

        private void framerControl_OnDocumentClosed(object sender, EventArgs e)
        {
            //nop
        }

        private void framerControl_OnSaveCompleted(object sender, AxDSOFramer._DFramerCtlEvents_OnSaveCompletedEvent e)
        {
            _docPath = e.fullFileLocation;
            Changed = true;
        }

        private void framerControl_OnFileCommand(object sender, AxDSOFramer._DFramerCtlEvents_OnFileCommandEvent e)
        {
            if (e.item == DSOFramer.dsoFileCommandType.dsoFileSaveAs || e.item == DSOFramer.dsoFileCommandType.dsoFileSave)
            {
                Changed = true;
                SaveDocument();
                e.cancel = true;
            }
        }

        private void PowerPointForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Changed)
                SaveDocument();
            else
            {
                if (framerControl.IsDirty)
                {
                    switch (MessageBoxExt.Show("Сохранить изменения?", "Вопрос", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            SaveDocument();
                            e.Cancel = false;
                            break;
                        case DialogResult.No: e.Cancel = false; break;
                        case DialogResult.Cancel: e.Cancel = true; break;
                    }
                }
            }

            Environment.CurrentDirectory = currPath;
        }

        private void SaveDocument()
        {
            if (string.IsNullOrEmpty(_docPath))
                _docPath = Path.ChangeExtension(Path.GetTempFileName(), ".ppt");
                //string file = Path.GetTempFileName() + ".ppt";
            framerControl.Save(_docPath, true, null, null);
        }

        private void framerControl_OnDocumentOpened(object sender, AxDSOFramer._DFramerCtlEvents_OnDocumentOpenedEvent e)
        {
            if (e.file != _docPath)
            {
                _docPath = e.file;
                Changed = true;
            }
        }

        private void PowerPointForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Changed && !string.IsNullOrEmpty(_docPath) && _lastModifyTime != DateTime.MinValue)
            {
                Changed = !_lastModifyTime.Equals(File.GetLastWriteTime(_docPath));
            }
        }
    }
}
