using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace DomainServices.PresentationManagement
{
    public class ServerSideGroupFileSourceEx : IResourceEx<FilesGroup>
    {
        private readonly string _directory;
        public ServerSideGroupFileSourceEx(string directory)
        {
            _directory = directory;
        }
        #region Implementation of IResourceEx<FilesGroup>

        public bool IsExists(FilesGroup descriptor)
        {
            return File.Exists(Path.Combine(_directory, Path.GetFileName(descriptor.Identity)));
        }

        public bool IsResourceExists(FilesGroup descriptor, string resourceId)
        {
            return File.Exists(
                Path.Combine(_directory, Path.GetFileName(resourceId)));
        }

        public string GetResourceFileName(FilesGroup descriptor, string resourceId)
        {
            return Path.Combine(_directory, Path.GetFileName(resourceId));
        }

        public string GetResourceFileName(FilesGroup descriptor)
        {
            return Path.Combine(_directory, Path.GetFileName(descriptor.Identity));
        }

        public string GetRealResourceFileName(FilesGroup descriptor, string resourceId)
        {
            return GetResourceFileName(descriptor, resourceId);
        }

        public string GetRealResourceFileName(FilesGroup descriptor)
        {
            return GetResourceFileName(descriptor);
        }

        public FilesGroup GetStoredSource(FilesGroup descriptor)
        {
            return new FilesGroup(descriptor.Identity, descriptor.Identity,
                                  descriptor.Files.Select(fp => new FileProperty()
                                  {
                                      FileName =
                                          GetResourceFileName(descriptor,
                                                              fp.FileName),
                                      Length =
                                          new FileInfo(
                                          GetResourceFileName(descriptor,
                                                              fp.FileName)).Length
                                  }));
        }

        /// <summary>
        /// на сервере эти функции не поддерживаются
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public FilesGroup SaveSource(UserIdentity sender, FilesGroup descriptor)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// на сервере эти функции не поддерживаются
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="descriptor"></param>
        /// <param name="fileDic"></param>
        /// <returns></returns>
        public FilesGroup SaveSource(UserIdentity sender, FilesGroup descriptor, Dictionary<string, string> fileDic)
        {
            throw new System.NotImplementedException();

            //foreach (KeyValuePair<string, string> pair in fileDic)
            //{
            //    string tempFile = pair.Value;
            //    string file = GetResourceFileName(descriptor, pair.Key);
            //    if (File.Exists(file))
            //        File.Delete(file);
            //    File.Move(tempFile, file);
            //}
        }

        public List<FilesGroup> SearchByName(FilesGroup descriptor)
        {
            return new List<FilesGroup>();
            //if (!File.Exists(Path.Combine(_directory, Path.GetFileName(descriptor.MainFile)))) return null;
            //FilesGroup other = new FilesGroup();
            //other.Identity = other.MainFile = descriptor.MainFile;
            //foreach (FileProperty fileProperty in descriptor.Files)
            //{
            //    string file = Path.Combine(_directory, Path.GetFileName(fileProperty.FileName));
            //    if (!File.Exists(file)) continue;
            //    other.Files.Add(new FileProperty() { FileName = fileProperty.FileName, Length = new FileInfo(file).Length });
            //}
            //return other;
        }

        #endregion

        public event Action<double, string> OnUploadSpeed;
        public void UploadSpeed(double speed, string display)
        {
            if (OnUploadSpeed != null)
            {
                OnUploadSpeed(speed, display);
            }
        }
    }
}
