using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

using Hosts.Plugins.VNC.SystemModule.Config;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VNC.SystemModule.Design
{
    /// <summary>
    /// состояние подключения
    /// </summary>
    [Serializable]
    public enum ConnectionStatus
    {
        /// <summary>
        /// подключен
        /// </summary>
        Connected,
        /// <summary>
        /// отключен
        /// </summary>
        Disconnected
    }

    /// <summary>
    /// управление рабочим столом
    /// </summary>
    [Serializable]
    public enum RemoteControl
    {
        /// <summary>
        /// запрещен
        /// </summary>
        [Description("Нет")]
        Disable,
        /// <summary>
        /// разрешен
        /// </summary>
        [Description("Да")]
        Enable
    }

    [Serializable]
    [XmlType("VNC")]
    public class VNCSourceDesign : Source, IAspectLock
    {
        public VNCSourceDesign()
        {
            AspectLock = true;
        }

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
                base.ResourceDescriptor = value;
                if (null == value) return;
                VNCResourceInfo info = value.ResourceInfo as VNCResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа VNCResourceInfo");
            }
        }

        /// <summary>
        /// Сетевой адрес компьютера (IP, порт и т.п.)
        /// Получаем из ResourceInfo
        /// </summary>
        [Category("Настройки")]
        [DisplayName("URI")]
        [XmlIgnore]
        public string Uri
        {
            get { return ((VNCResourceInfo) ResourceDescriptor.ResourceInfo).Uri; }
        }

        ///// <summary>
        ///// Коэффициент сжатия изображения удаленного рабочего стола
        ///// В требованиях отсутствует
        ///// </summary>
        //[TypeConverter(typeof (ServerSideScaleConverter))]
        //[DisplayName("Server Side Scale")]
        //[XmlAttribute("ServerSideScale")]
        //public int ServerSideScale
        //{
        //    get { return _serverSideScale; }
        //    set
        //    {
        //        _serverSideScale = value;
        //        if (!PossibleScaleFactor.Contains(_serverSideScale))
        //            throw new Exception(
        //                string.Format(
        //                    "VNCSourceDesign.ServerSideScale: Значение фактора должно лежать в пределах от {0} до {1}. Вы пытаетесь установить {2}",
        //                    PossibleScaleFactor.Min(), PossibleScaleFactor.Max(), _serverSideScale));
        //    }
        //}

        /// <summary>
        /// Состояние подключения
        /// Обязательный параметр
        /// По умолчению: Connected
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Состояние подключения")]
        [DefaultValue(ConnectionStatus.Connected)]
        [TypeConverter(typeof(CommonEnumConverter))]
        [XmlAttribute("ConnectionStatus")]
        public ConnectionStatus ConnectionStatus { get; set; }

        /// <summary>
        /// Управление рабочим столом
        /// Обязательный параметр
        /// По умолчанию: Disable
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Управление рабочим столом")]
        [DefaultValue(RemoteControl.Disable)]
        [TypeConverter(typeof(CommonEnumConverter))]
        [XmlAttribute("RemoteControl")]
        public RemoteControl RemoteControl { get; set; }

        /// <summary>
        /// Признак блокирования отношения ширины изображение к высоте
        /// Обязательный параметр
        /// По умолчанию: True
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Aspect Lock")]
        [TypeConverter(typeof(YesNoConverter))]
        [DefaultValue(true)]
        [XmlAttribute("AspectLock")]
        public bool AspectLock { get; set; }
    }

    //internal class ServerSideScaleConverter : TypeConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }

    //    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        VNCSourceDesign source = context.Instance as VNCSourceDesign;
    //        if (source == null) return base.GetStandardValues(context);
    //        return new StandardValuesCollection(source.PossibleScaleFactor);
    //    }

    //    public override bool IsValid(ITypeDescriptorContext context, object value)
    //    {
    //        VNCSourceDesign source = context.Instance as VNCSourceDesign;
    //        if (source == null || !(value is int)) return base.IsValid(context, value);
    //        return source.PossibleScaleFactor.Contains((int) value);
    //    }
    //}
}