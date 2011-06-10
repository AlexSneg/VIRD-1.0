using System;
using System.Linq;

using TechnicalServices.Entity;
using System.Collections.Generic;

namespace Domain.PresentationDesign.Client
{
    public class LoginServiceCallback : ILoginServiceCallback
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
}