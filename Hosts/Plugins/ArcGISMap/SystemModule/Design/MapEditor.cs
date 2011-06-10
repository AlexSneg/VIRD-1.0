using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using Hosts.Plugins.ArcGISMap.UI;

namespace Hosts.Plugins.ArcGISMap.SystemModule.Design
{
    public class MapEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
             if(context.PropertyDescriptor.IsReadOnly) return UITypeEditorEditStyle.None;
            if (context == null || context.Instance == null) return base.GetEditStyle(context);
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null) return base.EditValue(context, provider, value);

            ArcGISMapSourceDesign source = context.Instance as ArcGISMapSourceDesign;
            if (source != null)
            {
                MapSetupForm frm = new MapSetupForm(source);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    source.RefreshMap();
                    return value == null ? "" : null;
                }
            }
            return value;
        }
    }
}
