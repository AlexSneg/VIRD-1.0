using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.StandardSource.SystemModule.Design;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.StandardSource.Player
{
    internal class StandartSourcePlayerController : PlayerPlaginsRGBController<StandardSourceDeviceDesign, IStandartSourcePlayerView>
    {
        internal StandartSourcePlayerController(IPresentationClient presClient, Source source, IStandartSourcePlayerView view, 
            IPlayerCommand playerCommand, IEventLogging logging)
            : base(presClient, (HardwareSource)source, (StandardSourceDeviceDesign)source.Device, 
                playerCommand, logging, view)
        {
            UpdateView();
        }

        protected override void UpdateView() 
        {
            try
            {
                View.UpdateView(IsOnLine);
            }
            catch(Exception ex)
            {
                Logging.WriteError(ex.Message + ". Class - " + this.ToString());
            }
        }
    }
}
