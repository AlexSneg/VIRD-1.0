using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Xml.Serialization;
using Hosts.Plugins.AudioMixer.SystemModule.Config;
using TechnicalServices.Common.Editor;
using TechnicalServices.Common.ReadOnly;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Hosts.Plugins.AudioMixer.SystemModule.Design.TypeConverters;
using TechnicalServices.Persistence.CommonPersistence.Presentation;

namespace Hosts.Plugins.AudioMixer.SystemModule.Design
{
    [Serializable]
    [XmlType("AudioMixer")]
    public class AudioMixerDeviceDesign : Device, ICustomTypeDescriptor
    {
        /// <summary>
        /// Наличие матрицы логического матричного микшера 
        /// Получаем из Конфигуратора
        /// По умолчанию: True
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Наличие матрицы")]
        [XmlIgnore]
        [TypeConverter(typeof(YesNoConverter))]
        public bool HasMatrix
        {
            get { return ((AudioMixerDeviceConfig)Type).HasMatrix; }
        }

        /// <summary>
        /// Входы матрицы логического матричного микшера
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Входы")]
        [CollectionName("Входы матричного микшера")]
        [PropertiesNameAttribute("Параметры входов")]
        [XmlIgnore]
        [MatrixRequired]
        public ReadOnlyCollection InputList
        {
            get { return new ReadOnlyCollection(((AudioMixerDeviceConfig)Type).InputList); }
        }

        /// <summary>
        /// Выходы матрицы логического матричного микшера
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Выходы")]
        [CollectionName("Выходы матричного микшера")]
        [PropertiesNameAttribute("Параметры выходов")]
        [XmlIgnore]
        [MatrixRequired]
        public ReadOnlyCollection OutputList
        {
            get { return new ReadOnlyCollection(((AudioMixerDeviceConfig)Type).OutputList); }
        }

        /// <summary>
        /// Состояние ячеек матрицы
        /// </summary>
        [Browsable(false)]
        [XmlArray("AudioMixerMatrix")]
				public List<AudioMixerMatrixUnit> Matrix
        {
            get { return _matrix; }
            set { _matrix = value; }
        }
        private List<AudioMixerMatrixUnit> _matrix = new List<AudioMixerMatrixUnit>();

        /// <summary>
        /// Получить состояние ячейки матрицы
        /// Выключенные ячейки в коллекции не хранятся
        /// </summary>
        /// <param name="input">вход</param>
        /// <param name="output">выход</param>
        /// <returns>состояние (вкл/выкл)</returns>
        public bool GetMatrixUnit(AudioMixerInput input, AudioMixerOutput output)
        {
            AudioMixerMatrixUnit unit = Matrix.FirstOrDefault(s => (s.Input == input.Name) && (s.Output == output.Name));
            return unit != null ? unit.OnOffState : false;
        }

        /// <summary>
        /// Установить состояние ячейки матрицы
        /// Выключенные ячейки в коллекции не хранятся
        /// </summary>
        /// <param name="input">вход</param>
        /// <param name="output">выход</param>
        /// <param name="state">новое состояние</param>
        public void SetMatrixUnit(AudioMixerInput input, AudioMixerOutput output, bool state)
        {
            AudioMixerMatrixUnit unit = Matrix.FirstOrDefault(s => (s.Input == input.Name) && (s.Output == output.Name));
            if (state)
            {
                if (unit != null)
                    unit.OnOffState = state;
                else
                    Matrix.Add(new AudioMixerMatrixUnit(input, output, state));
            }
            else
            {
                if (unit != null)
                    Matrix.Remove(unit);
            }
        }

        /// <summary>
        /// Группы фейдеров
        /// Получаем из Конфигуратора
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Группы фейдеров")]
        [XmlArray("AudioMixerFaderGroupList")]
        [CollectionName("Группы фейдеров")]
        [CollectionFormName("Параметры групп фейдеров")]
        [PropertiesNameAttribute("Параметры групп фейдеров")]
        [TypeConverter(typeof(CollectionNameConverter))]
        [Editor(typeof(ReadOnlyAudioMixerFaderGroupDesignEditor), typeof(UITypeEditor))]
        public List<AudioMixerFaderGroupDesign> FaderGroupList
        {
            get
            {
                if (_isFaderGroupSyncronized || Type == null) return _faderGroupList;

                List<AudioMixerFaderGroupDesign> temp = new List<AudioMixerFaderGroupDesign>();
                foreach (AudioMixerFaderGroupConfig groupConfig in ((AudioMixerDeviceConfig)Type).FaderGroupList)
                {
                    AudioMixerFaderGroupDesign groupDesign = new AudioMixerFaderGroupDesign(groupConfig);
                    temp.Add(groupDesign);
                    // синхронизация элементов после десериализации
                    AudioMixerFaderGroupDesign groupSync = _faderGroupList.FirstOrDefault(s => s.Name == groupDesign.Name);
                    if (groupSync != null)
                        foreach (AudioMixerFaderDesign faderSync in groupSync.FaderList)
                        {
                            AudioMixerFaderDesign faderDesign = groupDesign.FaderList.FirstOrDefault(s => s.InstanceID == faderSync.InstanceID);
                            if (faderDesign != null)
                            {
                                faderDesign.Mute = faderSync.Mute;
                                faderDesign.BandValue = faderSync.BandValue;
                            }
                        }
                }
                _isFaderGroupSyncronized = true;
                _faderGroupList = temp;
                return _faderGroupList;
            }
            set { _faderGroupList = value; }
        }
        private List<AudioMixerFaderGroupDesign> _faderGroupList = new List<AudioMixerFaderGroupDesign>();
        private bool _isFaderGroupSyncronized;

        [Category("Настройки")]
        [DisplayName("Настройки сцены")]
        [XmlIgnore]
        [Editor(typeof(TypeEditors.SceneEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(SceneConverter))]
        public string Scene
        {
            get;
            set;
        }

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection propColl;
            if (attributes == null)
                propColl = TypeDescriptor.GetProperties(this, true);
            else
                propColl = TypeDescriptor.GetProperties(this, attributes, true);
            // в зависимости от наличия матрицы доступны различные свойства
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
            {
                MatrixRequiredAttribute attr = (MatrixRequiredAttribute)propertyDescriptor.Attributes[typeof(MatrixRequiredAttribute)];
                if (HasMatrix || (attr == null))
                    newColl.Add(propertyDescriptor);
            }
            return new PropertyDescriptorCollection(newColl.ToArray(), true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }

    internal class ReadOnlyAudioMixerFaderGroupDesignEditor : ReadOnlyCollectionEditorAdv<AudioMixerFaderGroupDesign>
    {
        public ReadOnlyAudioMixerFaderGroupDesignEditor(Type type) : base(type)
        {
        }
    }
}