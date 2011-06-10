using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Schema;

using TechnicalServices.Configuration.Common.Properties;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Configuration.Common
{
    public class CommonConfiguration : IConfiguration
    {
        private const string ConfigXsdFileSuffix = "Config.xsd";
        private const string LabelFile = "LabelStorage.xml";
        private const string _configurationSchemaFile = "ModuleConfiguration.xsd";

        private readonly ModuleConfiguration _config;
        private readonly ILabelStorageAdapter _labelStorageAdapter;
        private readonly SystemParametersAdapter _systemParametersAdapter;
        protected IEventLogging _logging;
        protected readonly ModuleLoader loader;

        public CommonConfiguration(ModuleLoader loader, ModuleConfiguration configuration, IEventLogging logging)
        {
            this.loader = loader;
            this._logging = logging;
            _config = configuration;
            _labelStorageAdapter = CreateLabelStorageAdapter(configuration);
            _systemParametersAdapter = new SystemParametersAdapter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="checkModules"></param>
        /// <param name="logging"></param>
        public CommonConfiguration(ModuleLoader loader, bool checkModules, IEventLogging logging)
            : this(loader, checkModules, logging, false)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="checkModules"></param>
        /// <param name="logging"></param>
        /// <param name="notThrowExceptionIfInvalideConfiguration"> используется ТОЛЬКО для дизайнера в автономном режиме</param>
        public CommonConfiguration(ModuleLoader loader, bool checkModules, IEventLogging logging, bool notThrowExceptionIfInvalideConfiguration)
        {
            this.loader = loader;
            this._logging = logging;
            string labelStoragePath;
            string moduleConfigFile = String.Empty;
            string configurationSchemaFile;
            try
            {
                string moduleConfigPath = labelStoragePath = Path.GetFullPath(ConfigurationFolder);
                moduleConfigFile = Path.Combine(moduleConfigPath, ConfigurationFile);
                labelStoragePath = Path.Combine(labelStoragePath, LabelFile);
                configurationSchemaFile = Path.Combine(moduleConfigPath, ConfigurationSchemaFile);
            }
            catch (ArgumentException ex)
            {
                throw new ModuleConfigurationException(moduleConfigFile);
            }

            try
            {
                _config = LoadModuleConfiguration(moduleConfigFile, configurationSchemaFile, this.loader.ModuleList);
                if (checkModules) CheckLoadedModules();
            }
            catch(Exception ex)
            {
                if (notThrowExceptionIfInvalideConfiguration)
                {
                    _config = new InvalideModuleConfiguration() {ErrorMessage = ex.Message, InnerException = ex};
                }
                else
                {
                    throw;
                }
            }

            _systemParametersAdapter = new SystemParametersAdapter();
            _labelStorageAdapter = CreateLabelStorageAdapter(_config, labelStoragePath);
        }

        protected virtual ILabelStorageAdapter CreateLabelStorageAdapter(ModuleConfiguration configuration)
        {
            return new LabelStorageAdapter(configuration, _logging);
        }

        protected virtual ILabelStorageAdapter CreateLabelStorageAdapter(ModuleConfiguration configuration, string labelStoragePath)
        {
            return new LabelStorageAdapter(configuration, labelStoragePath, _logging);
        }

        #region IConfiguration

        public ILabelStorageAdapter LabelStorageAdapter
        {
            [DebuggerStepThrough]
            get { return _labelStorageAdapter; }
        }

        public IModule[] ModuleList
        {
            [DebuggerStepThrough]
            get { return loader.ModuleList.ToArray(); }
        }

        public IEventLogging EventLog
        {
            [DebuggerStepThrough]
            get { return loader.EventLog; }
        }

        public ModuleConfiguration ModuleConfiguration
        {
            [DebuggerStepThrough]
            get { return _config; }
        }

        public string ScenarioFolder
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ScenarioFolder; }
            set { Settings.Default.ScenarioFolderSet = value; }
        }

        public string ScenarioSchemaFile
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ScenarioSchemaFile; }
        }

        public string DeviceResourceFolder
        {
            [DebuggerStepThrough]
            get { return Settings.Default.DeviceResourceFolder; }
        }

        public string GlobalSourceFolder
        {
            [DebuggerStepThrough]
            get { return Settings.Default.GlobalSourceFolder; }
            set { Settings.Default.GlobalSourceFolderSet = value; }
        }

        public string LocalSourceFolder
        {
            [DebuggerStepThrough]
            get { return Settings.Default.LocalSourceFolder; }
            set { Settings.Default.LocalSourceFolderSet = value; }
        }

        public string ConfigurationFolder
        {
            get { return Settings.Default.ConfigurationFolder; }
            set { Settings.Default.ConfigurationFolderSet = value; }
        }

        public string ConfigurationFile
        {
            get { return Settings.Default.ConfigurationFile.Trim(); }
            set { Settings.Default.ConfigurationFileSet = value; }
        }

        public string ConfigurationSchemaFile
        {
            get { return _configurationSchemaFile; }
        }

        public virtual bool IsClient
        {
            get { return true; }
        }

        public int PingInterval
        {
            [DebuggerStepThrough]
            get { return Settings.Default.PingInterval; }
        }

        public string ReloadImage
        {
            [DebuggerStepThrough]
            get { return Settings.SystemDefault.ReloadImage; }
        }

        public string BackgroundPresentationUniqueName
        {
            [DebuggerStepThrough]
            get { return Settings.SystemDefault.BackgroundPresentationUniqueName; }
        }

        public string DefaultWndsize
        {
            [DebuggerStepThrough]
            get { return Settings.SystemDefault.DefaultWndsize; }
        }

        public void SaveSystemParameters(ISystemParameters systemParameters)
        {
            _systemParametersAdapter.SaveSystemParameters(systemParameters);
        }

        public ISystemParameters LoadSystemParameters()
        {
            return _systemParametersAdapter.LoadSystemParameters();
        }

        public void SaveUserSettings()
        {
            Settings.Default.Save();
        }
        public void ReloadUserSettings()
        {
            Settings.Default.Reload();
        }

        #endregion

        private static ModuleConfiguration LoadModuleConfiguration(string fileName, string configurationSchemaFile, ICollection<IModule> moduleList)
        {
            ModuleConfiguration result = ModuleConfigurationExtenstion.Load(fileName, configurationSchemaFile, moduleList);
            result.InitReference();
            return result;
        }
        /// <summary>
        /// Получаем из файла ModuleConfiguration.xsd 
        /// список модулей используемых в системе, 
        /// после чего удаляем из списка 
        /// загруженных модулей неиспользуемые 
        /// </summary>
        private void CheckLoadedModules()
        {
            string fileName = Path.GetFullPath(ConfigurationFolder);
            fileName = Path.Combine(fileName, ConfigurationSchemaFile);
            //fileName = Path.ChangeExtension(fileName, "xsd");

            List<string> list = new List<string>();
            if (!File.Exists(fileName)) return;
            using (TextReader reader = new StreamReader(fileName))
            {
                XmlSchema xsd = XmlSchema.Read(reader, null);
                foreach (XmlSchemaInclude schemaObject in xsd.Includes)
                {
                    string value = schemaObject.SchemaLocation.Replace(ConfigXsdFileSuffix, "");
                    list.Add(value);
                }
            }
            loader.SelectModules(list);
        }

    }
}