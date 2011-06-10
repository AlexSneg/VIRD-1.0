using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.GangSwitch.SystemModule.Design;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using Hosts.Plugins.GangSwitch.SystemModule.Config;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.GangSwitch.Player
{
    internal class GangSwitchPlayerController : PlayerPlaginsHardController<GangSwitchDeviceDesign, IGangSwitchPlayerView>
    {
        public GangSwitchPlayerController(Device device, IPlayerCommand playerCommand, IGangSwitchPlayerView view, IEventLogging logging)
            : base((GangSwitchDeviceDesign)device, playerCommand, logging, view)
        {
            InitView();
            UpdateView();
        }

        private void InitView()
        {
            List<GangSwitchUnitDesign> states = Device.UnitList;
            List<GangSwitchUnitConfig> units = ((GangSwitchDeviceConfig)Device.Type).UnitList;
            foreach(GangSwitchUnitConfig unit in units)
            {
                GangSwitchUnitDesign state = states.Find(val => val.Name.Equals(unit.Name));
                View.AddSwitch(state, unit);
            }
        }

        protected override void UpdateView() 
        {
            bool isSuccess;
            int[] switchState = ExecuteCommandArrayInt32("SwitchGet", out isSuccess);
            if (isSuccess && (switchState != null) && (switchState.Length > 1))
            {
                bool[] result = new bool[Math.Min(switchState.Length - 1, switchState[0])];
                for (int i = 0; i < Math.Min(switchState.Length - 1, switchState[0]); i++)
                {
                    result[i] = Convert.ToBoolean(switchState[i + 1]);
                }
                View.UpdateView(isSuccess, result);
            }
        }
    }
}
