using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using Syncfusion.Windows.Forms.Diagram;
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
using System.Drawing;
using UI.PresentationDesign.DesignUI.Forms;
using UI.PresentationDesign.DesignUI.Controls;
using Link = Syncfusion.Windows.Forms.Diagram.Link;
using UI.PresentationDesign.DesignUI.Helpers;
using UI.PresentationDesign.DesignUI.Model;
using TechnicalServices.Common.Classes;
using UI.PresentationDesign.DesignUI.Services;
using UI.PresentationDesign.DesignUI.History;
using TechnicalServices.Interfaces;
using Domain.PresentationShow.ShowClient;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public delegate void ControllerCheckSelection(NodeCollection nodes);
    public delegate void CurrentSlideChanged(Slide slide);
    public delegate void LabelsChanged();

    public class SlideGraphController : PresentationDiagramControllerBase, ISelectionController, IDisposable, IPropertyContainer, ISimpleUndoRedoAction
    {
        #region Constants
        public const int MaxSlideCount = 1000;
        public const string NonamedSlide = "Без имени";
        public const string SlideName = "Сцена";
        #endregion

        #region events
        public event ControllerCheckSelection OnCheckSelection;
        public event CurrentSlideChanged OnCurrentSlideChanged;
        public event System.Action OnLabelListChanhed;
        #endregion

        #region Fields and properties
        bool _inited = false;
        Slide _prevSlide = null;
        string Author = String.Empty;

        SlideView slideViews(Slide slide)
        {
            var result = Model.Nodes.OfType<SlideView>().Where(s => s.Slide.Id == slide.Id);
            return result.SingleOrDefault();
        }

        IEnumerable<SlideView> slideViews()
        {
            return Model.Nodes.OfType<SlideView>();
        }


        public SlideView StartSlide
        {
            get { return slideViews().Where(s => s.Slide.Id == m_startSlideId).FirstOrDefault(); }
            set { m_startSlideId = value.Slide.Id; }
        }

        int m_startSlideId { get; set; }

        public Slide SelectedSlide
        {
            get
            {
                return GetSelectedSlide();
            }
        }

        public SlideView SelectedSlideView
        {
            get
            {
                if (GetSelectedSlide() != null)
                    return slideViews(GetSelectedSlide());
                return null;
            }
        }

        private static SlideGraphController _instance;
        public static SlideGraphController Instance
        {
            get
            {
                return _instance;
            }
        }

        Identity SlideIdenty;

        public int SlideCount
        {
            get
            {
                return Model.Nodes.OfType<SlideView>().Count();
            }
        }


        PlayerController m_PlayerController = null;

        #endregion

        #region Ctor and Factories

        readonly UserIdentity identity;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SlideGraphController()
        {
            _instance = this;
            identity = Thread.CurrentPrincipal as UserIdentity;
        }

        protected override void OnToolActivated(ToolEventArgs evtArgs)
        {
            base.OnToolActivated(evtArgs);
        }

        /// <summary>
        /// Фабрика Контроллера
        /// </summary>
        /// <returns>Созданный контроллер</returns>
        public static SlideGraphController CreateSlideGraphController()
        {
            SlideGraphController result = new SlideGraphController();
            return result;
        }

        public void InitController()
        {
            LoadPresentation(false);

            PresentationController.Instance.SendSelectionChanged(new[] { StartSlide.Slide });
            PresentationController.Instance.OnSlideLockChanged += new SlideLockChanged(Instance_OnSlideLockChanged);
            PresentationController.Instance.OnUnlockAllSlides += new Changed(Instance_OnUnlockAllSlides);
            PresentationController.Instance.OnGetAllSlides += new GetSlides(Instance_OnGetAllSlides);
            PresentationController.Instance.OnGetCreatedSlides += new GetSlides(Instance_OnGetCreatedSlides);
            PresentationController.Instance.OnSavePresentation += new SavePresentation(Instance_OnSavePresentation);
            PresentationController.Instance.OnPresentationChangedExternally += new PresentationDataChanged(Instance_OnPresentationChangedExternally);
            PresentationController.Instance.OnSlideChangedExternally += new SlideChanged(Instance_OnSlideChangedExternally);
            PresentationController.Instance.OnPresentationLockChanged += new PresentationLockChanged(Instance_OnPresentationLockChanged);
            DesignerClient.Instance.PresentationNotifier.OnLabelDeleted += PresentationNotifier_OnLabelDeleted;
            DesignerClient.Instance.PresentationNotifier.OnLabelAdded += PresentationNotifier_OnLabelAdded;
            DesignerClient.Instance.PresentationNotifier.OnLabelUpdated += PresentationNotifier_OnLabelAdded;
            ShowClient.Instance.OnGoToSlide += Instance_OnSlideChangedExternally;
            Model.HistoryManager.RecordComplete += new EventHandler(HistoryManager_RecordComplete);

            PleaseUpdateMe(true);
        }

        private void PresentationNotifier_OnLabelAdded(object sender, NotifierEventArg<TechnicalServices.Persistence.SystemPersistence.Configuration.Label> e)
        {
            if (OnLabelListChanhed != null)
                OnLabelListChanhed();
        }

        private void PresentationNotifier_OnLabelDeleted(object sender, NotifierEventArg<TechnicalServices.Persistence.SystemPersistence.Configuration.Label> e)
        {
            List<Slide> slds = GetAllSlides().Where(sld => sld.IsLocked && sld.LabelId.Equals(e.Data.Id)).ToList();
            if (slds.Count > 0)
            {
                StringBuilder message = new StringBuilder("Администратор удалил метку " + e.Data.Name);
                message.Append(". Метка использовалась в слайде: ");
                foreach (Slide item in slds)
                {
                    item.LabelId = TechnicalServices.Persistence.SystemPersistence.Configuration.Label.NullId;
                    message.Append(item.Name);
                }
                message.Append(". Метка будет удалена из слайда.");
                MessageBoxExt.Show(message.ToString(), "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (OnLabelListChanhed != null)
                OnLabelListChanhed();
        }

        private void PleaseUpdateMe(bool goHome)
        {
            ((Control)this.Viewer).Invoke(new MethodInvoker(() =>
            {
                RefreshDefaultSlidePath(true);

                if (goHome)
                    GoHome(true);

                if (OnCheckSelection != null)
                    OnCheckSelection(this.SelectionList);
            }));
        }

        private void LoadPresentation(bool rememberSelected)
        {
            Slide selected = SelectedSlide;

            if (rememberSelected)
                PresentationController.Instance.SuppressLayoutChanging = true;

            this.Model.BeginUpdate();
            this.Model.Clear();

            Presentation.Presentation m_Presentation = PresentationController.Instance.Presentation;
            PresentationInfo m_PresentationInfo = PresentationController.Instance.PresentationInfo;

            _inited = true;
            Author = m_PresentationInfo.Author;

            using (SplashForm form = SplashForm.CreateAndShowForm(false, false))
            {
                //create views for slides
                m_Presentation.SlideList.ForEach(slide =>
                {
                    form.Status = "Обработка сцены " + slide.Name;
                    form.Progress = (int)((float)(m_Presentation.SlideList.IndexOf(slide) + 1) / m_Presentation.SlideList.Count * 100.0f);

                    PointF? point = null;
                    if (m_Presentation.SlidePositionList.ContainsKey(slide.Id))
                    {
                        point = m_Presentation.SlidePositionList[slide.Id];
                    }

                    //slide.State = SlideState.Normal;
                    CreateSlideView(slide, point ?? GetNextSlideViewPos());
                });

                m_Presentation.SlideList.ForEach(slide =>
                {
                    slide.LinkList().ForEach(link =>
                    {
                        form.Status = "Обработка связи " + slide.Name;
                        form.Progress = (int)((slide.LinkList().IndexOf(link) + 1) / (float)slide.LinkList().Count * 100.0f);

                        SlideView v1 = slideViews(slide);
                        SlideView v2 = slideViews(link.NextSlide);
                        SlideLink slidelink = new SlideLink(PointF.Empty, PointF.Empty);

                        v1.CentralPort.TryConnect(slidelink.TailEndPoint);
                        v2.CentralPort.TryConnect(slidelink.HeadEndPoint);

                        slidelink.IsDefault = link.IsDefault;

                        Model.AppendChild(slidelink);
                        slidelink.Refresh();
                    });
                });

                StartSlide = slideViews(m_Presentation.StartSlide);
                StartSlide.IsStartSlide = true;

                UpdateStartSlide();
                RefreshDefaultSlidePath(true);

                SlideIdenty = new Identity(m_Presentation.SlideList.Max(s => s.Id));

                m_PresentationInfo.SlideInfoList.ForEach(s =>
                {
                    var slides = m_Presentation.SlideList.Where(slide => slide.Id == s.Id);
                    if (slides.Count() > 0)
                    {
                        //slideViews(slides.First()).IsLocked = s.LockingInfo != null;
                        if (s.LockingInfo != null)
                            slideViews(slides.First()).Lock(s.LockingInfo.RequireLock == RequireLock.ForEdit);
                        else
                            slideViews(slides.First()).Unlock();
                    }
                });
            }
            this.Model.EndUpdate();
            Model.HistoryManager.ClearHistory();

            if (selected != null && selected.IsLocked && PresentationController.Instance.CanUnlockSlide(selected))
            {
                Slide selClone = m_Presentation.SlideList.Where(s => s.Id == selected.Id).FirstOrDefault();
                if (selClone != null)
                {
                    selClone.SourceList.Clear();
                    selClone.SourceList.AddRange(selected.SourceList);

                    selClone.DisplayList.Clear();
                    foreach (Display d in selected.DisplayList)
                    {
                        Display newDisplay = d.Type.CreateNewDisplay();
                        selClone.DisplayList.Add(newDisplay);
                        foreach (Window w in d.WindowList)
                        {
                            newDisplay.WindowList.Add(w.SimpleClone());
                        }
                    }


                    if (rememberSelected && selected != null)
                    {
                        SelectSlideView(slideViews(selClone));
                    }
                }
            }
            PresentationController.Instance.SuppressLayoutChanging = false;
        }

        void Instance_OnSlideChangedExternally(int slideId)
        {
            Slide newSlide = PresentationController.Instance.Presentation.SlideList.Find(sld => sld.Id.Equals(slideId));
            SlideView newSlideView = slideViews(newSlide);
            SelectSlideView(newSlideView);
            if (OnCurrentSlideChanged != null)
                OnCurrentSlideChanged(newSlideView.Slide);
        }

        void Instance_OnSlideChangedExternally(Slide slide)
        {
            // обновляем и перещелкиваем только если изменен текущий слайд
            if (SelectedSlide.Id == slide.Id)
            {
                SlideView newSlideView = slideViews(slide);
                SelectSlideView(newSlideView);
                if (OnCurrentSlideChanged != null)
                    OnCurrentSlideChanged(newSlideView.Slide);
            }
        }

        void Instance_OnPresentationChangedExternally()
        {
            Model.HistoryManager.RecordComplete -= new EventHandler(HistoryManager_RecordComplete);

            SlideClibpoard.Clear();
            if (((Control)this.Viewer).IsHandleCreated)
                ((Control)this.Viewer).Invoke(new MethodInvoker(() => LoadPresentation(true)));
            else
                LoadPresentation(true);

            Model.HistoryManager.ClearHistory();
            Model.HistoryManager.RecordComplete += new EventHandler(HistoryManager_RecordComplete);
            PleaseUpdateMe(true);
        }

        void Instance_OnPresentationLockChanged(bool IsLocked)
        {
            ActivateTool(ToolDescriptor.SelectTool);

            if (OnCheckSelection != null)
                OnCheckSelection(this.SelectionList);
        }


        #endregion

        #region Selection routine

        /// <summary>
        /// Возвращает список выбранных сцен. Если ни одного сцены не выбрано - список будет пустым
        /// </summary>
        /// <returns>Перечисление сцен</returns>
        IEnumerable<Slide> GetSelectedSlides()
        {
            var selection = this.SelectionList.OfType<SlideView>();
            if (selection.Count() > 0)
            {
                return selection.Select(sv => sv.Slide);
            }
            return new List<Slide>();
        }

        /// <summary>
        /// Генерирует событие, оповещающее о выделеннии сцен
        /// </summary>
        public void CheckSelection()
        {
            if (!_inited) return;

            List<SlideLink> links = this.SelectionList.OfType<SlideLink>().ToList();
            IEnumerable<Slide> slides = GetSelectedSlides();

            if (slides.Count() > 0)
            {
                foreach (SlideLink link in links)
                    this.SelectionList.Remove(link);
            }

            CheckSelectionInt(slides);
        }

        public void CheckHitSelection()
        {
            if (!_inited) return;
            if (ActiveTool.Name != ToolDescriptor.SelectTool) return;

            var views = this.NodesHit.OfType<SlideView>();

            if (this.m_PlayerController != null)
            {
                //Slide slide = PresentationController.Instance.SelectedSlide;
                //if (slide != null && slide != _prevSlide && PresentationController.Instance.Presentation.SlideList.Count > 1 && (m_PlayerController.SlideHistory.Count == 0 || m_PlayerController.SlideHistory.Peek() != slide))
                //{
                //    _prevSlide = slide;
                //    m_PlayerController.SlideHistory.Push(slideViews(slide));
                //}
                //else
                //    m_PlayerController.SlideHistory.Clear();
            }

            if (views.Count() > 0)
                CheckSelectionInt(views.Select(sw => sw.Slide));
            else
                CheckSelection();
        }


        void CheckSelectionInt(IEnumerable<Slide> slides)
        {
            if (!_inited) return;
            //check if Slides must be pre-chached
            if (slides.Count() > 0)
            {
                Slide CurrentSlide = slides.LastOrDefault();
                if (CurrentSlide != null)
                {
                    PresentationController.Instance.LoadSlide(CurrentSlide);
                    PresentationController.Instance.CanSelectNextSlide = this.CanSelectNextSlide(CurrentSlide);
                    PresentationController.Instance.CanSelectPrevSlide = this.CanSelectPrevSlide(CurrentSlide);
                }
            }
            else
            {
                PresentationController.Instance.CanSelectNextSlide = false;
                PresentationController.Instance.CanSelectPrevSlide = false;
            }

            if (OnCheckSelection != null)
                OnCheckSelection(this.SelectionList);

            PresentationController.Instance.SendSelectionChanged(slides);
            (Viewer as Control).Invalidate();
        }


        /// <summary>
        /// True, если может быть выбрана пред. сцена
        /// </summary>
        public bool CanSelectPrevSlide(Slide ToSlide)
        {
            if (m_PlayerController != null && m_PlayerController.CanPlay)
                return m_PlayerController.CanGoPrev;
            if (ToSlide != null)
            {
                var slideView = slideViews(ToSlide);
                var links = slideView.GetIncomingSlideLinks();
                if (links.Count > 0)
                {
                    var defLinks = links.Where(l => l.IsDefault);
                    if (defLinks.Count() == 1)
                    {
                        return true;
                    }
                    else
                    {
                        if (links.Count == 1)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// true, если можно выбрать последнюю по МСУ сцену
        /// </summary>
        public bool CanSelectLastSlide()
        {
            if (Model.Nodes.Count > 0 && StartSlide != null)
            {
                Stack<SlideView> way = new Stack<SlideView>();
                SlideView slideView = StartSlide;
                List<SlideLink> links = slideView.GetOutgoingLinks();
                while (links.Count > 0)
                {
                    var defLinks = links.Where(l => l.IsDefault);
                    if (defLinks.Count() > 0)
                    {
                        slideView = defLinks.First().ToSlideView;
                        if (slideView != null)
                        {
                            links = slideView.GetOutgoingLinks();
                            if (way.Contains(slideView))
                            {
                                return false;
                            }
                        }
                        else
                            return false;

                        way.Push(slideView);
                    }
                    else
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// True, если может быть выбрана след.сцена
        /// </summary>
        public bool CanSelectNextSlide(Slide FromSlide)
        {
            if (m_PlayerController != null && m_PlayerController.CanPlay)
                return m_PlayerController.CanGoNext;

            if (FromSlide != null)
            {
                return slideViews(FromSlide).GetOutgoingLinks().Count > 0;
            }

            return false;
        }

        /// <summary>
        /// Выбирает предыдущую сцену
        /// </summary>
        public void GoPrevSlide()
        {
            Slide prevSlide = null;

            if (this.m_PlayerController != null && this.m_PlayerController.CanPlay)
            {
                int slideId;
                if (!m_PlayerController.GoToPrevSlide(PresentationController.Instance.Presentation, out slideId))
                    return;

                Slide newSlide = PresentationController.Instance.Presentation.SlideList.Find(sld => sld.Id.Equals(slideId));
                SlideView newSlideView = slideViews(newSlide);
                PresentationController.Instance.SendPlaySelectionChanged(newSlide);
                SelectSlideView(newSlideView);
                if (OnCurrentSlideChanged != null)
                    OnCurrentSlideChanged(newSlideView.Slide);
                return;
            }

            Slide slide = GetSelectedSlide();
            if (slide != null)
            {
                //get incoming links to slide, find first default
                var slideView = slideViews(slide);
                var links = slideView.GetIncomingSlideLinks();
                if (links.Count > 0)
                {
                    var defLinks = links.Where(l => l.IsDefault);
                    if (defLinks.Count() == 1)
                        slideView = defLinks.First().FromSlideView;
                    else
                        if (links.Count == 1)
                        {
                            slideView = links.First().FromSlideView;
                        }
                    SelectSlideView(slideView);
                    prevSlide = slideView.Slide;
                }
            }

            if (this.m_PlayerController != null)
                if (this.OnCurrentSlideChanged != null)
                    this.OnCurrentSlideChanged(prevSlide);
        }

        /// <summary>
        /// Возвращает выделенную сцену
        /// </summary>
        Slide GetSelectedSlide()
        {
            var selection = this.SelectionList.OfType<SlideView>();
            if (selection.Count() > 0)
            {
                return selection.First().Slide;
            }
            return null;
        }

        /// <summary>
        /// Выбирает следующую сцену по умолчанию
        /// </summary>
        public void GoNextSlide()
        {
            Slide slide = GetSelectedSlide();
            Slide nextSlide = null;
            if (m_PlayerController != null && m_PlayerController.CanPlay && PlayerController.Instance.CurrentPlayingSlide != null)
                slide = PlayerController.Instance.CurrentPlayingSlide;


            if (slide != null)
            {
                //find default next link
                List<SlideLink> links = slideViews(slide).GetOutgoingLinks();
                if (links.Count > 0)
                {
                    var defLinks = links.Where(l => l.IsDefault);
                    if (defLinks.Count() > 0)
                    {
                        var slideView = defLinks.First().ToSlideView;
                        SelectSlideView(slideView);
                        nextSlide = slideView.Slide;
                    }
                }
                CheckSelectionInt(new[] { nextSlide });
            }

            if (m_PlayerController != null && m_PlayerController.CanPlay)
            {
                if (nextSlide != null)
                {
                    if (m_PlayerController.PlaySlide(PresentationController.Instance.Presentation, nextSlide))
                    {
                        PresentationController.Instance.SendPlaySelectionChanged(nextSlide);
                        if (OnCurrentSlideChanged != null)
                            OnCurrentSlideChanged(nextSlide);
                    }
                }
            }
        }

        /// <summary>
        /// Выбирает следующую сцену, следуя по заданной ссылке
        /// </summary>
        /// <param name="link">Ссылка для перехода</param>
        public void GoNextSlide(SlideLink link)
        {
            if (link != null)
            {
                //find next slide by link
                var slideView = link.ToSlideView;
                SelectSlideView(slideView);
            }
        }

        public void SelectSlide(Slide slide)
        {
            SelectSlideView(slideViews(slide));
        }

        /// <summary>
        /// Устанавливает в состояние "Выбрана" заданную сцену
        /// </summary>
        /// <param name="slideView">Отображение сцены, которую нужно выбрать</param>
        public void SelectSlideView(SlideView slideView)
        {
            if (slideView != null)
            {
                SelectionList.Clear();
                SelectionList.Add(slideView);

                EnsureVisible(new PointF(slideView.BoundingRectangle.Left, slideView.BoundingRectangle.Bottom));
                CheckSelection();
            }
        }

        public void SelectSlideViewFromControl(SlideView slideView)
        {
            if (slideView != null && m_PlayerController != null && m_PlayerController.SlideHistory.Peek() != slideView.Slide)
            {
                if (m_PlayerController.PlaySlide(PresentationController.Instance.Presentation, slideView.Slide))
                    PresentationController.Instance.SendPlaySelectionChanged(slideView.Slide);
            }
            SelectSlideView(slideView);
        }


        /// <summary>
        /// Выделяет последний сцена сценария
        /// </summary>
        public void GoLastSlide()
        {
            SlideView slide = StartSlide;
            if (slide != null)
            {
                Stack<SlideView> way = new Stack<SlideView>();
                List<SlideLink> links = slide.GetOutgoingLinks();
                while (links.Count > 0)
                {
                    var defLinks = links.Where(l => l.IsDefault);
                    if (defLinks.Count() > 0)
                    {
                        slide = defLinks.First().ToSlideView;
                        links = slide.GetOutgoingLinks();
                        if (way.Contains(slide))
                        {
                            slide = way.Pop();
                            break;
                        }
                        way.Push(slide);
                    }
                    else
                        break;
                }
            }

            SelectSlideView(slide);
        }


        /// <summary>
        /// Выделяет стартовую сцену сценария
        /// </summary>
        public void GoHome(bool selectSlide)
        {
            if (StartSlide != null)
            {
                PointF pos = StartSlide.BoundingRectangle.Location;
                pos.X -= SlideView.MaxWidth;
                pos.Y -= SlideView.MaxHeight;
                this.View.Origin = pos;

                if (selectSlide)
                {
                    SelectSlideView(StartSlide);
                }
            }
        }

        public void GoToSlide(int slideId)
        {
            SlideView view = slideViews().Where(x => x.Slide.Id == slideId).FirstOrDefault();
            if (view != null)
            {
                PointF pos = view.BoundingRectangle.Location;
                pos.X -= SlideView.MaxWidth;
                pos.Y -= SlideView.MaxHeight;
                this.View.Origin = pos;
                SelectSlideView(view);
            }
        }

        internal bool IsSlideSelected(Slide slide)
        {
            return GetSelectedSlides().Contains(slide);
        }

        #endregion

        #region Commands
        /// <summary>
        /// Удаление выделенных элементов (Действует на связи и сцены)
        /// </summary>
        public void RemoveSelected()
        {
            if (SelectionList.Count > 0)
            {
                List<SlideView> selectedSlides = SelectionList.OfType<SlideView>().ToList();
                List<SlideLink> selectedLinks = SelectionList.OfType<SlideLink>().ToList();

                //удалить из выделения начальную сцену
                if (selectedSlides.Contains(StartSlide) & (Model.Nodes.OfType<SlideView>().Count() - SelectionList.OfType<SlideView>().Count() == 0))
                {
                    selectedSlides.Remove(StartSlide);
                    SelectionList.Remove(StartSlide);
                }

                //удалить из выделения заблокированные слайды
                foreach (SlideView view in selectedSlides.ToList())
                {
                    if (view.IsLocked)
                    {
                        selectedSlides.Remove(view);
                        SelectionList.Remove(view);
                    }
                }

                if (selectedSlides.Count > 0 || selectedLinks.Count > 0)
                {
                    if (MessageBoxExt.Show("Удалить выделенные объекты?", Properties.Resources.Confirmation,
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }) == DialogResult.OK)
                    {
                        bool updateStartSlide = false;
                        this.Model.HistoryManager.StartAtomicAction(CommandDescr.RemoveSelectedDescr);


                        selectedSlides.ForEach(s => SelectionList.AddRange(s.GetAllLinks()));
                        foreach (Node n in SelectionList)
                        {
                            n.EditStyle.AllowDelete = true;
                        }

                        List<SlideView> affectedSlides = new List<SlideView>();
                        selectedLinks.ForEach(l =>
                        {
                            if (!selectedSlides.Contains(l.FromSlideView) && !affectedSlides.Contains(l.FromSlideView))
                                affectedSlides.Add(l.FromSlideView);
                        });


                        selectedSlides.ForEach(s =>
                            {
                                if (SlideClibpoard != null)
                                {
                                    if (SlideClibpoard.Contains(s))
                                        SlideClibpoard.Remove(s);
                                }

                                s.GetIncomingSlideLinks().ForEach(l =>
                                    {
                                        if (!selectedSlides.Contains(l.FromSlideView) && !affectedSlides.Contains(l.FromSlideView))
                                            affectedSlides.Add(l.FromSlideView);
                                    });
                            });

                        if (selectedSlides.Contains(StartSlide))
                        {
                            updateStartSlide = true;
                            Model.HistoryManager.RecordPropertyChanged(this, String.Empty, "m_startSlideId");
                            StartSlide = Model.Nodes.OfType<SlideView>().Except(selectedSlides).First();
                        }

                        this.Delete();

                        affectedSlides.ForEach(s => RefreshDefaultLinkForSlide(s));
                        this.Model.HistoryManager.EndAtomicAction();

                        if (updateStartSlide)
                            UpdateStartSlide();

                        RefreshDefaultSlidePath(true);

                        if (selectedSlides.Count > 0)
                            GoHome(true);
                    }
                }
            }
        }

        private void UpdateStartSlide()
        {
            Model.HistoryManager.Pause();
            SlideView mStartSlide = StartSlide;
            foreach (var slide in Model.Nodes.OfType<SlideView>())
                slide.IsStartSlide = slide == mStartSlide;

            Model.HistoryManager.Resume();
        }


        /// <summary>
        /// Изменяет свойства сцены (Форма "Свойства")
        /// </summary>
        /// <param name="CurrentSlideView">Отображение сцены для изменения</param>
        /// <param name="Slide">Содержит требуемые изменения</param>
        /// <param name="NewDefLink">Новая ссылка по умолчанию</param>
        /// <param name="OldDefLink">Старая ссылка по умолчанию</param>
        /// <param name="IsStartup">True, если сцена - стартовая для сценария</param>
        internal void ChangeSlideData(SlideView CurrentSlideView, Slide Slide, SlideLink NewDefLink, bool IsStartup)
        {
            Model.HistoryManager.StartAtomicAction(CommandDescr.EditSlideDescr);
            Slide.Modified = DateTime.Now;
            string authorName = identity.User.FullName;
            if (String.IsNullOrEmpty(authorName))
                authorName = identity.User.Name;
            Slide.Author = authorName;
            Model.HistoryManager.RecordPropertyChanged(CurrentSlideView, String.Empty, "Slide");
            Slide.CopyTo(CurrentSlideView.Slide);

            if (NewDefLink != null)
            {
                RefreshDefaultLinkForSlide(CurrentSlideView, NewDefLink);
                foreach (var link in CurrentSlideView.GetOutgoingLinks().Except(new[] { NewDefLink }))
                {
                    Model.HistoryManager.RecordPropertyChanged(link, String.Empty, "IsDefault");
                    link.IsDefault = false;
                }
            }

            bool updateStartup = false;
            if (IsStartup)
            {
                Model.HistoryManager.RecordPropertyChanged(this, String.Empty, "m_startSlideId");
                StartSlide = CurrentSlideView;
                updateStartup = true;
            }
            else
            {
                if (StartSlide.Slide.Id == CurrentSlideView.Slide.Id && SlideCount > 1)
                {
                    Model.HistoryManager.RecordPropertyChanged(this, String.Empty, "m_startSlideId");
                    StartSlide = Model.Nodes.OfType<SlideView>().Except(new[] { CurrentSlideView }).First();
                    updateStartup = true;
                }
            }

            RefreshDefaultSlidePath(false);
            Model.HistoryManager.EndAtomicAction();

            if (updateStartup) UpdateStartSlide();
        }


        #endregion

        #region handlers
        void Instance_OnSlideLockChanged(Slide slide, bool IsLocked, LockingInfo info)
        {
            bool lockForShow = info != null && info.RequireLock == RequireLock.ForShow;
            if (slideViews(slide) != null)
            {
                if ((Viewer as Control).IsHandleCreated)
                {
                    ((Control)Viewer).Invoke(new MethodInvoker(() =>
                    {
                        if (IsLocked)

                            slideViews(slide).Lock(!lockForShow);
                        else
                        {
                            slideViews(slide).Unlock();
                        }
                    }));
                }
                else
                {
                    if (IsLocked)
                        slideViews(slide).Lock(!lockForShow);
                    else
                    {
                        slideViews(slide).Unlock();
                    }
                }
            }
        }

        void HistoryManager_RecordComplete(object sender, EventArgs e)
        {
            string[] descr;
            Model.HistoryManager.GetUndoDescriptions(1, out descr);
            if (descr != null)
            {
                UndoService.Instance.PushAction(new ControllerHistoryCommand(descr.Length > 0 ? descr[0] : String.Empty, this));
                PresentationController.Instance.PresentationChanged = true;
            }
        }

        protected override void Document_NodeCollectionChanged(CollectionExEventArgs evtArgs)
        {
            base.Document_NodeCollectionChanged(evtArgs);
            if (_inited)
            {
                if (evtArgs.ChangeType != CollectionExChangeType.Set)
                {
                    CheckSelection();
                }
            }
        }

        Slide[] Instance_OnGetCreatedSlides()
        {
            return GetCreatedSlides();
        }

        Slide[] Instance_OnGetAllSlides()
        {
            return GetAllSlides();
        }

        void Instance_OnUnlockAllSlides()
        {
            UnlockAllSlides();
        }

        //navigation
        public override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled = new[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Delete }.Contains(e.KeyCode);

            if (SelectedSlide != null)
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (CanSelectPrevSlide(SelectedSlide))
                        GoPrevSlide();
                }
                else
                    if (e.KeyCode == Keys.Right)
                    {
                        if (CanSelectNextSlide(SelectedSlide))
                            GoNextSlide();
                    }
            }

            if (!e.Handled)
                base.OnKeyDown(e);
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            e.Handled = new[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Delete }.Contains(e.KeyCode);

            if (!e.Handled)
                base.OnKeyUp(e);
        }

        #endregion

        #region Slides and SlideViews routine

        public List<TechnicalServices.Persistence.SystemPersistence.Configuration.Label> GetAllLabels()
        {
            var used = from s in slideViews() where s.Slide.LabelId != -1 && s.Slide != this.SelectedSlide select s.Slide.LabelId;
            var list = DesignerClient.Instance.ClientConfiguration.LabelStorageAdapter.GetLabelStorage().ToList();
            list.RemoveAll(x => used.Contains(x.Id));
            list.Insert(0, new TechnicalServices.Persistence.SystemPersistence.Configuration.Label() { Id = -1, Name = " ", IsSystem = false });
            return list;
        }

        public bool IsLabelUnique(int labelID, SlideView exceptOne)
        {
            foreach (SlideView sw in slideViews())
            {
                if (sw == exceptOne)
                    continue;

                if (sw.Slide.LabelId == labelID && labelID != -1)
                    return false;
            }

            return true;
        }

        string GetNextSlideName()
        {
            string name;
            int id = GetCurrentSlideId();

            while (true)
            {
                name = String.Format("{0} {1}", SlideName, id);
                if (IsSlideUniqueName(name, String.Empty)) break;
                id = GetNextSlideId();
            }

            return name;
        }


        int GetNextSlideId()
        {
            return SlideIdenty.NextID;
        }

        int GetCurrentSlideId()
        {
            return SlideIdenty.CurrentID;
        }

        public PointF GetNextSlideViewPos()
        {
            PointF pos;
            var slides = Model.Nodes.OfType<SlideView>();
            if (slides.Count() > 0)
            {
                pos = slides.Last().BoundingRectangle.Location;
                pos.X += SlideView.MaxWidth + SlideView.Margin;
                if (Model.LogicalSize.Width - SlideView.MaxWidth < pos.X)
                {
                    pos.X = 0;
                    pos.Y += SlideView.MaxHeight + SlideView.Margin;

                    if (Model.LogicalSize.Height - SlideView.MaxHeight < pos.Y)
                    {
                        pos.Y = 0;
                    }
                }
            }
            else
                pos = new PointF(SlideView.Margin, SlideView.Margin);

            return pos;
        }

        PointF EnsureVisible(PointF pos)
        {
            this.View.Origin = new PointF(pos.X - SlideView.MaxWidth * 2, pos.Y - SlideView.MaxHeight * 2);
            return pos;
        }

        /// <summary>
        /// Создание сцены рядом с последней созданной
        /// </summary>
        /// <returns>Созданный сцена</returns>
        public Slide CreateSlide()
        {
            PointF pos = GetNextSlideViewPos();

            //ensure child visible
            if (Model.Nodes.Count > 0)
                EnsureVisible(pos);

            return CreateSlide(pos);
        }

        /// <summary>
        /// Создает сцену в указанной позиции
        /// </summary>
        /// <param name="pos">Позиция сцены</param>
        /// <returns>Созданная сцена</returns>
        public Slide CreateSlide(PointF pos)
        {
            return AddSlide(new Slide { LabelId = -1, Author = Author, Name = GetNextSlideName() }, pos);
        }

        /// <summary>
        /// добавляет новую сцену в вычисляемой позиции
        /// </summary>
        /// <param name="slide"></param>
        /// <returns></returns>
        public Slide AddSlide(Slide slide)
        {
            return AddSlide(slide, GetNextSlideViewPos());
        }

        /// <summary>
        /// добавляет новую сцену в указанной позиции
        /// </summary>
        /// <param name="slide"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Slide AddSlide(Slide slide, PointF pos)
        {
            if (pos.X < 0) pos.X = 0;
            if (pos.Y < 0) pos.Y = 0;

            if (slideViews().Count() < MaxSlideCount)
            {
                UserIdentity ui = (Thread.CurrentPrincipal as UserIdentity);

                /*
                string authorName = Thread.CurrentPrincipal.Identity.Name;
                if (ui != null)
                {
                    authorName = ui.User.FullName;
                    if (String.IsNullOrEmpty(authorName.Trim()))
                        authorName = ui.User.Name;
                }
                */

                string authorName = Author;
                slide.Id = GetNextSlideId();
                slide.Modified = DateTime.Now;
                slide.State = SlideState.New;
                slide.Cached = true; //!!!!!!!!!

                //slide.Author = authorName;

                SlideView view = CreateSlideView(slide, pos);

                if (StartSlide == null)
                {
                    StartSlide = view;
                    StartSlide.IsStartSlide = true;
                }

                return slide;
            }
            else
            {
                MessageBoxExt.Show("Превышено ограничение на количество сцен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        /// <summary>
        /// Создает отображение сцены в указанной позиции
        /// </summary>
        /// <param name="slide">сцена</param>
        /// <param name="pos">Позиция</param>
        /// <returns>Отображение сцены</returns>
        public SlideView CreateSlideView(Slide slide, PointF pos)
        {
            this.SelectionList.Clear();

            //calc estimated document width
            float width = pos.X + SlideView.MaxWidth + SlideView.Margin;
            if (width > this.Model.DocumentSize.Width)
                this.Model.DocumentSize.Width = width;

            SlideView child = new SlideView(slide, pos);
            this.Model.AppendChild(child);

            return child;
        }


        /// <summary>
        /// Выбирает для сцены исходящий переход по умолчанию
        /// </summary>
        /// <param name="slide">сцена</param>
        public void RefreshDefaultLinkForSlide(SlideView view)
        {
            if (view == null) return;

            List<SlideLink> links = view.GetOutgoingLinks();
            if (links.Count > 0)
            {
                SlideLink link;
                List<SlideLink> lnks = links.Where(lnk => lnk.IsDefault).ToList();
                if (lnks.Count > 0)
                    link = lnks[0];
                else
                    link = links.First();
                RefreshDefaultLinkForSlide(view, link);
            }
        }

        public void RefreshDefaultLinkForSlide(SlideView view, SlideLink link)
        {
            List<SlideLink> links = view.GetOutgoingLinks();
            if (links.Count > 0)
            {
                Model.HistoryManager.RecordPropertyChanged(link, String.Empty, "IsDefault");
                link.IsDefault = true;
            }
        }

        public List<SlideView> GetLinksFromCurrent()
        {
            if (GetSelectedSlide() != null)
            {
                return slideViews(GetSelectedSlide()).GetOutgoingSlideViews();
            }

            return new List<SlideView>();
        }

        public bool IsSelectedSlideLocked()
        {
            return slideViews(GetSelectedSlide()).IsLocked;
        }

        public bool IsSlideUniqueName(string name, string exceptOne)
        {
            return !slideViews().Any(s => s.SlideName.ToUpper() == name.ToUpper() && s.SlideName.ToUpper() != exceptOne.ToUpper()); ;
        }

        #endregion

        #region SlideLink routine
        /// <summary>
        /// Called, when user creates link via "link tool"
        /// </summary>
        public bool CanCreateLink(SlideView slide1, SlideView slide2)
        {
            bool exists = false;
            exists = slide1.GetAllLinks().Any(l => l.ToSlideView == slide2 | l.FromSlideView == slide2);
            return !exists;
        }

        /// <summary>
        /// Создает связь по указанному списку сцен. Одновременно связь можно создать только между двумя слайдами
        /// </summary>
        /// <param name="slides">Перечисление сцен</param>
        /// <param name="IsDefault">True, если связь - по умолчанию</param>
        /// <returns>True, если связь создана</returns>
        public bool CreateLink(IEnumerable<SlideView> slides)
        {
            if (slides.Count() == 2)
            {
                SlideView FromSlideView = slides.Reverse().First();
                SlideView ToSlideView = slides.Reverse().Last();

                if (CanCreateLink(FromSlideView, ToSlideView))
                {
                    CreateLinkInternal(FromSlideView, ToSlideView);
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Создает связь по указанному списку сцен. Одновременно связь можно создать только между двумя слайдами
        /// </summary>
        /// <param name="slides"></param>
        /// <returns></returns>
        public bool CreateLink(IEnumerable<Slide> slides)
        {
            if (slides.Count() == 2)
            {
                SlideView fromSlideView = slideViews(slides.Reverse().First());
                SlideView toSlideView = slideViews(slides.Reverse().Last());
                if (fromSlideView == null || toSlideView == null) return false;
                return CreateLink(new[] { fromSlideView, toSlideView });
            }
            return false;
        }

        void CreateLinkInternal(SlideView FromSlideView, SlideView ToSlideView)
        {
            this.Model.BeginUpdate();
            SlideLink link = new SlideLink(PointF.Empty, PointF.Empty);
            FromSlideView.CentralPort.TryConnect(link.TailEndPoint);
            ToSlideView.CentralPort.TryConnect(link.HeadEndPoint);
            this.Model.AppendChild(link);
            link.IsDefault = FromSlideView.GetOutgoingLinks().Count == 1;
            this.Model.EndUpdate();

            RefreshDefaultSlidePath(true);
        }


        #endregion

        #region History routine

        /// <summary>
        /// Отмена последнего действия для модели и отображения
        /// </summary>
        public void Undo()
        {
            string[] descr;
            Model.HistoryManager.GetUndoDescriptions(10, out descr);


            if (Model.HistoryManager.CanUndo)
            {
                Model.HistoryManager.Undo();
                UpdateStartSlide();
                RefreshDefaultSlidePath(true);
                CheckSelection();
            }
        }


        /// <summary>
        /// Повтор отмененного действия для модели и отображения
        /// </summary>
        public void Redo()
        {
            if (Model.HistoryManager.CanRedo)
            {
                Model.HistoryManager.Redo();
                UpdateStartSlide();
                RefreshDefaultSlidePath(true);
                CheckSelection();
            }
        }


        /// <summary>
        /// Возвращает True, если внутренняя история контроллера может быть отменена на шаг назад
        /// </summary>
        public bool CanUndo()
        {
            return Model.HistoryManager.CanUndo && (PresentationController.Instance.PresentationLocked || DesignerClient.Instance.IsStandAlone);
        }

        /// <summary>
        /// Возвращает True, если последняя отмена внутренней истории контроллера может быть возвращена
        /// </summary>
        public bool CanRedo()
        {
            return Model.HistoryManager.CanRedo && (PresentationController.Instance.PresentationLocked || DesignerClient.Instance.IsStandAlone);
        }

        #endregion

        #region Default path routine
        /// <summary>
        /// Обновляет путь по умолчанию
        /// </summary>
        internal void RefreshDefaultSlidePath(bool pauseHistory)
        {
            if (pauseHistory)
                Model.HistoryManager.Pause();

            if (StartSlide != null)
            {
                List<SlideView> otherPath = new List<SlideView>();
                List<SlideView> defaultPath = new List<SlideView>();

                Model.BeginUpdate();

                SlideView view = StartSlide;
                while (view != null && !defaultPath.Contains(view))
                {
                    defaultPath.Add(view);
                    view = view.GetNextDefaultSlideView();
                }

                var allviews = Model.Nodes.OfType<SlideView>();

                otherPath.AddRange(allviews.Where(v => !defaultPath.Contains(v)));
                defaultPath.ForEach(v =>
                    {
                        if (!pauseHistory)
                            Model.HistoryManager.RecordPropertyChanged(v, String.Empty, "IsDefaultPathNode");
                        v.IsDefaultPathNode = true;
                    });
                otherPath.ForEach(v =>
                    {
                        if (!pauseHistory)
                            Model.HistoryManager.RecordPropertyChanged(v, String.Empty, "IsDefaultPathNode");
                        v.IsDefaultPathNode = false;
                    });

                Model.EndUpdate();
            }

            if (pauseHistory)
                Model.HistoryManager.Resume();
        }

        internal void MakeDefaultLinksFor(SlideView view)
        {
            Model.HistoryManager.StartAtomicAction(CommandDescr.UpdateDefLinkDescr);
            view.GetIncomingSlideLinks().ForEach(l =>
                {
                    Model.HistoryManager.RecordPropertyChanged(l, String.Empty, "IsDefault");
                    l.IsDefault = true;

                    //set other links undefault
                    l.FromSlideView.GetOutgoingLinks().ForEach(link =>
                    {
                        if (link != l)
                        {
                            Model.HistoryManager.RecordPropertyChanged(link, String.Empty, "IsDefault");
                            link.IsDefault = false;
                        }
                    });

                });

            Model.HistoryManager.EndAtomicAction();
            RefreshDefaultSlidePath(true);
        }

        #endregion

        #region Copy Paste

        List<SlideView> SlideClibpoard = new List<SlideView>();

        public override bool CanPaste
        {
            get
            {
                return SlideClibpoard != null && SlideClibpoard.Count > 0;
            }
        }

        public override void Copy()
        {
            List<SlideView> views = new List<SlideView>(SelectionList.OfType<SlideView>());
            SlideClibpoard = views;
        }

        public override void Paste()
        {
            PointF p = GetNextSlideViewPos();
            Paste(p);
        }

        public void Paste(PointF point)
        {
            if (!CanPaste) return;

            if (point.X < 0) point.X = 0;
            if (point.Y < 0) point.Y = 0;

            List<SlideLink> links = new List<SlideLink>();
            PointF lastPos = PointF.Empty;
            var slideEnum = SlideClibpoard.GetEnumerator();
            if (slideEnum.MoveNext())
            {
                PointF firstPoint = slideEnum.Current.GetPositionF();

                this.Model.HistoryManager.StartAtomicAction(CommandDescr.PasteElements);
                this.Model.BeginUpdate();

                Dictionary<SlideView, SlideView> mapping = new Dictionary<SlideView, SlideView>();
                Slide newSlide = CloneSlide(slideEnum.Current.Slide);
                newSlide.LabelId = -1;

                mapping.Add(slideEnum.Current, AddSlideView(newSlide, point));

                while (slideEnum.MoveNext())
                {
                    PointF pos = slideEnum.Current.GetPositionF();
                    pos.X = (pos.X - firstPoint.X) + point.X;
                    pos.Y = (pos.Y - firstPoint.Y) + point.Y;

                    Slide slide = CloneSlide(slideEnum.Current.Slide);
                    mapping.Add(slideEnum.Current, AddSlideView(slide, pos));

                    lastPos = pos;
                }

                //process the links
                foreach (SlideView view in SlideClibpoard)
                {
                    foreach (SlideLink linkView in view.GetOutgoingLinks())
                    {
                        if (SlideClibpoard.Contains(linkView.ToSlideView))
                        {
                            SlideView FromSlideView = mapping[view];
                            SlideView ToSlideView = mapping[linkView.ToSlideView];
                            SlideLink newLink = new SlideLink(Point.Empty, PointF.Empty);
                            FromSlideView.CentralPort.TryConnect(newLink.TailEndPoint);
                            ToSlideView.CentralPort.TryConnect(newLink.HeadEndPoint);

                            if (linkView.ToNode != null && linkView.FromNode != null)
                            {
                                newLink.IsDefault = linkView.IsDefault;
                                this.Model.AppendChild(newLink);
                            }
                            else
                            {
                                throw new ArgumentException("Некорректное поведение, собщите разработчику!", "linkView");
                            }
                        }
                    }
                }

                this.Model.EndUpdate();
                this.Model.HistoryManager.EndAtomicAction();
            }

            if (!lastPos.IsEmpty)
                EnsureVisible(lastPos);
        }

        SlideView AddSlideView(Slide slide, PointF pos)
        {
            SlideView child = new SlideView(slide, pos);
            this.Model.AppendChild(child);
            return child;
        }

        Slide CloneSlide(Slide from)
        {
            Slide result = (Slide)from.Clone();
            result.IsLocked = false;
            result.State = SlideState.New;
            result.Id = GetNextSlideId();
            result.Name = GetNextSlideName();

            // логика вся переехала в Slide.Clone
            //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1194
            //result.DisplayList.Clear();
            //foreach (Display d in from.DisplayList)
            //{
            //    Display newDisplay = d.Type.CreateNewDisplay();
            //    result.DisplayList.Add(newDisplay);
            //    foreach (Window w in d.WindowList)
            //    {
            //        newDisplay.WindowList.Add(w.SimpleClone());
            //    }
            //}

            //result.IsLocked = false;
            return result;
        }


        #endregion

        #region Save
        void Instance_OnSavePresentation()
        {
            SavePresentationChanges();
            Model.HistoryManager.ClearHistory();
        }

        Slide[] GetAllSlides()
        {
            return slideViews().Select(s => s.Slide).ToArray();
        }

        Slide[] GetCreatedSlides()
        {
            return slideViews().Where(s => s.Slide.State == SlideState.New).Select(s => s.Slide).ToArray();
        }

        public void SavePresentationChanges()
        {
            Presentation.Presentation m_Presentation = PresentationController.Instance.Presentation;
            m_Presentation.LinkDictionary.Clear();
            m_Presentation.SlideList.Clear();
            m_Presentation.SlidePositionList.Clear();

            foreach (SlideView view in Model.Nodes.OfType<SlideView>())
            {
                m_Presentation.SlideList.Add(view.Slide);
                SlideLinkList links = new SlideLinkList();
                foreach (SlideLink link in view.GetOutgoingLinks())
                {
                    Presentation.Link l = new Presentation.Link
                                              {
                                                  NextSlide = link.ToSlideView.Slide,
                                                  IsDefault = link.IsDefault
                                              };

                    if (!links.LinkList.Contains(l))
                        links.LinkList.Add(l);
                }

                int viewSlideId = view.Slide.Id;
                if (!m_Presentation.LinkDictionary.ContainsKey(viewSlideId))
                    m_Presentation.LinkDictionary.Add(viewSlideId, links);

            }


            foreach (var slide in slideViews())
            {
                Point p = slide.GetPosition();
                m_Presentation.SlidePositionList.Add(slide.Slide.Id, p);
            }

            m_Presentation.StartSlideId = m_startSlideId;
        }

        #endregion

        #region Locking/unlocking

        internal bool SaveNewSlides(string message)
        {
            Slide[] created = GetCreatedSlides();
            if (created.Length > 0)
            {
                if (MessageBoxExt.Show(message, "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    PresentationController.Instance.SavePresentation();
                }
                else
                    return false;
            }
            return true;
        }

        
        /// <summary>
        /// Блокирование текущей сцены
        /// </summary>
        internal void LockSlides()
        {
            if (!SaveNewSlides("Имеются несохраненные сцены.\r\nПеред блокировкой необходимо сохранить изменения.\r\nСохранить новые сцены?"))
                return;

            var slides = this.SelectionList.OfType<SlideView>();
            foreach (SlideView slideView in slides)
            {
                if (PresentationController.Instance.LockSlide(slideView.Slide))
                {
                    slideView.Slide.State = SlideState.Edit;
                    slideView.Lock();
                }
            }
        }

        /// <summary>
        /// Разблокирование текущей сцены
        /// </summary>
        public void UnlockSlides()
        {
            var slides = this.SelectionList.OfType<SlideView>();
            var locked = slides.Where(v => v.IsLocked).Select(v => v.Slide);
            if (PresentationController.Instance.SaveSlideChanges(locked.ToArray()))
            {
                foreach (SlideView slide in slides)
                {
                    //slide.Slide.Cached = false; //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1619
                    UnlockSlide(slide);
                }
            }
        }

        public void UnlockAllSlides()
        {
            var locked = slideViews().Where(w => w.IsLocked);
            if (PresentationController.Instance.SaveSlideChanges(locked.Select(w => w.Slide).ToArray()))
            {
                foreach (SlideView view in locked)
                {
                    view.Slide.Cached = false;
                    if (view.IsLocked)
                    {
                        UnlockSlide(view);
                    }
                }
            }
        }

        public bool IsAnySlidesLocked(IEnumerable<Slide> slides)
        {
            return slides.Any(s => s.IsLocked);
        }

        /// <summary>
        /// Есть ли на текущей сцене хоть один заблокированный слайд кроме данного.
        /// </summary>
        public bool IsAnySlidesLocked(Slide slide)
        {
            return Model.Nodes.OfType<SlideView>().Any(s => s.IsLocked && s.Slide != slide);
        }

        /// <summary>
        /// Есть ли на текущей сцене хоть один незаблокированный слайд кроме данного.
        /// </summary>
        public bool IsAnySlidesUnlocked(Slide slide)
        {
            return Model.Nodes.OfType<SlideView>().Any(s => !s.IsLocked && s.Slide != slide);
        }

        private void UnlockSlide(SlideView slideView)
        {
            if (PresentationController.Instance.UnlockSlide(slideView.Slide))
            {
                slideView.Slide.State = SlideState.Normal;
                slideView.Unlock();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            PresentationController.Instance.OnSlideLockChanged -= new SlideLockChanged(Instance_OnSlideLockChanged);
            PresentationController.Instance.OnUnlockAllSlides -= new Changed(Instance_OnUnlockAllSlides);
            PresentationController.Instance.OnGetAllSlides -= new GetSlides(Instance_OnGetAllSlides);
            PresentationController.Instance.OnGetCreatedSlides -= new GetSlides(Instance_OnGetCreatedSlides);
            PresentationController.Instance.OnSavePresentation -= new SavePresentation(Instance_OnSavePresentation);
            PresentationController.Instance.OnPresentationChangedExternally -= new PresentationDataChanged(Instance_OnPresentationChangedExternally);
            PresentationController.Instance.OnSlideChangedExternally -= new SlideChanged(Instance_OnSlideChangedExternally);
            PresentationController.Instance.OnPresentationLockChanged -= new PresentationLockChanged(Instance_OnPresentationLockChanged);
            DesignerClient.Instance.PresentationNotifier.OnLabelDeleted -= PresentationNotifier_OnLabelDeleted;
            ShowClient.Instance.OnGoToSlide -= Instance_OnSlideChangedExternally;
            _instance = null;
        }

        #endregion

        #region IPropertyContainer Members

        public string FullContainerName
        {
            get { return this.GetType().FullName; }
        }

        public object GetPropertyContainerByName(string strPropertyName)
        {
            return this;
        }

        #endregion

        #region Player

        public bool CanPrepare
        {
            get { return m_PlayerController != null && m_PlayerController.CanPrepare; }
        }

        public bool CanStart
        {
            get { return m_PlayerController != null && m_PlayerController.CanStart; }
        }

        public bool CanStop
        {
            get { return m_PlayerController != null && m_PlayerController.CanStop; }
        }

        public bool CanPlaySlide(Slide slide)
        {
            return m_PlayerController.CanPlay && !(slide == null || slide.Equals(m_PlayerController.CurrentPlayingSlide));
        }

        public bool CanPlay
        {
            get { return m_PlayerController != null && m_PlayerController.CanPlay; }
        }


        public void PreparePresentation()
        {
            if (m_PlayerController != null && this.SelectedSlide != null)
                m_PlayerController.Prepare(PresentationController.Instance.PresentationInfo);
        }

        public void StartPresentation()
        {
            if (m_PlayerController != null)
            {
                m_PlayerController.Start(PresentationController.Instance.PresentationInfo);
                if (this.SelectedSlide != PresentationController.Instance.SelectedSlide && PresentationController.Instance.SelectedSlide != null)
                {
                    this.SelectionList.Clear();
                    this.SelectSlideView(slideViews(PresentationController.Instance.SelectedSlide));
                }
                if (this.SelectedSlide != null)
                {
                    m_PlayerController.CurrentPlayingSlide = null;
                    //PresentationController.Instance.SendPlaySelectionChanged(this.SelectedSlide);
                }
                m_PlayerController.PlaySlide(PresentationController.Instance.Presentation, SelectedSlide);
            }
        }

        public void PausePresentation()
        {
            if (m_PlayerController != null)
            {
                m_PlayerController.Pause(PresentationController.Instance.Presentation);
            }
        }

        public void PlaySlide()
        {
            if (m_PlayerController != null && this.SelectedSlide != null)
            {
                if (m_PlayerController.PlaySlide(PresentationController.Instance.Presentation, this.SelectedSlide))
                    PresentationController.Instance.SendPlaySelectionChanged(this.SelectedSlide);
            }
        }

        public void EditSlide()
        {
            if (m_PlayerController != null && this.SelectedSlide != null)
                m_PlayerController.EditSlide(PresentationController.Instance.PresentationInfo, this.SelectedSlide);
        }

        public void StopPresentation()
        {
            if (m_PlayerController != null)
                m_PlayerController.Stop(PresentationController.Instance.PresentationInfo);
        }

        /// <summary>
        /// Назначить контроллер показа
        /// </summary>
        /// <param name="ctrl">Контроллер</param>
        public void AssignPlayerController(PlayerController ctrl)
        {
            m_PlayerController = ctrl;
        }
        #endregion

    }
}
