using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

using Hosts.Plugins.Light.SystemModule.Design;

using TechnicalServices.Common;
using TechnicalServices.Common.Editor;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Light.SystemModule.Config
{
    [Serializable]
    [XmlType("Light")]
    public class LightDeviceConfig : DeviceType, ISupportValidation, ICollectionItemValidation
    {
        private List<LightUnitConfig> _unitList = new List<LightUnitConfig>();

        public LightDeviceConfig(string name)
            : this()
        {
            Name = name;
            Type = "Освещение";
        }

        public LightDeviceConfig()
        {
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
        }

        /// <summary>
        /// Группы освещения
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Группы освещения")]
        [XmlArray("LightUnitList")]
        [TypeConverter(typeof (CollectionNameConverter))]
        [Editor(typeof (LightUnitEditor), typeof (UITypeEditor))]
        [CollectionName("Группы освещения")]
        [CollectionFormName("Группы освещения")]
        [PropertiesName("Параметры группы освещения")]
        public List<LightUnitConfig> UnitList
        {
            get { return _unitList; }
            set { _unitList = value; }
        }

        #region ICollectionItemValidation Members

        bool ICollectionItemValidation.ValidateItem(out string errorMessage)
        {
            for (int i = 0; i < UnitList.Count; i++)
            {
                if (string.IsNullOrEmpty(UnitList[i].Name))
                {
                    errorMessage = "Среди групп освещения имеется группа с незаполненным названием.";
                    return false;
                }
                for (int j = 0; j < UnitList.Count; j++)
                {
                    if (i == j) continue;
                    if (UnitList[i].Name == UnitList[j].Name)
                    {
                        errorMessage = string.Format("В списке есть группы с одинаковым названием '{0}'.",
                                                     UnitList[i].Name);
                        return false;
                    }
                }
            }
            errorMessage = "OK";
            return true;
        }

        #endregion

        #region ISupportValidation Members

        bool ISupportValidation.EnsureValidate(out string errormessage)
        {
            for (int i = 0; i < UnitList.Count; i++)
            {
                if (string.IsNullOrEmpty(UnitList[i].Name))
                {
                    errormessage = "Среди групп освещения имеется группа с незаполненным названием.";
                    return false;
                }
            }
            errormessage = "OK";
            return true;
        }

        #endregion

        public override Device CreateNewDevice()
        {
            return new LightDeviceDesign {Type = this};
        }
    }

    internal class LightUnitEditor : ClonableObjectCollectionEditorAdv<LightUnitConfig>
    {
        public LightUnitEditor(Type type)
            : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            LightDeviceConfig device = (LightDeviceConfig)Context.Instance;
            if (device.UnitList.Count >= 100)
            {
                //throw new IndexOutOfRangeException("Количество групп освещения не может превышать 100");
                Control owner = Form.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
                MessageBox.Show(owner, "Количество групп освещения не может превышать 100", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            LightUnitConfig unit = (LightUnitConfig)base.CreateInstance(itemType);
            int num = device.UnitList.Count + 1;
            unit.Name = "Группа освещения " + num;
            return unit;
        }

        protected override void OnRemoveItem(LightUnitConfig sender)
        {
            LightDeviceConfig device = (LightDeviceConfig)Context.Instance;
            device.UnitList.Remove(device.UnitList.FirstOrDefault(i => i.Name == sender.Name));
        }

    }
}