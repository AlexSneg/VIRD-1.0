using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Design.TypeEditors
{



    public class ExportFileEditor : UITypeEditor 
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
                    using (ExportFileForm ipfrm =
                      new ExportFileForm(((VDCTerminalResourceInfo)context.Instance).AbonentList))
                    {
                        if (svc.ShowDialog(ipfrm) == DialogResult.OK)
                        {
                            value = true;
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