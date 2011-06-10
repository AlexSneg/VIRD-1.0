using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using System.ServiceModel;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Common.Classes;

namespace Domain.PresentationDesign.Client
{
    [CallbackBehavior(UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class PresentationNotifier : IPresentationNotifier
    {
        private readonly IConfiguration _config;
        #region event map
        public event EventHandler<NotifierEventArg<TechnicalServices.Entity.LockingInfo>> OnObjectLocked;
        public event EventHandler<NotifierEventArg<TechnicalServices.Entity.LockingInfo>> OnObjectUnLocked;
        public event EventHandler<NotifierEventArg<ResourceDescriptor>> OnResourceAdded;
        public event EventHandler<NotifierEventArg<ResourceDescriptor>> OnResourceDeleted;
        public event EventHandler<NotifierEventArg<ResourceDescriptor>> OnResourceUpdated;
        public event EventHandler<NotifierEventArg<DeviceResourceDescriptor>> OnDeviceResourceAdded;
        public event EventHandler<NotifierEventArg<DeviceResourceDescriptor>> OnDeviceResourceDeleted;
        public event EventHandler<NotifierEventArg<DeviceResourceDescriptor>> OnDeviceResourceUpdated;
        public event EventHandler<NotifierEventArg<PresentationInfo>> OnPresentationAdded;
        public event EventHandler<NotifierEventArg<PresentationInfo>> OnPresentationDeleted;
        public event EventHandler<NotifierEventArg<IList<ObjectInfo>>> OnObjectChanged;
        public event EventHandler<NotifierEventArg<CommunicationState>> OnStateChanged;
        public event EventHandler<NotifierEventArg<Label>> OnLabelAdded;
        public event EventHandler<NotifierEventArg<Label>> OnLabelDeleted;
        public event EventHandler<NotifierEventArg<Label>> OnLabelUpdated;
        #endregion

        public PresentationNotifier(IConfiguration config)
        {
            _config = config;
        }

        public void StateChanged(CommunicationState state)
        {
            FireEvent(OnStateChanged, state);
        }

        #region IPresentationNotifier Members

        public void ObjectLocked(TechnicalServices.Entity.LockingInfo lockingInfo)
        {
            FireEvent(OnObjectLocked, lockingInfo);
        }

        public void ObjectUnLocked(TechnicalServices.Entity.LockingInfo lockingInfo)
        {
            FireEvent(OnObjectUnLocked, lockingInfo);
        }

        public void ResourceAdded(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor)
        {
            FireEvent(OnResourceAdded, resourceDescriptor);
        }

        public void ResourceDeleted(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor)
        {
            FireEvent(OnResourceDeleted, resourceDescriptor);
        }

        public void ResourceUpdated(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor)
        {
            FireEvent(OnResourceUpdated, resourceDescriptor);
        }

        public void DeviceResourceAdded(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor)
        {
            FireEvent(OnDeviceResourceAdded, resourceDescriptor);
        }

        public void DeviceResourceDeleted(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor)
        {
            FireEvent(OnDeviceResourceDeleted, resourceDescriptor);
        }

        public void DeviceResourceUpdated(UserIdentity userIdentity, DeviceResourceDescriptor resourceDescriptor)
        {
            FireEvent(OnDeviceResourceUpdated, resourceDescriptor);
        }

        public void PresentationAdded(UserIdentity userIdentity, PresentationInfo presentationInfo)
        {
            FireEvent(OnPresentationAdded, presentationInfo);
        }

        public void PresentationDeleted(UserIdentity userIdentity, PresentationInfo presentationInfo)
        {
            FireEvent(OnPresentationDeleted, presentationInfo);
        }

        public void ObjectChanged(UserIdentity userIdentity, IList<ObjectInfo> objectInfoList)
        {
            FireEvent(OnObjectChanged, objectInfoList);
        }

        public void LabelAdded(Label label)
        {
            FireEvent(OnLabelAdded, label);
        }

        public void LabelDeleted(Label label)
        {
            FireEvent(OnLabelDeleted, label);
        }

        public void LabelUpdated(Label label)
        {
            FireEvent(OnLabelUpdated, label);
        }

        void FireEvent<T>(EventHandler<NotifierEventArg<T>> handler, T data)
        {
            try
            {
                if (handler != null)
                    handler(this, new NotifierEventArg<T> { Data = data });
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format(
                    "PresentationNotifier.FireEvent: handler {0}, Data {1}\n{2}",
                    handler, data, ex));
            }
        }

        #endregion
    }

}
