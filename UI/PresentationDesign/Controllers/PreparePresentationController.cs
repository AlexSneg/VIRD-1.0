using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Domain.PresentationShow.ShowClient;
using TechnicalServices.Entity;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public delegate void WorkFinished(ShowClient.PreparationStatus status, String error, String warning);
    public delegate void WorkProgressChanged(int processed, int tota, string displayName);

    public class PreparePresentationController
    {
        private bool _canClose = false;
        private PresentationInfo _info = null;
        private ShowClient.PreparationStatus _preparationStatus = ShowClient.PreparationStatus.Ok;
        private String _status = "Выполняется подготовка сценария";

        public event WorkFinished OnWorkFinished;
        public event WorkProgressChanged OnProgressChanged;
        public event Action<DisplayType[], int[]> OnReceiveAgentResourcesList;
        public event Action<double, string, string> OnUploadSpeed;
        public event Action<string, bool> OnPreparationForDisplayEnded;
        public event Action<DisplayType> OnNotEnoughSpace;
        public event Action<string> OnLogMessage;
        int? _slideId;


        public PreparePresentationController(PresentationInfo info, int slideId)
        {
            _info = info;
            _slideId = slideId;
        }

        public PreparePresentationController(PresentationInfo info)
        {
            _info = info;
        }

        public bool CanClose
        {
            get { return _canClose; }
        }

        public void StartPrepare()
        {
            ShowClient.Instance.OnPreparationFinished += Instance_OnPreparationFinished;
            ShowClient.Instance.OnProgressChanged += Instance_OnProgressChanged;
            ShowClient.Instance.OnNotEnoughSpace += Instance_OnNotEnoughSpace;
            ShowClient.Instance.OnLogMessage += new Action<string>(Instance_OnLogMessage);
            ShowClient.Instance.OnReceiveAgentResourcesList += Instance_OnReceiveAgentResourcesList;
            ShowClient.Instance.OnUploadSpeed += new Action<double, string, string>(Instance_OnUploadSpeed);
            ShowClient.Instance.OnPreparationForDisplayEnded += new Action<string, bool>(Instance_OnPreparationForDisplayEnded);

            if(_slideId.HasValue)
                ShowClient.Instance.Load(_info, _slideId.Value, false);
            else
                ShowClient.Instance.Load(_info);
        }

        void Instance_OnLogMessage(string obj)
        {
            if (OnLogMessage != null)
            {
                OnLogMessage(obj);
            }
        }
        void Instance_OnPreparationForDisplayEnded(string obj, bool allOk)
        {
            if (OnPreparationForDisplayEnded != null)
                OnPreparationForDisplayEnded(obj, allOk);
        }



        void Instance_OnNotEnoughSpace(DisplayType obj)
        {
            if (OnNotEnoughSpace != null)
                OnNotEnoughSpace(obj);
        }

        void Instance_OnProgressChanged(int processed, int total, string displayName)
        {
            if (OnProgressChanged != null)
                OnProgressChanged(processed, total, displayName);
        }

        void Instance_OnReceiveAgentResourcesList(DisplayType[] agents, int[] resources)
        {
            if (OnReceiveAgentResourcesList != null)
                OnReceiveAgentResourcesList(agents, resources);
        }

        void Instance_OnUploadSpeed(double arg1, string arg2, string arg3)
        {
            if (OnUploadSpeed != null)
                OnUploadSpeed(arg1, arg2, arg3);
        }

        void Instance_OnPreparationFinished()
        {
            _canClose = true;
            ShowClient.Instance.OnPreparationFinished -= Instance_OnPreparationFinished;
            ShowClient.Instance.OnProgressChanged -= Instance_OnProgressChanged;
            ShowClient.Instance.OnNotEnoughSpace -= Instance_OnNotEnoughSpace;
            ShowClient.Instance.OnLogMessage -= new Action<string>(Instance_OnLogMessage);
            ShowClient.Instance.OnReceiveAgentResourcesList -= Instance_OnReceiveAgentResourcesList;
            ShowClient.Instance.OnUploadSpeed -= new Action<double, string, string>(Instance_OnUploadSpeed);
            ShowClient.Instance.OnPreparationForDisplayEnded -= new Action<string, bool>(Instance_OnPreparationForDisplayEnded);

            String error = String.Empty;
            String warning = string.Empty;
            _preparationStatus = ShowClient.Instance.HasError(_info, out error, out warning);
            switch (_preparationStatus)
            {
                    case ShowClient.PreparationStatus.Ok:
                    _status = "Подготовка сценария завершена успешно";
                    break;
                case ShowClient.PreparationStatus.Error:
                    _status = "Подготовка сценария завершена с ошибками";
                    break;
                case ShowClient.PreparationStatus.Warning:
                    _status = "Подготовка сценария завершена с предупреждениями";
                    break;
            }
            if (OnWorkFinished != null)
                OnWorkFinished(_preparationStatus, error, warning);
        }

        public void CheckPrepareResult()
        {
            //_hasErrors = ShowClient.Instance.HasError(_info, out _error);
            //_status = _hasErrors ? "Подготовка сценария завершена с ошибками" : "Подготовка сценария завершена успешно" ;
            switch (_preparationStatus)
            {
                case ShowClient.PreparationStatus.Ok:
                    _status = "Подготовка сценария завершена успешно";
                    break;
                case ShowClient.PreparationStatus.Error:
                    _status = "Подготовка сценария завершена с ошибками";
                    break;
                case ShowClient.PreparationStatus.Warning:
                    _status = "Подготовка сценария завершена с предупреждениями";
                    break;
            }
        }

        public void TerminateLoad(Domain.PresentationShow.ShowCommon.TerminateLoadCommand command, string display)
        {
            ShowClient.Instance.TerminateLoad(command, display);
            if (command == Domain.PresentationShow.ShowCommon.TerminateLoadCommand.StopAll)
            {
                ShowClient.Instance.OnPreparationFinished -= Instance_OnPreparationFinished;
                ShowClient.Instance.OnProgressChanged -= Instance_OnProgressChanged;
                ShowClient.Instance.OnNotEnoughSpace -= Instance_OnNotEnoughSpace;
                ShowClient.Instance.OnLogMessage -= new Action<string>(Instance_OnLogMessage);
                ShowClient.Instance.OnReceiveAgentResourcesList -= Instance_OnReceiveAgentResourcesList;
                ShowClient.Instance.OnUploadSpeed -= new Action<double, string, string>(Instance_OnUploadSpeed);
                ShowClient.Instance.OnPreparationForDisplayEnded -= new Action<string, bool>(Instance_OnPreparationForDisplayEnded);

                _preparationStatus = ShowClient.PreparationStatus.Error;
                _canClose = true;
            }

        }

        public String Status
        {
            get { return _status; }
        }

        public ShowClient.PreparationStatus PreparationStatus
        {
            get { return _preparationStatus; }
        }

        public void ResponseForSpace(DisplayType disp, bool p)
        {
            ShowClient.Instance.NotEnoughSpaceResponce(disp, p);
        }
    }
}
