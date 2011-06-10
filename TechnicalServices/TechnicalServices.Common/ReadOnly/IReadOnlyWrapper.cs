using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Interfaces
{
    public interface IReadOnlyWrapper
    {
        object Wrapped
        {
            get;
            set;
        }
    }
}
