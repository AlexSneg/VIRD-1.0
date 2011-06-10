using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using Hosts.Plugins.BusinessGraphics.UI;
using dotnetCHARTING.WinForms;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    public class ChartEditor : UITypeEditor
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

            BusinessGraphicsSourceDesign source = context.Instance as BusinessGraphicsSourceDesign;
            if (source != null)
            {
                ChartSetupForm frm = new ChartSetupForm(source) { Chart = source.Chart };
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    source.RefreshChart();
                    return value == null ? "" : null;
                }
            }
            return value;
        }
    }
}
