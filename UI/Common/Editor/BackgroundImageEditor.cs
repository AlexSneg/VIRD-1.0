using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;

using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.PowerPointLib;

namespace UI.Common.CommonUI.Editor
{
    public class BackgroundImageEditor : UITypeEditor
    {
        private const string pptExtension = @".ppt";

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null) return base.GetEditStyle(context);
            ActiveDisplay activeDisplay = context.Instance as ActiveDisplay;
            if (activeDisplay != null) return UITypeEditorEditStyle.Modal;
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
                return base.EditValue(context, provider, value);

            ActiveDisplay activeDisplay = context.Instance as ActiveDisplay;
            if (activeDisplay == null)
                return base.EditValue(context, provider, value);
            string result;

            if ((result = ShowFileDialog(activeDisplay)) == null)
            {
                return value;
            }
            else
            {
                if (Path.GetExtension(result).ToLower().Equals(pptExtension))
                {
                    PowerPointForm pf = new PowerPointForm();
                    pf.pathToPpt = result;
                    pf.ShowDialog();
                }
                return result;
            }
        }

        public static string ShowFileDialog(ActiveDisplay activeDisplay)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;
            openFile.RestoreDirectory = true;
            openFile.Filter = @"PowerPoint Presentations|*.ppt";
            ;
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