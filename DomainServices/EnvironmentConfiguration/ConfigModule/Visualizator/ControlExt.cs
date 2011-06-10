using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    public static class ControlExt
    {
        public static void InitBorderTitle(this Control ctrl, Window window, Control child)
        {
            if (window is ActiveWindow)
            {
                ActiveWindow wnd = (ActiveWindow)window;
                if (ctrl.Controls.ContainsKey("LabelTitle")) ctrl.Controls.RemoveByKey("LabelTitle");
                if (wnd.BorderVisible)
                {
                    ctrl.SuspendLayout();
                    child.SuspendLayout();
                    child.Left += wnd.BorderWidth;
                    child.Width -= wnd.BorderWidth << 1;
                    child.Height -= wnd.BorderWidth;

                    System.Windows.Forms.Label title = new System.Windows.Forms.Label();
                    title.AutoSize = false;
                    title.Name = "LabelTitle";
                    title.BackColor = wnd.BorderColorFrienly;
                    title.Font = new Font(wnd.TitleFont, wnd.TitleSize);
                    title.ForeColor = wnd.TitleColorFrienly;
                    title.Text = wnd.TitleText;
                    title.Left = wnd.BorderWidth;
                    title.Top = 0;
                    using (Graphics g = title.CreateGraphics())
                    {
                        title.Size = g.MeasureString(wnd.TitleText, title.Font).ToSize();
                    }
                    title.Width = child.Width;
                    title.AutoEllipsis = true;
                    title.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                    ctrl.Controls.Add(title);
                    if (title.Height == 0)
                    {
                        child.Top += wnd.BorderWidth;
                        child.Height -= wnd.BorderWidth;
                    }
                    else
                    {
                        child.Top += title.Height;
                        child.Height -= title.Height;
                    }

                    ctrl.BackColor = wnd.BorderColorFrienly;
                    child.ResumeLayout();
                    ctrl.ResumeLayout();
                    title.Invalidate();
                }
                ctrl.Text = wnd.TitleText;
            }
        }
    }
}
