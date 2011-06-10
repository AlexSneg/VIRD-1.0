using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Util.FileTransfer
{
    /// <summary>
    /// пока нет девайсовых ресурсов с файлами сделано тупо!!!!! если появятся то надо наследоваться от ClientSideAbstractTransfer и делать нормально
    /// </summary>
    public class ClientSideDeviceSourceTransfer : IClientResourceCRUD<DeviceResourceDescriptor>
    {
        private readonly IPresentationService _presentationService;
        private IDeviceSourceDAL _deviceSourceDAL;

        public ClientSideDeviceSourceTransfer(IPresentationService presentationService, IDeviceSourceDAL deviceSourceDAL)
        {
            _presentationService = presentationService;
            _deviceSourceDAL = deviceSourceDAL;
        }

        #region Implementation of IClientResourceCRUD<DeviceResourceDescriptor>

        public FileSaveStatus CreateSource(DeviceResourceDescriptor resource, out string otherResourceId)
        {
            throw new System.NotImplementedException();
        }

        public FileSaveStatus SaveSource(DeviceResourceDescriptor resource, out string otherResourceId)
        {
            otherResourceId = null;
            return _presentationService.SaveDeviceSource(Thread.CurrentPrincipal as UserIdentity,
                resource, SourceStatus.Update);
        }

        public bool GetSource(DeviceResourceDescriptor resource, bool autoCommit)
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<PartSendEventArgs> OnPartTransmit;
        public event EventHandler OnTerminate;
        public event EventHandler<OperationStatusEventArgs<DeviceResourceDescriptor>> OnComplete;
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
        }

        #endregion
    }
}
