using System;

namespace Hosts.PresentationDesigner.DesignerHost
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            using (DesignerHostImpl host = new DesignerHostImpl(args))
                host.Run();
        }
    }
}