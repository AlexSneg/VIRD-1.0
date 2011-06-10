using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Hosts.Plugins.IEDocument.Common;

namespace Hosts.Plugins.IEDocument.UI
{
    public partial class WebBrowserControl : UserControl
    {
        private AdvBrowser advBrowser;
        private string _currentUrl;

        //TO DO: Удалить
        //private int scrollPosition = 0;
        //private int prevScrollPosition = 0;


        private int scrollDelta = 100;
        private int scrollTop = 0;
        private int scrollLeft = 0;
        private int scrollTopAlt = 0;
        private int scrollLeftAlt = 0;
        private bool scrollNext = false;

        public WebBrowserControl()
        {
            InitializeComponent();
        }

        public void LoadPresentation(string currentUrl)
        {
            _currentUrl = currentUrl;
        }

        public void StartDocumentShow()
        {
            advBrowser = new AdvBrowser();
            advBrowser.Dock = DockStyle.Fill;
            this.Controls.Add(this.advBrowser);
            advBrowser.ScrollBarsEnabled = false;

            // Subscribe to Event(s) with the WindowsInterop Class
            WindowsInterop.SecurityAlertDialogWillBeShown +=
                new GenericDelegate<Boolean, Boolean>(this.WindowsInterop_SecurityAlertDialogWillBeShown);
            WindowsInterop.ConnectToDialogWillBeShown +=
                new GenericDelegate<String, String, Boolean>(this.WindowsInterop_ConnectToDialogWillBeShown);
            // Subscribe to the WebBrowser Control's DocumentCompleted event
            this.advBrowser.DocumentCompleted +=
                new WebBrowserDocumentCompletedEventHandler(this.advBrowser_DocumentCompleted);

            advBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(FirstZoom);
            if ((PostParams == "") || (PostParams == null))
            {
                advBrowser.Navigate(_currentUrl);
            }
            else
            {
                string postParams = "";
                ParsePOSTParams parsePostParams = new ParsePOSTParams();
                string incorrectParams = parsePostParams.Verification(PostParams);
                if (incorrectParams != "")
                {
                    advBrowser.Navigate(_currentUrl);
                }
                else
                {
                    foreach (Hosts.Plugins.IEDocument.Common.Param param in parsePostParams)
                    {
                        postParams = postParams + param.PostKey + "=" + param.PostValue + @"&";
                    }
                    postParams = postParams.Substring(0, postParams.Length - 1);

                    string vHeaders = "Content-Type: application/x-www-form-urlencoded\n";
                    byte[] postParamsByte = null;
                    //TO DO: Алгоритм подстановки кодировки в упрощённом виде. Переработать алгоритм подстановки кодировки.
                    if (PostParamsEncoding.Trim().ToUpper() != "UTF-8")
                    {
                        postParamsByte = Encoding.GetEncoding(1251).GetBytes(postParams);
                    }
                    else
                    {
                        postParamsByte = Encoding.GetEncoding(65001).GetBytes(postParams);
                    }
                    advBrowser.Navigate(_currentUrl, "", postParamsByte, vHeaders);
                }
            }
        }

        private void FirstZoom(object sender, WebBrowserNavigatedEventArgs e)
        {
            advBrowser.Zoom(ZoomProperty);
            advBrowser.Navigated -= new System.Windows.Forms.WebBrowserNavigatedEventHandler(FirstZoom);
        }

        private Boolean WindowsInterop_SecurityAlertDialogWillBeShown(Boolean blnIsSSLDialog)
        {
            // Return true to ignore and not show the 
            // "Security Alert" dialog to the user
            return true;
        }

        private Boolean WindowsInterop_ConnectToDialogWillBeShown(ref String sUsername, ref String sPassword)
        {
            // (Fill in the blanks in order to be able 
            // to return the appropriate Username and Password)
            sUsername = Login;
            sPassword = Password;
            //sUsername = "omsk\\iubaleht";
            //sPassword = "coder_0901";

            // Return true to auto populate credentials and not 
            // show the "Connect To ..." dialog to the user
            return true;
        }

        private void advBrowser_DocumentCompleted(Object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Enable the 2 buttons on the form
            //this.btnTestNoCredentialsDialog.Enabled = true;
            //this.btnTestNoSecurityAlertDialog.Enabled = true;

            this.Cursor = Cursors.Default;

            this.advBrowser.Document.Window.Scroll += new HtmlElementEventHandler(this.advBrowser_Scroll);
        }

        private void advBrowser_Scroll(object sender, HtmlElementEventArgs e)
        {
            scrollNext = true;
            scrollLeftAlt = scrollLeft + scrollDelta;
            scrollTopAlt = scrollTop + scrollDelta;
        }

        public void UpScroll()
        {
            //TO DO: Удалить, старый метод
            //if (scrollPosition > 0)
            //{
            //    scrollPosition = scrollPosition - scrollDelta;
            //}
            //HtmlDocument doc = this.advBrowser.Document;
            //doc.Body.ScrollTop = scrollPosition;

            if (scrollTop >= scrollDelta)
            {
                scrollTop = scrollTop - scrollDelta;
                this.advBrowser.Document.Window.ScrollTo(scrollLeft, scrollTop);
            }
        }

        public void DownScroll()
        {

            //TO DO: Удалить, старый метод
            //HtmlDocument doc = this.advBrowser.Document;
            //if ((prevScrollPosition != doc.Body.ScrollRectangle.Y) || (prevScrollPosition == 0))
            //{
            //    scrollPosition = scrollPosition + scrollDelta;
            //    prevScrollPosition = doc.Body.ScrollRectangle.Y;
            //}
            //doc.Body.ScrollTop = scrollPosition;

            if ((scrollNext == false) && (scrollTopAlt == 0))
            {
                scrollTop = scrollTop + scrollDelta;
                this.advBrowser.Document.Window.ScrollTo(scrollLeft, scrollTop);
            }
            else if (scrollNext == true)
            {
                scrollTop = scrollTop + scrollDelta;
                this.advBrowser.Document.Window.ScrollTo(scrollLeft, scrollTop);
            }
            scrollNext = false;
        }

        public void LeftScroll()
        {
            //TO DO: Удалить, старый метод
            //HtmlDocument doc = this.advBrowser.Document;
            //doc.Body.ScrollLeft = doc.Body.ScrollRectangle.X - scrollDelta;

            if (scrollLeft >= scrollDelta)
            {
                scrollLeft = scrollLeft - scrollDelta;
                this.advBrowser.Document.Window.ScrollTo(scrollLeft, scrollTop);
            }
        }

        public void RightScroll()
        {
            //TO DO: Удалить, старый метод
            //HtmlDocument doc = this.advBrowser.Document;
            //doc.Body.ScrollLeft = doc.Body.ScrollRectangle.X + scrollDelta;

            if ((scrollNext == false) && (scrollLeftAlt == 0))
            {
                scrollLeft = scrollLeft + scrollDelta;
                this.advBrowser.Document.Window.ScrollTo(scrollLeft, scrollTop);
            }
            else if (scrollNext == true)
            {
                scrollLeft = scrollLeft + scrollDelta;
                this.advBrowser.Document.Window.ScrollTo(scrollLeft, scrollTop);
            }
            scrollNext = false;

            //TO DO: Удалить
            //var varH = this.advBrowser.Document.Window.Size.Height;
            //var varW = this.advBrowser.Document.Window.Size.Width;
            //var varX = this.advBrowser.Document.Window.Position.X;
            //var varY = this.advBrowser.Document.Window.Position.Y;
            //var varL = this.advBrowser.Document.Body.ScrollLeft;
            //var varLL = this.advBrowser.Document.Body.ScrollRectangle.Left;
            //var varXX = this.advBrowser.Document.Body.ScrollRectangle.X;
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(new System.IO.FileStream(@"C:\temp\log\log.txt", System.IO.FileMode.Append), Encoding.GetEncoding(1251)))
            //{
            //    file.WriteLine("{0}:\t{1}", DateTime.Now, varL.ToString() + " " + varLL.ToString() + " " + varXX.ToString());
            //}
            
        }

        public void Zoom(int scale)
        {
            advBrowser.Zoom(scale);
            advBrowser.Document.Window.ScrollTo(scrollLeft, scrollTop);
        }

        public string Login
        { get; set; }

        public string Password
        { get; set; }

        public int ZoomProperty
        { get; set; }

        public string PostParams
        { get; set; }

        public string PostParamsEncoding
        { get; set; }
    }
}
