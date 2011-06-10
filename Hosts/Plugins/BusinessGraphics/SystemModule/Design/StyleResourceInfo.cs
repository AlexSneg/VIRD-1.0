using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using dotnetCHARTING.WinForms;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using TechnicalServices.Interfaces;
using System.Xml.Serialization;
using Hosts.Plugins.BusinessGraphics.UI.Controls;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    [Serializable]
    [DataContract]
    public class StyleResourceInfo : ResourceInfo, INonVisibleResource
    {
        [NonSerialized]
        Dictionary<string, StyleInfo> _styles = null;  

        [XmlIgnore]
        Dictionary<string, StyleInfo> styles
        {
            get
            {
                if (_styles == null)
                {
                    _styles = new Dictionary<string, StyleInfo>();
                    using (MemoryStream ms = new MemoryStream(StylesSerialized))
                    {
                        BinaryFormatter bf = new BinaryFormatter();

                        List<StyleInfo> info = bf.Deserialize(ms) as List<StyleInfo>;

                        if (info != null)
                        {
                            _styles = info.ToDictionary(i => i.Name, i => i);
                        }
                    }
                }
                return _styles;
            }
            set
            {
                _styles = value;
            }
        }

        public StyleResourceInfo()
            : base()
        {
        }

        [DataMember]
        private byte[] _stylesSerialized = null;

        //[DataMember]
        [XmlAttribute("StylesSerialized", DataType = "base64Binary")]
        public byte[] StylesSerialized
        {
            get
            {
                if (_stylesSerialized == null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(ms, styles.Values.ToList());
                        _stylesSerialized = ms.ToArray();
                    }
                }
                return _stylesSerialized;
            }
            set
            {
                _stylesSerialized = value;
            }
        }

        public void UpdateStyles()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, styles.Values.ToList());
                _stylesSerialized = ms.ToArray();
            }
        }

        [DataMember]
        [Browsable(false)]
        [XmlAttribute("Type")]
        public override string Type
        {
            get
            {
                return StyleResourceInfo.GetResourceType();
            }
            set
            {
            }
        }

        public static string GetResourceType()
        {
            return "StyleInfo";
        }

        public void LoadStyle(string styleName, CustomChart chart)
        {
            if (styleName == null) return; // Иногда валится исключение
            if (styles.ContainsKey(styleName))
                styles[styleName].ApplyTo(chart);
        }

        /// <summary>
        /// Получить данные стиля по его имени.
        /// </summary>
        /// <param name="styleName">Имя стиля.</param>
        /// <returns>Настройки стиля.</returns>
        public string GetStyleData(string styleName)
        {
            if (styleName == null) return null;
            if (styles.ContainsKey(styleName))
                return styles[styleName].Data;
            else return null;
        }

        /// <summary>
        /// Получить тип диаграммы для стиля.
        /// </summary>
        /// <param name="styleName">Имя стиля.</param>
        public DiagramTypeEnum GetDiagramType(string styleName)
        {
            if (styleName == null) return DiagramTypeEnum.ColumnDetail;
            if (styles.ContainsKey(styleName))
                return styles[styleName].Type;
            else
                return DiagramTypeEnum.ColumnDetail;
        }

        public void SaveStyle(string styleName, CustomChart chart, DiagramTypeEnum type)
        {
            if (styles.ContainsKey(styleName))
                styles[styleName].ApplyFrom(chart);
            else
            {
                styles.Add(styleName, new StyleInfo(chart) { Name = styleName, Type=type });
            }
        }

        public void RemoveStyle(string styleName)
        {
            if (styles.ContainsKey(styleName))
                styles.Remove(styleName);
        }

        internal IEnumerable<string> GetStyleNames()
        {
            return styles.Keys;
        }

    }

    [Serializable]
    public class StyleInfo
    {
        public StyleInfo()
        {
        }

        public StyleInfo(CustomChart chart)
            : this()
        {
            ApplyFrom(chart);
        }

        public DiagramTypeEnum Type{get; set; }


        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _data;

        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        //получить свойства из chart в Data
        internal void ApplyFrom(CustomChart chart)
        {
            Data = chart.SaveStyle();
        }

        //присвоить свойства из Data в chart
        internal  void ApplyTo(CustomChart chart)
        {
            chart.LoadStyle(Data);
        }
    }
}
