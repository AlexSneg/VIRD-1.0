using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Configuration.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Domain.Administration.AdministrationCommon
{
    [ServiceContract]    
    public interface ISystemParametersService
        {
            [OperationContract]
            [ServiceKnownType(typeof(SystemParameters))]
            ISystemParameters LoadSystemParameters();
            [OperationContract]
            [ServiceKnownType(typeof(SystemParameters))]
            void SaveSystemParameters(ISystemParameters systemParameters);
            [OperationContract]
            [ServiceKnownType(typeof(PresentationKey))]
            [ServiceKnownType(typeof(SlideKey))]
            PresentationInfoExt[] LoadPresentationWithOneSlide();
        }
}
