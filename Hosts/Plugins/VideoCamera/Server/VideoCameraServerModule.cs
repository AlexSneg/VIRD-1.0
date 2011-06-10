using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.VideoCamera.SystemModule.Config;
using Hosts.Plugins.VideoCamera.SystemModule.Design;

using TechnicalServices.HardwareEquipment.Util;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Hosts.Plugins.VideoCamera.Player;

namespace Hosts.Plugins.VideoCamera.Server
{
    public sealed class VideoCameraServerModule :
        HardwareEquipmentServerModule<VideoCameraSourceConfig, VideoCameraDeviceConfig, DisplayType>
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.VideoCamera);
        }

        protected override CommandDescriptor[] GetCommand(Device device1, Device device2)
        {
            List<CommandDescriptor> result = new List<CommandDescriptor>();
            VideoCameraDeviceDesign dev1 = device1 as VideoCameraDeviceDesign;
            VideoCameraDeviceDesign dev2 = device2 as VideoCameraDeviceDesign;
            if (dev2 != null)
            {
                if (dev2.HasPreciseControl)
                {
                    result.Add(new CommandDescriptor(dev2.Type.UID,
                                                     GetCommandByName(dev2, "CamSetPos").Command,
                                                     VideoCameraPlayerCommonController.normalizeValue(dev2.Pan, 0, 360),
                                                     VideoCameraPlayerCommonController.normalizeValue(dev2.Tilt, -180, 180),
                                                     VideoCameraPlayerCommonController.normalizeValue((int)dev2.Zoom, dev2.LowZoomBoundary, dev2.HighZoomBoundary)));
                }
                else
                {
                    result.Add(new CommandDescriptor(dev2.Type.UID,
                                                     GetCommandByName(dev2, "CamPresetLoad").Command,
                                                     dev2.Preset));
                }
            }
            return result.ToArray();
        }
    }
}