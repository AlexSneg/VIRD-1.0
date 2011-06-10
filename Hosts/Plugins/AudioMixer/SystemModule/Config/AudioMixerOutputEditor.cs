using System;

using TechnicalServices.Common.Editor;

namespace Hosts.Plugins.AudioMixer.SystemModule.Config
{
    internal class AudioMixerOutputEditor : ClonableObjectCollectionEditorAdv<AudioMixerOutput>
    {
        public AudioMixerOutputEditor(Type type)
            : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            AudioMixerDeviceConfig device = (AudioMixerDeviceConfig) Context.Instance;
            AudioMixerOutput unit = (AudioMixerOutput) base.CreateInstance(itemType);
            unit.Name = "Выход" + (device.OutputList.Count + 1);
            return unit;
        }
    }
}