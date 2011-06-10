using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Util.FileTransfer
{
    public class ServerSideGroupFileTransfer : ServerSideAbstractTransfer<FilesGroup>
    {
        private readonly string _directory;
        public ServerSideGroupFileTransfer(string directory, IResourceEx<FilesGroup> resourceEx) : base(resourceEx)
        {
            _directory = directory;
        }

        #region Overrides of ServerSideAbstractTransfer<FilesGroup>

        protected override IFileInfoProvider GetFileInfoProvider(FilesGroup resource, FilesGroup stored)
        {
            return new GroupFileInfoProvider(_directory, /*resource*/ stored);
        }

        #endregion
    }
}
