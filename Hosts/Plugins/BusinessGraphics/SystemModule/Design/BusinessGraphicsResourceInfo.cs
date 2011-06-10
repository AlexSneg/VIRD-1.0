using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Web.UI.Design;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using TechnicalServices.Interfaces;
using System.Windows.Forms;
using dotnetCHARTING.Mapping;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Exceptions;
using System.Reflection;
using System.Linq;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design.Helpers;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    [Serializable]
    public class BusinessGraphicsResourceInfo : ResourceFileInfo, ISupportCustomSaveState, INotifyPropertyChanged, ISupportValidation
    {
        private readonly string _odbcProcWrongName = "Строка ODBC-процедуры может содержать только цифро-буквенные выражения и знак подчеркивания";
        #region ctor
        public BusinessGraphicsResourceInfo()
        {
        }
        #endregion

        #region properties
        /// <summary>
        /// Тип источника (XML/ODBC)
        /// Обязательный параметр
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Тип источника")]
        [XmlAttribute("ProviderType")]
        [RefreshProperties(RefreshProperties.All)]
        public ProviderTypeEnum ProviderType
        {
            get
            {
                return providerType;
            }
            set
            {
                isFilesChanged = true;
                providerType = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs("ProviderType"));
            }
        }

        ProviderTypeEnum providerType;

        string file;

        /// <summary>
        /// Путь к файлу XML
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Путь к XML файлу")]
        [XmlIgnore]
        [ProviderTypeRequired(ProviderTypeEnum.XML)]
        [Editor(typeof(MyXMLFileUIEditor), typeof(UITypeEditor))]
        [ResourceFileAttribute("xmlResource", true, false)]
        public string File
        {
            get
            {
                return GetResourceFileName("xmlResource");
            }
            set
            {
                string _value = ValidationHelper.CheckLength(value, 250, "пути к файлу XML");
                if (_value != null)
                {
                    string prevvalue = file;
                    file = _value;
                    if (ValidateSchema())
                    {
                        isFilesChanged = true;
                        OnPropertyChanged(this, new PropertyChangedEventArgs("File"));
                        SetResourceFileName("xmlResource", file);
                    }
                    else
                    {
                        string fName = file;
                        file = prevvalue;
                        throw new InvalideFileException(String.Format("Указанный файл ({0}) не соответствует XML-схеме или сформирован некорректно!\r\nУкажите другой файл XML", fName));
                    }
                }
                else
                    file = value;
            }
        }


        string mapFile;


        /// <summary>
        /// Путь к файлу карты
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Путь к файлу с картой")]
        [XmlIgnore]
        [Editor(typeof(ShapeFileUIEditor), typeof(UITypeEditor))]
        [ResourceFileAttribute("mapResource", false, false)]
        public string MapFile
        {
            get
            {
                return GetResourceFileName("mapResource");
            }
            set
            {
                string _value = ValidationHelper.CheckLength(value, 250, "пути к файлу с картой");

                if (_value != null)
                {
                    string message;
                    if (ValidateMap(_value, out message))
                    {
                        isFilesChanged = true;
                        mapFile = _value;
                        OnPropertyChanged(this, new PropertyChangedEventArgs("MapFile"));
                        SetResourceFileName("mapResource", mapFile);
                        string dbfFile = Path.ChangeExtension(mapFile, "dbf");
                        if (System.IO.File.Exists(dbfFile))
                            MapDataFile = dbfFile;
                    }
                    else
                    {
                        throw new InvalideFileException(message);
                    }
                }
                else
                    mapFile = value;
            }
        }

        [XmlIgnore]
        [Browsable(false)]
        [ResourceFileAttribute("mapdataResource", false, false)]
        public string MapDataFile
        {
            get
            {
                return GetResourceFileName("mapdataResource");
            }
            protected set
            {
                SetResourceFileName("mapdataResource", value);
            }
        }
        [NonSerialized]
        private bool isFilesChanged = false;
        [NonSerialized]
        private bool isOdbcChanged = false;
        
        string odbc;
        /// <summary>
        /// Источник ODBC
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Строка подключения к ODBC-источнику")]
        [XmlAttribute("ODBC")]
        [ProviderTypeRequired(ProviderTypeEnum.ODBC)]
        public string ODBC
        {
            get
            {
                return odbc;
            }
            set
            {
                string _value = ValidationHelper.CheckLength(value, 250, "строки подключения");
                if (_value != null)
                {
                    if (String.IsNullOrEmpty(_value))
                        throw new InvalidParameterException("Строка подключения к ODBC-источнику не может пустой");
                    else
                    {
                        odbc = _value;
                        isOdbcChanged = true;
                    }
                }
                else
                    odbc = value;
            }
        }


        string odbcProc;
        /// <summary>
        /// Источник ODBC
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Строка вызова процедуры ODBC-источника")]
        [XmlAttribute("ODBCProc")]
        [ProviderTypeRequired(ProviderTypeEnum.ODBC)]
        public string ODBCProcedure
        {
            get
            {
                return odbcProc;
            }
            set
            {
                string _value = ValidationHelper.CheckLength(value, 250, "ODBC процедуры");
                if (_value != null)
                {
                    if (String.IsNullOrEmpty(_value))
                        throw new InvalidParameterException("Строка ODBC-процедуры не может пустой");
                    else
                    {
                        if (ValidationHelper.CheckODBCProcedureName(_value))
                        {
                            odbcProc = _value;
                            isOdbcChanged = true;
                        } else
                            throw new InvalidParameterException(_odbcProcWrongName);
                    }
                }
                else
                    odbcProc = value;
            }
        }

        int seriesAmount;
        int recordAmount;

        /*/// <summary>
        /// Количество серий
        /// Параметр заполняется Системой автоматически после подключения к источнику
        /// 0 - Системе не удалось рассчитать параметр / серий в источнике нет
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Количество серий")]
        [XmlAttribute("SeriesAmount"), ReadOnly(true)]
        public int SeriesAmount
        {
            get { return seriesAmount; }
            set
            {
                seriesAmount = value;
            }
        }

        /// <summary>
        /// Количество записей
        /// Параметр заполняется Системой автоматически после подключения к источнику
        /// 0 - Системе не удалось рассчитать параметр / записей в источнике нет
        /// </summary>
        [Category("Настройки")]
        [DisplayName("Количество записей")]
        [XmlAttribute("RecordAmount"), ReadOnly(true)]
        public int RecordAmount
        {
            get { return recordAmount; }
            set
            {
                recordAmount = value;
            }
        }*/

        #endregion

        #region resourceinfo overrides
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection propColl = base.GetProperties(attributes);

            // в зависимости от типа провайдера доступны различные свойства
            List<PropertyDescriptor> newColl = new List<PropertyDescriptor>(propColl.Count);
            foreach (PropertyDescriptor propertyDescriptor in propColl)
            {
                ProviderTypeRequiredAttribute attr = (ProviderTypeRequiredAttribute)propertyDescriptor.Attributes[typeof(ProviderTypeRequiredAttribute)];
                if ((attr == null) || (attr.ProviderType == ProviderType))
                    newColl.Add(propertyDescriptor);
            }

            //скрываем файловые свойства, потому что у нас иное поведение
            foreach (PropertyInfo propertyInfo in this.ResourceFileProperties)
            {
                PropertyDescriptor pd = TypeDescriptor.CreateProperty(this.GetType(),
                        propertyInfo.Name, typeof(string), attributes);

                if (newColl.LastIndexOf(pd) != -1)
                {
                    newColl.RemoveAt(newColl.LastIndexOf(pd));
                }
            }
            propColl = new PropertyDescriptorCollection(newColl.ToArray(), true);
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
            if (String.IsNullOrEmpty(file) && ProviderType == ProviderTypeEnum.XML)
            {
                errormessage = "Должен быть указан XML файл";
                return false;
            }
            if (!System.IO.File.Exists(file) && ProviderType == ProviderTypeEnum.XML)
            {
                errormessage = "Указанный XML файл не существует";
                return false;
            }
            if (!String.IsNullOrEmpty(mapFile) && !System.IO.File.Exists(mapFile))
            {
                errormessage = "Указанный SHP файл карты не существует";
                return false;
            }
            if (ProviderType == ProviderTypeEnum.ODBC)
            {
                bool isValid = ValidateODBC(out errormessage);
                if (isValid && isOdbcChanged) OnPropertyChanged(this, new PropertyChangedEventArgs("ODBC"));
                if (!isValid) return false;
            }
            if (isOdbcChanged || isFilesChanged)
                errormessage = "Изменение параметров источника может привести к некорректному отображению диаграммы";
            return true;
        }

        #endregion

        #region validating

        private bool ValidateMap(string value, out string errorMessage)
        {
            if (!System.IO.File.Exists(value))
            {
                errorMessage = "Указанный Вами файл не существует";
                return false;
            }
            try
            {
                if (MapDataEngine.LoadLayer(value) != null)
                {
                    errorMessage = string.Empty;
                    return true;
                }
            }
            catch
            {
            }
            errorMessage = "Указанный файл SHP некорректно сформирован или содержит ошибки!\r\nУкажите другой файл SHP";
            return false;
        }

        private bool ValidateODBC(out string errormessage)
        {
            errormessage = string.Empty;
            if (string.IsNullOrEmpty(odbcProc) || string.IsNullOrEmpty(odbc))
            {
                errormessage = "Заполните корректно строку подключения\r\n и укажите имя хранимой процедуры ";
                return false;
            }
            if (!ValidationHelper.CheckODBCProcedureName(odbcProc))
            {
                errormessage = _odbcProcWrongName;
                return false;
            }
            XmlTextReader dt;
            if (!ODBCProvider.TestConnection(odbc, odbcProc, out errormessage, out dt))
                return false;
            else
            {
                if (!ValidateSchema(dt))
                {
                    errormessage = String.Format("Указанный ODBC источник возвращает данные, не соответствующие XML-схеме!\r\nУкажите другой ODBC источник");
                    return false;
                }
            }
            return true;
        }

        private bool ValidateSchema()
        {
            if (System.IO.File.Exists(file))
            {
                using (Stream stream = System.IO.File.OpenRead(file))
                {
                    try
                    {
                        XmlTextReader dt = new XmlTextReader(stream);
                        return ValidateSchema(dt);
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
                        stream.Close();
                    }
                }
            }
            return true;
        }

        private bool ValidateSchema(XmlTextReader data)
        {
            try
            {
                //воспользовался устарвешней конструкцией, но зато она работает
                XmlValidatingReader validator = new XmlValidatingReader(data);
                StringReader sr = new StringReader(Properties.Resources.XMLFileSchema);
                XmlSchema xmlFileSchema = XmlSchema.Read(sr, new ValidationEventHandler(XmlSchemaValidation));
                validator.Schemas.Add(xmlFileSchema);
                validator.ValidationType = ValidationType.Schema;
                validator.ValidationEventHandler += XmlSchemaValidation;
                XmlDocument doc = new XmlDocument();
                doc.Load(validator);
                if (doc.DocumentElement == null) return false;
                XmlNodeList ser = doc.DocumentElement.SelectNodes(@"SeriesDefinitions/SeriesInfo");
                XmlNodeList pnt = doc.DocumentElement.SelectNodes(@"PointsDefinitions/PointInfo");
                XmlNodeList dt = doc.DocumentElement.SelectNodes(@"DataIntersections/DataInfo");
                if (dt.Count != ser.Count * pnt.Count)
                    return false;
                //while (validator.Read()) ;

                /*XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                StringReader sr = new StringReader(Properties.Resources.XMLFileSchema);
                XmlSchema xmlFileSchema = XmlSchema.Read(sr, new ValidationEventHandler(XmlSchemaValidation));
                settings.Schemas.Add(xmlFileSchema);
                settings.ValidationEventHandler += new ValidationEventHandler(XmlSchemaValidation);
                XmlReader reader = XmlReader.Create(data, settings);
                while (reader.Read()) ;*/
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void XmlSchemaValidation(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
                throw new InvalidXmlSchemaException();
            }
        }
        #endregion

        internal void SetAmount(int seriesAmount, int recordAmount)
        {
            this.seriesAmount = seriesAmount;
            this.recordAmount = recordAmount;
        }
    }

    #region help stuff
    public class ProviderTypeRequiredAttribute : Attribute
    {
        public ProviderTypeRequiredAttribute(ProviderTypeEnum type)
        {
            ProviderType = type;
        }

        public ProviderTypeEnum ProviderType { get; private set; }
    }

    public enum ProviderTypeEnum
    {
        XML, ODBC
    }
    #endregion
}
