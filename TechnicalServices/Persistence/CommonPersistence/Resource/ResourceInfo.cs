using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TechnicalServices.Exceptions;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Persistence.SystemPersistence.Resource
{
    [Serializable]
    [DataContract]
    public class ResourceInfo : IEquatable<ResourceInfo>, ICustomTypeDescriptor
    {
        public ResourceInfo()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public static string GenerateNewGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        [Browsable(false)]
        [XmlIgnore]
        [DataMember]
        public SourceType SourceType { get; set; }

        [DataMember]
        private string _sourceName;

        [XmlAttribute("SourceName")]
        [Browsable(false)]
        public string SourceName
        {
            get { return SourceType == null ? _sourceName : SourceType.Name; }
            set { _sourceName = value; }
        }

        [Browsable(false)]
        [Category("Общие параметры")]
        [DisplayName("Управление")]
        [TypeConverter(typeof(ExpandableReadOnlyConverter))]
        [XmlIgnore]
        [DataMember]
        public virtual DeviceType DeviceType { get; set; }

        [DataMember]
        private string _deviceName;

        [XmlAttribute("DeviceName")]
        [Browsable(false)]
        public string DeviceName
        {
            get { return DeviceType == null ? _deviceName : DeviceType.Name; }
            set { _deviceName = value; }
        }

        [Category("Общие параметры")]
        [DisplayName("Название")]
        [XmlAttribute("Name")]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                string _value = ValidationHelper.CheckLength(value, 128, "названия");
                if (String.IsNullOrEmpty(_value))
                    throw new InvalidParameterException("Название не может быть пустым");
                _name = _value;
            }
        }
        [DataMember]
        protected string _name = string.Empty;

        [DataMember]
        [Browsable(false)]
        [Category("Общие параметры")]
        [DisplayName("Плагин")]
        [XmlAttribute("Type")]
        [ReadOnly(true)]
        public virtual string Type { get; set; }

        [Category("Общие параметры")]
        [DisplayName("Комментарий")]
        [XmlAttribute("Comment")]
        public virtual string Comment
        {
            get { return _comment; }
            set { _comment = ValidationHelper.CheckLength(value, 250, "комментария"); }
        }
        [DataMember]
        private string _comment = string.Empty;

        /// <summary>
        /// уникальное имя ресурса сохраняется один раз для ресурса и таким образом является уникальным
        /// </summary>
        [DataMember]
        [Browsable(false)]
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [DataMember]
        [Browsable(false)]
        [XmlAttribute("IsHardware")]
        public bool IsHardware { get; set; }

        public bool Equals(ResourceInfo other)
        {
            return Type.Equals(other.Type, StringComparison.InvariantCultureIgnoreCase) &&
                   Id.Equals(other.Id, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool NameEquals(ResourceInfo other)
        {
            return Type.Equals(other.Type, StringComparison.InvariantCultureIgnoreCase) &&
                   Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public void Init(ModuleConfiguration config)
        {
            SourceType = config.SourceList.FirstOrDefault(value => value.Name.Equals(_sourceName));
            DeviceType = config.DeviceList.FirstOrDefault(value => value.Name.Equals(_deviceName));
        }

        #region Implementation of ICustomTypeDescriptor
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
            PropertyDescriptorCollection propColl;
            if (attributes == null)
                propColl = TypeDescriptor.GetProperties(this, true);
            else
                propColl = TypeDescriptor.GetProperties(this, attributes, true);
            // DSidorov - у аппаратных источников м.б. редактируемые свойства
            // Например, список абонентов в терминале ВКС
            //if (IsHardware)
            //{
            //    List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            //    foreach (PropertyDescriptor propertyDescriptor in propColl)
            //    {
            //        newColl.Add(TypeDescriptor.CreateProperty(GetType(),
            //            propertyDescriptor, new Attribute[] { ReadOnlyAttribute.Yes }));
            //    }
            //    propColl = new PropertyDescriptorCollection(newColl.ToArray(), true);
            //}
            return propColl;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }

    [DataContract]
    [Serializable]
    public class ResourceInfoForHardwareSource : ResourceInfo
    {
        [ReadOnly(true)]
        [XmlAttribute("Name")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        [DataMember]
        [Browsable(true)]
        [XmlAttribute("Type")]
        [ReadOnly(true)]
        public override string Type
        {
            get { return base.Type; }
            set { base.Type = value; }
        }

        [ReadOnly(true)]
        [XmlAttribute("Comment")]
        public override string Comment
        {
            get { return base.Comment; }
            set { base.Comment = value; }
        }

        [Category("Общие параметры")]
        [DisplayName("Тип оборудования")]
        [XmlIgnore]
        public string Model
        {
            get { return SourceType != null ? SourceType.Model : null; }
        }

        [Category("Общие параметры")]
        [DisplayName("Вход коммутатора")]
        [XmlIgnore]
        public int Input
        {
            get { return (SourceType != null) && (SourceType is HardwareSourceType) ? ((HardwareSourceType)SourceType).Input : 0; }
        }

        [Browsable(true)]
        [XmlIgnore]
        public override DeviceType DeviceType
        {
            get { return base.DeviceType; }
            set { base.DeviceType = value; }
        }

        [DataMember]
        private RGBParam _rgbParam = new RGBParam();

        [Browsable(true)]
        [XmlElement("RGBParam")]
        [DisplayName("Параметры RGB")]
        public RGBParam RGBParam
        {
            get { return _rgbParam; }
            set { _rgbParam = value; }
        }

    }

    [DataContract]
    [Serializable]
    public class ResourceFileInfo : ResourceInfo
    {
        private PropertyInfo[] _resourceFileProperties = null;
        //        private XmlSerializableDictionary<string, ResourceFileProperty> _resourceFileDictionary =
        //new XmlSerializableDictionary<string, ResourceFileProperty>();

        [DataMember]
        private List<ResourceFileProperty> _resourceFileList = new List<ResourceFileProperty>();

        /// <summary>
        /// установка мастерного ресурса с инициализацией!
        /// </summary>
        /// <param name="fileName"></param>
        public void SetMasterResource(string fileName)
        {
            foreach (PropertyInfo propertyInfo in ResourceFileProperties)
            {
                ResourceFileAttribute resourceFileAttribute = (ResourceFileAttribute)propertyInfo.GetCustomAttributes(typeof(ResourceFileAttribute), true)[0];
                if (resourceFileAttribute.Master)
                {
                    propertyInfo.SetValue(this, fileName, null);
                    break;
                }
            }
        }

        /// <summary>
        /// полное имя файла - чисто информативное, использовать програмно нельзя - лажа!
        /// НЕ ИСПОЛЬЗОВАТЬ setter - ЮЗАЕТСЯ ТОЛЬКО КЛИЕНТОМ В PROPERTYGRID
        /// </summary>
        [Editor("UI.Common.CommonUI.Editor.ResourceFileInfoEditor,CommonUI", typeof(UITypeEditor))]  //ResourceFileInfoEditor
        [TypeConverter("TechnicalServices.Common.TypeConverters.ResourceFileInfoFullFileNameTypeConverter, TechnicalServices.Common")]
        [Category("Настройки")]
        [DisplayName("Путь к файлу")]
        [Browsable(false)]
        [XmlIgnore]
        [ResourceFileAttribute("resource", true, true)]
        public string ResourceFullFileName
        {
            get { return GetResourceFileName("resource"); }
            protected set
            {
                value = ValidationHelper.CheckLength(value, 250, "\"Путь к файлу\"");
                SetResourceFileName("resource", value);
            }
        }

        //[DataMember]
        //[XmlElement("ResourceFileDictionary")]
        //[Browsable(false)]
        //public XmlSerializableDictionary<string, ResourceFileProperty> ResourceFileDictionary
        //{
        //    get { return _resourceFileDictionary; }
        //    set { _resourceFileDictionary = value; }
        //}

        [XmlArray("ResourceFileList")]
        [Browsable(false)]
        public List<ResourceFileProperty> ResourceFileList
        {
            get { return _resourceFileList; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public virtual string Filter
        {
            get { /*return _filter;*/  return @"Все файлы|*.*"; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public ResourceFileProperty MasterResourceProperty
        {
            get
            {
                return ResourceFileList.SingleOrDefault(rfp => rfp.Master);
            }
            //ResourceFileDictionary.Select(
            //            kv => kv.Value).SingleOrDefault(rfp => rfp.Master);
            //}
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<PropertyDescriptor> list = new List<PropertyDescriptor>(base.GetProperties(attributes).Cast<PropertyDescriptor>());
            foreach (PropertyInfo propertyInfo in ResourceFileProperties)
            {
                list.Add(TypeDescriptor.CreateProperty(this.GetType(),
                    propertyInfo.Name, typeof(string), attributes));
            }
            return new PropertyDescriptorCollection(list.ToArray(), true);
        }

        protected PropertyInfo[] ResourceFileProperties
        {
            get
            {
                if (_resourceFileProperties == null)
                {
                    List<PropertyInfo> propertyList = new List<PropertyInfo>();
                    PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        ResourceFileAttribute[] attributes =
                            (ResourceFileAttribute[])propertyInfo.GetCustomAttributes(typeof(ResourceFileAttribute), true);
                        if (attributes != null && attributes.Length == 1)
                        {
                            if (!propertyList.Exists(pi => pi.Name.Equals(propertyInfo.Name)))
                                propertyList.Add(propertyInfo);
                        }
                    }
                    _resourceFileProperties = propertyList.ToArray();
                }
                return _resourceFileProperties;
            }
        }

        /// <summary>
        /// устанавливает значение ресурса
        /// </summary>
        /// <param name="id">id ресурса - то самое что задается и через аттрибут ResourceFileAttribute</param>
        /// <param name="val">значение</param>
        /// <param name="ChangeName">менять ли имя ресурса</param>
        protected void SetResourceFileName(string id, string val, bool ChangeName)
        {
            //ResourceFileAttribute resourceFileAttribute = GetAttributeForProperty(true);
            //if (resourceFileAttribute == null) return;
            ResourceFileAttribute resourceFileAttribute = ResourceFileProperties.SelectMany(
                pi => (ResourceFileAttribute[])pi.GetCustomAttributes(typeof(ResourceFileAttribute), true)).FirstOrDefault(
                rfa => rfa.Id.Equals(id));
            if (resourceFileAttribute == null) return;
            ResourceFileProperty resourceFileProperty = new ResourceFileProperty
            {
                Length = new FileInfo(val).Length,
                ModifiedUtc = DateTime.Now.ToFileTimeUtc(),
                Id = resourceFileAttribute.Id,
                Master = resourceFileAttribute.Master,
                Required = resourceFileAttribute.Required,
                ResourceFullFileName = val,
                ResourceFileName = Path.GetFileName(val),
                Newly = true
            };
            //ResourceFileDictionary[resourceFileAttribute.Id] = resourceFileProperty;

            Init(resourceFileProperty, resourceFileAttribute); // Инициализацию проводим раньше, чтобы свойство нельзя было добавить неподдерживаемый ресурс

            int index = ResourceFileList.FindIndex(rfp => rfp.Id.Equals(resourceFileAttribute.Id));
            if (index >= 0) ResourceFileList[index] = resourceFileProperty;
            else ResourceFileList.Add(resourceFileProperty);
            if (resourceFileAttribute.Master && ChangeName)
                Name = Path.GetFileNameWithoutExtension(val);

        }

        /// <summary>
        /// устанавливает значение ресурса
        /// </summary>
        /// <param name="id">id ресурса - то самое что задается и через аттрибут ResourceFileAttribute</param>
        /// <param name="val">значение</param>
        protected void SetResourceFileName(string id, string val)
        {
            SetResourceFileName(id, val, true);
        }

        /// <summary>
        /// возвращает значение ресурса
        /// </summary>
        /// <param name="id">id ресурса - то самое что задается и через аттрибут ResourceFileAttribute</param>
        /// <returns></returns>
        protected string GetResourceFileName(string id)
        {
            //ResourceFileAttribute resourceFileAttribute = GetAttributeForProperty(false);
            //if (resourceFileAttribute == null) Trace.WriteLine(string.Format("resourceFileAttribute == null"));
            //if (resourceFileAttribute == null) return null;
            //ResourceFileProperty resourceFileProperty =
            //    ResourceFileList.Find(rfp => rfp.Id.Equals(resourceFileAttribute.Id));
            ResourceFileProperty resourceFileProperty =
                ResourceFileList.Find(rfp => rfp.Id.Equals(id));

            //foreach (ResourceFileProperty property in ResourceFileList)
            //{
            //    Trace.WriteLine(string.Format("ResourceFileProperty {0}-{1}", property.Id, property.ResourceFullFileName));
            //    if (resourceFileProperty == null) Trace.WriteLine(string.Format("resourceFileProperty == null"));
            //}
            return resourceFileProperty == null ? null : resourceFileProperty.ResourceFullFileName;
            //if (!ResourceFileDictionary.TryGetValue(resourceFileAttribute.Id, out resourceFileProperty)) return null;
            //return resourceFileProperty.ResourceFullFileName;
        }

        protected virtual void Init(ResourceFileProperty property, ResourceFileAttribute resourceFileAttribute)
        { }

        //private ResourceFileAttribute GetAttributeForProperty(bool set)
        //{
        //    StackTrace stackTrace = new StackTrace();
        //    for (int index = 0; index < stackTrace.FrameCount; index++)
        //    {
        //        StackFrame stackFrame = stackTrace.GetFrame(index);
        //        MethodBase method;
        //        if (null != stackFrame && (method = stackFrame.GetMethod()) !=null && !set)
        //        {
        //                Trace.WriteLine(string.Format("TRACE>>>>>> method {0}", method.Name));
        //        }

        //        if (null == stackFrame || (method = stackFrame.GetMethod()) == null ||
        //            method.MemberType != MemberTypes.Method) continue;
        //        PropertyInfo propertyInfo =
        //            ResourceFileProperties.FirstOrDefault(pi => (set ? pi.GetSetMethod(true) : pi.GetGetMethod(true)).Name.StartsWith(method.Name, StringComparison.InvariantCultureIgnoreCase));
        //        if (propertyInfo == null) continue;
        //        if (!set)
        //        {
        //            Trace.WriteLine(string.Format("TRACE_NEXT>>>>>> propertyInfo {0}, method {1}", propertyInfo.GetGetMethod(true).Name, method.Name));
        //        }
        //        ResourceFileAttribute[] attr =
        //            (ResourceFileAttribute[])propertyInfo.GetCustomAttributes(typeof(ResourceFileAttribute), true);
        //        if (!set)
        //        {
        //            Trace.WriteLine(string.Format("TRACE_NEXT_NEXT>>>>>> attr.Length {0}", attr.Length.ToString()));
        //        }
        //        if (attr.Length == 0) continue;
        //        return attr[0];
        //    }
        //    return null;
        //}

    }

    [Serializable]
    [DataContract]
    public class ResourceFileProperty
    {
        [XmlAttribute("Id", DataType = "ID")]
        [DataMember]
        public string Id { get; set; }
        [XmlAttribute("ResourceFileName")]
        [DataMember]
        public string ResourceFileName { get; set; }
        [XmlAttribute("Master")]
        [DataMember]
        public bool Master { get; set; }
        [XmlAttribute("Required")]
        [DataMember]
        public bool Required { get; set; }
        //[XmlIgnore]
        [XmlAttribute("ResourceFullFileName")]
        [DataMember]
        public string ResourceFullFileName { get; set; }
        //{
        //    get { return _resourceFullFileName; }
        //    set { _resourceFullFileName = value; }
        //}
        [XmlAttribute("FileSize")]
        [DataMember]
        public long Length { get; set; }
        /// <summary>
        /// FileTimeUtc
        /// </summary>
        [XmlAttribute("ModifiedUtc")]
        [DataMember]
        public long ModifiedUtc { get; set; }
        [XmlIgnore]
        [DataMember]
        public bool Newly { get; set; }
    }

    public static class ResourceInfoExt
    {
        private static XmlSerializer _serializer = null;
        private static object _sync = new object();
        //public static ResourceInfo GetResourceInfo(this ResourceInfo resourceInfo, Stream resourceInfoStream)
        //{
        //    XmlSerializer serializer = GetSerializer(resourceInfo);
        //    using (XmlReader reader = XmlReader.Create(resourceInfoStream))
        //    {
        //        return (ResourceInfo)serializer.Deserialize(reader);
        //    }
        //}

        public static void SaveToFile(this ResourceInfo resourceInfo, string file, Type[] extraTypes)
        {
            XmlSerializer serializer = GetSerializer(extraTypes);
            using (XmlWriter writer = XmlWriter.Create(file))
            {
                serializer.Serialize(writer, resourceInfo);
            }
        }

        //public static byte[] GetContent(this ResourceInfo resourceInfo)
        //{
        //    XmlSerializer serializer = GetSerializer(resourceInfo);
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        serializer.Serialize(stream, resourceInfo);
        //        return stream.GetBuffer();
        //    }
        //}

        public static ResourceInfo GetResourceInfo(this ResourceInfo resourceInfo, string file, Type[] extraTypes)
        {
            XmlSerializer serializer = GetSerializer(extraTypes);
            using (XmlReader reader = XmlReader.Create(file))
            {
                return (ResourceInfo)serializer.Deserialize(reader);
            }
        }

        public static ResourceInfo LoadFromFile(string file, Type[] extraTypes)
        {
            XmlSerializer serializer = GetSerializer(extraTypes);
            using (XmlReader reader = XmlReader.Create(file))
            {
                return (ResourceInfo)serializer.Deserialize(reader);
            }
        }

        public static ResourceInfo MakeClone(this ResourceInfo resourceInfo, Type[] extraTypes)
        {
            XmlSerializer serializer = GetSerializer(extraTypes);
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, resourceInfo);
                stream.Seek(0, SeekOrigin.Begin);
                return (ResourceInfo)serializer.Deserialize(stream);
            }
        }

        private static XmlSerializer GetSerializer(IEnumerable<Type> extraTypes)
        {
            if (_serializer == null)
            {
                lock (_sync)
                {
                    if (_serializer == null)
                    {
                        HashSet<Type> types = new HashSet<Type>(extraTypes) { typeof(ResourceFileInfo) };
                        _serializer = new XmlSerializer(typeof(ResourceInfo), types.ToArray());
                    }
                }
            }
            return _serializer;
        }

    }

    public class ExpandableReadOnlyConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection propColl = base.GetProperties(context, value, attributes);
            // все свойства только для чтения
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
                newColl.Add(TypeDescriptor.CreateProperty(
                    ((ResourceInfo)context.Instance).DeviceType.GetType(),
                    propertyDescriptor,
                    new Attribute[] { ReadOnlyAttribute.Yes }));
            return new PropertyDescriptorCollection(newColl.ToArray(), true);
        }
    }

    [Serializable]
    [DataContract]
    [TypeConverter("TechnicalServices.Common.TypeConverters.ExpandableNameConverter, TechnicalServices.Common")]
    public class RGBParam : ICustomTypeDescriptor
    {
        #region "Параметры RGB"
        // Группа "Параметры RGB"
        [DataMember]
        private short _vTotal = 768;

        [Category("Параметры RGB")]
        [DisplayName("Total_H")]
        [XmlAttribute("VTotal")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short VTotal
        {
            get { return _vTotal; }
            set { _vTotal = value; }
        }

        [DataMember]
        private short _vOffset = 0;

        [Category("Параметры RGB")]
        [DisplayName("Offset_H")]
        [XmlAttribute("VOffset")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short VOffset
        {
            get { return _vOffset; }
            set { _vOffset = value; }
        }

        [DataMember]
        private short _vHeight = 768;

        [Category("Параметры RGB")]
        [DisplayName("Active_H")]
        [XmlAttribute("VHeight")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        [DefaultValue(768)]
        public short VHeight
        {
            get { return _vHeight; }
            set { _vHeight = value; }
        }

        [DataMember]
        private short _hTotal = 1024;

        [Category("Параметры RGB")]
        [DisplayName("Total_W")]
        [XmlAttribute("HTotal")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short HTotal
        {
            get { return _hTotal; }
            set { _hTotal = value; }
        }

        [DataMember]
        private short _hOffset = 0;

        [Category("Параметры RGB")]
        [DisplayName("Offset_W")]
        [XmlAttribute("HOffset")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short HOffset
        {
            get { return _hOffset; }
            set { _hOffset = value; }
        }

        [DataMember]
        private short _hWidth = 1024;

        [Category("Параметры RGB")]
        [DisplayName("Active_W")]
        [XmlAttribute("HWidth")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        [DefaultValue(1024)]
        public short HWidth
        {
            get { return _hWidth; }
            set { _hWidth = value; }
        }

        [DataMember]
        private short _phase = 0;

        [Browsable(false)]
        [Category("Параметры RGB")]
        [DisplayName("Phase")]
        [XmlAttribute("Phase")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        [DefaultValue(0)]
        public short Phase
        {
            get { return _phase; }
            set { _phase = value; }
        }

        [DataMember]
        private short _vFreq = 60;

        [Category("Параметры RGB")]
        [DisplayName("V_FREQ")]
        [XmlAttribute("VFreq")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        [DefaultValue(60)]
        public short VFreq
        {
            get { return _vFreq; }
            set
            {
                if (value < 60 || value > 120) throw new ApplicationException("Значение должно быть в диапазоне от 60 до 120");
                _vFreq = value;
            }
        }

        [DataMember]
        private bool _vSyncNeg = false;

        [Browsable(false)]
        [Category("Параметры RGB")]
        [DisplayName("Polarity Negative V Sync")]
        [XmlAttribute("VSyncNeg")]
        [DefaultValue(false)]
        public bool VSyncNeg
        {
            get { return _vSyncNeg; }
            set { _vSyncNeg = value; }
        }

        [DataMember]
        private bool _hSyncNeg = false;

        [Browsable(false)]
        [Category("Параметры RGB")]
        [DisplayName("Polarity Negative H Sync")]
        [XmlAttribute("HSyncNeg")]
        [DefaultValue(false)]
        public bool HSyncNeg
        {
            get { return _hSyncNeg; }
            set { _hSyncNeg = value; }
        }

        [DataMember]
        private bool _manualSetting = false;

        [Category("Параметры RGB")]
        [DisplayName("Ручная настройка RGB")]
        [XmlAttribute("ManualSetting")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.YesNoConverter, TechnicalServices.Common")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool ManualSetting
        {
            get { return _manualSetting; }
            set { _manualSetting = value; }
        }

        #endregion

        public override string ToString()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RGBParam));
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, this);
            }
            return builder.ToString();
        }

        public static RGBParam Parse(string rgb)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RGBParam));
            using (StringReader reader = new StringReader(rgb))
            {
                return (RGBParam)serializer.Deserialize(reader);
            }
        }

        public static bool TryParse(string rgb, out RGBParam rgbParam)
        {
            rgbParam = null;
            try
            {
                rgbParam = Parse(rgb);
                return true;
            }
            catch
            {
                return false;
            }
        }

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
            PropertyDescriptorCollection propColl;
            if (attributes == null)
                propColl = TypeDescriptor.GetProperties(this, true);
            else
                propColl = TypeDescriptor.GetProperties(this, attributes, true);

            if (ManualSetting) return propColl;

            // в зависимости от наличия матрицы доступны различные свойства
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
            {
                if (propertyDescriptor.Name == "ManualSetting")
                    newColl.Add(propertyDescriptor);
                else
                    newColl.Add(new ReadOnlyPropertyDescriptor(propertyDescriptor));
            }
            return new PropertyDescriptorCollection(newColl.ToArray(), true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }

    [TypeConverter("TechnicalServices.Common.TypeConverters.ExpandableNameConverter, TechnicalServices.Common")]
    public class ReadOnlyRGBParam
    {
        private readonly RGBParam _rgbParam;

        public ReadOnlyRGBParam(RGBParam rgbParam)
        {
            _rgbParam = rgbParam;
        }

        #region "Параметры RGB"
        // Группа "Параметры RGB"
        [Category("Параметры RGB")]
        [DisplayName("Total_H")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short VTotal
        {
            get { return _rgbParam.VTotal; }
        }

        [Category("Параметры RGB")]
        [DisplayName("Offset_H")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short VOffset
        {
            get { return _rgbParam.VOffset; }
        }

        [Category("Параметры RGB")]
        [DisplayName("Active_H")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short VHeight
        {
            get { return _rgbParam.VHeight; }
        }

        [Category("Параметры RGB")]
        [DisplayName("Total_W")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short HTotal
        {
            get { return _rgbParam.HTotal; }
        }

        [Category("Параметры RGB")]
        [DisplayName("Offset_W")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short HOffset
        {
            get { return _rgbParam.HOffset; }
        }

        [Category("Параметры RGB")]
        [DisplayName("Active_W")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short HWidth
        {
            get { return _rgbParam.HWidth; }
        }

        [Browsable(false)]
        [Category("Параметры RGB")]
        [DisplayName("Phase")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short Phase
        {
            get { return _rgbParam.Phase; }
        }

        [Category("Параметры RGB")]
        [DisplayName("V_FREQ")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt16Converter, TechnicalServices.Common")]
        public short VFreq
        {
            get { return _rgbParam.VFreq; }
        }

        [Browsable(false)]
        [Category("Параметры RGB")]
        [DisplayName("Polarity Negative V Sync")]
        public bool VSyncNeg
        {
            get { return _rgbParam.VSyncNeg; }
        }

        [Browsable(false)]
        [Category("Параметры RGB")]
        [DisplayName("Polarity Negative H Sync")]
        public bool HSyncNeg
        {
            get { return _rgbParam.HSyncNeg; }
        }

        [Category("Параметры RGB")]
        [DisplayName("Ручная настройка RGB")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.YesNoConverter, TechnicalServices.Common")]
        public bool ManualSetting
        {
            get { return _rgbParam.ManualSetting; }
        }

        #endregion

    }

    internal sealed class ReadOnlyPropertyDescriptor : PropertyDescriptor
    {
        private readonly PropertyDescriptor _pd;

        public ReadOnlyPropertyDescriptor(PropertyDescriptor pd)
            : base(pd)
        {
            _pd = pd;
        }

        public override AttributeCollection Attributes
        {
            get { return _pd.Attributes; }
        }

        public override Type ComponentType
        {
            get { return _pd.ComponentType; }
        }

        public override TypeConverter Converter
        {
            get { return _pd.Converter; }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override Type PropertyType
        {
            get { return _pd.PropertyType; }
        }

        protected override void FillAttributes(IList attributeList)
        {
        }

        public override object GetEditor(Type editorBaseType)
        {
            return _pd.GetEditor(editorBaseType);
        }

        public override bool CanResetValue(object component)
        {
            return _pd.CanResetValue(component);
        }

        public override object GetValue(object component)
        {
            return _pd.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            _pd.ResetValue(component);
        }

        public override void SetValue(object component, object val)
        {
            _pd.SetValue(component, val);
        }

        // Determines whether a value should be serialized.
        public override bool ShouldSerializeValue(object component)
        {
            return _pd.ShouldSerializeValue(component);
        }
    }
}
