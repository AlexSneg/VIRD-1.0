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
using Hosts.Plugins.WordDocument.Common;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Label=System.Windows.Forms.Label;
//using Presentation=Microsoft.Office.Interop.Word.Presentation;

namespace Hosts.Plugins.WordDocument.UI
{
    public partial class WordForm : Form, IDoCommand
    {
        private Presentation _presentation;
        private string _fileName;
        //private AxDSOFramer.AxFramerControl axFramerControl1 = null;
        private AxHost _axHost = null;
        //private WordShowControl _axHost = null;

        public WordForm(int startPage, int startLine, int startZoom)
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
            //_axHost = new WordShowControl();
            _axHost = new AxHost();
            _axHost.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(_axHost);
            _axHost.StartPage = startPage;
            _axHost.StartLine = startLine;
            _axHost.StartZoom = startZoom;
        }

        public bool Init(DisplayType display, TechnicalServices.Persistence.SystemPersistence.Presentation.Window window)
        {
            this.InitBorderTitle(window, panel1);

            return true;
        }

        public void LoadPresentation(string fileName)
        {
            if (_axHost != null)
            {
                _axHost.LoadPresentation(fileName);
            }
        }

        public string DoCommand(string command)
        {
            string[] commands = command.Split(Constants.Delimeter);
            if (commands.Length == 0) return null;

            //return null;
            WordShowCommand com;
            try
            {
                com = (WordShowCommand)Enum.Parse(typeof(WordShowCommand), commands[0]);
            }
            catch (ArgumentException)
            {
                return null;
            }
            switch(com)
            {
                case WordShowCommand.Next:
                    Next();
                    break;
                case WordShowCommand.Prev:
                    Prev();
                    break;
                case WordShowCommand.Left:
                    LeftScroll();
                    break;
                case WordShowCommand.Right:
                    RightScroll();
                    break;
                case WordShowCommand.NextPage:
                    NextPage();
                    break;
                case WordShowCommand.PrevPage:
                    PrevPage();
                    break;
                case WordShowCommand.LastPage:
                    LastPage();
                    break;
                case WordShowCommand.FirstPage:
                    FirstPage();
                    break;
                case WordShowCommand.GoToPage:
                    if (commands.Length >= 2)
                        GoToPage(commands[1]);
                    break;
                case WordShowCommand.Zoom:
                    if (commands.Length >= 2)
                        Zoom(commands[1]);
                    break;
                case WordShowCommand.Status:
                    break;
            }
        //    return GetCurrentSlide();
            return GetCurrentPage(com);
        }

        private string GetCurrentPage(WordShowCommand com)
        {
            try
            {
                if (_axHost != null)
                {
                    if (com == WordShowCommand.Zoom)
                    {
                        return _axHost.CurrentZoom.ToString();
                    }
                    else
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

        private void GoToPage(string command)
        {
            try
            {
                if (_axHost != null)
                {
                    int index;
                    if (Int32.TryParse(command, out index))
                    {
                        _axHost.GoToPage(index);
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
            { }
        }

        private void Zoom(string command)
        {
            try
            {
                if (_axHost != null)
                {
                    int index;
                    if (Int32.TryParse(command, out index))
                    {
                        _axHost.Zoom(index);
                    }
                }
            }
            catch
            { }
        }

        private void Prev()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.Prev();
                }
            }
            catch
            { }
        }

        private void Next()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.Next();
                }
            }
            catch
            { }
        }

        private void LeftScroll()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.LeftScroll();
                }
            }
            catch
            { }
        }

        private void RightScroll()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.RightScroll();
                }
            }
            catch
            { }
        }
        
        private void PrevPage()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.PrevPage();
                }
                //if (_presentation != null && _presentation.SlideShowWindow != null)
                //{
                //    _presentation.SlideShowWindow.View.Previous();
                //}
            }
            catch
            { }
        }

        private void NextPage()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.NextPage();
                }

                //if (_presentation != null && _presentation.SlideShowWindow != null)
                //{
                //    _presentation.SlideShowWindow.View.Next();
                //}
            }
            catch
            { }
        }

        private void LastPage()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.LastPage();
                }
            }
            catch
            { }
        }

        private void FirstPage()
        {
            try
            {
                if (_axHost != null)
                {
                    _axHost.FirstPage();
                }
            }
            catch
            { }
        }

        private void WordForm_Shown(object sender, EventArgs e)
        {
            if (_axHost != null)
            {
                _axHost.StartDocumentShow();
            }
        }

        private void WordForm_Load(object sender, EventArgs e)
        {
        }

        private void WordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
