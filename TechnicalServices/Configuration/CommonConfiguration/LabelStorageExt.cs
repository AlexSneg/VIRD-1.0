using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Common
{
    public static class LabelStorageExt
    {
        internal static LabelStorage LoadStorage(string file, List<Label> systemLabels, IEventLogging logging)
        {
            string xmlPath = file;
            string xsdPath = Path.ChangeExtension(file, "xsd");

            XmlSerializer serializer = new XmlSerializer(typeof(LabelStorage));
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add("urn:labelstorage-schema", xsdPath);
            LabelStorage storage = null;
            //settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            using (XmlReader reader = XmlReader.Create(xmlPath, settings))
            {
                storage = (LabelStorage)serializer.Deserialize(reader);
            }
            // теперя надо проверить, что нет пользовательских меток, совпадающих с системными. если есть - пользовательские надо удалять
            List<Label> toRemove = storage.FindAll(
                label =>
                systemLabels.Any(
                    syslab =>
                    syslab.Id == label.Id || syslab.Name.Equals(label.Name, StringComparison.InvariantCultureIgnoreCase)));
            foreach (Label item in toRemove)
            {
                logging.WriteWarning(string.Format("Пользовательская метка <{0}> удалена ввиду конфликта названия или идентификатора с одноименной системной меткой", item.Name));
            }
            storage.RemoveAll(label=>toRemove.Any(tor => object.ReferenceEquals(tor, label)));
            // и оптяь сохраним
            storage.SaveStorage(file);
            return storage;
        }

        internal static void SaveStorage(this LabelStorage list, string file)
        {
            //отфильтровка системых меток
            LabelStorage labelStorage = new LabelStorage();
            labelStorage.AddRange(list.FindAll(x => x.IsSystem == false));
            
            using (StreamWriter writer = new StreamWriter(file))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LabelStorage));
                serializer.Serialize(writer, labelStorage);
            }
        }
    }
}
