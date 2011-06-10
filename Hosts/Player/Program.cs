using System;

namespace Hosts.Player.PlayerHost
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (PlayerHostImpl host = new PlayerHostImpl())
                host.Run();
        }
    }
}