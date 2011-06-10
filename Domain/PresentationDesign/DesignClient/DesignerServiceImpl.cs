using System;
using System.Collections.Generic;
using Domain.PresentationDesign.DesignCommon;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Domain.PresentationDesign.Client
{
    public class DesignerServiceImpl : IDesignerService
    {
        #region IDesignerService Members

        ModuleConfiguration IDesignerService.GetModuleConfiguration()
        {
            throw new NotImplementedException();
        }

        public ISystemParameters GetSystemParameters()
        {
            throw new NotImplementedException();
        }

        ICollection<string> IDesignerService.CheckModuleConfiguration()
        {
            throw new NotImplementedException();
        }

        public FilesGroup GetConfigFilesForExport()
        {
            throw new System.NotImplementedException();
        }

        public bool CheckVersion(string version)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Implementation of IFileTransfer

        public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        {
            throw new System.NotImplementedException();
        }

        public FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        {
            throw new System.NotImplementedException();
        }

        public void Terminate(UserIdentity userIdentity)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Implementation of IConfigurationTransfer

        public FilesGroup InitConfigurationExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            throw new System.NotImplementedException();
        }

        public void DoneConfigurationExport(UserIdentity userIdentity)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}