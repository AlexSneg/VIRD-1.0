using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Interfaces
{
    public interface IConfiguration
    {
        IEventLogging EventLog { get; }
        IModule[] ModuleList { get; }

        ModuleConfiguration ModuleConfiguration { get; }

        ILabelStorageAdapter LabelStorageAdapter { get; }
        //ISystemParametersAdapter SystemParametersAdapter { get; }

        string ScenarioFolder { get; set; }
        string ScenarioSchemaFile { get; }

        string DeviceResourceFolder { get; }

        string GlobalSourceFolder { get; set; }
        string LocalSourceFolder { get; set; }

        string ConfigurationFolder { get; set; }
        string ConfigurationFile { get; set; }
        string ConfigurationSchemaFile { get; }

        bool IsClient { get; }
        int PingInterval { get; }

        string ReloadImage { get; }
        string BackgroundPresentationUniqueName { get; }
        string DefaultWndsize { get; }

        void SaveSystemParameters(ISystemParameters systemParameters);
        ISystemParameters LoadSystemParameters();

        /// <summary>сохранить все проперти из Settings, могут изменится только те, что имели set-ры </summary>
        void SaveUserSettings();
        /// <summary>перечитать все проперти из Settings</summary>
        void ReloadUserSettings();
    }
}