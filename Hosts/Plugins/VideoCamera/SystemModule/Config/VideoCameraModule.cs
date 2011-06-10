using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;
using Hosts.Plugins.VideoCamera.SystemModule.Design;

namespace Hosts.Plugins.VideoCamera.SystemModule.Config
{
    public sealed class VideoCameraModule : ConfigurationModule
    {
        public override Type[] GetSource()
        {
            return new[] { typeof(VideoCameraSourceConfig) };
        }

        public override Type[] GetDevice()
        {
            return new[] { typeof(VideoCameraDeviceConfig) };
        }

        public override Type[] GetExtensionType()
        {
            return new[] { typeof(VideoCameraResourceInfo), typeof(VideoCameraDeviceResourceInfo) };
        }
    }
}