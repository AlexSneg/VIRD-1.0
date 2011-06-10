using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace UI.Common.CommonUI.Editor
{
    public class ResourceFileInfoEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null) return base.GetEditStyle(context);
            ResourceFileInfo resourceFileInfo = context.Instance as ResourceFileInfo;
            if (resourceFileInfo != null) return UITypeEditorEditStyle.Modal;
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null) return base.EditValue(context, provider, value);

            ResourceFileInfo resourceFileInfo = context.Instance as ResourceFileInfo;
            if (resourceFileInfo == null)
                return base.EditValue(context, provider, value);

            string result;
            if ((result = ShowFileDialog(resourceFileInfo))==null)
            {
                return value;
            }
            else
            {
                //resourceFileInfo.Init(result);
                return result;
            }
        }

        public static string ShowFileDialog(ResourceFileInfo resourceFileInfo)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;
            openFile.RestoreDirectory = true;
            openFile.Filter = resourceFileInfo.Filter;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                return openFile.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}