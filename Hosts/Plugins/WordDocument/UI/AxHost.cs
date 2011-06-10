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
using Hosts.Plugins.WordDocument.Common;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
//using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.WordDocument.UI
{
    public partial class AxHost : UserControl
    {
        //private Presentation _presentation;
        private Microsoft.Office.Interop.Word.Document _document;
        private string _fileName;
        private DsoFramerHelper _helper;
        private int line = 0;
        private int currentLine = 0;

        public AxHost()
        {
            InitializeComponent();
        }

        public int CurrentPosition
        {
            get
            {
                int currentPosition = 0;
                object objPosition = null;
                if (_document != null)
                {
                    objPosition = _document.ActiveWindow.ActivePane.Selection.get_Information(WdInformation.wdActiveEndAdjustedPageNumber);
                    int.TryParse(objPosition.ToString(), out currentPosition);
                }
                return currentPosition;
            }
        }

        public int CurrentZoom
        {
            get 
            {
                if (_document != null)
                {
                    return _document.ActiveWindow.View.Zoom.Percentage;
                }
                return 0;
            }
        }

        public void LoadPresentation(string fileName)
        {
            _fileName = fileName;
        }

        public void GoToPage(int page)
        {
            if (_document != null && page <= Convert.ToInt32(_document.ActiveWindow.ActivePane.Pages.Count.ToString()) && page > 0)
            {
                object wdGoToPage = WdGoToItem.wdGoToPage;
                object wdGoToNext = WdGoToDirection.wdGoToNext;
                object wdGoToPrev = WdGoToDirection.wdGoToPrevious;
                object count = 0;
                object name = "";
                int currentPosition = 0;
                
                object objPosition = _document.ActiveWindow.ActivePane.Selection.get_Information(WdInformation.wdActiveEndAdjustedPageNumber);
                int.TryParse(objPosition.ToString(), out currentPosition);
                if (page > currentPosition)
                {
                    count = page - currentPosition;
                    _document.ActiveWindow.Selection.GoTo(ref wdGoToPage, ref wdGoToNext, ref count, ref name);
                }
                else if (page < currentPosition)
                {
                    count = currentPosition - page;
                    _document.ActiveWindow.Selection.GoTo(ref wdGoToPage, ref wdGoToPrev, ref count, ref name);
                }

            }
                
        }

        public void Prev()
        {
            if (_document != null)
            {
                object down = 0;
                object up = 10;
                object left = 0;
                object right = 0;
                _document.ActiveWindow.ActivePane.SmallScroll(ref down, ref up, ref right, ref left);
            }
        }

        public void Next()
        {
            if (_document != null)
            {
                object down = 10;
                object up = 0;
                object left = 0;
                object right = 0;
                _document.ActiveWindow.ActivePane.SmallScroll(ref down, ref up, ref right, ref left);
            }
        }

        public void LeftScroll()
        {
            if (_document != null)
            {
                object down = 0;
                object up = 0;
                object left = 3;
                object right = 0;
                _document.ActiveWindow.ActivePane.SmallScroll(ref down, ref up, ref right, ref left);
            }
        }

        public void RightScroll()
        {
            if (_document != null)
            {
                object down = 0;
                object up = 0;
                object left = 0;
                object right = 3;
                _document.ActiveWindow.ActivePane.SmallScroll(ref down, ref up, ref right, ref left);
            }
        }
        
        public void PrevPage()
        {
            if (_document != null)
            {
                //object down = 0;
                //object up = 1;
                //_document.ActiveWindow.ActivePane.PageScroll(ref down, ref up);
                object wdGoToPage = WdGoToItem.wdGoToPage;
                object wdGoToPrev = WdGoToDirection.wdGoToPrevious;
                object count = 1;
                object name = "";
                _document.ActiveWindow.Selection.GoTo(ref wdGoToPage, ref wdGoToPrev, ref count, ref name);
            }
        }

        public void NextPage()
        {
            if (_document != null)
            {
                //object down = 1;
                //object up = 0;
                //_document.ActiveWindow.ActivePane.PageScroll(ref down, ref up);
                object wdGoToPage = WdGoToItem.wdGoToPage;
                object wdGoToNext = WdGoToDirection.wdGoToNext;
                object count = 1;
                object name = "";
                _document.ActiveWindow.Selection.GoTo(ref wdGoToPage, ref wdGoToNext, ref count, ref name);
            }
        }

        public void LastPage()
        {
            if (_document != null)
            {
                int count = 0;
                int.TryParse(_document.ActiveWindow.ActivePane.Pages.Count.ToString(), out count);
                GoToPage(count);
            }
        }

        public void FirstPage()
        {
            if (_document != null)
            {
                GoToPage(1);
            }
        }

        public void Zoom(int scale)
        {
            if(_document != null)
            {
                _document.ActiveWindow.View.Zoom.Percentage = scale;
            }
        }

        public void StartDocumentShow()
        {
            object missing = System.Reflection.Missing.Value;
            object readOnly = true;

            axFramerControl1.EventsEnabled = true;
            System.Windows.Forms.Application.DoEvents();

            axFramerControl1.Open(_fileName, readOnly, missing, missing, missing);

            //axFramerControl1.Open(_fileName, true, Type.Missing, Type.Missing, Type.Missing);
            //_presentation = axFramerControl1.ActiveDocument as Presentation;

            //_document = axFramerControl1.ActiveDocument as Microsoft.Office.Interop.Word.DocumentClass;
            _document = (Microsoft.Office.Interop.Word.Document)axFramerControl1.ActiveDocument;

            //TO DO: Удалить
            //axFramerControl1.EventsEnabled = true;
            //axFramerControl1.Select();
            if (_document != null)
            {
                //SlideShowSettings slideShowSettings = _presentation.SlideShowSettings;
                //slideShowSettings.ShowType = PpSlideShowType.ppShowTypeWindow;
                //slideShowSettings.ShowScrollbar = MsoTriState.msoFalse;
                //slideShowSettings.LoopUntilStopped = MsoTriState.msoTrue;

                //TO DO: Удалить
                // без задержки не всегда успевают правильно определится тулбары
                //Thread.Sleep(500);
                //System.Windows.Forms.Application.DoEvents();
                //_helper = new DsoFramerHelper(axFramerControl1);
                //_helper.Init();

                _document.PrintPreview();

                _document.Application.DisplayScrollBars = true;
                _document.Application.ActiveWindow.DisplayRulers = false;

                _document.CommandBars["Print Preview"].Enabled = false;
                _document.CommandBars["Print Preview"].Visible = false;

                if (StartPage != 1)
                    GoToPage(StartPage);
                if (StartLine != 1)
                {

                    object objCurrentLine = _document.ActiveWindow.ActivePane.Selection.get_Information(WdInformation.wdFirstCharacterLineNumber);
                    int.TryParse(objCurrentLine.ToString(), out currentLine);
                    line = currentLine + StartLine;

                    object wdGoToLine = WdGoToItem.wdGoToLine;
                    object wdGoToNext = WdGoToDirection.wdGoToNext;
                    object count = line;
                    object name = "";
                    //_document.ActiveWindow.Selection.GoTo(ref wdGoToLine, ref wdGoToNext, ref count, ref name);
                    _document.ActiveWindow.ActivePane.Pages[StartPage].Application.Selection.GoTo(ref wdGoToLine, ref wdGoToNext, ref count, ref name);
                }

                Zoom(StartZoom);
            }
        }


        private void AxHost_SizeChanged(object sender, EventArgs e)
        {
            if (_helper != null)
            {
                _helper.ResizeAxControl();
            }
        }

        public int StartPage
        { get; set; }

        public int StartLine
        { get; set; }

        public int StartZoom
        { get; set; }
    }
}
