using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.VNC.SystemModule.Design;

namespace Hosts.Plugins.VNC.Common
{
    public interface IVNCAction
    {
        void Connect();
        void Disconnect();
        bool IsConnected();
    }
}
