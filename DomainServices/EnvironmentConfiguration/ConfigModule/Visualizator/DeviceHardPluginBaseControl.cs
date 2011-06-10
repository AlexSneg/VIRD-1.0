using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Entity;
using Syncfusion.Windows.Forms.Tools;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    public partial class DeviceHardPluginBaseControl : UserControl, IPlayerPlagingBaseView
    {
        private bool _isAvailable = false;
        private IPlayerPlaginsController _controller;

        public DeviceHardPluginBaseControl()
        {
            InitializeComponent();
            this.cbaApplayAllPresentation.CheckedChanged += cbaApplayAllPresentation_CheckedChanged;
        }
        public void InitializeController(IPlayerPlaginsController controller)
        {
            _controller = controller;
        }
        public void SetAvailableStatus(bool isAvailable)
        {
            gpBottomCommon.Height = isAvailable ? 22 : 38;
            _isAvailable = isAvailable;
        }
        public void UpdateFreezeStatus(FreezeStatus status)
        {
            this.cbaApplayAllPresentation.CheckedChanged -= cbaApplayAllPresentation_CheckedChanged;
            cbaApplayAllPresentation.Checked = status == FreezeStatus.Freeze ? true : false;
            this.cbaApplayAllPresentation.CheckedChanged += cbaApplayAllPresentation_CheckedChanged;
        }
        public bool IsHardwareAvailable { get { return _isAvailable; } }

        public void SetControlPlayerTimerEnable(bool enable, int? millisec)
        {
            // чтобы не переделывать все - пропихиваем в контроллер
            if (_controller != null) //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-2055
                _controller.SetControlPlayerTimerEnable(enable, millisec);
        }

        //public event System.Action ControlPlayerTimerTickEvent;
        
        public event System.Action<string, IConvertible[]> PushCommandButtonEvent;
        public event System.Action<FreezeStatus> OnFreezeStatusChanged;

        protected virtual void sendPushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            if (PushCommandButtonEvent != null)
                PushCommandButtonEvent(command, parameters);
        }

        private void cbaApplayAllPresentation_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            FreezeStatus state = cbaApplayAllPresentation.Checked ? FreezeStatus.Freeze : FreezeStatus.UnFreeze;
            if (OnFreezeStatusChanged != null)
                OnFreezeStatusChanged(state);
        }
    }
}
