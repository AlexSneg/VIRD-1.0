using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EquipmentManagement.AgentCommon;
using TechnicalServices.PowerPointLib;
using TechnicalServices.Interfaces;

namespace DomainServices.EquipmentManagement.AgentManagement
{
    public class BackgroundProvider : IBackgroundProvider
    {
        private const string _pptExt = ".ppt";
        private FileInfo _previousImageFileInfo;
        private readonly Dictionary<string, string> _fileTemps = new Dictionary<string, string>();
        private readonly Dictionary<string, FileInfo> _fileDesc = new Dictionary<string, FileInfo>();
        private bool _disposed = false;

        public void SetBackgroundImage(string imageFileName, Control control)
        {
            if (string.IsNullOrEmpty(imageFileName) || !File.Exists(imageFileName))
            {
                SetNewBackground(control, null);
                return;
            }
            // если имидж не изменился, то не надо ничего менять
            if (IsPreviousFile(imageFileName)) return;
            //if (!IsFileChange(imageFileName)) return;
            // если простой имидж то его и загружаем, если PowerPoint, то вытаскиваем из него
            if (IsPowerPointPresentation(imageFileName))
            {
                string file;
                if (IsFileChange(imageFileName))
                {
                    file = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    //if (!Directory.Exists(file))
                    //    Directory.CreateDirectory(file);
                    bool isSuccess;
                    using (PowerPoint powerPoint = PowerPoint.OpenFile(imageFileName))
                    {
                        isSuccess = powerPoint.SaveSlideToFile(1, file);
                    }
                    if (!isSuccess) return;
                    _fileTemps[imageFileName] = file;
                }
                else
                {
                    file = _fileTemps[imageFileName];
                }
                LoadImageFromFile(control, file);
            }
            else
            {
                LoadImageFromFile(control, imageFileName);
            }
        }

        #region Тупой копипаст для IBackgroundSupport
        public void SetBackgroundImage(string imageFileName, IBackgroundSupport control)
        {
            if (string.IsNullOrEmpty(imageFileName) || !File.Exists(imageFileName))
            {
                SetNewBackground(control, null);
                _previousImageFileInfo = null;
                return;
            }
            // если имидж не изменился, то не надо ничего менять
            if (IsPreviousFile(imageFileName)) return;
            //if (!IsFileChange(imageFileName)) return;
            // если простой имидж то его и загружаем, если PowerPoint, то вытаскиваем из него
            if (IsPowerPointPresentation(imageFileName))
            {
                string file;
                if (IsFileChange(imageFileName))
                {
                    file = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    //if (!Directory.Exists(file))
                    //    Directory.CreateDirectory(file);
                    bool isSuccess;
                    using (PowerPoint powerPoint = PowerPoint.OpenFile(imageFileName))
                    {
                        isSuccess = powerPoint.SaveSlideToFile(1, file);
                    }
                    if (!isSuccess) return;
                    _fileTemps[imageFileName] = file;
                }
                else
                {
                    file = _fileTemps[imageFileName];
                }
                LoadImageFromFile(control, file);
            }
            else
            {
                LoadImageFromFile(control, imageFileName);
            }
        }

        void LoadImageFromFile(IBackgroundSupport control, string file)
        {
            Image image;
            try
            {
                image = Image.FromFile(file);
            }
            catch (Exception)
            {
                image = null;
                return;
            }
            SetNewBackground(control, image);
        }

        private void SetNewBackground(IBackgroundSupport control, Image image)
        {
            if (control.Background != null)
            {
                control.Background.Dispose();
            }

            control.Background = image;
        }

        #endregion

        private void LoadImageFromFile(Control control, string file)
        {
            Image image;
            try
            {
                image = Image.FromFile(file);
            }
            catch (Exception)
            {
                image = null;
                return;
            }
            SetNewBackground(control, image);
        }

        private void SetNewBackground(Control control, Image image)
        {
            if (control.BackgroundImage != null)
            {
                control.BackgroundImage.Dispose();
            }
            control.BackgroundImage = image;
        }

        private bool IsPreviousFile(string imageFileName)
        {
            FileInfo currentFileInfo = new FileInfo(imageFileName);
            if (_previousImageFileInfo == null
                ||
                !_previousImageFileInfo.Name.Equals(currentFileInfo.Name, StringComparison.InvariantCultureIgnoreCase)
                ||
                !_previousImageFileInfo.CreationTime.Equals(currentFileInfo.CreationTime)
                ||
                !_previousImageFileInfo.LastWriteTime.Equals(currentFileInfo.LastWriteTime))
            {
                _previousImageFileInfo = currentFileInfo;
                return false;
            }
            return true;
        }

        private bool IsFileChange(string imageFileName)
        {
            FileInfo currentFileInfo = new FileInfo(imageFileName);
            FileInfo oldFileInfo;
            _fileDesc.TryGetValue(imageFileName, out oldFileInfo);

            if (oldFileInfo == null
            ||
            !oldFileInfo.Name.Equals(currentFileInfo.Name, StringComparison.InvariantCultureIgnoreCase)
            ||
            !oldFileInfo.CreationTime.Equals(currentFileInfo.CreationTime)
            ||
            !oldFileInfo.LastWriteTime.Equals(currentFileInfo.LastWriteTime))
            {
                _fileDesc[imageFileName] = currentFileInfo;
                return true;
            }
            return false;
        }


        private bool IsPowerPointPresentation(string fileName)
        {
            return Path.GetExtension(fileName).ToLower().Equals(_pptExt,
                                                         StringComparison.InvariantCultureIgnoreCase);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        ~BackgroundProvider()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                foreach (string file in _fileTemps.Values)
                {
                    File.Delete(file);
                    //if (Directory.Exists(directory))
                    //    Directory.Delete(directory, true);
                }
            }
        }

    }
}
