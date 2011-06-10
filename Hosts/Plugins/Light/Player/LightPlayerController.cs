using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.Light.SystemModule.Design;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.Light.SystemModule.Config;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.Light.Player
{
    internal class LightPlayerController : PlayerPlaginsHardController<LightDeviceDesign, ILightPlayerView>
    {
        internal LightPlayerController(Device device, IPlayerCommand playerProvider, ILightPlayerView view, IEventLogging logging)
            : base((LightDeviceDesign)device, playerProvider, logging, view)
        {
            InitView();
            //if (IsShow)
                UpdateView();
        }

        private void InitView()
        {
            List<LightUnitDesign> states = Device.UnitList;
            foreach(LightUnitDesign item in states)
            {
                View.AddLightGroup(item);
            }
        }

        protected override void UpdateView()
        {
            int i=0;
            foreach (LightUnitConfig unitConfig in ((LightDeviceConfig)Device.Type).UnitList)
            {
                LightUnitDesign unit = new LightUnitDesign(unitConfig);
                i++;
                bool isSuccess;
                if (unit.IsAdjustable)
                    unit.Brightness = ExecuteCommandInt32("LightGetGroupLevel", out isSuccess, i);
                else
                    unit.Brightness = ExecuteCommandInt32("LightGetGroupState", out isSuccess, i);
                View.UpdateView(isSuccess, unit);
            }
            
        }

        protected override string ParseCommandAnswerParameter(string parameter, EquipmentCommand cmd)
        {
            if (parameter.IndexOf('(') == -1) return parameter;
            string res = parameter.Substring(parameter.IndexOf('(') + 1, parameter.IndexOf(')') - parameter.IndexOf('(') - 1);
            string[] ress = res.Split(',');
            return ress[1];
        }

    }
}
