using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.Monitor.SystemModule.Config;

using TechnicalServices.HardwareEquipment.Util;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.Monitor.Server
{
    public sealed class MonitorServerModule :
        HardwareEquipmentServerModule<SourceType, DeviceType, MonitorDisplayConfig>
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.Monitor);
        }

        public override void ShowDisplay(Display display, BackgroundImageDescriptor backgroundImageDescriptor)
        {
        }

        //public override void ShowDisplay(Display display, string presentationUniqueName)
        //{
        //    foreach (Mapping mapping in display.Type.MappingList)
        //    {
        //        foreach (string command in mapping.CommandList)
        //        {
        //            CommandDescriptor cmd = new CommandDescriptor(0, command);
        //            _controller.Send(cmd);
        //        }
        //    }
        //}

        protected override CommandDescriptor[] GetCommand(Display display1, Display display2)
        {
            //return base.GetCommand(display1, display2);

            if (display1 != null && display1.WindowList.Count > 1)
                throw new IndexOutOfRangeException("Monitor display1");
            if (display2 != null && display2.WindowList.Count > 1)
                throw new IndexOutOfRangeException("Monitor display2");
            if (display1 != null && display1.WindowList.Count == 1 &&
                !(display1.WindowList[0].Source.Type is HardwareSourceType))
                throw new ArgumentException("Monitor display1");
            if (display2 != null && display2.WindowList.Count == 1 &&
                !(display2.WindowList[0].Source.Type is HardwareSourceType))
                throw new ArgumentException("Monitor display2");

            int x, y;

            List<CommandDescriptor> list = new List<CommandDescriptor>(2);
            if (display1 == null && display2 != null)
            {
                if (display2.WindowList.Count == 1)
                {
                    x = ((HardwareSourceType) display2.WindowList[0].Source.Type).Input;
                    y = ((PassiveDisplayType) display2.Type).Output;
                    list.Add(new CommandDescriptor(0, "LogicSetTie", x, y, 1));
                }
            }
            if (display1 != null && display2 != null)
            {
                if (display1.WindowList.Count == 1)
                {
                    x = ((HardwareSourceType) display1.WindowList[0].Source.Type).Input;
                    y = ((PassiveDisplayType) display1.Type).Output;
                    list.Add(new CommandDescriptor(0, "LogicSetTie", x, y, 0));
                }

                if (display2.WindowList.Count == 1)
                {
                    x = ((HardwareSourceType) display2.WindowList[0].Source.Type).Input;
                    y = ((PassiveDisplayType) display2.Type).Output;
                    list.Add(new CommandDescriptor(0, "LogicSetTie", x, y, 1));
                }
            }

            return list.ToArray();
        }
    }
}