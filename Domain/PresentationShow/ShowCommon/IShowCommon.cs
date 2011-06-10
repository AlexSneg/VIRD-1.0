using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;

using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Domain.PresentationShow.ShowCommon
{
    /// <summary>
    /// статус для проверки хода подготовки к показу
    /// </summary>
    [Serializable]
    public enum PresentationShowPrepareStatus
    {
        ResourceLoading,
        ReadyToShow
    }

    /// <summary>
    /// действие при нехватке свободного места на агенте
    /// </summary>
    [Serializable]
    public enum AgentAction
    {
        /// <summary>
        /// кирдыкнуть все ресурсы
        /// </summary>
        Delete,
        /// <summary>
        /// плюнуть и продолжить
        /// </summary>
        Continue
    }

    /// <summary>
    /// результат показа сладйа
    /// </summary>
    [DataContract]
    public class ShowSlideResult : IEquatable<ShowSlideResult>
    {
        [DataMember] private string _errorMessage;
        [DataMember] private bool _isSuccess;

        public ShowSlideResult()
        {
        }

        public ShowSlideResult(bool isSuccess, string errorMessage)
        {
            _isSuccess = isSuccess;
            _errorMessage = errorMessage;
        }

        public bool IsSuccess
        {
            get { return _isSuccess; }
            set { _isSuccess = value; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public static ShowSlideResult Ok
        {
            get { return new ShowSlideResult(true, string.Empty); }
        }

        #region Implementation of IEquatable<ShowSlideResult>

        public bool Equals(ShowSlideResult other)
        {
            return IsSuccess == other.IsSuccess;
        }

        #endregion
    }

    [ServiceContract(SessionMode = SessionMode.Required,
        CallbackContract = typeof (IShowNotifier))]
    public interface IShowCommon : IPing
    {
        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        MemoryStream[] CaptureScreen(DisplayType[] displayList);

        /// <summary>
        /// показ или превью можно начинать когда PresentationShowPrepareStatus = ReadyToShow
        /// </summary>
        /// <param name="info">PresentationInfo</param>
        /// <returns></returns>
        [OperationContract(Name = "CheckStatusByPresentationInfo")]
        PresentationShowPrepareStatus CheckStatus(UserIdentity userIdentity, PresentationInfo info);

        /// <summary>
        /// показ или превью можно начинать когда PresentationShowPrepareStatus = ReadyToShow
        /// </summary>
        /// <param name="uniqueName">уникальное имя сценария</param>
        /// <returns></returns>
        [OperationContract(Name = "CheckStatusByPresentationUniqueName")]
        PresentationShowPrepareStatus CheckStatus(UserIdentity userIdentity, string uniqueName);

        //[OperationContract(Name = "LoadByPresentationInfo")]
        //bool Load(UserIdentity userIdentity, PresentationInfo info);

        [OperationContract(Name = "LoadByPresentationUniqueName")]
        bool Load(UserIdentity userIdentity, string uniqueName);

        [OperationContract(Name = "LoadSlide")]
        bool Load(UserIdentity userIdentity, string uniqueName, int slideId, bool autoPrepare);

        [OperationContract]
        bool TerminateLoad(TerminateLoadCommand command, string display);

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        void ResponseForNotEnoughFreeSpaceRequest(DisplayType displayType, AgentAction agentAction);

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        PreparationResult GetPreparationResult();

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        bool Start(UserIdentity identity, PresentationInfo info);

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        bool Stop(UserIdentity identity, PresentationInfo info);

        [OperationContract]
        bool Pause(string presentationUniqueName);

        [OperationContract]
        void CloseWindows();

        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        ShowSlideResult ShowSlideBySlideInfo(PresentationInfo info, SlideInfo slide, out bool isPrevEnable,
                                             out bool isNextEnable);

        [OperationContract(Name = "ShowSlideBySlideUniqueName")]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        ShowSlideResult ShowSlide(string uname, SlideInfo slide, out bool isPrevEnable, out bool isNextEnable);

        [OperationContract(Name = "ShowSlideBySlideIdAndUniqueName")]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        ShowSlideResult ShowSlide(string uname, int slideId, out bool isPrevEnable, out bool isNextEnable);

        //[ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        //bool ShowSlideBySlideId(PresentationInfo info, int slideId, out bool isPrevEnable, out bool isNextEnable);

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        ShowSlideResult ShowSlideByLabelId(PresentationInfo info, int labelId, out bool isPrevEnable,
                                           out bool isNextEnable);

        [OperationContract]
        ShowSlideResult GoToPrevSlide(out int slideId, out bool isPrevEnable, out bool isNextEnable);

        [OperationContract(Name = "GoToNextSlideDefault")]
        ShowSlideResult GoToNextSlide(out int slideId, out bool isPrevEnable, out bool isNextEnable);

        [OperationContract(Name = "GoToNextSlide")]
        ShowSlideResult GoToNextSlide(int nextSlideId, out int slideId, out bool isPrevEnable, out bool isNextEnable);

        /// <summary>
        /// команды для софтварных сорсов
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [OperationContract]
        //[ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        String DoSourceCommand(string sourceId, String command);

        /// <summary>
        /// команды ТОЛЬКО для аппаратных устройтсв
        /// Если команда будет посланна програмному источнику вернется null
        /// </summary>
        /// <param name="cmd">команда, будет передана контроллеру</param>
        /// <returns>ответ</returns>
        [OperationContract]
        String DoEquipmentCommand(CommandDescriptor cmd);

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        void FreezeEquipmentSetting(EquipmentType equipmentType, FreezeStatus status);

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        EquipmentType[] GetFreezedEquipment();

        [OperationContract]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        bool IsOnLine(EquipmentType equipmentType);

        [OperationContract]
        bool StartPlayer();

        [OperationContract]
        void StopPlayer();

        [OperationContract]
        bool IsShownByPlayer();

        [OperationContract]
        void SubscribeForNotification(UserIdentity identity);

        [OperationContract]
        void UnSubscribeForNotification(UserIdentity identity);
    }

    /// <summary>
    /// Варианты комманды прекращения загрузки ресурсов.
    /// </summary>
    public enum TerminateLoadCommand
    {
        /// <summary>
        /// Приостановить загрузку.
        /// </summary>
        PauseLoad,
        /// <summary>
        /// Отменить загрузку.
        /// </summary>
        StopLoad,
        /// <summary>
        /// Отменить загрузку для всех дисплеев.
        /// </summary>
        StopAll,
        /// <summary>
        /// Возобновить загрузку.
        /// </summary>
        ResumeLoad,
        /// <summary>
        /// Очистить дисковое пространство на агенте.
        /// </summary>
        ClearSpace,
        /// <summary>
        /// Завершить процесс загрузки источников.
        /// </summary>
        EndSourceUpload
    }

    public interface IShowNotifier
    {
        [OperationContract(IsOneWay = true)]
        void PreparationFinished();

        [OperationContract(IsOneWay = true)]
        void UploadProgress(int currentResource, int totalResources, string displayName);

        /// <summary>
        /// Получено новое значение скорости записи ресурса.
        /// </summary>
        /// <param name="speed">Скорость.</param>
        /// <param name="displayName">Агент.</param>
        /// <param name="file">Текущий файл.</param>
        [OperationContract(IsOneWay = true)]
        void UploadSpeed(double speed, string displayName, string file);
        /// <summary>
        /// Уведомление об окончании загрузки источников для дисплея.
        /// </summary>
        /// <param name="displayName">Имя дисплея</param>
        [OperationContract(IsOneWay = true)]
        void PreparationForDisplayEnded(string displayName, bool allOk);
        /// <summary>
        /// Занесение ошибки в лог.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void LogMessage(string messge);


        /// <summary>
        /// Получен список ресурсов для агентов.
        /// </summary>
        /// <param name="agents">Агенты.</param>
        /// <param name="resources">Ресурсы.</param>
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetAllKnownTypes", typeof(KnownTypeProvider))]
        void ReceiveAgentResourcesList(DisplayType[] agents, int[] resources);

        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        void NotEnoughFreeSpaceRequest(DisplayType displayType);

        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetAllKnownTypes", typeof (KnownTypeProvider))]
        void EquipmentStateChange(EquipmentType equipmentType, bool isOnLine);

        [OperationContract(IsOneWay = true)]
        void GotoSlideByExternalSystem(int slideId, bool isPrevEnable, bool isNextEnable);

        [OperationContract(IsOneWay = true)]
        void ShownStatusChanged(bool isShownByPlayer);

        [OperationContract(IsOneWay = true)]
        void Ping();
    }
}