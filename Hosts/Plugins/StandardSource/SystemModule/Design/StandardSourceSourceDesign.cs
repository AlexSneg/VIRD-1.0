using System;
using System.ComponentModel;
using System.Xml.Serialization;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using System.Drawing;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.StandardSource.SystemModule.Design
{
    [Serializable]
    [XmlType("StandardSourceSource")]
    public class StandardSourceSourceDesign : HardwareSource, IAspectLock//, IDesignInteractionSupport
    {        
        string strDefWndSize = String.Empty;

        [XmlIgnore]
        [Browsable(false)]
        public override Device Device
        {
            get { return base.Device; }
            set { base.Device = value; }
        }
       
        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {
                base.ResourceDescriptor = value;
                if (null == value) return;
                StandardSourceResourceInfo info = value.ResourceInfo as StandardSourceResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа StandardSourceResourceInfo");
            }
        }


        /*#region IDesignInteractionSupport Members

        public void UpdateServiceReference(IDesignServiceProvider provider)
        {
            IConfiguration conf = provider.GetService(typeof(IConfiguration)) as IConfiguration;
            if (conf != null)
            {
                strDefWndSize = conf.LoadSystemParameters().DefaultWndsize;
                string[] sizes=strDefWndSize.Split('*');
                int.TryParse(sizes[0], out _defaultWidth);
                int.TryParse(sizes[1], out _defaultHeight);
            }
        }

        public void InteractiveAction()
        {
            //nop
        }

        public bool SupportInteraction
        {
            get { return true; }
        }

        #endregion*/


        //#region ISourceSize Members
        //private int _defaultWidth = 800;
        //private int _defaultHeight = 600;

        //[XmlIgnore]
        //[Browsable(false)]
        //public int Width
        //{
        //    get { return _defaultWidth; }
        //}

        //[XmlIgnore]
        //[Browsable(false)]
        //public int Height
        //{
        //    get { return _defaultHeight; }
        //}

        /// <summary>
        /// Признак блокирования отношения ширины изображение к высоте
        /// Обязательный параметр
        /// По умолчанию: True
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Aspect Lock")]
        [DefaultValue(true)]
        [TypeConverter(typeof(YesNoConverter))]
        [XmlAttribute("AspectLock")]
        public bool AspectLock
        {
            get { return _aspectLock; }
            set { _aspectLock = value; }
        }
        private bool _aspectLock = true;

        //void ISourceSize.SetSize(System.Drawing.Size newSize)
        //{
        //}

        //#endregion
    }
}