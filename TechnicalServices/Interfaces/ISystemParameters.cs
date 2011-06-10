using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TechnicalServices.Interfaces
{
    public interface ISystemParameters
    {
         string ReloadImage { get; set; }
         string BackgroundPresentationUniqueName { get; set; }
         string DefaultWndsize { get; set; }
         int BackgroundPresentationRestoreTimeout { get; set; }
    }
}
