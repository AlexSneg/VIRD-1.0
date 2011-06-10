using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Configuration.Common;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Administration
{
    public class AdministrationConfiguration : CommonConfiguration, IAdministrationConfiguration
    {
        public AdministrationConfiguration(ModuleLoader loader, ModuleConfiguration configuration, IEventLogging logging)
            : base(loader, configuration, logging)
        {

        }
        private const bool _isStandalone = false;

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
