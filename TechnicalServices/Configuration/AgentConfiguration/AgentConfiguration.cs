using TechnicalServices.Configuration.Agent.Properties;
using TechnicalServices.Configuration.Common;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Agent
{
    public class AgentConfiguration : CommonConfiguration, IAgentConfiguration
    {
        public AgentConfiguration(ModuleLoader loader, ModuleConfiguration configuration, IEventLogging logging)
            : base(loader, configuration, logging)
        {
        }

        #region IAgentConfiguration Members

        public string AgentUID
        {
            get { return Settings.Default.AgentUID; }
        }

        public string Temp
        {
            get { return Settings.Default.Temp; }
        }

        public string RestoreImagePath
        {
            get { return Settings.Default.RestoreImagePath; }
        }

        #endregion
    }
}