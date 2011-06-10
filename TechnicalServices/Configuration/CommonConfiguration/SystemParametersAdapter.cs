using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
//using TechnicalServices.Configuration..Server;
//using TechnicalServices.Configuration.Server.Properties;
using TechnicalServices.Configuration.Common.Properties;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Common
{
    public class SystemParametersAdapter: ISystemParametersAdapter
    {
        public ISystemParameters LoadSystemParameters()
        {
            SystemParameters systemParameters = new SystemParameters();

            ConfigurationSection section = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).
                                        RootSectionGroup.SectionGroups["applicationSettings"].
                                        Sections["TechnicalServices.Configuration.Global.Properties.SystemParametersSettings"];
            
            //SettingElement systemNameElement = ((ClientSettingsSection)section).Settings.Get("SystemName");
            //if (systemNameElement != null)
            //    systemParameters.SystemName = systemNameElement.Value.ValueXml.InnerText;

            SettingElement reloadImageElement = ((ClientSettingsSection)section).Settings.Get("ReloadImage");
            if (reloadImageElement != null)
                systemParameters.ReloadImage = reloadImageElement.Value.ValueXml.InnerText;

            SettingElement backgroundPresentationElement = ((ClientSettingsSection)section).Settings.Get("BackgroundPresentationUniqueName");
            if (backgroundPresentationElement != null)
                systemParameters.BackgroundPresentationUniqueName = backgroundPresentationElement.Value.ValueXml.InnerText;

            SettingElement defaultWndsizeElement = ((ClientSettingsSection)section).Settings.Get("DefaultWndsize");
            if (defaultWndsizeElement != null)
                systemParameters.DefaultWndsize = defaultWndsizeElement.Value.ValueXml.InnerText;

            SettingElement defaultBkgPresRestoreTimeout = ((ClientSettingsSection)section).Settings.Get("BackgroundScenarioRestoreTimeOut");
            if (defaultBkgPresRestoreTimeout != null)
                systemParameters.BackgroundPresentationRestoreTimeout = Int32.Parse(defaultBkgPresRestoreTimeout.Value.ValueXml.InnerText);

            return systemParameters;
        }

        public void SaveSystemParameters(ISystemParameters systemParameters)
        {
            try
            {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //configFile,
                ConfigurationSection section =
                    config.RootSectionGroup.SectionGroups["applicationSettings"].Sections[
                        "TechnicalServices.Configuration.Global.Properties.SystemParametersSettings"];

                //SettingElement systemNameElement = ((ClientSettingsSection) section).Settings.Get("SystemName");
                //if (systemNameElement != null)
                //    systemNameElement.Value.ValueXml.InnerText = systemParameters.SystemName;

                SettingElement reloadImageElement = ((ClientSettingsSection) section).Settings.Get("ReloadImage");
                if (reloadImageElement != null)
                    reloadImageElement.Value.ValueXml.InnerText = systemParameters.ReloadImage;

                SettingElement backgroundPresentationElement =
                    ((ClientSettingsSection) section).Settings.Get("BackgroundPresentationUniqueName");
                if (backgroundPresentationElement != null)
                    backgroundPresentationElement.Value.ValueXml.InnerText =
                        systemParameters.BackgroundPresentationUniqueName;

                SettingElement defaultWndsizeElement = ((ClientSettingsSection) section).Settings.Get("DefaultWndsize");
                if (defaultWndsizeElement != null)
                    defaultWndsizeElement.Value.ValueXml.InnerText = systemParameters.DefaultWndsize;

                SettingElement defaultBkgPresRestoreTimeout = ((ClientSettingsSection)section).Settings.Get("BackgroundScenarioRestoreTimeOut");
                if (defaultBkgPresRestoreTimeout != null)
                    defaultBkgPresRestoreTimeout.Value.ValueXml.InnerText = systemParameters.BackgroundPresentationRestoreTimeout.ToString();

                section.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Full);
                section.SectionInformation.ForceSave = false;

                ConfigurationManager.RefreshSection("TechnicalServices.Configuration.Global.Properties.SystemParametersSettings");
            }
            catch (Exception ex)
            {
                throw new SystemParametersSaveException(ex.Message);
            }
            
        }
    }
}
