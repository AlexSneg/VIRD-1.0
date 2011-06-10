using System.Diagnostics;

using TechnicalServices.Configuration.Common;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Client
{
    public class ClientConfiguration : CommonConfiguration, IClientConfiguration
    {
        private readonly bool _isStandalone;

        public ClientConfiguration(ModuleLoader loader, ModuleConfiguration configuration, IEventLogging logging) :
            base(loader, configuration, logging)
        {
            _isStandalone = false;
        }

        public ClientConfiguration(ModuleLoader loader, IEventLogging logging) :
            base(loader, true, logging)
        {
            _isStandalone = true;
        }

        public ClientConfiguration(ModuleLoader loader, IEventLogging logging, bool notThrowExceptionIfInvalideConfiguration) :
            base(loader, true, logging, notThrowExceptionIfInvalideConfiguration)
        {
            _isStandalone = true;
        }


        public bool IsStandalone
        {
            [DebuggerStepThrough]
            get { return _isStandalone; }
        }

        public TechnicalServices.Entity.XmlSerializableDictionary<string, int> DevicePositions
        {
            get
            {
                //return null;
                return TechnicalServices.Configuration.Common.Properties.Settings.Default.DevicePositions;
            }
            set
            {
                //_devicePositionList = value;
            }
        }
        //TechnicalServices.Entity.XmlSerializableDictionary<string, int> _devicePositionList=new TechnicalServices.Entity.XmlSerializableDictionary<string,int>();

        protected override ILabelStorageAdapter CreateLabelStorageAdapter(ModuleConfiguration configuration)
        {
            return new ClientLabelStorageAdapter(configuration);
        }
        protected override TechnicalServices.Interfaces.ILabelStorageAdapter CreateLabelStorageAdapter(ModuleConfiguration configuration, string labelStoragePath)
        {
            return new ClientLabelStorageAdapter(configuration);
        }
    }
}