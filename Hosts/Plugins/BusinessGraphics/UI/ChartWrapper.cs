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

namespace Hosts.Plugins.BusinessGraphics.UI
{
    /// <summary>
    /// Обертка для диаграммы с отфильтрованными свойствами.
    /// </summary>
    internal class ChartWrapper : ICustomTypeDescriptor
    {
        CustomChart chart;
        [Category("Settings")]
        public SeriesType SeriesType
        {
            get
            {
                return (SeriesType)chart.DefaultSeries.Type;
            }
            set
            {
                chart.DefaultSeries.Type = value;
            }
        }
        [Category("Settings")]
        public GaugeType GaugeType
        {
            get
            {
                return chart.DefaultSeries.GaugeType;
            }
            set
            {
                chart.DefaultSeries.GaugeType = value;
            }
        }

        [Category("Settings")]
        [DisplayName("ChartArea")]
        public AreaWrapper ChartArea1
        {
            get
            {
                return new AreaWrapper(chart.ChartArea, chart.DefaultChartArea);
            }
            set
            {
            }
        }

        [Category("Settings")]
        [DisplayName("DefaultElement")]
        public DefaultElementWrapper DefaultElement
        {
            get
            {
                return new DefaultElementWrapper(chart.ChartArea.DefaultElement, chart);
            }
            set
            {
            }
        }


        public ChartWrapper(CustomChart chart)
        {
            this.chart = chart;
        }

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(chart);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(chart);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(chart);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(chart);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(chart);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(chart);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(chart, editorBaseType);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(chart, attributes);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(chart);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var list = ChartProperyProvider.GetProperties(attributes);
            var pd1 = TypeDescriptor.GetProperties(typeof(ChartWrapper))["SeriesType"];
            var pd2 = TypeDescriptor.GetProperties(typeof(ChartWrapper))["GaugeType"];
            var pd3 = TypeDescriptor.GetProperties(typeof(ChartWrapper))["ChartArea1"];
            var pd4 = TypeDescriptor.GetProperties(typeof(ChartWrapper))["DefaultElement"];
            list.Add(pd1);
            list.Add(pd2);
            list.Add(pd3);
            list.Add(pd4);
            return list;
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return ChartProperyProvider.GetProperties();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            if (pd != null &&
                (pd.Name == "GaugeType"
                || pd.Name == "SeriesType"
                || pd.Name == "ChartArea1"
                || pd.Name == "DefaultElement"
                )) return this;
            return chart;
        }
        #endregion
    }
}
