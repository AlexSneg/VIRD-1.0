using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnetCHARTING.WinForms;
using System.Windows.Forms;
using System.Drawing;
using Syncfusion.Windows.Forms.Tools;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design;
using System.Xml;
using Hosts.Plugins.BusinessGraphics.UI.Controls.Wrappers;
using System.Xml.Serialization;

namespace Hosts.Plugins.BusinessGraphics.UI.Controls
{
    public partial class CustomChart : Chart
    {
        /// <summary>находится контрол в режиме редактирования стиля </summary>
        internal bool isChartSetupMode = false;

        private bool _allowUserInteraction;
        /// <summary>разрешены ли интерактивные действия пользователем </summary>
        public bool AllowUserInteraction 
        { 
            get { return _allowUserInteraction; }
            set { _allowUserInteraction = value; }
        }
        private bool _isAllowMouseClickProcess = true;
        /// <summary>если кто отловил сообщение от chart-a и не хочет чтобы оно пошло на обработку 
        /// внутрь самого чарта, установи это флаг, флаг сбрасывается внутри чарта </summary>
        public bool isAllowMouseClickProcess 
        { 
            get {return _isAllowMouseClickProcess;} 
            set {_isAllowMouseClickProcess = value;} 
        }


        private DiagramTypeEnum _diagramType;
        /// <summary>тип диаграммы </summary>
        public DiagramTypeEnum DiagramType { get { return _diagramType; } }

        private SubDiagramTypeEnum _subDiagramType;
        /// <summary>подтип диаграммы </summary>
        public SubDiagramTypeEnum SubDiagramType { get { return _subDiagramType; } }
        
        private bool _subDiagramMode;
        /// <summary>режим диаграммы обобщенный/детализированный </summary>
        public bool SubDiagramMode { get { return _subDiagramMode; } }

        private List<ValueRange> _valueRange;
        public List<ValueRange> ValueRange { get { return _valueRange; } }

        public CustomChart()
            : base()
        {
            ChartArea.LegendBox.HeaderLabel.Font = MakeNonArialFont(ChartArea.LegendBox.HeaderLabel.Font);
            ChartArea.LegendBox.LabelStyle.Font = MakeNonArialFont(ChartArea.LegendBox.LabelStyle.Font);
            ChartArea.TitleBox.Label.Font = MakeNonArialFont(ChartArea.TitleBox.Label.Font);
            ChartArea.XAxis.Label.Font = MakeNonArialFont(ChartArea.XAxis.Label.Font);
            ChartArea.YAxis.Label.Font = MakeNonArialFont(ChartArea.YAxis.Label.Font);
            ChartArea.XAxis.ZeroTick.Label.Font = MakeNonArialFont(ChartArea.XAxis.ZeroTick.Label.Font);
            ChartArea.YAxis.ZeroTick.Label.Font = MakeNonArialFont(ChartArea.YAxis.ZeroTick.Label.Font);
            ChartArea.XAxis.DefaultTick.Label.Font = MakeNonArialFont(ChartArea.XAxis.DefaultTick.Label.Font);
            ChartArea.YAxis.DefaultTick.Label.Font = MakeNonArialFont(ChartArea.YAxis.DefaultTick.Label.Font);
            ChartArea.LegendBox.HeaderEntry.LabelStyle.Font = MakeNonArialFont(ChartArea.LegendBox.HeaderEntry.LabelStyle.Font);
            TitleBox.HeaderLabel.Font = MakeNonArialFont(TitleBox.HeaderLabel.Font);
            DefaultElement.SmartLabel.Font = MakeNonArialFont(DefaultElement.SmartLabel.Font);
            //DefaultElement.HatchColor = Color.FromArgb(0, Color.Red);
            //DefaultSeries.DefaultElement.HatchColor = Color.FromArgb(0, Color.Blue);

            ///Без этого десериализация не проходит.
            //AppDomain.CurrentDomain.AppendPrivatePath(System.Windows.Forms.Application.StartupPath + "\\Module");
        }

        /// <summary>
        /// Изменить начертание шрифта на какой-нибудь, кроме Arial.
        /// Arial плохо отображается в диаграмме при больших размерах.
        /// </summary>
        /// <param name="font">Шрифт.</param>
        /// <returns>Шрифт с измененным начертанием.</returns>
        private Font MakeNonArialFont(Font font)
        {
            return new Font("Tahoma", font.Size, font.Style);
        }

        /// <summary>возвращает котрол в исходное положение(без данных) </summary>
        public void ReInitChart(List<ValueRange> valueRange,
            DiagramTypeEnum diagramType, SubDiagramTypeEnum subDiagramType, bool allowUserInteraction,
            bool clearState, bool subDiagramMode)
        {
            _subDiagramMode = subDiagramMode;
            _diagramType = diagramType;
            _subDiagramType = subDiagramType;
            _valueRange = valueRange;
            _allowUserInteraction = allowUserInteraction;

            base.SeriesCollection.Clear();
            base.Mapping.MapLayerCollection.Clear();

            if (clearState)
            {
                base.ExtraChartAreas.Clear();
                base.XAxis.Markers.Clear();
                base.YAxis.Markers.Clear();
                base.SmartPalette.Clear();

                ClearStyle();
            }
        }

        /// <summary>
        /// Очистить стиль диаграммы.
        /// </summary>
        public void ClearStyle()
        {
            CustomChart chart = new CustomChart();
            LoadSettingsFromChart(chart);
        }

        /// <summary>
        /// Загрузить сохраненный стиль.
        /// </summary>
        /// <param name="data">Строка, содержащая сохраненный стиль.</param>
        public void LoadStyle(string data)
        {
            #region Куски старого механизма сохранения стилей
            // Создадим чарт с новым стилем 
            //CustomChart chart = new CustomChart();
            //this.LoadState(data, chart, true);
            //LoadSettingsFromChart(chart);
            #endregion

            //SoapFormatter formatter = new SoapFormatter();
            XmlSerializer serializer = new XmlSerializer(typeof(ChartSettingsWrapper));
            System.IO.MemoryStream ms = new System.IO.MemoryStream(Convert.FromBase64String(data));
            System.IO.Compression.GZipStream gzs = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress);

            //ChartSettingsWrapper my = (ChartSettingsWrapper)formatter.Deserialize(gzs);
            ChartSettingsWrapper my = (ChartSettingsWrapper)serializer.Deserialize(gzs);
            my.RestoreSettings(this);

            //base.LegendBox.Position = new Rectangle(new Point(0, 0), new Size(50, 50));
            //base.LegendBox.Position = new Size(50, 500);
        }

        /// <summary>
        /// Загрузить настройки из существующей диаграммы.
        /// </summary>
        /// <param name="chart">Диаграмма, настройки которой загружаются.</param>
        private void LoadSettingsFromChart(CustomChart chart)
        {
            base.Debug = chart.Debug;
            base.ClipGauges = chart.ClipGauges;
            base.LegendBox.Visible = chart.LegendBox.Visible;
            base.XAxis.Orientation = chart.XAxis.Orientation;
            base.DefaultSeries.Type = chart.DefaultSeries.Type;
            base.DefaultSeries.GaugeBorderShape = chart.DefaultSeries.GaugeBorderShape;
            base.DefaultSeries.GaugeType = chart.DefaultSeries.GaugeType;
            base.DefaultSeries.GaugeBorderBox.DefaultCorner = chart.DefaultSeries.GaugeBorderBox.DefaultCorner;
            base.ShadingEffectMode = chart.ShadingEffectMode;
            base.ChartArea.Background = chart.ChartArea.Background;
            base.ChartArea.Background.Mode = chart.ChartArea.Background.Mode;
            base.DefaultSeries.GaugeBorderBox.Background.Color = chart.DefaultSeries.GaugeBorderBox.Background.Color;

            base.DefaultSeries.DefaultElement.LabelTemplate = chart.DefaultSeries.DefaultElement.LabelTemplate;
            base.Palette = chart.Palette;
            base.DefaultSeries.DefaultElement.Transparency = chart.DefaultSeries.DefaultElement.Transparency;

            base.ChartArea.LegendBox.HeaderLabel = chart.ChartArea.LegendBox.HeaderLabel;
            base.ChartArea.LegendBox.LabelStyle = chart.ChartArea.LegendBox.LabelStyle;
            string titleText = base.ChartArea.TitleBox.Label.Text; // Сохраним название, чтобы не затерлось
            base.ChartArea.TitleBox.Label = chart.ChartArea.TitleBox.Label;
            base.ChartArea.TitleBox.Label.Text = titleText;
            base.ChartArea.XAxis.Label = chart.ChartArea.XAxis.Label;
            base.ChartArea.YAxis.Label = chart.ChartArea.YAxis.Label;
            base.ChartArea.XAxis.ZeroTick.Label = chart.ChartArea.XAxis.ZeroTick.Label;
            base.ChartArea.YAxis.ZeroTick.Label = chart.ChartArea.YAxis.ZeroTick.Label;
            base.ChartArea.XAxis.DefaultTick.Label = chart.ChartArea.XAxis.DefaultTick.Label;
            base.ChartArea.YAxis.DefaultTick.Label = chart.ChartArea.YAxis.DefaultTick.Label;
            base.DefaultElement.SmartLabel = chart.DefaultElement.SmartLabel;

            base.LegendBox = chart.LegendBox;
            base.ChartArea = chart.ChartArea;
            base.Background = chart.Background;
            base.DefaultElement = chart.DefaultElement;

            base.BackgroundImage = chart.BackgroundImage;
            base.BackgroundImageLayout = chart.BackgroundImageLayout;
            base.BorderStyle = chart.BorderStyle;
            base.Background = chart.Background;
            base.Depth = chart.Depth;
            base.ExplodedSliceAmount = chart.ExplodedSliceAmount;
            base.PieLabelMode = chart.PieLabelMode;
            base.ShadingEffectMode = chart.ShadingEffectMode;
            base.Use3D = chart.Use3D;
            base.XAxis = chart.XAxis;
            base.YAxis = chart.YAxis;
            base.TitleBox = chart.TitleBox;

            SetDiagramType(_subDiagramMode);
        }

        /// <summary>устанавливает тип и режим диаграммы (влияет на внешний вид) </summary>
        public void SetDiagramType(bool subDiagramMode)
        {
            _subDiagramMode = subDiagramMode;
            if (!subDiagramMode)
            {
                switch (DiagramType)
                {
                    case DiagramTypeEnum.ColumnGeneral:
                    case DiagramTypeEnum.ColumnDetail:
                        base.Type = ChartType.Combo;
                        break;
                    case DiagramTypeEnum.Map:
                        base.Type = ChartType.Map;
                        base.ChartArea.Background = new Background(Color.FromArgb(142, 195, 236), Color.FromArgb(63, 137, 200), 90);
                        base.LegendBox.Visible = false;
                        break;
                    case DiagramTypeEnum.PieGeneral:
                        base.Type = ChartType.Pies;
                        break;
                    case DiagramTypeEnum.PieDetail:
                        base.Type = ChartType.Pies;
                        base.PaletteName = dotnetCHARTING.WinForms.Palette.Two;
                        break;
                    case DiagramTypeEnum.Graph:
                        base.Type = ChartType.Combo;
                        base.DefaultSeries.Type = SeriesType.Line;
                        break;
                    case DiagramTypeEnum.Speedometer:
                        this.YAxis.Markers.Clear();
                        base.Type = ChartType.Gauges;
                        base.LegendBox.Position = LegendBoxPosition.None;
                        base.DefaultSeries.Background.Color = Color.White;
                        base.ClipGauges = false;
                        base.Use3D = true;
                        foreach (ValueRange r in _valueRange)
                        {
                            AxisMarker am = new AxisMarker("", new Background(r.Color), r.MinValue, r.MaxValue);
                            base.YAxis.Markers.Add(am);
                        }
                        break;
                    case DiagramTypeEnum.TrafficLight:
                        base.Type = ChartType.Gauges;
                        base.DefaultSeries.GaugeType = GaugeType.IndicatorLight;
                        base.LegendBox.Visible = false;
                        //base.ChartArea.ClearColors();
                        base.DefaultSeries.GaugeBorderShape = GaugeBorderShape.UseBox;
                        base.DefaultSeries.GaugeBorderBox.DefaultCorner = BoxCorner.Round;
                        base.DefaultSeries.GaugeBorderBox.Background.Color = Color.FromArgb(20, Color.Blue);
                        base.DefaultElement.SmartLabel.Color = Color.Black;
                        base.XAxis.Orientation = dotnetCHARTING.WinForms.Orientation.Top;
                        SmartPalette sp = new SmartPalette();
                        foreach (ValueRange r in _valueRange)
                        {
                            SmartColor sc = new SmartColor(r.Color, new ScaleRange(r.MinValue, r.MaxValue));
                            sp.Add("*", sc);
                        }
                        base.SmartPalette = sp;
                        break;
                    default: 
                        throw new NotSupportedException("Данный тип не поддерживается");
                }
            }
            else
            {
                switch (SubDiagramType)
                {
                    case SubDiagramTypeEnum.ColumnDetail:
                        base.Type = ChartType.Combo;
                        break;
                    case SubDiagramTypeEnum.PieDetail:
                        base.Type = ChartType.Pies;
                        break;
                    case SubDiagramTypeEnum.Graph:
                        base.Type = ChartType.Combo;
                        base.DefaultSeries.Type = SeriesType.Line;
                        break;
                    default:
                        throw new NotSupportedException("Данный тип для детализации не поддерживается");
                }
            }
        }


        /// <summary>
        /// Сохранить стиль диаграммы в строку.
        /// </summary>
        /// <returns>Сохраненные настройки стиля.</returns>
        public string SaveStyle()
        {
            #region Куски старого механизма сохранения стилей
            // После считывания сохраненного стиля многое ломается.
            // Например PMEDIAINFOVISDEV-1827.
            //string style = base.SaveState();
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(style);
            //XmlNode chartAreaNode = doc.SelectSingleNode("//DefaultChartArea");
            //chartAreaNode = doc.SelectSingleNode("//ChartArea");
            //RemoveChildNode(chartAreaNode, "YAxis/Label");
            //RemoveChildNode(chartAreaNode, "XAxis/Label");
            //RemoveChildNode(chartAreaNode, "YAxis/ZeroTick");
            //RemoveChildNode(chartAreaNode, "XAxis/ZeroTick");
            ////RemoveChildNode(chartAreaNode, "YAxis/DefaultTick");+ // Нельзя убирать, здесь сохраняется шрифт осей
            ////RemoveChildNode(chartAreaNode, "XAxis/DefaultTick");+
            //chartAreaNode.SelectSingleNode("XAxis").Attributes["InstanceID"].Value = null;
            //chartAreaNode.SelectSingleNode("YAxis").Attributes["InstanceID"].Value = null;
            //XmlNode node = doc.SelectSingleNode("*/YAxis");
            //node.ParentNode.RemoveChild(node);
            //node = doc.SelectSingleNode("*/XAxis");
            //node.ParentNode.RemoveChild(node);
            //StringBuilder builder = new StringBuilder();
            //XmlWriter writer = XmlWriter.Create(builder);
            //doc.WriteTo(writer);
            //writer.Close();
            #endregion
            //SoapFormatter formatter = new SoapFormatter();
            XmlSerializer serializer = new XmlSerializer(typeof(ChartSettingsWrapper));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream gzs = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);
            ChartSettingsWrapper my = new ChartSettingsWrapper(this);
            //formatter.Serialize(gzs, my);
            serializer.Serialize(gzs, my);
            gzs.Close();
            ms.Close();
            string r = Convert.ToBase64String(ms.ToArray());
            return r;
        }
        

        ///// <summary>
        ///// Удалить дочерний узел.
        ///// </summary>
        ///// <param name="chartAreaNode">Узел, от которого ищется дочерний.</param>
        ///// <param name="xpath">Путь для поиска.</param>
        //private static void RemoveChildNode(XmlNode chartAreaNode, string xpath)
        //{
        //    XmlNode node = chartAreaNode.SelectSingleNode(xpath);
        //    node.ParentNode.RemoveChild(node);
        //}

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            ProcessSelectionArea(e);
            isAllowMouseClickProcess = true;
        }
    }
}
