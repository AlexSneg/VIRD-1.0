using System;

using TechnicalServices.Interfaces.ConfigModule.System;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule
{
    public abstract class ConfigurationModule : IConfigurationModule
    {
        #region IConfigurationModule Members

        public virtual Type[] GetDevice()
        {
            return new Type[0];
        }

        public virtual Type[] GetDisplay()
        {
            return new Type[0];
        }

        public virtual Type[] GetSource()
        {
            return new Type[0];
        }

        public virtual Type[] GetMappingType()
        {
            return new Type[0];
        }

        public virtual Type[] GetExtensionType()
        {
            return new Type[0];
        }

        #endregion
    }
}