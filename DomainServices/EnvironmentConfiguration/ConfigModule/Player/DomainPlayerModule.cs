using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Player
{
    public abstract class DomainPlayerModule : IPlayerModule
    {
        public virtual Control CreateControlForSource(IEventLogging logging, Source source, Control parent, IPlayerCommand playerProvider, IPresentationClient client)
        {
            return null;
        }

        public virtual Control CreateControlForDevice(IEventLogging logging, Device device, Control parent, IPlayerCommand playerProvider, IPresentationClient client)
        {
            return null;
        }

        public virtual Control CreateControlForDisplay(IEventLogging logging, Display display, Control parent, IPlayerCommand playerProvider, IPresentationClient client)
        {
            return null;
        }
    }
}
