using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.VDCServer.SystemModule.Config;
using Hosts.Plugins.VDCServer.SystemModule.Design;

using TechnicalServices.HardwareEquipment.Util;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.VDCServer.Server
{
    public sealed class VDCServerServerModule :
        HardwareEquipmentServerModule<SourceType, VDCServerDeviceConfig, DisplayType>
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.VDCServer);
        }

        protected override CommandDescriptor[] GetCommand(Device device1, Device device2)
        {
            List<CommandDescriptor> result = new List<CommandDescriptor>();
            VDCServerDeviceDesign dev1 = device1 as VDCServerDeviceDesign;
            VDCServerDeviceDesign dev2 = device2 as VDCServerDeviceDesign;
            if (dev2 != null)
            {
                if (dev1 != null)
                {
                    //если конференция использовалась, то закончить конференцию
                    result.Add(new CommandDescriptor(dev1.Type.UID,
                                                     GetCommandByName(dev1, "DestroyConference").Command,
                                                     dev1.ConferenceName));
                }
                //то создадим новую
                result.Add(new CommandDescriptor(dev2.Type.UID,
                                                 GetCommandByName(dev2, "CreateConference").Command,
                                                 dev2.ConferenceName, Convert.ToInt32(dev2.VoiceSwitched),
                                                 Convert.ToInt32(dev2.Private),
                                                 string.IsNullOrEmpty(dev2.Password) ? string.Empty : dev2.Password,
                                                 dev2.Layout.LayoutNumber));
                //подсоединим участников
                foreach (VDCServerAbonentInfo item in dev2.Members)
                {
                    result.Add(new CommandDescriptor(dev2.Type.UID,
                                                     GetCommandByName(dev2, "CheckConference").Command,
                                                     dev2.ConferenceName));
                    result.Add(new CommandDescriptor(dev2.Type.UID,
                                                     GetCommandByName(dev2, "AddParticipant").Command,
                                                     dev2.ConferenceName,
                                                     item.Name,
                                                     item.Number1,
                                                     item.Number2,
                                                     item.ConnectionType.ToString(),
                                                     item.ConnectionQuality.ToString()));
                }
                if (dev2.ActiveMember != null)
                {
                    result.Add(new CommandDescriptor(dev2.Type.UID,
                                                     GetCommandByName(dev2, "CheckConference").Command,
                                                     dev2.ConferenceName));
                    result.Add(new CommandDescriptor(dev2.Type.UID,
                                                     GetCommandByName(dev2, "FocusParticipant").Command,
                                                     dev2.ActiveMember.Name));
                }
            }
            return result.ToArray();
        }
    }
}