using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.AudioMixer.SystemModule.Design
{
    public sealed class AudioMixerModule : PresentationModule
    {
        public override Type[] GetDevice()
        {
            return new[] { typeof(AudioMixerDeviceDesign) };
        }
    }
}