using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;


namespace Hosts.Plugins.PowerPointPresentation.Common
{
    public class PowerPoint : IDisposable
    {
        private Application _application;
        private Presentations _presentations;
        private Presentation _presentation;
        private Slides _slides;
        private Slide _slide;
        private bool _disposed = false;

        public static PowerPoint OpenFile(string fileName)
        {
            return new PowerPoint(fileName);
        }

        private PowerPoint(string fileName)
        {
            string appProgID = "PowerPoint.Application";
            Type pptType = Type.GetTypeFromProgID(appProgID);
            _application = (Application)Activator.CreateInstance(pptType, true);
            //_application.Visible = MsoTriState.msoTrue;
            _presentations = _application.Presentations;
            _presentation = _presentations.Open(fileName,
                                                MsoTriState.msoTrue,
                                                MsoTriState.msoTrue,
                                                MsoTriState.msoFalse);
        }

        public int NumberOfSlides
        {
            get
            {
                if (_presentation != null)
                {
                    return _presentation.Slides.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        ~PowerPoint()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;

            if (_slide != null)
            {
                Marshal.ReleaseComObject(_slide);
                _slide = null;
            }

            if (_slides != null)
            {
                Marshal.ReleaseComObject(_slides);
                _slide = null;
            }

            if (_presentation != null)
            {
                _presentation.Close();
                Marshal.ReleaseComObject(_presentation);
                _presentation = null;
            }

            if (_presentations != null)
            {
                Marshal.ReleaseComObject(_presentations);
                _presentations = null;
            }

            if (_application != null)
            {
                _application.Quit();
                Marshal.ReleaseComObject(_application);
                _application = null;
            }
            _disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}