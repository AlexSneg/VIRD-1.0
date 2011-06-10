using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxDSOFramer;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using Microsoft.Win32;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Label=System.Windows.Forms.Label;
using Hosts.Plugins.IEDocument.Common;

namespace Hosts.Plugins.IEDocument.UI
{
    public partial class IEForm : Form, IDoCommand
    {
        private Presentation _presentation;
        private string _fileName;
        private WebBrowserControl _webBrowserControl = null;        
        //private IEShowControl _axHost = null;

        protected int _croppingLeft = 0;
        protected int _croppingRight = 0;
        protected int _croppingTop = 0;
        protected int _croppingBottom = 0;
        private Label _messageLabel;

        public IEForm(string login, string password, int zoom, string postParams, string postParamsEncoding)
        {
            InitializeComponent();
            //_axHost = new IEShowControl();

            _webBrowserControl = new WebBrowserControl();
            _webBrowserControl.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(_webBrowserControl);
            _webBrowserControl.Login = login;
            _webBrowserControl.Password = password;
            _webBrowserControl.ZoomProperty = zoom;
            _webBrowserControl.PostParams = postParams;
            _webBrowserControl.PostParamsEncoding = postParamsEncoding;
        }

        public bool Init(DisplayType display, TechnicalServices.Persistence.SystemPersistence.Presentation.Window window)
        {
            this.InitBorderTitle(window, panel1);
            ActiveWindow activeWindow = window as ActiveWindow;
            if (activeWindow != null)
            {
                _croppingBottom = activeWindow.CroppingBottom;
                _croppingLeft = activeWindow.CroppingLeft;
                _croppingRight = activeWindow.CroppingRight;
                _croppingTop = activeWindow.CroppingTop;
            }

            return true;
        }

        public void LoadPresentation(string fileName)
        {
            if (_webBrowserControl != null)
            {
                _webBrowserControl.LoadPresentation(fileName);
            }
        }

        public string DoCommand(string command)
        {
            string[] commands = command.Split(Constants.Delimeter);
            if (commands.Length == 0) return null;

            //return null;
            IEShowCommand com;
            try
            {
                com = (IEShowCommand)Enum.Parse(typeof(IEShowCommand), commands[0]);
            }
            catch (ArgumentException)
            {
                return null;
            }
            switch (com)
            {
                case IEShowCommand.Up:
                    UpScroll();
                    break;
                case IEShowCommand.Down:
                    DownScroll();
                    break;
                case IEShowCommand.Left:
                    LeftScroll();
                    break;
                case IEShowCommand.Right:
                    RightScroll();
                    break;
                case IEShowCommand.Status:
                    break;
                case IEShowCommand.Zoom:
                    if (commands.Length >= 2)
                        Zoom(commands[1]);
                    break;
            }
            return GetCurrentValue(com);
        }

        //TO DO: Переработать передавать текущую страницу в плагине IE не нужно
        private string GetCurrentValue(IEShowCommand com)
        {
            try
            {
                if (_webBrowserControl != null)
                {
                    if (com == IEShowCommand.Zoom)
                    {
                        return _webBrowserControl.ZoomProperty.ToString();
                    }
                    else
                        return "0";
                }
            }
            catch
            { }
            return "0";
        }

        private void UpScroll()
        {
            try
            {
                if (_webBrowserControl != null)
                {
                    _webBrowserControl.UpScroll();
                }
            }
            catch
            { }
        }

        private void DownScroll()
        {
            try
            {
                if (_webBrowserControl != null)
                {
                    _webBrowserControl.DownScroll();
                }
            }
            catch
            { }
        }

        private void LeftScroll()
        {
            try
            {
                if (_webBrowserControl != null)
                {
                    _webBrowserControl.LeftScroll();
                }
            }
            catch
            { }
        }

        private void RightScroll()
        {
            try
            {
                if (_webBrowserControl != null)
                {
                    _webBrowserControl.RightScroll();
                }
            }
            catch
            { }
        }

        private void Zoom(string command)
        {
            try
            {
                if (_webBrowserControl != null)
                {
                    int scale;
                    if (Int32.TryParse(command, out scale))
                    {
                        _webBrowserControl.Zoom(scale);
                        _webBrowserControl.ZoomProperty = scale;
                    }
                }
            }
            catch
            { }
        }

        private void IEForm_Shown(object sender, EventArgs e)
        {
            if (_webBrowserControl != null)
            {
                _webBrowserControl.StartDocumentShow();
            }
        }

        private void IEForm_Load(object sender, EventArgs e)
        {
        }

        private void IEForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
