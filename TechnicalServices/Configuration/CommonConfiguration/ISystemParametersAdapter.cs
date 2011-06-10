using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;


namespace TechnicalServices.Configuration.Common
{
    public interface ISystemParametersAdapter
    {
        ISystemParameters LoadSystemParameters();
        void SaveSystemParameters(ISystemParameters systemParameters);
    }
}
