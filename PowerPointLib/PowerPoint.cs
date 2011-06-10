using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;


namespace TechnicalServices.PowerPointLib
{
    public class PowerPoint : IDisposable
    {
        private Application _application;
        private Presentations _presentations;
        private Presentation _presentation;
        private Slides _slides;
        private Slide _slide;
        private bool _disposed = false;
        private string _fileName;
        // PowwerPoint не может создавать изображения из слайдов с большим размером по большей стороне
        private const int MAGIC_NUMBER = 3072;

        public static PowerPoint OpenFile(string fileName)
        {
            return new PowerPoint(fileName);
        }

        private PowerPoint(string fileName)
        {
            string appProgID = "PowerPoint.Application";
            Type pptType = Type.GetTypeFromProgID(appProgID);
            _application = (Application)Activator.CreateInstance(pptType, true);
            _fileName = fileName;
            //_application.Visible = MsoTriState.msoTrue;
            _presentations = _application.Presentations;
            _presentation = _presentations.Open(fileName,
                                                MsoTriState.msoTrue,
                                                MsoTriState.msoTrue,
                                                MsoTriState.msoFalse);
        }

        ~PowerPoint()
        {
            Dispose();
        }

        public void SaveSlidesToFolder(string folder)
        {
            if (_presentation == null)
                throw new NullReferenceException("PowerPoint Presentation is null");
            _presentation.SaveAs(folder, PpSaveAsFileType.ppSaveAsPNG, MsoTriState.msoTrue);
        }

        public bool SaveSlideToFile(int slideNumber, string fileName, int width, int height)
        {
            if (_presentation == null)
                throw new NullReferenceException("PowerPoint Presentation is null");
            if (slideNumber > _presentation.Slides.Count) return false;
            //_presentation.Slides[slideNumber].Select();
            //_presentation.SaveAs(fileName, PpSaveAsFileType.ppSaveAsPNG, MsoTriState.msoTrue);
            _slide = _presentation.Slides[slideNumber];
            if (_slide == null) return false;
            MethodInfo mi = _slide.GetType().GetMethod("Export");
            if ((width == -1) || (height == -1))
                mi.Invoke(_slide, new object[] { fileName, "PNG", Missing.Value, Missing.Value });
            else
            {
                // надо ввсети ограничение, чтобы не было больше MAGIC_NUMBER, если будет больше, PPT 2007 хреново работает
                int new_width = width;
                int new_height = height;
                int maxSize = Math.Max(new_width, new_height);
                if (maxSize > MAGIC_NUMBER)
                {
                    new_width = (int)(MAGIC_NUMBER/(maxSize*1.0)*new_width);
                    new_height = (int) (MAGIC_NUMBER/(maxSize*1.0)*new_height);
                }
                mi.Invoke(_slide, new object[] {fileName, "PNG", new_width, new_height});
            }
            //_presentation.Slides[slideNumber].Export(fileName, "PNG", Missing.Value, Missing.Value);
            return true;
        }

        public string[] SaveSlideAsPptFiles(string folder)
        {
            if (_presentation == null)
                throw new NullReferenceException("PowerPoint Presentation is null");
            int countOfSlides = _presentation.Slides.Count;
            List<string> files = new List<string>(countOfSlides);
            string tempFile = Path.Combine(folder, Path.ChangeExtension(Path.GetRandomFileName(),".ppt"));
            File.Copy(_fileName, tempFile, true);
            try
            {
                for (int i = 1; i <= countOfSlides; i++)
                {
                    using (PowerPoint powerPoint = PowerPoint.OpenFile(tempFile))
                    {
                        files.Add(powerPoint.SaveOnlyOneSlide(i, folder));
                    }
                }
                return files.ToArray();
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        public string SaveOnlyOneSlide(int slideNumber, string folder)
        {
            int j = 1;
            int count = _presentation.Slides.Count;
            for (int i = 1; i <= count; i++)
            {
                if (i == slideNumber)
                {
                    j++;
                    continue;
                }
                //_presentation.Slides[i].Select();
                _presentation.Slides[j].Delete();
            }
            string fileName = Path.Combine(folder, string.Format("{0}_Slide{1}.ppt",
                Path.GetFileNameWithoutExtension(_fileName), slideNumber));
            _presentation.SaveAs(fileName, PpSaveAsFileType.ppSaveAsPresentation, MsoTriState.msoFalse);
            return fileName;
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
