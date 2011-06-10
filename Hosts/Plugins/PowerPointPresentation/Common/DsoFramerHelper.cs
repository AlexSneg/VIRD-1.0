using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Hosts.Plugins.PowerPointPresentation.Common
{
    internal class DsoFramerHelper
    {
        private readonly Control _axControl;
        private bool _webIsVisible = false;
        private string _parentWindow = string.Empty;
        private RECT _rect;

        private delegate bool WindowEnumDelegate(IntPtr hwnd,
                                     int lParam);

        [DllImport("user32.dll")]
        private static extern int EnumChildWindows(IntPtr hwnd,
                                                   WindowEnumDelegate del,
                                                   int lParam);
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hwnd,
                                               StringBuilder bld, int size);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);


        [Serializable, StructLayout(LayoutKind.Sequential)]
        [DebuggerDisplay("Left = {Left},Top = {Top}, Width = {Width},Height = {Height}")]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            public int Height { get { return Bottom - Top; } }
            public int Width { get { return Right - Left; } }
            public Size Size { get { return new Size(Width, Height); } }

            public Point Location { get { return new Point(Left, Top); } }

            // Handy method for converting to a System.Drawing.Rectangle
            public Rectangle ToRectangle()
            { return Rectangle.FromLTRB(Left, Top, Right, Bottom); }

            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }

            public override int GetHashCode()
            {
                return Left ^ ((Top << 13) | (Top >> 0x13))
                  ^ ((Width << 0x1a) | (Width >> 6))
                  ^ ((Height << 7) | (Height >> 0x19));
            }

            #region Operator overloads

            public static implicit operator Rectangle(RECT rect)
            {
                return rect.ToRectangle();
            }

            public static implicit operator RECT(Rectangle rect)
            {
                return FromRectangle(rect);
            }

            #endregion
        }

        public DsoFramerHelper(Control axControl)
        {
            _axControl = axControl;
        }

        private bool WindowEnumProc(IntPtr hwnd, int lParam)
        {
            // get the text from the window
            StringBuilder bld = new StringBuilder(256);
            GetWindowText(hwnd, bld, 256);
            string text = bld.ToString();

            if (text.Length > 0)
            {
                if (text.ToLower().Equals("Web".ToLower()))
                {
                    uint style = (uint)PowerPointUtils.GetWindowLong(hwnd, PowerPointUtils.GWL_STYLE);
                    if ((style & (uint)PowerPointUtils.WindowStyles.WS_VISIBLE) != 0)
                    {
                        _webIsVisible = true;
                        IntPtr par = GetParent(hwnd);
                        if (par != IntPtr.Zero)
                        {
                            GetWindowText(par, bld, 256);
                            _parentWindow = bld.ToString();
                        }
                        _rect = new RECT();
                        GetClientRect(hwnd, out _rect);
                        ChangeSize();
                        ResizeAxControl();
                    }
                }
            }
            return true;
        }

        private void ChangeSize()
        {
                if (_parentWindow.ToLower().Equals("MsoDockTop".ToLower()) ||
                    _parentWindow.ToLower().Equals("MsoDockBottom".ToLower()))
                {
                    _axControl.Height += _rect.Height;
                }
                else if (_parentWindow.ToLower().Equals("MsoDockLeft".ToLower()) ||
                         _parentWindow.ToLower().Equals("MsoDockRight".ToLower()))
                {
                    _axControl.Width += _rect.Width;
                }
        }

        public void Init()
        {
            WindowEnumDelegate del = new WindowEnumDelegate(WindowEnumProc);

            // call the win32 function
            EnumChildWindows(_axControl.Handle, del, 0);
        }

        public void ResizeAxControl()
        {
            if (_webIsVisible)
            {

                int x = 0;
                int y = 0;
                if (_parentWindow.ToLower().Equals("MsoDockTop".ToLower()))
                {
                    y -= _rect.Height;
                }
                else if (_parentWindow.ToLower().Equals("MsoDockBottom".ToLower()))
                {
                    //y += _rect.Height;
                }
                else if (_parentWindow.ToLower().Equals("MsoDockRight".ToLower()))
                {
                    //x -= _rect.Width;
                }
                else if (_parentWindow.ToLower().Equals("MsoDockLeft".ToLower()))
                {
                    x -= _rect.Width;
                }
                PowerPointUtils.MoveWindow(_axControl.Handle, x, y, _axControl.Width, _axControl.Height, true);
            }
        }
    }
}
