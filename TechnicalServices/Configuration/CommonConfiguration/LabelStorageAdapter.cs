using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Configuration.Common
{
    

    public class LabelStorageAdapter : ILabelStorageAdapter
    {
        private LabelStorage _labelStorage = new LabelStorage();
        //private bool IsInit;
        private int _lastLabelId;
        private readonly List<Label> lockedLabels = new List<Label>();
        private string _filePath;
        private object _syncObject;
        private IEventLogging _logging;

        public  event EventHandler<LabelEventArg> OnDelete;
        public  event EventHandler<LabelEventArg> OnAdd;
        public  event EventHandler<LabelEventArg> OnUpdate;

        public LabelStorageAdapter(ModuleConfiguration config, IEventLogging logging)
        {
            //удаленная загрузка OnLine
            _logging = logging;
            if (config == null) return;
            _labelStorage.AddRange(config.LabelList);
            _lastLabelId = (_labelStorage.Count > 0) ? _labelStorage.Max(x => x.Id) : 0;
            _syncObject = ((ICollection)lockedLabels).SyncRoot;
            Check();
        }

        public LabelStorageAdapter(ModuleConfiguration config, string path, IEventLogging logging)
            : this(config, logging)
        {
            _filePath = path;
            LabelStorage customLabelStorage = LabelStorageExt.LoadStorage(path, config.LabelList, logging);
            _labelStorage.AddRange(customLabelStorage);
            _lastLabelId = (_labelStorage.Count > 0) ? _labelStorage.Max(x => x.Id) : 0;
            Check();
        }

        private void Check()
        {
            IEnumerable<IGrouping<string, Label>> list = _labelStorage.GroupBy(l => l.Name);
            foreach (IGrouping<string, Label> labels in list)
            {
                if (labels.Count() > 1)
                {
                    int count = 0;
                    int countSysLabel = 0;
                    foreach (Label label in labels)
                    {
                        if (label.IsSystem)
                        {
                            countSysLabel++;
                            continue;
                        }
                        count++;
                    }
                    if (countSysLabel > 1) //в принципе сюда мы не заюдем, т.к. система упадет раньше при считывании меток в момент сверки xml со схемой
                        throw new ApplicationException(String.Format("Нарушена уникальность меток. Обнаружены системные метки с именем {0}, количество меток = {1}", labels.Key, countSysLabel));
                    if (count > 1)
                        throw new ApplicationException(String.Format("Нарушена уникальность меток. Обнаружены пользовательские метки с именем {0}, количество пользовательских меток = {1}", labels.Key, count));
                }
            }
        }

        private void DeleteEvent(object sender, Label labelInfo)
        {
            if(OnDelete!= null)
            {
                OnDelete(sender, new LabelEventArg(labelInfo));
            }
        }

        private void AddEvent(object sender, Label label)
        {
            if (OnAdd != null)
            {
                OnAdd(sender, new LabelEventArg(label));
            }
        }

        private void UpdateEvent(object sender, Label label)
        {
            if (OnUpdate != null)
            {
                OnUpdate(sender, new LabelEventArg(label));
            }
        }

        public Label[] GetLabelStorage()
        {
            return _labelStorage.ToArray();
        }

        public LabelError AddLabel(Label labelInfo)
        {
            LabelError error;
            LabelError whatCheck = LabelError.LabelAlreadyExist |
                                   LabelError.NoName |
                                   LabelError.SystemLabel;

            error = CheckLabelInfo(labelInfo, whatCheck);
            if (error == 0)
            {
                labelInfo.Id = NextLabelId;
                _labelStorage.Add(labelInfo);
                _labelStorage.SaveStorage(_filePath);
                AddEvent(this, labelInfo);
                return LabelError.NoError;
            }
            return error;
        }

        public LabelError DeleteLabel(Label labelInfo)
        {
            LabelError error;

            LabelError whatCheck = LabelError.LockedAlready |
                                    LabelError.DeletedAlready |
                                    LabelError.SystemLabel;

            error = CheckLabelInfo(labelInfo, whatCheck);

            if (error == 0)
            {
                int index = _labelStorage.FindIndex(x => x.Name == labelInfo.Name);
                try
                {
                    _labelStorage.RemoveAt(index);
                }
                catch
                {
                    return LabelError.NoDeleted;
                }
                _labelStorage.SaveStorage(_filePath);
                DeleteEvent(this, labelInfo);
                return LabelError.NoError;
            }
            return error;
        
        }

        public LabelError UpdateLabel(Label labelInfo)
        {
            LabelError error;

            LabelError whatCheck = LabelError.NoName |
                                  LabelError.LabelAlreadyExist |
                                  LabelError.DeletedAlready |
                                  LabelError.SystemLabel|
                                  LabelError.UnlockedAlready;

            //TODO проверять если логин или FIO поменялись

            error = CheckLabelInfo(labelInfo, whatCheck);
            
            if (error == 0)
            {
                Label labelInfoPrev = _labelStorage.Find(x => x.Id == labelInfo.Id);
                labelInfoPrev.Name = labelInfo.Name;
                _labelStorage.SaveStorage(_filePath);
                UpdateEvent(this, labelInfo);
                return LabelError.NoError;
            }
            return error;
        }


       
        private int NextLabelId
        {
            [DebuggerStepThrough]
            get { return Interlocked.Increment(ref _lastLabelId); }
        }


        #region LOCK
        private void LockLabelListAdd(Label labelInfo)
        {
            lock (_syncObject)
            {
                lockedLabels.Add(labelInfo);
            }
        }

        private void LockLabelListDelete(Label userInfo)
        {
            lock (_syncObject)
            {
                int index = lockedLabels.FindIndex(x => x.Id == userInfo.Id);
                if (index > -1)
                    lockedLabels.RemoveAt(index);
            }
        }

        public LabelError LockLabel(Label labelInfo)
        {
            LabelError error;
            LabelError whatCheck = LabelError.LockedAlready |
                                    LabelError.DeletedAlready;
            error = CheckLabelInfo(labelInfo, whatCheck);
            if (error == 0)
            {
                LockLabelListAdd(labelInfo);
                return LabelError.NoError;
            }
            return error;
        }

        public LabelError UnlockLabel(Label userInfo)
        {
            LabelError error;
            LabelError whatCheck =  LabelError.DeletedAlready | 
                                    LabelError.UnlockedAlready;
            error = CheckLabelInfo(userInfo, whatCheck);
            
            if (error == 0)
            {
                LockLabelListDelete(userInfo);
                return LabelError.NoError;
            }
            return error;
        }
        #endregion

        #region CheckUserInfo
        public LabelError CheckLabelInfo(Label labelInfo, LabelError forCheck)
        {
            LabelError error = new LabelError();
            //Проверка на залнение логина
            if ((forCheck & LabelError.NoName) == LabelError.NoName)
            {
                if (labelInfo == null || labelInfo.Name.Trim().Length == 0)
                {
                    error |= LabelError.NoName;
                }
            }

            //Это системная  метка, с которой запрещены какие - либо операции
            if ((forCheck & LabelError.SystemLabel) == LabelError.SystemLabel)
            {
                if ((labelInfo != null) && (labelInfo.IsSystem))
                {
                    error |= LabelError.SystemLabel;
                }
            }

            //Проверка на то, что метка уже удалена
            if ((forCheck & LabelError.DeletedAlready) == LabelError.DeletedAlready)
            {
                int labelInfoIndex = -1;
                if (labelInfo != null)
                    labelInfoIndex = _labelStorage.FindIndex(x => x.Id == labelInfo.Id);

                if (labelInfoIndex == -1)
                {
                    error |= LabelError.DeletedAlready;
                }
            }

            //Проверка на, то что такое название метки уже есть в системе
            if ((forCheck & LabelError.LabelAlreadyExist) == LabelError.LabelAlreadyExist)
            {
                //_userStorage.Find(value => value.Name.Equals(labelInfo.Name, StringComparison.InvariantCultureIgnoreCase)&& value.Id!=labelInfo.Id);
                if (labelInfo != null && _labelStorage.Exists(x => x.Id != labelInfo.Id && x.Name != null && x.Name.ToLower().Trim() == labelInfo.Name.ToLower().Trim()))
                {
                    error |= LabelError.LabelAlreadyExist;
                }
            }

            //Проверка на, то что метка с таким Id заблокрована
            if ((forCheck & LabelError.LockedAlready) == LabelError.LockedAlready)
            {
                if (labelInfo != null && lockedLabels.Exists(x => x.Id == labelInfo.Id))
                {
                    error |= LabelError.LockedAlready;
                }
            }

            //Проверка на, то что метка с таким Id уже не заблоктрована
            if ((forCheck & LabelError.UnlockedAlready) == LabelError.UnlockedAlready)
            {
                if (labelInfo != null && !lockedLabels.Exists(x => x.Id == labelInfo.Id))
                {
                    error |= LabelError.UnlockedAlready;
                }
            }

            /*if ((forCheck & LabelError.UsedInPresenatation) == LabelError.UsedInPresenatation)
            {
                //IPresentationDAL pres= ;
                
                pres.GetPresentationWhichContainsLabel(labelInfo.Id);
                if (labelInfo != null && !lockedLabels.Exists(x => x.Id == labelInfo.Id))
                {
                    error |= LabelError.UsedInPresenatation;
                }
            }*/


            return error;
        }

        #endregion

        

    }
}

