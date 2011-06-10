using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using Domain.Administration.AdministrationCommon;
using TechnicalServices.Communication.Communication.Client;
using TechnicalServices.Configuration.Client;
using TechnicalServices.Configuration.Common;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;


namespace Domain.Administration.AdministrationClient
{
    public class AdministrationClient: IAdministrationService
    {
        private static AdministrationClient _instance = new AdministrationClient();
        private IClientConfiguration _configuration;
        private AdministrationClient()
        {
        }
        public static AdministrationClient Instance
        {
            get { return _instance; }
        }
        
        private SimpleClient<IAdministrationService> _svc;

        //public IClientConfiguration ClientConfiguration
        //{
            //get { return _configuration; }
        //}

        public bool Initialize(IClientConfiguration configuration)
        {
            using (_svc = new SimpleClient<IAdministrationService>())
            {
                try
                {
                    _svc.Open();
                }
                catch (Exception /*ex*/)
                {
                    return false;
                }
                _configuration = configuration;
                return true;
            }
        }
        #region User
        public UserInfo[] GetUserStorage()
        {
            using (_svc = new SimpleClient<IAdministrationService>())
            {
                try
                {
                    _svc.Open();
                }
                catch (Exception /*ex*/)
                {
                    NoConnectionException error = new NoConnectionException();
                    throw error;
                }
                return _svc.Channel.GetUserStorage();
            }
        }

        public UserInfo FindSystemUser()
        {
            using (_svc = new SimpleClient<IAdministrationService>())
            {
                try
                {
                    _svc.Open();
                }
                catch (Exception /*ex*/)
                {
                    NoConnectionException error = new NoConnectionException();
                    throw error;
                }
                return _svc.Channel.FindSystemUser();
            }
        }

        public UserError AddUser(UserInfo userInfo)
        {
            return UserHandler("AddUser", userInfo);
        }

        public UserError DeleteUser(UserInfo userInfo)
        {
            return UserHandler("DeleteUser", userInfo);
        }

        public UserError UpdateUser(UserInfo userInfo)
        {
            return UserHandler("UpdateUser", userInfo);
        }

        public UserError UnlockUser(UserInfo userInfo)
        {
            return UserHandler("UnlockUser", userInfo);
        }

        public UserError LockUser(UserInfo userInfo)
        {
            return UserHandler("LockUser", userInfo);
        }

        private UserError UserHandler(string proc, UserInfo userInfo)
        {
            using (_svc = new SimpleClient<IAdministrationService>())
            {
                try
                {
                    _svc.Open();
                }
                catch (Exception /*ex*/)
                {
                    NoConnectionException error = new NoConnectionException();
                    throw error;
                }
                Type channel = typeof(IUserService);
                MethodInfo method = channel.GetMethod(proc, new Type[] { typeof(UserInfo)});
                object[] parameters ={userInfo};
                return (UserError)method.Invoke(_svc.Channel, parameters);
            }
        }
        #endregion

        #region Label
        public Label[] GetLabelStorage()
        {
            using (_svc = new SimpleClient<IAdministrationService>())
            {
                try
                {
                    _svc.Open();
                }
                catch (Exception /*ex*/)
                {
                    NoConnectionException error = new NoConnectionException();
                    throw error;
                }
                return _svc.Channel.GetLabelStorage();
            }
        }

        public LabelError AddLabel(Label labelInfo)
        {
            return LabelHandler("AddLabel", labelInfo);
        }

        public LabelError DeleteLabel(Label labelInfo)
        {
            return LabelHandler("DeleteLabel", labelInfo);
        }

        public LabelError UpdateLabel(Label labelInfo)
        {
            return LabelHandler("UpdateLabel", labelInfo);
        }

        public LabelError UnlockLabel(Label labelInfo)
        {
            return LabelHandler("UnlockLabel", labelInfo);
        }

        public LabelError LockLabel(Label labelInfo)
        {
            return LabelHandler("LockLabel", labelInfo);
        }


        private LabelError LabelHandler(string proc, Label labelInfo)
        {
            using (_svc = new SimpleClient<IAdministrationService>())
            {
                try
                {
                    _svc.Open();
                }
                catch (Exception /*ex*/)
                {
                    NoConnectionException error = new NoConnectionException();
                    throw error;
                }
                Type channel = typeof(ILabelService);
                MethodInfo method = channel.GetMethod(proc, new Type[] { typeof(Label) });
                object[] parameters = {labelInfo};
                try
                {
                    return (LabelError)method.Invoke(_svc.Channel, parameters);
                }
                catch (TargetInvocationException tiEx)
                {
                    if (tiEx.InnerException is FaultException<LabelUsedInPresentationException>)
                    {
                        throw new LabelUsedInPresentationException(((FaultException<LabelUsedInPresentationException>)tiEx.InnerException).Detail.Message);
                    }
                    throw tiEx;         
                }
            }
        }

        #endregion

        #region SystemParameters

        public ISystemParameters LoadSystemParameters()
        {
            using (_svc = new SimpleClient<IAdministrationService>())
            {
                try
                {
                    _svc.Open();
                }
                catch (Exception /*ex*/)
                {
                    NoConnectionException error = new NoConnectionException();
                    throw error;
                }
                return _svc.Channel.LoadSystemParameters();
            }
        }

        public void SaveSystemParameters(ISystemParameters systemParameters)
        {
            using (_svc = new SimpleClient<IAdministrationService>())
            {
                try
                {
                    _svc.Open();
                }
                catch (Exception /*ex*/)
                {
                    NoConnectionException error = new NoConnectionException();
                    throw error;
                }

                try
                {
                    _svc.Channel.SaveSystemParameters(systemParameters);
                }
                catch (FaultException Ex)
                {
                   throw new SystemParametersSaveException(Ex.Message);
                }

            }
        }

        public PresentationInfoExt[] LoadPresentationWithOneSlide()
        {
            using (_svc = new SimpleClient<IAdministrationService>())
            {
                try
                {
                    _svc.Open();
                }
                catch (Exception /*ex*/)
                {
                    NoConnectionException error = new NoConnectionException();
                    throw error;
                }
                return _svc.Channel.LoadPresentationWithOneSlide();
            }
        
        }

        #endregion
    }
}
