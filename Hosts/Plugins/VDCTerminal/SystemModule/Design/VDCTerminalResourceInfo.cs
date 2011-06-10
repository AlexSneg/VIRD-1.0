using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml.Serialization;
using TechnicalServices.Common.Editor;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Common;
using Hosts.Plugins.VDCTerminal.SystemModule.Design.TypeConverters;
using Hosts.Plugins.VDCTerminal.SystemModule.Design.TypeEditors;
using TechnicalServices.Persistence.CommonPersistence.Presentation.PropertySorterConverter;

namespace Hosts.Plugins.VDCTerminal.SystemModule.Design
{
    [DataContract]
    [Serializable]
    public class VDCTerminalResourceInfo : ResourceInfoForHardwareSource
    {
        /// <summary>
        /// Справочник абонентов
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Справочник абонентов")]
        [XmlArray("AbonentList")]
        [TypeConverter(typeof(CollectionNameConverter))]
        [Editor(typeof(VDCTerminalAbonentEditor), typeof(UITypeEditor))]
        [CollectionName("Абоненты")]
        [CollectionFormName("Справочник абонентов")]
        [PropertiesName("Параметры абонентов")]
        [DataMember]
        public List<VDCTerminalAbonentInfo> AbonentList
        {
            get { return _abonentList; }
            set { _abonentList = value; }
        }

        private List<VDCTerminalAbonentInfo> _abonentList = new List<VDCTerminalAbonentInfo>();

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

    }

    internal class VDCTerminalAbonentEditor : ClonableObjectCollectionEditorAdv<VDCTerminalAbonentInfo>
    {
        public VDCTerminalAbonentEditor(Type type)
            : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            VDCTerminalResourceInfo device = (VDCTerminalResourceInfo)Context.Instance;
            if (device.AbonentList.Count >= 100)
            {
                //throw new IndexOutOfRangeException("Количество групп фейдеров не может превышать 32");
                Control owner = Form.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
                MessageBox.Show(owner, "Количество абонетов не может превышать 100", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            VDCTerminalAbonentInfo unit = (VDCTerminalAbonentInfo)base.CreateInstance(itemType);
            return unit;
        }

        protected override void OnRemoveItem(VDCTerminalAbonentInfo sender)
        {
            VDCTerminalResourceInfo device = (VDCTerminalResourceInfo)Context.Instance;
            device.AbonentList.Remove(device.AbonentList.FirstOrDefault(i => 
                i.Name == sender.Name && 
                i.Number1 == sender.Number1 && 
                i.Number2 == sender.Number2 && 
                i.ConnectionQuality == sender.ConnectionQuality && 
                i.ConnectionType == sender.ConnectionType));
        }
    }
}