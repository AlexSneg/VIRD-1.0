using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VDCServer.SystemModule.Design
{
    [DataContract]
    [Serializable]
    public class VDCServerResourceInfo : ResourceInfo
    {
        /// <summary>
        /// Справочник абонентов
        /// </summary>
        [DataMember]
        [Category("Настройки")]
        [DisplayName("Справочник абонентов")]
        [XmlArray("AbonentList", Namespace = "VDCServerResourceInfo")]
        [TypeConverter(typeof(CollectionNameConverter))]
        public List<VDCServerAbonentInfo> AbonentList
        {
            get { return _abonentList; }
            set { _abonentList = value; }
        }
        private List<VDCServerAbonentInfo> _abonentList = new List<VDCServerAbonentInfo>();

    }
}
