using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Entity;

namespace TechnicalServices.Interfaces
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IPing
    {
        [OperationContract(IsOneWay = true)]
        void Ping(UserIdentity identity);
    }
}
