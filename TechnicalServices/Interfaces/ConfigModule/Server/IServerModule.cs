using System;
using System.IO;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Interfaces.ConfigModule.Server
{
    public class EqiupmentStateChangeEventArgs : EventArgs
    {
        private readonly bool _isOnLine;
        private readonly EquipmentType _equipmentType;
        public EqiupmentStateChangeEventArgs(EquipmentType equipmentType, bool isOnLine)
        {
            _isOnLine = isOnLine;
            _equipmentType = equipmentType;
        }

        public bool IsOnLine {get { return _isOnLine;}}
        public EquipmentType EquipmentType { get { return _equipmentType; } }
    }

    public interface IServerModule
    {
        void Init(IConfiguration config, IModule module, IControllerChannel controller);
        void CheckLicense();
        void Done();
        MemoryStream CaptureScreen(DisplayType displayType);
        void CloseWindows();
        void ShowDisplay(Display display, BackgroundImageDescriptor backgroundImageDescriptor);
        bool IsOnLine(EquipmentType equipmentType);
        //ResourceForUpload[] GetResourcesForUpload(Display display, ResourceDescriptor[] resourceDescriptors,
        //    out bool isEnoughFreeSpace);
        ResourceDescriptor[] GetResourcesForUpload(DisplayType displayType, ResourceDescriptor[] resourceDescriptors,
            out bool isEnoughFreeSpace);

        /// <summary>
        /// загрузка ресурсов на агента
        /// </summary>
        /// <param name="displayType">Дисплей из конфигурации</param>
        /// <param name="resourceDescriptors">ресурсы для загрузки</param>
        /// <param name="sourceDAL">сорсдал который будет юзаться для поиска ресурса</param>
        /// <returns>ресурсы которые не удалось загрузить</returns>
        ResourceDescriptor[] UploadResources(DisplayType displayType,
            ResourceDescriptor[] resourceDescriptors, ISourceDAL sourceDAL);

        /// <summary>
        /// Удалить только что загруженные ресурсы.
        /// </summary>
        void DeleteResourcesUploaded(DisplayType displayType,
            ResourceDescriptor[] resourceDescriptors);

        //ResourceDescriptor[] UploadResources(Display display,
        //    ResourceForUpload[] resourceDescriptors, ISourceDAL sourceDAL);
        /// <summary>
        /// останов загрузки ресурсов - может вызываться, даже если процесс загрузки не начат
        /// </summary>
        void TerminateUpload();
        /// <summary>
        /// Останов загрузки для клиента с определенным именем.
        /// </summary>
        /// <param name="client">Имя клиента.</param>
        void TerminateUpload(string client);
        void DeleteAllResources(DisplayType displayType);
        string DoSourceCommand(DisplayType displayType, string sourceId, string command);
        /// <summary>
        /// преостановление показа текущего салйда
        /// </summary>
        void Pause();
        /// <summary>
        /// получение списка комманд которые необходимо выполнить при переходе к новому сладйу - актуально для хардварных устройств
        /// </summary>
        /// <param name="slide1">предыдущий слайд или null если не было предыдущего слайда</param>
        /// <param name="slide2">текущий слайд</param>
        /// <returns>список команд которые необходимо послать контроллеру</returns>
        CommandDescriptor[] GetCommand(Slide slide1, Slide slide2);
        CommandDescriptor[] GetCommand(Slide slide1, Slide slide2, EquipmentType[] freezedEquipment);
        event EventHandler OnResourceTransmit;
        event Action<double, string, string> OnUploadSpeed;
        event EventHandler<EqiupmentStateChangeEventArgs> OnStateChange;
    }
}