using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using System.ComponentModel;

namespace TechnicalServices.Persistence.SystemPersistence.Presentation
{
    [Serializable]
    [XmlRoot(Namespace = "urn:slidebulk-schema")]
    public class SlideBulk
    {
        private readonly List<Slide> _slideList = new List<Slide>();
        private XmlSerializableDictionary<int, SlideLinkList> _linkDictionary =
            new XmlSerializableDictionary<int, SlideLinkList>();
        private XmlSerializableDictionary<int, Point> _slidePositionList =
            new XmlSerializableDictionary<int, Point>();


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

        /// <summary>
        /// должно вызываться после десериализации для инициализации ссылочных полей
        /// </summary>
        /// <returns>возвращается список оборудования которое отсутсвует в конфигурации и таким образом было исключено из сценария</returns>
        public string[] InitReference(ModuleConfiguration config, ResourceDescriptor[] descriptors,
            DeviceResourceDescriptor[] deviceResourceDescriptors)
        {
            foreach (KeyValuePair<int, SlideLinkList> pair in _linkDictionary)
            {
                foreach (Link link in pair.Value.LinkList)
                {
                    link.NextSlide = SlideList.Find(sl => sl.Id == link.NextSlideId);
                    if (link.NextSlide == null)
                        throw new KeyNotFoundException("SlideBulk.InitReference: Нет такого слайда в коллекции");
                }
            }

            // Initreference для каждого слайда
            List<string> deletedEquipment = new List<string>();
            foreach (Slide sl in SlideList)
            {
                deletedEquipment.AddRange(sl.InitReference(config, descriptors, deviceResourceDescriptors, true));
            }
            return deletedEquipment.Distinct().ToArray();
        }
    }
}
