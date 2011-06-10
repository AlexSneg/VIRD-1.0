using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Common;
using TechnicalServices.Common.Notification;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Security.Security;

namespace TechnicalServices.Security.Security
{
    public class LoginService : ILoginService
    {
        private readonly IServerConfiguration _configuration;
        private readonly LoginStorage _loginStorage;

        NotificationManager<LoginService>.NotificationStore<UserIdentity,ILoginNotifier> _store;

        public LoginService(IServerConfiguration configuration)
        {
            _configuration = configuration;
            _loginStorage = new LoginStorage(_configuration);
            _store =
                NotificationManager<LoginService>.Instance.RegisterDuplexService<UserIdentity, ILoginNotifier>(
                    NotifierBehaviour.OneInstance);
            _loginStorage.OnAddItem += new EventHandler<KeyCollection<UserIdentity>.KeyEventArgs>(_loginStorage_OnAddItem);
            _loginStorage.OnRemoveItem += new EventHandler<KeyCollection<UserIdentity>.KeyEventArgs>(_loginStorage_OnRemoveItem);
        }

        public UserIdentity FindSystemIdentity()
        {
            return _loginStorage.FindSystemUserIdentity();
        }

        void _loginStorage_OnRemoveItem(object sender, KeyCollection<UserIdentity>.KeyEventArgs e)
        {
            _store.Notify(e.Key, this, "LoginStatusChange", e.Key, LogOnStatus.LogOff);
        }

        void _loginStorage_OnAddItem(object sender, KeyCollection<UserIdentity>.KeyEventArgs e)
        {
            _store.Notify(e.Key, this, "LoginStatusChange", e.Key, LogOnStatus.LogOn);
        }

        #region ILoginService Members

        public TechnicalServices.Entity.UserIdentity Login(string name, byte[] hash, string hostName)
        {
            return _loginStorage.Login(name, hash, hostName);
        }

        public void Logoff(TechnicalServices.Entity.UserIdentity user)
        {
            _loginStorage.Logoff(user);
        }

        public IList<UserIdentity> GetUserLoginCollection()
        {
            return _loginStorage.GetUserLoginCollection();
        }

        public void Subscribe(UserIdentity user)
        {
            _store.Subscribe(this, user);
        }

        public void UnSubscribe(UserIdentity user)
        {
            _store.Unsubscribe(this, user);
        }

        #endregion

        #region Implementation of IPing

        public void Ping(UserIdentity identity)
        {
            if (identity != null)
                _store.RefreshSubscribers(identity);
        }

        #endregion
    }
}