using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    public class SourceEventArg<TResource> : EventArgs
        where TResource : ResourceDescriptorAbstract
    {
        private readonly UserIdentity _userIdentity;
        private readonly TResource _resourceDescriptor;
        public SourceEventArg(TResource resourceDescriptor,
            UserIdentity userIdentity)
        {
            _resourceDescriptor = resourceDescriptor;
            _userIdentity = userIdentity;
        }

        public TResource ResourceDescriptor
        {
            get { return _resourceDescriptor; }
        }

        public UserIdentity UserIdentity
        {
            get { return _userIdentity; }
        }
    }

    public interface ISourceDAL : IResourceEx<ResourceDescriptor>
    {
        ResourceDescriptor MakeFullClone(ResourceDescriptor descriptor);
        //void SaveSource(FileTransferObject obj);
        //string SaveSourceWithAnotherName(FileTransferObject obj);
        bool DeleteSource(UserIdentity sender, ResourceDescriptor descriptor);
        FileSaveStatus GetSourceStatus(ResourceDescriptor descriptor);
        /// <summary>
        /// возвращает true если все обязательные ресурсы для источника существуют
        /// </summary>
        /// <param name="descriptor">ResourceDescriptor</param>
        /// <returns></returns>
        bool IsResourceAvailable(ResourceDescriptor descriptor);
        //FileSaveStatus CreateSource(FileTransferObject obj);
        //FileTransferObject? GetSource(ResourceDescriptor resourceDescriptor);
        Dictionary<string, IList<ResourceDescriptor>> GetGlobalSources();
        //ResourceDescriptor[] GetGlobalSources();
        Dictionary<string, IList<ResourceDescriptor>> GetLocalSources(string presentationUniqueName);   //PresentationInfo info
        //ResourceDescriptor[] GetLocalSources(PresentationInfo info);
        Dictionary<string, IList<ResourceDescriptor>> GetAllSources(IPresentationDAL presentationDAL);
        ResourceDescriptor GetStoredSource(string resourceId, string type, string presentationUniqueName);
        //ResourceDescriptor[] GetAllSources(IPresentationDAL presentationDAL);
        void DeleteLocalSourceFolder(PresentationInfo info);
        void DeleteLocalSourceFolder(string uniqueName);
        //bool IsExists(ResourceDescriptor resourceDescriptor);
        //string GetResourceFileName(ResourceDescriptor resourceDescriptor);
        ResourceDescriptor CopySourceFromGlobalToLocal(UserIdentity sender, ResourceDescriptor resourceDescriptor, string presentationUniqueName);
        ResourceDescriptor CopySourceFromLocalToGlobal(UserIdentity sender, ResourceDescriptor resourceDescriptor);
        void CopySourceFromLocalToLocal(UserIdentity userIdentity, string fromUniqueName, string toUniqueName);
        event EventHandler<SourceEventArg<ResourceDescriptor>> OnResourceAdded;
        event EventHandler<SourceEventArg<ResourceDescriptor>> OnResourceDeleted;
        event EventHandler<SourceEventArg<ResourceDescriptor>> OnResourceUpdated;
    }

    public interface IResourceEx<T>
    {
        bool IsExists(T descriptor);
        bool IsResourceExists(T descriptor, string resourceId);
        string GetResourceFileName(T descriptor, string resourceId);
        /// <summary>
        /// возвращается имя мастерного ресурса
        /// </summary>
        /// <param name="descriptor">ResourceDescriptor</param>
        /// <returns></returns>
        string GetResourceFileName(T descriptor);
        /// <summary>
        /// возвращает имя ресурсного файла какойон есть в реальности а не тот который хранится в хранилище ресурсов
        /// именно под этим именем ресурс лежит на сервере где крутится агент
        /// </summary>
        /// <param name="descriptor">ResourceDescriptor</param>
        /// <param name="resourceId">айдишник ресурса внутри сорса</param>
        /// <returns></returns>
        string GetRealResourceFileName(T descriptor, string resourceId);
        /// <summary>
        /// возвращается имя мастерного ресурса
        /// </summary>
        /// <param name="descriptor">ResourceDescriptor</param>
        /// <returns></returns>
        string GetRealResourceFileName(T descriptor);
        /// <summary>
        /// возвращает ресурс из хранилища если есть, если нет null
        /// </summary>
        /// <param name="descriptor">ResourceDescriptor</param>
        /// <returns></returns>
        T GetStoredSource(T descriptor);
        /// <summary>
        /// сохранение нересурсного сорса или только описания ресурсного сорса
        /// </summary>
        /// <param name="sender">пользователь</param>
        /// <param name="descriptor">ResourceDescriptor</param>
        /// <returns></returns>
        T SaveSource(UserIdentity sender, T descriptor);
        /// <summary>
        /// сохранение ресурсного сорса
        /// </summary>
        /// <param name="sender">пользователь</param>
        /// <param name="descriptor">ResourceDescriptor</param>
        /// <param name="fileDic">Dictionary содержащий айдишник внутреннего ресурса в качестве ключа и имя файлового ресцрса в качестве значения</param>
        /// <returns></returns>
        T SaveSource(UserIdentity sender, T descriptor, Dictionary<string, string> fileDic);
        /// <summary>
        /// Поиск дескрипторов по имени - результат НЕ содержит descriptor на входе
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        List<T> SearchByName(T descriptor);
        event Action<double, string> OnUploadSpeed;
        void UploadSpeed(double speed, string display);
    }
}
