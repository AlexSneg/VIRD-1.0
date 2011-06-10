using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces
{
    public class PresentationEventArg : EventArgs
    {
        private readonly PresentationInfo _presentationInfo;
        private readonly UserIdentity _userIdentity;
        public PresentationEventArg(PresentationInfo presentationInfo,
            UserIdentity userIdentity)
        {
            _presentationInfo = presentationInfo;
            _userIdentity = userIdentity;
        }

        public PresentationInfo PresentationInfo
        {
            get { return _presentationInfo; }
        }

        public UserIdentity UserIdentity
        {
            get { return _userIdentity; }
        }
    }

    public class ObjectChangedEventArg : EventArgs
    {
        private readonly List<ObjectKey> _objectList = new List<ObjectKey>();
        public ObjectChangedEventArg(IEnumerable<ObjectKey> objectList)
        {
            ObjectList.AddRange(objectList);
        }

        public List<ObjectKey> ObjectList
        {
            get { return _objectList; }
        }
    }


    public interface IPresentationDAL
    {
        void Init(ISourceDAL sourceDAL, IDeviceSourceDAL deviceSourceDAL);
        //ILockService LockService { get; set; }
        IList<PresentationInfo> GetPresentationInfoList();
        PresentationInfo GetPresentationInfo(string uniqueName);
        PresentationInfo GetPresentationInfoByPresentationName(string name);
        Slide[] LoadSlides(string presentationUniqueName, int[] slideIdArr, ISourceDAL sourceDAL,
            IDeviceSourceDAL deviceSourceDAL);
        //IList<Presentation> Import(params string[] files);
        //bool Export(string fileName, Presentation presentation);
        //bool Export(params Presentation[] presentationArr);
        //Stream GetPresentationAsStream(string uniqueName);
        Presentation GetPresentation(string uniqueName, ISourceDAL sourceDAL, IDeviceSourceDAL deviceSourceDAL, out string[] deletedElements);
        /// <summary>
        /// загрузка презентации из любого файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sourceDAL">нужен для загрузки источников</param>
        /// <param name="deviceSourceDAL">нужен для загрузки ресурсов девайсов</param>
        /// <param name="deletedElements">оборудование удаленное так как его нет в текущей конфигурации</param>
        /// <returns></returns>
        Presentation LoadPresentation(string fileName, ISourceDAL sourceDAL, IDeviceSourceDAL deviceSourceDAL, out string[] deletedElements);
        SlideBulk LoadSlideBulk(string fileName, ResourceDescriptor[] resourceDescriptors,
            DeviceResourceDescriptor[] deviceResourceDescriptors);
        /// <summary>
        /// возвращает полное имя файла - презентации. Юзать аккуратно!!!!
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <returns></returns>
        string GetPresentationFile(string uniqueName);
        //bool SavePresentationChanges(UserIdentity userIdentity, Stream presentation);
        bool SavePresentation(UserIdentity sender, Presentation presentation);
        void SaveSlideBulk(string fileName, SlideBulk slideBulk);
        //bool CreatePresentation(UserIdentity userIdentity, string uniqueName, string name);
        bool DeletePresentation(UserIdentity sender, Presentation presentation);
        bool DeletePresentation(UserIdentity sender, string uniqueName);
        //bool ContainsSource(Presentation presentation, ResourceDescriptor resourceDescriptor);
        PresentationInfo[] GetPresentationWhichContainsSource(ResourceDescriptor resourceDescriptor);
        PresentationInfo[] GetPresentationWhichContainsSource(DeviceResourceDescriptor deviceResourceDescriptor);
        PresentationInfo[] GetPresentationWhichContainsLabel(int labelId);
        event EventHandler<PresentationEventArg> OnPresentationAdded;
        event EventHandler<PresentationEventArg> OnPresentationDeleted;
    }
}
