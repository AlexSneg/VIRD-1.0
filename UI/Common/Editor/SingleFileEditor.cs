using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace UI.Common.CommonUI.Editor
{
    public abstract class SingleFileEditor: UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null) return base.GetEditStyle(context);
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null) return base.EditValue(context, provider, value);

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;
            openFile.RestoreDirectory = true;
            openFile.Filter = this.GetFilter();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                return openFile.FileName;
            }
            else
            {
                return null;
            }    
            
        }

        protected virtual string GetFilter()
        {
            return "Все файлы|*.* ";
        }
    }
}
