using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Interfaces.ConfigModule.System;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;

namespace TechnicalServices.Persistence.CommonPresentation
{
    public class PresentationDAL : IPresentationDAL
    {
        private const string _presentationExtension = ".xml";
        private const string _targetNameSpace = "urn:presentation-schema";

        protected readonly IConfiguration _configuration;

        /// <summary>
        /// хранилище описаний презентации - в качестве ключа - уникальное имя презентации
        /// </summary>
        protected readonly Dictionary<string, PresentationDescriptor> _presentationStorage =
            new Dictionary<string, PresentationDescriptor>(100);
        /// <summary>
        /// хранилище уникальных имен презентации - в качестве ключа - имя презентации
        /// </summary>
        protected readonly Dictionary<string, string> _presentationStorageByName =
            new Dictionary<string, string>(100);


        protected readonly ReaderWriterLock _sync = new ReaderWriterLock();

        public PresentationDAL(IConfiguration configuration)
        {
            Debug.Assert(configuration != null);

            _configuration = configuration;

            if (!Directory.Exists(_configuration.ScenarioFolder))
                throw new ApplicationException(String.Format("Нет директории: {0}", _configuration.ScenarioFolder));
        }

        private string[] PresentationFiles
        {
            get
            {
                return Directory.GetFiles(_configuration.ScenarioFolder,
                                          String.Format("*{0}", _presentationExtension));
            }
        }

        private XmlSerializer _presentationSerializer = null;
        protected XmlSerializer PresentationSerializaer
        {
            get
            {
                if (_presentationSerializer == null)
                {
                    _presentationSerializer = new XmlSerializer(typeof(Presentation), GetAttributeOverrides());
                }
                return _presentationSerializer;
            }
        }

        private XmlSerializer _slideBulkSerializer = null;
        protected XmlSerializer SlideBulkSerializaer
        {
            get
            {
                if (_slideBulkSerializer == null)
                {
                    _slideBulkSerializer = new XmlSerializer(typeof(SlideBulk), GetAttributeOverrides());
                }
                return _slideBulkSerializer;
            }
        }


        //public ILockService LockService { get; set; }

        #region IPresentationDAL Members

        public void Init(ISourceDAL sourceDAL, IDeviceSourceDAL deviceSourceDAL)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                _presentationStorage.Clear();
                _presentationStorageByName.Clear();
                string[] files = PresentationFiles;
                foreach (string file in files)
                {
                    string[] deletedEquipment;
                    Presentation presentation = LoadPresentation(file, sourceDAL, deviceSourceDAL, out deletedEquipment);
                    if (presentation == null) continue;
                    _presentationStorage[presentation.UniqueName] =
                        new PresentationDescriptor(presentation, file);
                    _presentationStorageByName[presentation.Name] = presentation.UniqueName;
                }
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public virtual IList<PresentationInfo> GetPresentationInfoList()
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                List<PresentationInfo> presentationInfos = new List<PresentationInfo>(_presentationStorage.Count);
                foreach (PresentationDescriptor descriptor in _presentationStorage.Values)
                {
                    presentationInfos.Add((PresentationInfo)descriptor);
                }
                return presentationInfos;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public virtual PresentationInfo GetPresentationInfo(string uniqueName)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                PresentationDescriptor presentationDescriptor;
                if (_presentationStorage.TryGetValue(uniqueName, out presentationDescriptor))
                {
                    // Проверяем наличие файла и если он все в порядке
                    if (File.Exists(presentationDescriptor.PresentationPath))
                        return (PresentationInfo)presentationDescriptor;
                }
                return null;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }

        }

        public PresentationInfo GetPresentationInfoByPresentationName(string name)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                //PresentationDescriptor presentationDescriptor;
                string uniqueName;
                if (_presentationStorageByName.TryGetValue(name, out uniqueName))
                    return GetPresentationInfo(uniqueName);
                else return null;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public Slide[] LoadSlides(string presentationUniqueName, int[] slideIdArr, ISourceDAL sourceDAL,
            IDeviceSourceDAL deviceSourceDAL)
        {
            string[] deletedEquipment;
            Presentation presentation = GetPresentation(presentationUniqueName,
                                                        sourceDAL, deviceSourceDAL, out deletedEquipment);
            List<Slide> slides = new List<Slide>(slideIdArr.Length);
            if (presentation == null) return slides.ToArray();
            foreach (int id in slideIdArr)
            {
                Slide slide = presentation.SlideList.Find(sl => sl.Id == id);
                slides.AddNotNull(slide);
            }
            return slides.ToArray();
        }

        private bool SaveToFile(string fileName, Presentation presentation)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                XmlSerializer serializer = PresentationSerializaer;
                    //new XmlSerializer(typeof(Presentation), GetAttributeOverrides());
                //ValidatePresentation(presentation, serializer );
                using (XmlWriter writer = XmlWriter.Create(fileName))
                {
                    if (writer == null) return false;
                    serializer.Serialize(writer, presentation);
                }
                return true;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public string GetPresentationFile(string uniqueName)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                PresentationDescriptor presentationDescriptor;
                if (!_presentationStorage.TryGetValue(uniqueName, out presentationDescriptor))
                    return null;
                return presentationDescriptor.PresentationPath;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public virtual bool SavePresentation(UserIdentity sender, Presentation presentation)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                string fileName = GetFileName(presentation);
                string fileNameBasedOnPresentationName = GetFileNameBasedOnPresentationName(presentation.Name);
                if (!fileName.Equals(fileNameBasedOnPresentationName,
                        StringComparison.InvariantCultureIgnoreCase) &&
                        File.Exists(fileNameBasedOnPresentationName)) return false;
                presentation.LastChangeDate = DateTime.Now;
                bool isSuccess = SaveToFile(fileNameBasedOnPresentationName,
                    presentation);
                if (isSuccess)
                {
                    PresentationInfo presentationInfo = new PresentationInfo(presentation);
                    if (IsNewPresentation(presentationInfo))
                    {
                        PresentationAdded(sender, presentationInfo);
                    }
                    string oldName = null;
                    PresentationDescriptor descriptor;
                    if (_presentationStorage.TryGetValue(presentation.UniqueName, out descriptor))
                        oldName = descriptor.PresentationInfo.Name;
                    _presentationStorage[presentation.UniqueName] = new PresentationDescriptor(presentation, fileNameBasedOnPresentationName);
                    if (!string.IsNullOrEmpty(oldName)) _presentationStorageByName.Remove(oldName);
                    _presentationStorageByName[presentation.Name] = presentation.UniqueName;
                    if (!fileName.Equals(fileNameBasedOnPresentationName,
                        StringComparison.InvariantCultureIgnoreCase))
                        DeleteFile(fileName);
                }
                return isSuccess;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public void SaveSlideBulk(string fileName, SlideBulk slideBulk)
        {
            XmlSerializer serializer = SlideBulkSerializaer;
                //new XmlSerializer(typeof(SlideBulk), GetAttributeOverrides());
            using (XmlWriter writer = XmlWriter.Create(fileName))
            {
                serializer.Serialize(writer, slideBulk);
            }
        }


        public bool DeletePresentation(UserIdentity sender, Presentation presentation)
        {
            return DeletePresentation(sender, presentation.UniqueName);
        }

        public virtual bool DeletePresentation(UserIdentity sender, string uniqueName)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                string fileName = GetFileName(uniqueName);
                DeleteFile(fileName);
                PresentationDescriptor descriptor = _presentationStorage[uniqueName];
                _presentationStorage.Remove(uniqueName);
                _presentationStorageByName.Remove(descriptor.PresentationInfo.Name);
                PresentationDeleted(sender, descriptor.PresentationInfo);
                return true;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }

        }

        public PresentationInfo[] GetPresentationWhichContainsSource(ResourceDescriptor resourceDescriptor)
        {
            List<PresentationInfo> presentationInfos = new List<PresentationInfo>();
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                foreach (KeyValuePair<string, PresentationDescriptor> keyValuePair in _presentationStorage)
                {
                    if (keyValuePair.Value.ContainsSoftwareSource(resourceDescriptor))
                        presentationInfos.Add((PresentationInfo)keyValuePair.Value);
                }
                return presentationInfos.ToArray();
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public PresentationInfo[] GetPresentationWhichContainsSource(DeviceResourceDescriptor resourceDescriptor)
        {
            List<PresentationInfo> presentationInfos = new List<PresentationInfo>();
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                foreach (KeyValuePair<string, PresentationDescriptor> keyValuePair in _presentationStorage)
                {
                    if (keyValuePair.Value.ContainsDeviceSource(resourceDescriptor))
                        presentationInfos.Add((PresentationInfo)keyValuePair.Value);
                }
                return presentationInfos.ToArray();
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public PresentationInfo[] GetPresentationWhichContainsLabel(int labelId)
        {
            List<PresentationInfo> presentationInfos = new List<PresentationInfo>();
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                foreach (KeyValuePair<string, PresentationDescriptor> keyValuePair in _presentationStorage)
                {
                    if (keyValuePair.Value.ContainsLabel(labelId))
                        presentationInfos.Add((PresentationInfo)keyValuePair.Value);
                }
                return presentationInfos.ToArray();
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        private void PresentationAdded(UserIdentity sender, PresentationInfo presentationInfo)
        {
            if (OnPresentationAdded != null)
            {
                OnPresentationAdded(this, new PresentationEventArg(presentationInfo, sender));
            }
        }

        private void PresentationDeleted(UserIdentity sender, PresentationInfo presentationInfo)
        {
            if (OnPresentationDeleted != null)
            {
                OnPresentationDeleted(this, new PresentationEventArg(presentationInfo, sender));
            }
        }

        public event EventHandler<PresentationEventArg> OnPresentationAdded;
        public event EventHandler<PresentationEventArg> OnPresentationDeleted;

        public virtual Presentation GetPresentation(string uniqueName, ISourceDAL sourceDAL, IDeviceSourceDAL deviceSourceDAL, out string[] deletedElements)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                deletedElements = new string[] {};
                PresentationDescriptor descriptor;
                if (!_presentationStorage.TryGetValue(uniqueName, out descriptor)) return null;
                return LoadPresentation(descriptor.PresentationPath, sourceDAL, deviceSourceDAL, out deletedElements);
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        #endregion

        protected List<ResourceDescriptor> GetDescriptorsForLoadPresentation(ISourceDAL sourceDAL, string presentationUniqueName)
        {
            List<ResourceDescriptor> descriptors = new List<ResourceDescriptor>();
            foreach (KeyValuePair<string, IList<ResourceDescriptor>> pair in sourceDAL.GetLocalSources(presentationUniqueName))
            {
                descriptors.AddRange(pair.Value);
            }
            //descriptors.AddRange(sourceDAL.GetLocalSources(new PresentationInfo(presentation)).Values);
            foreach (KeyValuePair<string, IList<ResourceDescriptor>> pair in sourceDAL.GetGlobalSources())
            {
                descriptors.AddRange(pair.Value);
            }
            return descriptors;
        }

        protected virtual bool IsPresentationExists(PresentationInfo presentationInfo)
        {
            string fileName = GetFileName(presentationInfo.UniqueName);
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                return true;
            return false;
        }

        private bool IsNewPresentation(PresentationInfo presentationInfo)
        {
            return !_presentationStorage.ContainsKey(presentationInfo.UniqueName);
        }

        public Presentation LoadPresentation(string file, ISourceDAL sourceDAL, IDeviceSourceDAL deviceSourceDAL, out string[] deletedElements)
        {
            XmlSerializer serializer = PresentationSerializaer;
            deletedElements = new string[] {};
                //new XmlSerializer(typeof(Presentation),
                //                                         GetAttributeOverrides());
            try
            {
                ValidateFile(file);
                using (XmlReader reader = XmlReader.Create(file))
                {
                    Presentation presentation = (Presentation)serializer.Deserialize(reader);
                    List<ResourceDescriptor> descriptors = GetDescriptorsForLoadPresentation(sourceDAL,
                                                                                             presentation.UniqueName);
                    DeviceResourceDescriptor[] deviceResourceDescriptors =
                        deviceSourceDAL.GetGlobalSources().SelectMany(kv => kv.Value).ToArray();
                    //descriptors.AddRange(sourceDAL.GetGlobalSources());
                    deletedElements = presentation.InitReference(_configuration.ModuleConfiguration,  _configuration.LabelStorageAdapter.GetLabelStorage(),
                        descriptors.ToArray(), deviceResourceDescriptors);
                    if (deletedElements != null && deletedElements.Length != 0)
                    {
                        string output = deletedElements.Aggregate((cur, next) => cur + ", " + next);
                        _configuration.EventLog.WriteWarning(
                            string.Format(
                                "При инициализации сценария {0} не было обнаружено в конфигурации следующее оборудование:\n {1}",
                                presentation.Name, output));
                    }
                    return presentation;
                }
            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format("PresentationDAL.LoadPresentation: file {0} \n{1}", file, ex));
            }
            return null;
        }

        public SlideBulk LoadSlideBulk(string fileName, ResourceDescriptor[] resourceDescriptors,
            DeviceResourceDescriptor[] deviceResourceDescriptors)
        {
            XmlSerializer serializer = SlideBulkSerializaer;
                //new XmlSerializer(typeof(SlideBulk),
                //                                         GetAttributeOverrides());
            try
            {
                using (XmlReader reader = XmlReader.Create(fileName))
                {
                    SlideBulk slideBulk = (SlideBulk)serializer.Deserialize(reader);
                    //descriptors.AddRange(sourceDAL.GetGlobalSources());
                    string[] deletedEquipment = slideBulk.InitReference(
                        _configuration.ModuleConfiguration, resourceDescriptors, deviceResourceDescriptors);
                    if (deletedEquipment != null && deletedEquipment.Length != 0)
                    {
                        string output = deletedEquipment.Aggregate((cur, next) => cur + ", " + next);
                        _configuration.EventLog.WriteWarning(
                            string.Format(
                                "При инициализации файла со сценами {0} не было обнаружено в конфигурации следующее оборудование:\n {1}",
                                fileName, output));
                    }
                    return slideBulk;
                }
            }
            catch (Exception ex)
            {
                _configuration.EventLog.WriteError(string.Format("PresentationDAL.LoadSlideBulk: file {0} \n{1}", fileName, ex));
            }
            return null;
        }


        private void DeleteFile(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName) && File.Exists(fileName))
                File.Delete(fileName);
        }

        //private bool IsPresentationNameChange(string presentationName, string fileName)
        //{
        //    string oldpresentationName = Path.GetFileNameWithoutExtension(fileName);
        //    return !presentationName.Equals(oldpresentationName, StringComparison.InvariantCultureIgnoreCase);
        //}

        private void ValidateFile(string file)
        {
            // Проводим валидацию, отдельно от десереализации, потому-что если вместе то валится
            using (XmlReader reader = XmlReader.Create(file, GetReaderSettings()))
                while (reader.Read()) ;
        }

        private XmlReaderSettings GetReaderSettings()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(_targetNameSpace, _configuration.ScenarioSchemaFile);
            return settings;
        }

        private string GetFileName(Presentation presentation)
        {
            PresentationDescriptor descriptor;
            if (_presentationStorage.TryGetValue(presentation.UniqueName, out descriptor))
                return descriptor.PresentationPath;
            return GetFileNameBasedOnPresentationName(presentation.Name);
        }

        private string GetFileNameBasedOnPresentationName(string presentationName)
        {
            return Path.Combine(_configuration.ScenarioFolder,
                                String.Format("{0}.xml", presentationName));
        }

        private string GetFileName(string uniqueName)
        {
            PresentationDescriptor descriptor;
            if (_presentationStorage.TryGetValue(uniqueName, out descriptor))
                return descriptor.PresentationPath;
            return null;
        }

        protected XmlAttributeOverrides GetAttributeOverrides()
        {
            XmlAttributeOverrides ovr = new XmlAttributeOverrides();
            XmlAttributes attrsDevice = new XmlAttributes();
            XmlAttributes attrsSource = new XmlAttributes();
            XmlAttributes attrsDisplay = new XmlAttributes();
            XmlAttributes attrsWindow = new XmlAttributes();

            attrsWindow.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof(Window)));
            foreach (IModule module in _configuration.ModuleList)
            {
                IPresentationModule plugin = module.SystemModule.Presentation;
                foreach (Type type in plugin.GetDevice())
                    attrsDevice.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                foreach (Type type in plugin.GetSource())
                    attrsSource.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                foreach (Type type in plugin.GetDisplay())
                    attrsDisplay.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
                foreach (Type type in plugin.GetWindow())
                    attrsWindow.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
            }

            ovr.Add(typeof(Display), "WindowList", attrsWindow);
            ovr.Add(typeof(Slide), "DeviceList", attrsDevice);
            ovr.Add(typeof(Slide), "SourceList", attrsSource);
            ovr.Add(typeof(Slide), "DisplayList", attrsDisplay);

            return ovr;
        }
    }
}
