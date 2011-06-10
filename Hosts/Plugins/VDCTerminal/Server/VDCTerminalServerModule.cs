using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.VDCTerminal.SystemModule.Config;
using Hosts.Plugins.VDCTerminal.SystemModule.Design;

using TechnicalServices.HardwareEquipment.Util;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.VDCTerminal.Server
{
    public sealed class VDCTerminalServerModule :
        HardwareEquipmentServerModule<VDCTerminalSourceConfig, VDCTerminalDeviceConfig, DisplayType>
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.VDCTerminal);
        }

        protected override CommandDescriptor[] GetCommand(Device device1, Device device2)
        {
            List<CommandDescriptor> result = new List<CommandDescriptor>();
            VDCTerminalDeviceDesign dev1 = device1 as VDCTerminalDeviceDesign;
            VDCTerminalDeviceDesign dev2 = device2 as VDCTerminalDeviceDesign;
            if (dev2 != null)
            {
                if (dev1 != null)
                    result.Add(new CommandDescriptor(dev2.Type.UID,
                                                     GetCommandByName(dev2, "Disconnect").Command));

                if (dev2.Abonent != null)
                    result.Add(new CommandDescriptor(dev2.Type.UID,
                                                     GetCommandByName(dev2, "Dial").Command,
                                                     dev2.Abonent.Name,
                                                     dev2.Abonent.Number1,
                                                     dev2.Abonent.Number2,
                                                     dev2.Abonent.ConnectionType.ToString(),
                                                     dev2.Abonent.ConnectionQuality.ToString()));
                else if (!string.IsNullOrEmpty(dev2.DirectNumber))
                    result.Add(new CommandDescriptor(dev2.Type.UID,
                                                     GetCommandByName(dev2, "Dial").Command,
                                                     string.Empty,
                                                     dev2.DirectNumber,
                                                     string.Empty,
                                                     ConnectionTypeEnum.Auto.ToString(),
                                                     ConnectionQualityEnum.Auto.ToString()));

                result.Add(new CommandDescriptor(dev2.Type.UID,
                                                 GetCommandByName(dev2, "SetAutoAnswer").Command,
                                                 Convert.ToInt32(dev2.AutoResponse)));
                result.Add(new CommandDescriptor(dev2.Type.UID,
                                                 GetCommandByName(dev2, "SetContent").Command,
                                                 Convert.ToInt32(dev2.PeopleConnect)));
                result.Add(new CommandDescriptor(dev2.Type.UID,
                                                 GetCommandByName(dev2, "SetPrivacy").Command,
                                                 Convert.ToInt32(dev2.Privacy)));
                result.Add(new CommandDescriptor(dev2.Type.UID,
                                                 GetCommandByName(dev2, "SetDND").Command,
                                                 Convert.ToInt32(dev2.DND)));
                result.Add(new CommandDescriptor(dev2.Type.UID,
                                                 GetCommandByName(dev2, "SetPIP").Command,
                                                 Convert.ToInt32(dev2.PiP)));
            }
            return result.ToArray();
        }
    }
}