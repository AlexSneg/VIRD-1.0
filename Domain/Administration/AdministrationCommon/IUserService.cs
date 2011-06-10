using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;

namespace Domain.Administration.AdministrationCommon
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        UserInfo[] GetUserStorage();
        [OperationContract]
        UserError AddUser(UserInfo userInfo);
        [OperationContract]
        UserError DeleteUser(UserInfo userInfo);
        [OperationContract]
        UserError UpdateUser(UserInfo userInfo);
        [OperationContract]
        UserError UnlockUser(UserInfo userInfo);
        [OperationContract]
        UserError LockUser(UserInfo userInfo);
        [OperationContract]
        UserInfo FindSystemUser();




    }
}
