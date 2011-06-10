using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Util.FileTransfer
{
    public class ClientSidePresentationSchemaTransfer : ClientSideAbstractTransfer<FilesGroup, FileProperty>
    {
        protected readonly IPresentationTransfer _presentationTransfer;
        protected readonly string _directory;
        public ClientSidePresentationSchemaTransfer(string directory, IPresentationTransfer presentationTransfer)
        {
            _directory = directory;
            _presentationTransfer = presentationTransfer;
        }
        #region Overrides of ClientSideAbstractTransfer<FilesGroup,FileProperty>

        protected override void DoneDownload(UserIdentity userIdentity)
        {
            _presentationTransfer.DonePresentationTransfer(userIdentity);
        }

        protected override IFileTransfer FileTransport
        {
            get { return _presentationTransfer; }
        }

        protected override IFileInfoProvider<FileProperty> GetFileInfoProvider(FilesGroup resource, FilesGroup serverResource)
        {
            return new GroupFileInfoProvider(_directory, resource);
        }

        protected override FilesGroup InitDownload(UserIdentity userIdentity, FilesGroup resource)
        {
            return _presentationTransfer.InitPresentationSchemaExport(userIdentity, resource);
        }

        protected override FileSaveStatus OnCreateSource(FilesGroup resource, out string otherResourceId)
        {
            throw new System.NotImplementedException();
        }

        protected override FileSaveStatus OnSaveSource(FilesGroup resource, out string otherResourceId)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
