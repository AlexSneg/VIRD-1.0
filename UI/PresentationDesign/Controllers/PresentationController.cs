using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using UI.PresentationDesign.DesignUI.Classes.History;
using UI.PresentationDesign.DesignUI.Classes.Model;
using UI.PresentationDesign.DesignUI.Classes.View;
using Presentation = TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Threading;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using TechnicalServices.Entity;
using Domain.PresentationDesign.Client;
using UI.PresentationDesign.DesignUI.Classes.Helpers;
using TechnicalServices.Common.Utils;
using UI.PresentationDesign.DesignUI.Controllers;
using UI.PresentationDesign.DesignUI.Controls.SourceTree;
using TechnicalServices.Configuration.Common;
using UI.PresentationDesign.DesignUI.Services;
using System.ComponentModel;
using UI.PresentationDesign.DesignUI.Model;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using UI.PresentationDesign.DesignUI.Helpers;
using Domain.PresentationShow.ShowClient;
using TechnicalServices.Common.Classes;
using Link=TechnicalServices.Persistence.SystemPersistence.Presentation.Link;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    #region delegates
    /// <summary>
    /// Делегат на событие изменения выделения, вызываемого из PresentationController
    /// </summary>
    /// <param name="NewSelection">Список выделенных сцен</param>
    public delegate void SlideSelectionChanged(IEnumerable<Slide> NewSelection);

    /// <summary>
    /// Событие блокировки сцены текущего сценария
    /// </summary>
    public delegate void SlideLockChanged(Slide slide, bool IsLocked, LockingInfo info);

    /// <summary>
    /// Событие внтуреннего изменения блокировки сценария
    /// </summary>
    public delegate void PresentationLockChanged(bool IsLocked);

    public delegate void MonitorListChanged(IEnumerable<Display> newList);

    public delegate void CurrentSourceChanged(Source newSource);

    public delegate void CurrentDeviceChanged(Device newDevice);

    public delegate void SelectedResourceChanged(SourceWindow window);

    public delegate Slide[] GetSlides();

    public delegate void SavePresentation();

    public delegate void Changed();

    public delegate void SlideChanged(Slide slide);

    public delegate void PresentationDataChanged();

    public delegate void SlideLayoutChanged(SlideLayout NewSlideLayout);

    public delegate void PresentationLockedExternally(string UserName, RequireLock lockType);

    public delegate void PresentationUnlockedExternally(string UserName);

    #endregion

    /// <summary>
    /// Контроллер презентаций. Используется в PresentationDiagram
    /// </summary>
    public partial class PresentationController : IDisposable
    {
        #region Constants
        public const int MaxSlideCount = 1000;
        public const string SlideName = "Сцена";
        #endregion

        #region Events

        /// <summary>
        /// Обновилось выделение сцены
        /// </summary>
        public event SlideSelectionChanged OnSlideSelectionChanged;

        public event SlideChanged OnPlaySelectionChanged;

        /// <summary>
        /// Пользователь заблокировал сценарий
        /// </summary>
        public event PresentationLockChanged OnPresentationLockChanged;

        /// <summary>
        /// Пользователь заблокировал сцену
        /// </summary>
        public event SlideLockChanged OnSlideLockChanged;

        public event MonitorListChanged OnMonitorListChanged;

        public event CurrentSourceChanged OnSourceChanged;

        public event CurrentDeviceChanged OnDeviceChanged;

        public event SelectedResourceChanged OnSelectedResourceChanged;

        public event GetSlides OnGetCreatedSlides;

        public event GetSlides OnGetAllSlides;

        public event SavePresentation OnSavePresentation;

        public event Changed OnChanged;

        public event Changed OnPresentationRemoved;

        public event Changed OnUnlockAllSlides;

        public event PresentationDataChanged OnPresentationChangedExternally;

        public event SlideChanged OnSlideChangedExternally;

        /// <summary>вызывается, если текущий пользователь залочил сцену для редактирования, 
        /// а в этот момент кто то запустил показ сцены, в этом случае все изменения откатываются, 
        /// а пользователю надо вывести предупреждение </summary>
        public event SlideChanged OnOtherUserLockForShow;

        public event SlideLayoutChanged OnSlideLayoutChanged;

        public event PresentationLockedExternally OnPresentationLockedExternally;

        public event PresentationUnlockedExternally OnPresentationUnlockedExternally;

        public event Action<EquipmentType, bool?> OnHardwareStateChanged = delegate { };

        #endregion

        #region Fields and properties

        public bool SuppressLayoutChanging { get; set; }

        public static CommonConfiguration Configuration
        {
            get;
            set;
        }

        static bool _changed;

        public bool PresentationChanged
        {
            get { return _changed; }
            set
            {
                if (_changed != value)
                {
                    _changed = value;
                    if (OnChanged != null) OnChanged();
                }
            }
        }

        public string ChangedTextStatus
        {
            get
            {
                return PresentationChanged ? "Сценарий изменен" : String.Empty;
            }
        }

        Presentation.Presentation m_Presentation;
        PlayerController m_PlayerController = null;

        /// <summary>
        /// Возвращает выбранную сцена сценария или null, если нет выбранной сцены
        /// </summary>
        Slide _selectedSlide;
        public Slide SelectedSlide
        {
            get { return _selectedSlide; }
            set
            {
                bool flag = false;
                if (_selectedSlide != value)
                    flag = true;
                _selectedSlide = value;

                if (flag)
                    PerformChangeLayout();
            }
        }

        private static PresentationController _instance;

        public static PresentationController Instance
        {
            get
            {
                return _instance;
            }
        }

        bool _locked;

        //PresentationLockedByMe
        public bool PresentationLocked
        {
            get
            {
                return _locked;
            }
            set
            {
                _locked = value;
                if (OnPresentationLockChanged != null)
                    OnPresentationLockChanged(_locked);
            }
        }

        public bool SomeSlidesLocked
        {
            get
            {
                return m_Presentation.SlideList.Any(s => s.IsLocked);
            }
        }


        /// <summary>
        /// Возвращает ассоциированную презентацию
        /// </summary>
        public Presentation.Presentation Presentation
        {
            get { return m_Presentation; }
        }

        PresentationInfo m_PresentationInfo;
        public PresentationInfo PresentationInfo
        {
            get { return m_PresentationInfo; }
        }

        UserIdentity _identity;

        public UserIdentity UserIdentity
        {
            get { return _identity; }
        }

        public bool CanSelectPrevSlide { get; set; }
        public bool CanSelectNextSlide { get; set; }

        Display _selectedDisplay;
        public Display SelectedDisplay
        {
            get { return _selectedDisplay; }
            set
            {
                _selectedDisplay = value;
                _selectedDisplayGroup = null;
                PerformChangeLayout();
            }
        }

        List<Display> _selectedDisplayGroup;
        public List<Display> SelectedDisplayGroup
        {
            get { return _selectedDisplayGroup; }
            set
            {
                _selectedDisplayGroup = value;
                _selectedDisplay = null;
                PerformChangeLayout();
            }
        }

        SlideLayout _layout;
        public SlideLayout CurrentSlideLayout
        {
            get { return _layout; }
        }

        Identity sourceID = new Identity(0);
        public Identity SourceID 
        {
            get
            {
                return sourceID;
            }
            set
            {
                sourceID = value;
            }
        }

        #endregion

        #region Ctor and Factories

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PresentationController()
        {
            _instance = this;
            _identity = Thread.CurrentPrincipal as UserIdentity;
        }

        /// <summary>
        /// Назначить контроллер показа
        /// </summary>
        /// <param name="ctrl">Контроллер</param>
        public void AssignPlayerController(PlayerController ctrl)
        {
            m_PlayerController = ctrl;
        }

        /// <summary>
        /// Фабрика Контроллера
        /// </summary>
        /// <returns>Созданный контроллер</returns>
        public static PresentationController CreatePresentationController()
        {
            PresentationController result = new PresentationController();
            return result;
        }

        ///// <summary>
        ///// Фабрика сценария
        ///// </summary>
        //public static Presentation.Presentation NewPresentation(string AName, string authorName)
        //{
        //    Presentation.Presentation result = new Presentation.Presentation() { Name = AName, Author = authorName };
        //    Slide slide = new Slide { Id = 1, LabelId = -1, Name = "Сцена 1", Author = authorName, Modified = DateTime.Now, State = SlideState.New };
        //    result.SlideList.Add(slide);
        //    result.StartSlide = slide;

        //    return result;
        //}

        /// <summary>
        /// Фабрика сценария
        /// </summary>
        public static Presentation.Presentation NewPresentation(string AName, string authorName, int numberOfSlides)
        {
            Presentation.Presentation result = new Presentation.Presentation() { Name = AName, Author = authorName };
            DateTime now = DateTime.Now;
            for (int i = 1; i<=numberOfSlides; i++)
            {
                Slide slide = new Slide { Id = i, LabelId = -1, Name = string.Format("Сцена {0}", i), Author = authorName, Modified = now, State = SlideState.New };
                result.SlideList.Add(slide);
                if (i == 1) result.StartSlide = slide;
            }
            for (int i = 0; i < numberOfSlides - 1; i++ )
            {
                SlideLinkList list = new SlideLinkList();
                list.LinkList.Add(new Link() { IsDefault = true, NextSlide = result.SlideList[i+1]});
                result.LinkDictionary.Add(i + 1, list);
            }
            return result;
        }


        /// <summary>
        /// Выполняет загрузку презентации и дополнительных данных
        /// </summary>
        public void AssignPresentation(Presentation.Presentation APresentation, PresentationInfo APresentationInfo)
        {
            m_Presentation = APresentation;
            m_PresentationInfo = APresentationInfo;
            if (!DesignerClient.Instance.IsStandAlone)
            {
                SubscribeForMonitor(true);
            }
        }

        public void RefreshLockingInfo()
        {
            if (!DesignerClient.Instance.IsStandAlone)
            {
                if (m_PresentationInfo is PresentationInfoExt)
                {
                    LockingInfo li = ((PresentationInfoExt)m_PresentationInfo).LockingInfo;
                    if (li != null)
                    {
                        this.PresentationLocked = li.UserIdentity.Equals(_identity);
                        return;
                    }
                }
                this.PresentationLocked = false;
                return;
            }

            this.PresentationLocked = true;
        }

        #endregion

        #region Layout
        private void PerformChangeLayout()
        {
            _layout = null;
            if (SelectedSlide != null)
            {
                if (_selectedDisplay != null)
                    _layout = new SlideLayout(new Display[] { _selectedDisplay }, SelectedSlide);
                else
                {
                    if (_selectedDisplayGroup != null)
                        _layout = new SlideLayout(_selectedDisplayGroup, SelectedSlide);
                }
            }

            if (OnSlideLayoutChanged != null && !SuppressLayoutChanging)
                OnSlideLayoutChanged(_layout);
        }
        #endregion

        #region monitorign locks and unlocks
        private void SubscribeForMonitor(bool NotOnlyDelegates)
        {
            DesignerClient.Instance.PresentationNotifier.OnObjectLocked += new EventHandler<NotifierEventArg<LockingInfo>>(PresentationNotifier_OnObjectLocked);
            DesignerClient.Instance.PresentationNotifier.OnObjectUnLocked += new EventHandler<NotifierEventArg<LockingInfo>>(PresentationNotifier_OnObjectUnLocked);
            DesignerClient.Instance.PresentationNotifier.OnObjectChanged += new EventHandler<NotifierEventArg<IList<ObjectInfo>>>(PresentationNotifier_OnObjectChanged);
            DesignerClient.Instance.PresentationNotifier.OnPresentationDeleted += new EventHandler<NotifierEventArg<PresentationInfo>>(PresentationNotifier_OnPresentationDeleted);
            ShowClient.Instance.OnEquipmentStateChanged += new Action<EquipmentType, bool?>(Instance_OnEquipmentStateChanged);

            if (NotOnlyDelegates)
                DesignerClient.Instance.PresentationWorker.SubscribeForMonitor(new PresentationKey(m_Presentation.UniqueName));
        }

        void Instance_OnEquipmentStateChanged(EquipmentType arg1, bool? arg2)
        {
            OnHardwareStateChanged(arg1, arg2);
        }

        void PresentationNotifier_OnPresentationDeleted(object sender, NotifierEventArg<PresentationInfo> e)
        {
            if (e.Data.UniqueName == m_Presentation.UniqueName)
            {
                if (OnPresentationRemoved != null)
                    OnPresentationRemoved();
            }
        }

        void PresentationNotifier_OnObjectChanged(object sender, NotifierEventArg<IList<ObjectInfo>> e)
        {
            if (e.Data != null)
            {
                foreach (ObjectInfo info in e.Data)
                {
                    if (!_identity.Equals(info.UserIdentity))
                    {
                        if (info.ObjectKey.GetObjectType() == ObjectType.Presentation)
                        {
                            if (m_Presentation.UniqueName == ((PresentationKey)info.ObjectKey).PresentationUniqueName && !PresentationLocked)
                            {
                                m_PresentationInfo = DesignerClient.Instance.PresentationWorker.GetPresentationInfo(m_Presentation.UniqueName);
                                Slide[] slides = GetLockedSlides();
                                m_Presentation = m_PresentationInfo.CreatePresentationStub();
                                //merge
                                foreach (Slide s in slides)
                                {
                                    Slide dest = m_Presentation.SlideList.Find(sl => sl.Id == s.Id);
                                    if (dest == null) continue;
                                    dest.SaveSlideLevelChanges(s);
                                    dest.IsLocked = s.IsLocked;
                                    dest.Cached = s.Cached;
                                    dest.State = s.State;
                                    //dest.SourceList.Clear();
                                    //dest.SourceList.AddRange(s.SourceList);
                                    //dest.DeviceList.Clear();
                                    //dest.DeviceList.AddRange(s.DeviceList);
                                    //dest.DisplayList.Clear();
                                    //dest.DisplayList.AddRange(s.DisplayList);
                                }

                                //update presentation
                                if (OnPresentationChangedExternally != null)
                                    OnPresentationChangedExternally();

                                continue;
                            }
                        }

                        if (info.ObjectKey.GetObjectType() == ObjectType.Slide)
                        {
                            string presentationUniqueName =
                                ((PresentationKey) ((SlideKey) info.ObjectKey).PresentationKey).PresentationUniqueName;
                            int slideId = ((SlideKey) info.ObjectKey).Id;
                            if (presentationUniqueName == this.m_Presentation.UniqueName)
                            {
                                Slide slide = m_Presentation.SlideList.FirstOrDefault(s => s.Id == ((SlideKey)info.ObjectKey).Id);
                                if (slide != null)
                                {
                                    slide.Cached = false;
                                    LoadSlide(slide);

                                    //refresh slide data
                                    if (OnSlideChangedExternally != null)
                                        OnSlideChangedExternally(slide);

                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UnsubscribeFromMonitor(bool NotOnlyDelegates)
        {
            DesignerClient.Instance.PresentationNotifier.OnObjectLocked -= new EventHandler<NotifierEventArg<LockingInfo>>(PresentationNotifier_OnObjectLocked);
            DesignerClient.Instance.PresentationNotifier.OnObjectUnLocked -= new EventHandler<NotifierEventArg<LockingInfo>>(PresentationNotifier_OnObjectUnLocked);
            DesignerClient.Instance.PresentationNotifier.OnObjectChanged -= new EventHandler<NotifierEventArg<IList<ObjectInfo>>>(PresentationNotifier_OnObjectChanged);
            DesignerClient.Instance.PresentationNotifier.OnPresentationDeleted -= new EventHandler<NotifierEventArg<PresentationInfo>>(PresentationNotifier_OnPresentationDeleted);
            ShowClient.Instance.OnEquipmentStateChanged -= new Action<EquipmentType, bool?>(Instance_OnEquipmentStateChanged);
            if (NotOnlyDelegates)
                DesignerClient.Instance.PresentationWorker.UnSubscribeForMonitor(new PresentationKey(Presentation.UniqueName));
        }

        private object _lockUpdateSlideStatus = new object();
        private void UpdateSlideStatus(SlideKey key, LockingInfo info)
        {
            //это касается нашего сценария?
            if (((PresentationKey)(key.PresentationKey)).PresentationUniqueName == this.m_Presentation.UniqueName)
            {
                Slide slide = m_Presentation.SlideList.FirstOrDefault(s => s.Id == key.Id);
                lock (_lockUpdateSlideStatus)
                {
                    if (slide != null)
                    {
                        //update lock info
                        SlideInfo sInfo = m_PresentationInfo.SlideInfoList.FirstOrDefault(s => s.Id == key.Id);
                        bool otherUserLockForShow = ((sInfo.LockingInfo != null) && (info != null) && !sInfo.LockingInfo.UserIdentity.Equals(info.UserIdentity) && (info.RequireLock == RequireLock.ForShow));
                        sInfo.LockingInfo = info;
                        if ((info == null) || otherUserLockForShow) //slide changed something else
                        {
                            slide.Cached = false;
                            slide.State = SlideState.Normal;
                            LoadSlide(slide);
                            //if (otherUserLockForShow && (OnSlideChangedExternally != null))
                            //    OnSlideChangedExternally(slide);

                            if (SelectedSlide.Id == slide.Id)
                                PerformChangeLayout();
                        }
                        SendOnSlideLockChanged(slide, info != null, info);
                        if (otherUserLockForShow && (OnOtherUserLockForShow != null) && PresentationChanged)
                        {
                            OnOtherUserLockForShow(slide);
                        }
                    }
                }
            }
        }

        void PresentationNotifier_OnObjectUnLocked(object sender, NotifierEventArg<LockingInfo> e)
        {
            if (!e.Data.UserIdentity.Equals(_identity))
            {
                if (e.Data.ObjectKey is SlideKey)
                {
                    UpdateSlideStatus((SlideKey)e.Data.ObjectKey, null);
                    return;
                }

                if (e.Data.ObjectKey.GetObjectType() == ObjectType.Presentation)
                {
                    if (m_Presentation.UniqueName == ((PresentationKey)e.Data.ObjectKey).PresentationUniqueName)
                    {
                        //this.PresentationLocked = false;

                        if (OnPresentationUnlockedExternally != null)
                        {
                            UserIdentity i = e.Data.UserIdentity;
                            string name = String.IsNullOrEmpty(i.User.FullName) ? i.Name : i.User.FullName;
                            OnPresentationUnlockedExternally(name);
                        }
                    }
                }
            }
        }

        void PresentationNotifier_OnObjectLocked(object sender, NotifierEventArg<LockingInfo> e)
        {
            if (!e.Data.UserIdentity.Equals(_identity))
            {
                if (e.Data.ObjectKey is SlideKey)
                {
                    UpdateSlideStatus((SlideKey)e.Data.ObjectKey, e.Data);
                    return;
                }

                if (e.Data.ObjectKey.GetObjectType() == ObjectType.Presentation)
                {
                    if (m_Presentation.UniqueName == ((PresentationKey)e.Data.ObjectKey).PresentationUniqueName)
                    {
                        //this.PresentationLocked = false;

                        if (OnPresentationLockedExternally != null)
                        {
                            UserIdentity i = e.Data.UserIdentity;
                            string name = String.IsNullOrEmpty(i.User.FullName) ? i.Name : i.User.FullName;
                            OnPresentationLockedExternally(name, e.Data.RequireLock);
                        }
                    }
                }
            }
        }

        #endregion

        #region Slides

        public bool IsSelectedSlideLocked()
        {
            return SelectedSlide.IsLocked;
        }

        public bool IsSlidesLocked(IEnumerable<Slide> slides)
        {
            return slides.All(s => s.IsLocked);
        }

        public bool IsSlideUniqueName(string name, string exceptOne)
        {
            return !m_Presentation.SlideList.Any(s => s.Name.ToUpper() == name.ToUpper() && s.Name.ToUpper() != exceptOne.ToUpper());
        }

        #endregion

        #region Monitoring
        public void NotifyMonitorListChanged(IEnumerable<Display> displays)
        {
            if (OnMonitorListChanged != null)
                OnMonitorListChanged(displays);
        }
        #endregion

        #region Sources
        public void NotifyCurrentSourceChanged(Source newSource)
        {
            if (OnSourceChanged != null)
                OnSourceChanged(newSource);
        }
        #endregion

        #region Devices
        public void NotifyCurrentDeviceChanged(Device newDevice)
        {
            if (OnDeviceChanged != null)
                OnDeviceChanged(newDevice);
        }


        #endregion

        #region Load/Save, Lock/Unlock

        public void UnlockAllSlides(bool p)
        {
            if (p)
                UnlockAllSlides();
            else
                foreach (Slide s in GetLockedSlides())
                    DesignerClient.Instance.PresentationWorker.ReleaseLockForSlide(m_PresentationInfo.UniqueName, s.Id);
        }

        public bool CanUnlockSlides(IEnumerable<Slide> Slides)
        {
            return Slides.All(s => CanUnlockSlide(s));
        }

        public bool CanLockSlide(Slide slide)
        {
            LockingInfo info = GetSlideLockingInfo(slide);
            if (info != null)
                return false;
            else
                return true;
        }

        public bool CanUnlockSlide(Slide slide)
        {
            LockingInfo info = GetSlideLockingInfo(slide);
            if (info != null && !info.UserIdentity.Equals(this._identity))
                return false;

            if (info == null)
                return false;

            return true;
        }

        public bool CanUnlockSomeSlides(IEnumerable<Slide> Slides)
        {
            return Slides.Any(s => CanUnlockSlide(s));
        }

        public LockingInfo GetSlideLockingInfo(Slide slide)
        {
            var slides = m_PresentationInfo.SlideInfoList.Where(s => s.Id == slide.Id);
            if (slides.Count() > 0)
            {
                return slides.First().LockingInfo;
            }
            return null;
        }

        public bool LockSlide(Slide slide)
        {
            LockingInfo[] info = DesignerClient.Instance.PresentationWorker.GetLockingInfo(new[] { new SlideKey(m_PresentationInfo.UniqueName, slide.Id) });
            string reasons = String.Empty;
            if (info.Length > 0)
            {
                reasons = PresentationStatusInfo.GetSlideLockingInfoDescr(slide.Name, info.First());
            }

            if (CanLockSlide(slide))
            {
                if (DesignerClient.Instance.PresentationWorker.AcquireLockForSlide(m_PresentationInfo.UniqueName, slide.Id, RequireLock.ForEdit))
                {
                    //обновляем информацию в сценарии
                    m_PresentationInfo.SlideInfoList.Where(s => s.Id == slide.Id).First().LockingInfo = new LockingInfo(_identity, RequireLock.ForEdit, new SlideKey(m_PresentationInfo.UniqueName, slide.Id));

                    SendOnSlideLockChanged(slide, true, info.FirstOrDefault());
                    return true;
                }
                else
                {
                    MessageBoxExt.Show(String.Format("Блокировка невозможна!\r\n{0}", reasons), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }

        public bool UnlockSlide(Slide slide)
        {
            if (!CanUnlockSlide(slide))
            {
                //MessageBoxExt.Show(String.Format("Вы не можете разблокировать сцену {0}, потому что она заблокирована другим пользователем", slide.Name), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            bool ok = true;
            if (slide.Cached)
                ok = SaveSlideChanges(new[] { slide });

            if (ok)
            {
                //slide.Cached = false; //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1619
                bool result = DesignerClient.Instance.PresentationWorker.ReleaseLockForSlide(m_PresentationInfo.UniqueName, slide.Id);
                if (result)
                {
                    //обновляем информацию в сценарии
                    m_PresentationInfo.SlideInfoList.Where(s => s.Id == slide.Id).First().LockingInfo = null;
                    SendOnSlideLockChanged(slide, false, null);
                }

                return result;
            }

            return ok;
        }

        public void UnlockAllSlides()
        {
            if (OnUnlockAllSlides != null)
                OnUnlockAllSlides();
        }

        public Slide[] GetLockedSlides()
        {
            var slides = GetAllSlides().Where(s => s.IsLocked);
            if (slides.Count() > 0)
                return slides.ToArray();
            else
                return new Slide[] { };
        }

        public Slide[] GetCreatedSlides()
        {
            if (OnGetCreatedSlides != null)
                return OnGetCreatedSlides();

            return new Slide[] { };
        }

        public Slide[] GetAllSlides()
        {
            if (OnGetAllSlides != null)
                return OnGetAllSlides();

            return new Slide[] { };
        }

        public void LoadSlide(Slide CurrentSlide)
        {
            if (!CurrentSlide.Cached && CurrentSlide.State != SlideState.New && CurrentSlide.State != SlideState.Edit)
            {
                Slide[] slideArray = DesignerClient.Instance.PresentationWorker.LoadSlides(PresentationController.Instance.PresentationInfo.UniqueName, new[] { CurrentSlide.Id });
                if (slideArray != null && slideArray.Length > 0)
                {
                    Slide slide = slideArray.First();
                    CurrentSlide.SaveSlideLevelChanges(slide);
                    //CurrentSlide.DeviceList.Clear();
                    //CurrentSlide.DeviceList.AddRange(slide.DeviceList);
                    //CurrentSlide.DisplayList.Clear();
                    //CurrentSlide.DisplayList.AddRange(slide.DisplayList);
                    //CurrentSlide.SourceList.Clear();
                    //CurrentSlide.SourceList.AddRange(slide.SourceList);
                    CurrentSlide.Cached = true;
                }
            }
        }

        public void SavePresentationChanges()
        {
            if (OnSavePresentation != null)
                OnSavePresentation();
        }

        public bool SavePresentation()
        {
            SavePresentationChanges();
            UpdatePresentationInfo();

            if (PresentationLocked)
            {
                bool ok = SavePresentationOnly();
                if (!ok) return false;
                PresentationInfo info = DesignerClient.Instance.PresentationWorker.GetPresentationInfo(PresentationController.Instance.Presentation.UniqueName);
                PresentationInfo.LastChangeDate = info.LastChangeDate;

                UndoService.Instance.ClearHistory();

                if (DesignerClient.Instance.IsStandAlone)
                    ok = SaveAllSlides();

                if (!ok) return false;

                //сбросить состояния сцен
                SaveStates();
            }

            if (GetLockedSlides().Length > 0)
            {
                //сохранить только изменения в заблокированных сценах
                if (!SaveCachedSlides())
                    return false;
            }

            UndoService.Instance.ClearHistory();
            PresentationChanged = false;
            return true;
        }

        private void UpdatePresentationInfo()
        {
            m_PresentationInfo.Name = m_Presentation.Name ?? String.Empty;
            m_PresentationInfo.UniqueName = m_Presentation.UniqueName;
            m_PresentationInfo.Author = m_Presentation.Author ?? String.Empty;
            m_PresentationInfo.CreationDate = m_Presentation.CreationDate;
            m_PresentationInfo.LastChangeDate = m_Presentation.LastChangeDate;
            m_PresentationInfo.Comment = m_Presentation.Comment ?? String.Empty;
            m_PresentationInfo.SlideCount = m_Presentation.SlideList.Count;
            m_PresentationInfo.StartSlideId = m_Presentation.StartSlideId;

            foreach (Slide slide in m_Presentation.SlideList)
            {
                SlideInfo info = new SlideInfo(m_PresentationInfo, slide);
                if (!m_PresentationInfo.SlideInfoList.Contains(info))
                {
                    m_PresentationInfo.SlideInfoList.Add(info);
                }
                else
                {
                    //save locking info
                    int index = m_PresentationInfo.SlideInfoList.IndexOf(info);
                    LockingInfo l_info = m_PresentationInfo.SlideInfoList[index].LockingInfo;
                    info.LockingInfo = l_info;
                    m_PresentationInfo.SlideInfoList[index] = info;
                }
            }

            m_PresentationInfo.DisplayGroupList.Clear();
            m_PresentationInfo.DisplayGroupList.AddRange(m_Presentation.DisplayGroupList);
            m_PresentationInfo.SlidePositionList.Clear();
            foreach (KeyValuePair<int, Point> pair in m_Presentation.SlidePositionList)
            {
                m_PresentationInfo.SlidePositionList.Add(pair.Key, pair.Value);
            }
            m_PresentationInfo.DisplayPositionList.Clear();
            foreach (KeyValuePair<string, int> pair in m_Presentation.DisplayPositionList)
            {
                m_PresentationInfo.DisplayPositionList.Add(pair.Key, pair.Value);
            }

            m_PresentationInfo.SlideLinkInfoList.Clear();
            foreach (KeyValuePair<int, SlideLinkList> pair in m_Presentation.LinkDictionary)
            {
                if (pair.Value.LinkList.Count > 0)
                {
                    List<LinkInfo> linkInfoList = new List<LinkInfo>();
                    foreach (Presentation.Link link in pair.Value.LinkList)
                    {
                        linkInfoList.Add(new LinkInfo(link));
                    }

                    m_PresentationInfo.SlideLinkInfoList.Add(pair.Key, linkInfoList);
                }
            }
        }


        public void SaveStates()
        {
            m_Presentation.SlideList.ForEach(s => s.State = SlideState.Normal);
        }

        public bool SaveAllSlides()
        {
            return SaveSlideChanges(GetAllSlides());
        }

        public bool SaveCachedSlides()
        {
            var cached = GetAllSlides().Where(s => s.Cached && s.IsLocked && CanUnlockSlide(s));
            if (cached.Count() > 0)
                return SaveSlideChanges(cached.ToArray());

            return true;
        }

        public bool SaveSlideChanges(Slide[] slides)
        {
            if (!DesignerClient.Instance.IsStandAlone)
                slides = (from sl in slides where CanUnlockSlide(sl) select sl).ToArray();

            if (slides.Length > 0)
            {
                // валидация дисплеев
                Slide slide = null;
                try
                {
                    for (int i = 0; i < slides.Length; i++)
                    {
                        slide = slides[i];
                        foreach (var d in slide.DisplayList)
                            d.Validate(slide);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxExt.Show(ex.Message, "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1499
                    if (slide != null)
                        SlideGraphController.Instance.SelectSlide(slide);
                    return false;
                }

                int[] slideIdNotLocked;
                ResourceDescriptor[] resourceNotExists;
                DeviceResourceDescriptor[] deviceResourceNotExists;
                int[] labelNotExists;
                if (
                    !DesignerClient.Instance.PresentationWorker.SaveSlideChanges(Presentation.UniqueName,
                                                                                 slides, out slideIdNotLocked,
                                                                                 out resourceNotExists,
                                                                                 out deviceResourceNotExists,
                                                                                 out labelNotExists))
                {
                    StringBuilder reasons = new StringBuilder();
                    if (slideIdNotLocked != null && slideIdNotLocked.Length > 0)
                    {
                        reasons.Append("Следующие сцены не заблокированы: ");
                        var ids = slideIdNotLocked.Take(10);
                        reasons.Append(
                            Presentation.SlideList.Where(s => ids.Contains(s.Id)).Select(s => s.Name).Aggregate(
                                (first, second) => first + "\r\n" + second));
                        if (slideIdNotLocked.Length > 10)
                            reasons.Append("\r\n...\r\n");
                        reasons.Append("\r\n--------------\r\n");
                    }

                    if (resourceNotExists != null && resourceNotExists.Length > 0)
                    {
                        reasons.Append("Следующие ресурсы не найдены: ");
                        reasons.Append(
                            resourceNotExists.Take(10).Select(r => r.ResourceInfo.Name).Aggregate(
                                (first, second) => first + "\r\n" + second));
                        if (resourceNotExists.Length > 10)
                            reasons.Append("\r\n...\r\n");
                        reasons.Append("\r\n--------------\r\n");
                    }
                    if (deviceResourceNotExists != null && deviceResourceNotExists.Length > 0)
                    {
                        reasons.Append("Следующие ресурсы девайсов не найдены: ");
                        reasons.Append(
                            deviceResourceNotExists.Take(10).Select(r => r.ResourceInfo.Name).Aggregate(
                                (first, second) => first + "\r\n" + second));
                        if (deviceResourceNotExists.Length > 10)
                            reasons.Append("\r\n...\r\n");
                        reasons.Append("\r\n--------------\r\n");

                    }

                    MessageBoxExt.Show("Не удалось сохранить одну или несколько сцен\r\n" + reasons.ToString(),
                                       "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    //String outId;
                    foreach (var dev in (from s in slides from d in s.DeviceList where d.DeviceResourceDescriptor != null select d))
                        SaveDeviceResourceDescriptor(dev.DeviceResourceDescriptor);
                }
            }
            return true;
        }

        public bool SavePresentationOnly()
        {
            string reasons =
                PresentationStatusInfo.GetPresentationStatusDescr(PresentationController.Instance.Presentation);
            ResourceDescriptor[] descr;
            DeviceResourceDescriptor[] deviceResourceNotExists;
            int[] labelNotExists;
            UserIdentity[] whoLock;
            int[] slidesAlreadyExistsId;

            var slides = GetCreatedSlides();
            if (PresentationLocked)
            {
                PresentationInfo pi = new PresentationInfo(Presentation);
                SavePresentationResult result = DesignerClient.Instance.PresentationWorker.SavePresentationChanges(
                    pi, slides, out descr, out deviceResourceNotExists, out labelNotExists,
                    out whoLock, out slidesAlreadyExistsId);

                if (result != SavePresentationResult.Ok)
                {
                    string message = reasons;
                    if (result == SavePresentationResult.SlideLocked)
                    {
                        string userWhoLocked = string.Empty;
                        if (whoLock != null && whoLock.Length > 0)
                            userWhoLocked = whoLock[0].User.Name;
                        message = string.Format("Сцена заблокирована пользователем {0}", userWhoLocked);
                    }
                    MessageBoxExt.Show(String.Concat("Сценарий сохранить не удалось\r\n", message), "Ошибка",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
            }

            return false;
        }

        public bool IsLockGUICommandEnabled(IEnumerable<Slide> slides)
        {
            return slides.Count() > 0 && slides.Any(s => !s.IsLocked);
        }

        public bool IsUnlockGUICommandEnabled(IEnumerable<Slide> slides)
        {
            return slides.Count() > 0 && ((slides.Count() == 1 && CanUnlockSlides(slides)) || CanUnlockSomeSlides(slides));
        }

        /// <summary>
        ///  сохранение ресурсов девайсов и на сервер и в кэш
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public FileSaveStatus SaveDeviceResourceDescriptor(DeviceResourceDescriptor descriptor)
        {
            return DesignerClient.Instance.PresentationWorker.SaveDeviceSource(descriptor);
        }


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                UnsubscribeFromMonitor(true);

            _instance.OnChanged = null;
            _instance.OnGetAllSlides = null;
            _instance.OnGetCreatedSlides = null;
            _instance.OnMonitorListChanged = null;
            _instance.OnPresentationChangedExternally = null;
            _instance.OnPresentationLockChanged = null;
            _instance.OnPresentationLockedExternally = null;
            _instance.OnPresentationRemoved = null;
            _instance.OnSavePresentation = null;
            _instance.OnSelectedResourceChanged = null;
            _instance.OnSlideChangedExternally = null;
            _instance.OnSlideLayoutChanged = null;
            _instance.OnSlideLockChanged = null;
            _instance.OnSlideSelectionChanged = null;
            _instance.OnSourceChanged = null;
            _instance.OnUnlockAllSlides = null;
            _instance.OnPresentationUnlockedExternally = null;
            _instance = null;
        }

        #endregion

        #region Event sink
        public void SendSelectionChanged(IEnumerable<Slide> slides)
        {
            if (slides.Count() > 0)
                this.SelectedSlide = slides.First();

            if (OnSlideSelectionChanged != null)
                OnSlideSelectionChanged(slides);
        }

        public void SendPlaySelectionChanged(Slide newSlide)
        {
            this.SelectedSlide = newSlide;
            if (OnPlaySelectionChanged != null)
                OnPlaySelectionChanged(newSlide);
        }

        public void SendOnSlideLockChanged(Slide slide, bool Locked, LockingInfo info)
        {
            if (OnSlideLockChanged != null)
                OnSlideLockChanged(slide, Locked, info);
        }

        public void SendOnSelectedResourceChanged(SourceWindow selected)
        {
            if (OnSelectedResourceChanged != null)
                OnSelectedResourceChanged(selected);
        }
        #endregion


    }

}
