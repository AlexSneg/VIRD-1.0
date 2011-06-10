using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.AudioMixer.SystemModule.Config
{
    public sealed class AudioMixerModule : ConfigurationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(AudioMixerDeviceConfig) };
        }
    }
}