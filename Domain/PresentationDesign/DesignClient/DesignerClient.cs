using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Domain.PresentationDesign.DesignCommon;

using DomainServices.EnvironmentConfiguration.ConfigModule;

using TechnicalServices.Communication.Communication.Client;
using TechnicalServices.Configuration.Client;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPresentation;
using TechnicalServices.Security.SecurityCommon;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces.ConfigModule.Designer;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Common.Classes;

namespace Domain.PresentationDesign.Client
{
    public class DesignerClient : IDesignerClient
    {
        private static readonly DesignerClient _instance = new DesignerClient();
        //private IDesignerService _designerService;
        private readonly List<UserIdentity> _userCollection = new List<UserIdentity>();
        private DuplexClient<ILoginService> _loginServiceClient;
        private IClientConfiguration _configuration;
        private IPresentationClient _standalonePresentationClient;
        private IPresentationClient _remotePresentationClient;
        private ISourceDAL _sourceDAL;
        private IDeviceSourceDAL _deviceSourceDAL;
        private Dictionary<Type, IModule> _moduleMapping = null;

        #region module mapping

        private IModule getModule(Type sourceType)
        {
            if (_moduleMapping == null)
            {
                if (_configuration == null)
                    return null;

                _moduleMapping = new Dictionary<Type, IModule>();
                foreach (IModule module in _configuration.ModuleList)
                    foreach (
                        Type type in
                            module.SystemModule.Presentation.GetSource().Concat(
                                module.SystemModule.Configuration.GetDevice()).Concat(
                                module.SystemModule.Configuration.GetSource()))
                        _moduleMapping.Add(type, module);
            }

            IModule mod = null;
            if (_moduleMapping.TryGetValue(sourceType, out mod))
                return mod;
            return null;
        }

        public IPlayerModule GetPlayerModule(Type sourceType)
        {
            IModule module = getModule(sourceType);
            if (module != null)
                return module.PlayerModule;
            return null;
        }

        public IDesignerModule GetDesignerModule(Type sourceType)
        {
            IModule module = getModule(sourceType);
            if (module != null)
                return module.DesignerModule;
            return null;
        }

        #endregion

        private PresentationNotifier _presentationNotifier = null; //   new PresentationNotifier();

        public IPresentationClient PresentationWorker
        {
            get
            {
                if (IsStandAlone)
                {
                    if (_standalonePresentationClient == null)
                        _standalonePresentationClient = PresentationClientFactory.CreateStandAlonePresentationWorker(
                            SourceDAL, DeviceSourceDAL, ClientConfiguration, PresentationNotifier);
                    return _standalonePresentationClient;
                }
                else
                {
                    if (_remotePresentationClient == null)
                    {
                        RemotePresentationClient remotePresentationClient = PresentationClientFactory.CreatePresentationClient(SourceDAL, DeviceSourceDAL,
                            PresentationNotifier, ClientConfiguration);
                        remotePresentationClient.OnChanged += new EventHandler<ClientState>(_presentationClient_OnChanged);
                        _remotePresentationClient = remotePresentationClient;
                    }
                    return _remotePresentationClient;
                }

                //if (_presentationWorker == null)
                //{
                //    _presentationWorker = PresentationClientFactory.CreatePresentationWorker(IsStandAlone, _presentationNotifier);
                //}

                //return _presentationWorker;
            }
        }

        public IPresentationClient StandalonePresentationWorker
        {
            get
            {
                if (_standalonePresentationClient == null)
                    _standalonePresentationClient = PresentationClientFactory.CreateStandAlonePresentationWorker(
                        SourceDAL, DeviceSourceDAL, ClientConfiguration, PresentationNotifier);
                return (IPresentationClient)_standalonePresentationClient;
            }
        }

        public void Reconnect()
        {
            //TODO : Переподключение при обрыве связи !
        }

        void _presentationClient_OnChanged(object sender, ClientState e)
        {
            this.State = e.State;
            PresentationNotifier.StateChanged(e.State);
        }

        public PresentationNotifier PresentationNotifier
        {
            get
            {
                if (_presentationNotifier == null)
                {
                    _presentationNotifier = new PresentationNotifier(_configuration);
                    _presentationNotifier.OnLabelAdded += _presentationNotifier_OnLabelAdded;
                    _presentationNotifier.OnLabelDeleted += _presentationNotifier_OnLabelDeleted;
                    _presentationNotifier.OnLabelUpdated += _presentationNotifier_OnLabelUpdated;
                }
                return _presentationNotifier;
            }
        }

        private void _presentationNotifier_OnLabelUpdated(object sender, NotifierEventArg<Label> e)
        {
            _configuration.LabelStorageAdapter.UpdateLabel(e.Data);
        }

        private void _presentationNotifier_OnLabelDeleted(object sender, NotifierEventArg<Label> e)
        {
            _configuration.LabelStorageAdapter.DeleteLabel(e.Data);
        }

        private void _presentationNotifier_OnLabelAdded(object sender, NotifierEventArg<Label> e)
        {
            _configuration.LabelStorageAdapter.AddLabel(e.Data);
        }

        public IClientConfiguration ClientConfiguration
        {
            get { return _configuration; }
        }

        public ISourceDAL SourceDAL
        {
            get { return _sourceDAL; }
        }

        public IDeviceSourceDAL DeviceSourceDAL
        {
            get { return _deviceSourceDAL; }
        }

        private ILoginService loginService
        {
            get { return _loginServiceClient.Channel; }
        }

        [CallbackBehavior(UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Reentrant)]
        private class LoginServiceCallback : ILoginNotifier
        {
            private readonly List<UserIdentity> _list;

            public LoginServiceCallback(List<UserIdentity> list)
            {
                _list = list;
            }

            #region ILoginServiceCallback Members

            public void LoginStatusChange(UserIdentity user, LogOnStatus newLoginStatus)
            {
                switch (newLoginStatus)
                {
                    case LogOnStatus.LogOn:
                        _list.Add(user);
                        break;
                    case LogOnStatus.LogOff:
                        //int index = _list. FindIndex(value => value.Name == user.Name);
                        //if (index >= 0)
                        //    _list.RemoveAt(index);
                        _list.Remove(user);
                        break;
                    default:
                        throw new ApplicationException("Ненене Дэвид Блейн ненене");
                }
            }

            #endregion
        }

        private LoginServiceCallback _callBack;

        private DesignerClient()
        {
        }

        public CommunicationState State
        {
            get;
            set;
        }

        public static DesignerClient Instance
        {
            get { return _instance; }
        }

        public bool IsStandAlone { get; private set; }

        public ValuePair<int, int> DefaultWndsize
        {
            get 
            {
                string[] tmp = ClientConfiguration.LoadSystemParameters().DefaultWndsize.Split('*');
                ValuePair<int, int> result;
                try
                {
                    result = new ValuePair<int, int>(Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]));
                }
                catch
                {
                    result = new ValuePair<int, int>(800, 600);
                }
                return result;
            }
        }

        public void Initialize(IClientConfiguration configuration, bool isStandAlone)
        {
            _configuration = configuration;
            IsStandAlone = isStandAlone;
            _sourceDAL = new SourceDALCaching(_configuration, isStandAlone);
            _deviceSourceDAL = new DeviceSourceDALCaching(_configuration, isStandAlone);
        }

        public bool Authenticate(UserIdentity previousId)
        {
            if (_loginServiceClient == null)
            {
                _callBack = new LoginServiceCallback(_userCollection);
                _loginServiceClient = new DuplexClient<ILoginService>(new InstanceContext(_callBack), _configuration.PingInterval);
                _loginServiceClient.Open();
                _loginServiceClient.OnChanged += new EventHandler<ClientState>(_loginServiceClient_OnChanged);
            }

            UserIdentity principal = loginService.Login(previousId.User.Name, previousId.User.Hash, Environment.MachineName);
            if (principal == null || !principal.IsAuthenticated)
            {
                return false;
            }

            AppDomain.CurrentDomain.SetThreadPrincipal(principal);

            loginService.Subscribe(principal);

            _userCollection.AddRange(loginService.GetUserLoginCollection());
            return true;
        }

        public bool Authenticate(IUserCredential tool, string role)
        {
            if (IsStandAlone)
            {
                UserIdentity principal = new UserIdentity(UserInfo.Empty, true, new TimeSpan(0), "local");
                AppDomain.CurrentDomain.SetThreadPrincipal(principal);

                _userCollection.Add(principal);
            }
            else
            {
                if (_loginServiceClient == null)
                {
                    _callBack = new LoginServiceCallback(_userCollection);
                    _loginServiceClient = new DuplexClient<ILoginService>(new InstanceContext(_callBack), _configuration.PingInterval);
                    _loginServiceClient.Open();
                    _loginServiceClient.OnChanged += new EventHandler<ClientState>(_loginServiceClient_OnChanged);
                }
                string loginName, password;
            again: ;

                if (tool.GetUserCredential(out loginName, out password))
                {
                    byte[] hash = SecurityUtils.PasswordToHash(password);

                    UserIdentity principal = loginService.Login(loginName, hash, Environment.MachineName);
                    if (principal == null || !principal.IsAuthenticated)
                    {
                        tool.FailedLogin();
                        goto again;
                    }

                    if (role != null && !principal.IsInRole(role))
                    {
                        tool.FailedRole(role);
                        loginService.Logoff(principal);
                        goto again;
                    }

                    AppDomain.CurrentDomain.SetThreadPrincipal(principal);

                    loginService.Subscribe(principal);

                    _userCollection.AddRange(loginService.GetUserLoginCollection());
                }
                else
                {
                    return false;
                    //throw new ApplicationException("User cancel");
                }
            }
            return true;
        }

        void _loginServiceClient_OnChanged(object sender, ClientState e)
        {
            if (e.State == CommunicationState.Opened)
            {
                UserIdentity identity = Thread.CurrentPrincipal as UserIdentity;
                if (identity != null)
                    loginService.Subscribe(identity);
            }
        }

        public void Done()
        {
            if (IsStandAlone) return;
            UserIdentity identity = Thread.CurrentPrincipal as UserIdentity;
            if (_loginServiceClient != null)
            {
                try
                {
                    if (identity != null)
                    {
                        loginService.UnSubscribe(identity);
                        loginService.Logoff(identity);
                    }
                }
                catch (Exception ex)
                {
                    _configuration.EventLog.WriteError(String.Format("DesignerClient.Done(): {0}", ex));
                }
                finally
                {
                    _loginServiceClient.Dispose();
                    _loginServiceClient = null;
                }
            }

            if (_remotePresentationClient != null)
            {
                try
                {
                    if (identity != null)
                    {
                        PresentationWorker.UnSubscribeForGlobalMonitoring();
                    }
                }
                catch (Exception ex)
                {
                    _configuration.EventLog.WriteError(String.Format("DesignerClient.Done(): {0}", ex));
                }
                finally
                {
                    _remotePresentationClient.Dispose();
                    _remotePresentationClient = null;
                }
            }
        }
    }
}

