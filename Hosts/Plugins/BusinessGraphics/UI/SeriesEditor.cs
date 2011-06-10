using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design;

namespace Hosts.Plugins.BusinessGraphics.UI
{
    public class SeriesEditor : UITypeEditor
    {
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
                    string[] strings = value.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> sourceSeries = ((BusinessGraphicsSourceDesign)context.Instance).GetSeriesList();
                    strings = newVisibleSeries(sourceSeries, strings);

                    SeriesEditControl flctrl = new SeriesEditControl(sourceSeries, strings.ToList());
                    flctrl.Tag = svc;
                    svc.DropDownControl(flctrl);
                    value = flctrl.VisibleSeries;

                }
            }

            return base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(
          ITypeDescriptorContext context)
        {
            if (context != null)
                return UITypeEditorEditStyle.DropDown;
            else
                return base.GetEditStyle(context);
        }

        /// <summary>обновляет список видимых серий, может получится что источник изменился, 
        /// а там совсем другие серии </summary>
        private string[] newVisibleSeries(List<string> source, string[] current)
        {
            List<string> result = new List<string>();
            foreach (string item in current)
            {
                if (source.Contains(item))
                    result.Add(item);
            }
            if (result.Count == 0)
                result.AddRange(source);
            return result.ToArray();
        }
    }
}
