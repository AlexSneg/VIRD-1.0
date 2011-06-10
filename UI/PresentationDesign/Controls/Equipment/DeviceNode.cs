using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Tools;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace UI.PresentationDesign.DesignUI.Controls.Equipment
{
    public class DeviceNode : TreeNodeAdv
    {
        public Device Device
        {
            get;
            set;
        }

        public DeviceType DeviceType
        {
            get;
            set;
        }

        public DeviceNode(Device ADevice)
            : base(ADevice.Type.Name)
        {
            this.Device = ADevice;
        }

        public DeviceNode(DeviceType ADeviceType)
            : base(ADeviceType.Name)
        {
            this.DeviceType = ADeviceType;
            this.Device = ADeviceType.CreateNewDevice();
        }

    }
}
