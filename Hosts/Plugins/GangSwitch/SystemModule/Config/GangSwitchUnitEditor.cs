using System;
using System.Linq;
using System.Windows.Forms;

using TechnicalServices.Common.Editor;
using System.Diagnostics;

namespace Hosts.Plugins.GangSwitch.SystemModule.Config
{
    internal class GangSwitchUnitEditor : ClonableObjectCollectionEditorAdv<GangSwitchUnitConfig>
    {
        public GangSwitchUnitEditor(Type type)
            : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            GangSwitchDeviceConfig device = (GangSwitchDeviceConfig)Context.Instance;
            if (device.UnitList.Count >= 100)
            {
                //throw new IndexOutOfRangeException("Количество переключателей не может превышать 100");
                Control owner = Form.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
                MessageBox.Show(owner, "Количество переключателей не может превышать 100", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            GangSwitchUnitConfig unit = (GangSwitchUnitConfig)base.CreateInstance(itemType);
            int num = device.UnitList.Count + 1;
            unit.Name = "Переключатель" + num;
            unit.OnStateName = "Вкл" + num;
            unit.OffStateName = "Выкл" + num;
            return unit;
        }

        protected override void OnRemoveItem(GangSwitchUnitConfig sender)
        {
            GangSwitchDeviceConfig device = (GangSwitchDeviceConfig)Context.Instance;
            device.UnitList.Remove(device.UnitList.FirstOrDefault(i => i.Name == sender.Name));
        }
    }
}