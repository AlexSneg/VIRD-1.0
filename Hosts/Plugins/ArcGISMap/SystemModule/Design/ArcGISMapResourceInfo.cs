using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using TechnicalServices.Interfaces;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Exceptions;
using System.Reflection;
using System.Linq;

namespace Hosts.Plugins.ArcGISMap.SystemModule.Design
{
    [Serializable]
    public class ArcGISMapResourceInfo : ResourceFileInfo, ISupportCustomSaveState, INotifyPropertyChanged, ISupportValidation
    {
        #region ctor
        public ArcGISMapResourceInfo()
        {
        }
        #endregion

        #region properties
        #endregion

        #region resourcefileinfo overrides
        [Browsable(false)]
        [XmlIgnore]
        public override string Filter
        {
            get { return @"Файлы карт (*.mxd)|*.mxd|Все файлы|*.*"; }
        }
        #endregion

        #region resourceinfo overrides
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection propColl = base.GetProperties(attributes);

            //// в зависимости от типа провайдера доступны различные свойства
            //List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            //foreach (PropertyDescriptor propertyDescriptor in propColl)
            //{
            //    ProviderTypeRequiredAttribute attr = (ProviderTypeRequiredAttribute)propertyDescriptor.Attributes[typeof(ProviderTypeRequiredAttribute)];
            //    if ((attr == null) || (attr.ProviderType == ProviderType))
            //        newColl.Add(propertyDescriptor);
            //}

            ////скрываем файловые свойства, потому что у нас иное поведение
            //foreach (PropertyInfo propertyInfo in this.ResourceFileProperties)
            //{
            //    PropertyDescriptor pd = TypeDescriptor.CreateProperty(this.GetType(),
            //            propertyInfo.Name, typeof(string), attributes);

            //    if (newColl.LastIndexOf(pd) != -1)
            //    {
            //        newColl.RemoveAt(newColl.LastIndexOf(pd));
            //    }
            //}
            //propColl = new PropertyDescriptorCollection(newColl.ToArray(), true);
            return propColl;
        }
        #endregion

        

        #region INotifyPropertyChanged Members

        [NonSerialized]
        private HashSet<PropertyChangedEventHandler> handlers = new HashSet<PropertyChangedEventHandler>();

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                CheckHandlers();

                handlers.Add(value);
            }
            remove
            {
                CheckHandlers();
                handlers.Remove(value);
            }
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            CheckHandlers();

            if (handlers.Count > 0)
            {
                foreach (PropertyChangedEventHandler handler in handlers)
                {
                    handler.Invoke(sender, args);
                }
            }
        }

        void CheckHandlers()
        {
            if (handlers == null)
                handlers = new HashSet<PropertyChangedEventHandler>();
        }


        #endregion

        #region ISupportCustomSaveState Members

        public void GetState(out object[] ObjectContext)
        {
            ObjectContext = new object[] { handlers };
        }

        public void SetState(object[] ObjectContext)
        {
            if (ObjectContext != null && ObjectContext.Length > 0)
            {
                HashSet<PropertyChangedEventHandler> set = ObjectContext[0] as HashSet<PropertyChangedEventHandler>;
                if (set != null)
                {
                    handlers = set;
                    OnPropertyChanged(this, new PropertyChangedEventArgs("*"));
                }
            }
        }

        #endregion

        #region ISupportValidation Members

        public bool EnsureValidate(out string errormessage)
        {
            errormessage = "OK";
            if (String.IsNullOrEmpty(ResourceFullFileName))
            {
                errormessage = "Должен быть указан файл карты";
                return false;
            }
            if (!System.IO.File.Exists(ResourceFullFileName))
            {
                errormessage = "Указанный файл карты не существует";
                return false;
            }
            return true;
        }

        #endregion
    }
}
