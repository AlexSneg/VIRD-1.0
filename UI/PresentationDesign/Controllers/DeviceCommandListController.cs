using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces.ConfigModule.Player;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using Domain.PresentationShow.ShowClient;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Domain.PresentationDesign.Client;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public class DeviceCommandListController : CommandListController
    {
        private static DeviceCommandListController _instance;
        private Device _device = null;
        private IPlayerModule _playerModule = null;

        protected DeviceCommandListController()
        {
            PresentationController.Instance.OnDeviceChanged += Instance_OnDeviceChanged;
        }

        public static void CreateController()
        {
            _instance = new DeviceCommandListController();
        }

        void Instance_OnDeviceChanged(Device newDevice)
        {
            _device = newDevice;
            if (_device != null)
            {
                _playerModule = ShowClient.Instance.GetPlayerModule(_device.Type.GetType());
            }
            FillCommandList();
            FireListChanged();
        }

        public override Control CreateManagementControl(Control parent)
        {
            if (_device != null)
                return _playerModule.CreateControlForDevice(
                    DesignerClient.Instance.ClientConfiguration.EventLog,
                    _device, 
                    parent, 
                    ShowClient.Instance,
                    DesignerClient.Instance.PresentationWorker);
            return base.CreateManagementControl(parent);
        }

        public static CommandListController Instance
        {
            get { return _instance; }
        }

        protected override void FillCommandList()
        {
            this._commandList.Clear();
            if (_device != null)
            {
                _device.Type.CommandList.ForEach(x => _commandList.Add(new KeyValuePair<String, object>(x.Name, x)));
                if (_commandList.Count > 0)
                    return;

            }
            base.FillCommandList();
        }

        public override void Dispose()
        {
            if (PresentationController.Instance != null)
                PresentationController.Instance.OnDeviceChanged -= Instance_OnDeviceChanged;
            base.Dispose();
            _instance = null;
        }
    }
}
