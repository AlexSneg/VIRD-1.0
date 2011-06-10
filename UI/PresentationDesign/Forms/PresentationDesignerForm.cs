using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Syncfusion.Runtime.Serialization;
using UI.ImportExport.ImportExportUI.Controllers;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Classes.View;
using UI.PresentationDesign.DesignUI.Forms;
using Syncfusion.Windows.Forms.Tools;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Threading;
using UI.PresentationDesign.DesignUI.Services;
using Domain.PresentationDesign.Client;
using TechnicalServices.Entity;
using Syncfusion.Windows.Forms;
using UI.PresentationDesign.DesignUI.Classes.Helpers;
using UI.PresentationDesign.DesignUI.Controllers;
using System.Diagnostics;
using UI.PresentationDesign.DesignUI.Helpers;
using System.IO;
using Action=Syncfusion.Windows.Forms.Tools.Action;

namespace UI.PresentationDesign.DesignUI
{
    public partial class PresentationDesignerForm : RibbonForm
    {
        #region fields
        string lockedByUser = "Сценарий заблокирован пользователем {0} {1}";

        Presentation m_Presentation;
        PresentationInfo m_PresentationInfo;
        const string Title = "ВИРД - Подготовка сценариев: {0}";
        bool isLocked;
        UserIdentity identity;
        Changed PresentationChanged;
        bool presentationRemoved = false;
        private int m_SlideToNavigate = -1;

        #endregion

        #region Props
        public bool StartedFromPlayer { get; set; }
        #endregion

        #region Constructors
        public PresentationDesignerForm()
        {
            InitializeComponent();
            layoutPreviewMenuButton.Visible = previewToolButton.Visible = !DesignerClient.Instance.IsStandAlone;
        }

        private void RefreshTitle()
        {
            this.Text = String.Format(Title, m_Presentation.Name);
        }

        public PresentationDesignerForm(PresentationInfo aPresentationInfo)
        {
            m_PresentationInfo = aPresentationInfo;
            m_Presentation = aPresentationInfo.CreatePresentationStub();
            PresentationController.CreatePresentationController();
            PresentationController.Instance.PresentationChanged = false;
            PresentationChanged = new Changed(() =>
            {
                this.saveMenuButton.Enabled = PresentationController.Instance.PresentationChanged;
                this.savePresentationToolButton.Enabled = PresentationController.Instance.PresentationChanged;
                this.ChangedStatus.Visible = PresentationController.Instance.PresentationChanged;
                this.ChangedStatus.Text = PresentationController.Instance.ChangedTextStatus;
            });
            PresentationController.Instance.OnChanged += PresentationChanged;
            PresentationController.Instance.OnPresentationLockChanged += new PresentationLockChanged(Instance_OnPresentationLockChanged);
            PresentationController.Instance.OnPresentationRemoved += new Changed(Instance_OnPresentationRemoved);
            PresentationController.Instance.OnPresentationLockedExternally += new PresentationLockedExternally(Instance_OnPresentationLockedExternally);
            PresentationController.Instance.OnPresentationUnlockedExternally += new PresentationUnlockedExternally(Instance_OnPresentationUnlockedExternally);
            PresentationController.Instance.OnSlideSelectionChanged += new SlideSelectionChanged(Instance_OnSlideSelectionChanged);
            PresentationController.Instance.OnOtherUserLockForShow += new SlideChanged(Instance_OnOtherUserLockForShow);
            UndoService.CreateUndoService();
            PresentationController.Instance.AssignPresentation(m_Presentation, m_PresentationInfo);
            InitializeComponent();

            RefreshTitle();
            this.statusStrip.ContextMenuStrip = null;
            this.ChangedStatus.Visible = false;

            this.WindowState = FormWindowState.Maximized;
            UndoService.Instance.OnHistoryChanged += new HistoryChanged(OnHistoryChanged);

            identity = Thread.CurrentPrincipal as UserIdentity;
            slideDiagram.SwitchPlayerMode(false);

            toolStripEx2.Enabled = false;
            PresentationController.Instance.RefreshLockingInfo();


            LockingInfo li = ((PresentationInfoExt)m_PresentationInfo).LockingInfo;
            if (li != null)
            {
                string info = String.Format(lockedByUser, 
                    string.IsNullOrEmpty(li.UserIdentity.User.FullName) ? li.UserIdentity.User.Name : li.UserIdentity.User.FullName,
                    li.RequireLock == RequireLock.ForShow ? "для показа" :  "для редактирования");
                this.LockingStatus.Visible = true;
                this.LockingStatus.Text = info;
                if (li.RequireLock == RequireLock.ForShow)
                {
                    layoutPreviewMenuButton.Enabled = false;
                    previewToolButton.Enabled = false;
                }
            }

            if (layoutPreviewMenuButton.Enabled || previewToolButton.Enabled)
            {
                layoutPreviewMenuButton.Enabled = previewToolButton.Enabled = !LayoutController.Instance.IsShownByPlayer();
            }

            SlideGraphController.Instance.OnSlideHover += new EventHandler<SlideEventArgs>(Instance_OnSlideHover);

            updateMenuButton.Enabled = !DesignerClient.Instance.IsStandAlone;
            refreshDisplayMenuButton.Enabled = !DesignerClient.Instance.IsStandAlone;
            refreshSlidesMenuButton.Enabled = !DesignerClient.Instance.IsStandAlone;
            commonSourcesRefreshMenuButton.Enabled = !DesignerClient.Instance.IsStandAlone;
            equipmentRefreshMenuButton.Enabled = !DesignerClient.Instance.IsStandAlone;
            toXmlMenuButton.Enabled = !DesignerClient.Instance.IsStandAlone;

            LayoutController.Instance.OnShownStatusChanged += new Action<bool>(Instance_OnShownStatusChanged);
            layoutPreviewMenuButton.Visible = previewToolButton.Visible = !DesignerClient.Instance.IsStandAlone;
        }

        void Instance_OnShownStatusChanged(bool isShownByPlayer)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<bool>(Instance_OnShownStatusChanged), isShownByPlayer);
            }
            else
            {
                layoutPreviewMenuButton.Enabled = previewToolButton.Enabled = !isShownByPlayer;
            }
        }

        private void Instance_OnOtherUserLockForShow(Slide slide)
        {
            this.Invoke(new MethodInvoker(() => 
            {
                MessageBoxExt.Show(string.Format("Ваши изменения на сцене {0} отменены, т.к. сцена запущена для показа", slide.Name), "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }));
        }

        void Instance_OnSlideSelectionChanged(IEnumerable<Slide> NewSelection)
        {
            if (layoutPreviewMenuButton != null)
            {
                layoutPreviewMenuButton.Enabled = previewToolButton.Enabled = (NewSelection == null || NewSelection.Count() == 0 ? false : true)
                    && !LayoutController.Instance.IsShownByPlayer();
            }
        }

        void Instance_OnSlideHover(object sender, SlideEventArgs e)
        {
            if (e.Slide != null)
            {
                LockingInfo li = PresentationController.Instance.GetSlideLockingInfo(e.Slide);
                if (li != null)
                {
                    SlideLockingStatus.Text = PresentationStatusInfo.GetSlideLockingInfoDescr(li);
                    SlideLockingStatus.Visible = true;
                    return;
                }
            }
            else
                SlideLockingStatus.Visible = false;
        }

        private bool _userAttracted = false;
        void Instance_OnPresentationLockedExternally(string UserName, RequireLock lockType)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                string info = String.Format(lockedByUser, UserName, 
                    lockType == RequireLock.ForShow ? "для показа" : "для редактирования");
                if (!_userAttracted)
                {
                    _userAttracted = true;
                    MessageBoxExt.Show(info, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _userAttracted = false;
                }
                this.LockingStatus.Visible = true;
                this.LockingStatus.Text = info;
                if (lockType == RequireLock.ForShow)
                {
                    layoutPreviewMenuButton.Enabled = previewToolButton.Enabled = false;
                }
            }));
        }

        void Instance_OnPresentationUnlockedExternally(string UserName)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                this.LockingStatus.Visible = false;
                this.LockingStatus.Text = String.Empty;
                layoutPreviewMenuButton.Enabled = previewToolButton.Enabled = !LayoutController.Instance.IsShownByPlayer();
            }));
        }


        void Instance_OnPresentationRemoved()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                MessageBoxExt.Show("Сценарий был удален другим пользователем и будет закрыт", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.presentationRemoved = true;
                this.Close();
            }));
        }


        void Instance_OnPresentationLockChanged(bool IsLocked)
        {
            if (!IsLocked)
            {
                this.LockingStatus.Visible = false;
                this.LockingStatus.Text = String.Empty;
                DisableEditing(); //пользователь разблокировал сценарий, запретим редактирование
            }
            else
            {
                if (!DesignerClient.Instance.IsStandAlone)
                {
                    string info = String.Format(lockedByUser, 
                        String.IsNullOrEmpty(identity.User.FullName) ? identity.Name : identity.User.FullName,
                        "для редактирования"); //судя по всему это событие приходит когда сам в дизайнере лочишь, поэтому прописал константой "для редактирования"
                    this.LockingStatus.Visible = true;
                    this.LockingStatus.Text = info;
                }
                EnableEditing();
            }
        }


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            toolStripEx2.Enabled = true;
        }

        #endregion

        #region Initialization
        public void Init(bool editMode)
        {
            if (m_Presentation == null)
            {
                throw new NullReferenceException("m_Presentation");
            }

            if (DesignerClient.Instance.IsStandAlone)
                SetEditMode(true);
            else
                SetEditMode(editMode);
        }

        public void NavigateToSlide(int slideId)
        {
            m_SlideToNavigate = slideId;
        }

        private void PresentationDesignerForm_Load(object sender, EventArgs e)
        {
            AppStateSerializer serializer = new AppStateSerializer(SerializeMode.XMLFile, Application.StartupPath + "\\dockstate.xml");
            this.dockingManager.LoadDockState(serializer);

            if (m_SlideToNavigate != -1)
            {
                this.slideDiagram.GoToSlide(m_SlideToNavigate);
            }
            lockMenuButton.Enabled = !StartedFromPlayer;
            lockSlideMenuButton.Enabled = !StartedFromPlayer;
            if (StartedFromPlayer)
            {
                SlideGraphController.Instance.LockSlides();
            }
            if (JustCreatedPresentation) slideDiagram.LockSlide();
        }

        /// <summary>
        /// Признак только что созданной презентации.
        /// </summary>
        public bool JustCreatedPresentation = false;
        #endregion

        #region Locking and edit mode

        private void EnableEditing()
        {
            SetEditMode(true);
        }

        void SetEditMode(bool mode)
        {
            if (this.IsHandleCreated)
                this.Invoke(new MethodInvoker(() =>
                {
                    UpdateMode(mode);
                }));
            else
                UpdateMode(mode);
        }

        void UpdateMode(bool mode)
        {
            lockMenuButton.Enabled = !DesignerClient.Instance.IsStandAlone && !StartedFromPlayer;
            lockToolButton.Enabled = !DesignerClient.Instance.IsStandAlone && !StartedFromPlayer; ;

            lockMenuButton.Checked = mode;
            //PresentationController.Instance.PresentationLocked = mode;

            ToolTipInfo info = superToolTip1.GetToolTip(lockToolButton);
            info.Body.Text = !mode ? "Установить блокировку сценария" : "Снять блокировку сценария";

            lockToolButton.Checked = mode;

            saveMenuButton.Enabled = false;
            savePresentationToolButton.Enabled = false;
            isLocked = mode;
        }

        private void DisableEditing()
        {
            SetEditMode(false);
        }

        /// <summary>
        /// Сохранение сценария
        /// </summary>
        bool SavePresentation()
        {
            return PresentationController.Instance.SavePresentation();
        }

        private void UnlockPresentation(bool p)
        {
            if (p)
                UnlockPresentation();
            else
                DesignerClient.Instance.PresentationWorker.ReleaseLockForPresentation(m_Presentation.UniqueName);
        }

        private bool UnlockPresentation()
        {
            string d = PresentationStatusInfo.GetPresentationStatusDescr(m_Presentation.UniqueName, m_Presentation.Name);

            if (DesignerClient.Instance.PresentationWorker.ReleaseLockForPresentation(m_Presentation.UniqueName))
            {
                PresentationController.Instance.PresentationLocked = false;
                return true;
            }
            else
            {
                MessageBoxExt.Show(String.Concat("Невозможно снять блокировку.\r\n", d), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LockPresentation()
        {
            //Блокировка
            if (!PresentationController.Instance.PresentationLocked)
            {
                string d = PresentationStatusInfo.GetPresentationStatusDescr(m_Presentation.UniqueName, m_Presentation.Name);
                if (DesignerClient.Instance.PresentationWorker.AcquireLockForPresentation(m_Presentation.UniqueName, RequireLock.ForEdit))
                {
                    PresentationController.Instance.PresentationLocked = true;
                }
                else
                {
                    //PresentationController.Instance.PresentationLocked = false;
                    //невозможно установить блокировку
                    MessageBoxExt.Show(String.Concat("Невозможно установить блокировку.\r\n", d), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //сохранение сценария
                if (SavePresentation())
                    UnlockPresentation();
            }
        }

        #endregion

        #region History
        void OnHistoryChanged(Object Target)
        {
            undoStripButton.Enabled = undoMenuButton.Enabled = UndoService.Instance.CanUndo;
            redoStripButton.Enabled = redoMenuButton.Enabled = UndoService.Instance.CanRedo;
        }
        #endregion

        #region История - действия пользователя
        private void undoStripButton_Click(object sender, EventArgs e)
        {
            if (UndoService.Instance.CanUndo)
                UndoService.Instance.Undo();
        }

        private void redoStripButton_Click(object sender, EventArgs e)
        {
            if (UndoService.Instance.CanRedo)
                UndoService.Instance.Redo();
        }
        #endregion

        #region Меню сцена - действия пользователя
        private void addLinkMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.LinkToolActivate();
        }

        private void addSlideMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.CreateSlideToolActivate();
        }

        private void lockSlideMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.LockSlide();
        }

        private void unlockSlideMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.UnLockSlide();
        }

        private void copyMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.Copy();
        }

        private void pasteMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.Paste();
        }

        private void slidePropsMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.ShowSlideProperties();
        }

        private void removeMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.RemoveSelected();
        }

        private void goFirstSlideMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.GoFirst();
        }

        private void goPrevSlideMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.GoPrev();
        }

        private void goNextSlideMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.GoNext();
        }

        private void goLastSlideMenuButton_Click(object sender, EventArgs e)
        {
            slideDiagram.GoLast();
        }

        private void slidesMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            refreshSlidesMenuButton.Enabled = !DesignerClient.Instance.IsStandAlone && slideDiagram.SelectedSlideCount > 0;

            //Открывается меню "сцена"
            if (slideDiagram.SelectedSlideCount > 0 && !DesignerClient.Instance.IsStandAlone)
            {
                var slides = slideDiagram.GetSelectedSlides();
                lockSlideMenuButton.Enabled = PresentationController.Instance.IsLockGUICommandEnabled(slides);
                unlockSlideMenuButton.Enabled = PresentationController.Instance.IsUnlockGUICommandEnabled(slides);
            }
            else
            {
                unlockSlideMenuButton.Enabled = false;
                lockSlideMenuButton.Enabled = false;
            }
            unlockAllMenuButton.Enabled = SlideGraphController.Instance.IsAnySlidesLocked((Slide)null);
            lockAllMenuButton.Enabled = SlideGraphController.Instance.IsAnySlidesUnlocked((Slide)null);

            addSlideMenuButton.Enabled = addLinkMenuButton.Enabled = PresentationController.Instance.PresentationLocked || DesignerClient.Instance.IsStandAlone;

            slidePropsMenuButton.Enabled = slideDiagram.SelectedSlideCount == 1;

            copyMenuButton.Enabled = slideDiagram.CanCopy && (PresentationController.Instance.PresentationLocked || DesignerClient.Instance.IsStandAlone);
            pasteMenuButton.Enabled = slideDiagram.CanPaste && (PresentationController.Instance.PresentationLocked || DesignerClient.Instance.IsStandAlone);
            removeMenuButton.Enabled = slideDiagram.CanRemove && (PresentationController.Instance.PresentationLocked || DesignerClient.Instance.IsStandAlone);
            slideToXmlMenuButton.Enabled = slideDiagram.SelectedSlideCount > 0;
            slideFromXMLMenuButton.Enabled = PresentationController.Instance.PresentationLocked || DesignerClient.Instance.IsStandAlone;

            goFirstSlideMenuButton.Enabled = true;
            Slide selected = PresentationController.Instance.SelectedSlide;
            goPrevSlideMenuButton.Enabled = selected != null && PresentationController.Instance.CanSelectPrevSlide;
            goNextSlideMenuButton.Enabled = selected != null && PresentationController.Instance.CanSelectNextSlide;
        }

        private void refreshSlidesMenuButton_Click(object sender, EventArgs e)
        {
            // TODO: Обновление сцен
        }

        private void slideToXmlMenuButton_Click(object sender, EventArgs e)
        {
            if (PresentationController.Instance.PresentationChanged)
            {
                MessageBoxAdv.Show("Изменения не сохранены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // актуализируем презентацию
            SlideGraphController.Instance.SavePresentationChanges();
            ExportSlideController.Instanse.Export(PresentationController.Instance.Presentation, slideDiagram.GetSelectedSlides().ToArray());
        }

        private void slideFromXMLMenuButton_Click(object sender, EventArgs e)
        {
            ImportSlideController.Instanse.Import(m_Presentation,
                SourcesController.Instance.GetResources(false).Union(SourcesController.Instance.GetResources(true)).ToArray(),
                SourcesController.Instance.GetDeviceResources().ToArray(),
                SlideGraphController.Instance.AddSlide,
                SlideGraphController.Instance.CreateLink,
                SlideGraphController.Instance.IsSlideUniqueName,
                SlideView.Margin, SlideView.MaxHeight);
        }

        #endregion

        #region Shown
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            sourcesControl.SelectGlobalSource();
            StatusLabel.Text = Description.GetModeDescription(DesignerClient.Instance.IsStandAlone);
        }
        #endregion

        #region Сценарий
        private void savePresentationToolButton_Click(object sender, EventArgs e)
        {
            SavePresentation();
        }

        private void toXmlMenuButton_Click(object sender, EventArgs e)
        {
            ExportPresentationController.Instanse.Export(new PresentationInfo[] { m_PresentationInfo });
        }

        private void searchMenuButton_Click(object sender, EventArgs e)
        {
            FindItemController.Instance.ShowSearchForm(ItemToSearch.Slide);
        }

        private void propertiesMenuButton_Click(object sender, EventArgs e)
        {
            //Свойства сценария
            using (PresentationPropertiesForm form = new PresentationPropertiesForm(m_PresentationInfo))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    m_Presentation.Name = m_PresentationInfo.Name;
                    m_Presentation.Comment = m_PresentationInfo.Comment;
                    m_Presentation.Author = m_PresentationInfo.Author;

                    RefreshTitle();
                    PresentationController.Instance.PresentationChanged = true;
                    savePresentationToolButton.Enabled = true;
                    saveMenuButton.Enabled = true;
                }
            }
        }

        private void updateMenuButton_Click(object sender, EventArgs e)
        {
            //TODO: Обновить сценарий
        }

        private void presentationMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            saveMenuButton.Enabled = savePresentationToolButton.Enabled = PresentationController.Instance.PresentationChanged;
            lockMenuButton.Enabled = lockToolButton.Enabled;

            if (DesignerClient.Instance.IsStandAlone || StartedFromPlayer)
            {
                updateMenuButton.Enabled = false;
                lockMenuButton.Enabled = lockToolButton.Enabled = false;
            }
        }

        #endregion

        #region Вид
        private void viewMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            //Открывается drop-down "Вид"! Обновить команды
            //this.dockingManager.SetDockVisibility(displayListControl, 
        }
        #endregion

        #region Дисплеи
        private void displayMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            //Открывается drop-down "Дисплей"! Обновить команды
            createDisplayGroupMenuButton.Enabled = displayListControl.CreateDisplayGroupEnabled;
            removeDisplayGroupMenuButton.Enabled = displayListControl.RemoveDisplayGroupEnabled;
        }

        private void createDisplayGroupMenuButton_Click(object sender, EventArgs e)
        {
            //Создать группу дисплеев
            displayListControl.CreateDisplayGroup();
        }

        private void displayPropsMenuButton_Click(object sender, EventArgs e)
        {
            //Дисплей - Свойства
            displayListControl.ShowProperties();
        }

        private void removeDisplayGroupMenuButton_Click(object sender, EventArgs e)
        {
            //Удалить группу дисплеев
            displayListControl.RemoveDisplayGroup();
        }

        private void findDisplayMenuButton_Click(object sender, EventArgs e)
        {
            FindItemController.Instance.ShowSearchForm(ItemToSearch.Display);
        }

        private void refreshDisplayMenuButton_Click(object sender, EventArgs e)
        {
            //TODO: Дисплей - Обновить
        }

        #endregion

        #region Change visibility
        private void displaysMenuButton_Click(object sender, EventArgs e)
        {
            //Вид - Дисплеи
            dockingManager.SetDockVisibility(displayListControl, displaysMenuButton.Checked);
        }

        private void sourcesMenuButton_Click(object sender, EventArgs e)
        {
            //Вид - Источники
            dockingManager.SetDockVisibility(sourcesControl, sourcesMenuButton.Checked);
        }

        private void equipmentMenuButton_Click(object sender, EventArgs e)
        {
            //Вид - Оборудование
            dockingManager.SetDockVisibility(equipmentControl, equipmentMenuButton.Checked);
        }

        private void viewSourceProps_Click(object sender, EventArgs e)
        {
            //Вид - Свойства источника
            dockingManager.SetDockVisibility(sourcePropertiesControl, viewSourceProps.Checked);
        }
        #endregion

        #region Источники

        private void sourcesMenuDropButton_DropDownOpening(object sender, EventArgs e)
        {
        }

        private void presentationSourcesMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            // Открывается меню Источники сценария
            addPresenationSourceMenuButton.Enabled = sourcesControl.PresentationSourceAddOperationEnable;
            removeSourceMenuButton.Enabled = sourcesControl.PresentationSourceOperationsEnabled;
            copyPresentationSourceMenuButton.Enabled = sourcesControl.PresentationSourceOperationsEnabled;
            viewSourceMenuButton.Enabled = sourcesControl.PresentationSourceOperationsEnabled;
        }

        private void commonSourcesMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            //Открывается меню Общие источники
            commonSourcesAddMenuButton.Enabled = sourcesControl.GlobalSourceAddOperationEnable;       //sourcesControl.GlobalSourceOperationsEnabled;
            commonSourcesRemoveMenuButton.Enabled = sourcesControl.GlobalSourceOperationsEnabled;
            commonSourcesCopyMenuButton.Enabled = sourcesControl.GlobalSourceOperationsEnabled;
            commonSourcesViewMenuButton.Enabled = sourcesControl.GlobalSourceOperationsEnabled;
        }

        private void commonSourcesSearchMenuButton_Click(object sender, EventArgs e)
        {
            //Источники - Поиск
            sourcesControl.FindSource();
        }

        private void commonSourcesPropsMenuButton_Click(object sender, EventArgs e)
        {
            //Показать свойства выбранного источника
            if (sourcesControl.GlobalSourceOperationsEnabled)
                sourcesControl.ShowGlobalSourceProperties();
            else
                if (sourcesControl.PresentationSourceOperationsEnabled)
                    sourcesControl.ShowPresentationSourceProperties();
        }

        private void commonSourcesRefreshMenuButton_Click(object sender, EventArgs e)
        {
            //Источники - Обновить
            sourcesControl.UpdateSources();
        }

        private void addPresenationSourceMenuButton_Click(object sender, EventArgs e)
        {
            //Добавление источника сценария
            sourcesControl.AddPresentationSource();
        }

        private void copyPresentationSourceMenuButton_Click(object sender, EventArgs e)
        {
            //Источники - Источники сценария - Копировать
            sourcesControl.CopySourceToGlobal();
        }

        private void removeSourceMenuButton_Click(object sender, EventArgs e)
        {
            //Источники - Источники сценария - Удалить
            sourcesControl.RemovePresentationSource();
        }

        private void viewSourceMenuButton_Click(object sender, EventArgs e)
        {
            //Источники - Источники сценария - Просмотр
            sourcesControl.PresentationSourcePreview();
        }

        private void commonSourcesAddMenuButton_Click(object sender, EventArgs e)
        {
            //Добавление Общего источника
            sourcesControl.AddGlobalSource();
        }

        private void commonSourcesCopyMenuButton_Click(object sender, EventArgs e)
        {
            //Источники - Общие источники - Копировать
            sourcesControl.CopySourceToPresentation();
        }

        private void commonSourcesRemoveMenuButton_Click(object sender, EventArgs e)
        {
            //Источники - Общие источники - Удалить
            sourcesControl.RemoveGlobalSource();
        }

        private void commonSourcesViewMenuButton_Click(object sender, EventArgs e)
        {
            //Источники - Общие источники - Просмотр
            sourcesControl.GlobalSourcePreview();
        }

        private void sourcesSearchMenuButton_Click(object sender, EventArgs e)
        {
            //Источники - Общие источники - Поиск
            sourcesControl.FindSource(true);
        }
        #endregion

        #region Оборудование
        private void equipmentDropMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            //TODO: Открывается drop-down "Оборудование"! Обновить команды
        }

        private void findEquimpentMenuButton_Click(object sender, EventArgs e)
        {
            FindItemController.Instance.ShowSearchForm(ItemToSearch.Device);
        }

        private void equipmentPropsMenuButton_Click(object sender, EventArgs e)
        {
            //TODO: Оборудование - Свойства
        }

        private void equipmentRefreshMenuButton_Click(object sender, EventArgs e)
        {
            //TODO: Оборудование - Обновить
        }
        #endregion

        #region Раскладка
        private void layoutMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            //Открывается drop-down "Раскладка"! Обновить команды
            bool enabled = LayoutController.Instance.SelectedWindow != null;
            bool canedit = LayoutController.Instance.CanEdit || DesignerClient.Instance.IsStandAlone;
            layoutWindowRemoveMenuButton.Enabled = enabled && canedit;
            windowBringToFrontMenuButton.Enabled = enabled && canedit;
            windowSendToBackMenuButton.Enabled = enabled && canedit;
            windowMoveForwardMenuButton.Enabled = enabled && canedit;
            windowMoveBackwardMenuButton.Enabled = enabled && canedit;
            windowFullSizeMenuButton.Enabled = enabled && canedit;
            bool segmented = LayoutController.Instance.IsSegmentedDisplay();
            x1Button.Enabled = enabled && canedit;
            x2Button.Enabled = enabled && canedit;
            x3Button.Enabled = enabled && canedit;
            x3Button.Visible = x2Button.Visible = x1Button.Visible = segmented;

            layoutBackgroundMenuButton.Enabled = canedit;
            layoutPreviewMenuButton.Visible = previewToolButton.Visible = !DesignerClient.Instance.IsStandAlone;
        }


        private void layoutWindowPropsMenuButton_Click(object sender, EventArgs e)
        {
            //Раскладка - Свойства окна
            slideLayoutControl.ShowWindowProps();
        }

        private void layoutWindowRemoveMenuButton_Click(object sender, EventArgs e)
        {
            //Раскладка - Удалить окно
            slideLayoutControl.RemoveWindow();
        }

        private void windowBringToFrontMenuButton_Click(object sender, EventArgs e)
        {
            //Раскладка - На передний план
            slideLayoutControl.WindowBringToFront();
        }

        private void windowSendToBackMenuButton_Click(object sender, EventArgs e)
        {
            //Раскладка - На задний план
            slideLayoutControl.WindowSendToBack();
        }

        private void windowMoveForwardMenuButton_Click(object sender, EventArgs e)
        {
            //Раскладка - Вперед
            slideLayoutControl.WindowMoveForward();
        }

        private void windowMoveBackwardMenuButton_Click(object sender, EventArgs e)
        {
            //Раскладка - Назад
            slideLayoutControl.WindowMoveBackward();
        }

        private void layoutBackgroundMenuButton_Click(object sender, EventArgs e)
        {
            //Раскладка - Фон
            slideLayoutControl.BackgroundProps();
        }

        private void layoutPreviewMenuButton_Click(object sender, EventArgs e)
        {
            //Раскладка - Предварительный просмотр
            slideLayoutControl.ShowPreview();
        }

        void findSlideMenuButton_Click(object sender, System.EventArgs e)
        {
            FindItemController.Instance.ShowSearchForm(ItemToSearch.Slide);
        }
        #endregion

        #region User actions
        private void closeMenuButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lockMenuButton_Click(object sender, EventArgs e)
        {
            toolStripEx2.Enabled = false;
            toolStripEx2.Text = "Выполнение...";

            LockPresentation();

            toolStripEx2.Text = String.Empty;
            toolStripEx2.Enabled = true;
        }


        private void PresentationDesignerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !this.presentationRemoved)
            {
                if (/*PresentationController.Instance.PresentationLocked && */PresentationController.Instance.PresentationChanged)
                {
                    switch (MessageBoxExt.Show(Properties.Resources.SavePresentation, Properties.Resources.Confirmation, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes: e.Cancel = !SavePresentation(); break;
                        case DialogResult.Cancel: e.Cancel = true; return;
                        case DialogResult.No:
                            break;
                            //{
                            //    if (PresentationController.Instance.PresentationLocked && !StartedFromPlayer)
                            //        UnlockPresentation(false);
                            //    PresentationController.Instance.UnlockAllSlides(false);
                            //}
                            //return;
                    }
                }


                if (e.Cancel) return;
            }

            if (e.CloseReason != CloseReason.None)
            {
                bool isOk = true;

                if (!this.presentationRemoved)
                {
                    //снятие блокировки
                    if (PresentationController.Instance.PresentationLocked && !DesignerClient.Instance.IsStandAlone)
                    {
                        isOk = StartedFromPlayer ? true : UnlockPresentation();
                    }

                    if (isOk)
                    {
                        if (PresentationController.Instance.SomeSlidesLocked && !DesignerClient.Instance.IsStandAlone)
                        {
                            try
                            {
                                //unlock in silent mode
                                PresentationController.Instance.UnlockAllSlides(false);
                            }
                            catch
                            {
                                //MessageBox.Show("Во время разблокирови сцен произошла ошибка: " + ex.Message);
                                isOk = false;
                            }
                        }
                    }
                }

                //if (isOk)
                {
                    PresentationController.Instance.OnChanged -= PresentationChanged;
                    PresentationController.Instance.OnPresentationLockChanged -= Instance_OnPresentationLockChanged;
                    PresentationController.Instance.OnPresentationRemoved -= Instance_OnPresentationRemoved;
                    PresentationController.Instance.OnPresentationLockedExternally -= Instance_OnPresentationLockedExternally;
                    PresentationController.Instance.OnPresentationUnlockedExternally -= Instance_OnPresentationUnlockedExternally;

                    UndoService.Instance.OnHistoryChanged -= new HistoryChanged(OnHistoryChanged);

                    SlideGraphController.Instance.Dispose();
                    SourcesController.Instance.Dispose();
                    LayoutController.Instance.OnShownStatusChanged -= Instance_OnShownStatusChanged;

                    LayoutController.Instance.Dispose();
                    DisplayController.Instance.Dispose();
                    PresentationController.Instance.Dispose();
                }

                string path = Application.StartupPath + "\\dockstate.xml";

                if (File.Exists(path))
                    File.Delete(path);

                AppStateSerializer serializer = new AppStateSerializer(SerializeMode.XMLFile, path);
                this.dockingManager.SaveDockState(serializer);
                serializer.PersistNow();
            }
        }

        #endregion
        
        private void sourcesMenuDropButton_Click(object sender, EventArgs e)
        {

        }

        void lockAllMenuButton_Click(object sender, System.EventArgs e)
        {
            slideDiagram.SetLockForAllSlides(true);
        }

        void unlockAllMenuButton_Click(object sender, System.EventArgs e)
        {
            slideDiagram.SetLockForAllSlides(false);
        }

        void windowFullSizeMenuButton_Click(object sender, System.EventArgs e)
        {
            slideLayoutControl.SetWindowFullSize();
        }


        void x3Button_Click(object sender, System.EventArgs e)
        {
            LayoutController.Instance.Set3x3Size();
        }

        void x2Button_Click(object sender, System.EventArgs e)
        {
            LayoutController.Instance.Set2x2Size();
        }

        void x1Button_Click(object sender, System.EventArgs e)
        {
            LayoutController.Instance.Set1x1Size();
        }

    }
}
