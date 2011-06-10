using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Interfaces
{
    public interface IDesignServiceProvider
    {
        void InvalidateView();
        object GetService(Type service);
        bool IsActive();
    }
}
