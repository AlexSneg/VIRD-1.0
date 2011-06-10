using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Interfaces.ConfigModule.Player
{
    public interface IPlayerModule
    {
        Control CreateControlForSource(IEventLogging logging, Source source, Control parent, IPlayerCommand playerProvider, IPresentationClient client);
        Control CreateControlForDevice(IEventLogging logging, Device device, Control parent, IPlayerCommand playerProvider, IPresentationClient client);
        Control CreateControlForDisplay(IEventLogging logging, Display display, Control parent, IPlayerCommand playerProvider, IPresentationClient client);
    }
}