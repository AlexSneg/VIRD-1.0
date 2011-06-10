using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Configuration.Common
{
    public static class ModuleConfigurationExtenstion
    {
        public static ModuleConfiguration Load(string fileName, string configurationSchemaFile, ICollection<IModule> moduleList)
        {
            try
            {
                XmlAttributeOverrides ovr = new XmlAttributeOverrides();

                XmlAttributes attrsDevice = new XmlAttributes();
                XmlAttributes attrsSource = new XmlAttributes();
                XmlAttributes attrsDisplay = new XmlAttributes();
                XmlAttributes attrsMapping = new XmlAttributes();

                attrsMapping.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof(Mapping)));
                foreach (IModule item in moduleList)
                {
                    foreach (Type type in item.SystemModule.Configuration.GetDevice())
                        attrsDevice.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                    foreach (Type type in item.SystemModule.Configuration.GetSource())
                        attrsSource.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                    foreach (Type type in item.SystemModule.Configuration.GetDisplay())
                        attrsDisplay.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                    foreach (Type type in item.SystemModule.Configuration.GetMappingType())
                        attrsMapping.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                }

                ovr.Add(typeof(ModuleConfiguration), "DeviceList", attrsDevice);
                ovr.Add(typeof(ModuleConfiguration), "SourceList", attrsSource);
                ovr.Add(typeof(ModuleConfiguration), "DisplayList", attrsDisplay);
                ovr.Add(typeof(DisplayType), "MappingList", attrsMapping);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add("urn:configuration-schema", configurationSchemaFile);
                // Проводим валидацию, отдельно от десереализации, потому-что если вместе то валится
                using (XmlReader reader = XmlReader.Create(fileName, settings))
                    while (reader.Read())
                    {
                    }
                // Проводим десереализации без валидации, потому-что если вместе то валится
                XmlSerializer serializer = new XmlSerializer(typeof(ModuleConfiguration), ovr);
                using (XmlReader reader = XmlReader.Create(fileName))
                    return (ModuleConfiguration)serializer.Deserialize(reader);
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
    }
}