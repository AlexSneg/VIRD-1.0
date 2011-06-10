using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using dotnetCHARTING.WinForms;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Exceptions;
using Hosts.Plugins.BusinessGraphics.UI.Controls;
using TechnicalServices.Common.TypeConverters;
using Syncfusion.Windows.Forms.Tools;
using System.Globalization;
using Syncfusion.Windows.Forms.Diagram;

namespace Hosts.Plugins.BusinessGraphics.UI
{
    public partial class ChartSetupForm : Office2007Form
    {
        CustomChart chart1;
        public CustomChart Chart
        {
            get
            {
                return chart1;
            }
            set
            {
                chart1 = value;
                propertyGrid1.SelectedObject = new ChartWrapper(value);
                source.isChartSetupMode = true;
            }
        }

        const string defaultName = "По умолчанию";

        BusinessGraphicsSourceDesign source;
        string DiagramStyle;
        string DiagramStyleData;
        string SubDiagramStyle;
        string SubDiagramStyleData;

        SuperToolTip tip;

        public ChartSetupForm(BusinessGraphicsSourceDesign source)
        {
            InitializeComponent();

            // Устанавливаем размер документа, чтобы не было лишнего пустого поля.
            diagram1.Model.DocumentSize.Width = source.Chart.Width;
            diagram1.Model.DocumentSize.Height = source.Chart.Height;
            this.source = source;

            if (String.IsNullOrEmpty(source.DiagramStyle))
                source.DiagramStyle = defaultName;

            if (String.IsNullOrEmpty(source.SubDiagramStyle))
                source.SubDiagramStyle = defaultName;

            source.LoadStyles();
            this.DiagramStyleData = source.DiagramStyleData;
            this.DiagramStyle = source.DiagramStyle;
            this.SubDiagramStyleData = source.SubDiagramStyleData;
            this.SubDiagramStyle = source.SubDiagramStyle;

            if (source.DiagramStyle != defaultName && source.DiagramStyleData==null) this.DiagramStyleData = this.GetStyleData(source.DiagramStyle);
            if (source.SubDiagramStyle != defaultName && source.SubDiagramStyleData == null) this.SubDiagramStyleData = this.GetStyleData(source.SubDiagramStyle);

            this.tabControlAdv1.SelectedIndexChanged += new System.EventHandler(this.tabControlAdv1_SelectedIndexChanged);
            ReloadStyleList();
            styleComboBox.SelectedItem = DiagramStyle;

            source.InitializeChart(true);
            this.chart1 = source.Chart;

            LoadStyle();
            RefreshChart();
            this.FormClosing += new FormClosingEventHandler(ChartSetupForm_FormClosing);
            styleComboBox.SelectedValueChanged += new EventHandler(styleComboBox_SelectedValueChanged);

            saveButton.Enabled = styleComboBox.SelectedValue.ToString() != defaultName;
            removeButton.Enabled = styleComboBox.SelectedValue.ToString() != defaultName;
            if (!source.AllowSubDiagram)
            {
                this.tabControlAdv1.TabPages[1].Hide();
            }
            else
            {
                this.tabControlAdv1.TabPages[1].Show();
            }

            zoomCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            zoomCombo.Items.AddRange(Percent.CreateList());
            zoomCombo.Text = "100%";
            
            tip = new SuperToolTip();
            tip.UseFading = SuperToolTip.FadingType.System;
        }

        /// <summary>
        /// Просмотриваем детализированную диаграмму.
        /// </summary>
        private bool IsDetailedMode
        {
            get
            {
                return this.tabControlAdv1.SelectedIndex != 0;
            }
        }
        /// <summary>
        /// Признак перезагрузки списка стилей.
        /// </summary>
        bool reloadFlag = false;
        /// <summary>
        /// Обновить список доступных стилей.
        /// </summary>
        private void ReloadStyleList()
        {
            try
            {
                reloadFlag = true;
                styleComboBox.DataSource = null;
                List<string> styleNames = new List<string>(BusinessGraphicsSourceDesign.DiagramStyleList.Where(p => StyleForType(p)));
                styleComboBox.DataSource = styleNames;
            }
            finally { reloadFlag = false; }
        }

        /// <summary>
        /// Соответствует ли стиль текущему типу диаграммы.
        /// </summary>
        bool StyleForType(string style)
        {
            if (style == defaultName) return true;
            DiagramTypeEnum type = source.stylesInfo.GetDiagramType(style);
            if (GetCurrentDiagramType() == type) return true;
            else return false;
        }

        /// <summary>
        /// Признак того, что в текущем стиле что-то изменялось.
        /// </summary>
        private bool somethingChanged = false;

        void ChartSetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chart1 != null)
            {
                source.isChartSetupMode = false;
                //source.InitializeChart(true);
            }
        }

        private void chart1_Load(object sender, EventArgs e)
        {
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            somethingChanged = true;
            RefreshChart();

            if (source.AutoSetMode) // Если включена автонастройка, то проверять пользовательский ввод.
            {
                {
                    Font changedFont = null;
                    if (e.ChangedItem.Label == "Font")
                    {
                        changedFont = e.ChangedItem.Value as Font;
                    }
                    if (e.ChangedItem.Parent.Label == "Font")
                    {
                        changedFont = e.ChangedItem.Parent.Value as Font;
                    }
                    if (changedFont != null)
                    {
                        if (source.CheckFontSizeIncorrect(changedFont))
                        {
                            MessageBoxAdv.Show("Размер шрифта не соответствует требованиям эргономичности. Рекомендуется увеличить размер шрифта", "Внимание", MessageBoxButtons.OK);
                        }
                    }
                }

                if (e.ChangedItem.Label == "Width" && e.ChangedItem.Parent != null && e.ChangedItem.Parent.Label == "Line" && e.ChangedItem.Value is int)
                {
                    float width = (float)Convert.ToDouble(e.ChangedItem.Value);
                    if (source.CheckLineWidthIncorrect(width))
                    {
                        MessageBoxAdv.Show("Толщина линии не соответствует требованиям эргономичности. Рекомендуется увеличить толщину линии", "Внимание", MessageBoxButtons.OK);
                    }
                }

                if (e.ChangedItem.Label == "Interval" && e.ChangedItem.Parent != null && e.ChangedItem.Parent.Value is Axis && e.ChangedItem.Value is double)
                {
                    float interval = (float)Convert.ToDouble(e.ChangedItem.Value);
                    if (source.CheckGridIntervalIncorrect(interval))
                    {
                        MessageBoxAdv.Show("Частота сетки не соответствует требованиям эргономичности. Рекомендуется увеличить толщину линии", "Внимание", MessageBoxButtons.OK);
                    }
                }
            }
        }


        /// <summary>
        /// Обновить отображаемую диаграмму.
        /// </summary>
        private void RefreshChart(bool fromSource)
        {
            splitContainer1.Panel2.SuspendLayout();
            CustomChart c;
            if (!fromSource && propertyGrid1.SelectedObject != null) c = (CustomChart)((ChartWrapper)(propertyGrid1.SelectedObject)).GetPropertyOwner(null);
            else c = chart1;
            c.RefreshChart(); // Обновим внешний вид перед выводом
            Bitmap bitmap=new Bitmap(chart1.Width, chart1.Height);
            chart1.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, chart1.Width, chart1.Height));
            model1.Clear();
            model1.AppendChild(new ImageNode(bitmap));
            splitContainer1.Panel2.ResumeLayout();
        }

        /// <summary>
        /// Обновить отображаемую диаграмму.
        /// </summary>
        private void RefreshChart()
        {
            RefreshChart(false);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (tabControlAdv1.SelectedIndex == 0)
            {
                if (this.DiagramStyle != defaultName)
                {
                    SaveStyle(this.DiagramStyle);
                }
            }
            else
            {
                if (this.SubDiagramStyle != defaultName)
                {
                    SaveStyle(this.SubDiagramStyle);
                }
            }
        }

        private void saveAs_Click(object sender, EventArgs e)
        {
            using (InputForm i = new InputForm())
            {
                i.StyleName = tabControlAdv1.SelectedIndex == 0 ? source.DiagramStyle : source.SubDiagramStyle;
                while (i.ShowDialog() == DialogResult.OK)
                {
                    string styleName = i.StyleName.Trim();
                    if (BusinessGraphicsSourceDesign.DiagramStyleList.Contains(styleName))
                    {
                        if (MessageBoxAdv.Show("Стиль с таким именем уже существует. Создать копию стиля?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            styleName = "Копия " + styleName;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (!BusinessGraphicsSourceDesign.DiagramStyleList.Contains(styleName))
                    {
                        SaveStyle(styleName);
                        if (tabControlAdv1.SelectedIndex == 0)
                            this.DiagramStyle = styleName;
                        else
                            this.SubDiagramStyle = styleName;

                        BusinessGraphicsSourceDesign.DiagramStyleList.Add(styleName);
                        ReloadStyleList();
                        styleComboBox.SelectedItem = styleName;

                        break;
                    }
                    else
                        MessageBoxAdv.Show("Копия стиля уже также имеется. Выберите новое название.");
                }
            }
        }

        private void SaveStyle(string styleName)
        {
            bool notCreated = source.stylesInfo == null;
            if (notCreated)
                source.stylesInfo = new StyleResourceInfo() { Name = "StyleResourceInfo" };
            DiagramTypeEnum type = GetCurrentDiagramType();
            source.stylesInfo.SaveStyle(styleName, this.chart1, type);
            UpdateStylesInfo(notCreated);

            source.InitializeChart(true); // В процессе сохранения сбрасываются данные диаграммы, надо восстановить.
            LoadStyle();
        }

        /// <summary>
        /// Получить тип текущей диаграммы (для детализации возвращается аналог).
        /// </summary>
        /// <returns></returns>
        private DiagramTypeEnum GetCurrentDiagramType()
        {
            DiagramTypeEnum type;
            if (tabControlAdv1.SelectedIndex == 0)
            {
                switch (source.DiagramType) // Тип для основной
                {
                    case DiagramTypeEnum.ColumnDetail: type = DiagramTypeEnum.ColumnGeneral; break;
                    case DiagramTypeEnum.PieDetail: type = DiagramTypeEnum.PieGeneral; break;
                    default: type = source.DiagramType; break;
                }
            }
            else
            {
                switch (source.SubDiagramType) // Тип для детализированной
                {
                    case SubDiagramTypeEnum.ColumnDetail: type = DiagramTypeEnum.ColumnGeneral; break;
                    case SubDiagramTypeEnum.Graph: type = DiagramTypeEnum.Graph; break;
                    case SubDiagramTypeEnum.PieDetail: type = DiagramTypeEnum.PieGeneral; break;
                    default: type = DiagramTypeEnum.ColumnDetail; break;
                }
            }
            return type;
        }

        private void remove_Click(object sender, EventArgs e)
        {
            string styleName = IsDetailedMode ? SubDiagramStyle : DiagramStyle;

            if (styleName != defaultName)
            {
                if (MessageBoxAdv.Show("Подтвердите удаление стиля?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    RemoveStyle();
                }
            }
        }

        void styleComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (reloadFlag) return; // Идет перезагрузка стилей, ничего делать не надо
            if (styleComboBox.SelectedValue != null)
            {
                if (IsDetailedMode)
                {
                    this.SubDiagramStyle = styleComboBox.Text;
                    this.SubDiagramStyleData = GetStyleData(styleComboBox.Text);
                }
                else
                {
                    this.DiagramStyle = styleComboBox.Text;
                    this.DiagramStyleData = GetStyleData(styleComboBox.Text);
                }
                LoadStyle();

                saveButton.Enabled = styleComboBox.SelectedValue.ToString() != defaultName;
                removeButton.Enabled = styleComboBox.SelectedValue.ToString() != defaultName;
            }
            propertyGrid1.Refresh();
        }

        /// <summary>
        /// Получить настройки стиля.
        /// </summary>
        /// <param name="styleName">Имя стиля.</param>
        /// <returns>Строка, содержащая настройки.</returns>
        private string GetStyleData(string styleName)
        {
            if (source.stylesInfo == null) return null;
            return source.stylesInfo.GetStyleData(styleName);
        }


        /// <summary>
        /// Загрузить текущий стиль и обновить диаграмму.
        /// </summary>
        private void LoadStyle()
        {
            string data = IsDetailedMode ? this.SubDiagramStyleData : this.DiagramStyleData;
            if (data != null)
            {
                source.Chart.ClearStyle();
                this.chart1.LoadStyle(data);
            }
            else
            {
                this.chart1.ClearStyle();
            }
            source.FixChartErgonomic(this.chart1);
            RefreshChart(true);
        }

        private void RemoveStyle()
        {
            bool notCreated = source.stylesInfo == null;
            if (notCreated)
                source.stylesInfo = new StyleResourceInfo() { Name = "StyleResourceInfo" };

            string sName = IsDetailedMode ? SubDiagramStyle : DiagramStyle;

            source.stylesInfo.RemoveStyle(sName);
            this.DiagramStyleData = null;
            styleComboBox.SelectedItem = defaultName;
            BusinessGraphicsSourceDesign.DiagramStyleList.Remove(sName);
            ReloadStyleList();
            UpdateStylesInfo(notCreated);
        }

        private void UpdateStylesInfo(bool notCreated)
        {
            source.stylesInfo.UpdateStyles();
            string otherResourceId;
            if (notCreated)
            {
                source.resourceCRUD.CreateSource(new ResourceDescriptor(false, String.Empty, source.stylesInfo), out otherResourceId);
            }
            else
            {
                String s;
                source.resourceCRUD.SaveSource(new ResourceDescriptor(false, String.Empty, source.stylesInfo), out otherResourceId);
            }
        }

        private void tabControlAdv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlAdv1.SelectedIndex == 0)
            {
                if (somethingChanged) this.SubDiagramStyleData = chart1.SaveStyle();
                source.SetNormalMode();
                this.chart1 = source.Chart;
                Chart = chart1;
            }
            if (tabControlAdv1.SelectedIndex == 1)
            {
                if (somethingChanged) this.DiagramStyleData = chart1.SaveStyle();
                source.SetDetailedMode();
                this.chart1 = source.Chart;
                source.SetDetailedMode();
                Chart = chart1;
            }
            ReloadStyleList();
            //RefreshChart();
            LoadStyle();

            try
            {
                reloadFlag = true;

                if (tabControlAdv1.SelectedIndex == 0)
                {
                    styleComboBox.SelectedItem = DiagramStyle;
                }
                else
                {
                    styleComboBox.SelectedItem = SubDiagramStyle;
                }
            }
            finally
            {
                reloadFlag = false;
            }
            somethingChanged = false;
        }

        private void ChartSetupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            chart1.ClearStyle();
            // Перейдем в полный режим и перенециализируем диаграмму
            tabControlAdv1.SelectedIndex = 0;
            source.isChartSetupMode = false;
            //source.InitializeChart(true);
            LoadStyle();
            // Для карт есть какой-то глюк. После chart.SaveState() любой вызов char.DrawToBitmap сбрасывает Shapes.Hotspot.URI
            // От этого такой жестокий workaround с пересозданием объекта диаграммы.
            // https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1701
            // Переинициализируем.
            if (source.DiagramType == DiagramTypeEnum.Map)
            {
                source.ReInitChart(true, false);
                source.InitializeChart(true);
                source.ApplyChartStyle();
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (IsDetailedMode)
            {
                source.SubDiagramStyleData = chart1.SaveStyle();
                source.DiagramStyleData = DiagramStyleData;
            }
            else
            {
                source.DiagramStyleData = chart1.SaveStyle();
                source.SubDiagramStyleData = SubDiagramStyleData;
            }
            this.DiagramStyleData = source.DiagramStyleData;
            this.SubDiagramStyleData = source.SubDiagramStyleData;
            source.DiagramStyle = DiagramStyle;
            source.SubDiagramStyle = SubDiagramStyle;
        }

        private void zoomCombo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (byte)Keys.Enter)
            {
                float value;
                if (ValidateInput(out value))
                {
                    if (value < Percent.MinValue)
                        value = Percent.MinValue;

                    if (value > Percent.MaxValue)
                        value = Percent.MaxValue;

                    this.Text = Percent.GetTextForZoom(value);
                    diagram1.View.Magnification = value;
                }
               
                e.Handled = true;
            }

            if(!e.Handled)
                base.OnKeyPress(e);
        }
        
        bool ValidateInput(out float value)
        {
            value = 0;
            float f;
            if (float.TryParse(zoomCombo.Text, out f))
            {
                value = f;
                zoomCombo.Text = Math.Round(f, 0, MidpointRounding.AwayFromZero).ToString() + "%";
                return true;
            }
            else
            {
                //вернуть первоначальное значение
                if (diagram1.View != null)
                    zoomCombo.Text = Percent.GetTextForZoom(diagram1.View.Magnification);
                else
                    zoomCombo.SelectedIndex = Percent.GetIndexForPercentage(100);

                //show error
                ToolTipInfo t_info = new ToolTipInfo();
                t_info.Header.Text = "Ошибка";
                t_info.Body.Text = String.Format("Для масштаба требуется числовое значение (десятичный разделитель: \"{0}\")", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                t_info.Footer.Text = "Значение масштаба указывается в процентах";
                tip.Show(t_info, this.PointToScreen(new Point(zoomCombo.Bounds.Right, zoomCombo.Bounds.Bottom)), 1000);
                zoomCombo.ComboBox.Focus();

                return false;
            }
        }

        private void zoomCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (diagram1.View != null)
            {
                diagram1.View.Magnification = (zoomCombo.Items[zoomCombo.SelectedIndex] as Percent).Value;
            }
        }
    }

    /// <summary>
    /// Аналог ZoomComboHelpe. Сделан, так как нельзя из плагина лездь в DesignUI.
    /// </summary>
    internal class Percent
    {
        static List<Percent> lst = new List<Percent>();
        
        public const int MaxValue = 500;
        public const int MinValue = 10;

        static Percent()
        {
            lst.Add(new Percent { Value = 10 });
            lst.Add(new Percent { Value = 20 });
            lst.Add(new Percent { Value = 50 });
            lst.Add(new Percent { Value = 100 });
            lst.Add(new Percent { Value = 150 });
            lst.Add(new Percent { Value = 200 });
            lst.Add(new Percent { Value = 300 });
            lst.Add(new Percent { Value = 500 });
        }
        
        public int Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Value + "%";
        }

        public static Percent[] CreateList()
        {

            return lst.ToArray();
        }
        
        public static int GetIndexForPercentage(int percent)
        {
            var l = lst.Find(z => z.Value == percent);
            if (l != null)
                return lst.IndexOf(l);
            return -1;
        }

        public static string GetTextForZoom(float m)
        {
            return String.Format("{0}%", Math.Round(m));
        }
    }

    /// <summary>
    /// Перенесен из UIDesign. 
    /// </summary>
    public class ImageNode : Syncfusion.Windows.Forms.Diagram.Rectangle
    {
        Image _image;

        public ImageNode(ImageNode dest)
            : base(dest)
        {
            this.EnableShading = true;
        }


        public ImageNode(Image img)
            : base(0, 0, img.Width, img.Height, MeasureUnits.Pixel)
        {
            _image = img;
            this.EditStyle.HidePinPoint = true;
            this.EditStyle.HideRotationHandle = true;
            this.EditStyle.Enabled = false;
        }

        public override object Clone()
        {
            ImageNode node = new ImageNode(this);
            node._image = this._image;
            return node;
        }

        protected override void Render(System.Drawing.Graphics gfx)
        {
            base.Render(gfx);
            gfx.DrawImage(_image, this.BoundingRect);
        }
    }
}
