using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.AudioMixer.SystemModule.Config;
using Hosts.Plugins.AudioMixer.SystemModule.Design;

using TechnicalServices.HardwareEquipment.Util;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.AudioMixer.Server
{
    public sealed class AudioMixerServerModule :
        HardwareEquipmentServerModule<SourceType, AudioMixerDeviceConfig, DisplayType>
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.AudioMixer);
        }

        protected override CommandDescriptor[] GetCommand(Device device1, Device device2)
        {
            List<CommandDescriptor> result = new List<CommandDescriptor>();
            AudioMixerDeviceDesign dev1 = device1 as AudioMixerDeviceDesign;
            AudioMixerDeviceDesign dev2 = device2 as AudioMixerDeviceDesign;
            if (dev2 != null)
            {
                if (dev2.HasMatrix)
                {
                    int i = 0;
                    int matrixId = Convert.ToInt32(((AudioMixerDeviceConfig) dev2.Type).InstanceID);
                    foreach (AudioMixerInput input in ((AudioMixerDeviceConfig) dev2.Type).InputList)
                    {
                        i++;
                        int j = 0;
                        foreach (AudioMixerOutput output in ((AudioMixerDeviceConfig) dev2.Type).OutputList)
                        {
                            j++;
                            int state = Convert.ToInt32(dev2.GetMatrixUnit(input, output));
                            result.Add(new CommandDescriptor(dev2.Type.UID,
                                                             GetCommandByName(dev2, "MixerSetTie").Command,
                                                             matrixId, j, i, state));
                        }
                    }
                }
                foreach (AudioMixerFaderGroupDesign group in dev2.FaderGroupList)
                {
                    foreach (AudioMixerFaderDesign fader in group.FaderList)
                    {
                        result.Add(new CommandDescriptor(dev2.Type.UID,
                                                         GetCommandByName(dev2, "MixerSetFader").Command,
                                                         Convert.ToInt32(fader.InstanceID), Convert.ToInt32(fader.Mute),
                                                         fader.BandValueNormalize));
                    }
                }
            }
            return result.ToArray();
        }
    }
}