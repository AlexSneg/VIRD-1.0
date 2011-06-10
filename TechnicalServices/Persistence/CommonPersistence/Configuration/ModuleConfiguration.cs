using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.SystemPersistence.Configuration
{
    [Serializable, XmlRoot(Namespace = "urn:configuration-schema", ElementName = "Configuration")]
    public class ModuleConfiguration : ICollectionItemValidation
    {
        private string _name;
        private string _comment;

        private readonly List<DeviceType> _deviceList = new List<DeviceType>();
        private readonly List<DisplayType> _displayList = new List<DisplayType>();
        private readonly List<SourceType> _sourceList = new List<SourceType>();
        private List<Label> _labelList = new List<Label>();

        //private readonly Dictionary<string, ResourceInfo> _resourceInfoDic = new Dictionary<string, ResourceInfo>();

        ///// <summary>
        ///// инфо необходимое для записи дополнительной информации о свойсвах ресурсов для програмных источников
        ///// создается при загрузке конфигурации, не сохраняется при десериализации, так как предоставляется плагинами
        ///// </summary>
        //[XmlIgnore]
        //public Dictionary<string, ResourceInfo> ResourceInfoDic
        //{
        //    get { return _resourceInfoDic; }
        //}

        [XmlAttribute("Name")]
        [DisplayName("Наименование конфигурации")]
        [TypeConverter(typeof(NameTypeConverter))]
        public string Name
        {
            get { return _name; } 
            set
            {
                if (!String.IsNullOrEmpty(value) && value.Length > 50)
                    throw new ApplicationException("Наименование конфигурации превышает длинну 50 символов");
                _name = value;
            }
        }

        #region Поддержка свойства Name
        class NameTypeConverter : StringConverter
        {
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                var ret = (string)base.ConvertFrom(context, culture, value);
                if (ret.Trim().Length < 1) throw new FormatException("Пустое значение недопустимо");
                return ret.Trim();
            }
        }
        #endregion


        [XmlAttribute("Comment")]
        [DisplayName("Комментарий")]
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (!String.IsNullOrEmpty(value) && value.Length > 500)
                    throw new ApplicationException("Комментарий превышает длинну 500 символов");
                _comment = value;
            }
        }

        [XmlAttribute("LastChangeDate")]
        [DisplayName("Дата модификации конфигурации")]
        [ReadOnly(true)]
        public DateTime LastChangeDate { get; set; }

        /// <summary>
        /// Коллекция устройств
        /// </summary>
        [XmlArray("DeviceList")]
        [Browsable(false)]
        public List<DeviceType> DeviceList
        {
            get { return _deviceList; }
        }

        /// <summary>
        /// Коллекция источников
        /// </summary>
        [XmlArray("SourceList")]
        [Browsable(false)]
        public List<SourceType> SourceList
        {
            get { return _sourceList; }
        }

        /// <summary>
        /// Коллекция дисплеев
        /// </summary>
        [XmlArray("DisplayList")]
        [Browsable(false)]
        public List<DisplayType> DisplayList
        {
            get { return _displayList; }
        }

        /// <summary>
        /// Коллекция доступных меток для слайдов
        /// </summary>
        [XmlArray("LabelList")]
        [DisplayName("Системные метки")]
        //[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        [EditorAttribute("TechnicalServices.Common.Editor.LabelCollectionEditor, TechnicalServices.Common", typeof(UITypeEditor))]
        [CollectionFormName("Системные метки")]
        [CollectionNameAttribute("Системные метки")]
        public List<Label> LabelList
        {
            get { return _labelList; }
            set { _labelList = value; }
        }

        /// <summary>
        /// Инициализация ссылок. 
        /// Выполняется после десериализации конфигурации.
        /// </summary>
        public void InitReference()
        {
            foreach (DisplayType display in _displayList)
            {
                foreach (Mapping value in display.MappingList)
                {
                    foreach (SourceType item in _sourceList)
                    {
                        if (item.Name != value.SourceName) continue;
                        value.Source = item;
                        break;
                    }
                    if (value.Source == null) throw new KeyNotFoundException("Не найден source для display");
                }
            }

            foreach (SourceType sourceType in _sourceList)
            {
                if (sourceType.UID == -1) continue;
                sourceType.DeviceType = _deviceList.FirstOrDefault(dev => dev.UID == sourceType.UID);
            }

            //// TODO переделать получение ResourceInfo через IModule
            //// загрузка инфы о ресурсах
            //foreach (SourceType sourceType in _sourceList)
            //{
            //    //ResourceInfoAttribute attr = (ResourceInfoAttribute)Attribute.GetCustomAttribute(sourceType.GetType(), typeof(ResourceInfoAttribute));
            //    ResourceInfo info = sourceType.CreateNewResourceInfo();
            //    if (info != null)
            //    {
            //        if (!ResourceInfoDic.ContainsKey(sourceType.Type))
            //            ResourceInfoDic.Add(sourceType.Type, info);

            //    }
            //}
        }

        #region ICollectionItemValidation Members

        public bool ValidateItem(out string errorMessage)
        {
            errorMessage = "";
            foreach (IGrouping<string, Label> grouping in LabelList.GroupBy(l=>l.Name))
            {
                if (grouping.Count() > 1)
                {
                    errorMessage = String.Format("Обнаружены метки с одинаковым именем:\"{0}\"", grouping.Key);
                    return false;
                }
            }
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Класс содержащий доступные метки для слайдов
    /// </summary>
    [Serializable]
    public class Label : IEquatable<Label>, ICloneable
    {
        public Label()
        {
            _name = "label";
        }
        /// <summary>
        /// Уникальный идентификатор метки
        /// </summary>
        
        //[Browsable(false)]
        [XmlAttribute("Id")]
        [ReadOnly(true)]
        public int Id { get; set; }

        private string _name;
        /// <summary>
        /// Имя метки, должно быть уникальным
        /// </summary>
        [XmlAttribute("Name")]
        [DisplayName("Название")]
        public string Name
        {
            get { return _name; }
            set
            {
                value = ValidationHelper.CheckIsNullOrEmpty(value, "Название");
                value = ValidationHelper.CheckLength(value, 50, "Название");
                _name = value;
            }
        }

        /// <summary>
        /// Признак системной метки.
        /// Если метка системная, то это означает, 
        /// что она прошита в контроллере и ее 
        /// нельзя удалить из "Администратора"
        /// </summary>
        [XmlAttribute("IsSystem")]
        [DisplayName("Признак")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.LabelIsSystemConverter, TechnicalServices.Common")]
        [ReadOnly(true)]
        public bool IsSystem { get; set; }

        /// <summary>
        /// значение, соответсвующее отсутсвие ссылки на метку
        /// </summary>
        [XmlIgnore]
        public static int NullId { get { return -1; } }

        public bool Equals(Label other)
        {
            if (other == null)
                return false;
            return this.Id.Equals(other.Id);
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }

    [DebuggerDisplay("UID = {UID}, Type = {Type}, Name = {Name}")]
    [Serializable]
    public abstract class EquipmentType : IEquatable<EquipmentType>, IComparable<EquipmentType>, IComparable, ICustomTypeDescriptor
    {
        /// <summary>
        /// Наименование устройства (источник, оборудование, дисплей)
        /// Обязательное уникальное (для устройств одного типа) значение
        /// По умолчанию: [название плагина]_[порядковый номер]
        /// </summary>
        [Category("Общие параметры")]
        [DisplayName("Название")]
        [XmlAttribute("Name")]
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                value = ValidationHelper.CheckLength(value, 128, "Названия");
                _name = ValidationHelper.CheckIsNullOrEmpty(value, "Название");

            }
        }
        private string _name;
        /// <summary>
        /// Наименование плагина
        /// Обязательное значение
        /// </summary>
        [Browsable(false)]
        [Category("Общие параметры")]
        [DisplayName("Плагин")]
        [XmlAttribute("Type")]
        public string Type { get; set; }

        /// <summary>
        /// Тип устройства
        /// Обязательное значение
        /// По умолчанию: первое значение из списка
        /// </summary>
        [Category("Общие параметры")]
        [DisplayName("Тип оборудования")]
        [XmlAttribute("Model")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.ModelConverter, TechnicalServices.Common")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual string Model { get; set; }

        /// <summary>
        /// Комментарий (max 500 char)
        /// </summary>
        [Category("Общие параметры")]
        [DisplayName("Комментарий")]
        [XmlAttribute("Comment")]
        public virtual string Comment
        {
            get { return _comment; }
            set
            {
                _comment = ValidationHelper.CheckLength(value, 250, "Комментария");
            }
        }
        private string _comment;

        /// <summary>
        /// Аппаратный идентификатор для оборудования
        /// Используется для отправки команд контроллеру
        /// </summary>
        [Category("Общие параметры")]
        [DisplayName("ID оборудования")]
        [XmlAttribute("UID")]
        public virtual int UID
        {
            get { return _uid; }
            set { _uid = ValidationHelper.CheckRange(value, -1, ushort.MaxValue, "ID оборудования"); }
        }

        private int _uid;

        /// <summary>
        /// Список команд поддерживаемых устройством.
        /// Команды отправляются на контроллер.
        /// </summary>
        [Browsable(false)]
        [Category("Общие параметры")]
        [DisplayName("Список команд")]
        [XmlArray("CommandList")]
        public List<EquipmentCommand> CommandList
        {
            get { return _commandList; }
        }
        private readonly List<EquipmentCommand> _commandList = new List<EquipmentCommand>();

        /// <summary>
        /// признак хардварного устройства
        /// </summary>
        public abstract bool IsHardware { get; }

        public bool Equals(EquipmentType other)
        {
            return UID == other.UID
                && Type.Equals(other.Type, StringComparison.InvariantCultureIgnoreCase)
                && Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return String.Intern(string.Format("{0}{1}{2}", UID, Type, Name)).GetHashCode();
        }

        public override string ToString()
        {
            return String.Empty;
        }

        #region IComparable<EquipmentType> Members

        public int CompareTo(EquipmentType other)
        {
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is EquipmentType)
                return CompareTo(obj as EquipmentType);
            return this.GetHashCode().CompareTo(obj.GetHashCode());
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

        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection propColl = attributes == null
                                                        ? TypeDescriptor.GetProperties(this, true)
                                                        : TypeDescriptor.GetProperties(this, attributes, true);
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
            {
                if (propertyDescriptor.Name == "Model")
                    newColl.Add(new ModelPropertyDescriptor(propertyDescriptor));
                else
                    newColl.Add(propertyDescriptor);
            }
            return new PropertyDescriptorCollection(newColl.ToArray(), true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }

    internal sealed class ModelPropertyDescriptor : PropertyDescriptor
    {
        private readonly Dictionary<Type, Preset[]> _list = new Dictionary<Type, Preset[]>();
        private readonly PropertyDescriptor _descr;

        internal ModelPropertyDescriptor(PropertyDescriptor descr)
            : base(descr)
        {
            _descr = descr;
        }

        public override bool CanResetValue(object component)
        {
            return _descr.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get { return _descr.ComponentType; }
        }

        public override object GetValue(object component)
        {
            return _descr.GetValue(component);
        }

        public override bool IsReadOnly
        {
            get { return _descr.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return _descr.PropertyType; }
        }

        public override void ResetValue(object component)
        {
            _descr.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            _descr.SetValue(component, value);
            
            SourceType sourceType = component as SourceType;
            if (sourceType == null) return;
            DeviceType deviceType = sourceType.DeviceType;
            if (deviceType == null) return;

            if (String.IsNullOrEmpty((string)value)) return;

            string model = value.ToString();
            Type type = deviceType.GetType();
            if (!_list.ContainsKey(type)) _list.Add(type, Preset.GetPresetting(type.Assembly));
            Preset preset = _list[type].FirstOrDefault(i => i.Name == model);
            if (preset == null) return;
            if (preset.PropertyList.Count == 0) return;

            PropertyInfo[] propertiesDevice = type.GetProperties();
            foreach (NameValuePair pair in preset.PropertyList)
            {
                PropertyInfo pInfo = propertiesDevice.FirstOrDefault(p => p.Name == pair.Name);
                if (pInfo == null) continue;
                if (!pInfo.CanWrite || pInfo.PropertyType.IsClass) continue;
                if (pInfo.PropertyType.IsEnum)
                {
                    //FieldInfo fi = pInfo.PropertyType.GetField(Enum.GetName(pInfo.PropertyType, pair.Value));
                    object val = Enum.Parse(pInfo.PropertyType, pair.Value);
                    if (val != null) pInfo.SetValue(deviceType, val, new object[] { });
                }
                else if (pInfo.PropertyType.IsValueType)
                {
                    object val = Convert.ChangeType(pair.Value, pInfo.PropertyType);
                    pInfo.SetValue(deviceType, val, new object[] { });
                }
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return _descr.ShouldSerializeValue(component);
        }
    }

    [Serializable]
    public class EquipmentCommand
    {
        /// <summary>
        /// Имя команды
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Непосредственно команда или набор команд 
        /// разделенных разделителем, отправляемых на контроллер
        /// </summary>
        [XmlAttribute("Command")]
        public string Command { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("Answer")]
        [DefaultValue("OK")]
        public string Answer { get; set; }
    }

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class DeviceType : EquipmentType
    {
        public abstract Device CreateNewDevice();

        public Device CreateNewDevice(Dictionary<String, IList<DeviceResourceDescriptor>> resources)
        {
            IList<DeviceResourceDescriptor> descriptors = null;
            Device device = CreateNewDevice();
            if (resources.TryGetValue(device.Type.Type, out descriptors))
            {
                device.DeviceResourceDescriptor = descriptors.FirstOrDefault(x => x.ResourceInfo.DeviceType.Name == device.Type.Name);
            }
            return device;
        }

        [Browsable(false)]
        [XmlIgnore]
        public virtual bool Visible { get { return true; } }

        public ResourceInfo CreateNewResourceInfo(ModuleConfiguration config)
        {
            ResourceInfo resourceInfo = CreateNewResourceInfoProtected();
            if (resourceInfo == null) return null;
            resourceInfo.Name = Name;
            resourceInfo.Type = Type;
            resourceInfo.Comment = Comment;
            resourceInfo.IsHardware = IsHardware;

            if (config != null)
            {
                resourceInfo.DeviceType = this;
                resourceInfo.SourceType = config.SourceList.FirstOrDefault(source => source.IsHardware && source.UID == this.UID);
            }
            return resourceInfo;
        }

        protected virtual ResourceInfo CreateNewResourceInfoProtected()
        {
            return null;
        }

    }

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class DeviceTypeAsSource : DeviceType
    {
        [Browsable(false)]
        [XmlAttribute("Name")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        [Browsable(false)]
        [XmlAttribute("Model")]
        public override string Model
        {
            get { return base.Model; }
            set { base.Model = value; }
        }

        [Browsable(false)]
        [XmlAttribute("Comment")]
        public override string Comment
        {
            get { return base.Comment; }
            set { base.Comment = value; }
        }

        [Browsable(false)]
        [XmlAttribute("UID")]
        public override int UID
        {
            get { return base.UID; }
            set { base.UID = value; }
        }
    }

    [Serializable]
    public abstract class SourceType : EquipmentType
    {
        public ResourceInfo CreateNewResourceInfo(ModuleConfiguration config)
        {
            ResourceInfo resourceInfo = CreateNewResourceInfoProtected();
            resourceInfo.Name = Name;
            resourceInfo.Type = Type;
            resourceInfo.Comment = Comment;
            resourceInfo.IsHardware = IsHardware;

            if (config != null)
            {
                resourceInfo.SourceType = this;
                resourceInfo.DeviceType = config.DeviceList.FirstOrDefault(device => device.IsHardware && device.UID == this.UID);
            }
            return resourceInfo;
        }

        public ResourceInfo CreateNewResourceInfo()
        {
            return CreateNewResourceInfo(null);
        }

        public ResourceInfo CreateNewResourceInfoNumbered(int number)
        {
            ResourceInfo resourceInfo = CreateNewResourceInfo();
            resourceInfo.Name = string.Format("{0}_{1}",
                                              resourceInfo.Name, number);
            return resourceInfo;
        }

        protected virtual ResourceInfo CreateNewResourceInfoProtected()
        {
            return new ResourceInfo();
        }

        public abstract Source CreateNewSource(Slide slide, ModuleConfiguration moduleConfiguration, Dictionary<String, IList<DeviceResourceDescriptor>> resources, Display display);

        [XmlIgnore]
        [Browsable(false)]
        public virtual bool IsSupportPreview
        {
            get { return false; }
        }

        /// <summary>
        /// Используется в конфигураторе
        /// </summary>
        [XmlIgnore]
        [DisplayName("Управление")]
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// Используется только в Конфигураторе, при создании источника
        /// </summary>
        /// <returns></returns>
        public abstract DeviceType CreateDeviceType();
    }

    [Serializable]
    public abstract class HardwareSourceType : SourceType
    {
        /// <summary>
        /// Вход коммутатора (только для аппаратного источника)
        /// Обязательное уникальное (кроме 0) значение
        /// По умолчанию: 0 (источник не подключен к коммутатору)
        /// </summary>
        [Category("Общие параметры")]
        [DisplayName("Вход коммутатора")]
        [XmlAttribute("Input")]
        public int Input
        {
            get { return _input; }
            set { _input = ValidationHelper.CheckRange(value, 0, 100, "Вход коммутатора"); }
        }

        private int _input;
    }

    [Serializable]
    public abstract class DisplayType : EquipmentType
    {
        private readonly List<Mapping> _mappingList = new List<Mapping>();

        /// <summary>
        /// Ширина экрана (в пикселях) (1..4096)
        /// Обязательный параметр
        /// По умолчанию: 1024
        /// </summary>
        [Browsable(false)]
        //[DefaultValue(1024)]
        [XmlAttribute("Width")]
        public int Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }

        /// <summary>
        /// Высота экрана (в пикселях) (1..4096)
        /// Обязательный параметр
        /// По умолчанию: 768
        /// </summary>
        [Browsable(false)]
        //[DefaultValue(768)]
        [XmlAttribute("Height")]
        public int Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }

        /// <summary>
        /// Разрешение экрана (в пикселях)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Разрешение (px)")]
        [XmlIgnore]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        virtual public Size<int> Size
        {
            get { return _size; }
            set 
            { 
                _size = value; 
            }
        }
        private Size<int> _size = new Size<int>(800, 4096, 600, 4096);

        [XmlArray("MappingList")]
        [Browsable(false)]
        public List<Mapping> MappingList
        {
            get { return _mappingList; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public virtual bool SupportsCaptureScreen
        {
            get { return false; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public virtual bool SupportsMultiWindow
        {
            get { return true; }
        }

        public abstract Display CreateNewDisplay();
        public abstract Mapping CreateMapping(SourceType source);
    }

    [Serializable]
    public abstract class PassiveDisplayType : DisplayType
    {
        /// <summary>
        /// Выход коммутатора (только для пассивного дисплея)
        /// Обязательное уникальное (кроме 0) значение
        /// По умолчанию: 0 (дисплей не подключен к коммутатору)
        /// </summary>
        [Category("Общие параметры")]
        [DisplayName("Выход коммутатора")]
        [XmlAttribute("Output")]
        public int Output { get; set; }
    }

    [Serializable]
    [XmlType("Mapping")]
    public class Mapping
    {
        private readonly List<string> _commandList = new List<string>();
        private SourceType _source;

        /// <summary>
        /// Имя источника.
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// !!!     ИСПОЛЬЗУЕТСЯ ТОЛЬКО ДЛЯ СЕРИАЛИЗАЦИИ В XML              !!! 
        /// !!! Увижу использование, кроме оговоренных случаев, ЯЙЦА ОТОРВУ !!! 
        /// !!! Имя атрибута соответствует имени свойства, которое          !!!
        /// !!! необходимо использовать                                     !!!
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        [XmlAttribute("Source")]
        public string SourceName { get; set; }

        /// <summary>
        /// Источник который можно расместить на данном дисплее
        /// </summary>
        [XmlIgnore]
        public SourceType Source
        {
            get { return _source; }
            set
            {
                if (value == _source) return;
                _source = value;
                SourceName = _source.Name;
            }
        }

        [XmlArray("CommandList"), XmlArrayItem("Command")]
        //[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        public List<string> CommandList
        {
            get { return _commandList; }
        }
    }

    [Serializable]
    public class Size<T>
    {
        [TypeConverter(typeof (RangeCheckConverter))]
        public T X { get; set; }
        [TypeConverter(typeof(RangeCheckConverter))]
        public T Y { get; set; }

        /// <summary>
        /// Признак необходимости производить проверку на диапазон.
        /// </summary>
        public bool Range = false;
        public T MinX;
        public T MinY;
        public T MaxX;
        public T MaxY;
        
        /// <summary>
        /// Создать объект с ограничением на величины.
        /// </summary>
        /// <param name="minX">Минимальное значение компонента.</param>
        /// <param name="maxX">Максимальное значение компонента.</param>
        public Size(T min, T max)
        {
            Range = true;
            MinX = min;
            MaxX = max;
            MinY = min;
            MaxY = max;
        }
        
        /// <summary>
        /// Создать объект с ограничением на величины.
        /// </summary>

        /// <param name="min">Минимальное значение компонента X.</param>
        /// <param name="max">Максимальное значение компонента X.</param>
        /// <param name="minY">Минимальное значение компонента Y.</param>
        /// <param name="maxY">Максимальное значение компонента Y.</param>
        public Size(T minX, T maxX, T minY, T maxY)
        {
            Range = true;
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public Size(){ }

        public override string ToString()
        {
            return String.Format("{0}; {1}", X, Y);
        }
    }
    /// <summary>
    /// Ковертер для проверки диапазонов элементов Size<>.
    /// </summary>
    class RangeCheckConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (context.PropertyDescriptor.PropertyType == typeof(int))
            {
                Size<int> obj=((Size<int>)context.Instance);
                int result = int.Parse(value.ToString());
                if (obj.Range) // Нужна проверка
                {
                    if (context.PropertyDescriptor.Name == "X")
                    {
                        if (result < obj.MinX) throw new ArgumentException(string.Format("Значение не может быть меньше {0}", obj.MinX));
                        if (result > obj.MaxX) throw new ArgumentException(string.Format("Значение не может быть больше {0}", obj.MaxX));
                    }
                    else
                    {
                        if (result < obj.MinY) throw new ArgumentException(string.Format("Значение не может быть меньше {0}", obj.MinY));
                        if (result > obj.MaxY) throw new ArgumentException(string.Format("Значение не может быть больше {0}", obj.MaxY));
                    }
                }

                RefreshGrid(context);

                return result;
            }
            else
                if (context.PropertyDescriptor.PropertyType == typeof(decimal))
                {
                    Size<decimal> obj = ((Size<decimal>)context.Instance);
                    decimal result = decimal.Parse(value.ToString());
                    if (context.PropertyDescriptor.Name == "X")
                    {
                        if (result < obj.MinX) throw new ArgumentException(string.Format("Значение не может быть меньше {0}", obj.MinX));
                        if (result > obj.MaxX) throw new ArgumentException(string.Format("Значение не может быть больше {0}", obj.MaxX));
                    }
                    else
                    {
                        if (result < obj.MinY) throw new ArgumentException(string.Format("Значение не может быть меньше {0}", obj.MinY));
                        if (result > obj.MaxY) throw new ArgumentException(string.Format("Значение не может быть больше {0}", obj.MaxY));
                    }
                    RefreshGrid(context);
                    return result;
                }
                else return null;
        }

        /// <summary>
        /// Обновить PropertyGrid.
        /// </summary>
        private static void RefreshGrid(ITypeDescriptorContext context)
        {
            object gi = (((System.Windows.Forms.GridItem)context).Parent);
            System.Reflection.PropertyInfo pi = gi.GetType().GetProperty("OwnerGrid");
            object pg = pi.GetValue(gi, new object[0]);
            System.Windows.Forms.PropertyGrid grid = pg as System.Windows.Forms.PropertyGrid;

            if (grid != null)
            {
                grid.Refresh();
            }
        }
    }

}