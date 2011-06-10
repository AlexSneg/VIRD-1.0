using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Common.Properties
{
    public sealed partial class Settings : ISystemParameters
    {

        private ConfigurationSectionGroup sectionGroup =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).RootSectionGroup.SectionGroups["applicationSettings"];//.
                                        //Sections["TechnicalServices.Configuration.Global.Properties.SystemParametersSettings"];

        public Settings()
        {

        }

        static public ISystemParameters SystemDefault
        {
            get { return Default; }
        }

        string ISystemParameters.ReloadImage
        {
            get
            {
                if (sectionGroup != null)
                {
                    ConfigurationSection section =
                        sectionGroup.Sections[
                            "TechnicalServices.Configuration.Global.Properties.SystemParametersSettings"];
                    SettingElement systemNameElement = ((ClientSettingsSection) section).Settings.Get("ReloadImage");
                    return systemNameElement.Value.ValueXml.InnerText;
                }
                return "";
            }
            set { }

        }

        string ISystemParameters.BackgroundPresentationUniqueName
        {
            get
            {
                if (sectionGroup != null)
                {
                    ConfigurationSection section =
                        sectionGroup.Sections[
                            "TechnicalServices.Configuration.Global.Properties.SystemParametersSettings"];
                    SettingElement systemNameElement =
                        ((ClientSettingsSection) section).Settings.Get("BackgroundPresentationUniqueName");
                    return systemNameElement.Value.ValueXml.InnerText;
                }
                return "";
            }
            set { }
    
        }


        string ISystemParameters.DefaultWndsize
        {
            get
            {
                if (sectionGroup != null)
                {
                    ConfigurationSection section =
                        sectionGroup.Sections[
                            "TechnicalServices.Configuration.Global.Properties.SystemParametersSettings"];
                    SettingElement systemNameElement = ((ClientSettingsSection) section).Settings.Get("DefaultWndsize");
                    return systemNameElement.Value.ValueXml.InnerText;
                }
                return "";
            }
            set { }
    
        }

        int ISystemParameters.BackgroundPresentationRestoreTimeout
        {
            get
            {
                if (sectionGroup != null)
                {
                    ConfigurationSection section =
                        sectionGroup.Sections[
                            "TechnicalServices.Configuration.Global.Properties.SystemParametersSettings"];
                    SettingElement systemNameElement = ((ClientSettingsSection)section).Settings.Get("BackgroundScenarioRestoreTimeOut");
                    int res;
                    if (Int32.TryParse(systemNameElement.Value.ValueXml.InnerText, out res))
                        return res;
                    else return 60;
                }
                return 60;
            }
            set { }

        }

    }
}
