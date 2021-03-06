﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TechnicalServices.Configuration.Common.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Module")]
        public string ModuleFolder {
            get {
                return ((string)(this["ModuleFolder"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ModuleConfiguration")]
        public string ConfigurationFolder {
            get {
                return ((string)(this["ConfigurationFolder"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Presentation")]
        public string ScenarioFolder {
            get {
                return ((string)(this["ScenarioFolder"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Presentation\\presentation.xsd")]
        public string ScenarioSchemaFile {
            get {
                return ((string)(this["ScenarioSchemaFile"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LocalSources")]
        public string LocalSourceFolder {
            get {
                return ((string)(this["LocalSourceFolder"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("GlobalSources")]
        public string GlobalSourceFolder {
            get {
                return ((string)(this["GlobalSourceFolder"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public int PingInterval {
            get {
                return ((int)(this["PingInterval"]));
            }
        }

        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ModuleConfiguration.xml")]
        public string ConfigurationFile {
            get {
                return ((string)(this["ConfigurationFile"]));
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DeviceResources")]
        public string DeviceResourceFolder
        {
            get
            {
                return ((string)(this["DeviceResourceFolder"]));
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DevicePositionList {
            get {
                return ((string)(this["DevicePositionList"]));
            }
        }

        public TechnicalServices.Entity.XmlSerializableDictionary<string, int> DevicePositions
        {
            get
            {
                if (_devicePositions == null) _devicePositions = GetDevicePositions();
                return _devicePositions;
            }
        }
        TechnicalServices.Entity.XmlSerializableDictionary<string, int> _devicePositions;

        private  TechnicalServices.Entity.XmlSerializableDictionary<string, int> GetDevicePositions()
        {
            string str = this.DevicePositionList;
            if (string.IsNullOrEmpty(str)) return new TechnicalServices.Entity.XmlSerializableDictionary<string,int>();
            System.IO.StringReader stringReader = new System.IO.StringReader(str);
            System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
            settings.ConformanceLevel = System.Xml.ConformanceLevel.Fragment;
            System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader, settings);
            TechnicalServices.Entity.XmlSerializableDictionary<string, int> dict = new TechnicalServices.Entity.XmlSerializableDictionary<string, int>();
            reader.Read();
            dict.ReadXml(reader);
            return dict;
        }

        private string GetDevicePositionsAsString()
        {
            if (this.DevicePositions.Count==null) return "";
            System.Text.StringBuilder builder = new System.Text.StringBuilder(); 
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.ConformanceLevel = System.Xml.ConformanceLevel.Fragment;
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(builder, settings);
            this.DevicePositions.WriteXml(writer);
            writer.Close();
            return "<root>" + builder.ToString() + "</root>";
        }
    }
}
