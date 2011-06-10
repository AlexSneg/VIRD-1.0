using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using TechnicalServices.Configuration.Configurator.Properties;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Configuration.Configurator
{
    public class ConfiguratorConfiguration : IConfiguration
    {
        private const string _configurationSchemaFile = "ModuleConfiguration.xsd";
        private readonly IEventLogging _eventLog;

        public ConfiguratorConfiguration(IEventLogging eventLog)
        {
            _eventLog = eventLog;
        }

        public string[] Load(List<Type> displayList, List<Type> deviceList, List<Type> sourceList, List<Type> mappingList)
        {
            string moduleConfigPath = Path.GetFullPath(Settings.Default.ConfigurationFolder);
            moduleConfigPath = Path.Combine(moduleConfigPath, Settings.Default.ConfigurationFile);
            string[] modules = ModuleConfigurationExtenstion.LoadSchema(moduleConfigPath);
            ModuleConfiguration = ModuleConfigurationExtenstion.Load(moduleConfigPath, displayList, deviceList,
                                                                     sourceList, mappingList);
            ModuleConfiguration.InitReference();
            return modules;
        }

        public void Save(List<Type> displayList, List<Type> deviceList, List<Type> sourceList, List<Type> mappingList,
                         IEnumerable<string> modules)
        {
            if (ModuleConfiguration == null) return;
            string moduleConfigPath = Path.GetFullPath(Settings.Default.ConfigurationFolder);
            moduleConfigPath = Path.Combine(moduleConfigPath, Settings.Default.ConfigurationFile);
            ModuleConfiguration.LastChangeDate = DateTime.Now;
            ModuleConfiguration.SaveSchema(moduleConfigPath, modules.ToArray());
            ModuleConfiguration.Save(moduleConfigPath, displayList, deviceList, sourceList, mappingList);
        }

        #region IConfiguration

        public ILabelStorageAdapter LabelStorageAdapter
        {
            [DebuggerStepThrough]
            get { return null; }
        }

        public IModule[] ModuleList
        {
            [DebuggerStepThrough]
            get { return new IModule[] { }; }
        }

        public IEventLogging EventLog
        {
            [DebuggerStepThrough]
            get { return _eventLog; }
        }

        public ModuleConfiguration ModuleConfiguration { get; private set; }

        public string ScenarioFolder
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ScenarioFolder; }
            [DebuggerStepThrough]
            set { throw new NotImplementedException(); }
        }

        public string ScenarioSchemaFile
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ScenarioSchemaFile; }
        }

        public string DeviceResourceFolder
        {
            [DebuggerStepThrough]
            get { throw new NotImplementedException(); }
        }

        public string GlobalSourceFolder
        {
            [DebuggerStepThrough]
            get { throw new NotImplementedException(); }
            [DebuggerStepThrough]
            set { throw new NotImplementedException(); }
        }

        public string LocalSourceFolder
        {
            [DebuggerStepThrough]
            get { return Settings.Default.LocalSourceFolder; }
            [DebuggerStepThrough]
            set { throw new NotImplementedException(); }
        }

        public string ConfigurationFolder
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ConfigurationFolder; }
            [DebuggerStepThrough]
            set { throw new NotImplementedException(); }
        }

        public string ConfigurationFile
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ConfigurationFile; }
            [DebuggerStepThrough]
            set { throw new NotImplementedException(); }
        }

        public string ConfigurationSchemaFile
        {
            [DebuggerStepThrough]
            get { return _configurationSchemaFile; }
        }

        public virtual bool IsClient
        {
            [DebuggerStepThrough]
            get { return true; }
        }

        public int PingInterval
        {
            [DebuggerStepThrough]
            get { return 0; }
        }

        public string ReloadImage
        {
            [DebuggerStepThrough]
            get { return String.Empty; }
        }

        public string BackgroundPresentationUniqueName
        {
            [DebuggerStepThrough]
            get { return String.Empty; }
        }

        public string DefaultWndsize
        {
            [DebuggerStepThrough]
            get { return String.Empty; }
        }

        public void SaveSystemParameters(ISystemParameters systemParameters)
        {
            throw new NotImplementedException();
        }

        public ISystemParameters LoadSystemParameters()
        {
            throw new NotImplementedException();
        }

        public void SaveUserSettings()
        {
            throw new NotImplementedException();
        }

        public void ReloadUserSettings()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}