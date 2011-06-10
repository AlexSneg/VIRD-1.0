using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;


namespace Domain.Administration.AdministrationCommon
{
    [ServiceContract]
    public interface IAdministrationService : IUserService, ILabelService, ISystemParametersService
    {
        

    }
}
