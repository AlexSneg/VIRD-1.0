using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using TechnicalServices.Common.Editor;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Resource;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.ComponentModel;
using TechnicalServices.Common.TypeConverters;
using System.Collections.Generic;
using Hosts.Plugins.VDCServer.SystemModule.Config;
using System.Drawing.Design;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Interfaces;
using Hosts.Plugins.VDCServer.SystemModule.Design.TypeConverters;
using Hosts.Plugins.VDCServer.SystemModule.Design.TypeEditors;
using TechnicalServices.Persistence.CommonPersistence.Presentation.PropertySorterConverter;
using TechnicalServices.Common;
using TechnicalServices.Persistence.CommonPersistence.Presentation;

namespace Hosts.Plugins.VDCServer.SystemModule.Design
{
    [Serializable]
    [XmlType("VDCServer")]
    [TypeConverter(typeof(PropertySorter))]
    public class VDCServerDeviceDesign : Device, ISupportValidation, ICollectionItemValidation
    {
        /// <summary>
        /// Состояние конференции (Активна/Неактивна)
        /// Обязательное значение
        /// По умолчанию: Активна
        /// </summary>
        [DefaultValue(true)]
        [Category("Настройки")]
        [DisplayName("Состояние конференции")]
        [XmlAttribute("IsConferenceActive")]
        [TypeConverter(typeof(ConferenceStateConverter))]
        public bool IsConferenceActoive
        {
            get { return _isConferenceActive; }
            set { _isConferenceActive = value; }
        }
        private bool _isConferenceActive = true;

        /// <summary>
        /// Название конференции.
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Название конференции")]
        [DefaultValue("Конференция")]
        [XmlAttribute("ConferenceName")]
        public string ConferenceName
        {
            get { return _conferenceName; }
            set
            {
                ValidationHelper.CheckIsNullOrEmpty(value, "Название конференции");
                _conferenceName = ValidationHelper.CheckLength(value, 50, "названия");
            }
        }
        private string _conferenceName = "Конференция";

        /// <summary>
        /// Комментарий к конференции
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Комментарий к конференции")]
        [XmlAttribute("Comments")]
        public string Comments
        {
            get { return _comments; }
            set
            {
                _comments = ValidationHelper.CheckLength(value, 250, "комментария");

            }
        }
        private string _comments;

        /// <summary>
        /// Voice-switched.
        /// </summary>
        [DefaultValue(true)]
        [Category("Настройки")]
        [DisplayName("Voice-Switched")]
        [XmlAttribute("VoiceSwitched")]
        [TypeConverter(typeof(YesNoConverter))]
        public bool VoiceSwitched
        {
            get { return _voiceSwitched; }
            set { _voiceSwitched = value; }
        }
        private bool _voiceSwitched = true;

        /// <summary>
        /// Закрытая.
        /// </summary>
        [Category("Настройки")]
        [DefaultValue(false)]
        [DisplayName("Закрытая")]
        [XmlAttribute("Private")]
        [TypeConverter(typeof(YesNoConverter))]
        public bool Private
        {
            get { return _private; }
            set { _private = value; }
        }
        private bool _private = false;

        /// <summary>
        /// Пароль.
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Пароль")]
        [XmlAttribute("Password")]
        public string Password
        {
            get { return _password; }
            set
            {
                _password = ValidationHelper.CheckLength(value, 50, "пароля");
            }
        }
        private string _password;

        /// <summary>
        /// Участники.
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Участники")]
        [XmlArray("Members"), XmlArrayItem("ConferenceMember")]
        [EditorReadonly(typeof(MembersEditor))]
        [Editor(typeof(MembersEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(CollectionNameConverter))]
        public List<VDCServerAbonentInfo> Members
        {
            get { return _members; }
            set { _members = value; }
        }
        private List<VDCServerAbonentInfo> _members = new List<VDCServerAbonentInfo>();

        /// <summary>
        /// Раскладка.
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Раскладка")]
        [XmlElement("Layout")]
        [TypeConverter(typeof(ActiveLayoutConverter))]
        [Editor(typeof(ActiveLayoutUITypeEditor), typeof(UITypeEditor))]
        public ScreenLayout Layout
        {
            get { return _layout; }
            set { _layout = value; }
        }
        private ScreenLayout _layout;

        /// <summary>
        /// Активный абонент (фокус).
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Активный абонент (фокус)")]
        [XmlElement("ActiveMember")]
        [TypeConverter(typeof(ActiveMemberConverter))]
        public VDCServerAbonentInfo ActiveMember
        {
            get { return _activeMember; }
            set { _activeMember = value; }
        }
        private VDCServerAbonentInfo _activeMember;

        [Category("Настройки")]
        [DisplayName("Справочник абонентов")]
        [XmlIgnore]
        [TypeConverter(typeof(CollectionNameConverter))]
        [Editor(typeof(VDCServerAbonentEditor), typeof(UITypeEditor))]
        [CollectionName("Абоненты")]
        [CollectionFormName("Справочник абонентов")]
        [PropertiesName("Параметры абонентов")]
        [ForceDeviceResourceSave]
        public List<VDCServerAbonentInfo> AbonentList
        {
            get
            {
                if (DeviceResourceDescriptor == null || DeviceResourceDescriptor.ResourceInfo == null)
                    return new List<VDCServerAbonentInfo>();
                return ((VDCServerResourceInfo)DeviceResourceDescriptor.ResourceInfo).AbonentList;
            }
            set
            {
                if (DeviceResourceDescriptor == null || DeviceResourceDescriptor.ResourceInfo == null)
                    return;
                ((VDCServerResourceInfo)DeviceResourceDescriptor.ResourceInfo).AbonentList = value;
            }
        }

        [Category("Настройки")]
        [Browsable(false)]
        [XmlIgnore]
        public override TechnicalServices.Persistence.SystemPersistence.Resource.DeviceResourceDescriptor DeviceResourceDescriptor
        {
            get
            {
                return base.DeviceResourceDescriptor;
            }
            set
            {
                base.DeviceResourceDescriptor = value;
                if (null == value) return;
                VDCServerResourceInfo info = value.ResourceInfo as VDCServerResourceInfo;
                if (info == null) throw new Exception("ResourceInfo должен быть типа VDCServerResourceInfo");
            }
        }

        [Category("Настройки")]
        [DisplayName("Импорт абонентов")]
        [Browsable(true)]
        [XmlIgnore]
        [TypeConverter(typeof(ImportNameConverter))]
        [Editor(typeof(ImportFileEditor), typeof(UITypeEditor))]
        [PropertyOrder(30)]
        public string ImportContacts
        { get { return _import; } set { _import = value; } }
        [XmlIgnore]
        [Browsable(false)]
        private string _import;


        [Category("Настройки")]
        [DisplayName("Экспорт абонентов")]
        [Browsable(true)]
        [XmlIgnore]
        [TypeConverter(typeof(ExportNameConverter))]
        [Editor(typeof(ExportFileEditor), typeof(UITypeEditor))]
        [PropertyOrder(31)]
        public string ExportContacts
        { get { return _export; } set { _export = value; } }
        [XmlIgnore]
        [Browsable(false)]
        private string _export;


        #region ISupportValidation Members

        bool ISupportValidation.EnsureValidate(out string errormessage)
        {
            for(int i=0; i< AbonentList.Count; i++)
            {
                VDCServerAbonentInfo abonent = AbonentList[i];
                if(string.IsNullOrEmpty(abonent.Name))
                {
                    errormessage = string.Format("В справочнике абонентов контакт №{0} имеет незаполненное поле 'Имя'.", i);
                    return false;
                }
                if (string.IsNullOrEmpty(abonent.Number1))
                {
                    errormessage = string.Format("В справочнике абонентов контакт №{0} имеет незаполненное поле 'Номер1'.", i);
                    return false;
                }
            }

            if (this.Private) // Закрытая конференция обязательно должна быть с паролем
            {
                if (string.IsNullOrEmpty(this.Password))
                {
                    errormessage = "Для закрытой конференции обязательно должно быть заполнено поле 'Пароль'.";
                    return false;
                }
            }
            if (this.Layout == null)
            {
                errormessage = "Должна быть установлена раскладка.";
                return false;
            }
            errormessage = "OK";
            return true;
        }
        #endregion

        #region ICollectionItemValidation Members

        bool ICollectionItemValidation.ValidateItem(out string errorMessage)
        {
            for (int i = 0; i < AbonentList.Count; i++)
            {
                for (int j = 0; j < AbonentList.Count; j++)
                {
                    if (i == j) continue;
                    if (AbonentList[i].Name == AbonentList[j].Name)
                    {
                        errorMessage = string.Format("Абонент '{0}' встречается в списке абонентов несколько раз.", AbonentList[j].Name);
                        return false;
                    }
                }
            }
            errorMessage = "Нет ошибок.";
            return true;
        }

        #endregion
    }

    internal class VDCServerAbonentEditor : ClonableObjectCollectionEditorAdv<VDCServerAbonentInfo>
    {
        public VDCServerAbonentEditor(Type type) : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            VDCServerDeviceDesign device = (VDCServerDeviceDesign)Context.Instance;
            if (device.AbonentList.Count >= 100)
            {
                //throw new IndexOutOfRangeException("Количество групп фейдеров не может превышать 32");
                Control owner = Form.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
                MessageBox.Show(owner, "Количество абонетов не может превышать 100", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            VDCServerAbonentInfo unit = (VDCServerAbonentInfo)base.CreateInstance(itemType);
            return unit;
        }

        protected override void OnRemoveItem(VDCServerAbonentInfo sender)
        {
            VDCServerDeviceDesign device = (VDCServerDeviceDesign)Context.Instance;
            device.AbonentList.Remove(device.AbonentList.FirstOrDefault(i => 
                i.Name == sender.Name &&
                i.Number1 == sender.Number1 &&
                i.Number2 == sender.Number2 &&
                i.ConnectionQuality == sender.ConnectionQuality &&
                i.ConnectionType == i.ConnectionType));
        }
        
    }
}