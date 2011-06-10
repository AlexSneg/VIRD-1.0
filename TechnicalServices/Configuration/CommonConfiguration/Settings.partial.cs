using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;

namespace TechnicalServices.Configuration.Common.Properties
{
    public sealed partial class Settings
    {
        #region Set-ры для изменяемых свойств
        //специально сделал такое извращение с Set свойствами, т.к. иначе бы пришлось 
        //свойства из ApplicationSetting превращать в UserSetting, 
        //но в этом случае свойсва в app.config переедут в другую секцию и перестанут зачитываться автоматом
        public string ScenarioFolderSet
        {
            set
            {
                this["ScenarioFolder"] = value;
            }
        }
        public string ConfigurationFolderSet
        {
            set
            {
                this["ConfigurationFolder"] = value;
            }
        }
        public string LocalSourceFolderSet
        {
            set
            {
                this["LocalSourceFolder"] = value;
            }
        }
        public string GlobalSourceFolderSet
        {
            set
            {
                this["GlobalSourceFolder"] = value;
            }
        }
        public string ConfigurationFileSet
        {
            set
            {
                this["ConfigurationFile"] = value;
            }
        }

        public string DevicePositionListSet
        {
            set
            {
                this["DevicePositionList"] = value;
            }
        }
        #endregion

        /// <summary>переопределил этот метод, т.к. народ пожелал свойства сохранять в конфиге приложения </summary>
        public override void Save()
        {
            Type thisType = this.GetType();
            PropertyInfo[] properties = thisType.GetProperties();
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ClientSettingsSection sectionProperty = (ClientSettingsSection)config.RootSectionGroup.SectionGroups["applicationSettings"].Sections[thisType.Namespace + ".Settings"];
            //пробегусь по всем пропертям в конфиг файле, и перепишу в них новые значения
            foreach (SettingElement item in sectionProperty.Settings)
            {
                try
                {
                    PropertyInfo property = properties.First(prop => prop.Name.Equals(item.Name));
                    if (item.Name == "DevicePositionList")
                    {
                        item.Value.ValueXml.InnerText = this.GetDevicePositionsAsString();
                    }
                    else
                    {
                        item.Value.ValueXml.InnerText = property.GetValue(this, null).ToString();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    //TODO: нашел секцию в конфиге которой нет в CommonConfiguration
                    //либо она лишняя, либо забыли продублировать
                }
            }
            sectionProperty.SectionInformation.ForceSave = true;
            config.Save();
            sectionProperty.SectionInformation.ForceSave = false;
        }
    }
}
