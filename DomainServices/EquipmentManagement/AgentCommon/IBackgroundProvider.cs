using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DomainServices.EquipmentManagement.AgentCommon
{
    public interface IBackgroundProvider : IDisposable
    {
        void SetBackgroundImage(string imageFileName, Control control);
    }
}
