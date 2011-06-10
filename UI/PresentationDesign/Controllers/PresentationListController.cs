using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.PowerPointLib;
using UI.PresentationDesign.DesignUI.Classes.Helpers;
using UI.PresentationDesign.DesignUI.Forms;
using System.Threading;
using Domain.PresentationDesign.Client;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPresentation;
using Syncfusion.Windows.Forms;
using System.Windows.Forms;
using TechnicalServices.Common.Utils;
using System.ComponentModel;
using UI.PresentationDesign.DesignUI.Helpers;
using TechnicalServices.Common.Classes;


namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    public class PresentationListController
    {
        #region fields and properties
        SortableBindingList<PresentationInfo> pList;
        UserIdentity identity;
        PresentationListForm view;

        bool filtered = false;

        public UserIdentity UserIdentity
        {
            get
            {
                return identity;
            }
        }

        Identity presentationID = new Identity(0);

        static PresentationListController _instance;
        public static PresentationListController Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region ctor
        public PresentationListController(PresentationListForm AView)
        {
            _instance = this;
            view = AView;

            identity = Thread.CurrentPrincipal as UserIdentity;

            DesignerClient.Instance.PresentationNotifier.OnObjectLocked += new EventHandler<NotifierEventArg<LockingInfo>>(PresentationNotifier_OnObjectLocked);
            DesignerClient.Instance.PresentationNotifier.OnObjectUnLocked += new EventHandler<NotifierEventArg<LockingInfo>>(PresentationNotifier_OnObjectUnLocked);
            DesignerClient.Instance.PresentationNotifier.OnPresentationAdded += new EventHandler<NotifierEventArg<PresentationInfo>>(PresentationNotifier_OnPresentationAdded);
            DesignerClient.Instance.PresentationNotifier.OnPresentationDeleted += new EventHandler<NotifierEventArg<PresentationInfo>>(PresentationNotifier_OnPresentationDeleted);
            DesignerClient.Instance.PresentationNotifier.OnObjectChanged += new EventHandler<NotifierEventArg<IList<ObjectInfo>>>(PresentationNotifier_OnObjectChanged);

            DesignerClient.Instance.PresentationWorker.SubscribeForGlobalMonitoring();
        }

        #endregion

        #region Notifications

        void PresentationNotifier_OnObjectChanged(object sender, NotifierEventArg<IList<ObjectInfo>> e)
        {
            view.Invoke(new MethodInvoker(() =>
                {
                    foreach (var info in e.Data)
                    {
                        if (info.ObjectKey is PresentationKey)
                        {
                            string uid = ((PresentationKey)info.ObjectKey).PresentationUniqueName;
                            var pr = pList.Where(p => p.UniqueName == uid).FirstOrDefault();
                            if (pr != null)
                            {
                                int i = pList.IndexOf(pr);
                                pList[i] = DesignerClient.Instance.PresentationWorker.GetPresentationInfo(uid);
                            }
                        }
                    }

                    view.RefreshView();
                }));
        }

        void PresentationNotifier_OnPresentationDeleted(object sender, NotifierEventArg<PresentationInfo> e)
        {
            if (pList.Contains(e.Data))
            {
                view.Invoke(new MethodInvoker(() =>
                    {
                        var pr = pList.Where(p => p.UniqueName == e.Data.UniqueName).FirstOrDefault();

                        if (pr != null)
                            pList.Remove(pr);

                        view.RefreshView();
                    }));
            }
        }

        void PresentationNotifier_OnPresentationAdded(object sender, NotifierEventArg<PresentationInfo> e)
        {
            //if (creatingPresentation == null || !creatingPresentation.Equals(e.Data))
            if (!pList.Contains(e.Data))
            {
                view.Invoke(new MethodInvoker(() =>
                                                  {
                                                      pList.Add(e.Data);
                                                      view.Sort();
                                                  }));
            }
            creatingPresentation = null;
        }

        void UpdateKeyStatus(PresentationKey key, LockingInfo info)
        {
            var pp = pList.Where(p => p.UniqueName == key.PresentationUniqueName);
            if (pp.Count() > 0)
            {
                PresentationInfo pi = pp.First();
                PresentationInfoExt ext = new PresentationInfoExt(pi, info);
                int i = pList.IndexOf(pi);
                if ((i != -1) && i <= pList.Count - 1)
                    pList[i] = ext;

                view.Invoke(new MethodInvoker(() =>
                {
                    view.RefreshView();
                }));
            }
        }

        void PresentationNotifier_OnObjectUnLocked(object sender, NotifierEventArg<LockingInfo> e)
        {
            if (e.Data.ObjectKey is PresentationKey)
                UpdateKeyStatus((PresentationKey)e.Data.ObjectKey, null);
        }

        void PresentationNotifier_OnObjectLocked(object sender, NotifierEventArg<LockingInfo> e)
        {
            if (e.Data.ObjectKey is PresentationKey)
                UpdateKeyStatus((PresentationKey)e.Data.ObjectKey, e.Data);
        }

        #endregion

        #region Filtering
        public IEnumerable<PresentationInfo> FilterPresentationsBySlides(IEnumerable<PresentationInfo> result)
        {
            if (result == null) return null;
            return result.Where(p => p.SlideInfoList.Any(TestSlideCondition));
        }

        public IEnumerable<PresentationInfo> FilterPresenations()
        {
            if (pList == null) return null;
            return pList.Where(TestPresentationCondition);
        }

        public bool TestSlideCondition(SlideInfo slide)
        {
            string author = (String.IsNullOrEmpty(slide.Author) ? String.Empty : slide.Author).ToUpper();
            string name = (String.IsNullOrEmpty(slide.Name) ? String.Empty : slide.Name).ToUpper();
            string comment = (String.IsNullOrEmpty(slide.Comment) ? String.Empty : slide.Comment).ToUpper();

            return
            (author.Contains(view.slideFilterControl.Author.ToUpper()) || view.slideFilterControl.Author == String.Empty) &&
            (name.Contains(view.slideFilterControl.SlideName.ToUpper()) || view.slideFilterControl.SlideName == String.Empty) &&
            (slide.Modified >= view.slideFilterControl.FromDate && slide.Modified <= view.slideFilterControl.ToDate) &&
            (comment.Contains(view.slideFilterControl.Comment.ToUpper()) || view.slideFilterControl.Comment == String.Empty);
        }


        public bool TestPresentationCondition(PresentationInfo p)
        {
            return
                (p.Author.ToUpper().Contains(view.presentationFilterControl.Author.ToUpper()) || view.presentationFilterControl.Author == String.Empty) &&
                (p.Comment.ToUpper().Contains(view.presentationFilterControl.Comment.ToUpper()) || view.presentationFilterControl.Comment == String.Empty) &&
                (p.LastChangeDate >= view.presentationFilterControl.FromDate && p.LastChangeDate <= view.presentationFilterControl.ToDate) &&
                (p.Name.ToUpper().Contains(view.presentationFilterControl.PresentationName.ToUpper()) || view.presentationFilterControl.PresentationName == String.Empty) &&
                (p.SlideCount >= view.presentationFilterControl.FromSlideCount && p.SlideCount <= view.presentationFilterControl.ToSlideCount);
        }



        public void Unfilter()
        {
            filtered = false;
            view.presentationFilterControl.Clear();
            view.slideFilterControl.Clear();
            view.DataSource = pList;
        }

        #endregion

        #region Presentation commands

        Mutex GetPresentationMutex(PresentationInfo pi)
        {
            return new Mutex(false, "Opened::" + pi.UniqueName);
        }


        public void LoadPresentationList()
        {
            PushSelectedRow();

            int columnIndex = view.GridView.SortedColumn != null ? view.GridView.SortedColumn.Index : 1;
            SortOrder sOrder = view.GridView.SortOrder;
            ListSortDirection order = sOrder == SortOrder.Descending
                                          ? ListSortDirection.Descending
                                          : ListSortDirection.Ascending;

            IList<PresentationInfo> list = DesignerClient.Instance.PresentationWorker.GetPresentationInfoList().Cast<PresentationInfo>()/*.OrderBy(p => p.Name)*/.ToList();
            pList = new SortableBindingList<PresentationInfo>(list);
            view.DataSource = pList;
            view.GridView.Sort(view.GridView.Columns[columnIndex], order);

            PopSelectedRow();
        }

        public LockingInfo GetPresentationStatus(PresentationInfo info)
        {
            var pp = pList.Where(p => p.UniqueName == info.UniqueName);
            if (pp.Count() > 0)
            {
                return ((PresentationInfoExt)pp.First()).LockingInfo;
            }
            return null;
        }


        public bool RemovePresentation(PresentationInfo info, ref PresentationStatus status)
        {
            if (DesignerClient.Instance.IsStandAlone)
            {
                Mutex presentationMutex = GetPresentationMutex(info);
                if (!presentationMutex.WaitOne(1, true))
                {
                    status = PresentationStatus.AlreadyLocallyOpened;
                }
                presentationMutex.Close();
            }

            if (status == PresentationStatus.ExistsAndUnLocked)
            {
                if (DesignerClient.Instance.PresentationWorker.DeletePresentation(info.UniqueName))
                {
                    pList.Remove(info);
                    return true;
                }
            }

            return false;
        }

        public void Filter()
        {
            PushSelectedRow();

            filtered = true;
            IEnumerable<PresentationInfo> result = FilterPresenations().ToList();
            result = FilterPresentationsBySlides(result).ToList();
            if (result != null)
            {
                view.DataSource = new SortableBindingList<PresentationInfo>(result.ToList());
            }

            PopSelectedRow();
        }

        int rowIndex = -1;

        private void PushSelectedRow()
        {
            if (view.GridView.SelectedRows.Count > 0)
                rowIndex = view.GridView.SelectedRows[0].Index;
        }

        private void PopSelectedRow()
        {
            if (rowIndex > -1)
            {
                foreach (DataGridViewRow r in view.GridView.Rows)
                    r.Selected = false;

                if (view.GridView.Rows.Count - 1 >= rowIndex)
                {
                    DataGridViewRow row = view.GridView.Rows[rowIndex];
                    row.Selected = true;
                    view.GridView.FirstDisplayedScrollingRowIndex = row.Index;
                }
            }
        }

        public static void OpenPresentationByName(String name)
        {
        }

        public void OpenPresentation(PresentationInfo info, bool isDesign)
        {
            if (!CheckForConfiguration())
                return;
            info = DesignerClient.Instance.PresentationWorker.GetPresentationInfo(info.UniqueName);
            if (info != null)
            {
                if (isDesign)
                {
                    Mutex presentationMutex = GetPresentationMutex(info);
                    if (!presentationMutex.WaitOne(1, true))
                    {
                        MessageBoxExt.Show("Сценарий уже открыт на вашем компьютере!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    view.ShowDesigner(info, false);

                    LoadPresentationList();
                    presentationMutex.Close();
                }
                else
                {
                    if (!DesignerClient.Instance.PresentationWorker.AcquireLockForPresentation(info.UniqueName, RequireLock.ForShow))
                    {
                        UserIdentity uid;
                        PresentationStatus s = DesignerClient.Instance.PresentationWorker.GetPresentationStatus(info.UniqueName, out uid);
                        String fio = string.IsNullOrEmpty(uid.User.FullName) ? uid.User.Name : uid.User.FullName;
                        if (uid.User.Name != (Thread.CurrentPrincipal as UserIdentity).User.Name)
                        {
                            MessageBoxAdv.Show(String.Format("Сценарий \"{0}\" заблокирован пользователем: {1}.\r\nДля показа сценария необходимо его разблокировать.", info.Name, fio), "Показ сценария");
                            return;
                        }
                    }
                    try
                    {
                        view.ShowPlayer(info);
                    }
                    finally
                    {
                        DesignerClient.Instance.PresentationWorker.ReleaseLockForPresentation(info.UniqueName);
                    }
                }
            }
            else
            {
                MessageBoxExt.Show("Невозможно открыть сценарий!\r\nВозможно он был удален.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pList.Remove(info);
                view.RefreshView();
            }
            RestoreFilter(info);
        }

        #region CreatePresentation

        PresentationInfo creatingPresentation;

        public void CreatePresentation()
        {
            if (!CheckForConfiguration())
                return;

            string authorName = identity.User.FullName;
            if (String.IsNullOrEmpty(authorName))
                authorName = identity.User.Name;

            string name = FindNextName();
            Presentation presentation = PresentationController.NewPresentation(name, authorName, 1);
            PresentationInfo pi = new PresentationInfo(presentation);

            DialogResult result;
            using (PresentationPropertiesForm dlg = new PresentationPropertiesForm(pi, true))
                result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!CreatePresentationWithOneSlide(pi))
                    return;
            }
            else
            {
                presentationID = new Identity(presentationID.CurrentID - 1);
            }

            RestoreFilter(creatingPresentation);
        }

        public void CreatePresentationFromPpt()
        {
            if (!CheckForConfiguration())
                return;

            string authorName = identity.User.FullName;
            if (String.IsNullOrEmpty(authorName))
                authorName = identity.User.Name;

            Presentation presentation = PresentationController.NewPresentation(FindNextName(), authorName, 0);
            PresentationInfo pi = new PresentationInfo(presentation);

            DialogResult result;
            using (PresentationPropertiesForm dlg = new PresentationPropertiesForm(pi, true))
                result = dlg.ShowDialog(view);
            if (result == DialogResult.OK)
            {
                // выбор файла PPT
                string selectedFile = null;
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Multiselect = false;
                    openFileDialog.Filter = "Презентация PowerPoint (*.ppt)|*.ppt";
                    openFileDialog.RestoreDirectory = true;
                    result = openFileDialog.ShowDialog(view);
                    selectedFile = openFileDialog.FileName;
                }
                if (result == DialogResult.OK && !string.IsNullOrEmpty(selectedFile))
                {
                    try
                    {
                        // обработка файла
                        string[] files = CreatePptFilesFromFile(selectedFile);
                        try
                        {
                            // выбор активного дисплея
                            DisplayType selectedDisplay = null;
                            using (SelectActiveDisplayForm form = new SelectActiveDisplayForm(
                                PresentationController.Configuration.ModuleConfiguration.DisplayList.Where(
                                    dt => !dt.IsHardware)))
                            {
                                if (form.ShowDialog(view) == DialogResult.Cancel)
                                    return;
                                selectedDisplay = form.SelectedDisplay;
                            }
                            BackgroundImageDescriptor[] descriptors = CreateBackgroundImageDescriptorsFromFiles(files,
                                                                                                                pi);
                            if (!SaveBackGroundDescriptors(descriptors))
                            {
                                MessageBoxExt.Show("Не удалось создать подложки!", "Ошибка", MessageBoxButtons.OK,
                                                   MessageBoxIcon.Error);
                                return;
                            }
                            // создание сценария с кучей сценой
                            if (!CreatePresentationWithBackgrounds(pi, descriptors, selectedDisplay))
                                return;
                        }
                        finally
                        {
                            foreach (string file in files)
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    catch
                    {
                        MessageBoxExt.Show("Выбран некорректный файл PowerPoint. Создание сценария невозможно", "Ошибка", MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    // создание сценария с одной сценой
                    if (!CreatePresentationWithOneSlide(pi))
                        return;
                }
            }
            else
            {
                presentationID = new Identity(presentationID.CurrentID - 1);
            }

            RestoreFilter(creatingPresentation);
        }

        private void RestoreFilter(PresentationInfo pi)
        {
            LoadPresentationList();

            //возможно будет убрано 
            rowIndex = pList.IndexOf(pi);
            PopSelectedRow();

            if (filtered)
                Filter();
        }

        private bool CreatePresentationWithOneSlide(PresentationInfo pi)
        {
            Presentation presentation = PresentationController.NewPresentation(pi.Name, pi.Author, 1);
            presentation.UniqueName = pi.UniqueName;
            presentation.Comment = pi.Comment;
            pi = new PresentationInfo(presentation);

            creatingPresentation = pi;
            int[] labelNotExists;

            if (CreatePresentationResult.Ok == DesignerClient.Instance.PresentationWorker.CreatePresentation(pi, out labelNotExists))
            {
                if (!DesignerClient.Instance.IsStandAlone)
                {
                    if (!DesignerClient.Instance.PresentationWorker.AcquireLockForPresentation(pi.UniqueName, RequireLock.ForEdit))
                    {
                        MessageBoxExt.Show("Не удалось заблокировать созданный сценарий для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    pi = new PresentationInfoExt(pi, new LockingInfo(identity, RequireLock.ForEdit, new PresentationKey(pi.UniqueName)));
                }
                else
                    pi = new PresentationInfoExt(pi, null);

                ShowJustCreatedPresentation(pi);
            }
            else
                MessageBoxExt.Show("Не удалось создать сценарий!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return true;
        }

        private void ShowJustCreatedPresentation(PresentationInfo pi)
        {
            Mutex presentationMutex = GetPresentationMutex(pi);
            presentationMutex.WaitOne(1, true);

            view.ShowDesigner(pi, true, true);

            LoadPresentationList();

            presentationMutex.Close();
        }

        private bool CreatePresentationWithBackgrounds(PresentationInfo presentationInfo, BackgroundImageDescriptor[] descriptors, DisplayType selectedDisplay)
        {
            PresentationInfo pi;
            string uniqueName = presentationInfo.UniqueName;
            Presentation presentation = PresentationController.NewPresentation(presentationInfo.Name, presentationInfo.Author, descriptors.Count());
            presentation.UniqueName = uniqueName;
            presentation.Comment = presentationInfo.Comment;
            for (int i = 0; i < presentation.SlideList.Count; i++)
            {
                Display display = selectedDisplay.CreateNewDisplay();
                ActiveDisplay activeDisplay = display as ActiveDisplay;
                if (activeDisplay != null)
                {
                    activeDisplay.BackgroundImage = descriptors[i].Id;
                    presentation.SlideList[i].DisplayList.Add(activeDisplay);
                }
            }

            pi = new PresentationInfo(presentation);

            creatingPresentation = pi;
            int[] labelNotExists;

            if (CreatePresentationResult.Ok == DesignerClient.Instance.PresentationWorker.CreatePresentation(pi, out labelNotExists))
            {
                if (!DesignerClient.Instance.IsStandAlone)
                {
                    if (!DesignerClient.Instance.PresentationWorker.AcquireLockForPresentation(pi.UniqueName, RequireLock.ForEdit))
                    {
                        MessageBoxExt.Show("Не удалось заблокировать созданный сценарий для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    foreach (Slide slide in presentation.SlideList)
                    {
                        if (!DesignerClient.Instance.PresentationWorker.AcquireLockForSlide(pi.UniqueName, slide.Id, RequireLock.ForEdit))
                        {
                            MessageBoxExt.Show("Не удалось заблокировать созданную сцену для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    pi = new PresentationInfoExt(pi, new LockingInfo(identity, RequireLock.ForEdit, new PresentationKey(pi.UniqueName)));
                }
                else
                    pi = new PresentationInfoExt(pi, null);

                int[] notLockedSlide;
                ResourceDescriptor[] notExists;
                DeviceResourceDescriptor[] deviceResourceDescriptorsNotExists;
                if (!DesignerClient.Instance.PresentationWorker.SaveSlideChanges(pi.UniqueName,
                                                                                 presentation.SlideList.ToArray(),
                                                                                 out notLockedSlide,
                                                                                 out notExists,
                                                                                 out
                                                                                         deviceResourceDescriptorsNotExists,
                                                                                 out labelNotExists))
                {
                    MessageBoxExt.Show("Не удалось сохранить созданные сцены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!DesignerClient.Instance.IsStandAlone)
                    foreach (Slide slide in presentation.SlideList)
                    {
                        DesignerClient.Instance.PresentationWorker.ReleaseLockForSlide(pi.UniqueName, slide.Id);
                    }

                ShowJustCreatedPresentation(pi);
            }
            else
                MessageBoxExt.Show("Не удалось создать сценарий!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return true;
        }

        #region SaveBackGroundDescriptors

        private bool SaveBackGroundDescriptors(BackgroundImageDescriptor[] backgroundImageDescriptors)
        {
            List<BackgroundImageDescriptor> loaded = new List<BackgroundImageDescriptor>(backgroundImageDescriptors.Count());
            foreach (BackgroundImageDescriptor descriptor in backgroundImageDescriptors)
            {
                string otherResourceId;
                FileSaveStatus status = ClientResourceCRUD.SaveSource(descriptor, out otherResourceId);
                if (status == FileSaveStatus.Ok)
                {
                    loaded.Add(descriptor);
                }
                else
                {
                    // все стираем
                    foreach (BackgroundImageDescriptor imageDescriptor in loaded)
                    {
                        DesignerClient.Instance.PresentationWorker.DeleteSource(imageDescriptor);
                    }
                    return false;
                }
            }
            return true;
        }

        private IClientResourceCRUD<ResourceDescriptor> clientResourceCRUD = null;
        private IClientResourceCRUD<ResourceDescriptor> ClientResourceCRUD
        {
            get
            {
                if (clientResourceCRUD == null)
                {
                    clientResourceCRUD = DesignerClient.Instance.PresentationWorker.GetResourceCrud();
                    //clientResourceCRUD.OnPartTransmit += client_OnPartTransmit;
                    //clientResourceCRUD.OnComplete += client_OnComplete;
                }
                return clientResourceCRUD;
            }
        }

        SplashForm splash = null;
        void client_OnComplete(object sender, OperationStatusEventArgs<ResourceDescriptor> e)
        {
            CloseSplash();
        }

        void CloseSplash()
        {
            if (splash != null && !splash.IsDisposed)
            {
                splash.Invoke(new MethodInvoker(() => splash.HideForm()));
                splash = null;
            }
        }

        void client_OnPartTransmit(object sender, PartSendEventArgs e)
        {
            if (e.NumberOfParts <= 1) return;

            if (splash == null)
            {
                splash = SplashForm.CreateAndShowForm(true, true);
                splash.OnCancel += new EventHandler(splash_OnCancel);
            }

            splash.Progress = (int)(e.Part * 1f / e.NumberOfParts * 100f);
            Application.DoEvents();
        }

        void splash_OnCancel(object sender, EventArgs e)
        {
            if (ClientResourceCRUD != null)
                ClientResourceCRUD.Terminate();
        }

        #endregion

        private BackgroundImageDescriptor[] CreateBackgroundImageDescriptorsFromFiles(string[] files, PresentationInfo pi)
        {
            List<BackgroundImageDescriptor> resources = new List<BackgroundImageDescriptor>(files.Count());
            foreach (string file in files)
            {
                resources.Add(new BackgroundImageDescriptor(file, pi.UniqueName));
            }
            return resources.ToArray();
        }

        private string[] CreatePptFilesFromFile(string file)
        {
            string tempPath = Path.GetTempPath();
            using (PowerPoint powerPoint = PowerPoint.OpenFile(file))
            {
                return powerPoint.SaveSlideAsPptFiles(tempPath);
            }
        }

        private bool CheckForConfiguration()
        {
            if (DesignerClient.Instance.IsStandAlone)
            {
                // супер пупер проверка - если невалидная конфигурация а такое в дизайнере может быть!!!!! - месага
                InvalideModuleConfiguration invalideModuleConfiguration =
                    PresentationController.Configuration.ModuleConfiguration as InvalideModuleConfiguration;
                if (invalideModuleConfiguration != null)
                {
                    MessageBoxExt.Show(invalideModuleConfiguration.ErrorMessage, "Ошибка", MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        #endregion

        private string FindNextName()
        {
            int iCount = 0;
            int maxIterations = Int32.MaxValue - 1;
            while (true)
            {
                string name = "Новый сценарий " + presentationID.NextID;
                if (!pList.Any(pi => pi.Name == name)) return name;

                ++iCount;
                if (iCount > maxIterations) throw new OverflowException("Слишком много имен");
            }
        }

        public void AcceptChangeProperties(PresentationInfo source, PresentationInfo dest)
        {
            source.Name = dest.Name;
            source.Comment = dest.Comment;
            source.Author = dest.Author;
        }

        public bool IsUniqueName(string name, string exceptOne)
        {
            return !pList.Any(p => p.Name.ToUpper() == name.ToUpper() && p.Name.ToUpper() != exceptOne.ToUpper());
        }

        #endregion
    }
}
