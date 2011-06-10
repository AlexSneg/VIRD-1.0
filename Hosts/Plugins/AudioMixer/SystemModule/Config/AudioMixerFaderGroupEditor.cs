using System;
using System.Linq;
using System.Windows.Forms;

using Syncfusion.Windows.Forms;

using TechnicalServices.Common.Editor;
using System.Diagnostics;

namespace Hosts.Plugins.AudioMixer.SystemModule.Config
{
    internal class AudioMixerFaderGroupEditor : ClonableObjectCollectionEditorAdv<AudioMixerFaderGroupConfig>
    {
        public AudioMixerFaderGroupEditor(Type type)
            : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            AudioMixerDeviceConfig device = (AudioMixerDeviceConfig) Context.Instance;
            if (device.FaderGroupList.Count >= 32)
            {
                //throw new IndexOutOfRangeException("Количество групп фейдеров не может превышать 32");
                Control owner = Form.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
                MessageBox.Show(owner, "Количество групп фейдеров не может превышать 32", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            AudioMixerFaderGroupConfig unit = (AudioMixerFaderGroupConfig) base.CreateInstance(itemType);
            int num = device.FaderGroupList.Count + 1;
            unit.Name = "Группа фейдеров " + num;
            return unit;
        }

        protected override void OnRemoveItem(AudioMixerFaderGroupConfig sender)
        {
            AudioMixerDeviceConfig device = (AudioMixerDeviceConfig) Context.Instance;
            device.FaderGroupList.Remove(device.FaderGroupList.FirstOrDefault(i => i.Name == sender.Name));
        }
    }
}