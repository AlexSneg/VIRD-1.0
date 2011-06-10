using System;

namespace Hosts.ActiveDisplayAgent.AgentHost
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // По умолчанию запускается в System32, а у нас в конфигах
            // относительные пути а не абсолютные
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            using (AgentHostImpl host = new AgentHostImpl())
                host.Run();
        }
    }
}