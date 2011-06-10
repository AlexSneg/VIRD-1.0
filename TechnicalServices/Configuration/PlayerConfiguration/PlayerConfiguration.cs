using TechnicalServices.Configuration.Common;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Player
{
    public class PlayerConfiguration : CommonConfiguration, IPlayerConfiguration
    {
        private const bool _isStandalone = false;

        public PlayerConfiguration(ModuleLoader loader, ModuleConfiguration configuration, IEventLogging logging) :
            base(loader, configuration, logging)
        {
        }

        #region IClientConfiguration Members

        public bool IsStandalone
        {
            get { return _isStandalone; }
        }

        public TechnicalServices.Entity.XmlSerializableDictionary<string, int> DevicePositions
        {
            get
            {
                return _devicePositionList;
            }
            set
            {
                _devicePositionList = value;
            }
        }
        TechnicalServices.Entity.XmlSerializableDictionary<string, int> _devicePositionList = new TechnicalServices.Entity.XmlSerializableDictionary<string, int>();

        #endregion
    }
}