using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Hosts.Plugins.VideoCamera.SystemModule.Design;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VideoCamera.Player
{
    internal class VideoCameraPlayerCommonController : PlayerPlaginsRGBController<VideoCameraDeviceDesign, IVideoCameraPlayerCommonView>
    {
        private int _currentPreset = 0;
        private IVideoCameraPlayerView _preciseView;

        internal VideoCameraPlayerCommonController(IPresentationClient presClient, Source source,
            IVideoCameraPlayerCommonView view, IPlayerCommand playerCommand, IEventLogging logging)
            : base(presClient, (HardwareSource)source,
            (VideoCameraDeviceDesign)source.Device, playerCommand, logging, view)
        {
            _currentPreset = Device.Preset;
            View.InitializeData(Device);
            if (Device.HasPreciseControl)
            {
                View.PushCommandButtonEvent -= ViewPushCommandButtonEvent;
                View.PushCommandButtonEvent += detailsForm_PushCommandButtonEvent;
            }
            View.UpCommandButtonEvent += _view_UpCommandButtonEvent;
            View.DetailExecuteEvent += _view_DetailExecuteEvent;
            UpdateView();
        }

        private void _view_DetailExecuteEvent()
        {
            VideoCameraPlayerExtForm detailsForm = new VideoCameraPlayerExtForm(Device.Name);
            _preciseView = detailsForm.videoCameraPlayerControl1;
            _preciseView.InitializeData(Device.PresetAmount, _currentPreset, Device.IsDomical,
                Device.LowZoomBoundary, Device.HighZoomBoundary);
            ValueThree<int, int, int> state = new ValueThree<int, int, int>(Device.Pan, Device.Tilt, (int)Device.Zoom);
            _preciseView.UpdateView(state);
            UpdateView();
            _preciseView.PushCommandButtonEvent += detailsForm_PushCommandButtonEvent;
            detailsForm.ShowDialog();
            _preciseView.PushCommandButtonEvent -= detailsForm_PushCommandButtonEvent;
            _preciseView = null;
        }

        private void detailsForm_PushCommandButtonEvent(string command, IConvertible[] parameters)
        {
            bool isSuccess;
            switch (command)
            {
                case "CamPresetSave":
                    UpdateView();
                    Device.SavePreset((int)parameters[0]);
                    SaveDeviceToServer();
                    break;
                case "CamPresetLoad":
                    _currentPreset = (int)parameters[0];
                    //Device.LoadPreset((int)parameters[0]);
                    Device.Preset = _currentPreset;
                    CamSetPos(Device.Pan, Device.Tilt, (int)Device.Zoom);
                    UpdateView();
                    break;
                case "CamHome":
                    Device.Home();
                    CamSetPos(Device.Pan, Device.Tilt, (int)Device.Zoom);
                    UpdateView();
                    break;
                case "CamSetPos":
                    CamSetPos((int)parameters[0], (int)parameters[1], (int)parameters[2]);
                    break;
                default:
                    ExecuteCommand(command, out isSuccess, parameters);
                    break;
            }
        }

        private void SaveDeviceToServer()
        {
            try
            {
                if (_presClient != null && Device.DeviceResourceDescriptor != null)
                {
                    FileSaveStatus status = _presClient.SaveDeviceSource(Device.DeviceResourceDescriptor);
                    if (status != FileSaveStatus.Ok)
                    {
                        Logging.WriteWarning("Не удалось сохранить файл на сервер, причина - " + status.ToString() +
                            ". Class - " + this.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError(ex.Message + ". Class - " + this.ToString());
            }
        }

        private void CamSetPos(int pan, int tilt, int zoom)
        {
            bool isSuccess;
            ExecuteCommand("CamSetPos", out isSuccess,
                normalizeValue(pan, 0, 360),
                normalizeValue(tilt, -180, 180),
                normalizeValue(zoom, Device.LowZoomBoundary, Device.HighZoomBoundary));

        }
        internal static int normalizeValue(int value, decimal minValue, decimal maxValue)
        {
            if (minValue == maxValue) return 0;
            return (int)Math.Round(((decimal)value - minValue) / (maxValue - minValue) * 100);
        }
        internal static int deNormalizeValue(int norm, decimal minValue, decimal maxValue)
        {
            return (int)Math.Round((decimal)norm * (maxValue - minValue) / 100 + minValue);
        }

        private void _view_UpCommandButtonEvent()
        {
            bool isSuccess;
            ExecuteCommand("CamStop", out isSuccess, null);
        }

        protected override void UpdateView()
        {
            ValueThree<int, int, int> state = null;
            if (Device.HasPreciseControl)
            {
                bool isSuccess = false;
                int[] tmpResult = ExecuteCommandArrayInt32("CamGetPos", out isSuccess);
                if (isSuccess && (tmpResult != null) && (tmpResult.Length >= 3))
                {
                    state = new ValueThree<int, int, int>(
                        deNormalizeValue(tmpResult[0], 0, 360),
                        deNormalizeValue(tmpResult[1], -180, 180),
                        deNormalizeValue(tmpResult[2], Device.LowZoomBoundary, Device.HighZoomBoundary));
                    
                    Device.Pan = state.Value1;
                    Device.Tilt = state.Value2;
                    Device.Zoom = state.Value3;
                }
            }
            if ((_preciseView != null) && (state != null))
               _preciseView.UpdateView(state);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (Device.HasPreciseControl)
                View.PushCommandButtonEvent -= detailsForm_PushCommandButtonEvent;
            View.UpCommandButtonEvent -= _view_UpCommandButtonEvent;
            View.DetailExecuteEvent -= _view_DetailExecuteEvent;
        }
    }
}
