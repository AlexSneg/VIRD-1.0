using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using TechnicalServices.Common;
using TechnicalServices.Configuration.Server;
using TechnicalServices.Entity;

namespace TechnicalServices.Security.Security
{
    // IK: Необходим спец. логин для системы, 
    // чтобы блокировать текущий слайд показа презентации
    // который ессно нельзя редактировать
    public class LoginStorage : KeyCollection<UserIdentity>
    {
        private readonly IServerConfiguration _configuration;

        public LoginStorage(IServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        private TimeSpan GenerateCookie()
        {
            return new TimeSpan(DateTime.Now.Ticks);
        }

        public UserIdentity Login(string name, byte[] hash, string hostName)
        {
            SyncRoot.AcquireWriterLock(Timeout.Infinite);
            try
            {
                Debug.Assert(name != null);
                Debug.Assert(hash != null);
                Debug.Assert(hash.Length == 16);

                UserIdentity result;

                UserInfo user = _configuration.UserStorageAdapter.FindUserByName(name);
                if (user == null)
                {
                    return null;
                    //result = new UserIdentity(null, false, TimeSpan.MinValue, hostName);
                    ////LogonNotify(result);
                    //return result;
                }

                // IK: Это лишняя проверка, так как это делает XSD,
                // написал ее на всякий случай
                Debug.Assert(user.Hash != null);
                Debug.Assert(user.Hash.Length == 16);

                bool isEqual = true;
                for (int i = 0; isEqual && i < hash.Length; i++)
                    isEqual = hash[i] == user.Hash[i];

                if (isEqual ^ user.Enable)
                {
                    result = new UserIdentity(user, false, TimeSpan.MinValue, hostName);
                    //LogonNotify(result);
                    return result;
                }

                Debug.Assert(user.Enable);
                TimeSpan cookie = GenerateCookie();
                result = new UserIdentity(user, true, cookie, hostName);

                AddItem(result);
                return result;
            }
            finally
            {
                SyncRoot.ReleaseWriterLock();
            }
        }

        public void Logoff(UserIdentity value)
        {
            Remove(value);
        }

        public IList<UserIdentity> GetUserLoginCollection()
        {
            List<UserIdentity> list = new List<UserIdentity>(Dictionary);
            return list;
        }

        public UserIdentity FindSystemUserIdentity()
        {
            UserInfo user = _configuration.UserStorageAdapter.FindSystemUser();
            return new UserIdentity(user, true, TimeSpan.MinValue, "localhost");
        }
    }
}