using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Util.FileTransfer
{
    public class ClientSideConfigurationTransfer : ClientSideAbstractTransfer<FilesGroup, FileProperty>
    {
        private readonly IConfigurationTransfer _configurationTransfer;
        private readonly string _directory;

        public ClientSideConfigurationTransfer(string directory, IConfigurationTransfer configurationTransfer)
        {
            _directory = directory;
            this._configurationTransfer = configurationTransfer;
        }

        #region Overrides of ClientSideAbstractTransfer<string>


        //protected override bool OnGetSource(FilesGroup resource)
        //{
        //    UserIdentity userIdentity = Thread.CurrentPrincipal as UserIdentity;
        //    FilesGroup filesGroup = _groupFileTransfer.InitFileTransfer(userIdentity, resource);
        //    try
        //    {
        //        if (filesGroup == null) return false;
        //        _provider = new GroupFileInfoProvider(_directory, resource);
        //        _fileSaver = new FileSaver(userIdentity, _provider, true);
        //        int numberOfParts = filesGroup.Files.Select(fp => _provider.GetNumberOfParts(fp.FileName)).Sum();
        //        int part = 0;
        //        foreach (FileProperty fileProperty in filesGroup.Files)
        //        {
        //            bool isComplete = false;
        //            FileTransferObject? obj;
        //            do
        //            {
        //                obj = _groupFileTransfer.Receive(userIdentity, fileProperty.FileName);
        //                if (_terminate.WaitOne(0) || !obj.HasValue)
        //                {
        //                    _groupFileTransfer.Terminate(userIdentity);
        //                    _fileSaver.Terminate();
        //                    return false;
        //                }
        //                isComplete = _fileSaver.Save(obj.Value);
        //                PartTransfer(part, numberOfParts);
        //                part++;
        //            } while (!isComplete);
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        if (_fileSaver != null)
        //            _fileSaver.Terminate();
        //        throw;
        //    }
        //    finally
        //    {
        //        _groupFileTransfer.DoneFileTransfer(userIdentity);
        //    }
        //}

        protected override void DoneDownload(UserIdentity userIdentity)
        {
            _configurationTransfer.DoneConfigurationExport(userIdentity);
        }

        protected override IFileTransfer FileTransport
        {
            get { return _configurationTransfer; }
        }

        protected override IFileInfoProvider<FileProperty> GetFileInfoProvider(FilesGroup resource, FilesGroup serverResource)
        {
            return new GroupFileInfoProvider(_directory, resource);
        }

        protected override FilesGroup InitDownload(UserIdentity userIdentity, FilesGroup resource)
        {
            return _configurationTransfer.InitConfigurationExport(userIdentity, resource);
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
