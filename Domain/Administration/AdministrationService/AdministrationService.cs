using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TechnicalServices.Configuration.Common;
using TechnicalServices.Configuration.Server;
using Domain.Administration.AdministrationCommon;
using System.ServiceModel;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;


namespace Domain.Administration.AdministrationService
{
    
    [ServiceBehavior(
     InstanceContextMode = InstanceContextMode.Single,
     ConcurrencyMode = ConcurrencyMode.Single
        #if DEBUG
    , IncludeExceptionDetailInFaults = true
        #endif
    )]
    public class AdministrationService : IAdministrationService,  IDisposable
    {
        private readonly IServerConfiguration _serviceConfiguration;
        private readonly ILoginService _loginService;
        private readonly IPresentationWorker _worker;
        //private readonly ISystemParametersAdapter _systemParameters;

        public AdministrationService(IServerConfiguration serviceConfiguration, ILoginService loginService, IPresentationWorker worker)
        {
            Debug.Assert(serviceConfiguration != null, "IServerConfiguration не может быть null");
            Debug.Assert(loginService != null, "ILoginService не может быть null");
            Debug.Assert(worker != null, "IPresentationWorker не может быть null");
            
            _serviceConfiguration = serviceConfiguration;
            _loginService = loginService;
            _worker = worker;
            //_systemParameters = new SystemParametersAdapter();
        }

        #region Implementation of IAdministrationService : IUserService

        public UserInfo[] GetUserStorage()
        {
            return _serviceConfiguration.UserStorageAdapter.GetUserStorage();
        }

        public UserError AddUser(UserInfo userInfo)
        {
            return _serviceConfiguration.UserStorageAdapter.AddUser(userInfo);
        }

        public UserError DeleteUser(UserInfo userInfo)
        {
            if (IsUserLogin(userInfo))
            {
                return UserError.WorkInSystem;
            }
            /*if (userInfo == null)
            {
                return UserError.DeletedAlready;
            }*/
            return _serviceConfiguration.UserStorageAdapter.DeleteUser(userInfo);
        }

        public UserError UpdateUser(UserInfo userInfo)
        {
            if(IsUserLogin(userInfo))
            {
                return UserError.WorkInSystem;
            }
            return _serviceConfiguration.UserStorageAdapter.UpdateUser(userInfo);
        }

        
        public UserError UnlockUser(UserInfo userInfo)
        {
            return _serviceConfiguration.UserStorageAdapter.UnlockUser(userInfo);
        }

        public UserError LockUser(UserInfo userInfo)
        {
            if (IsUserLogin(userInfo))
            {
                return UserError.WorkInSystem;
            }
            return _serviceConfiguration.UserStorageAdapter.LockUser(userInfo);
        }


       

        private bool IsUserLogin(UserInfo userInfo)
        {
            IList<UserIdentity> userIdentities = _loginService.GetUserLoginCollection();
            if (userInfo!=null && userIdentities.Any(x => x.User.Id == userInfo.Id))
            {
                return true;
            }
            return false;
        }

        public UserInfo FindSystemUser()
        {
            return _serviceConfiguration.UserStorageAdapter.FindSystemUser();
        }
        #endregion


        #region Implementation of IAdministrationService : ILabelService

        public Label[] GetLabelStorage()
        {
            return _serviceConfiguration.LabelStorageAdapter.GetLabelStorage();
        }

        public LabelError AddLabel(Label labelInfo)
        {
            return _serviceConfiguration.LabelStorageAdapter.AddLabel(labelInfo);
        }

        public LabelError DeleteLabel(Label labelInfo) 
        {
            CheckLabelUsedPresentation("Нельзя удалить метку", labelInfo.Id);
            return _serviceConfiguration.LabelStorageAdapter.DeleteLabel(labelInfo);
        }

        public LabelError UpdateLabel(Label labelInfo)
        {
            
            return _serviceConfiguration.LabelStorageAdapter.UpdateLabel(labelInfo);
        }


        public LabelError UnlockLabel(Label labelInfo)
        {
            return _serviceConfiguration.LabelStorageAdapter.UnlockLabel(labelInfo);
        }

        public LabelError LockLabel(Label labelInfo)
        {
            CheckLabelUsedPresentation("Нельзя редактировать метку", labelInfo.Id);
            return _serviceConfiguration.LabelStorageAdapter.LockLabel(labelInfo);
        }

        void CheckLabelUsedPresentation(string message, int labelId)
        {
            PresentationInfo[] presentationInfos = _worker.GetPresentationWhichContainsLabel(labelId);
            if (presentationInfos.Count() > 0)
            {
                List<string> list = new List<string>();
                list.AddRange(presentationInfos.Select(x => x.Name));

                string result = message + ". Метка используется в сценарии(ях):";
                foreach (string str in list)
                {
                    result += Environment.NewLine + str;
                }
                LabelUsedInPresentationException deleteError = new LabelUsedInPresentationException(result);
                throw new FaultException<LabelUsedInPresentationException>(deleteError, new FaultReason("Нельзя редактировать/удалять исользуемую метку"));
            }
        }

        #endregion

        #region Implementation of IAdministrationService : ISystemServices

        public PresentationInfoExt[] LoadPresentationWithOneSlide()
        {
            IList<PresentationInfoExt> presentationList =
                ((List<PresentationInfoExt>)_worker.GetPresentationInfoList()).FindAll(x => x.SlideCount == 1);
            return presentationList.ToArray();
        }

        public ISystemParameters LoadSystemParameters()
        {
                return _serviceConfiguration.LoadSystemParameters();
        }
        
        public void SaveSystemParameters(ISystemParameters systemParameters)
        {
            try
            {
                _serviceConfiguration.SaveSystemParameters(systemParameters);
            }
            catch (SystemParametersSaveException ex)
            {
                SystemParametersSaveException deleteError = new SystemParametersSaveException(ex.Message);
                throw new FaultException<SystemParametersSaveException>(deleteError, new FaultReason(ex.Message));
            }
        }
        
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion


    }
}
