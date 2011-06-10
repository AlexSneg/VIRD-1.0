using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using TechnicalServices.Configuration.Common;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Configuration.Server.Properties;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Server
{
    public class ServerConfiguration : CommonConfiguration, IServerConfiguration
    {
        private const string UserStorageFile = "UserStorage.xml";

        public ServerConfiguration(ModuleLoader loader, IEventLogging logging)
            : base(loader, true, logging)
        {
            string path = Path.GetFullPath(Settings.Default.UserStorageFolder);
            path = Path.Combine(path, UserStorageFile);
            UserStorageAdapter.Instance.Init(path);
        }

        #region IServerConfiguration Members

        public override bool IsClient
        {
            get { return false; }
        }

        public UserStorageAdapter UserStorageAdapter
        {
            [DebuggerStepThrough]
            get { return UserStorageAdapter.Instance; }
        }


        public Uri ControllerURI
        {
            [DebuggerStepThrough]
            get { return new Uri(Settings.Default.ControllerURI); }
        }

        public int ControllerReceiveTimeout
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ControllerReceiveTimeout; }
            
        }

        public int ControllerCheckTimeout
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ControllerCheckTimeout; }
        }


        public string ControllerLibrary
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ControllerLibrary; }
        }

        public Uri ExternalSystemControllerUri
        {
            [DebuggerStepThrough]
            get { return new Uri(Settings.Default.ExternalSystemControllerUri); }
        }

        public string ExternalSystemControllerLibrary
        {
            [DebuggerStepThrough]
            get { return Settings.Default.ExternalSystemControllerLibrary; }
        }

        public string[] GetConfigurationFiles()
        {
            // все *.xml и *.xsd файлы в конфигурационной папке - относятся к конфигурационным файлам
            const string xmlPattern = "*.xml";
            const string xsdPattern = "*.xsd";
            return Directory.GetFiles(ConfigurationFolder, xmlPattern).
                Union(Directory.GetFiles(ConfigurationFolder, xsdPattern)).
                ToArray();
        }

        public string[] GetPresentationSchemaFiles()
        {
            const string xsdPattern = "*.xsd";
            return Directory.GetFiles(ScenarioFolder, xsdPattern).ToArray();
        }


        #endregion
    }
}