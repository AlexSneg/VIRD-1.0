using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using TechnicalServices.Common.ReadOnly;

namespace Hosts.Plugins.VDCServer.SystemModule.Design.TypeEditors
{
    public class MembersEditor : UITypeEditor
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
                    VDCServerDeviceDesign device = 
                        (context.Instance is ReadOnlyObject) ?
                            (VDCServerDeviceDesign)((ReadOnlyObject)context.Instance).Wrapped :
                            (VDCServerDeviceDesign)context.Instance;

                    List<VDCServerAbonentInfo> list = device.AbonentList;
                    List<VDCServerAbonentInfo> selected;
                    if (value is ReadOnlyCollection)
                    {
                        selected = new List<VDCServerAbonentInfo>();
                        foreach (ReadOnlyObject info in (ICollection)value)
                            selected.Add((VDCServerAbonentInfo)info.Wrapped);
                    }
                    else
                        selected = (List<VDCServerAbonentInfo>)value;                    

                    using (MembersListForm ipfrm =
                        new MembersListForm(list, selected, value is ReadOnlyCollection))
                    {
                        if (svc.ShowDialog(ipfrm) == DialogResult.OK)
                        {
                            if (!(value is ReadOnlyCollection))
                                value = ipfrm.values;
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