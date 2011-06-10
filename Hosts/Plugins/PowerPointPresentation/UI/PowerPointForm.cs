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
using Hosts.Plugins.PowerPointPresentation.Common;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Win32;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Label=System.Windows.Forms.Label;
using Presentation=Microsoft.Office.Interop.PowerPoint.Presentation;

namespace Hosts.Plugins.PowerPointPresentation.UI
{
    public partial class PowerPointForm : Form, IDoCommand
    {
        private Presentation _presentation;
        private string _fileName;
        //private AxDSOFramer.AxFramerControl axFramerControl1 = null;
        private AxHost _axHost = null;
        //private PowerPointShowControl _axHost = null;

        public PowerPointForm()
        {
            InitializeComponent();
            Guid dsoGuid = new Guid("00460182-9E5E-11d5-B7C8-B8269041DD57");
            string keyName = @"CLSID\{" + dsoGuid + "}";
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(keyName))
                if (key == null)
                {
                    Label label = new Label();
                    label.Text = "В системе не установлен DSO компонент необходимый для просмотра";
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Dock = DockStyle.Fill;
                    this.panel1.Controls.Add(label);
                    return;
                }
            //_axHost = new PowerPointShowControl();
            _axHost = new AxHost();
            _axHost.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(_axHost);
        }

        public bool Init(DisplayType display, Window window)
        {
            this.InitBorderTitle(window, panel1);

            return true;
        }

        public void LoadPresentation(string fileName)
        {
            if (_axHost != null)
            {
                //    _axHost.CreateApplication();
                _axHost.LoadPresentation(fileName);
            }
            //_fileName = fileName;
        }

        public string DoCommand(string command)
        {
            string[] commands = command.Split(Constants.Delimeter);
            if (commands.Length == 0) return null;
            PowerPointShowCommand com;
            try
            {
                com = (PowerPointShowCommand)Enum.Parse(typeof(PowerPointShowCommand), commands[0]);
            }
            catch(ArgumentException)
            {
                return null;
            }
            switch(com)
            {
                case PowerPointShowCommand.NextSlide:
                    Next();
                    break;
                case PowerPointShowCommand.PrevSlide:
                    Prev();
                    break;
                case PowerPointShowCommand.GoToSlide:
                    if (commands.Length >= 2)
                        GoToSlide(commands[1]);
                    break;
                case PowerPointShowCommand.Status:
                    break;
            }
            return GetCurrentSlide();
        }

        private string GetCurrentSlide()
        {
            try
            {
                if (_axHost != null)
                {
                    return _axHost.CurrentPosition.ToString();
                }
                //if (_presentation != null && _presentation.SlideShowWindow != null)
                //{
                //    return _presentation.SlideShowWindow.View.CurrentShowPosition.ToString();
                //}
            }
            catch
            { }
            return "0";
        }

        private void GoToSlide(string command)
        {
            try
            {
                if (_axHost != null)
                {
                        int index;
                        if (Int32.TryParse(command, out index))
                        {
                            _axHost.GotoSlide(index);
                        }
                }

                //if (_presentation != null && _presentation.SlideShowWindow != null)
                //{
                //    int index;
                //    if (Int32.TryParse(command, out index))
                //    {
                //        if (index <= _presentation.Slides.Count)
                //            _presentation.SlideShowWindow.View.GotoSlide(index, MsoTriState.msoFalse);
                //    }
                //}
            }
            catch
            {}
        }

        private void Prev()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.Prev();
                }
                //if (_presentation != null && _presentation.SlideShowWindow != null)
                //{
                //    _presentation.SlideShowWindow.View.Previous();
                //}
            }
            catch
            {}
        }

        private void Next()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.Next();
                }

                //if (_presentation != null && _presentation.SlideShowWindow != null)
                //{
                //    _presentation.SlideShowWindow.View.Next();
                //}
            }
            catch
            {}
        }

        private void PowerPointForm_Shown(object sender, EventArgs e)
        {
            if (_axHost != null)
            {
                _axHost.StartSlideShow();
            }
        }

        private void PowerPointForm_Load(object sender, EventArgs e)
        {
        }

        private void PowerPointForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
