using TechnicalServices.Interfaces.ConfigModule.System;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule
{
    public abstract class SystemModule<TConfig, TDesign> : ISystemModule
        where TConfig : IConfigurationModule, new()
        where TDesign : IPresentationModule, new()
    {
        private readonly TConfig _configuration = new TConfig();
        private readonly TDesign _presentation = new TDesign();

        #region ISystemModule Members

        public string Name { get; set; }

        public IConfigurationModule Configuration
        {
            get { return _configuration; }
        }

        public IPresentationModule Presentation
        {
            get { return _presentation; }
        }

        #endregion
    }
}