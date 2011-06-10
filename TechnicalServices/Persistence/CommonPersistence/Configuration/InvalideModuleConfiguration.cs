using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TechnicalServices.Persistence.SystemPersistence.Configuration
{
    /// <summary>
    /// НЕ ЮЗАТЬ ЭТОТ КЛАСС - используется ТОЛЬКО в дизайнере в автономном режиме, потому что дизайнер в этом режиме должен работать до некоторого момента даже без конфигурации
    /// для этого используется эта заглушка!
    /// </summary>
    [Serializable]
    public class InvalideModuleConfiguration : ModuleConfiguration
    {
        public string ErrorMessage { get; set;}
        public Exception InnerException { get; set; }
    }
}
