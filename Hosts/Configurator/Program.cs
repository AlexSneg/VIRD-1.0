using System;

namespace Hosts.Configurator.ConfiguratorHost
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (ConfiguratorHostImpl host = new ConfiguratorHostImpl())
                host.Run();
        }
    }
}