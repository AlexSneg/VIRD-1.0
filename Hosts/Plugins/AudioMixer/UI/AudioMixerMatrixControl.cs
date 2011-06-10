using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hosts.Plugins.AudioMixer.SystemModule.Config;
using Syncfusion.Windows.Forms.Tools;
using System.Threading;
using TechnicalServices.Entity;
using Action=Syncfusion.Windows.Forms.Tools.Action;

namespace Hosts.Plugins.AudioMixer.UI
{
    public partial class AudioMixerMatrixControl : UserControl
    {
        //private readonly int _firstWidthCol = 50;
        private readonly int _heightRow = 22;
        private readonly int _minColWidth = 31;
        private int _colNumber;
        private int _rowNumber;
        private int _widthCol { get { return Math.Max(((this.Width - gpOutput.Width - (_rowNumber > 11 ? _scrollBarWidth : 0)) / (_colNumber)) - 2, _minColWidth); } }
        private int _matrixId;
        private int _scrollBarWidth = 20;
        private Dictionary<ValuePair<int, int>, ValuePair<AudioMixerInput, AudioMixerOutput>> _matrix =
            new Dictionary<ValuePair<int, int>, ValuePair<AudioMixerInput, AudioMixerOutput>>();

        public AudioMixerMatrixControl()
        {
            InitializeComponent();
        }

        public void InitializeMatrix(int matrixId,
            List<AudioMixerInput> inputs, List<AudioMixerOutput> outputs, 
            Func<AudioMixerInput,AudioMixerOutput, bool> getMatrixUnit )
        {
            _matrixId = matrixId;
            _colNumber = inputs.Count;
            _rowNumber = outputs.Count;
            this.Height = (_rowNumber + 2) * _heightRow + (_colNumber > 12 ? _scrollBarWidth : 0);
            this.Width = GetMinWidthControl(_colNumber) - 20;

            for (int i = 0; i < outputs.Count; i++ )
            {
                AudioMixerOutput row = outputs[i];
                createLabel(row, i);
                for (int j = 0; j < inputs.Count; j++)
                {
                    AudioMixerInput col = inputs[j];
                    if (i == 0) createLabel(col, j);
                    createCheckBox(getMatrixUnit(col, row), i, j);
                    _matrix.Add(
                        new ValuePair<int, int>(i, j),
                        new ValuePair<AudioMixerInput, AudioMixerOutput>(col, row));
                }
            }
            RefreshInputLabelLocation();
            //RefreshOutputLabelLocation();
        }

        public int GetMinWidthControl(int countInputs)
        {
            return Math.Min(this.MaximumSize.Width, 
                countInputs * _minColWidth + gpOutput.Width + (_rowNumber > 11 ? _scrollBarWidth : 0)) + 25;
            //- магическое число, возникает из-за того этот дочерний контрол всегда чуть меньше чем родительский
        }

        /// <summary>
        /// вернет состояние матрицы в таком эе виде как и задано в AudioMixerDeviceDesign
        /// </summary>
        public List<AudioMixerMatrixUnit> GetState
        {
            get
            {
                List<AudioMixerMatrixUnit> result = new List<AudioMixerMatrixUnit>();
                foreach (Control item in gpDetail.Controls)
                {
                    CheckBoxAdv cntrl = item as CheckBoxAdv;
                    if ((cntrl != null) && (cntrl.Name.StartsWith("CB_")) && cntrl.Checked)
                    {
                        string[] tmp = cntrl.Name.Split('_');
                        ValuePair<int, int> key = new ValuePair<int, int>(
                            Convert.ToInt32(tmp[1])-1,
                            Convert.ToInt32(tmp[2])-1);
                        AudioMixerMatrixUnit mUnit = new AudioMixerMatrixUnit(_matrix[key].Value1, _matrix[key].Value2, true);
                        result.Add(mUnit);
                    }
                }
                return result;
            }
        }

        public void UpdateMixerTies(bool[][] tiesState)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<bool[][]>(UpdateMixerTies), tiesState);
                return;
            }
            for (int i = 0; i < tiesState.Length; i++)
            {
                for (int j = 0; j < tiesState[i].Length; j++)
                {
                    //формат: CB_{номер выхода}_{номер входа}
                    Control[] cntrls = gpDetail.Controls.Find(string.Format("CB_{0}_{1}", i + 1, j + 1), false);
                    if ((cntrls != null) && (cntrls.Length > 0))
                    {
                        CheckBoxAdv cntrl = cntrls[0] as CheckBoxAdv;
                        if (cntrl != null)
                        {
                            cntrl.CheckedChanged -= chck_CheckedChanged;
                            cntrl.Checked = tiesState[i][j];
                            cntrl.CheckedChanged += chck_CheckedChanged;
                        }
                    }
                }
            }
        }
        public event Action<string, IConvertible[]> OnAudioMixerMatrixStateEvent;

        private void createLabel(AudioMixerOutput put, int num)
        {
            AutoLabel al = new AutoLabel();
            al.Location = new Point(1, (num + 1)* _heightRow + 1);
            al.Text = put.Name;
            al.Name = "AMO_"+(num + 1).ToString() + "_1";
            if (((num + 1) >= 10) && (_colNumber > 12)) al.Visible = false;
            if ((num + 1) > 10) al.Visible = false;
            gpOutput.Controls.Add(al);
        }

        private void createLabel(AudioMixerInput put, int num)
        {
            AutoLabel al = new AutoLabel();
            al.Location = new Point(num * _widthCol, 1);
            al.AutoSize = false;
            al.TextAlign = ContentAlignment.MiddleCenter;
            al.Size = new Size(_widthCol, 18);
            al.Text = put.Name;
            al.Padding = new Padding(0);
            al.Margin = new Padding(0);
            al.Name = "AMI_1_" + (num + 1).ToString();
            gpInput.Controls.Add(al);
        }

        private void createCheckBox(bool unit, int row, int col)
        {
            int _size = 16;
            CheckBoxAdv chck = new CheckBoxAdv();
            chck.Text = string.Empty;
            chck.Name = string.Format("CB_{0}_{1}", row + 1, col + 1);//формат: CB_{номер выхода}_{номер входа}
            chck.Location = new Point(col * _widthCol, row * _heightRow);
            chck.Left += (_widthCol - _size) / 2;
            chck.Size = new Size(_size, _size);
            chck.Style = CheckBoxAdvStyle.Office2007;
            chck.ThemesEnabled = true;
            chck.Tag = "MixerSetTie";
            chck.CheckedChanged += chck_CheckedChanged;
            chck.Checked = unit;
            gpDetail.Controls.Add(chck);
        }

        private void chck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBoxAdv cntrl = sender as CheckBoxAdv;
            if (cntrl != null)
            {
                string[] tmp = cntrl.Name.Split('_');
                if ((OnAudioMixerMatrixStateEvent != null) && (tmp.Length >=3))
                {
                    int numOut = Convert.ToInt32(tmp[2]);
                    int numIn = Convert.ToInt32(tmp[1]);
                    int val = Convert.ToInt32(cntrl.Checked);
                    OnAudioMixerMatrixStateEvent((string)cntrl.Tag, new IConvertible[] { _matrixId, numIn, numOut, val });
                }
            }
        }

        private void gpDetail_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                RefreshInputLabelLocation();
            else if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                RefreshOutputLabelLocation();
        }

        private void RefreshInputLabelLocation()
        {
            for (int i = 1; i <= _colNumber; i++)
            {
                Control[] cntrls = gpDetail.Controls.Find(string.Format("CB_{0}_{1}", 1, i), false);
                if ((cntrls != null) && (cntrls.Length > 0) && (cntrls[0] is CheckBoxAdv))
                {
                    CheckBoxAdv item = cntrls[0] as CheckBoxAdv;
                    Control[] cntrls2 = gpInput.Controls.Find(string.Format("AMI_{0}_{1}", 1, i), false);
                    cntrls2[0].Visible = true;
                    cntrls2[0].Left = item.Left - 6;
                    if (cntrls2[0].Left > gpDetail.ClientRectangle.Width - 10)
                        cntrls2[0].Visible = false;
                }
            }
        }
        private void RefreshOutputLabelLocation()
        {
            for (int i = 1; i <= _rowNumber; i++)
            {
                Control[] cntrls = gpDetail.Controls.Find(string.Format("CB_{0}_{1}", i, 1), false);
                if ((cntrls != null) && (cntrls.Length > 0) && (cntrls[0] is CheckBoxAdv))
                {
                    CheckBoxAdv item = cntrls[0] as CheckBoxAdv;
                    Control[] cntrls2 = gpOutput.Controls.Find(string.Format("AMO_{0}_{1}", i, 1), false);
                    cntrls2[0].Visible = true;
                    cntrls2[0].Top = item.Top + 23;
                    if ((cntrls2[0].Top > gpDetail.ClientRectangle.Height + 17) || (cntrls2[0].Top < 13))
                        cntrls2[0].Visible = false;
                }
            }
        }
    }

    
}
