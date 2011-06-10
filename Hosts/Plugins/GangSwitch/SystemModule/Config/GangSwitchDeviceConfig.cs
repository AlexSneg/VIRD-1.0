using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Xml.Serialization;

using Hosts.Plugins.GangSwitch.SystemModule.Design;

using TechnicalServices.Common;
using TechnicalServices.Common.Editor;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.GangSwitch.SystemModule.Config
{
    [Serializable]
    [XmlType("GangSwitch")]
    public class GangSwitchDeviceConfig : DeviceType, ISupportValidation, ICollectionItemValidation
    {
        private List<GangSwitchUnitConfig> _unitList = new List<GangSwitchUnitConfig>();

        public GangSwitchDeviceConfig(string name) : this()
        {
            Name = name;
            Type = "Логический блок переключателей";
        }

        public GangSwitchDeviceConfig()
        {
        }

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
        }

        /// <summary>
        /// Переключатели
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Переключатели")]
        [XmlArray("GangSwitchUnitList")]
        [TypeConverter(typeof (CollectionNameConverter))]
        [Editor(typeof (GangSwitchUnitEditor), typeof (UITypeEditor))]
        [CollectionName("Переключатели")]
        [CollectionFormName("Переключатели")]
        [PropertiesName("Параметры переключателей")]
        public List<GangSwitchUnitConfig> UnitList
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
                    errorMessage = "Среди переключателей имеется переключатель с незаполненным названием.";
                    return false;
                }
                if (string.IsNullOrEmpty(UnitList[i].OffStateName))
                {
                    errorMessage = string.Format("У переключателя \"{0}\" не заполнено название состояния \"Выкл.\".",
                                                 UnitList[i].Name);
                    return false;
                }
                if (string.IsNullOrEmpty(UnitList[i].OnStateName))
                {
                    errorMessage = string.Format("У переключателя \"{0}\" не заполнено название состояния \"Вкл.\".",
                                                 UnitList[i].Name);
                    return false;
                }
                if (UnitList[i].OnStateName == UnitList[i].OffStateName)
                {
                    errorMessage =
                        string.Format(
                            "У переключателя \"{0}\" название состояния \"Вкл.\" совпадает с название состояния \"Выкл.\".",
                            UnitList[i].Name);
                    return false;
                }
                for (int j = 0; j < UnitList.Count; j++)
                {
                    if (i == j) continue;
                    if (UnitList[i].Name == UnitList[j].Name)
                    {
                        errorMessage = string.Format("Среди переключателей есть два с одинаковым именем \"{0}\".",
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
                    errormessage = "Среди переключателей имеется переключатель с незаполненным названием.";
                    return false;
                }
                if (string.IsNullOrEmpty(UnitList[i].OffStateName))
                {
                    errormessage = string.Format("У переключателя \"{0}\" не заполнено название состояния \"Выкл.\".",
                                                 UnitList[i].Name);
                    return false;
                }
                if (string.IsNullOrEmpty(UnitList[i].OnStateName))
                {
                    errormessage = string.Format("У переключателя \"{0}\" не заполнено название состояния \"Вкл.\".",
                                                 UnitList[i].Name);
                    return false;
                }
                if (UnitList[i].OnStateName == UnitList[i].OffStateName)
                {
                    errormessage =
                        string.Format(
                            "У переключателя \"{0}\" название состояния \"Вкл.\" совпадает с название состояния \"Выкл.\".",
                            UnitList[i].Name);
                    return false;
                }
                for (int j = 0; j < UnitList.Count; j++)
                {
                    if (i == j) continue;
                    if (UnitList[i].Name == UnitList[j].Name)
                    {
                        errormessage = string.Format("Среди переключателей есть два с одинаковым именем \"{0}\".",
                                                     UnitList[i].Name);
                        return false;
                    }
                }
            }
            errormessage = "OK";
            return true;
        }

        #endregion

        public override Device CreateNewDevice()
        {
            return new GangSwitchDeviceDesign {Type = this};
        }
    }
}