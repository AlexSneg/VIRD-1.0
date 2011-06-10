using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Runtime.Serialization;
using TechnicalServices.Interfaces;
using System.Drawing;
using dotnetCHARTING.WinForms;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using dotnetCHARTING.Mapping;
using System.Drawing.Design;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Common.Classes;
using System.Drawing.Drawing2D;
using System.Data.Odbc;
using Hosts.Plugins.BusinessGraphics.UI;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design.Helpers;
using Hosts.Plugins.BusinessGraphics.UI.Controls;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    [Serializable]
    [XmlType("BusinessGraphics")]
    public partial class BusinessGraphicsSourceDesign : Source, ICustomTypeDescriptor, IDesignRenderSupport, ISourceSize, IDesignInteractionSupport, ICollectionItemValidation, ISourceContentSize
    {
        #region fields
        const string appData = "+xPejKmu57b6C8DHnguJOGqHs6PLOjs1dT829JZeyLn+5kO29DBxTmgYRCFcaVHFjAiKgTKPzULaOAFBI2kb4xK3sLD6ARYNhQhZPRtjjNs=";

        [NonSerialized]
        private IDesignServiceProvider service = null;

        [NonSerialized]
        private CustomChart chart = null;

        [NonSerialized]
        private Bitmap buffer = null;

        private Dictionary<string, float> seriesDef;

        private int _width = 600;
        private int _height = 350;

        [NonSerialized]
        private SeriesCollection data = null;

        [NonSerialized]
        private string detalizedSeriesName = String.Empty;

        internal bool subDiagramMode = false;

        [NonSerialized]
        private bool _isChartSetupMode = false;
        internal bool isChartSetupMode 
        { 
            get { return _isChartSetupMode; }
            set { _isChartSetupMode = value; if (chart != null) chart.isChartSetupMode = value; }
        }

        [Browsable(false)]
        [XmlAttribute("H")]
        //[XmlIgnore]
        /// <summary>
        /// Высота экрана в метрах.
        /// </summary>
        public double H{get; set;}

        [Browsable(false)]
        [XmlAttribute("B")]
        //[XmlIgnore]
        /// <summary>
        /// Ширина экрана в метрах.
        /// </summary>
        public double B{get; set;}

        [Browsable(false)]
        [XmlAttribute("L")]
        //[XmlIgnore]
        /// <summary>
        /// Расстояние от зрителя до экрана в метрах.
        /// </summary>
        public double L{get; set;}

        [Browsable(false)]
        [XmlAttribute("Bp")]
        //[XmlIgnore]
        /// <summary>
        /// Ширина  экрана в пикселях.
        /// </summary>
        public double Bp{get; set;}

        [Browsable(false)]
        [XmlAttribute("Hp")]
        //[XmlIgnore]
        /// <summary>
        /// Высота экрана в пикселях.
        /// </summary>
        public double Hp{get; set;}

        #endregion

        #region ctor
        public BusinessGraphicsSourceDesign()
        {
        }

        #endregion

        #region Work with chart
        void BusinessGraphicsSourceDesign_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Здесь мы узнаем о том, что какие то свойства экземпляра ресурса изменены, надо переинциализировать Chart
            InitializeChart(true);
        }

        [XmlIgnore]
        [Browsable(false)]
        public bool IsPlayerMode
        {
            get;
            set;
        }

        [XmlIgnore]
        [Browsable(false)]
        public ActiveWindow Wnd
        {
            get;
            set;
        }

        /// <summary>
        /// инициализирует данные, если надо может их очистить и проинициализировать заново
        /// </summary>
        internal void InitializeData(bool clearExistsData)
        {
            if (clearExistsData) data = null;
            if (data == null)    data = getData();
        }

        /// <summary>инициализирует заново чарт, может сбрасывать внешний вид и данные </summary>
        /// <param name="clearState">очистить ли визуальные настройки чарта</param>
        internal void InitializeChart(bool clearState)
        {
            if (!IsPlayerMode)
            {
                if (service == null) return;
                if (!service.IsActive()) return;
            }
            ReInitChart(false, clearState);
            UpdateDiagramType();

            chart.Width = _width;
            chart.Height = _height;

            if (ResourceDescriptor != null && ResourceDescriptor.ResourceInfo != null)
                chart.Title = ResourceDescriptor.ResourceInfo.Name;
            else
                chart.Title = Properties.Resources.NotDefined;

            InitializeData(false);
            chart.SeriesCollection.Add(data); 
            LinkMapData();
            ApplyChartStyle();
            FixChartErgonomic(chart);
            UpdateVisibleSeries();
            RefreshChart();
        }

        /// <summary>
        /// Применить к чарту стиль (или сохраненные ранее настройки).
        /// </summary>
        public void ApplyChartStyle()
        {
            if (this.DiagramStyleData != null)
            {
                //chart.LoadState(this.DiagramStyleData, chart);
                chart.LoadStyle(this.DiagramStyleData);
            }
            else
            {
                if (stylesInfo != null)
                {
                    stylesInfo.LoadStyle(this.DiagramStyle, this.chart);
                }
            }
        }

        /// <summary>
        /// Переинициализировать чарт.
        /// </summary>
        /// <param name="recreate">Удалить старый чарт и создать новый.</param>
        /// <param name="clearState">очистить ли визуальные настройки чарта</param>
        public void ReInitChart(bool recreate, bool clearState)
        {
            if(recreate && (chart != null))
            {
                chart.MouseClick -= ChartClick;
                chart = null;
            }
            //subDiagramMode = false;
            if (chart == null)
            {
                chart = new CustomChart { Size = new Size(_width, _height), Application = appData, NoDataText = Properties.Resources.NoDataLbl };
                chart.MouseClick += ChartClick;
            }
            chart.ReInitChart(ValueRanges, DiagramType, SubDiagramType, AllowUserInteraction, clearState, subDiagramMode);
        }


        private void UpdateVisibleSeries()
        {
            if ((chart != null) && (DiagramType != DiagramTypeEnum.Speedometer) && (DiagramType != DiagramTypeEnum.TrafficLight))
            {
                var strs = visibleSeries.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length > 0)
                {
                    foreach (string s in GetSeriesList())
                    {
                        Series series = data.GetSeries(s);
                        if (series != null)
                        {
                            if (strs.Contains(s))
                            {
                                if (!series.Visible)
                                    chart.SeriesCollection.Add(series);
                            }

                            series.Visible = strs.Contains(s);
                        }
                    }
                }
                else
                {
                    foreach(Series series in data)
                    {
                        series.Visible = false;
                    }
                }
                for (int i = 0; i < chart.SeriesCollection.Count; i++)
                {
                    if (data.GetSeries(chart.SeriesCollection[i].Name)!=null) chart.SeriesCollection[i].Visible = data.GetSeries(chart.SeriesCollection[i].Name).Visible;
                }
            }
        }

        /// <summary>
        /// Перерисовать диаграмму.
        /// </summary>
        public void RefreshChart()
        {
            buffer = new Bitmap(_width, _height);
            if (chart != null)
            {
                chart.RefreshChart();
                if (buffer != null)
                    chart.DrawToBitmap(buffer, new Rectangle(0, 0, _width, _height));
                if (service != null && service.IsActive())
                    service.InvalidateView();
            }
        }

        private void ChartClick(object sender, MouseEventArgs e)
        {
            if (!AllowUserInteraction || _isChartSetupMode) return;
            if (AllowSubDiagram)
            {
                if (subDiagramMode)
                {
                    SetNormalMode();
                    chart.isAllowMouseClickProcess = false;
                }
                else
                {
                    switch (DiagramType)
                    {
                        case DiagramTypeEnum.ColumnGeneral:
                        case DiagramTypeEnum.PieGeneral:
                            HitTestInfo hit = chart.HitTest(e.Location);
                            Element el = hit.Object as Element;
                            if (el != null)
                            {
                                detalizedSeriesName = el.Name;
                                SetDetailedMode();
                                chart.isAllowMouseClickProcess = false;
                            }
                            break;
                        case DiagramTypeEnum.Map:
                            detalizedSeriesName = GetShapeName(e.Location);
                            if (seriesDef.ContainsKey(detalizedSeriesName))
                            {
                                SetDetailedMode();
                                chart.isAllowMouseClickProcess = false;
                            }
                            break;
                    }
                }
                chart.RefreshChart();
                if (service != null && service.IsActive())
                    service.InvalidateView();
            }
        }

        internal void SetNormalMode()
        {
            subDiagramMode = false;
            detalizedSeriesName = String.Empty;
            chart.Mapping.MapLayerCollection.Clear();
            chart.SeriesCollection.Clear();

            ReInitChart(false, true);
            UpdateDiagramType();
            InitializeData(true);
            chart.SeriesCollection.Add(data);
            UpdateVisibleSeries();
            LinkMapData();
        }

        internal void SetDetailedMode()
        {
            subDiagramMode = true;
            chart.Mapping.MapLayerCollection.Clear();
            chart.SeriesCollection.Clear();
            
            ReInitChart(false, true);
            UpdateDiagramType();
            InitializeData(true);
            chart.SeriesCollection.Add(data);
        }

        private void UpdateDiagramType()
        {
            chart.SetDiagramType(subDiagramMode);
            if (!subDiagramMode)
            {
                if (DiagramType == DiagramTypeEnum.Map)
                {
                    string mapFile = GetFile(FileType.mapResource);
                    if (!string.IsNullOrEmpty(mapFile))
                        InitializeMap(mapFile);
                    else
                        MessageBox.Show("SHP файл карты не найден или не указан\r\n" + mapFile, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (chart != null && this.DiagramStyleData != null)
                {
                    this.chart.LoadStyle(this.DiagramStyleData);
                }
                //else // Загрузка стиля вообщем-то не нужна, данные всегда есть после настройки
                //if (stylesInfo != null && chart != null && !String.IsNullOrEmpty(this.DiagramStyle))
                //    stylesInfo.LoadStyle(this.DiagramStyle, this.chart);
            }
            else
            {
                if (chart != null && this.SubDiagramStyleData != null)
                {
                    this.chart.LoadStyle(this.SubDiagramStyleData);
                }
                //else
                //if (stylesInfo != null && chart != null && !String.IsNullOrEmpty(this.SubDiagramStyle))
                //    stylesInfo.LoadStyle(this.SubDiagramStyle, this.chart);
            }
        }
        
        private SeriesCollection getData()
        {
            SeriesCollection SC = new SeriesCollection();
            if (this.ResourceDescriptor != null && this.ResourceDescriptor.ResourceInfo != null)
            {
                Dictionary<string, float> pointDef;
                Dictionary<Intersection, float> intersections;
                XmlTextReader xmlReader = null;
                BusinessGraphicsResourceInfo info = ((BusinessGraphicsResourceInfo)this.ResourceDescriptor.ResourceInfo);

                switch (info.ProviderType)
                {
                    case ProviderTypeEnum.XML:
                        {
                            string fName = GetFile(FileType.xmlResource);
                            if (!string.IsNullOrEmpty(fName))
                                xmlReader = new XmlTextReader(fName);
                            else
                                xmlReader = ODBCProvider.GetEmptyXml();
                            break;
                        }

                    case ProviderTypeEnum.ODBC:
                        {
                            bool isSuccess = ODBCProvider.ReadXml(info.ODBC, info.ODBCProcedure, out xmlReader);
                            break;
                        }
                }

                DataChartXmlReader.ReadDataChartFromXml(xmlReader, out seriesDef, out pointDef, 
                    out intersections, ref defaultSeries);

                info.SetAmount(seriesDef.Count, pointDef.Count);

                SC.Add(FillData(seriesDef, pointDef, intersections));
            }
            return SC;
        }

        internal List<string> GetSeriesList()
        {
            return seriesDef.Keys.ToList();
        }

        private void UpdateInteractiveActions()
        {
            if (chart != null)
                chart.AllowUserInteraction = AllowUserInteraction;
        }

        [XmlIgnore, Browsable(false)]
        public CustomChart Chart
        {
            get { return this.chart; }
        }

        #endregion

        #region Общие свойства
        /// <summary>
        /// Тип источника (XML/ODBC)
        /// Получаем из ResourceInfo
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Тип источника")]
        [XmlIgnore, ReadOnly(true)]
        public ProviderTypeEnum ProviderType
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null) return ProviderTypeEnum.XML;
                return ((BusinessGraphicsResourceInfo)ResourceDescriptor.ResourceInfo).ProviderType;
            }
        }

        int refreshInterval = 0;

        /// <summary>
        /// Период между обновлениями данных из ODBC источника в секундах (0..1000)
        /// Обязательный параметр
        /// По умолчанию: 0 (обновление не выполняется)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Время обновления данных")]
        [DefaultValue(0)]
        [XmlAttribute("ODBCRefreshInterval")]
        [ProviderTypeRequired(ProviderTypeEnum.ODBC)]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt32Converter, TechnicalServices.Common")]
        public int ODBCRefreshInterval
        {
            get { return refreshInterval; }
            set
            {
                if ((value <= 1000) && (value >= 0))
                    refreshInterval = value;
                else
                    throw new ArgumentException("Значение интервала обновления должно быть от 0 до 1000");
            }
        }

        const string setupChart = "Настройка диаграммы";
        [Category("Настройки")]
        [DisplayName(setupChart)]
        [Editor(typeof(ChartEditor), typeof(UITypeEditor))]
        [XmlIgnore]
        public string SetupChart
        {
            get
            {
                return setupChart;
            }
            set
            {
                
            }
        }

        List<ValueRange> valueRange;
        [Category("Настройки")]
        [DisplayName("Диапазон значений")]
        [XmlIgnore]
        [Editor(typeof(RangeCollectionEditor), typeof(UITypeEditor))]
        public List<ValueRange> ValueRanges
        {
            get
            {
                if (valueRange == null)
                {
                    valueRange = new List<ValueRange> { 
                        new ValueRange { MinValue = 0, MaxValue = 30, Color = Color.Green },
                        new ValueRange { MinValue = 31, MaxValue = 60, Color = Color.Yellow },
                        new ValueRange { MinValue = 61, MaxValue = 100, Color = Color.Red}
                    };
                }

                return valueRange;
            }
            set { valueRange = value; }
        }

        [Browsable(false)]
        [XmlAttribute("ValueRangesString")]
        public string ValueRangesString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int n = 0;
                ValueRanges.ForEach(v => sb.AppendFormat("{0} {1} {2}{3}", v.MinValue, v.MaxValue, ColorTranslator.ToHtml(v.Color), (++n != ValueRanges.Count ? "|" : String.Empty)));
                return sb.ToString();

            }
            set
            {
                valueRange = new List<ValueRange>();
                string v = value;
                foreach (string s in v.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] p = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        float min = float.Parse(p[0]);
                        float max = float.Parse(p[1]);
                        Color c = ColorTranslator.FromHtml(p[2]);

                        ValueRange result = new ValueRange { Color = c, MinValue = min, MaxValue = max };
                        valueRange.Add(result);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }


        string defaultSeries = String.Empty;

        [Category("Настройки")]
        [DisplayName("Серия по умолчанию")]
        [XmlAttribute("DefaultSeries")]
        [TypeConverter(typeof(SeriesListConverter))]
        public string DefaultSeries
        {
            get
            {
                return defaultSeries;
            }
            set
            {
                defaultSeries = value;
                InitializeData(true);
                InitializeChart(true);
            }
        }

        /// <summary>
        /// Тип диаграммы
        /// Обязательный параметр
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Тип диаграммы")]
        [XmlAttribute("DiagramType")]
        [TypeConverter(typeof(DiagramTypeEnumConverter))]
        [RefreshProperties(RefreshProperties.All)]
        public DiagramTypeEnum DiagramType
        {
            get { return _diagramType; }
            set
            {
                _diagramType = value;
                AllowSubDiagram = false;
                if(chart!=null) chart.ClearStyle();
                DiagramStyle = "По умолчанию";
                DiagramStyleData = null;
                FixChartErgonomic(chart);
                InitializeData(true);
                InitializeChart(true);
            }
        }
        private DiagramTypeEnum _diagramType;

        private string _diagramStyle;

        /// <summary>
        /// Стиль отображения диаграммы
        /// Стили хранят все настройки вида диаграммы в особом хранилище
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Стиль диаграммы")]
        [XmlAttribute("DiagramStyle")]
        [TypeConverter(typeof(BusinessGraphicsStyleConverter))]
        [Browsable(false)]
        public string DiagramStyle
        {
            get { return _diagramStyle; }
            set { _diagramStyle = value;
                InitializeChart(true); }
        }

        /// <summary>
        /// Данные стиля диаграммы.
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("DiagramStyleData")]
        public string DiagramStyleData
        {
            get { return _diagramStyleData; }
            set { _diagramStyleData = value; }
        }
        private string _diagramStyleData;

        /// <summary>
        /// Данные стиля детализации.
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("SubDiagramStyleData")]
        public string SubDiagramStyleData
        {
            get { return _subDiagramStyleData; }
            set { _subDiagramStyleData = value; }
        }
        private string _subDiagramStyleData;

        /// <summary>
        /// Детализация диаграммы (On/Off)
        /// Доступно при DiagramType in («Географическая карта», «Пирог», «Столбики»)
        /// По умолчанию: false
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Детализация")]
        [DefaultValue(false)]
        [XmlAttribute("AllowSubDiagram")]
        [TypeConverter(typeof(OnOffConverter))]
        [RefreshProperties(RefreshProperties.All)]
        public bool AllowSubDiagram { get; set; }

        private bool _allowUserInteraction = true;

        /// <summary>
        /// Интерактивные действия пользователя на диаграмме (On/Off)
        /// По умолчанию: true
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Интерактивные действия")]
        [DefaultValue(true)]
        [XmlAttribute("AllowUserInteraction")]
        [TypeConverter(typeof(OnOffConverter))]
        public bool AllowUserInteraction
        {
            get { return _allowUserInteraction; }
            set
            {
                _allowUserInteraction = value;
                UpdateInteractiveActions();
            }
        }



        /// <summary>
        /// Тип детализированной диаграммы
        /// Доступно при AllowSubDiagram = true
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Тип детализированной диаграммы")]
        [XmlAttribute("SubDiagramType")]
        [TypeConverter(typeof(CommonEnumConverter))]
        public SubDiagramTypeEnum SubDiagramType 
        { 
            get 
            {
                return _subDiagramType;
            } 
            set 
            {
                _subDiagramType = value;
                SubDiagramStyle = "По умолчанию";
                this.SubDiagramStyleData = null;
            } 
        }
        private SubDiagramTypeEnum _subDiagramType;


        private string _subDiagramStyle;
        /// <summary>
        /// Стиль отображения детализированной диаграммы
        /// Стили хранят все настройки вида диаграммы в особом хранилище
        /// Доступно при AllowSubDiagram = true
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Стиль детализированной диаграммы")]
        [XmlAttribute("SubDiagramStyle")]
        [TypeConverter(typeof(BusinessGraphicsStyleConverter))]
        [Browsable(false)]
        public string SubDiagramStyle
        {
            get { return _subDiagramStyle; }
            set { _subDiagramStyle = value; }
        }

        string visibleSeries = String.Empty;
        [Category("Настройки")]
        [DisplayName("Видимость серий")]
        [Editor(typeof(SeriesEditor), typeof(UITypeEditor))]
        [XmlAttribute("VisibleSeries")]
        public string VisibleSeries
        {
            get { return visibleSeries; }
            set
            {
                visibleSeries = value;
                InitializeChart(true);
            }
        }

        private bool _autoSetMode = false;

        [Category("Настройки")]
        [DisplayName("Режим автонастройки")]
        [TypeConverter(typeof(YesNoConverter))]
        [XmlAttribute("AutoSetMode")]
        public bool AutoSetMode
        {
            get { return _autoSetMode; }
            set 
            { 
                _autoSetMode = value;
                if (_autoSetMode)
                {
                    FixChartErgonomic(chart);
                    if(chart!=null)this.DiagramStyleData = chart.SaveStyle();
                    InitializeChart(true);
                }
            }
        }

        #endregion

        #region overrides

        [Browsable(false)]
        [XmlIgnore]
        public override string Model
        {
            get { return base.Model; }
        }

        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {

                if (service != null)
                {
                    //отписываемся от нотификаций предыдущего дескриптора
                    if (base.ResourceDescriptor != null)
                    {
                        ((BusinessGraphicsResourceInfo)base.ResourceDescriptor.ResourceInfo).PropertyChanged -= new PropertyChangedEventHandler(BusinessGraphicsSourceDesign_PropertyChanged);
                    }
                }

                BusinessGraphicsResourceInfo info = value.ResourceInfo as BusinessGraphicsResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа BusinessGraphicsResourceInfo");
                base.ResourceDescriptor = value;

                UpdateInfo();
            }
        }
        #endregion

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection propColl;
            if (attributes == null)
                propColl = TypeDescriptor.GetProperties(this, true);
            else
                propColl = TypeDescriptor.GetProperties(this, attributes, true);
            // в зависимости от типов провайдера и диаграммы доступны различные свойства
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
            {
                ProviderTypeRequiredAttribute attr = (ProviderTypeRequiredAttribute)propertyDescriptor.Attributes[typeof(ProviderTypeRequiredAttribute)];
                if ((attr != null) && (attr.ProviderType != ProviderType))
                    continue;

                if (propertyDescriptor.Name == "ValueRanges" && DiagramType != DiagramTypeEnum.TrafficLight &&
                    DiagramType != DiagramTypeEnum.Map && DiagramType != DiagramTypeEnum.Speedometer /*&& DiagramType != DiagramTypeEnum.ColumnDetail*/)
                    continue;

                if (propertyDescriptor.Name == "DefaultSeries" && DiagramType != DiagramTypeEnum.TrafficLight &&
                     DiagramType != DiagramTypeEnum.Speedometer && DiagramType != DiagramTypeEnum.PieDetail /*&& DiagramType != DiagramTypeEnum.ColumnDetail*/)
                    continue;

                if (propertyDescriptor.Name == "VisibleSeries" &&
                    (DiagramType == DiagramTypeEnum.TrafficLight ||
                     DiagramType == DiagramTypeEnum.Speedometer ||
                     DiagramType == DiagramTypeEnum.PieDetail ||
                     DiagramType == DiagramTypeEnum.Map)
                    )
                    continue;

                if ((propertyDescriptor.Name == "AllowSubDiagram") &&
                    (DiagramType != DiagramTypeEnum.ColumnGeneral) &&
                    (DiagramType != DiagramTypeEnum.PieGeneral) &&
                    (DiagramType != DiagramTypeEnum.Map))
                    continue;

                if (((propertyDescriptor.Name == "SubDiagramType") ||
                    (propertyDescriptor.Name == "SubDiagramStyle") ||
                    (propertyDescriptor.Name == "SubDiagramControl")) &&
                    (!AllowSubDiagram))
                    continue;

                newColl.Add(propertyDescriptor);
            }
            propColl = new PropertyDescriptorCollection(newColl.ToArray(), true);
            return propColl;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

        #region IDesignRenderSupport Members

        public void Render(System.Drawing.Graphics gfx, System.Drawing.RectangleF area)
        {
            if (buffer == null) RefreshChart();
            if (buffer != null)
                gfx.DrawImage(buffer, area);
        }

        public void UpdateReference(IServiceProvider provider)
        {
        }

        #endregion

        #region ISourceSize Members
        [Browsable(false)]
        public int Width
        {
            get
            {
                if (chart == null) return _width;

                return chart.Size.Width;
            }
        }

        [Browsable(false)]
        public int Height
        {
            get
            {

                if (chart == null) return _height;
                return chart.Size.Height;
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public bool AspectLock
        {
            get
            {
                return false;
            }
            set
            {
                //nop
            }
        }

        public void SetSize(Size newSize)
        {
            if (chart != null)
                chart.Size = newSize;

            _width = newSize.Width;
            _height = newSize.Height;

            RefreshChart();
        }

        #endregion

        #region Work with styles

        [Browsable(false)]
        [XmlIgnore]
        public static List<string> DiagramStyleList = new List<string> { "По умолчанию" };

        private static void ClearStyleList()
        {
            DiagramStyleList = new List<string> { "По умолчанию" };
        }

        [NonSerialized]
        [XmlIgnore]
        internal IClientResourceCRUD<ResourceDescriptor> resourceCRUD = null;

        [NonSerialized]
        [XmlIgnore]
        internal StyleResourceInfo stylesInfo = null;

        public void LoadStyles()
        {
            if (service != null)
            {
                IResourceProvider resourceProvider = (IResourceProvider)GetService(typeof(IResourceProvider));
                if (resourceProvider == null) return;

                var r = resourceProvider.GetResourcesByType(StyleResourceInfo.GetResourceType(), false).FirstOrDefault();
                if (r != null && r.ResourceInfo != null)
                {
                    //DiagramStyleList.Clear();
                    ClearStyleList();
                    stylesInfo = r.ResourceInfo as StyleResourceInfo;
                    DiagramStyleList.AddRange(stylesInfo.GetStyleNames());
                }
            }
        }
        #endregion

        #region Ergonomics
        /// <summary>
        /// Исправить размер шрифта, если он не соответсвует требованиям эргономичности.
        /// </summary>
        /// <param name="font">Проверяемый шрифт.</param>
        /// <returns>Исправленный шритф.</returns>
        public Font FixFontSize(Font font)
        {
            if (CheckFontSizeIncorrect(font))
            {
                double sizeNeeded = Math.Sin(0.333 * Math.PI / 180) * L * Hp / H;
                Font sampleFont = new Font(font.FontFamily, 100f, font.Style); // Шрифт 100pt для оценки необходимого размера
                double sampleSize = sampleFont.GetHeight((float)(this.Hp / (this.H*(100/2.54)))); // Узнаем размер шрифта в пикселях
                return (new Font(font.FontFamily, (float)Math.Round((sampleFont.Size * (sizeNeeded / sampleSize))+0.5, 0, MidpointRounding.AwayFromZero), font.Style));
            }
            else
            {
                return font;
            }
        }

        /// <summary>
        /// Проверить, надо ли исправить размер шрифта.
        /// </summary>
        /// <param name="font">Шрифт.</param>
        /// <returns>Признак некорректности размера.</returns>
        public bool CheckFontSizeIncorrect(Font font)
        {
            double fontSize = font.GetHeight((float)(this.Hp / (this.H * (100 / 2.54))));/*changedFont.Size*/ ; // Узнаем размер шрифта в пикселях
            return !(Math.Asin((fontSize * this.H) / (this.L * this.Hp)) * 180 / Math.PI > 0.333);
        }

        /// <summary>
        /// Исправить размер шрифта, если он не соответсвует требованиям эргономичности.
        /// </summary>
        /// <param name="font">Проверяемый шрифт.</param>
        /// <returns>Исправленный шритф.</returns>
        public void FixLineWidth(Line line)
        {
            if (CheckLineWidthIncorrect(line.Width))
            {
                double widthH = Math.Sin(0.033 * Math.PI / 180) * L * Hp / H;
                double widthB = Math.Sin(0.033 * Math.PI / 180) * L * Bp / B;
                line.Width = Convert.ToInt32(Math.Ceiling(Math.Max(widthB, widthH)));
            }
            
        }

        /// <summary>
        /// Проверить, надо ли исправить толщину линий.
        /// </summary>
        /// <param name="width">Толщина линии.</param>
        /// <returns>Признак некорректности толщины.</returns>
        public bool CheckLineWidthIncorrect(double width)
        {
            return 
                  Math.Min(Math.Asin((width * this.H) / (this.L * this.Hp)) * 180 / Math.PI, 
                           Math.Asin((width * this.B) / (this.L * this.Bp)) * 180 / Math.PI) < 0.033;
        }


        /// <summary>
        /// Исправить частоту координатной сетки.
        /// </summary>
        /// <param name="font">Проверяемый шрифт.</param>
        /// <returns>Исправленный шритф.</returns>
        public void FixGridInterval(Axis axis)
        {
            if (CheckGridIntervalIncorrect(axis.Interval))
            {
                double intervalH = Math.Sin(3.5 * Math.PI / 180) * L * Hp / H;
                double intervalB = Math.Sin(3.5 * Math.PI / 180) * L * Bp / B;
                axis.Interval = Convert.ToInt32(Math.Ceiling(Math.Max(intervalH, intervalB)));
            }

        }

        /// <summary>
        /// Проверить, надо ли исправить толщину линий.
        /// </summary>
        /// <param name="width">Толщина линии.</param>
        /// <returns>Признак некорректности толщины.</returns>
        public bool CheckGridIntervalIncorrect(double interval)
        {
            return
                  Math.Min(Math.Asin((interval * this.H) / (this.L * this.Hp)) * 180 / Math.PI,
                           Math.Asin((interval * this.B) / (this.L * this.Bp)) * 180 / Math.PI) < 3.5;
        }


        /// <summary>
        /// Исправляет диаграмму в соответсвии с требованиями эргономичности.
        /// </summary>
        /// <param name="chart"></param>
        public void FixChartErgonomic(CustomChart chart)
        {
            if (chart == null) return;
            if (!this.AutoSetMode) return;
            //TODO: Вместо того, чтобы каждый раз считать, лучше один раз вычислить минимальные значения
            // Шрифты
            chart.ChartArea.Label.Font = FixFontSize(chart.ChartArea.Label.Font);
            chart.ChartArea.LegendBox.HeaderLabel.Font = FixFontSize(chart.ChartArea.LegendBox.HeaderLabel.Font);
            chart.ChartArea.LegendBox.LabelStyle.Font = FixFontSize(chart.ChartArea.LegendBox.LabelStyle.Font);
            chart.ChartArea.TitleBox.Label.Font = FixFontSize(chart.ChartArea.TitleBox.Label.Font);
            chart.ChartArea.XAxis.Label.Font = FixFontSize(chart.ChartArea.XAxis.Label.Font);
            chart.ChartArea.YAxis.Label.Font = FixFontSize(chart.ChartArea.YAxis.Label.Font);
            chart.ChartArea.XAxis.ZeroTick.Label.Font = FixFontSize(chart.ChartArea.XAxis.ZeroTick.Label.Font);
            chart.ChartArea.YAxis.ZeroTick.Label.Font = FixFontSize(chart.ChartArea.YAxis.ZeroTick.Label.Font);
            chart.ChartArea.XAxis.DefaultTick.Label.Font = FixFontSize(chart.ChartArea.XAxis.DefaultTick.Label.Font);
            chart.ChartArea.YAxis.DefaultTick.Label.Font = FixFontSize(chart.ChartArea.YAxis.DefaultTick.Label.Font);
            chart.DefaultElement.SmartLabel.Font = FixFontSize(chart.DefaultElement.SmartLabel.Font);
            chart.ChartArea.LegendBox.HeaderEntry.LabelStyle.Font = FixFontSize(chart.ChartArea.LegendBox.HeaderEntry.LabelStyle.Font);
            
            // Толщина линий
            FixLineWidth(chart.ChartArea.XAxis.Line);
            FixLineWidth(chart.ChartArea.YAxis.Line);
            FixLineWidth(chart.ChartArea.XAxis.DefaultTick.Line);
            FixLineWidth(chart.ChartArea.YAxis.DefaultTick.Line);
            FixLineWidth(chart.DefaultSeries.Line);

            // Интевал
            FixGridInterval(chart.ChartArea.XAxis);
            FixGridInterval(chart.ChartArea.YAxis);
        }
        #endregion

        #region IDesignInteractionSupport Members

        [NonSerialized]
        [XmlIgnore]
        IPresentationNotifier notifier;

        public void UpdateServiceReference(IDesignServiceProvider provider)
        {
            service = provider;

            resourceCRUD = service.GetService(typeof(IClientResourceCRUD<ResourceDescriptor>)) as IClientResourceCRUD<ResourceDescriptor>;
            notifier = (IPresentationNotifier)service.GetService(typeof(IPresentationNotifier));

            UpdateInfo();
            LoadStyles();

            if (notifier != null)
            {
                notifier.OnResourceAdded += notifier_OnResourceUpdatedOrAdded;
                notifier.OnResourceUpdated += notifier_OnResourceUpdatedOrAdded;
            }
        }

        void notifier_OnResourceUpdatedOrAdded(object sender, NotifierEventArg<ResourceDescriptor> e)
        {
            if (e.Data.ResourceInfo != null && e.Data.ResourceInfo.Type == StyleResourceInfo.GetResourceType())
            {
                this.stylesInfo = e.Data.ResourceInfo as StyleResourceInfo;
            }
        }

        void UpdateInfo()
        {
            //visibleSeries = "";
            InitializeChart(true);

            if (service != null)
            {
                if (base.ResourceDescriptor != null)
                {
                    ((BusinessGraphicsResourceInfo)this.ResourceDescriptor.ResourceInfo).PropertyChanged += new PropertyChangedEventHandler(BusinessGraphicsSourceDesign_PropertyChanged);
                }
            }
        }

        public void InteractiveAction()
        {
            ChartForm frm = new ChartForm();
            //frm.TopMost = true;
            frm.Text = this.ResourceDescriptor.ResourceInfo.Name;
            AddChartToContainer(frm, Wnd);
            frm.ShowDialog();
            chart.ClearInteractiveInfo(); // Очистим координаты кликов
            RemoveChartFromContainer(frm);
            SetNormalMode();
            InitializeChart(true);
        }

        public void AddChartToContainer(ContainerControl container, ActiveWindow wnd)
        {
            if (this.chart != null)
            {
                this.chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));

                this.chart.Left = 0;
                this.chart.Top = 0;
                this.chart.Width = container.ClientSize.Width;
                this.chart.Height = container.ClientSize.Height;

                container.InitBorderTitle(wnd, this.chart);

                container.Controls.Add(this.chart);
                this.chart.RefreshChart();
            }
        }

        public void RemoveChartFromContainer(ContainerControl container)
        {
            if (this.chart != null)
                container.Controls.Remove(this.chart);
        }

        [Browsable(false)]
        public bool SupportInteraction
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region ICollectionItemValidation Members

        public bool ValidateItem(out string errorMessage)
        {
            errorMessage = "OK";
            StringBuilder res = new StringBuilder();
            List<ValueRange> intersect = new List<ValueRange>();
            for (int i = 0; i < ValueRanges.Count; i++)
            {
                ValueRange currRange = ValueRanges[i];
                if (!intersect.Contains(currRange))
                {
                    List<ValueRange> intersectT = ValueRanges.Where(v =>
                        !v.Equals(currRange) && (
                        (v.MinValue >= currRange.MinValue && v.MinValue <= currRange.MaxValue) ||
                        (v.MaxValue >= currRange.MinValue && v.MaxValue <= currRange.MaxValue))).ToList();
                    if (intersectT.Count > 0)
                    {
                        intersect.Add(currRange);
                        intersect.AddRange(intersectT);
                        res.AppendFormat("Интервал <{0}> пересакается с ", currRange.ToString());
                        int n = 0;
                        intersectT.ForEach(ins => res.Append("<" + ins.ToString() + (++n != intersectT.Count ? ">,<" : ">")));
                    }
                }
            }
            if (intersect.Count > 0)
            {
                errorMessage = res.ToString();
                return false;
            }
            return true;
        }

        #endregion

        #region ISourceContentSize Members
        void ISourceContentSize.SetContentSize(Size newSize)
        {
            this.SetSize(newSize);
        }

        #endregion
    }
    

    #region Enums
    public enum DiagramTypeEnum
    {
        [Description("Столбики обобщенные")]
        ColumnGeneral,
        [Description("Столбики детализированные")]
        ColumnDetail,
        [Description("Географическая карта")]
        Map,
        [Description("Пирог обобщенный")]
        PieGeneral,
        [Description("Пирог детализированный")]
        PieDetail,
        [Description("График")]
        Graph,
        [Description("Спидометр")]
        Speedometer,
        [Description("Светофор")]
        TrafficLight
    }

    public enum SubDiagramTypeEnum
    {
        [Description("Столбики детализированные")]
        ColumnDetail,
        [Description("Пирог детализированный")]
        PieDetail,
        [Description("График")]
        Graph
    }

    internal enum FileType
    {
        xmlResource,
        mapResource,
        mapdataResource
    }

    #endregion

    #region Style attribute
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class ChartStyleAttribute : Attribute
    {
        readonly string name;

        public ChartStyleAttribute(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
    }
    #endregion

    #region helpers for xml table
    class Intersection : IEquatable<Intersection>
    {
        public string SeriesName { get; set; }
        public string PointName { get; set; }

        public Intersection(string SeriesName, string PointName)
        {
            this.SeriesName = SeriesName;
            this.PointName = PointName;
        }

        public override bool Equals(object obj)
        {
            if (obj is Intersection)
                return this.Equals((Intersection)obj);
            return false;
        }

        public static bool operator ==(Intersection obj1, Intersection obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Intersection obj1, Intersection obj2)
        {
            return !obj1.Equals(obj2);
        }

        public override int GetHashCode()
        {
            return SeriesName.GetHashCode() + PointName.GetHashCode();
        }

        public bool Equals(Intersection other)
        {
            return other.PointName == PointName && other.SeriesName == SeriesName;
        }
    }
    #endregion

    #region work with ranges
    [Serializable]
    public class ValueRange : ICloneable, ICollectionItemValidation, IEquatable<ValueRange>
    {
        [Category("Общие")]
        [DisplayName("Минимальное значение")]
        public float MinValue { get; set; }

        [Category("Общие")]
        [DisplayName("Максимальное значение")]
        public float MaxValue { get; set; }

        [Category("Общие")]
        [DisplayName("Окраска")]
        public Color Color { get; set; }

        public override string ToString()
        {
            return String.Format("{0},{1}", MinValue, MaxValue);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool ValidateItem(out string errorMessage)
        {
            errorMessage = string.Empty;
            if (MinValue >= MaxValue)
            {
                errorMessage = "Максимальное значение должно быть больше минимального";
                return false;
            }
            if (Color == Color.Empty)
            {
                errorMessage = "Задайте цвет диапазона";
                return false;
            }
            return true;
        }

        public bool Equals(ValueRange other)
        {
            if (other == null) return false;
            if ((this.MinValue == other.MinValue) &&
                (this.MaxValue == other.MaxValue) &&
                (this.Color == other.Color))
                return true;
            return false;
        }
    }
    #endregion

}
