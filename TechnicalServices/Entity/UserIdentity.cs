using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;

namespace TechnicalServices.Entity
{
    [Serializable]
    public class UserIdentity : IIdentity, IPrincipal, IEquatable<UserIdentity>
    {
        //public const string AdministratorRoleName = "Администратор";
        //public const string OperatorRoleName = "Оператор";

        private readonly TimeSpan _cookie;
        private readonly string _hostName;
        private readonly bool _isAuthenticated;
        private readonly UserInfo _user;

        public UserIdentity(UserInfo user, bool isAuthenticated, TimeSpan cookie, string hostName)
        {
            Debug.Assert(user != null, "user не может быть пустым");
            Debug.Assert(!String.IsNullOrEmpty(hostName), "hostName не может быть пустой");

            _user = user;
            _cookie = cookie;
            _isAuthenticated = isAuthenticated;
            _hostName = hostName;
        }

        public UserInfo User
        {
            [DebuggerStepThrough]
            get { return _user; }
        }

        public TimeSpan Cookie
        {
            [DebuggerStepThrough]
            get { return _cookie; }
        }

        public string HostName
        {
            [DebuggerStepThrough]
            get { return _hostName; }
        }

        #region IIdentity Members

        public string AuthenticationType
        {
            [DebuggerStepThrough]
            get { return String.Empty; }
        }

        public bool IsAuthenticated
        {
            [DebuggerStepThrough]
            get { return _isAuthenticated; }
        }

        public string Name
        {
            [DebuggerStepThrough]
            get { return User.Name; }
        }

        #endregion

        #region IPrincipal Members

        public IIdentity Identity
        {
            [DebuggerStepThrough]
            get { return this; }
        }

        public bool IsInRole(string role)
        {
            Debug.Assert(!String.IsNullOrEmpty(role), "Роль пользователя не может быть пустой");
            switch (role)
            {
                case UserRole.Operator:
                    return _user.IsOperator;
                case UserRole.Administrator:
                    return _user.IsAdmin;
                default:
                    return false;
            }
        }

        #endregion

        #region override

        public bool Equals(UserIdentity other)
        {
            return ToString().Equals(other.ToString());
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2}-{3}", 
                _user.Name, _cookie.Ticks, _isAuthenticated, _hostName);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is UserIdentity)
            {
                return ToString().Equals(obj.ToString());
            }
            return base.Equals(obj);
        }

        #endregion
    }
}