using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

using DomainServices.EnvironmentConfiguration.ConfigModule;

using TechnicalServices.Configuration.LoadModules.Properties;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;

namespace TechnicalServices.Configuration.LoadModules
{
    public class ModuleLoader : IDisposable
    {
        private const string LoadingWarning = "Директория \"{0}\" не существует, поск модулей будет произведен в текущей директории";
        private const string InfoMesage = "Для конфигурации загружены следующие модули:";
        private const string ModuleInfoFormat = "{0}Location:{1}{2}\tFullName:{3}";

        private readonly IEventLogging _logService;
        private readonly List<IModule> _moduleList;

        public ModuleLoader(IEventLogging logging)
        {
            Debug.Assert(logging != null, "Нет системы логирования");
            _logService = logging;

            string path = Path.GetFullPath(Settings.Default.ModuleFolder);
            if (!Directory.Exists(path))
                _logService.WriteWarning(String.Format(LoadingWarning, path));

            string[] fileList = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
            IEnumerable<string> libraryList = getFilesByAttribyte(fileList);
            _moduleList = loadInstanceFromAssembly(libraryList, logging);
        }

        public List<IModule> ModuleList
        {
            [DebuggerStepThrough]
            get { return _moduleList; }
        }

        public IEventLogging EventLog
        {
            [DebuggerStepThrough]
            get { return _logService; }
        }

        public List<string> GetVersionList()
        {
            List<string> list = new List<string>();
            foreach (IModule module in _moduleList)
                list.Add(module.GetType().Assembly.FullName);
            return list;
        }

        public void SelectModules(List<string> list)
        {
            for (int i = _moduleList.Count - 1; i >= 0; i--)
            {
                IModule module = _moduleList[i];
                if (!list.Contains(module.Name)) 
                    _moduleList.RemoveAt(i);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            _moduleList.Clear();
        }

        #endregion

        private static IEnumerable<string> getFilesByAttribyte(IEnumerable<string> fileList)
        {
            return AssemblyReflectionLoadHelper.AppDomainGetFilesByModuleAttribute(fileList);
        }

        private static List<IModule> loadInstanceFromAssembly(IEnumerable<string> fileList, IEventLogging logging)
        {
            List<IModule> result = new List<IModule>(8);
            StringBuilder message = new StringBuilder(InfoMesage);
            foreach (string fileName in fileList)
            {
                Assembly assembly = Assembly.LoadFrom(fileName);
                object[] list = assembly.GetCustomAttributes(typeof(ModuleAttribute), false);
                foreach (ModuleAttribute attr in list)
                {
                    IModule module = (IModule)assembly.CreateInstance(attr.ModuleType.FullName);
                    module.Name = attr.Name;
                    result.Add(module);
                }

                message.AppendFormat(CultureInfo.InvariantCulture,
                                     ModuleInfoFormat,
                                     Environment.NewLine,
                                     assembly.Location,
                                     Environment.NewLine,
                                     assembly.FullName);
            }
            logging.WriteInformation(message.ToString());
            return result;
        }
    }
}