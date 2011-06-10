using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;

namespace TechnicalServices.Configuration.Server
{
    public class UserStorageAdapter 
    {
        private UserStorage _userStorage; //= new UserStorage();
        private static UserStorageAdapter _instance  = new UserStorageAdapter();

        private int _lastUserInfoId;
        private readonly List<UserInfo> lockedUsers = new List<UserInfo>();
        private object _syncObject;

        private string _filePath;

        private UserStorageAdapter()
        {
            _syncObject = ((ICollection) lockedUsers).SyncRoot;
        }

        public static UserStorageAdapter Instance
        {
            get { return _instance; }
        }

        private bool IsInit = false;
        public void Init(string path)
        {
            if(IsInit) return;
            IsInit = true;
            _filePath = path;
            _userStorage = UserStorageExt.LoadStorage(path);
            _lastUserInfoId = _userStorage.Max(x=>x.Id);
        }

        private int NextUserInfoId
        {
            [DebuggerStepThrough]
            get { return Interlocked.Increment(ref _lastUserInfoId); }
        }


        public UserInfo FindUserByName(string name)
        {
            return _userStorage.Find(value => value.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
        public UserInfo FindSystemUser()
        {
            return _userStorage.OrderByDescending(value => value.Priority).First();
        }
        public UserInfo[] GetUserStorage()
        {
            return _userStorage.ToArray();
        }

        public UserError AddUser(UserInfo userInfo)
        {
            UserError error = new UserError();
            UserError whatCheck = UserError.NoLogin | 
                                  UserError.NoPassword |
                                  UserError.NoFIO | 
                                  UserError.LoginAlreadyExist|
                                  UserError.FIOAlreadyExist |
                                  UserError.NoRole;
            
            error = CheckUserInfo(userInfo, whatCheck);
            if (error== 0)
            {
                userInfo.Id = NextUserInfoId;
                _userStorage.Add(userInfo);
                _userStorage.SaveStorage(_filePath);
                return UserError.NoError;
            }
            return error;
        }

        public UserError DeleteUser(UserInfo userInfo)
        {
            UserError error = new UserError();

            UserError whatCheck = UserError.LockedAlready |
                                    UserError.DeletedAlready |
                                    UserError.NoMoreAdminAfterDelete;

            error = CheckUserInfo(userInfo, whatCheck);

            if (error == 0)
            {
                int index = _userStorage.FindIndex(x => x.Name == userInfo.Name);
                //bool removeResult = 
                try
                {
                    _userStorage.RemoveAt(index);
                }
                catch
                {
                    return UserError.NoDeleted;
                }
               _userStorage.SaveStorage(_filePath);
               return UserError.NoError;
            }
            return error;
        }

        public UserError UpdateUser(UserInfo userInfo)
        {
            UserError error = new UserError();

            UserError whatCheck = UserError.NoLogin |
                                  UserError.NoPassword |
                                  UserError.NoFIO |
                                  UserError.LastUserWithAdminRole|
                                  UserError.LoginAlreadyExist |
                                  UserError.FIOAlreadyExist |
                                  UserError.DeletedAlready |
                                  UserError.NoRole |
                                  UserError.UnlockedAlready;
            //TODO проверять если логин или FIO поменялись

            error = CheckUserInfo(userInfo, whatCheck);

            if (error == 0)
            {
                UserInfo userInfoPrev = _userStorage.Find(x => x.Id == userInfo.Id);
                userInfoPrev.Name = userInfo.Name;
                userInfoPrev.Hash = userInfo.Hash;
                userInfoPrev.IsAdmin = userInfo.IsAdmin;
                userInfoPrev.IsOperator = userInfo.IsOperator;
                userInfoPrev.Comment = userInfo.Comment;
                userInfoPrev.FullName = userInfo.FullName;

                _userStorage.SaveStorage(_filePath);
                return UserError.NoError;
            }
            return error;
        }

        private void LockUserListAdd(UserInfo userInfo)
        {
            lock (_syncObject)
            {
                lockedUsers.Add(userInfo);
            }
        }

        private void LockUserListDelete(UserInfo userInfo)
        {
            lock (_syncObject)
            {
                int index = lockedUsers.FindIndex(x => x.Id == userInfo.Id);
                if (index>-1)
                    lockedUsers.RemoveAt(index);
            }
        }

        public UserError LockUser(UserInfo userInfo)
        {
            UserError error = new UserError();

            UserError whatCheck =   UserError.LockedAlready | 
                                    UserError.DeletedAlready;

            error = CheckUserInfo(userInfo, whatCheck);

            if (error == 0)
            {
                LockUserListAdd(userInfo);
                return UserError.NoError;
            
            }
            return error;
        }

        public UserError UnlockUser(UserInfo userInfo)
        {
            UserError error = new UserError();

            UserError whatCheck =   UserError.DeletedAlready | 
                                    UserError.UnlockedAlready;

            error = CheckUserInfo(userInfo, whatCheck);

            if (error == 0)
            {
                
                 LockUserListDelete(userInfo);
                return UserError.NoError;
            }
            return error;
        }

        #region CheckUserInfo
        public UserError CheckUserInfo(UserInfo userInfo,UserError forCheck)
        {
            UserError error = new UserError();


            
            //Проверка на залнение логина
            if((forCheck&UserError.NoLogin)==UserError.NoLogin)
            {
                if (userInfo== null || userInfo.Name.Trim().Length == 0)
                {
                    error |= UserError.NoLogin;
                }
            }

            //В системе останется после удаления хотя бы одни пользователь с ролью Администратор  
            if ((forCheck & UserError.NoMoreAdminAfterDelete) == UserError.NoMoreAdminAfterDelete)
            {
                UserInfo systemUserInfo = FindSystemUser();
                if (userInfo != null) 
                {
                    if (!_userStorage.Exists(x => x.Id != userInfo.Id && x.IsAdmin && x.Id!= systemUserInfo.Id))
                    {
                        error |= UserError.NoMoreAdminAfterDelete;
                    }
                }
            }

            if ((forCheck & UserError.LastUserWithAdminRole) == UserError.LastUserWithAdminRole)
            {
                UserInfo systemUserInfo = FindSystemUser();
                if (userInfo != null)
                {
                    if (!_userStorage.Exists(x => x.Id != userInfo.Id && x.IsAdmin && x.Id != systemUserInfo.Id) && !userInfo.IsAdmin)
                    {
                        error |= UserError.LastUserWithAdminRole;
                    }
                }
            }


            //Проверка на залнение FIO
            if ((forCheck & UserError.NoFIO) == UserError.NoFIO)
            {
                if (userInfo == null || userInfo.FullName.Trim().Length == 0)
                {
                    error |= UserError.NoFIO;
                }
            }

            //Проверка на заполнение пароля
            if ((forCheck & UserError.NoPassword) == UserError.NoPassword)
            {
                if (userInfo == null || userInfo.Hash.Length == 0)
                {
                    error |= UserError.NoPassword;
                }
            }

            //Проверка на залнение хотя бы одной роли
            if ((forCheck & UserError.NoRole) == UserError.NoRole)
            {
                if ((userInfo == null) ||(!userInfo.IsAdmin && !userInfo.IsOperator))
                {
                    error |= UserError.NoRole;
                }
            }
            
            //Проверка на то, что пользователь уже удален
            if ((forCheck & UserError.DeletedAlready) == UserError.DeletedAlready)
            {
                int userInfoIndex = -1;
                if(userInfo!= null)
                    userInfoIndex = _userStorage.FindIndex(x => x.Id == userInfo.Id);

                if (userInfoIndex == -1)
                {
                    error |= UserError.DeletedAlready;
                }
            }

            //Проверка на, то что такой Login уже есть в системе
            if ((forCheck & UserError.LoginAlreadyExist) == UserError.LoginAlreadyExist)
            {
                //_userStorage.Find(value => value.Name.Equals(userInfo.Name, StringComparison.InvariantCultureIgnoreCase)&& value.Id!=userInfo.Id);
                if (userInfo != null && _userStorage.Exists(x => x.Id != userInfo.Id && x.Name != null && x.Name.ToLower().Trim() == userInfo.Name.ToLower().Trim() ))
                {
                    error |= UserError.LoginAlreadyExist;
                }
            }

            //Проверка на, то что такое FIO уже есть в системе
            if ((forCheck & UserError.FIOAlreadyExist) == UserError.FIOAlreadyExist)
            {
                if (userInfo != null && _userStorage.Exists(x => x.FullName!=null && x.FullName.ToLower().Trim() == userInfo.FullName.ToLower().Trim() && x.Id != userInfo.Id))
                {
                    error |= UserError.FIOAlreadyExist;
                }
            }

            //Проверка на, то что пользователь с таким Id заблокрован 
            if ((forCheck & UserError.LockedAlready) == UserError.LockedAlready)
            {
                
                if (userInfo != null && lockedUsers.Exists(x => x.Id == userInfo.Id))
                {
                    error |= UserError.LockedAlready;
                }
            }

            //Проверка на, то что пользователь с таким Id уже не заблоктрована
            if ((forCheck & UserError.UnlockedAlready) == UserError.UnlockedAlready)
            {
                if (userInfo != null && !lockedUsers.Exists(x => x.Id == userInfo.Id))
                {
                    error |= UserError.UnlockedAlready;
                }
            }

            return error;
        }

#endregion

        

    }
}
