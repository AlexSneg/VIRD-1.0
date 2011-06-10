using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Common.Caching;
using TechnicalServices.Common.Locking;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.CommonPresentation
{
    public class PresentationDALCaching : PresentationDAL
    {
        private const int _hours = 24;
        private static readonly TimeSpan _cachingTime = TimeSpan.FromHours(_hours);
        private readonly Cache<Presentation> _cache = new Cache<Presentation>(_cachingTime);

        public PresentationDALCaching(IConfiguration configuration)
            : base(configuration)
        { }

        public override bool DeletePresentation(UserIdentity sender, string uniqueName)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                bool isSuccess = base.DeletePresentation(sender, uniqueName);
                if (isSuccess)
                    _cache.Delete(ObjectKeyCreator.CreatePresentationKey(uniqueName));
                return isSuccess;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }

        }

        public override Presentation GetPresentation(string uniqueName, ISourceDAL sourceDAL, IDeviceSourceDAL deviceSourceDAL, out string[] deletedElements)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                ObjectKey key = ObjectKeyCreator.CreatePresentationKey(uniqueName);
                Presentation presentation = _cache.Read(key);
                if (presentation != null)
                {
                    try
                    {
                        DeviceResourceDescriptor[] deviceResourceDescriptors =
                            deviceSourceDAL.GetGlobalSources().SelectMany(kv => kv.Value).ToArray();
                        // если презентация взята из кэша, нужно обновить ссылки
                        deletedElements =  presentation.InitReference(_configuration.ModuleConfiguration, _configuration.LabelStorageAdapter.GetLabelStorage(),
                                                   GetDescriptorsForLoadPresentation(sourceDAL, presentation.UniqueName).ToArray(),
                                                       deviceResourceDescriptors);
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
                    catch (Exception ex)
                    {
                        _configuration.EventLog.WriteError(string.Format("PresentationDALCaching.GetPresentation: uniqueName: {0} \n{1}", uniqueName, ex));
                    }
                }
                presentation = base.GetPresentation(uniqueName, sourceDAL, deviceSourceDAL, out deletedElements);
                if (presentation != null) _cache.Add(key, presentation);
                return presentation;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public override bool SavePresentation(UserIdentity sender, Presentation presentation)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                bool isSuccess = base.SavePresentation(sender, presentation);
                if (isSuccess)
                    _cache.Add(ObjectKeyCreator.CreatePresentationKey(presentation),
                        presentation);
                return isSuccess;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }
    }
}
