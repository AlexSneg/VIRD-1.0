//#undef DEBUG
using System;
using System.Reflection;
using System.ServiceProcess;

namespace Hosts.VisualizationCore.VisualizationCoreHost
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
#if DEBUG
            try
            {
                ServiceBase service = new VisualizationCoreService();
                MethodInfo method;
                method = service.GetType().GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
                method.Invoke(service, new object[] {null});
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
                method = service.GetType().GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
                method.Invoke(service, new object[] {});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
#else
            // Сервис по умолчанию запускается в System32, а у нас в конфигах относительные пути а не абсолютные
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] { new VisualizationCoreService() };
			ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}