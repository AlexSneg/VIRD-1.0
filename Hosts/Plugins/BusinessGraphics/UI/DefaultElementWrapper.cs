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
    /// Обертка для DefaultElement с отфильтрованными свойствами.
    /// </summary>
    [TypeConverter(typeof(ExpandableNameConverter))]
    internal class DefaultElementWrapper
    {
        Element element;
        CustomChart chart;
        public DefaultElementWrapper(Element element, CustomChart chart)
        {
            this.element = element;
            this.chart = chart;
        }
        
        public int GraphLineWidth
        {
            get
            {
                return chart.DefaultSeries.Line.Width;
            }
            set
            {
                chart.DefaultSeries.Line.Width = value;
            }
        }

        public int Transparency
        {
            get
            {
                return element.Transparency;
            }
            set
            {
                element.Transparency = value;
            }
        }
        public bool ExplodeSlice
        {
            get
            {
                return element.ExplodeSlice;
            }
            set
            {
                element.ExplodeSlice = value;
            }
        }
        public Color HatchColor
        {
            get
            {
                return element.HatchColor;
            }
            set
            {
                element.HatchColor = value;
            }
        }
        public System.Drawing.Drawing2D.HatchStyle HatchStyle
        {
            get
            {
                return element.HatchStyle;
            }
            set
            {
                element.HatchStyle = value;
            }
        }

        public bool ShowValue
        {
            get
            {
                return element.ShowValue;
            }
            set
            {
                element.ShowValue = value;
            }
        }

        [TypeConverter(typeof(ExpandableNameConverter))]
        public SmartLabel SmartLabel
        {
            get
            {
                return element.SmartLabel;
            }
            set
            {
                element.SmartLabel = value;
            }
        }
    }
}
