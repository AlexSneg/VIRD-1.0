using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TechnicalServices.Entity
{
    [Serializable]
    //[DataContract]
    public class XmlSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        private const string _namespace = "urn:serializableDictionary";

        public XmlSerializableDictionary()
            : base()
        { }

        protected XmlSerializableDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        private const string ITEM = "item";
        private const string KEY = "key";
        private const string VALUE = "value";

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey), _namespace);
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue), _namespace);

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement(ITEM);
                reader.ReadStartElement(KEY);
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement(VALUE);
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey), _namespace);
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue), _namespace);
            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement(ITEM);
                writer.WriteStartElement(KEY);
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement(VALUE);
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
