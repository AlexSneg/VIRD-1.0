using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml.Serialization;

using Hosts.Plugins.VDCServer.SystemModule.Design;
using TechnicalServices.Common;
using TechnicalServices.Common.Editor;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.VDCServer.SystemModule.Config
{
    [Serializable]
    [XmlType("VDCServer")]
    public class VDCServerDeviceConfig : DeviceType, ICollectionItemValidation
    {
        private List<ScreenLayout> _screenLayoutList = new List<ScreenLayout>();
        private string _serverAddress = "0.0.0.0";

        public VDCServerDeviceConfig()
        {
        }

        public VDCServerDeviceConfig(string name)
        {
            Name = name;
            Type = "Сервер ВКС";
        }

        //public override Device CreateNewDevice(Dictionary<string, IList<DeviceResourceDescriptor>> resources)
        //{
        //    IList<DeviceResourceDescriptor> descriptors = null;
        //    Device retValue = base.CreateNewDevice(resources);
        //    if (resources.TryGetValue(retValue.Type.Type, out descriptors))
        //    {
        //        retValue.DeviceResourceDescriptor = descriptors.FirstOrDefault(x => x.ResourceInfo.DeviceType.Name == retValue.Type.Name);
        //    }
        //    return retValue;
        //}

        [XmlIgnore]
        [Browsable(false)]
        public override bool IsHardware
        {
            get { return true; }
        }

        [XmlIgnore]
        [Browsable(false)]
        [ReadOnly(true)]
        public int RequestPeriod
        {
            get { return 15; }
        }

        /// <summary>
        /// IP сервера.
        /// </summary>
        [Category("Настройки")]
        [DisplayName("IP сервера")]
        [DefaultValue("0.0.0.0")]
        [XmlAttribute("ServerAddress")]
        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                value = ValidationHelper.CheckIsNullOrEmpty(value, "IP сервера");
                _serverAddress = ValidationHelper.CheckLength(value, 50, "IP сервера");
            }
        }


        /// <summary>
        /// Переключатели
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Раскладки видеоокон")]
        [XmlArray("ScreenLayoutList")]
        [TypeConverter(typeof (CollectionNameConverter))]
        [Editor(typeof (ScreenLayoutEditor), typeof (UITypeEditor))]
        [CollectionName("Раскладки")]
        [CollectionFormName("Раскладки видеоокон")]
        [PropertiesName("Параметры раскладок")]
        public List<ScreenLayout> ScreenLayoutList
        {
            get { return _screenLayoutList; }
            set { _screenLayoutList = value; }
        }

        public override Device CreateNewDevice()
        {
            return new VDCServerDeviceDesign {Type = this};
        }

        protected override ResourceInfo CreateNewResourceInfoProtected()
        {
            return new VDCServerResourceInfo();
        }

        public bool ValidateItem(out string errorMessage)
        {
            for (int i = 0; i < ScreenLayoutList.Count; i++)
            {

                for (int j = 0; j < ScreenLayoutList.Count; j++)
                {
                    if (i == j) continue;
                    if (ScreenLayoutList[i].LayoutName == ScreenLayoutList[j].LayoutName)
                    {
                        errorMessage = string.Format("В списке есть раскладки с одинаковым названием '{0}'.",
                                                     ScreenLayoutList[i].LayoutName);
                        return false;
                    }
                    if (ScreenLayoutList[i].LayoutNumber == ScreenLayoutList[j].LayoutNumber)
                    {
                        errorMessage = string.Format("В списке есть раскладки с одинаковым номером '{0}'.",
                                                     ScreenLayoutList[i].LayoutNumber);
                        return false;
                    }
                }
            }
            errorMessage = null;
            return true;
        }
    }

    internal class ScreenLayoutEditor : ClonableObjectCollectionEditorAdv<ScreenLayout>
    {
        public ScreenLayoutEditor(Type type)
            : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            VDCServerDeviceConfig device = (VDCServerDeviceConfig)Context.Instance;
            //if (device.ScreenLayoutList.Count >= 100)
            //{
            //    //throw new IndexOutOfRangeException("Количество раскладок не может превышать 100");
            //    Control owner = Form.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
            //    MessageBox.Show(owner, "Количество раскладок не может превышать 100", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return null;
            //}

            ScreenLayout unit = (ScreenLayout)base.CreateInstance(itemType);
            int num = device.ScreenLayoutList.Count + 1;
            unit.LayoutNumber = num;
            unit.LayoutName = "Раскладка " + num;
            return unit;
        }


    }
}