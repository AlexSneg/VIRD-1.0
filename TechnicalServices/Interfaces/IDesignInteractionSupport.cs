using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Interfaces
{
    public interface IDesignInteractionSupport
    {
        void UpdateServiceReference(IDesignServiceProvider provider);
        void InteractiveAction();
        bool SupportInteraction { get; }
    }
}
