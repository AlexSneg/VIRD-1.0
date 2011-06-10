using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using TechnicalServices.Common;
using TechnicalServices.Common.Editor;
using TechnicalServices.Common.ReadOnly;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.CommonPersistence.Presentation;

namespace Hosts.Plugins.AudioMixer.SystemModule.Config
{
    [Serializable]
    [XmlType("AudioMixerInput")]
    public class AudioMixerInput : ICloneable, ICollectionItemValidation
    {
        private int _index;
        private string _name;

        /// <summary>
        /// Название входа матрицы логического матричного микшера
        /// Обязательный параметр
        /// Название уникально среди всех входов
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название входа")]
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = ValidationHelper.CheckLength(value, 50, "названия входа"); }
        }

        /// <summary>
        /// Позиция входа матрицы логического матричного микшера
        /// Обязательный параметр
        /// Позиция уникальна среди всех входов
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Позиция входа")]
        [XmlAttribute("Index")]
        [Browsable(false)]
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public override string ToString()
        {
            return Name;
        }

        public bool ValidateItem(out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(Name))
            {
                errorMessage = "Название входа не может быть пустым";
                return false;
            }
            return true;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }


    [Serializable]
    [XmlType("AudioMixerOutput")]
    public class AudioMixerOutput : ICloneable, ICollectionItemValidation
    {
        private int _index;
        private string _name;

        /// <summary>
        /// Название выхода матрицы логического матричного микшера
        /// Обязательный параметр
        /// Название уникально среди всех выходов
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название выхода")]
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = ValidationHelper.CheckLength(value, 50, "названия выхода"); }
        }

        /// <summary>
        /// Позиция выхода матрицы логического матричного микшера
        /// Обязательный параметр
        /// Позиция уникальна среди всех выходов
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Позиция выхода")]
        [XmlAttribute("Index")]
        [Browsable(false)]
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public override string ToString()
        {
            return Name;
        }

        public bool ValidateItem(out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(Name))
            {
                errorMessage = "Название выхода не может быть пустым";
                return false;
            }
            return true;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }


    [Serializable]
    [XmlType("AudioMixerMatrixUnit")]
    public class AudioMixerMatrixUnit
    {
        public AudioMixerMatrixUnit(AudioMixerInput input, AudioMixerOutput output, bool state) : this()
        {
            Input = input.Name;
            Output = output.Name;
            OnOffState = state;
        }

        public AudioMixerMatrixUnit()
        {
        }

        /// <summary>
        /// Название входа матрицы логического матричного микшера
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("Input")]
        public string Input { get; set; }

        /// <summary>
        /// Название выхода матрицы логического матричного микшера
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("Output")]
        public string Output { get; set; }

        /// <summary>
        /// Состояние ячейки матрицы логического матричного микшера
        /// По умолчанию: False
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("OnOffState")]
        public bool OnOffState { get; set; }
    }


    [Serializable]
    [XmlType("AudioMixerFaderGroup")]
    public class AudioMixerFaderGroupConfig : ICloneable, ICollectionItemValidation
    {
        private List<AudioMixerFaderConfig> _faderList = new List<AudioMixerFaderConfig>();
        private string _name;

        /// <summary>
        /// Название группы фейдеров
        /// Обязательный параметр
        /// Название уникально среди всех групп фейдеров
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название группы")]
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = ValidationHelper.CheckLength(value, 50, "названия группы"); }
        }

        /// <summary>
        /// Фейдеры
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Фейдеры")]
        [XmlArray("AudioMixerFaderList")]
        [TypeConverter(typeof (CollectionNameConverter))]
        [Editor(typeof (AudioMixerFaderEditor), typeof (UITypeEditor))]
        [CollectionFormName("Фейдеры группы")]
        [CollectionName("Фейдеры")]
        [PropertiesName("Параметры фейдеров")]
        public List<AudioMixerFaderConfig> FaderList
        {
            get { return _faderList; }
            set { _faderList = value; }
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }

        public bool ValidateItem(out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(Name))
            {
                errorMessage = "Название группы не может быть пустым";
                return false;
            }
            for (int i = 0; i < FaderList.Count; i++)
            {

                for (int j = 0; j < FaderList.Count; j++)
                {
                    if (i == j) continue;
                    if (FaderList[i].InstanceID == FaderList[j].InstanceID)
                    {
                        errorMessage = string.Format("В списке есть фейдеры с одинаковым идентификатором '{0}'.",
                                                     FaderList[i].InstanceID);
                        return false;
                    }
                    if (FaderList[i].Name == FaderList[j].Name)
                    {
                        errorMessage = string.Format("В списке есть фейдеры с одинаковым именем '{0}'.",
                                                     FaderList[i].Name);
                        return false;
                    }
                }
            }

            return true;
        }
    }

    internal class AudioMixerFaderEditor : ClonableObjectCollectionEditorAdv<AudioMixerFaderConfig>
    {
        public AudioMixerFaderEditor(Type type)
            : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            AudioMixerFaderGroupConfig item = (AudioMixerFaderGroupConfig)Context.Instance;
            if (item.FaderList.Count >= 100)
            {
                //throw new IndexOutOfRangeException("Количество фейдеров не может превышать 100");
                Control owner = Form.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
                MessageBox.Show(owner, "Количество фейдеров не может превышать 100", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            
            AudioMixerFaderConfig unit = (AudioMixerFaderConfig)base.CreateInstance(itemType);
            unit.InstanceID = (item.FaderList.Count + 1).ToString();
            unit.Name = "Фейдер " + (item.FaderList.Count + 1);
            return unit;
        }

        protected override void OnRemoveItem(AudioMixerFaderConfig sender)
        {
            AudioMixerFaderGroupConfig item = (AudioMixerFaderGroupConfig)Context.Instance;
            item.FaderList.Remove(item.FaderList.FirstOrDefault(i => i.Name == sender.Name));
        }

    }

    [Serializable]
    [XmlType("AudioMixerFaderGroup")]
    public class AudioMixerFaderGroupDesign
    {
        private List<AudioMixerFaderDesign> _faderList = new List<AudioMixerFaderDesign>();

        public AudioMixerFaderGroupDesign(AudioMixerFaderGroupConfig config)
        {
            Name = config.Name;
            foreach (AudioMixerFaderConfig faderConfig in config.FaderList)
                FaderList.Add(new AudioMixerFaderDesign(faderConfig));
        }

        public AudioMixerFaderGroupDesign()
        {
        }

        /// <summary>
        /// Название группы фейдеров
        /// Обязательный параметр
        /// Название уникально среди всех групп фейдеров
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название группы")]
        [ReadOnly(true)]
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Фейдеры
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Фейдеры")]
        [XmlArray("AudioMixerFaderList")]
        [CollectionFormName("Параметры фейдеров")]
        [TypeConverter(typeof (CollectionNameConverter))]
        [Editor(typeof(ReadOnlyAudioMixerFaderDesignEditor), typeof(UITypeEditor))]
        public List<AudioMixerFaderDesign> FaderList
        {
            get { return _faderList; }
            set { _faderList = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    internal class ReadOnlyAudioMixerFaderDesignEditor : ReadOnlyCollectionEditorAdv<AudioMixerFaderDesign>
    {
        public ReadOnlyAudioMixerFaderDesignEditor(Type type) : base(type)
        {
        }
    }


    [Serializable]
    [XmlType("AudioMixerFader")]
    public class AudioMixerFaderConfig : ICloneable, ICollectionItemValidation
    {
        private string _instanceID;
        private string _name;

        /// <summary>
        /// Instance ID фейдера
        /// Обязательный параметр
        /// Уникально внутри экземпляра аудиомикшера
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Instance ID")]
        [XmlAttribute("InstanceID")]
        public string InstanceID
        {
            get { return _instanceID; }
            set { _instanceID = ValidationHelper.CheckLength(value, 50, "Instance ID"); }
        }

        /// <summary>
        /// Диапазон изменения значений фейдера
        /// Обязательный параметр
        /// По умолчанию: "0..100%"
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Диапазон")]
        [DefaultValue(FaderBandEnum.Full)]
        [XmlAttribute("Band")]
        [TypeConverter(typeof (CommonEnumConverter))]
        public FaderBandEnum Band { get; set; }

        /// <summary>
        /// Название фейдера
        /// Обязательный параметр
        /// Название уникально внутри группы фейдеров
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название")]
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = ValidationHelper.CheckLength(value, 50, "названия фейдера"); }
        }

        /// <summary>
        /// Отображать фейдер в окне оперативного управления
        /// Обязательный параметр
        /// По умолчанию: False
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Оперативное управление")]
        [DefaultValue(false)]
        [XmlAttribute("HasOnlineControl")]
        [TypeConverter(typeof (YesNoConverter))]
        public bool HasOnlineControl { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }

        public bool ValidateItem(out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(InstanceID) || string.IsNullOrEmpty(Name))
            {
                errorMessage = "Instance ID и Название фейдера не могут быть пустыми";
                return false;
            }
            return true;
        }
    }


    [Serializable]
    [XmlType("AudioMixerFader")]
    public class AudioMixerFaderDesign
    {
        private int _bandValue;

        public AudioMixerFaderDesign(AudioMixerFaderConfig config) : this()
        {
            InstanceID = config.InstanceID;
            Band = config.Band;
            Name = config.Name;
            HasOnlineControl = config.HasOnlineControl;
        }

        public AudioMixerFaderDesign(AudioMixerFaderDesign copy)
            : this()
        {
            InstanceID = copy.InstanceID;
            Band = copy.Band;
            Name = copy.Name;
            HasOnlineControl = copy.HasOnlineControl;
            Mute = copy.Mute;
            BandValue = copy.BandValue;
        }

        public AudioMixerFaderDesign()
        {
        }

        /// <summary>
        /// Instance ID фейдера
        /// Обязательный параметр
        /// Уникально внутри экземпляра аудиомикшера
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("InstanceID")]
        public string InstanceID { get; set; }

        /// <summary>
        /// Диапазон изменения значений фейдера
        /// Обязательный параметр
        /// По умолчанию: "0..100%"
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Диапазон")]
        [ReadOnly(true)]
        [XmlIgnore]
        [TypeConverter(typeof (CommonEnumConverter))]
        public FaderBandEnum Band { get; set; }

        /// <summary>
        /// Название фейдера
        /// Обязательный параметр
        /// Название уникально внутри группы фейдеров
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название")]
        [ReadOnly(true)]
        [XmlIgnore]
        public string Name { get; set; }

        /// <summary>
        /// Отображать фейдер в окне оперативного управления
        /// Обязательный параметр
        /// По умолчанию: False
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Оперативное управление")]
        [ReadOnly(true)]
        [XmlIgnore]
        [TypeConverter(typeof (YesNoConverter))]
        public bool HasOnlineControl { get; set; }

        /// <summary>
        /// Заглушить / не заглушать
        /// Обязательный параметр
        /// По умолчанию: False
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Mute")]
        [DefaultValue(false)]
        [XmlAttribute("Mute")]
        [TypeConverter(typeof (YesNoConverter))]
        public bool Mute { get; set; }

        /// <summary>
        /// Текущее значение (в рамках диапазона)
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Текущее значение")]
        [XmlAttribute("BandValue")]
        public int BandValue
        {
            get { return _bandValue; }
            set { _bandValue = (value < LowerBand ? LowerBand : (value > UpperBand ? UpperBand : value)); }
        }

        [XmlIgnore]
        public int BandValueNormalize
        {
            get { return GetBandValueNormalize(_bandValue); }
            set { _bandValue = GetBandValueDeNormalize(value); }
        }

        public int LowerBand
        {
            get
            {
                switch (Band)
                {
                    case FaderBandEnum.Band_60_20:
                    case FaderBandEnum.Band_60_0:
                        return -60;
                    case FaderBandEnum.Band_15:
                        return -15;
                    case FaderBandEnum.Band_10:
                        return -10;
                    case FaderBandEnum.Band_5:
                        return -5;
                    case FaderBandEnum.Full:
                        return 0;
                    default:
                        return int.MinValue;
                }
            }
        }

        public int UpperBand
        {
            get
            {
                switch (Band)
                {
                    case FaderBandEnum.Band_60_20:
                        return 20;
                    case FaderBandEnum.Band_60_0:
                        return 0;
                    case FaderBandEnum.Band_15:
                        return 15;
                    case FaderBandEnum.Band_10:
                        return 10;
                    case FaderBandEnum.Band_5:
                        return 5;
                    case FaderBandEnum.Full:
                        return 100;
                    default:
                        return int.MaxValue;
                }
            }
        }

        public FaderUnitEnum Unit
        {
            get
            {
                switch (Band)
                {
                    case FaderBandEnum.Full:
                        return FaderUnitEnum.Percent;
                    default:
                        return FaderUnitEnum.Decibel;
                }
            }
        }

        public string UnitString
        {
            get
            {
                object[] attrs = typeof (FaderUnitEnum).GetField(Unit.ToString()).GetCustomAttributes(true);
                foreach (object item in attrs)
                {
                    DescriptionAttribute attr = item as DescriptionAttribute;
                    if (attr != null)
                        return attr.Description;
                }
                return string.Empty;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public int GetBandValueNormalize(int bandValue)
        {
            double value = bandValue;
            if (Unit == FaderUnitEnum.Decibel)
                value = 100d*(bandValue - LowerBand)/(UpperBand - LowerBand);
            return (int) Math.Round(value);
        }

        public int GetBandValueDeNormalize(int bandValue)
        {
            double level = bandValue;
            if (Unit == FaderUnitEnum.Decibel)
                level = (double) bandValue*(UpperBand - LowerBand)/100 + LowerBand;
            return (int) Math.Round(level);
        }
    }

    public enum FaderUnitEnum
    {
        [Description("%")] Percent,
        [Description("")] Decibel
    }

    public enum FaderBandEnum
    {
        [Description("0..100%")] Full,
        [Description("60..+20 дБ")] Band_60_20,
        [Description("-60..0 дБ")] Band_60_0,
        [Description("-15..+15 дБ")] Band_15,
        [Description("-10..+10 дБ")] Band_10,
        [Description("-5..+5 дБ")] Band_5
    }
}