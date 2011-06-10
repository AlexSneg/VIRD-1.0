using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using TechnicalServices.Exceptions;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Configuration.Configurator
{
    public static class ModuleConfigurationExtenstion
    {
        public static void Save(this ModuleConfiguration moduleConfiguration, string fileName, List<Type> displayList,
                                List<Type> deviceList, List<Type> sourceList, List<Type> mappingList)
        {
            XmlAttributeOverrides ovr = new XmlAttributeOverrides();

            XmlAttributes attrsDevice = new XmlAttributes();
            XmlAttributes attrsSource = new XmlAttributes();
            XmlAttributes attrsDisplay = new XmlAttributes();
            XmlAttributes attrsMapping = new XmlAttributes();

            attrsMapping.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof (Mapping)));

            foreach (Type type in sourceList)
                attrsSource.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
            foreach (Type type in deviceList)
                attrsDevice.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
            foreach (Type type in displayList)
                attrsDisplay.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
            foreach (Type type in mappingList)
                attrsMapping.XmlArrayItems.Add(new XmlArrayItemAttribute(type));

            ovr.Add(typeof (ModuleConfiguration), "DeviceList", attrsDevice);
            ovr.Add(typeof (ModuleConfiguration), "SourceList", attrsSource);
            ovr.Add(typeof (ModuleConfiguration), "DisplayList", attrsDisplay);
            ovr.Add(typeof (DisplayType), "MappingList", attrsMapping);

            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Indent = true;

            XmlSerializer serializer = new XmlSerializer(typeof (ModuleConfiguration), ovr, new Type[] {}, null,
                                                         "urn:configuration-schema");
            using (XmlWriter writer = XmlWriter.Create(fileName, setting))
            {
                serializer.Serialize(writer, moduleConfiguration);
            }
        }

        public static ModuleConfiguration Load(string fileName, List<Type> displayList, List<Type> deviceList,
                                               List<Type> sourceList, List<Type> mappingList)
        {
            string configurationSchemaFile = Path.ChangeExtension(fileName, "xsd");
            try
            {
                XmlAttributeOverrides ovr = new XmlAttributeOverrides();

                XmlAttributes attrsDevice = new XmlAttributes();
                XmlAttributes attrsSource = new XmlAttributes();
                XmlAttributes attrsDisplay = new XmlAttributes();
                XmlAttributes attrsMapping = new XmlAttributes();

                attrsMapping.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof (Mapping)));

                foreach (Type type in sourceList)
                    attrsSource.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                foreach (Type type in deviceList)
                    attrsDevice.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                foreach (Type type in displayList)
                    attrsDisplay.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                foreach (Type type in mappingList)
                    attrsMapping.XmlArrayItems.Add(new XmlArrayItemAttribute(type));

                ovr.Add(typeof (ModuleConfiguration), "DeviceList", attrsDevice);
                ovr.Add(typeof (ModuleConfiguration), "SourceList", attrsSource);
                ovr.Add(typeof (ModuleConfiguration), "DisplayList", attrsDisplay);
                ovr.Add(typeof (DisplayType), "MappingList", attrsMapping);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add("urn:configuration-schema", configurationSchemaFile);
                // Проводим валидацию, отдельно от десереализации, потому-что если вместе то валится
                using (XmlReader reader = XmlReader.Create(fileName, settings))
                    while (reader.Read())
                    {
                    }
                // Проводим десереализации без валидации, потому-что если вместе то валится
                XmlSerializer serializer = new XmlSerializer(typeof (ModuleConfiguration), ovr);
                using (XmlReader reader = XmlReader.Create(fileName))
                    return (ModuleConfiguration) serializer.Deserialize(reader);
            }
            catch (FileNotFoundException ex)
            {
                throw new ModuleConfigurationException(ex.FileName);
            }
            catch (XmlSchemaException ex)
            {
                throw new ModuleConfigurationException(new Uri(ex.SourceUri).AbsolutePath, ex);    //configurationSchemaFile
            }
            catch (XmlException ex)
            {
                throw new ModuleConfigurationException(new Uri(ex.SourceUri).AbsolutePath, ex);
            }
        }

        public static string[] LoadSchema(string fileName)
        {
            List<string> result = new List<string>();
            fileName = Path.ChangeExtension(fileName, "xsd");
            using (StreamReader reader = new StreamReader(fileName))
            {
                XmlSchema xsd = XmlSchema.Read(reader, null);
                foreach (XmlSchemaInclude include in xsd.Includes)
                    result.Add(include.SchemaLocation.Replace(@"Config.xsd", String.Empty));
                result.Remove("Common");
            }
            return result.ToArray();
        }

        public static void SaveSchema(this ModuleConfiguration moduleConfiguration, string fileName, string[] list)
        {
            fileName = Path.ChangeExtension(fileName, "xsd");
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                XmlSchema xsd = new XmlSchema();
                xsd.Namespaces.Add("", @"urn:configuration-schema");
                xsd.Namespaces.Add("xsd", @"http://www.w3.org/2001/XMLSchema");
                xsd.Namespaces.Add("config", @"urn:configuration-schema");
                xsd.TargetNamespace = @"urn:configuration-schema";
                xsd.ElementFormDefault = XmlSchemaForm.Qualified;

                XmlSchemaInclude include = new XmlSchemaInclude();
                include.SchemaLocation = "CommonConfig.xsd";
                xsd.Includes.Add(include);
                foreach (string item in list)
                {
                    include = new XmlSchemaInclude();
                    include.SchemaLocation = item + @"Config.xsd";
                    xsd.Includes.Add(include);
                }
                xsd.Write(writer);
            }
        }
    }
}