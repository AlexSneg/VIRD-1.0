using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using Hosts.Plugins.AudioMixer.SystemModule.Config;
using Hosts.Plugins.AudioMixer.UI;

namespace Hosts.Plugins.AudioMixer.SystemModule.Design.TypeEditors
{
    class SceneEditor : UITypeEditor
    {
        /// <summary>
        /// Реализация метода редактирования
        /// </summary>
        public override Object EditValue(
          ITypeDescriptorContext context,
          IServiceProvider provider,
          Object value)
        {
            if ((context != null) && (provider != null))
            {
                IWindowsFormsEditorService svc =
                  (IWindowsFormsEditorService)
                  provider.GetService(typeof(IWindowsFormsEditorService));

                if (svc != null)
                {
                    
                    AudioMixerDeviceDesign device = ((AudioMixerDeviceDesign)context.Instance);
                    AudioMixerDeviceConfig deviceType =((AudioMixerDeviceConfig)device.Type);
                    int inputListCount = ((AudioMixerDeviceConfig)device.Type).InputList.Count;
                    int fadersCount = device.FaderGroupList.Sum(fGroup => fGroup.FaderList.Count);
                    using (SceneEditorForm form =
                      new SceneEditorForm(inputListCount, fadersCount))
                    {
                        IAudioMixerFullView _fullView = form.view;
                        _fullView.InitializeMatrix(device.HasMatrix, Convert.ToInt32(deviceType.InstanceID),
                            ((AudioMixerDeviceConfig)device.Type).InputList,
                            ((AudioMixerDeviceConfig)device.Type).OutputList,
                            device.GetMatrixUnit);
                        _fullView.InitializeFaderGroups(device.FaderGroupList);

                        if (svc.ShowDialog(form) == DialogResult.OK)
                        {
                            device.Matrix = form.view.GetMatrixState;
                            form.view.SaveFaders(device.FaderGroupList);
                            return value == null ? "" : null ;
                        }

                        
                    }
                }
            }

            return base.EditValue(context, provider, value);
        }

        /// <summary>
        /// Возвращаем стиль редактора - модальное окно
        /// </summary>
        public override UITypeEditorEditStyle GetEditStyle(
          ITypeDescriptorContext context)
        {
            if (context != null)
                return UITypeEditorEditStyle.Modal;
            else
                return base.GetEditStyle(context);
        }
    }

}
