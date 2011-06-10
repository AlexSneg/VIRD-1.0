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
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Drawing.Design;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Common.Classes;
using System.Drawing.Drawing2D;
using System.Data.Odbc;
using Hosts.Plugins.ArcGISMap.UI.Controls;

namespace Hosts.Plugins.ArcGISMap.SystemModule.Design
{
    [Serializable]
    [XmlType("ArcGISMap")]
    public partial class ArcGISMapSourceDesign : Source, IDesignRenderSupport, ISourceSize, ICollectionItemValidation, ISourceContentSize
    {
        #region fields
        [NonSerialized]
        private IDesignServiceProvider service = null;

        [NonSerialized]
        private MapControl map = null;

        [NonSerialized]
        private Bitmap buffer = null;

        private int _width = 600;
        private int _height = 350;

        #endregion

        #region ctor
        public ArcGISMapSourceDesign()
        {
        }

        #endregion

        #region Work with chart
        void ArcGISMapSourceDesign_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Здесь мы узнаем о том, что какие то свойства экземпляра ресурса изменены, надо переинциализировать Map
            InitializeMap();
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

        internal void InitializeMap()
        {
            RefreshMap();
        }


        public void RefreshMap()
        {
            buffer = new Bitmap(_width, _height);
            if (map != null)
            {
                if (buffer != null)
                    map.DrawToBitmap(buffer, new Rectangle(0, 0, _width, _height));
                if (service != null && service.IsActive())
                    service.InvalidateView();
            }
        }

        [XmlIgnore, Browsable(false)]
        public MapControl Map
        {
            get { return this.map; }
        }

        #endregion

        #region Общие свойства
        const string setupMap = "Настройка карты";
        [Category("Настройки")]
        [DisplayName(setupMap)]
        [Editor(typeof(MapEditor), typeof(UITypeEditor))]
        [XmlIgnore]
        public string SetupMap
        {
            get
            {
                return setupMap;
            }
            set
            {

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
                        ((ArcGISMapResourceInfo)base.ResourceDescriptor.ResourceInfo).PropertyChanged -= new PropertyChangedEventHandler(ArcGISMapSourceDesign_PropertyChanged);
                    }
                }

                ArcGISMapResourceInfo info = value.ResourceInfo as ArcGISMapResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа ArcGISMapResourceInfo");
                base.ResourceDescriptor = value;
            }
        }
        #endregion

        #region IDesignRenderSupport Members

        public void Render(System.Drawing.Graphics gfx, System.Drawing.RectangleF area)
        {
            if (buffer == null) RefreshMap();
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
                if (map == null) return _width;

                return map.Size.Width;
            }
        }

        [Browsable(false)]
        public int Height
        {
            get
            {

                if (map == null) return _height;
                return map.Size.Height;
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
            if (map != null)
                map.Size = newSize;

            _width = newSize.Width;
            _height = newSize.Height;

            RefreshMap();
        }

        #endregion

        #region ICollectionItemValidation Members

        public bool ValidateItem(out string errorMessage)
        {
            errorMessage = "OK";
            //StringBuilder res = new StringBuilder();
            //List<ValueRange> intersect = new List<ValueRange>();
            //for (int i = 0; i < ValueRanges.Count; i++)
            //{
            //    ValueRange currRange = ValueRanges[i];
            //    if (!intersect.Contains(currRange))
            //    {
            //        List<ValueRange> intersectT = ValueRanges.Where(v =>
            //            !v.Equals(currRange) && (
            //            (v.MinValue >= currRange.MinValue && v.MinValue <= currRange.MaxValue) ||
            //            (v.MaxValue >= currRange.MinValue && v.MaxValue <= currRange.MaxValue))).ToList();
            //        if (intersectT.Count > 0)
            //        {
            //            intersect.Add(currRange);
            //            intersect.AddRange(intersectT);
            //            res.AppendFormat("Интервал <{0}> пересакается с ", currRange.ToString());
            //            int n = 0;
            //            intersectT.ForEach(ins => res.Append("<" + ins.ToString() + (++n != intersectT.Count ? ">,<" : ">")));
            //        }
            //    }
            //}
            //if (intersect.Count > 0)
            //{
            //    errorMessage = res.ToString();
            //    return false;
            //}
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
}
