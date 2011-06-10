using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
//using PrnHost.OfficeUserControl;


namespace TechnicalServices.PowerPointLib
{
    public partial class PowerPointControl : UserControl
    {
        public PowerPointControl()
        {
            InitializeComponent();
        }
        private ApplicationClass _app;
        private Presentations _presentations;
        private Presentation _presentation;
        private IntPtr _handler;
        public void CreateApplication()
        {
            if (_app != null) return;
            _app = new ApplicationClass();
            _handler = new IntPtr(_app.HWND);
            
            PowerPointUtils.SetParent(_handler, panel2.Handle);

            AdjustMenu(false);
            
            uint style = (uint)PowerPointUtils.GetWindowLong(_handler, PowerPointUtils.GWL_STYLE);
            style &= ~(uint)(PowerPointUtils.WindowStyles.WS_CAPTION | PowerPointUtils.WindowStyles.WS_SYSMENU |
                PowerPointUtils.WindowStyles.WS_SIZEBOX | PowerPointUtils.WindowStyles.WS_BORDER |
                PowerPointUtils.WindowStyles.WS_DLGFRAME | PowerPointUtils.WindowStyles.WS_THICKFRAME);

            PowerPointUtils.SetWindowLong(_handler, PowerPointUtils.GWL_STYLE, (IntPtr)style);

            _app.Visible = MsoTriState.msoTrue;
            PowerPntControl_SizeChanged(this, EventArgs.Empty);

        }

        public void LoadFile(string fileName)
        {
            _presentations = _app.Presentations;
            _presentation = _presentations.Open(fileName,
                                                MsoTriState.msoFalse,
                                                MsoTriState.msoFalse,
                                                MsoTriState.msoTrue);
        }

        public void AdjustMenu (bool valueType)
        {
            for (int i = 1; i <= _app.CommandBars.Count; i++)
            {
                CommandBar bar = _app.CommandBars[i];
                if (bar.Name == "Menu Bar")
                {
                    for (int j = 1; j <= bar.accChildCount; j++)
                    {
                        CommandBarControl control = (CommandBarControl)bar.get_accChild(j);
                        if (control.Caption == @"&File")
                        {
                            for (int k = 1; k <= control.accChildCount; k++)
                            {
                                CommandBarControl chControl = (CommandBarControl)control.get_accChild(k);
                                if (chControl.Caption == @"Save &As...")
                                {
                                    chControl.Enabled = valueType;
                                }
                            }
                        }
                    }
                    bar.Protection = MsoBarProtection.msoBarNoCustomize;
                    bar.Enabled = true;
                    bar.Visible = true;
                    _app.CommandBars.DisableCustomize = true;
                }
            }
        }

        public void CloseApplication()
        {

            if (_presentation != null)
            {
                _presentation.Close();
                Marshal.ReleaseComObject(_presentation);
                _presentation = null;
            }

            if (_presentations != null)
            {
                Marshal.ReleaseComObject(_presentations);
                _presentations = null;
            }
            if (_app != null)
            {
                _handler = IntPtr.Zero;

                object saveChanges = false;
                _app.Quit();
                _app = null;
            }
        }

        private void PowerPntControl_SizeChanged(object sender, EventArgs e)
        {
            if (_handler == IntPtr.Zero) return;

            int borderWidth = SystemInformation.Border3DSize.Width;
            int borderHeight = SystemInformation.Border3DSize.Height;
            int captionHeight = SystemInformation.CaptionHeight;
            int statusHeight = SystemInformation.ToolWindowCaptionHeight;

            PowerPointUtils.MoveWindow(_handler,
                -(borderWidth << 1),
                -(borderHeight << 1),
                this.panel2.Width + (borderWidth << 2),
                this.panel2.Height + (borderHeight << 2) + captionHeight,
                true);

        }
    }
}
