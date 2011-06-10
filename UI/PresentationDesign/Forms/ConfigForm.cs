using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Specialized;
using System.IO;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using TechnicalServices.Interfaces;
using UI.PresentationDesign.DesignUI.Controls.Config;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class ConfigForm : PropertyDialog
    {
        private bool _changed = false;
        private Type settingsType;
        private List<string> exclusionConfigFile = new List<string>() { "LabelStorage.xml" };
        private IConfiguration _configs;

        public ConfigForm()
        {
            InitializeComponent();
        }
        public ConfigForm(IConfiguration configs) : this()
        {
            _configs = configs;
            settingsType = _configs.GetType();
            settingsGrid[1, 2].Control = CreateFilePathTextBoxControl("GlobalSourceFolder");
            settingsGrid[2, 2].Control = CreateFilePathTextBoxControl("ConfigurationFolder");
            settingsGrid[3, 2].ChoiceList = getConfigurations();
            settingsGrid[3, 2].Text = _configs.ConfigurationFile;
            settingsGrid[3, 2].ExclusiveChoiceList = true;
            settingsGrid[4, 2].Control = CreateFilePathTextBoxControl("ScenarioFolder");
            settingsGrid[5, 2].Control = CreateFilePathTextBoxControl("LocalSourceFolder");
        }
        protected override string QuestionOnExit
        {
            get{ return "Изменения вступят в силу после перезапуска приложения. Сохранить?"; }
        }
        public override bool Changed()
        {
            settingsGrid.ConfirmChanges();
            if (!_configs.ConfigurationFile.Equals(settingsGrid[3, 2].Text))
            {
                _configs.ConfigurationFile = settingsGrid[3, 2].Text;
                _changed = true;
            }
            return _changed;
        }

        private StringCollection getConfigurations()
        {
            StringCollection result = new StringCollection();
            DirectoryInfo dInfo = new DirectoryInfo(_configs.ConfigurationFolder);
            foreach (FileInfo item in dInfo.GetFiles("*.xml").Where(file => !exclusionConfigFile.Exists(exclFile => exclFile.Equals(file.Name))))
            {
                result.Add(item.Name);
            }
            return result;
        }

        private FilePathTextBox CreateFilePathTextBoxControl(string propertyName)
        {
            FilePathTextBox result = new FilePathTextBox();
            result.SelectedPath = settingsType.GetProperty(propertyName).GetValue(_configs, null).ToString();
            result.SelectedPath = Path.GetFullPath(result.SelectedPath);
            result.OnPathChanged += result_OnPathChanged;
            result.Tag = propertyName;
            return result;
        }

        private void result_OnPathChanged(FilePathTextBox sender)
        {
            string fullPath = sender.SelectedPath;
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            if (fullPath.StartsWith(appPath))
                fullPath = fullPath.Replace(appPath + "\\", "");
            settingsType.GetProperty((string)sender.Tag).SetValue(_configs, fullPath, null);
            if (sender.Tag.Equals("ConfigurationFolder"))
            {
                settingsGrid[3, 2].ChoiceList = getConfigurations();
                settingsGrid[3, 2].Text = settingsGrid[3, 2].ChoiceList.Contains(_configs.ConfigurationFile)
                    ? _configs.ConfigurationFile : string.Empty;
            }
            _changed = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            settingsGrid.CancelUpdate();
            _configs.ReloadUserSettings();
            settingsGrid[3, 2].Text = _configs.ConfigurationFile;
            _changed = false;
        }

        public override bool AcceptChanges()
        {
            _configs.SaveUserSettings();
            CloseMe();
            return true;
        }

        public override void CancelChanges()
        {
            _configs.ReloadUserSettings();
            DialogResult = DialogResult.Cancel;
            CloseMe();
        }

        private void settingsGrid_CellClick(object sender, GridCellClickEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColIndex;
            Control control = settingsGrid[rowIndex, colIndex].Control;
            if (control == null) return;
            //вычислим координаты внутри контрола
            Point point = new Point(e.MouseEventArgs.X - control.Location.X, e.MouseEventArgs.Y - control.Location.Y);
            Button button = FindButton(control.Controls, point);
            if (button != null)
            {
                button.PerformClick();
            }
        }

        private Button FindButton(Control.ControlCollection collection, Point point)
        {
            foreach (Control control in collection)
            {
                if (!control.Bounds.Contains(point)) continue;
                if (control is Button) return (Button)control;
            }
            return null;
        }
    }
}
