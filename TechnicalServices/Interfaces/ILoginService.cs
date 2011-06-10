using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Entity;

namespace TechnicalServices.Interfaces
{
    [ServiceContract(CallbackContract = typeof(ILoginNotifier), SessionMode = SessionMode.Required)]
    public interface ILoginService : IPing
    {
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        UserIdentity Login(string name, byte[] hash, string hostName);
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void Logoff(UserIdentity user);
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        IList<UserIdentity> GetUserLoginCollection();
        [OperationContract(Name = "LoginSubscribe")]
        void Subscribe(UserIdentity user);
        [OperationContract(Name = "LoginUnSubscribe")]
        void UnSubscribe(UserIdentity user);
    }

    public interface ILoginNotifier
    {
        [OperationContract(IsOneWay = true)]
        void LoginStatusChange(UserIdentity user, LogOnStatus newLoginStatus);
    }
}
