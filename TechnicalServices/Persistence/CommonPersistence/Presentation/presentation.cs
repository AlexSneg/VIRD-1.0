using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Persistence.CommonPersistence.Presentation.PropertySorterConverter;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.SystemPersistence.Presentation
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "urn:presentation-schema")]
    public class Presentation
    {
        private readonly List<DisplayGroup> _displayGroupList = new List<DisplayGroup>();
        private readonly List<ResourceDescriptor> _localResourceDescriptorList = new List<ResourceDescriptor>();
        private readonly List<Slide> _slideList = new List<Slide>();

        private XmlSerializableDictionary<int, SlideLinkList> _linkDictionary =
            new XmlSerializableDictionary<int, SlideLinkList>();

        private XmlSerializableDictionary<int, Point> _slidePositionList = new XmlSerializableDictionary<int, Point>();
        private XmlSerializableDictionary<string, int> _displayPositionList = new XmlSerializableDictionary<string, int>();
        private Slide _startSlide;

        public Presentation()
            : this(Guid.NewGuid().ToString("N"))
        {
        }

        public Presentation(string uniqueName)
        {
            UniqueName = uniqueName;
            CreationDate = DateTime.Now;
            LastChangeDate = CreationDate;
        }


        [XmlArray("DisplayGroupList"), XmlArrayItem("DisplayGroup")]
				//[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        public List<DisplayGroup> DisplayGroupList
        {
            get { return _displayGroupList; }
        }

        /// <summary>
        /// позиция слайдов на сценарии, в качестве ключа - айдишник слайда
        /// </summary>
        [XmlElement("SlidePositionList")]
        public XmlSerializableDictionary<int, Point> SlidePositionList
        {
            get { return _slidePositionList; }
            set { _slidePositionList = value; }
        }

        /// <summary>
        /// позиция дисплеев в списке дисплеев, в качестве ключа - айдишник слайда
        /// </summary>
        [XmlElement("DisplayPositionList")]
        public XmlSerializableDictionary<string, int> DisplayPositionList
        {
            get { return _displayPositionList; }
            set { _displayPositionList = value; }
        }

        /// <summary>
        /// линки - вынесено из слайдов, в качестве ключа - айдишник слайдов
        /// </summary>
        [XmlElement("LinkDictionary")]
        public XmlSerializableDictionary<int, SlideLinkList> LinkDictionary
        {
            get { return _linkDictionary; }
            set { _linkDictionary = value; }
        }

        [XmlArray("SlideList"), XmlArrayItem("Slide")]
				//[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        public List<Slide> SlideList
        {
            get { return _slideList; }
        }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("UniqueName")]
        public string UniqueName { get; set; }

        [XmlAttribute("Author")]
        public string Author { get; set; }

        [XmlAttribute("CreationDate")]
        public DateTime CreationDate { get; set; }

        [XmlAttribute("LastChangeDate")]
        public DateTime LastChangeDate { get; set; }

        [XmlAttribute("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// !!!     ИСПОЛЬЗУЕТСЯ ТОЛЬКО ДЛЯ СЕРИАЛИЗАЦИИ В XML              !!! 
        /// !!! Увижу использование, кроме оговоренных случаев, ЯЙЦА ОТОРВУ !!! 
        /// !!! Имя атрибута соответствует имени свойства, которое          !!!
        /// !!! необходимо использовать                                     !!!
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        [XmlAttribute("StartSlide")]
        public int StartSlideId { get; set; }

        [XmlIgnore]
        public Slide StartSlide
        {
            get { return _startSlide; }
            set
            {
                if (value == _startSlide) return;
                _startSlide = value;
                StartSlideId = _startSlide.Id;
            }
        }

        /// <summary>
        /// список локальных ресурсов доступных для данной презентации
        /// </summary>
        [XmlIgnore]
				//[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        public List<ResourceDescriptor> LocalResourceDescriptorList
        {
            get { return _localResourceDescriptorList; }
        }

        public static string GenerateNewUniqueName()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// должно вызываться после десериализации для инициализации ссылочных полей
        /// </summary>
        /// <returns>возвращается список оборудования которое отсутсвует в конфигурации и таким образом было исключено из сценария</returns>
        public string[] InitReference(ModuleConfiguration config, IEnumerable<Label> labelList,
                                      ResourceDescriptor[] descriptors,
                                      DeviceResourceDescriptor[] deviceResourceDescriptors)
        {
            foreach (ResourceDescriptor descriptor in descriptors)
            {
                if (descriptor.IsLocal)
                {
                    if (!LocalResourceDescriptorList.Contains(descriptor))
                        LocalResourceDescriptorList.Add(descriptor);
                }
            }

            foreach (KeyValuePair<int, SlideLinkList> pair in _linkDictionary)
            {
                foreach (Link link in pair.Value.LinkList)
                {
                    // на прямое использование link в linq ругается Решарпер
                    Link l = link;
                    Slide nextSlide = SlideList.Find(sl => sl.Id == l.NextSlideId);
                    if (nextSlide == null)
                        throw new KeyNotFoundException("Presentation.InitReference: Нет такого слайда в коллекции");
                    else
                    {
                        link.NextSlide = nextSlide;
                    }
                }
            }

            // стартовый слайд:
            Slide slide = SlideList.Find(sl => sl.Id == StartSlideId);
            if (slide == null)
                throw new KeyNotFoundException("Presentation.InitReference: Нет такого слайда в коллекции: " +
                                               StartSlideId);
            StartSlide = slide;

            // Initreference для каждого слайда
            List<string> deletedElements = new List<string>();
            foreach (Slide sl in SlideList)
            {
                // метки кирдыкаем тут, а то InitReference вызывается на клиенте- шоб туда метки не тащить
                if (sl.LabelId != Label.NullId && !labelList.Any(label=>label.Id == sl.LabelId))
                {
                    deletedElements.Add(string.Format("Метка с идентификатором {0}", sl.LabelId));
                    sl.LabelId = Label.NullId;
                }
                deletedElements.AddRange(sl.InitReference(config, descriptors, deviceResourceDescriptors));
            }
            return deletedElements.Distinct().ToArray();
        }

        /// <summary>
        /// удаляет ссылку на ResourceDescriptor если есть
        /// </summary>
        /// <param name="descriptor">ResourceDescriptor</param>
        /// <returns> если есть и удалена спешно - true, иначе false</returns>
        public bool Remove(ResourceDescriptor descriptor)
        {
            bool isDeleted = false;
            foreach (Slide slide in SlideList)
            {
                if (slide.Remove(descriptor))
                {
                    LocalResourceDescriptorList.Remove(descriptor);
                    isDeleted = true;
                }
            }
            return isDeleted;
        }

        /// <summary>
        /// удаляет ссылку на DeviceResourceDescriptor если есть
        /// </summary>
        /// <param name="descriptor">DeviceResourceDescriptor</param>
        /// <returns> если есть и удалена спешно - true, иначе false</returns>
        public bool Remove(DeviceResourceDescriptor descriptor)
        {
            bool isDeleted = false;
            foreach (Slide slide in SlideList)
            {
                if (slide.Remove(descriptor))
                {
                    isDeleted = true;
                }
            }
            return isDeleted;
        }

        public override bool Equals(object obj)
        {
            Presentation presentation = obj as Presentation;
            if (presentation != null)
                return UniqueName.Equals(presentation.UniqueName,
                                         StringComparison.InvariantCultureIgnoreCase);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return UniqueName.GetHashCode();
        }

    }

    /// <summary>
    /// описание группы дисплеев - содержит только имена дисплеев!!!
    /// задача распознования на каком слайде какой дисплей из группы
    /// и обновления слайдов - полностью лежит на клиентском софте!!!!
    /// </summary>
    [Serializable]
    public class DisplayGroup : IComparable<DisplayGroup>
    {
        private readonly List<string> _displayNameList = new List<string>();
        private string _type = string.Empty;

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Comment")]
        public string Comment { get; set; }

        [XmlAttribute("Width")]
        public int Width { get; set; }

        [XmlAttribute("Height")]
        public int Height { get; set; }

        [XmlAttribute("Type")]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// элементы - имена дисплеев
        /// </summary>
        [XmlArray("DisplayNameList"), XmlArrayItem("Name")]
				//[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        public List<string> DisplayNameList
        {
            get { return _displayNameList; }
        }

        #region IComparable<DisplayGroup> Members

        public int CompareTo(DisplayGroup other)
        {
            return Name.CompareTo(other.Name);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public class Slide : ICloneable
    {
        [DataMember] private readonly List<Device> _deviceList = new List<Device>();
        [DataMember] private readonly List<Display> _displayList = new List<Display>();
        //private readonly List<Link> _linkList = new List<Link>();
        [DataMember] private readonly List<Source> _sourceList = new List<Source>();

        [XmlIgnore]
        public bool IsLocked { get; set; }

        [DataMember]
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [DataMember]
        [XmlAttribute("Label")]
        public int LabelId { get; set; }

        [DataMember]
        [XmlAttribute("Comment")]
        public string Comment { get; set; }

        [DataMember]
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// !!!     ИСПОЛЬЗУЕТСЯ ТОЛЬКО ДЛЯ СЕРИАЛИЗАЦИИ В XML              !!! 
        /// !!! Увижу использование, кроме оговоренных случаев, ЯЙЦА ОТОРВУ !!! 
        /// !!! Имя атрибута соответствует имени свойства, которое          !!!
        /// !!! необходимо использовать                                     !!!
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [XmlAttribute("Time")]
        public long TimeTicks
        {
            get { return Time.Ticks; }
            set { Time = new TimeSpan(value); }
        }

        [DataMember]
        [XmlIgnore]
        public TimeSpan Time { get; set; }

        [DataMember]
        [XmlAttribute("State")]
        public SlideState State { get; set; }

        [DataMember]
        [XmlAttribute("Author")]
        public string Author { get; set; }

        [DataMember]
        [XmlAttribute("Modified")]
        public DateTime Modified { get; set; }

        [XmlArray("DeviceList")]
				//[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        public List<Device> DeviceList
        {
            get { return _deviceList; }
        }

        [XmlArray("SourceList")]
				//[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        public List<Source> SourceList
        {
            get { return _sourceList; }
        }

        [XmlArray("DisplayList")]
				//[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        public List<Display> DisplayList
        {
            get { return _displayList; }
        }

        [XmlIgnore]
        public bool Cached { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            Slide slide = new Slide
                              {
                                  Id = Id,
                                  Name = Name,
                                  Author = Author,
                                  Comment = Comment,
                                  LabelId = LabelId,
                                  Modified = Modified,
                                  Time = Time,
                                  State = State
                              };
            slide.DeviceList.AddRange(DeviceList.Select(dev=>(Device)dev.Clone()));
            slide.SourceList.AddRange(SourceList.Select(source => (Source)source.Clone()));

            foreach (Display display in this.DisplayList)
            {
                Display displayClone = (Display)display.Clone();
                displayClone.WindowList.Clear();
                foreach (Window window in display.WindowList)
                {
                    XmlSerializer cloner = new XmlSerializer(window.GetType());
                    StringBuilder sb = new StringBuilder();
                    using (XmlWriter x = XmlWriter.Create(sb))
                    {
                        cloner.Serialize(x, window);
                        Window result = (Window) cloner.Deserialize(new StringReader(sb.ToString()));
                        if (window.Source != null)
                        {
                            Source source = slide.SourceList.Find(s => s.Id == window.Source.Id);
                            if (source != null)
                            {
                                result.Source = source;
                            }
                        }
                        displayClone.WindowList.Add(result);
                    }
                }
                slide.DisplayList.Add(displayClone);
            }
            //slide.DisplayList.AddRange(DisplayList.Select(dis=>(Display)dis.Clone()));
            // для дисплеев нужно сделать правильные ссылки для окон
            slide.IsLocked = IsLocked;
            return slide;
        }

        #endregion

        public string[] InitReference(ModuleConfiguration config, ResourceDescriptor[] descriptors,
                                      DeviceResourceDescriptor[] deviceResourceDescriptors)
        {
            return InitReference(config,descriptors, deviceResourceDescriptors, false);
        }

        public string[] InitReference(ModuleConfiguration config, ResourceDescriptor[] descriptors,
                                      DeviceResourceDescriptor[] deviceResourceDescriptors,
                                      bool preserveSourceIfResourceNotFound)
        {
            List<Device> deviceNotExistsInConfig = new List<Device>(DeviceList.Count);
            foreach (Device device in DeviceList)
            {
                // на прямое использование device в linq ругается Решарпер
                Device d = device;
                DeviceType deviceType = config.DeviceList.Find(configDevice => configDevice.Name.Equals(d.EquipmentType));
                if (deviceType == null)
                {
                    deviceNotExistsInConfig.Add(device);
                    continue;
                }
                else
                {
                    device.Type = deviceType;
                }

                if (!string.IsNullOrEmpty(device.ResourceId))
                {
                    DeviceResourceDescriptor deviceResourceDescriptor = FindDescriptor(device, deviceResourceDescriptors);
                    if (deviceResourceDescriptor == null && !preserveSourceIfResourceNotFound)
                        throw new KeyNotFoundException(
                            string.Format(
                                "Presentation.InitReference: Нет такого ресурса в списке ресурсов девайса: Type: {0}, ResourceId: {1}",
                                device.EquipmentType, device.ResourceId));
                    device.DeviceResourceDescriptor = deviceResourceDescriptor;
                }
                //throw new KeyNotFoundException("Presentation.InitReference: Нет такого типа в конфигураторе " +
                //                               device.EquipmentType);
            }
            //исключаем эти устройства из списка
            DeviceList.RemoveAll(dev=>deviceNotExistsInConfig.Exists(d=>object.ReferenceEquals(d,dev)));

            List<Source> sourceNotExistsInConfig = new List<Source>(SourceList.Count);
            foreach (Source source in SourceList)
            {
                // на прямое использование source в linq ругается Решарпер
                Source s = source;
                SourceType sourceType = config.SourceList.Find(configSource => configSource.Name.Equals(s.EquipmentType));
                if (sourceType == null)
                {
                    sourceNotExistsInConfig.Add(source);
                    continue;
                    //throw new KeyNotFoundException("Presentation.InitReference: Нет такого типа в конфигураторе " +
                    //                               source.EquipmentType);
                }
                else
                {
                    source.Type = sourceType;
                }
                if (!string.IsNullOrEmpty(source.ResourceId))
                {
                    ResourceDescriptor descriptor = FindDescriptor(source, descriptors);
                    if (descriptor == null && !preserveSourceIfResourceNotFound)
                        throw new KeyNotFoundException(
                            string.Format(
                                "Presentation.InitReference: Нет такого ресурса в списке ресурсов: Type: {0}, ResourceId: {1}, IsLocal: {2}",
                                source.EquipmentType, source.ResourceId, source.IsLocal));
                    source.ResourceDescriptor = descriptor;
                }
                if (!string.IsNullOrEmpty(source.DeviceId))
                {
                    Device d =
                        DeviceList.Find(
                            device =>
                            device.Type.Name.Equals(s.DeviceId, StringComparison.InvariantCultureIgnoreCase));
                    if (d == null)
                        throw new KeyNotFoundException(
                            "Presentation.InitReference: Нет такого устройства в коллекции устройств у слайда " +
                            source.DeviceId);
                    else
                    {
                        source.Device = d;
                    }
                }
            }
            // исключаем сорсы из списка
            SourceList.RemoveAll(source=>sourceNotExistsInConfig.Exists(s=>object.ReferenceEquals(s,source)));

            List<Display> displayNotExistsInConfig = new List<Display>(DisplayList.Count);
            foreach (Display display in DisplayList)
            {
                // на прямое использование display в linq ругается Решарпер
                Display d = display;
                DisplayType displayType = 
                    config.DisplayList.Find(configDisplay => configDisplay.Name.Equals(d.EquipmentType));
                if (displayType == null)
                {
                    displayNotExistsInConfig.Add(display);
                    continue;
                    //throw new KeyNotFoundException("Presentation.InitReference: Нет такого типа в конфигураторе " +
                    //                               display.EquipmentType);
                }
                else
                {
                    display.Type = displayType;
                }
                //TODO
                // пока окна будут удаляться для всех источников!
                List<Window> deletedWindow = new List<Window>(display.WindowList.Count);
                foreach (Window window in display.WindowList)
                {
                    // на прямое использование window в linq ругается Решарпер
                    Window w = window;
                    Source s = SourceList.Find(source => source.Id.Equals(w.SourceId));
                    if (s == null)
                    {
                        deletedWindow.Add(window);
                        //throw new KeyNotFoundException(
                        //    "Presentation.InitReference: Нет такого сорса в коллекции сорсов у слайда");
                    }
                    else
                    {
                        window.Source = s;
                        display.AfterWindowAdded(window, this);
                    }
                }
                display.WindowList.RemoveAll(win=>deletedWindow.Exists(dw=>object.ReferenceEquals(dw,win)));
            }
            // исключаем дисплеи из списка
            DisplayList.RemoveAll(dis=>displayNotExistsInConfig.Exists(d=>object.ReferenceEquals(d,dis)));

            return deviceNotExistsInConfig.Select(dev => dev.EquipmentType).
                Union(sourceNotExistsInConfig.Select(source => source.EquipmentType)).
                Union(displayNotExistsInConfig.Select(dis => dis.EquipmentType)).ToArray();
        }

        public bool Remove(ResourceDescriptor descriptor)
        {
            IEnumerable<Source> deletedSourceList = SourceList.Where(s => s.ResourceDescriptor != null && s.ResourceDescriptor.ResourceInfo != null
                                  && s.ResourceDescriptor.Equals(descriptor));
            if (deletedSourceList.Count() == 0) return false;
            foreach (Display display in DisplayList)
            {
                display.WindowList.RemoveAll(win => deletedSourceList.Any(s => s.Equals(win.Source)));
            }
            return SourceList.RemoveAll(s=>deletedSourceList.Any(ds=>s.Equals(ds))) != 0;
        }

        public bool Remove(DeviceResourceDescriptor descriptor)
        {
            IEnumerable<Device> deletedDeviceList = DeviceList.Where(d => d.DeviceResourceDescriptor != null && d.DeviceResourceDescriptor.ResourceInfo != null
                                  && d.DeviceResourceDescriptor.Equals(descriptor));
            if (deletedDeviceList.Count() == 0) return false;
            return DeviceList.RemoveAll(d => deletedDeviceList.Any(dd => d.Equals(dd))) != 0;
        }

        private static DeviceResourceDescriptor FindDescriptor(Device device,
                                                        IEnumerable<DeviceResourceDescriptor> deviceResourceDescriptors)
        {
            foreach (DeviceResourceDescriptor descriptor in deviceResourceDescriptors)
            {
                if (device.ResourceId != null &&
                    device.ResourceId.Equals(descriptor.ResourceInfo.Id, StringComparison.InvariantCultureIgnoreCase) &&
                    device.Type.Type.Equals(descriptor.ResourceInfo.Type, StringComparison.InvariantCultureIgnoreCase))
                    return descriptor;
            }
            return null;
        }

        private static ResourceDescriptor FindDescriptor(Source source,
                                                         IEnumerable<ResourceDescriptor> resourceDescriptors)
        {
            foreach (ResourceDescriptor descriptor in resourceDescriptors)
            {
                if (source.ResourceId != null && source.IsLocal == descriptor.IsLocal &&
                    source.ResourceId.Equals(descriptor.ResourceInfo.Id, StringComparison.InvariantCultureIgnoreCase) &&
                    source.Type.Type.Equals(descriptor.ResourceInfo.Type, StringComparison.InvariantCultureIgnoreCase))
                    return descriptor;
            }
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [TypeConverter("TechnicalServices.Common.TypeConverters.SourceConverter, TechnicalServices.Common")]
    public abstract class Source : Equipment<SourceType>, IEquatable<Source>, IComponent, ICloneable
    {
        private static readonly object EventDisposed = new object();
        private Device _device;
        private bool _isLocal;
        private ResourceDescriptor _resourceDescriptor;
        private string _resourceId;
        [NonSerialized] private EventHandlerList events;
        [NonSerialized] private ISite site;

        [Browsable(false)]
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [Browsable(false)]
        [XmlAttribute("IsLocal")]
        public bool IsLocal
        {
            get
            {
                if (ResourceDescriptor == null)
                    return _isLocal;
                return ResourceDescriptor.IsLocal;
            }
            set { _isLocal = value; }
        }

        [Browsable(false)]
        [XmlAttribute("ResourceId")]
        public string ResourceId
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null)
                    return _resourceId;
                return ResourceDescriptor.ResourceInfo.Id;
            }
            set { _resourceId = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public override string Name
        {
            get { return base.Name; }
        }

        [Category("Общие параметры")]
        [DisplayName("Название")]
        [RefreshProperties(RefreshProperties.All)]
        [XmlIgnore]
        public virtual ResourceDescriptor ResourceDescriptor
        {
            get { return _resourceDescriptor; }
            set
            {
                _resourceDescriptor = value;
                if (_resourceDescriptor == null)
                {
                    ResourceId = null;
                    return;
                }
                if (
                    !_resourceDescriptor.ResourceInfo.Type.Equals(Type.Type, StringComparison.InvariantCultureIgnoreCase))
                    throw new InvalidCastException(string.Format("Ресурс должен быть типа {0}", Type.Type));
                //IsLocal = _resourceDescriptor.IsLocal;
                //Name = _resourceDescriptor.ResourceInfo.Name;

                UpdateDevice(value);
            }
        }

        [XmlIgnore]
        public override string Comment
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null)
                    return base.Comment;
                return ResourceDescriptor.ResourceInfo.Comment;
            }
        }

        [Browsable(false)]
        [XmlAttribute("Device")]
        public string DeviceId { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        [Category("Настройки")]
        [DisplayName("Управление")]
        public virtual Device Device
        {
            get { return _device; }
            set
            {
                if (_device != value)
                {
                    _device = value;
                    DeviceId = _device.Type.Name;
                }
            }
        }

        // Properties
        protected virtual bool CanRaiseEvents
        {
            get { return true; }
        }

        internal bool CanRaiseEventsInternal
        {
            get { return CanRaiseEvents; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IContainer Container
        {
            get { return site != null ? site.Container : null; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        protected bool DesignMode
        {
            get { return ((site != null) && site.DesignMode); }
        }

        protected EventHandlerList Events
        {
            get
            {
                if (events == null)
                    events = new EventHandlerList();
                return events;
            }
        }

        #region IComponent Members

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler Disposed
        {
            add { Events.AddHandler(EventDisposed, value); }
            remove { Events.RemoveHandler(EventDisposed, value); }
        }

        // Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [XmlIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISite Site
        {
            get { return site; }
            set { site = value; }
        }

        #endregion

        #region IEquatable<Source> Members

        public bool Equals(Source other)
        {
            return Id == other.Id /*&& this.IsLocal == other.IsLocal*/;
        }

        #endregion

        /// <summary>
        /// при редактировании свойств аппаратных источников в PropertyGrid
        /// если выбрали другой ресурс, надо поменять ссылку на девайс
        /// </summary>
        /// <param name="res"></param>
        private void UpdateDevice(ResourceDescriptor res)
        {
            if (res.ResourceInfo.SourceType != null)
                Type = res.ResourceInfo.SourceType;

            if ((res.ResourceInfo.DeviceType == null) || (res.Site == null))
                return;

            IResourceProvider resourceProvider = (IResourceProvider) res.Site.GetService(typeof (IResourceProvider));
            if (resourceProvider == null) return;

            Device = resourceProvider.GetDeviceByName(res.ResourceInfo.DeviceType);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this)
                {
                    if ((site != null) && (site.Container != null))
                    {
                        site.Container.Remove(this);
                    }
                    if (events != null)
                    {
                        EventHandler handler = (EventHandler) events[EventDisposed];
                        if (handler != null)
                        {
                            handler(this, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        ~Source()
        {
            Dispose(false);
        }

        protected virtual object GetService(Type service)
        {
            return site != null ? site.GetService(service) : null;
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class HardwareSource : Source
    {
        [Category("Общие параметры")]
        [DisplayName("Вход коммутатора")]
        [XmlIgnore]
        public int Input
        {
            get { return ((HardwareSourceType) Type).Input; }
        }

        [XmlIgnore]
        [Browsable(true)]
        public override Device Device
        {
            get { return base.Device; }
            set { base.Device = value; }
        }

        [Browsable(true)]
        [XmlIgnore]
        [Category("Параметры RGB")]
        [DisplayName("Параметры RGB")]
        public ReadOnlyRGBParam RGBParam
        {
            get {return new ReadOnlyRGBParam(((ResourceInfoForHardwareSource)this.ResourceDescriptor.ResourceInfo).RGBParam);}
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class SoftwareSource : Source
    {
        private string _contentPath;

        [Browsable(false)]
        [XmlIgnore]
        public override string Model
        {
            get { return base.Model; }
        }

        [Browsable(false)]
        [XmlAttribute("Path")]
        public string ContentPath
        {
            get
            {
                if (ResourceDescriptor == null || ResourceDescriptor.ResourceInfo == null)
                    return _contentPath;
                ResourceFileProperty resourceFileProperty =
                    ((ResourceFileInfo) ResourceDescriptor.ResourceInfo).MasterResourceProperty;
                return resourceFileProperty != null ? resourceFileProperty.ResourceFileName : null;
            }
            set { _contentPath = value; }
        }

        [XmlIgnore]
        public override ResourceDescriptor ResourceDescriptor
        {
            get { return base.ResourceDescriptor; }
            set
            {
                base.ResourceDescriptor = value;
                if (null == value)
                {
                    ContentPath = null;
                    return;
                }
                ResourceFileInfo info = value.ResourceInfo as ResourceFileInfo;
                if (info == null) throw new Exception("ResourceInfo додлжен быть типа ResourceFileInfo");
                //ContentPath = info.ResourceFileName;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    [TypeConverter(typeof (PropertySorter))]
    public abstract class Device : Equipment<DeviceType>, IEquatable<Device>, ICloneable
    {
        private DeviceResourceDescriptor _resourceDescriptor;
        private string _resourceId;

        [Browsable(false)]
        [XmlAttribute("ResourceId")]
        public string ResourceId
        {
            get
            {
                if (DeviceResourceDescriptor == null || DeviceResourceDescriptor.ResourceInfo == null)
                    return _resourceId;
                return DeviceResourceDescriptor.ResourceInfo.Id;
            }
            set { _resourceId = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public virtual DeviceResourceDescriptor DeviceResourceDescriptor
        {
            get { return _resourceDescriptor; }
            set
            {
                _resourceDescriptor = value;
                if (_resourceDescriptor == null)
                {
                    ResourceId = null;
                    return;
                }
                if (
                    !_resourceDescriptor.ResourceInfo.Type.Equals(Type.Type, StringComparison.InvariantCultureIgnoreCase))
                    throw new InvalidCastException(string.Format("Ресурс должен быть типа {0}", Type.Type));
            }
        }

        #region Implementation of IEquatable<Device>

        public bool Equals(Device other)
        {
            return Type.Equals(other.Type);
        }

        #endregion

        #region Implementation of ICloneable

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    [Serializable]
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public abstract class DeviceAsSource : Device
    {
        [Browsable(false)]
        [XmlIgnore]
        public override string PluginName
        {
            get { return base.PluginName; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public override string Name
        {
            get { return base.Name; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public override string Model
        {
            get { return base.Model; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public override string Comment
        {
            get { return base.Comment; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class Display : Equipment<DisplayType>, IEquatable<Display>, ICloneable
    {
        private List<Window> _windowList = new List<Window>();

        public virtual void AfterWindowAdded(Window wnd, Slide slide)
        {
            if (wnd.Source is ISourceContentSize)
            {
                ((ISourceContentSize)wnd.Source).SetContentSize(new Size(wnd.Width, wnd.Height));
            }

        }
        /// <summary>
        /// Ширина экрана (в пикселях) (1..4096)
        /// Получаем из Конфигуратора
        /// По умолчанию: 1024
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int Width
        {
            get { return Type.Width; }
        }

        /// <summary>
        /// Высота экрана (в пикселях) (1..4096)
        /// Получаем из Конфигуратора
        /// По умолчанию: 768
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int Height
        {
            get { return Type.Height; }
        }

        /// <summary>
        /// Разрешение экрана (в пикселях)
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Разрешение (px)")]
        [XmlIgnore]
        [TypeConverter(typeof (ExpandableObjectConverter))]
        virtual public Size<int> Size
        {
            get { return Type.Size; }
        }

        [Category("Общие параметры")]
        [DisplayName("Группа")]
        [ReadOnly(true)]
        [XmlAttribute("DisplayGroup")]
        public string DisplayGroup { get; set; }

        [Browsable(false)]
        [XmlArray("WindowList"), XmlArrayItem(typeof (Window))]
        public List<Window> WindowList
        {
            get { return _windowList; }
        }

        #region IEquatable<Display> Members

        public bool Equals(Display other)
        {
            return Type.Type == other.Type.Type && Type.Name == other.Type.Name;
        }

        #endregion

        /// <summary>
        /// Валидация всех окон дисплея
        /// </summary>
        public virtual void Validate(Slide slide)
        {
        }

        public Window CreateWindow(Source source, Slide slide)
        {
            Window window = CreateWindowProtected(source, slide);
            window.Source = source;
            ISourceSize sourceSize = source as ISourceSize;
            IAspectLock aspectLock = source as IAspectLock;
            if (!Type.SupportsMultiWindow)
            {
                // если диспелей не поддерживает многооконность но сорс поддерживает AspectLock - его надо отключать
                if (aspectLock != null)
                {
                    aspectLock.AspectLock = false;
                }
                window.Width = Width;
                window.Height = Height;
            }
            else if (sourceSize != null)
            {
                window.Width = sourceSize.Width;
                window.Height = sourceSize.Height;
            }
            return window;
        }

        protected abstract Window CreateWindowProtected(Source source, Slide slide);

        public override int GetHashCode()
        {
            return Type.Name.GetHashCode();
        }

        public virtual object Clone()
        {
            Display display = (Display)this.MemberwiseClone();
            display._windowList = new List<Window>();
            display.WindowList.AddRange(this.WindowList);
            return display;
        }

        /// <summary>
        /// дополнительная фильтрация возможных ResourceDescriptor в соответсвии с маппингом
        /// сейчас используется тольок в одном месте - при выборе справа источника для данного окна
        /// </summary>
        /// <param name="descriptors"></param>
        /// <param name="currentWindow">текущее окно на раскладке</param>
        /// <returns></returns>
        public virtual ResourceDescriptor[] ApplyAdditionalFilter(ResourceDescriptor[] descriptors, Window currentWindow)
        {
            List<ResourceDescriptor> list = new List<ResourceDescriptor>(descriptors.Length);
            IEnumerable<SourceType> mapping = this.Type.MappingList.Select(map => map.Source);
            foreach (ResourceDescriptor descriptor in descriptors)
            {
                if (descriptor != null && descriptor.ResourceInfo != null)
                {
                    if (descriptor.ResourceInfo.IsHardware && descriptor.ResourceInfo.SourceType != null)
                    {
                        if (mapping.Any(map=>map.Equals(descriptor.ResourceInfo.SourceType)))
                        {
                            list.Add(descriptor);
                        }
                    }
                    else
                    {
                        if (mapping.Any(map=>map.Type.Equals(descriptor.ResourceInfo.Type)))
                        {
                            list.Add(descriptor);
                        }
                    }
                }
            }
            return list.ToArray();
        }
    }

    /// <summary>
    /// Свойства окна с источником для всех типов дисплеев
    /// </summary>
    [Serializable]
    [XmlType("Window")]
    public class Window
    {
        private int _height;
        private Source _source;
        private int _width;
        private bool sizeChanging;

        [Browsable(false)]
        [Category("Размер/расположение")]
        [DisplayName("X")]
        [XmlAttribute("Left")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.Int32ConverterLocalized, TechnicalServices.Common")]
        public virtual int Left { get; set; }

        [Browsable(false)]
        [Category("Размер/расположение")]
        [DisplayName("Y")]
        [XmlAttribute("Top")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.Int32ConverterLocalized, TechnicalServices.Common")]
        public virtual int Top { get; set; }

        [Browsable(false)]
        [Category("Размер/расположение")]
        [DisplayName("W")]
        [XmlAttribute("Width")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.Int32ConverterLocalized, TechnicalServices.Common")]
        public virtual int Width
            //{ get; set; }
        {
            get { return _width; }
            set
            {
                if (value == _width) return;
                if (value < 1) return;
                if (sizeChanging) return;
                sizeChanging = true;
                int oldValue = _width;
                _width = value;
                IAspectLock aspL = Source as IAspectLock;
                if (aspL != null)
                {
                    if (aspL.AspectLock)
                    {
                        int val = 0;
                        ISourceSize src = Source as ISourceSize;
                        if ((src != null) && (src.Width != 0) && (src.Height != 0))
                        {
                            //это мы берем соотношение сторон исходя из исходных размеров источника
                            val = (int)Math.Round((_width * ((double)src.Height / (double)src.Width)));
                            _height = val;
                        }
                        else
                            if (oldValue != 0)
                            {
                                //если исходные размеры не доступны, то берем как отношение между новым и старым 
                                //значением, в этом случае можно потерять точность
                                val = (int)Math.Round((_height * (_width / (double)oldValue)));
                                _height = val;
                            }
                    }
                }
                sizeChanging = false;
            }
        }


        [Browsable(false)]
        [Category("Размер/расположение")]
        [DisplayName("H")]
        [XmlAttribute("Height")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.Int32ConverterLocalized, TechnicalServices.Common")]
        public virtual int Height
        {
            get { return _height; }
            set
            {
                if (value == _height) return;
                if (value < 1) return;
                if (sizeChanging) return;
                sizeChanging = true;
                int oldValue = _height;
                _height = value;
                IAspectLock aspL = Source as IAspectLock;
                if (aspL != null)
                {
                    if (aspL.AspectLock)
                    {
                        int val = 0;
                        ISourceSize src = Source as ISourceSize;
                        if ((src != null) && (src.Width != 0) && (src.Height != 0))
                        {
                            //это мы берем соотношение сторон исходя из исходных размеров источника
                            val = (int)Math.Round((_height * ((double)src.Width / (double)src.Height)));
                            _width = val;
                        }
                        else
                            if (oldValue != 0)
                            {
                                //если исходные размеры не доступны, то берем как отношение между новым и старым 
                                //значением, в этом случае можно потерять точность
                                val = (int)Math.Round((_width * (_height / (double)oldValue)));
                                _width = val;
                            }
                    }
                }
                sizeChanging = false;
            }
        }


        [Browsable(false)]
        [XmlAttribute("ZOrder")]
        public byte ZOrder { get; set; }

        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// !!!     ИСПОЛЬЗУЕТСЯ ТОЛЬКО ДЛЯ СЕРИАЛИЗАЦИИ В XML              !!! 
        /// !!! Увижу использование, кроме оговоренных случаев, ЯЙЦА ОТОРВУ !!! 
        /// !!! Имя атрибута соответствует имени свойства, которое          !!!
        /// !!! необходимо использовать                                     !!!
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [XmlAttribute("Source")]
        [Browsable(false)]
        public string SourceId { get; set; }

        [XmlIgnore]
        [Category("Настройки")]
        [DisplayName("Источник")]
        public Source Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    SourceId = _source.Id;
                }
            }
        }
    }

    /// <summary>
    /// Свойства окна с источником для активных дисплеев
    /// </summary>
    [Serializable]
    [XmlType("ActiveWindow")]
    public class ActiveWindow : Window, ICustomTypeDescriptor
    {
        // Группа "Размер/расположение" *****************************

        private string _borderColor = "Blue";
        private int _borderWidth;
        private int _croppingBottom;
        private int _croppingLeft;
        private int _croppingRight;
        private int _croppingTop;
        private string _titleColor = "White";
        private string _titleFont = "Arial";
        private int _titleSize = 14;
        private string _titleText;

        [Browsable(true)]
        [XmlAttribute("Left")]
        public override int Left
        {
            get { return base.Left; }
            set { base.Left = value; }
        }

        [Browsable(true)]
        [XmlAttribute("Top")]
        public override int Top
        {
            get { return base.Top; }
            set { base.Top = value; }
        }

        [Browsable(true)]
        [XmlAttribute("Width")]
        public override int Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        [Browsable(true)]
        [XmlAttribute("Height")]
        public override int Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        // Группа "Обрезка" *****************************************

        [Category("Обрезка")]
        [DisplayName("Слева")]
        [DefaultValue(0)]
        [XmlAttribute("CroppingLeft")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt32Converter, TechnicalServices.Common")]
        public int CroppingLeft
        {
            get { return _croppingLeft; }
            set
            {
                _croppingLeft = (value < 0
                                     ? 0
                                     : value);
                // STAS убрано ограничение на максимальную обрезку
                                     //(value >= Width - CroppingRight - 100 ? Width - CroppingRight - 100 : value));
            }
        }

        [Category("Обрезка")]
        [DisplayName("Справа")]
        [DefaultValue(0)]
        [XmlAttribute("CroppingRight")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt32Converter, TechnicalServices.Common")]
        public int CroppingRight
        {
            get { return _croppingRight; }
            set
            {
                _croppingRight = (value < 0
                                      ? 0
                                      : value);
                //STAS убрано ограничения на максимальную обрезку
                                      //(value >= Width - CroppingLeft - 100 ? Width - CroppingLeft - 100 : value));
            }
        }

        [Category("Обрезка")]
        [DisplayName("Сверху")]
        [DefaultValue(0)]
        [XmlAttribute("CroppingTop")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt32Converter, TechnicalServices.Common")]
        public int CroppingTop
        {
            get { return _croppingTop; }
            set
            {
                _croppingTop = (value < 0
                                    ? 0
                                    : value);
                //STAS убрано ограничения на максимальную обрезку
                                    //(value >= Height - CroppingBottom - 100 ? Height - CroppingBottom - 100 : value));
            }
        }

        [Category("Обрезка")]
        [DisplayName("Снизу")]
        [DefaultValue(0)]
        [XmlAttribute("CroppingBottom")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt32Converter, TechnicalServices.Common")]
        public int CroppingBottom
        {
            get { return _croppingBottom; }
            set
            {
                _croppingBottom = (value < 0
                                       ? 0
                                       : value);
                                //STAS убрано ограничения на максимальную обрезку
                                       //(value >= Height - CroppingTop - 100 ? Height - CroppingTop - 100 : value));
            }
        }

        // Группа "Рамка" ****************************************

        [Category("Рамка")]
        [DisplayName("Показывать рамку и заголовок")]
        [DefaultValue(false)]
        [XmlAttribute("BorderVisible")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.YesNoConverter, TechnicalServices.Common")]
        public bool BorderVisible { get; set; }

        [Category("Рамка")]
        [DisplayName("Ширина")]
        [DefaultValue(0)]
        [XmlAttribute("BorderWidth")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.NonNegativeInt32Converter, TechnicalServices.Common")]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = (value < 0 ? 0 : (value > 20 ? 20 : value)); }
        }

        [Category("Рамка")]
        [DisplayName("Цвет")]
        [XmlIgnore]
        public Color BorderColorFrienly
        {
            get { return ParseColor(BorderColor); }
            set { BorderColor = value.Name; }
        }

        [Browsable(false)]
        [XmlAttribute("BorderColor")]
        public string BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        // Группа "Заголовок" *************************************

        //[Category("Заголовок")]
        //[DisplayName("Показывать заголовок")]
        //[DefaultValue(false)]
        //[XmlAttribute("TitleVisible")]
        //[TypeConverter("TechnicalServices.Common.TypeConverters.YesNoConverter, TechnicalServices.Common")]
        //public bool TitleVisible { get; set; }

        [Category("Заголовок")]
        [DisplayName("Текст заголовка")]
        [XmlAttribute("TitleText")]
        public string TitleText
        {
            get { return _titleText; }
            set { _titleText = ValidationHelper.CheckLength(value, 100, "заголовка"); }
        }

        [Category("Заголовок")]
        [DisplayName("Шрифт")]
        [DefaultValue("Arial")]
        [XmlAttribute("TitleFont")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.FontFamilyConverter, TechnicalServices.Common")]
        public string TitleFont
        {
            get { return _titleFont; }
            set { _titleFont = value; }
        }

        [Category("Заголовок")]
        [DisplayName("Размер")]
        [DefaultValue(14)]
        [XmlAttribute("TitleSize")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.FontSizeConverter, TechnicalServices.Common")]
        public int TitleSize
        {
            get { return _titleSize; }
            set { _titleSize = value < 8 ? 8 : (value > 100 ? 100 : value); }
        }

        [Category("Заголовок")]
        [DisplayName("Цвет")]
        [XmlIgnore]
        public Color TitleColorFrienly
        {
            get { return ParseColor(TitleColor); }
            set { TitleColor = value.Name; }
        }

        [Browsable(false)]
        [XmlAttribute("TitleColor")]
        public string TitleColor
        {
            get { return _titleColor; }
            set { _titleColor = value; }
        }

        private static Color ParseColor(string nameColor)
        {
            if (Enum.IsDefined(typeof(KnownColor), nameColor))
                return Color.FromName(nameColor);
            else
                return Color.FromArgb(Int32.Parse(nameColor, System.Globalization.NumberStyles.AllowHexSpecifier));
            
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
            PropertyDescriptorCollection propColl = attributes == null
                                                        ? TypeDescriptor.GetProperties(this, true)
                                                        : TypeDescriptor.GetProperties(this, attributes, true);
            // фильтрация свойств в зависимости от типа источника
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
            {
                if (propertyDescriptor.Name.StartsWith("Cropping") &&
                    (!(Source is HardwareSource)) &&
                    (!(Source.GetType().ToString().EndsWith("VNCSourceDesign"))))
                    continue;
                if (propertyDescriptor.Category == "Параметры RGB" &&
                    (!(Source is HardwareSource)))
                    continue;
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [TypeConverter(typeof (PropertySorter))]
    public abstract class Equipment<T>
        where T : EquipmentType
    {
        private readonly List<Command> _commandList = new List<Command>();
        private T _type;

        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// !!!     ИСПОЛЬЗУЕТСЯ ТОЛЬКО ДЛЯ СЕРИАЛИЗАЦИИ В XML              !!! 
        /// !!! Увижу использование, кроме оговоренных случаев, ЯЙЦА ОТОРВУ !!! 
        /// !!! Имя атрибута соответствует имени свойства, которое          !!!
        /// !!! необходимо использовать                                     !!!
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [XmlAttribute("Type")]
        [Browsable(false)]
        public string EquipmentType { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public T Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                EquipmentType = _type.Name;
            }
        }

        [Category("Общие параметры")]
        [DisplayName("Плагин")]
        [XmlIgnore]
        public virtual string PluginName
        {
            get { return _type.Type; }
        }

        [Category("Общие параметры")]
        [DisplayName("Название")]
        [XmlIgnore]
        public virtual string Name
        {
            get { return _type.Name; }
        }

        [Category("Общие параметры")]
        [DisplayName("Тип оборудования")]
        [XmlIgnore]
        public virtual string Model
        {
            get { return _type.Model; }
        }

        [Category("Общие параметры")]
        [DisplayName("Комментарий")]
        [XmlIgnore]
        public virtual string Comment
        {
            get { return _type.Comment; }
        }

        [Browsable(false)]
        [XmlArray("CommandList")]
        public List<Command> CommandList
        {
            get { return _commandList; }
        }

        public override string ToString()
        {
            return String.Empty;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Link
    {
        private Slide _nextSlide;

        [XmlAttribute("IsDefault")]
        public bool IsDefault { get; set; }

        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// !!!     ИСПОЛЬЗУЕТСЯ ТОЛЬКО ДЛЯ СЕРИАЛИЗАЦИИ В XML              !!! 
        /// !!! Увижу использование, кроме оговоренных случаев, ЯЙЦА ОТОРВУ !!! 
        /// !!! Имя атрибута соответствует имени свойства, которое          !!!
        /// !!! необходимо использовать                                     !!!
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [XmlAttribute("NextSlide")]
        public int NextSlideId { get; set; }

        [XmlIgnore]
        public Slide NextSlide
        {
            get { return _nextSlide; }
            set
            {
                if (value == _nextSlide) return;
                _nextSlide = value;
                NextSlideId = _nextSlide.Id;
            }
        }
    }

    [Serializable]
    public class SlideLinkList
    {
        private readonly List<Link> _linkList = new List<Link>();

        [XmlArray("LinkList"), XmlArrayItem("Link")]
				//[TypeConverter("TechnicalServices.Common.TypeConverters.CollectionNameConverter, TechnicalServices.Common")]
        public List<Link> LinkList
        {
            get { return _linkList; }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Command
    {
        [XmlAttribute("command")]
        public string command { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class ActiveDisplay : Display
    {
        [Browsable(false)]
        [XmlAttribute]
        [Editor("UI.Common.CommonUI.Editor.BackgroundImageEditor,CommonUI", typeof (UITypeEditor))]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
            public string BackgroundImage { get; set; }

        [Browsable(false)]
        [XmlAttribute]
        public bool IsVideoWall { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Настройки")]
        [XmlIgnore]
        public string AgentUID
        {
            get { return ((DisplayTypeUriCapture)Type).AgentUID; }
        }


        protected override Window CreateWindowProtected(Source source, Slide slide)
        {
            return new Window();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class PassiveDisplay : Display
    {
        [Category("Общие параметры")]
        [DisplayName("Выход коммутатора")]
        [XmlIgnore]
        public int Output
        {
            get { return ((PassiveDisplayType) Type).Output; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum SlideState
    {
        Normal = 0,
        Edit,
        New,
        Delete
    }
}