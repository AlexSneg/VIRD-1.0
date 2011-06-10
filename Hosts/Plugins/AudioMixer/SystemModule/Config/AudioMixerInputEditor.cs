using System;

using TechnicalServices.Common.Editor;

namespace Hosts.Plugins.AudioMixer.SystemModule.Config
{
    internal class AudioMixerInputEditor : ClonableObjectCollectionEditorAdv<AudioMixerInput>
    {
        public AudioMixerInputEditor(Type type)
            : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            AudioMixerDeviceConfig device = (AudioMixerDeviceConfig) Context.Instance;
            AudioMixerInput unit = (AudioMixerInput) base.CreateInstance(itemType);
            unit.Name = "Вход" + (device.InputList.Count + 1);
            return unit;
        }
    }
}