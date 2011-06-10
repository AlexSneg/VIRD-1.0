using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Interfaces
{
    public interface ISupportCustomSaveState
    {
        void GetState(out object[] ObjectContext);
        void SetState(object[] ObjectContext);
    }
}
