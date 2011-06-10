using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Util.FileTransfer
{
    public class ClientSideStandAloneDeviceSourceTransfer : IClientResourceCRUD<DeviceResourceDescriptor>
    {
        private readonly IResourceEx<DeviceResourceDescriptor> _resourceDAL;
        private readonly ManualResetEvent _inProgress = new ManualResetEvent(false);

        public ClientSideStandAloneDeviceSourceTransfer(IResourceEx<DeviceResourceDescriptor> resourceDAL)
        {
            this._resourceDAL = resourceDAL;
        }

        private FileSaveStatus Save(DeviceResourceDescriptor resourceDescriptor, out string otherResourceId)
        {
            otherResourceId = null;
            FileSaveStatus status;
            ResourceFileInfo resourceFileInfo = resourceDescriptor.ResourceInfo as ResourceFileInfo;
            Dictionary<string, string> fileDic = new Dictionary<string, string>();
            if (resourceFileInfo != null)
            {
                foreach (ResourceFileProperty resourceFileProperty in resourceFileInfo.ResourceFileList)
                {
                    if (!resourceFileProperty.Newly) continue;
                    string newFileName = _resourceDAL.GetResourceFileName(resourceDescriptor, resourceFileProperty.Id);
                    if (!File.Exists(resourceFileProperty.ResourceFullFileName)) continue;
                    File.Copy(resourceFileProperty.ResourceFullFileName, newFileName, true);
                    fileDic.Add(resourceFileProperty.Id, newFileName);
                }
                //if (!File.Exists(fileName))
                //    throw new FileNotFoundException(
                //        string.Format("Файл {0} не найден!", resourceFileInfo.ResourceFullFileName));

            }
            return SaveSource(resourceDescriptor, fileDic);
        }

        private FileSaveStatus SaveSource(DeviceResourceDescriptor resourceDescriptor, Dictionary<string, string> fileDic)
        {
            UserIdentity userIdentity = Thread.CurrentPrincipal as UserIdentity;
            return null != _resourceDAL.SaveSource(userIdentity, resourceDescriptor, fileDic) ? FileSaveStatus.Ok : FileSaveStatus.Abort;
        }

        private void Complete(FileSaveStatus status, string otherResourceId, DeviceResourceDescriptor resource)
        {
            if (OnComplete != null)
            {
                OnComplete(this, new OperationStatusEventArgs<DeviceResourceDescriptor>(status, otherResourceId, resource));
            }
        }

        #region Implementation of IClientResourceCRUD<DeviceResourceDescriptor>

        public FileSaveStatus CreateSource(DeviceResourceDescriptor resource, out string otherResourceId)
        {
            _inProgress.Set();
            FileSaveStatus status = FileSaveStatus.Abort;
            otherResourceId = null;
            try
            {
                // копируем источник в папку сорсов
                if (_resourceDAL.IsExists(resource)) return FileSaveStatus.Exists;
                return status = Save(resource, out otherResourceId);
            }
            finally
            {
                Complete(status, otherResourceId, resource);
                _inProgress.Reset();
            }
        }

        public FileSaveStatus SaveSource(DeviceResourceDescriptor resource, out string otherResourceId)
        {
            _inProgress.Set();
            FileSaveStatus status = FileSaveStatus.Abort;
            otherResourceId = null;
            try
            {
                return status = Save(resource, out otherResourceId);
            }
            finally
            {
                Complete(status, otherResourceId, resource);
                _inProgress.Reset();
            }
        }

        public bool GetSource(DeviceResourceDescriptor resource, bool autoCommit)
        {
            return _resourceDAL.IsExists(resource);
        }

        public event EventHandler<PartSendEventArgs> OnPartTransmit;
        public event EventHandler OnTerminate;
        public event EventHandler<OperationStatusEventArgs<DeviceResourceDescriptor>> OnComplete;
        public event Action<double, string> OnUploadSpeed;
        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        public void UploadSpeed(double speed, string file)
        {
            if(OnUploadSpeed != null)
            {
                OnUploadSpeed(speed, file);
            }
        }

        public void RollBack()
        {
            throw new System.NotImplementedException();
        }

        public void Terminate()
        {
            _inProgress.WaitOne();
            if (OnTerminate != null)
            {
                OnTerminate(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
