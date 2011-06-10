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
    /// Обертка для ChartArea с отфильтрованными свойствами.
    /// </summary>
    [TypeConverter(typeof(ExpandableNameConverter))]
    internal class AreaWrapper
    {
        ChartArea area;
        ChartArea defaultArea;
        public AreaWrapper(ChartArea area, ChartArea defaultArea)
        {
            this.area = area;
            this.defaultArea = defaultArea;
        }

        public string Title
        {
            get
            {
                return area.Title;
            }
            set
            {
                area.Title = value;
            }
        }

        [TypeConverter(typeof(ExpandableNameConverter))]
        public Box TitleBox
        {
            get
            {
                return area.TitleBox;
            }
            set
            {
                area.TitleBox = value;
            }
        }



        [TypeConverter(typeof(ExpandableNameConverter))]
        public Background Background
        {
            get
            {
                return area.Background;
            }
            set
            {
                area.Background = value;
            }
        }

        [TypeConverter(typeof(ExpandableNameConverter))]
        public dotnetCHARTING.WinForms.Label Label
        {
            get
            {
                return area.Label;
            }
            set
            {
                area.Label = value;
            }
        }

        [TypeConverter(typeof(ExpandableNameConverter))]
        public LegendBox LegendBox
        {
            get
            {
                return area.LegendBox;
            }
            set
            {
                area.LegendBox = value;
            }
        }

        [TypeConverter(typeof(ExpandableNameConverter))]
        public Shadow Shadow
        {
            get
            {
                return area.Shadow;
            }
            set
            {
                area.Shadow = value;
            }
        }

        [TypeConverter(typeof(ExpandableNameConverter))]
        public Axis XAxis
        {
            get
            {
                return area.XAxis;
            }
            set
            {
                area.XAxis = value;
            }
        }

        [TypeConverter(typeof(ExpandableNameConverter))]
        public Axis YAxis
        {
            get
            {
                return area.YAxis;
            }
            set
            {
                area.YAxis = value;
            }
        }


        #region Corner
        [Description("Bottom left corner."), DefaultValue(BoxCorner.Square)]
        [Category("Settings")]
        public BoxCorner CornerBottomLeft
        {
            get
            {
                return defaultArea.CornerBottomLeft;
            }
            set
            {
                defaultArea.CornerBottomLeft = value;
            }
        }
        [DefaultValue(BoxCorner.Square), Description("Bottom right corner.")]
        [Category("Settings")]
        public BoxCorner CornerBottomRight
        {
            get
            {
                return defaultArea.CornerBottomRight;
            }
            set
            {
                defaultArea.CornerBottomRight = value;
            }
        }
        [Description(""), DefaultValue(8)]
        [Category("Settings")]
        public int CornerSize
        {
            get
            {
                return defaultArea.CornerSize;
            }
            set
            {
                defaultArea.CornerSize = value;
            }
        }
        [DefaultValue(BoxCorner.Square), Description("Top left corner.")]
        [Category("Settings")]
        public BoxCorner CornerTopLeft
        {
            get
            {
                return defaultArea.CornerTopLeft;
            }
            set
            {
                defaultArea.CornerTopLeft = value;
            }
        }
        [Description("Top right corner."), DefaultValue(BoxCorner.Square)]
        [Category("Settings")]
        public BoxCorner CornerTopRight
        {
            get
            {
                return defaultArea.CornerTopRight;
            }
            set
            {
                defaultArea.CornerTopRight = value;
            }
        }
        [Category("Settings")]
        [Description("Default corner."), DefaultValue(BoxCorner.Square)]
        public BoxCorner DefaultCorner
        {
            get
            {
                return defaultArea.DefaultCorner;
            }
            set
            {
                defaultArea.DefaultCorner = value;
            }
        }
        #endregion
    }
}
