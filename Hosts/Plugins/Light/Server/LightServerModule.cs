using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.Light.SystemModule.Config;
using Hosts.Plugins.Light.SystemModule.Design;

using TechnicalServices.HardwareEquipment.Util;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Light.Server
{
    public sealed class LightServerModule : HardwareEquipmentServerModule<SourceType, LightDeviceConfig, DisplayType>
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.Light);
        }

        protected override CommandDescriptor[] GetCommand(Device device1, Device device2)
        {
            List<CommandDescriptor> result = new List<CommandDescriptor>();
            LightDeviceDesign dev1 = device1 as LightDeviceDesign;
            LightDeviceDesign dev2 = device2 as LightDeviceDesign;
            if (dev2 != null)
            {
                int i = 0;
                foreach (LightUnitDesign item in dev2.UnitList)
                {
                    i++;
                    string command = item.IsAdjustable
                                         ?
                                             GetCommandByName(dev2, "LightSetGroupLevel").Command
                                         :
                                             GetCommandByName(dev2, "LightSetGroupState").Command;
                    result.Add(new CommandDescriptor(dev2.Type.UID, command, i, item.Brightness));
                }
            }
            return result.ToArray();
        }
    }
}