using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Util.FileTransfer
{
    public class ClientSideStandAloneSourceTransfer : IClientResourceCRUD<ResourceDescriptor>
    {
        private readonly IResourceEx<ResourceDescriptor> _resourceDAL;
        private readonly ManualResetEvent _inProgress = new ManualResetEvent(false);

        public ClientSideStandAloneSourceTransfer(IResourceEx<ResourceDescriptor> resourceDAL)
        {
            _resourceDAL = resourceDAL;
        }

        #region Implementation of IClientResourceCRUD

        public FileSaveStatus CreateSource(ResourceDescriptor resourceDescriptor, out string otherResourceId)
        {
            _inProgress.Set();
            FileSaveStatus status = FileSaveStatus.Abort;
            otherResourceId = null;
            try
            {
                // копируем источник в папку сорсов
                if (_resourceDAL.IsExists(resourceDescriptor)) return FileSaveStatus.Exists;
                return status = Save(resourceDescriptor, out otherResourceId);
            }
            finally
            {
                Complete(status, otherResourceId, resourceDescriptor);
                _inProgress.Reset();
            }
        }

        private FileSaveStatus Save(ResourceDescriptor resourceDescriptor, out string otherResourceId)
        {
            //FileSaveStatus status;
            otherResourceId = null;
            List<ResourceDescriptor> resourceDescriptors = _resourceDAL.SearchByName(resourceDescriptor);
            if (resourceDescriptors != null && resourceDescriptors.Count > 0)
            {
                otherResourceId = resourceDescriptors.First().ResourceInfo.Id;
                return FileSaveStatus.ExistsWithSameName;
            }
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

        public FileSaveStatus SaveSource(ResourceDescriptor resourceDescriptor, out string otherResourceId)
        {
            _inProgress.Set();
            FileSaveStatus status = FileSaveStatus.Abort;
            otherResourceId = null;
            try
            {
                return status = Save(resourceDescriptor, out otherResourceId);
            }
            finally
            {
                Complete(status, otherResourceId, resourceDescriptor);
                _inProgress.Reset();
            }
        }

        public bool GetSource(ResourceDescriptor resourceDescriptor, bool autoCommit)
        {
            return _resourceDAL.IsExists(resourceDescriptor);
        }

        public event EventHandler<PartSendEventArgs> OnPartTransmit;
        public event EventHandler OnTerminate;
        public event EventHandler<OperationStatusEventArgs<ResourceDescriptor>> OnComplete;
        public event Action<double, string> OnUploadSpeed;

        public void UploadSpeed(double speed, string file)
        {
            if(OnUploadSpeed != null)
            {
                OnUploadSpeed(speed, file);
            }
        }

        public void Commit()
        {
        }

        public void RollBack()
        {
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

        private FileSaveStatus SaveSource(ResourceDescriptor resourceDescriptor, Dictionary<string, string> fileDic)
        {
            UserIdentity userIdentity = Thread.CurrentPrincipal as UserIdentity;
            return null != _resourceDAL.SaveSource(userIdentity, resourceDescriptor, fileDic) ? FileSaveStatus.Ok : FileSaveStatus.Abort;
        }

        private void Complete(FileSaveStatus status, string otherResourceId, ResourceDescriptor resource)
        {
            if (OnComplete != null)
            {
                OnComplete(this, new OperationStatusEventArgs<ResourceDescriptor>(status, otherResourceId, resource));
            }
        }

    }
}
