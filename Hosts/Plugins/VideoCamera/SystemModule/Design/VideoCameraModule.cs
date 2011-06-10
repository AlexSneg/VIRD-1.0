using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;

namespace Hosts.Plugins.VideoCamera.SystemModule.Design
{
    public sealed class VideoCameraModule : PresentationModule
    {
        public override Type[] GetSource()
        {
            return new[] { typeof(VideoCameraSourceDesign) };
        }

        public override Type[] GetDevice()
        {
            return new[] { typeof(VideoCameraDeviceDesign) };
        }
    }
}