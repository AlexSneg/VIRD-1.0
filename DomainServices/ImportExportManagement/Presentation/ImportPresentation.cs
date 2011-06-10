using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DomainServices.ImportExportCommon;
using TechnicalServices.Configuration.Client;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util.FileTransfer;

namespace DomainServices.ImportExportClientManagement.Presentation
{
    public class ImportPresentation
    {

        private IClientConfiguration _config;
        private readonly IPresentationClient _remotePresentationWorker;
        private readonly IPresentationClient _standalonePresentationWorker;
        private readonly IClientResourceCRUD<ResourceDescriptor> _clientResourceCRUD;
        //private readonly IClientResourceCRUD<DeviceResourceDescriptor> _clientDeviceResourceCRUD;

        //private readonly Func<bool, IContinue> _userInteractiveDelegate;
        private IContinue _continue;
        //private readonly Action<string> _errorMessageDelegate;
        //private readonly Action<string> _successMessageDelegate;
        //private readonly Func<string, HowImport> _replaceOrNewPresentationDelegate;
        private readonly UserIdentity _userIdentity;
        //private readonly IClientResourceCRUD<ResourceDescriptor> _clientResourceCRUD;

        private readonly IImportPresentationController _importPresentationController;


        public ImportPresentation(IClientConfiguration config,
            IPresentationClient remotePresentationWorker,
            IPresentationClient standalonePresentationWorker,
            //Func<bool, IContinue> userInteractiveDelegate,
            //Action<string> errorMessageDelegate,
            //Action<string> successMessageDelegate,
            //Func<string, HowImport> replaceOrNewPresentationDelegate
            IImportPresentationController importPresentationController)
        {
            _config = config;
            _remotePresentationWorker = remotePresentationWorker;
            _standalonePresentationWorker = standalonePresentationWorker;
            _clientResourceCRUD = _remotePresentationWorker.GetResourceCrud();
            //_clientDeviceResourceCRUD = remotePresentationWorker.GetDeviceResourceCrud();
            //_userInteractiveDelegate = userInteractiveDelegate;
            //_errorMessageDelegate = errorMessageDelegate;
            //_successMessageDelegate = successMessageDelegate;
            //_replaceOrNewPresentationDelegate = replaceOrNewPresentationDelegate;
            _importPresentationController = importPresentationController;
            //_clientResourceCRUD = ClientSourceTransferFactory.CreateClientFileTransfer(
            //    false, _remotePresentationWorker, _standalonePresentationWorker.SourceDAL);
            _userIdentity = Thread.CurrentPrincipal as UserIdentity;
        }

        #region public
        public void Import(string[] presentationFiles)
        {
            try
            {
                _continue = _importPresentationController.GetUserInterActive(presentationFiles.Length == 1);    // _userInteractiveDelegate.Invoke(presentationFiles.Length == 1);
                foreach (string presentationFile in presentationFiles)
                {
                    ImportOnePresentation(presentationFile);
                }
                //_importPresentationController.SuccessMessage("Импорт сценариев завершен");
                //_successMessageDelegate.Invoke("Импорт сценариев завершен");
            }
            catch (InterruptOperationException)
            {
                if (presentationFiles.Length > 1)
                    _importPresentationController.ErrorMessage(string.Format("Импорт сценариев был прерван пользователем"));
                //_errorMessageDelegate.Invoke(string.Format("Импорт сценариев был прерван пользователем"));
            }
            catch (Exception ex)
            {
                _importPresentationController.ErrorMessage(string.Format("При импорте сценариев произошла неизвестная ошибка: {0}", ex));
                //_errorMessageDelegate.Invoke(string.Format("При импорте сценариев произошла неизвестная ошибка: {0}", ex));
            }
        }

        public event Action<PresentationInfo> OnPresentationAdded;
        public event Action<PresentationInfo> OnPresentationDeleted;

        #endregion

        #region private
        private void PresentationAdded(PresentationInfo info)
        {
            if (OnPresentationAdded != null)
            {
                OnPresentationAdded.Invoke(info);
            }
        }

        private void PresentationDelete(PresentationInfo info)
        {
            if (OnPresentationDeleted != null)
            {
                OnPresentationDeleted.Invoke(info);
            }
        }


        private void ImportOnePresentation(string presentationFile)
        {
            string[] deletedEquipment;
            TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation
                = _standalonePresentationWorker.LoadPresentation(presentationFile, out deletedEquipment);
            if (presentation == null)
            {
                if (!_continue.Continue(string.Format("Содержание файла {0} некорректно. Импорт сценария невозможен", presentationFile)))
                    throw new InterruptOperationException(presentationFile);
                else
                    return;
            }
            if (deletedEquipment != null && deletedEquipment.Length > 0)
            {
                _importPresentationController.SuccessMessage(string.Format("Конфигурация сценария {0} отличается от текущей конфигурации системы. Отсутствующие в конфигурации системы объекты будут удалены из сценария", presentation.Name));
            }
            try
            {
                UploadPresentationAndSources(presentation);
            }
            catch (Exception ex)
            {
                if (ex is InterruptOperationException) throw;
                if (!_continue.Continue(ex.Message))
                    throw new InterruptOperationException(presentationFile);
            }
        }

        private void UploadPresentationAndSources(
            TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation)
        {
            HowImport howImport = HowImport.Cancel;
            try
            {
                string uniquePresentationName = presentation.UniqueName;
                PresentationInfo storedPresentationInfo =
                    _remotePresentationWorker.GetPresentationInfo(presentation.UniqueName);
                //if (storedPresentationInfo == null) return HowImport.New;
                // такая презентация есть
                howImport = CreateNewPresentationIfNeeded(presentation, ref storedPresentationInfo);
                SaveNewResources(presentation, uniquePresentationName);
                TechnicalServices.Persistence.SystemPersistence.Presentation.Slide[] newSlideArr = SavePresentationChanges(presentation, storedPresentationInfo);
                SaveSlideChanges(presentation, newSlideArr);
                if (howImport == HowImport.New)
                    PresentationAdded(new PresentationInfo(presentation));
            }
            catch (Exception)
            {
                if (howImport == HowImport.New)
                {
                    _remotePresentationWorker.DeletePresentation(presentation.UniqueName);
                }
                throw;
            }
        }

        private void SaveSlideChanges(TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation,
            IEnumerable<TechnicalServices.Persistence.SystemPersistence.Presentation.Slide> newSlideArr)
        {
            bool isSuccess;
            // сохраняем слайды
            TechnicalServices.Persistence.SystemPersistence.Presentation.Slide[] changedSlides = presentation.SlideList.Where(sl => !newSlideArr.Contains(sl)).ToArray();
            try
            {
                ResourceDescriptor[] notExistedResources;
                DeviceResourceDescriptor[] notExistedDeviceResources;
                foreach (TechnicalServices.Persistence.SystemPersistence.Presentation.Slide slide in changedSlides)
                {
                    isSuccess = _remotePresentationWorker.AcquireLockForSlide(presentation.UniqueName, slide.Id,
                                                                              RequireLock.ForEdit);
                    if (!isSuccess)
                        throw new ApplicationException(string.Format("Сцену {0} сценария {1} не удалось залочить",
                                                                     slide.Name, presentation.Name));
                }
                int[] notLockedSlide;
                int[] labelNotexists;
                isSuccess = _remotePresentationWorker.SaveSlideChanges(presentation.UniqueName,
                                                                       changedSlides, out notLockedSlide,
                                                                       out notExistedResources, out notExistedDeviceResources,
                                                                       out labelNotexists);
                if (!isSuccess)
                {
                    string notLockedSl = string.Empty;
                    if (notLockedSlide != null && notLockedSlide.Length != 0)
                    {
                        notLockedSl =
                            changedSlides.Where(sl => notLockedSlide.Contains(sl.Id)).
                                Select(sl => sl.Name).
                                Aggregate((prev, next) => prev + " ," + next);
                    }
                    string notExistedRes = string.Empty;
                    if (notExistedResources != null && notExistedResources.Length != 0)
                    {
                        notExistedRes =
                            notExistedResources.Select(rd => rd.ResourceInfo.Name).
                                Aggregate((prev, next) => prev + " ," + next);

                    }
                    string notExistedDeviceRes = string.Empty;
                    if (notExistedDeviceResources != null && notExistedDeviceResources.Length != 0)
                    {
                        notExistedDeviceRes =
                            notExistedDeviceResources.Select(rd => rd.ResourceInfo.Name).
                                Aggregate((prev, next) => prev + " ," + next);
                    }
                    throw new ApplicationException(string.Format("ошибка при сохранении сценария {0}. Не залочены сцены \n{1}\nНе существуют ресурсы \n{2}\nНе существуют ресурсы девайсов\n{3}",
                                                                 presentation.Name, notLockedSl, notExistedRes, notExistedDeviceRes));
                }
            }
            finally
            {
                foreach (TechnicalServices.Persistence.SystemPersistence.Presentation.Slide slide in changedSlides)
                {

                    _remotePresentationWorker.ReleaseLockForSlide(presentation.UniqueName, slide.Id);
                }
            }
        }

        private TechnicalServices.Persistence.SystemPersistence.Presentation.Slide[] SavePresentationChanges(
            TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation,
            PresentationInfo storedPresentationInfo)
        {
            // теперь только сохраняем изменеия в презентации
            TechnicalServices.Persistence.SystemPersistence.Presentation.Slide[] newSlideArr = storedPresentationInfo == null
                                      ? presentation.SlideList.ToArray()
                                      :
                                          presentation.SlideList.Where(
                                              sl =>
                                              !storedPresentationInfo.SlideInfoList.Exists(
                                                   si => si.Id == sl.Id)).ToArray();
            bool isSuccess = _remotePresentationWorker.AcquireLockForPresentation(presentation.UniqueName,
                                                                                  RequireLock.ForEdit);
            if (!isSuccess)
            {
                throw new ApplicationException(string.Format("Сценарий {0} не удалось залочить", presentation.Name));

            }
            try
            {
                ResourceDescriptor[] notExistedResources;
                DeviceResourceDescriptor[] notExistedDeviceResources;
                int[] labelNotExists;
                UserIdentity[] whoLock;
                int[] slidesAlreadyExistsId;
                SavePresentationResult result = _remotePresentationWorker.SavePresentationChanges(new PresentationInfo(presentation),
                                                                              newSlideArr,
                                                                              out notExistedResources,
                                                                              out notExistedDeviceResources,
                                                                              out labelNotExists,
                                                                              out whoLock,
                                                                              out slidesAlreadyExistsId);
                if (result != SavePresentationResult.Ok)
                {
                    StringBuilder builder = new StringBuilder();
                    if (result == SavePresentationResult.ResourceNotExists)
                    {
                        string notExistedRes = string.Empty;
                        if (notExistedResources != null && notExistedResources.Length != 0)
                        {
                            notExistedRes =
                                notExistedResources.Select(rd => rd.ResourceInfo.Name).Aggregate(
                                    (prev, next) => prev + " ," + next);
                            builder.AppendFormat("Не найдены ресурсы {0}\n", notExistedRes);
                        }
                        string notExistedDeviceRes = string.Empty;
                        if (notExistedDeviceResources != null && notExistedDeviceResources.Length != 0)
                        {
                            notExistedDeviceRes =
                                notExistedDeviceResources.Select(rd => rd.ResourceInfo.Name).
                                    Aggregate((prev, next) => prev + " ," + next);
                            builder.AppendFormat("Не найдены ресурсы для устройств {0}\n", notExistedDeviceRes);
                        }
                    }
                    if (result == SavePresentationResult.LabelNotExists)
                    {
                        builder.AppendFormat("Не найдены метки {0}\n",
                            labelNotExists.Select(lb => lb.ToString()).Aggregate((prev, next) => prev + " ," + next));
                    }
                    if (result == SavePresentationResult.PresentationNotExists)
                    {
                        builder.AppendFormat("Сценарий {0} уже удален\n", presentation.Name);
                    }
                    if (result == SavePresentationResult.PresentationNotLocked)
                    {
                        builder.AppendFormat("Сценарий {0} не заблокирован\n", presentation.Name);
                    }
                    if (result == SavePresentationResult.SlideAlreadyExists)
                    {
                        builder.AppendFormat("Некоторые сцены сценария {0} уже есть в системе и таким образом не могут быть добавлены\n", presentation.Name);
                    }
                    if (result == SavePresentationResult.SlideLocked)
                    {
                        builder.AppendFormat("Некоторые сцены сценария {0} заблокированы другими пользователями\n", presentation.Name);
                    }

                    if (result == SavePresentationResult.Unknown)
                    {
                        builder.AppendFormat("Неизвестная ошибка");
                    }

                    throw new ApplicationException(string.Format("ошибка при сохранении сценария {0}.\n {1} ",
                        presentation.Name, builder));
                }
            }
            finally
            {
                _remotePresentationWorker.ReleaseLockForPresentation(presentation.UniqueName);
            }
            return newSlideArr;
        }

        private void SaveNewResources(
            TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation,
            string uniquePresentationName)
        {
            // загружаем новые источники
            List<ResourceDescriptor> newResourcesList = new List<ResourceDescriptor>(100);
            AddNewSources(_standalonePresentationWorker.GetGlobalSources(),
                          _remotePresentationWorker.GetGlobalSources(),
                          newResourcesList);
            AddNewSources(_standalonePresentationWorker.GetLocalSources(uniquePresentationName),
                          _remotePresentationWorker.GetLocalSources(presentation.UniqueName),
                          newResourcesList);
            bool isResourceDeleted = CheckAndRemoveResourceDescriptorIfNeeded(presentation, newResourcesList);

            // источники девайсов
            List<DeviceResourceDescriptor> newDeviceResourcesList = new List<DeviceResourceDescriptor>(100);
            AddNewSources(_standalonePresentationWorker.GetGlobalDeviceSources(),
                          _remotePresentationWorker.GetGlobalDeviceSources(),
                          newDeviceResourcesList);

            isResourceDeleted |= CheckAndRemoveDeviceResourceDescriptorIfNeeded(presentation,
                newDeviceResourcesList);

            if (isResourceDeleted)
            {
                _importPresentationController.SuccessMessage(string.Format(
                    "В сценарии {0} есть ссылки на программные источники, удаленные из Системы. Ссылки на данные источники бдут удалены из сценария",
                    presentation.Name));
            }
            // сохраняем
            foreach (ResourceDescriptor rd in newResourcesList)
            {
                // если есть файловый ресурс, описание которого есть в локальной системе, но самого ресурса нет удаленно, то такие ресурсы надо кирдыкать из презентации
                // чтобы не трогать существующие сделаем полный клон
                ResourceDescriptor resourceDescriptor = _standalonePresentationWorker.SourceDAL.MakeFullClone(rd);
                string otherResourceId = null;
                ResourceFileInfo resourceFileInfo = resourceDescriptor.ResourceInfo as ResourceFileInfo;
                if (resourceFileInfo != null)
                {
                    foreach (ResourceFileProperty property in resourceFileInfo.ResourceFileList)
                    {
                        property.Newly = true;
                    }
                }
                FileSaveStatus saveStatus = _clientResourceCRUD.CreateSource(resourceDescriptor, out otherResourceId);
                do
                {
                    if (saveStatus == FileSaveStatus.Exists || saveStatus == FileSaveStatus.ExistsWithSameName)
                    {
                        SameSourceAction sameSourceAction;
                        if (!_importPresentationController.ExistsSameSource(
                            string.Format("Источник с названием {0} уже есть в системе", resourceDescriptor.ResourceInfo.Name),
                            out sameSourceAction))
                            throw new InterruptOperationException(resourceDescriptor.ResourceInfo.Name);
                        switch (sameSourceAction)
                        {
                            case SameSourceAction.Copy:
                                resourceDescriptor.ResourceInfo.Name = "Копия " + resourceDescriptor.ResourceInfo.Name;
                                saveStatus = _clientResourceCRUD.CreateSource(resourceDescriptor, out otherResourceId);
                                break;
                            case SameSourceAction.Replace:
                                resourceDescriptor.ResourceInfo.Id = otherResourceId;
                                //string newResourceId;
                                saveStatus = _clientResourceCRUD.SaveSource(resourceDescriptor, out otherResourceId);
                                //if (saveStatus == FileSaveStatus.Ok)
                                break;
                        }
                    }
                    else if (saveStatus != FileSaveStatus.Ok)
                        //if (saveStatus != FileSaveStatus.Ok)
                        throw new ApplicationException(string.Format("Источник {0} не создался на сервере",
                                                                     resourceDescriptor.ResourceInfo.Name));

                } while (saveStatus != FileSaveStatus.Ok);
                ReplaceResourceDescriptor(presentation, rd, resourceDescriptor);
            }

            // DeviceResourceDescriptor не могут быть созданы на клиенте!!!
            //foreach (DeviceResourceDescriptor descriptor in newDeviceResourcesList)
            //{
            //    FileSaveStatus saveStatus = _clientDeviceResourceCRUD.CreateSource(descriptor);
            //    if (saveStatus != FileSaveStatus.Ok)
            //        throw new ApplicationException(string.Format("Источник {0} не создался на сервере",
            //                                                     descriptor.ResourceInfo.Name));

            //}
        }

        private bool CheckAndRemoveDeviceResourceDescriptorIfNeeded(
            TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation,
            List<DeviceResourceDescriptor> newDeviceResourcesList)
        {
            bool isDeleted = false;
            // отберем только те ресурсы, которые являются файловыми
            IEnumerable<DeviceResourceDescriptor> checkedDescriptor =
                newDeviceResourcesList.Where(rd => (rd.ResourceInfo is ResourceFileInfo)
                && !_standalonePresentationWorker.DeviceSourceDAL.IsResourceAvailable(rd)
                    && !_remotePresentationWorker.IsResourceAvailable(rd));
            foreach (DeviceResourceDescriptor descriptor in checkedDescriptor)
            {
                if (presentation.Remove(descriptor))
                    isDeleted = true;
            }
            newDeviceResourcesList.RemoveAll(rd => checkedDescriptor.Any(crd => crd.Equals(rd)));
            return isDeleted;
        }

        private bool CheckAndRemoveResourceDescriptorIfNeeded(
            TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation,
            List<ResourceDescriptor> newResourceDescriptorList)
        {
            bool isDeleted = false;
            // отберем только те ресурсы, которые являются файловыми
            IEnumerable<ResourceDescriptor> checkedDescriptor =
                newResourceDescriptorList.Where(rd => (rd.ResourceInfo is ResourceFileInfo)
                && !_standalonePresentationWorker.SourceDAL.IsResourceAvailable(rd)
                    && !_remotePresentationWorker.IsResourceAvailable(rd));
            foreach (ResourceDescriptor descriptor in checkedDescriptor)
            {
                if (presentation.Remove(descriptor))
                    isDeleted = true;
            }
            newResourceDescriptorList.RemoveAll(rd => checkedDescriptor.Any(crd => crd.Equals(rd)));
            return isDeleted;
        }

        private void ReplaceResourceDescriptor(
            TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation,
            ResourceDescriptor oldResourceDescriptor, ResourceDescriptor newDescriptor)
        {
            //if (string.IsNullOrEmpty(newResourceId) || oldResourceDescriptor.ResourceInfo.Id.Equals(newResourceId)) return;
            //oldResourceDescriptor.ResourceInfo.Id = newResourceId;
            foreach (TechnicalServices.Persistence.SystemPersistence.Presentation.Slide slide in presentation.SlideList)
            {
                foreach (Source source in slide.SourceList)
                {
                    ResourceDescriptor descriptor = source.ResourceDescriptor;
                    if (descriptor != null && descriptor.ResourceInfo != null
                        && descriptor.Equals(oldResourceDescriptor))
                    {
                        source.ResourceDescriptor = newDescriptor;
                    }
                }
            }
        }

        private HowImport CreateNewPresentationIfNeeded(
            TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation,
            ref PresentationInfo storedPresentationInfo)
        {
            // по желанию заказчика мы не отслеживаем идентификатор сценария, если вдруг это требование вернется то 
            // надо будет вносить изменения
            HowImport howImport = HowImport.New;
            if (storedPresentationInfo != null)
            {
                //howImport = AskUserReplaceOrNew(presentation);
                if (howImport == HowImport.Cancel)
                    throw new InterruptOperationException(presentation.Name);
                if (howImport == HowImport.New)
                {
                    presentation.UniqueName =
                        TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation.GenerateNewUniqueName();
                    _remotePresentationWorker.CopySourceFromLocalToLocal(storedPresentationInfo.UniqueName,
                                                                         presentation.UniqueName);
                }
            }
            //HowImport howImport = ResolveConflictWithExistedPresentation(presentation);
            if (howImport == HowImport.New)
            {
                CreatePresentationResult createPresentationResult;
                do
                {
                    int[] labelNotExists;
                    storedPresentationInfo = new PresentationInfo(presentation);
                    createPresentationResult =
                        _remotePresentationWorker.CreatePresentation(storedPresentationInfo, out labelNotExists);
                    if (createPresentationResult != CreatePresentationResult.Ok)
                    {
                        switch (createPresentationResult)
                        {
                            case CreatePresentationResult.SameUniqueNameExists:
                                throw new ApplicationException(string.Format("Какая то фигня, такого не может быть при создании сценария {0} вернулась ошибка, что такой сценарий уже существует - бред",
                                                                             presentation.Name));
                            case CreatePresentationResult.SameNameExists:
                                ResolveSameNameConflict(presentation);
                                //presentation.Name = "Копия_" + presentation.Name;
                                break;
                            case CreatePresentationResult.LabelNotExists:
                                throw new ApplicationException(
                                    string.Format("В сценарии {0} существуют метки, которых нет в системе",
                                             presentation.Name));
                            default:
                                throw new ApplicationException(string.Format("неизвестный статус при создании новой презентации {0}", createPresentationResult));
                        }
                    }
                } while (createPresentationResult != CreatePresentationResult.Ok);
            }
            return howImport;
        }

        // попадаем сюда, если уже есть в системе презентация с тем же именем
        private void ResolveSameNameConflict(TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation)
        {
            HowImport howToImport = AskUserReplaceOrNew(presentation);
            if (HowImport.Cancel == howToImport)
                throw new InterruptOperationException(presentation.Name);
            if (HowImport.New == howToImport)
            {
                presentation.Name = "Копия_" + presentation.Name;
                return;
            }
            if (HowImport.Replace == howToImport)
            {
                PresentationInfoExt stored = _remotePresentationWorker.GetPresentationInfoByName(presentation.Name);
                if (stored == null)
                    return;
                if (!_remotePresentationWorker.DeletePresentation(stored.UniqueName))
                {
                    // если не смогли удалить значит чето залочено
                    if (stored.LockingInfo != null && stored.LockingInfo.ObjectKey.GetObjectType() == ObjectType.Presentation)
                    {
                        string user = string.IsNullOrEmpty(stored.LockingInfo.UserIdentity.User.FullName)
                                          ? stored.LockingInfo.UserIdentity.User.Name
                                          : stored.LockingInfo.UserIdentity.User.FullName;
                        throw new ApplicationException(
                            string.Format(
                                "Сценарий {0} заблокирован пользователем {1}. Замена сценария невозможна",
                                stored.Name, user));
                    }
                    else if (stored.SlideInfoList.Any(si=>si.LockingInfo != null))
                    {
                        throw new ApplicationException(
                            string.Format(
                            "Сцены сценария {0} заблокированы. Замена сценария невозможна",
                            stored.Name));
                    }
                    throw new ApplicationException(
                        string.Format(
                            "неизвестная ошибка при замене сценария {0}",
                            stored.Name));
                }
                else
                {
                    PresentationDelete(stored);
                }
                return;
            }
        }

        private static void AddNewSources<TResource>(Dictionary<string, IList<TResource>> localSources,
            IDictionary<string, IList<TResource>> remoteSources,
            List<TResource> newResourcesList)
            where TResource : ResourceDescriptorAbstract
        {
            foreach (KeyValuePair<string, IList<TResource>> pair in localSources)
            {
                IList<TResource> remoteList;
                if (remoteSources.TryGetValue(pair.Key, out remoteList))
                {
                    foreach (TResource resourceDescriptor in pair.Value)
                    {
                        if (!remoteList.Any(rd => rd.Equals(resourceDescriptor)))
                        {
                            newResourcesList.Add(resourceDescriptor);
                        }
                    }
                    //newResourcesList.AddRange(pair.Value.Except(remoteList));
                }
                else
                {
                    newResourcesList.AddRange(pair.Value);
                }
            }
        }

        //private HowImport ResolveConflictWithExistedPresentation(TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation)
        //{
        //    PresentationInfo storedPresentationInfo = _remotePresentationWorker.GetPresentationInfo(presentation.UniqueName);
        //    if (storedPresentationInfo == null) return HowImport.New;
        //    // такая презентация есть
        //    HowImport howImport = AskUserReplaceOrNew(presentation);
        //    if (howImport == HowImport.New)
        //    {
        //        _remotePresentationWorker.CopySourceFromLocalToLocal(_userIdentity, storedPresentationInfo.UniqueName,
        //                                                              presentation.UniqueName);
        //    }
        //    return howImport;
        //}

        private HowImport AskUserReplaceOrNew(TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation)
        {
            return
                _importPresentationController.HowToImportRequest(presentation.Name);
            //_replaceOrNewPresentationDelegate.Invoke(
            //    string.Format(
            //        "Сценарий с названием {0} уже есть в системе",
            //        presentation.Name));
        }


        #endregion
    }
}
