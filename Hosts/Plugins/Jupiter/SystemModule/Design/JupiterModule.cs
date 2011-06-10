using System;
using DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Collections.Generic;

namespace Hosts.Plugins.Jupiter.SystemModule.Design
{
    public sealed class JupiterModule : PresentationModule
    {
        internal static Dictionary<JupiterWindow, JupiterDisplayDesign> _windowMapping = new Dictionary<JupiterWindow, JupiterDisplayDesign>();
        internal static Dictionary<JupiterWindow, Slide> _slideMapping = new Dictionary<JupiterWindow, Slide>();

        public override Type[] GetDevice()
        {
            return new[] { typeof(JupiterDeviceDesign) };
        }

        public override Type[] GetDisplay()
        {
            return new[] { typeof(JupiterDisplayDesign) };
        }

        public override Type[] GetWindow()
        {
            return new[] { typeof(JupiterWindow), typeof(ActiveWindow) };
        }
    }
}