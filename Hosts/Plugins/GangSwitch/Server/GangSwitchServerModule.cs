using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.GangSwitch.SystemModule.Config;
using Hosts.Plugins.GangSwitch.SystemModule.Design;

using TechnicalServices.HardwareEquipment.Util;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.GangSwitch.Server
{
    public sealed class GangSwitchServerModule :
        HardwareEquipmentServerModule<SourceType, GangSwitchDeviceConfig, DisplayType>
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.GangSwitch);
        }

        protected override CommandDescriptor[] GetCommand(Device device1, Device device2)
        {
            List<CommandDescriptor> result = new List<CommandDescriptor>();
            GangSwitchDeviceDesign dev1 = device1 as GangSwitchDeviceDesign;
            GangSwitchDeviceDesign dev2 = device2 as GangSwitchDeviceDesign;
            if (dev2 != null)
            {
                int i = 0;
                foreach (GangSwitchUnitDesign item in dev2.UnitList)
                {
                    i++;
                    string command = item.OnOffState
                                         ?
                                             GetCommandByName(dev2, "SwitchOn").Command
                                         :
                                             GetCommandByName(dev2, "SwitchOff").Command;
                    result.Add(new CommandDescriptor(dev2.Type.UID, command, i));
                }
            }
            return result.ToArray();
        }
    }
}