using System;
using System.Collections.Generic;

namespace TechnicalServices.Entity
{
    [Serializable]
    public class FilesGroup : IEquatable<FilesGroup>, IId
    {
        private readonly List<FileProperty> _files = new List<FileProperty>();

        public FilesGroup(){}

        public FilesGroup(string identity, string mainFile, IEnumerable<FileProperty> files)
        {
            Identity = identity;
            MainFile = mainFile;
            Files.AddRange(files);
        }

        public string Identity;
        public string MainFile;
        public List<FileProperty> Files
        {
            get { return _files; }
        }

        #region Implementation of IEquatable<FilesGroup>

        public bool Equals(FilesGroup other)
        {
            return this.Identity.Equals(other.Identity, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Implementation of IId

        public string Id
        {
            get { return Identity; }
            set { Identity = value; }
        }

        #endregion
    }

    [Serializable]
    public class FileProperty
    {
        public string FileName;
        public long Length;
    }
}
