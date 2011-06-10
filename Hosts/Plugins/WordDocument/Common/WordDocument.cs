using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;


namespace Hosts.Plugins.WordDocument.Common
{
    public class WordDocument : IDisposable
    {
        private Microsoft.Office.Interop.Word.Application _application;
        private Document _document;
        //private Presentations _presentations;
        //private Presentation _presentation;
        //private Slides _slides;
        //private Slide _slide;
        private bool _disposed = false;

        public static WordDocument OpenFile(string fileName)
        {
            return new WordDocument(fileName);
        }

        private WordDocument(string fileName)
        {
            //string appProgID = "Word.Application";
            //Type docType = Type.GetTypeFromProgID(appProgID);
            //_application = (Application)Activator.CreateInstance(docType, true);
           
            ////_application.Visible = MsoTriState.msoTrue;

            ////TO DO: Метод Open
            ////_presentations = _application.Presentations;
            ////_presentation = _presentations.Open(fileName,
            ////                                    MsoTriState.msoTrue,
            ////                                    MsoTriState.msoTrue,
            ////                                    MsoTriState.msoFalse);
            object objFileName = fileName;
            object falseValue = false;
            object trueValue = true;
            object missing = Type.Missing;
            _application = new Microsoft.Office.Interop.Word.Application();
            _document = _application.Documents.Open(ref objFileName, ref missing, ref trueValue,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing);
        }

        public int NumberOfPages
        {
            get
            {
                if (_document != null)
                {
                    return _document.ActiveWindow.ActivePane.Pages.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        ~WordDocument()
        {
            Dispose();
        }

        public void Dispose()
        {
            object missing = Type.Missing;

            if (_disposed) return;

            //if (_slide != null)
            //{
            //    Marshal.ReleaseComObject(_slide);
            //    _slide = null;
            //}

            //if (_slides != null)
            //{
            //    Marshal.ReleaseComObject(_slides);
            //    _slide = null;
            //}

            //if (_presentation != null)
            //{
            //    _presentation.Close();
            //    Marshal.ReleaseComObject(_presentation);
            //    _presentation = null;
            //}

            //if (_presentations != null)
            //{
            //    Marshal.ReleaseComObject(_presentations);
            //    _presentations = null;
            //}

            if (_document != null)
            {
                _document.Close(ref missing, ref missing, ref missing);
                Marshal.ReleaseComObject(_document);
                _document = null;
            }

            if (_application != null)
            {
                //TO DO: Метод Quit() с параметрами
                //_application.Quit();
                _application.Quit(ref missing, ref missing, ref missing);
                Marshal.ReleaseComObject(_application);
                _application = null;
            }
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        //public void Dispose()
        //{ }
    }
}