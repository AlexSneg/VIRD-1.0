using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using System.Runtime.InteropServices;
using System.Media;

namespace UI.Common.CommonUI.Forms
{
    public partial class MessageBoxForm : Office2007Form
    {
        private enum SystemLocStrings
        {
            OK,
            Cancel,
            Abort,
            Retry,
            Ignore,
            Yes,
            No,
            Close,
            Help,
            TryAgain,
            Continue
        }

        static SystemLocStrings[] s_buttonLocIDs = null;

        static DialogResult[][] s_buttons = null;

        DialogResult[] m_buttons = null;
        MessageBoxButtons m_buttonsInt;

        static MessageBoxForm()
        {
            if (s_buttons == null)
                s_buttons = new DialogResult[][] { new DialogResult[] { DialogResult.OK }, new DialogResult[] { DialogResult.OK, DialogResult.Cancel }, new DialogResult[] { DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore }, new DialogResult[] { DialogResult.Yes, DialogResult.No, DialogResult.Cancel }, new DialogResult[] { DialogResult.Yes, DialogResult.No }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel } };

            if (s_buttonLocIDs == null)
            {
                SystemLocStrings[] stringsArray = new SystemLocStrings[8];
                stringsArray[0] = SystemLocStrings.Close;
                stringsArray[2] = SystemLocStrings.Cancel;
                stringsArray[3] = SystemLocStrings.Abort;
                stringsArray[4] = SystemLocStrings.Retry;
                stringsArray[5] = SystemLocStrings.Ignore;
                stringsArray[6] = SystemLocStrings.Yes;
                stringsArray[7] = SystemLocStrings.No;
                s_buttonLocIDs = stringsArray;
            }
        }

        public MessageBoxForm()
        {
            InitializeComponent();
            buttonPanel.Padding = new Padding(5, 0, 5, 0);
            buttonPanel.Margin = buttonPanel.Padding;
        }

        public DialogResult ShowForm(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, string[] captions)
        {
            return ShowForm(owner, text, caption, buttons, icon, captions, true);
        }

        public DialogResult ShowForm(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, string[] captions, bool useCancelButton)
        {
            this.Text = caption;
            label1.Text = text;
            int height = label1.Height;
            int width = label1.Width;

            height += 25;

            label1.AutoSize = false;
            label1.Width = width;
            label1.Height = height;

            m_buttonsInt = buttons;
            InitializeButtons(buttons, captions, useCancelButton);
            InitializeImage(icon);

            //this.TopMost = true;

            if (owner != null)
                return this.ShowDialog(owner);
            else
                return this.ShowDialog();
        }

        private void InitializeImage(MessageBoxIcon icon)
        {
            SystemSound question = null;
            Icon hand = null;
            switch (icon)
            {
                case MessageBoxIcon.Hand:
                    hand = SystemIcons.Hand;
                    question = SystemSounds.Hand;
                    break;

                case MessageBoxIcon.Question:
                    hand = SystemIcons.Question;
                    question = SystemSounds.Question;
                    break;

                case MessageBoxIcon.Exclamation:
                    hand = SystemIcons.Exclamation;
                    question = SystemSounds.Exclamation;
                    break;

                case MessageBoxIcon.Asterisk:
                    hand = SystemIcons.Asterisk;
                    question = SystemSounds.Asterisk;
                    break;

                default:
                    question = SystemSounds.Beep;
                    break;
            }
            if (hand != null)
            {
                this.pictureBox1.Image = hand.ToBitmap();
            }

            if (question != null)
                question.Play();
        }

        private void InitializeButtons(MessageBoxButtons AButtons, string[] captions, bool useCancelButton)
        {
            DialogResult[] buttons = s_buttons[(int)AButtons];
            m_buttons = buttons;

            int i = 0;
            ButtonAdv noButton = null;
            List<Control> controls = new List<Control>();
            foreach (DialogResult button in buttons)
            {
                ButtonAdv btnControl = new ButtonAdv();
                btnControl.Appearance = ButtonAppearance.Office2007;
                btnControl.DialogResult = button;
                btnControl.Margin = new Padding(5, 0, 5, 0);
                btnControl.Name = "btn" + button.ToString();
                btnControl.AutoSize = true; //Size = new Size(0x4b, 0x17);
                if (captions.Length - 1 >= i)
                    btnControl.Text = captions[i];
                else
                    btnControl.Text = GetLocString(button);

                btnControl.TabIndex = i++;
                btnControl.UseVisualStyle = true;
                if (button == DialogResult.Cancel && useCancelButton)
                {
                    this.CancelButton = btnControl;
                }

                if (button == DialogResult.No)
                {
                    noButton = btnControl;
                }

                controls.Add(btnControl);
            }

            if (buttons.Contains(DialogResult.No) && CancelButton == null && useCancelButton)
                CancelButton = noButton;

            buttonPanel.Controls.AddRange(controls.ToArray());
            buttonPanel.Left = (base.ClientSize.Width - buttonPanel.Width) / 2 - buttonPanel.Margin.Left;

            controls.First().Focus();
            //this.ControlBox = buttons.Contains(DialogResult.Cancel);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            bool flag = m_buttonsInt == MessageBoxButtons.OK ? false : true;

            foreach (DialogResult result in this.m_buttons)
            {
                if (result == base.DialogResult)
                {
                    flag = false;
                    break;
                }
            }
            e.Cancel = flag && (!IsCancellable() || base.DialogResult != DialogResult.Cancel);
        }

        bool IsCancellable()
        {
            switch (m_buttonsInt)
            {
                case MessageBoxButtons.YesNo:
                    return true;

                case MessageBoxButtons.YesNoCancel:
                    return true;

                case MessageBoxButtons.OKCancel:
                    return true;
            }
            return false;

        }

        private string GetLocString(DialogResult btn)
        {
            string str = null;
            SystemLocStrings strings = s_buttonLocIDs[(int)btn];
            try
            {
                str = NativeMethod.MB_GetString((int)strings);
            }
            catch
            {
            }
            return str;
        }

        private void label1_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void MessageBoxForm_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void MessageBoxForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }

    public static class NativeMethod
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetActiveWindow();
 
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern string MB_GetString(int i);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }

}
