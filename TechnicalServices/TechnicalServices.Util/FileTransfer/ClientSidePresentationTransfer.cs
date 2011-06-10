using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Util.FileTransfer
{
    public class ClientSidePresentationTransfer : ClientSidePresentationSchemaTransfer
    {
        #region Overrides of ClientSideAbstractTransfer<FilesGroup,FileProperty>

        public ClientSidePresentationTransfer(string directory, IPresentationTransfer presentationTransfer) : base(directory, presentationTransfer)
        {}

        protected override FilesGroup InitDownload(UserIdentity userIdentity, FilesGroup resource)
        {
            return _presentationTransfer.InitPresentationExport(userIdentity, resource);
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
