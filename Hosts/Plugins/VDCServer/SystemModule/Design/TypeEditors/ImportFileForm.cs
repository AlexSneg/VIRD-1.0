using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Syncfusion.Windows.Forms;

namespace Hosts.Plugins.VDCServer.SystemModule.Design.TypeEditors
{
    public partial class ImportFileForm : Form
    {
        List<VDCServerAbonentInfo> abonents;
        public ImportFileForm(List<VDCServerAbonentInfo> list)
        {
            abonents = list;
            InitializeComponent();
        }

        private void ImportFileForm_Load(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
            }
        }

        string fileName;

        private void ImportFileForm_Shown(object sender, EventArgs e)
        {
            bool wasErrors=false;
            int importCount = 0;
            if (fileName != null)
            {
                TextReader reader = new StreamReader(fileName);
                string line;
                abonents.Clear();
                while ((line = reader.ReadLine())!=null)
                {
                    VDCServerAbonentInfo info = new VDCServerAbonentInfo();
                    Regex reg = new Regex("'(?<name>.*)';'(?<number1>.*)';'(?<number2>.*)';'(?<connection_type>.*)';'(?<connection_quality>.*)';");
                    Match match = reg.Match(line);
                    if (match != null && match.Length > 0)
                    {
                        if (match.Groups["name"].Value.Trim().Length == 0) continue; // обязательные поля
                        if (match.Groups["number1"].Value.Trim().Length == 0) continue;
                        info.Name = match.Groups["name"].Value;
                        info.Number1 = match.Groups["number1"].Value;
                        info.Number2 = match.Groups["number2"].Value;
                        info.ConnectionType = (ConnectionTypeVDCServerEnum)Enum.Parse(typeof(ConnectionTypeVDCServerEnum), match.Groups["connection_type"].Value, true);
                        info.ConnectionQuality = (ConnectionQualityVDCServerEnum)Enum.Parse(typeof(ConnectionQualityVDCServerEnum), match.Groups["connection_quality"].Value, true);
                        abonents.Add(info);
                        importCount ++;
                    }
                    else
                    {
                        wasErrors = true;
                    }
                    
                }
                reader.Close();
                if (importCount == 0)
                {
                    MessageBoxAdv.Show("Файл не содержит ни одной корректной строки для импорта.", "Ошибка");
                }
                else
                {
                    if (wasErrors)
                    {
                        MessageBoxAdv.Show("При импорте часть строк было пропущено, так как они имели неверный формат.", "Ошибка");
                    }
                    if (importCount > 100)
                    {
                        abonents.RemoveRange(100, abonents.Count - 100);
                        MessageBoxAdv.Show("При импорте обнаружено более 100 записей, оставлены первые 100 записей", "Ошибка");
                    }
                }
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }
    }
}
