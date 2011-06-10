using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DomainServices.PresentationManagement;
using TechnicalServices.Common;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Util.FileTransfer;

namespace Domain.PresentationDesign.DesignService
{
    internal class ConfigurationExportHelper : IConfigurationTransfer
    {
        private readonly IServerConfiguration _config;
        private readonly ServerSideGroupFileTransfer _serverSideGroupFileTransfer = null;

        public ConfigurationExportHelper(IServerConfiguration config)
        {
            _config = config;
            _serverSideGroupFileTransfer = new ServerSideGroupFileTransfer(config.ConfigurationFolder, new ServerSideGroupFileSourceEx(_config.ConfigurationFolder));
        }

        #region Implementation of IFileTransfer

        public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        {
            return _serverSideGroupFileTransfer.Send(userIdentity, obj);
        }

        public FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        {
            try
            {
                return _serverSideGroupFileTransfer.Receive(userIdentity, resourceId);
            }
            catch(Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ConfigurationExportHelper.Receive: {0}", ex));
                return null;
            }
        }

        public void Terminate(UserIdentity userIdentity)
        {
            _serverSideGroupFileTransfer.Terminate(userIdentity);
        }

        #endregion

        #region Implementation of IConfigurationTransfer

        public FilesGroup GetConfigFilesForExport()
        {
            string[] configFiles = _config.GetConfigurationFiles();
            string configFile = _config.ConfigurationFile;
            return new FilesGroup(configFile, configFile,
                configFiles.Select(cf => new FileProperty() { FileName = cf, Length = new FileInfo(cf).Length }));
        }

        public FilesGroup InitConfigurationExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            return _serverSideGroupFileTransfer.InitSourceDownload(userIdentity, filesGroup);
        }

        public void DoneConfigurationExport(UserIdentity userIdentity)
        {
            _serverSideGroupFileTransfer.DoneSourceTransfer(userIdentity);
        }

        #endregion

        public bool IsUserParticipateInResourceTransfer(UserIdentity userIdentity)
        {
            return _serverSideGroupFileTransfer.Contains(userIdentity);
        }

    }
}
