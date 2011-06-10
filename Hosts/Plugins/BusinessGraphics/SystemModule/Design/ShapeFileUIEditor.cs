using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;


namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    public abstract class SingleFileEditor : UITypeEditor
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

    public class ShapeFileUIEditor : SingleFileEditor
    {
        protected override string GetFilter()
        {
            return "Shape-файлы (*.shp)|*.shp|Все файлы (*.*)|*.*";
        }
    }

    public class MyXMLFileUIEditor : SingleFileEditor
    {
        protected override string GetFilter()
        {
            return "XML-файлы (*.xml)|*.xml|Все файлы (*.*)|*.*";
        }
    }
}
