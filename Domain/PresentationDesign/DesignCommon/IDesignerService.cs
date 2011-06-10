using System.Collections.Generic;
using System.ServiceModel;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace Domain.PresentationDesign.DesignCommon
{
    [ServiceContract]
    public interface IDesignerService : IConfigurationTransfer
    {
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        ModuleConfiguration GetModuleConfiguration();

        [OperationContract]
        [ServiceKnownType(typeof(SystemParameters))]
        ISystemParameters GetSystemParameters();

        [OperationContract]
        ICollection<string> CheckModuleConfiguration();

        [OperationContract]
        bool CheckVersion(string version);
    }
}