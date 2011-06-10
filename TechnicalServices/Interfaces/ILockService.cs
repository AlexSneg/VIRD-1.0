using System;
using System.ServiceModel;
using TechnicalServices.Entity;

namespace TechnicalServices.Interfaces
{
    //[ServiceContract(/*CallbackContract = typeof(ILockCanceled),*/ SessionMode = SessionMode.Required)]
    public interface ILockService
    {
        //IPresentationDAL PresentationDAL { get; set; }
        //[OperationContract(IsInitiating = true, IsTerminating = false)]
        bool AcquireLock(ICommunicationObject communicationObject, UserIdentity user, ObjectKey objectKey, RequireLock requireLock);
        //[OperationContract(IsInitiating = false, IsTerminating = false)]
        //LockStatus GetLockStatus(ObjectKey objectKey, out UserIdentity owner);
        //[OperationContract(IsInitiating = false, IsTerminating = false)]
        bool ReleaseLock(UserIdentity user, ObjectKey objectKey);
        LockingInfo GetLockInfo(ObjectKey objectKey);

        event Action<UserIdentity, ObjectKey, LockingInfo> AddItem;
        event Action<UserIdentity, ObjectKey, LockingInfo> RemoveItem;
    }

    //public interface ILockCanceled
    //{
    //    [OperationContract(IsOneWay = true)]
    //    void LockCanceled(UserIdentity user, ObjectKey objectKey, RequireLock requireLock);
    //}
}