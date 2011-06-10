using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;
using TechnicalServices.Entity;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Player
{
    public abstract class PlayerPlaginsHardController<TDevice, TView>
        : PlayerPlaginsController<TDevice, TView>
        where TDevice : Device
        where TView : IPlayerPlaginHardBaseView
    {
        public PlayerPlaginsHardController(TDevice device, IPlayerCommand playerCommand, 
            IEventLogging logging, TView view) 
            : base(device, playerCommand, logging, view)
        {
            View.UpdateFreezeStatus(PlayerCommand.GetFreezedEquipment(device.Type));
            View.OnFreezeStatusChanged += View_OnFreezeStatusChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            View.OnFreezeStatusChanged -= View_OnFreezeStatusChanged;
        }
        private void View_OnFreezeStatusChanged(FreezeStatus state)
        {
            PlayerCommand.FreezeEquipmentSetting(Device.Type, state);
        }
    }
}
