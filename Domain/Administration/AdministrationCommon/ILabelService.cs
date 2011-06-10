using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Persistence.SystemPersistence.Configuration;


namespace Domain.Administration.AdministrationCommon
{
    [ServiceContract]
    public interface ILabelService
    {
            [OperationContract]
            Label[] GetLabelStorage();
            [OperationContract]
            LabelError AddLabel(Label labelInfo);
            [OperationContract]
            [FaultContractAttribute(
              typeof(LabelUsedInPresentationException)
              )]
            LabelError DeleteLabel(Label labelInfo);
            [OperationContract]
            LabelError UpdateLabel(Label labelInfo);
            [OperationContract]
            LabelError UnlockLabel(Label labelInfo);
            [OperationContract]
            [FaultContractAttribute(
                  typeof(LabelUsedInPresentationException)
                  )]
            LabelError LockLabel(Label labelInfo);


    }
}
