using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechnicalServices.Common;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Util.FileTransfer;

namespace DomainServices.PresentationManagement
{
    internal class PresentationExportHelper : IPresentationTransfer
    {
        private readonly IServerConfiguration _config;
        private readonly IPresentationDAL _presentationDAL;
        private readonly ServerSideGroupFileTransfer _serverSidePresentationSchemaTransfer = null;
        private readonly ServerSideGroupFileTransfer _serverSidePresentationTransfer = null;

        public PresentationExportHelper(IServerConfiguration config, IPresentationDAL presentationDAL)
        {
            _config = config;
            _presentationDAL = presentationDAL;
            _serverSidePresentationSchemaTransfer = new ServerSideGroupFileTransfer(
                config.ScenarioFolder, new ServerSideGroupFileSourceEx(config.ScenarioFolder));
            _serverSidePresentationTransfer = new ServerSideGroupFileTransfer(
                _config.ScenarioFolder, new ServerSideGroupFileSourceEx(_config.ScenarioFolder));
        }

        #region Implementation of IFileTransfer

        public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        {
            if (_serverSidePresentationSchemaTransfer.Contains(userIdentity))
                return _serverSidePresentationSchemaTransfer.Send(userIdentity, obj);
            else if (_serverSidePresentationTransfer.Contains(userIdentity))
                return _serverSidePresentationTransfer.Send(userIdentity, obj);
            return FileSaveStatus.Abort;
        }

        public FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        {
            if (_serverSidePresentationSchemaTransfer.Contains(userIdentity))
                return _serverSidePresentationSchemaTransfer.Receive(userIdentity, resourceId);
            else if (_serverSidePresentationTransfer.Contains(userIdentity))
                return _serverSidePresentationTransfer.Receive(userIdentity, resourceId);
            return null;
        }

        public void Terminate(UserIdentity userIdentity)
        {
            if (_serverSidePresentationSchemaTransfer.Contains(userIdentity))
                _serverSidePresentationSchemaTransfer.Terminate(userIdentity);
            else if (_serverSidePresentationTransfer.Contains(userIdentity))
                _serverSidePresentationTransfer.Terminate(userIdentity);
        }

        #endregion

        #region Implementation of IPresentationTransfer

        public FilesGroup GetPresentationForExport(string uniqueName)
        {
            string presentationFile = _presentationDAL.GetPresentationFile(uniqueName);
            if (string.IsNullOrEmpty(presentationFile)) return null;
            FilesGroup filesGroup = new FilesGroup(presentationFile, presentationFile,
                new FileProperty[] {new FileProperty()
                                        {
                                            FileName = presentationFile,
                                            Length = new FileInfo(presentationFile).Length
                                        }
                });
            return filesGroup;
        }

        public FilesGroup InitPresentationExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            return _serverSidePresentationTransfer.InitSourceDownload(userIdentity, filesGroup);
        }

        public FilesGroup GetPresentationSchemaFilesForExport()
        {
            string[] presentationSchemaFiles = _config.GetPresentationSchemaFiles();
            FilesGroup filesGroup = new FilesGroup(
                _config.ScenarioSchemaFile, _config.ScenarioSchemaFile,
                presentationSchemaFiles.Select(file=>new FileProperty()
                                                         {
                                                             FileName = file, Length = new FileInfo(file).Length
                                                         }));
            return filesGroup;
        }

        public FilesGroup InitPresentationSchemaExport(UserIdentity userIdentity, FilesGroup filesGroup)
        {
            return _serverSidePresentationSchemaTransfer.InitSourceDownload(userIdentity, filesGroup);
        }

        public void DonePresentationTransfer(UserIdentity userIdentity)
        {
            if (_serverSidePresentationSchemaTransfer.Contains(userIdentity))
                _serverSidePresentationSchemaTransfer.DoneSourceTransfer(userIdentity);
            else if (_serverSidePresentationTransfer.Contains(userIdentity))
                _serverSidePresentationTransfer.DoneSourceTransfer(userIdentity);
        }

        #endregion

        public bool Contains(UserIdentity userIdentity)
        {
            return _serverSidePresentationSchemaTransfer.Contains(userIdentity) ||
                   _serverSidePresentationTransfer.Contains(userIdentity);
        }
    }
}
