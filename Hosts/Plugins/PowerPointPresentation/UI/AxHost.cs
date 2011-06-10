using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Hosts.Plugins.PowerPointPresentation.Common;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

namespace Hosts.Plugins.PowerPointPresentation.UI
{
    public partial class AxHost : UserControl
    {
        private Presentation _presentation;
        private string _fileName;
        private DsoFramerHelper _helper;

        public AxHost()
        {
            InitializeComponent();
        }

        public int CurrentPosition
        {
            get
            {
                if (_presentation != null && _presentation.SlideShowWindow != null)
                    return _presentation.SlideShowWindow.View.CurrentShowPosition;
                return 0;
            }
        }

        public void LoadPresentation(string fileName)
        {
            _fileName = fileName;
        }

        public void GotoSlide(int slide)
        {
            if (_presentation != null && _presentation.SlideShowWindow != null && slide <= _presentation.Slides.Count && slide > 0)
                _presentation.SlideShowWindow.View.GotoSlide(slide, MsoTriState.msoFalse);
        }

        public void Prev()
        {
            if (_presentation != null && _presentation.SlideShowWindow != null)
            {
                _presentation.SlideShowWindow.View.Previous();
            }
        }

        public void Next()
        {
            if (_presentation != null && _presentation.SlideShowWindow != null)
            {
                _presentation.SlideShowWindow.View.Next();
            }
        }

        public void StartSlideShow()
        {
            axFramerControl1.Open(_fileName, true, Type.Missing, Type.Missing, Type.Missing);
            _presentation = axFramerControl1.ActiveDocument as Presentation;
            axFramerControl1.EventsEnabled = true;
            axFramerControl1.Select();
            if (_presentation != null)
            {
                SlideShowSettings slideShowSettings = _presentation.SlideShowSettings;
                slideShowSettings.ShowType = PpSlideShowType.ppShowTypeWindow;
                slideShowSettings.ShowScrollbar = MsoTriState.msoFalse;
                slideShowSettings.LoopUntilStopped = MsoTriState.msoTrue;
                //slideShowSettings.Application.SlideShowBegin += new EApplication_SlideShowBeginEventHandler(Application_SlideShowBegin);
                axFramerControl1.PrintPreview();
                // без задержки не всегда успевают правильно определится тулбары
                Thread.Sleep(500);
                System.Windows.Forms.Application.DoEvents();
                _helper = new DsoFramerHelper(axFramerControl1);
                _helper.Init();
            }
        }

        private void AxHost_SizeChanged(object sender, EventArgs e)
        {
            if (_helper != null)
            {
                _helper.ResizeAxControl();
            }
        }
    }
}
