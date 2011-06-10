using System.ComponentModel;
using System.Configuration.Install;
using Microsoft.Win32;

namespace Hosts.VisualizationCore.VisualizationCoreHost
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (RegistryKey ckey = Registry.LocalMachine.OpenSubKey(
                string.Format(@"SYSTEM\CurrentControlSet\Services\{0}",serviceInstaller.ServiceName), true))
            {
                if (ckey != null)
                {
                    object val = ckey.GetValue("Type");
                    if (val != null)
                    {
                        int ival = (int) val;
                        ckey.SetValue("Type", (ival | 0x100));
                    }
                }
            }
        }
    }
}