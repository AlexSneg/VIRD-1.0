using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;

namespace TechnicalServices.Util.FileTransfer
{
    internal class GroupFileInfoProvider : IFileInfoProvider<FileProperty>
    {
        private readonly FilesGroup _filesGroup;
        private readonly string _directory;
        public GroupFileInfoProvider(string directory, FilesGroup filesGroup)
        {
            _filesGroup = filesGroup;
            _directory = directory;
        }
        #region Implementation of IFileInfoProvider

        public string GetResourceId(FileProperty property)
        {
            return property.FileName;
        }

        public string Identity
        {
            get { return UniqueName; }
        }

        public string UniqueName
        {
            get { return _filesGroup.Identity; }
        }

        public string GetFileName(string fileId)
        {
            if (Path.GetFileName(_filesGroup.Identity).Equals(Path.GetFileName(fileId)))
                return Path.Combine(_directory, Path.GetFileName(_filesGroup.MainFile));
            return Path.Combine(_directory, Path.GetFileName(fileId));
        }

        public void SaveFile(UserIdentity userIdentity, Dictionary<string, string> fileDic)
        {
            foreach (KeyValuePair<string, string> pair in fileDic)
            {
                string tempFile = pair.Value;
                string file = GetFileName(pair.Key);
                if (File.Exists(file))
                    File.Delete(file);
                File.Move(tempFile, file);
            }
        }

        public int GetNumberOfParts(string resourceId)
        {
            return (int)(_filesGroup.Files.Find(rfp => Path.GetFileName(rfp.FileName).Equals(Path.GetFileName(resourceId))).Length / (Constants.PartSize + 1)) + 1;
        }

        public int GetNumberOfParts()
        {
            return (int)_filesGroup.Files.Sum(fp => fp.Length / (Constants.PartSize + 1) + 1);
        }

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<FileProperty> GetEnumerator()
        {
            return _filesGroup.Files.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
