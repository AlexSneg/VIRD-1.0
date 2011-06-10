using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    public partial class SourceHardPluginBaseControl : UserControl, IPlayerPlaginRGBBaseView
    {
        private IPlayerPlaginsController _controller;
        private bool _isAvailable = false;
        private bool _CollapsedRGBOption;
        private int _sourceHeight;
        private ResourceInfoForHardwareSource _resource;

        public SourceHardPluginBaseControl()
        {
            InitializeComponent();
            SetAvailableStatus(true);
        }
        public void InitializeController(IPlayerPlaginsController controller)
        {
            _controller = controller;
        }
        public void SetAvailableStatus(bool isAvailable)
        {
            alStatus.Visible = !isAvailable;
            _isAvailable = isAvailable;
        }
        public bool IsHardwareAvailable { get { return _isAvailable; } }

        //public event System.Action ControlPlayerTimerTickEvent;
        public event Action<string, IConvertible[]> PushCommandButtonEvent;
        
        public void SetControlPlayerTimerEnable(bool enable, int? millisec)
        {
            // чтобы не переделывать все - пропихиваем в контроллер
            _controller.SetControlPlayerTimerEnable(enable, millisec);
        }

        protected void InitializeData()
        {
            _sourceHeight = this.Height;
        }

        private void _OnCollapsedRGBOption(bool collapsedRGBOption)
        {
            if (collapsedRGBOption)
            {
                gpRGBOption.Visible = false;
                baRGBOption.Text = baRGBOption.Text.Replace('<', '>');
                this.Height = _sourceHeight - gpRGBOption.Height;
            }
            else
            {
                gpRGBOption.Visible = true;
                baRGBOption.Text = baRGBOption.Text.Replace('>', '<');
                this.Height = _sourceHeight;
            }
        }
        protected virtual void sendPushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            if (PushCommandButtonEvent != null)
                PushCommandButtonEvent(command, parameters);
        }
        public bool CollapsedRGBOption
        {
            get { return _CollapsedRGBOption; }
            set 
            { 
                _CollapsedRGBOption = value;
                _OnCollapsedRGBOption(_CollapsedRGBOption);
            }
        }

        private void baRGBOption_Click(object sender, EventArgs e)
        {
            CollapsedRGBOption = !CollapsedRGBOption;
        }

        public void UpdateRGBSettings(ResourceInfoForHardwareSource settings)
        {
            _resource = settings;
            // H (Height) мапится на ResourceInfo_V (Vertical) 
            // W (Width) мапится на ResourceInfo_H (Horizontal) 
            nudTotalH.Value = _resource.RGBParam.VTotal;
            nudTotalW.Value = _resource.RGBParam.HTotal;
            nudOffsetH.Value = _resource.RGBParam.VOffset;
            nudOffsetW.Value = _resource.RGBParam.HOffset;
            nudActiveH.Value = _resource.RGBParam.VHeight;
            nudActiveW.Value = _resource.RGBParam.HWidth;
            nudVFreq.Value = _resource.RGBParam.VFreq;
        }
        public event Action<ResourceInfoForHardwareSource> OnRGBSettingsChanged;
        private void buttonAdv1_Click(object sender, EventArgs e)
        {
            // ResourceInfo_V (Vertical) мапится на H (Height) 
            // ResourceInfo_H (Horizontal) мапится на W (Width) 
            _resource.RGBParam.HTotal = (short)nudTotalW.Value;
            _resource.RGBParam.VTotal = (short)nudTotalH.Value;
            _resource.RGBParam.HOffset = (short)nudOffsetW.Value;
            _resource.RGBParam.VOffset = (short)nudOffsetH.Value;
            _resource.RGBParam.VHeight = (short)nudActiveH.Value;
            _resource.RGBParam.HWidth = (short)nudActiveW.Value;
            _resource.RGBParam.VFreq = (short)nudVFreq.Value;
            if (OnRGBSettingsChanged != null)
                OnRGBSettingsChanged(_resource);
        }
    }
}
