using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TechnicalServices.Configuration.Server
{
    public static class UserStorageExt
    {
        internal static UserStorage LoadStorage(string file)
        {
            string xmlPath = file;
            string xsdPath = Path.ChangeExtension(file, "xsd");

            XmlSerializer serializer = new XmlSerializer(typeof (UserStorage));
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add("urn:userstorage-schema", xsdPath);
            //settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            using (XmlReader reader = XmlReader.Create(xmlPath, settings))
            {
                    return (UserStorage) serializer.Deserialize(reader);
            }
            


        }

        internal static void SaveStorage(this UserStorage list, string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                XmlSerializer serializer = new XmlSerializer(typeof (UserStorage));
                serializer.Serialize(writer, list);
            }
        }
    }
}