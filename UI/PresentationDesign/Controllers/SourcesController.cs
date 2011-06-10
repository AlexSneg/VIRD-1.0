using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using TechnicalServices.Configuration.Common;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;
using TechnicalServices.Util.FileTransfer;
using UI.Common.CommonUI.Editor;
using UI.PresentationDesign.DesignUI.Controls;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Domain.PresentationDesign.Client;
using DomainServices.EnvironmentConfiguration.ConfigModule;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using UI.PresentationDesign.DesignUI.Controls.SourceTree;
using UI.PresentationDesign.DesignUI.Services;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using TechnicalServices.Common;
using System.IO;
using System.Collections;
using UI.PresentationDesign.DesignUI.Forms;
using UI.PresentationDesign.DesignUI.Controllers;
using TechnicalServices.Common.Utils;
using TechnicalServices.Entity;
using UI.PresentationDesign.DesignUI.Controls.SourceProperties;
using System.ComponentModel;
using UI.PresentationDesign.DesignUI.Helpers;
using Domain.PresentationShow.ShowClient;
using TechnicalServices.Interfaces.ConfigModule.Designer;
using TechnicalServices.Common.Classes;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    public class SourcesController : IDisposable
    {
        #region fields and properties
        private CommonConfiguration _config = PresentationController.Configuration;
        Dictionary<string, SourceCategory> global_categories;
        Dictionary<string, SourceCategory> local_categories;
        PresentationInfo m_presentation;
        private SourcesControl _view;
        Dictionary<ResourceDescriptor, ISourceNode> resourceNodes;
        ServiceProvidingContainer serviceContainer;
        bool localResourceCopying = false;
        ISourceNode nodeToRemove = null;
        private IClientResourceCRUD<ResourceDescriptor> clientResourceCRUDForSave = null;
        private IClientResourceCRUD<ResourceDescriptor> clientResourceCRUDForGet = null;
        private List<ResourceDescriptor> nonVisibleResources = new List<ResourceDescriptor>();
        private static SourcesController _instance;
        SplashForm splash;

        public static SourcesController Instance
        {
            get
            {
                return _instance;
            }
        }


        public void SelectSource(ResourceDescriptor source, bool isLocal)
        {
            ISourceNode node = null;
            if (resourceNodes.TryGetValue(source, out node))
            {
                _view.SelectSource(node, isLocal);
            }
        }

        public IClientResourceCRUD<ResourceDescriptor> ClientResourceCRUDForSave
        {
            get
            {
                if (clientResourceCRUDForSave == null)
                {
                    clientResourceCRUDForSave = DesignerClient.Instance.PresentationWorker.GetResourceCrud();
                    //clientResourceCRUD = ClientSourceTransferFactory.CreateClientFileTransfer(
                    //    DesignerClient.Instance.IsStandAlone,
                    //    DesignerClient.Instance.PresentationWorker,
                    //    DesignerClient.Instance.SourceDAL);

                    clientResourceCRUDForSave.OnPartTransmit += new EventHandler<PartSendEventArgs>(_resourceCRUD_OnPartTransmit);
                    clientResourceCRUDForSave.OnComplete += new EventHandler<OperationStatusEventArgs<ResourceDescriptor>>(_resourceCRUD_OnComplete);
                    clientResourceCRUDForSave.OnTerminate += new EventHandler(_resourceCRUD_OnTerminate);
                }
                return clientResourceCRUDForSave;
            }
        }

        public IClientResourceCRUD<ResourceDescriptor> ClientResourceCRUDForGet
        {
            get
            {
                if (clientResourceCRUDForGet == null)
                {
                    clientResourceCRUDForGet = DesignerClient.Instance.PresentationWorker.GetResourceCrud();
                    //clientResourceCRUD = ClientSourceTransferFactory.CreateClientFileTransfer(
                    //    DesignerClient.Instance.IsStandAlone,
                    //    DesignerClient.Instance.PresentationWorker,
                    //    DesignerClient.Instance.SourceDAL);

                    clientResourceCRUDForGet.OnPartTransmit += new EventHandler<PartSendEventArgs>(_resourceCRUD_OnPartTransmit);
                    clientResourceCRUDForGet.OnComplete += new EventHandler<OperationStatusEventArgs<ResourceDescriptor>>(clientResourceCRUDForGet_OnComplete);
                    clientResourceCRUDForGet.OnTerminate += new EventHandler(_resourceCRUD_OnTerminate);
                }
                return clientResourceCRUDForGet;
            }
        }


        #endregion

        #region ctor & factory
        SourcesController(SourcesControl AView, PresentationInfo APresentation)
        {
            _instance = this;
            _view = AView;

            resourceNodes = new Dictionary<ResourceDescriptor, ISourceNode>();
            m_presentation = APresentation = new PresentationInfo(APresentation); //избавляемся от *Ext

            Dictionary<string, IList<ResourceDescriptor>> local_rd = DesignerClient.Instance.PresentationWorker.GetLocalSources(APresentation.UniqueName);
            Dictionary<string, IList<ResourceDescriptor>> common_rd = DesignerClient.Instance.PresentationWorker.GetGlobalSources();
            
            global_categories = new Dictionary<string, SourceCategory>();
            local_categories = new Dictionary<string, SourceCategory>();

            List<String> sourceTypes = new List<string>();
            foreach (SourceType t in _config.ModuleConfiguration.SourceList)
            {
                if (!sourceTypes.Contains(t.Type))
                {
                    sourceTypes.Add(t.Type);

                    if (!(t.IsHardware))
                    {
                        SourceCategory local_cat = new SourceCategory(t, false) { Icon = Properties.Resources.soft };
                        local_categories.Add(t.Type, local_cat);
                    }

                    SourceCategory global_cat = new SourceCategory(t, true) { Icon = Properties.Resources.soft };
                    global_categories.Add(t.Type, global_cat);
                }
            }

            foreach (var rd in local_rd.Union(common_rd))
            {
                foreach (var r in rd.Value)
                {
                    if (r.ResourceInfo is INonVisibleResource)
                    {
                        nonVisibleResources.Add(r);
                    }
                }
            }


            /// Сортировка источников: сперва программные, потом аппаратные, в каждой группе -- по названию.
            foreach (var category in global_categories.Union(local_categories).OrderBy(gos => gos.Value.IsHardware ? "1" + gos.Key: "0" + gos.Key))
            {
                IEnumerable<ResourceDescriptor> e = new List<ResourceDescriptor>();
                if (local_rd.ContainsKey(category.Key))
                    e = local_rd[category.Key];
                if (common_rd.ContainsKey(category.Key))
                    e = e.Union(common_rd[category.Key]);

                foreach (ResourceDescriptor resource in e/*.OrderBy(res=>res.ResourceInfo.Name)*/)
                {
                    if (!(resource is BackgroundImageDescriptor) && !(resource is INonVisibleResource))
                    {
                        ISourceNode node = null;
                        SourceType type = null;

                        if (resource.ResourceInfo.IsHardware)
                            type = _config.ModuleConfiguration.SourceList.Where(t => t.Name == resource.ResourceInfo.Name && t.Type == resource.ResourceInfo.Type).FirstOrDefault();
                        else
                            type = category.Value.Type;

                        if (type != null)
                            node = new SourceWindow(resource) { SourceType = type };


                        if (node != null)
                        {
                            if ((category.Value.Global && !resource.IsLocal) || (!category.Value.Global && resource.IsLocal))
                            {
                                category.Value.Resources.Add(node);
                                resourceNodes.Add(resource, node);
                            }


                            if (resource.ResourceInfo != null && resource.ResourceInfo.IsHardware && type != null)
                            {
                                node.IsOnline = ShowClient.Instance.IsOnLine(type);
                            }
                        }

                    }
                    else
                    {

                        if (resource.ResourceInfo is INonVisibleResource)
                        {
                            nonVisibleResources.Add(resource);
                        }

                    }
                }

                _view.AddSourceCategory(category.Value, category.Value.Global);
            }

            _view.SelectFirstGlobalSource();
            UndoService.Instance.OnHistoryChanged += new HistoryChanged(Instance_OnHistoryChanged);
            DesignerClient.Instance.PresentationNotifier.OnResourceAdded += new EventHandler<NotifierEventArg<ResourceDescriptor>>(PresentationNotifier_OnResourceAdded);
            DesignerClient.Instance.PresentationNotifier.OnResourceDeleted += new EventHandler<NotifierEventArg<ResourceDescriptor>>(PresentationNotifier_OnResourceDeleted);
            //DesignerClient.Instance.PresentationNotifier.OnObjectChanged += new EventHandler<NotifierEventArg<IList<ObjectInfo>>>(PresentationNotifier_OnObjectChanged);
            PresentationController.Instance.OnSelectedResourceChanged += new SelectedResourceChanged(Instance_OnSelectedResourceChanged);
            PresentationController.Instance.OnPresentationChangedExternally += new PresentationDataChanged(Instance_OnPresentationChangedExternally);
            PresentationController.Instance.OnHardwareStateChanged += new Action<EquipmentType, bool?>(Instance_OnHardwareStateChanged);
            PresentationController.Instance.OnSlideSelectionChanged += new SlideSelectionChanged(Instance_OnSlideSelectionChanged);
        }


        void Instance_OnSlideSelectionChanged(IEnumerable<Slide> NewSelection)
        {
            Slide slide = NewSelection.FirstOrDefault();
            if (slide != null)
            {
                try
                {
                    slide.InitReference(_config.ModuleConfiguration, resourceNodes.Keys.ToArray(), GetDeviceResources().ToArray());
                }
                catch
                {
                    MessageBoxAdv.Show("Ошибка при загрузке ресурсов для сцены");
                }
            }
        }

        void Instance_OnHardwareStateChanged(EquipmentType arg1, bool? arg2)
        {
            foreach (var node in resourceNodes)
            {
                if (node.Key.ResourceInfo.IsHardware && node.Value.SourceType.Equals(arg1))//node.Key.ResourceInfo.Type == arg1.Type)
                {
                    MethodInvoker mi = new MethodInvoker(() =>
                    {
                        _view.OnHardwareStateChanged(node.Value, arg2);
                    });

                    if (_view.IsHandleCreated)
                        _view.Invoke(mi);
                    else
                        mi.Invoke();
                }
            }
        }

        public static SourcesController CreateSourceController(SourcesControl view, PresentationInfo presentation)
        {
            return new SourcesController(view, presentation);
        }
        #endregion

        #region Handlers
        void Instance_OnPresentationChangedExternally()
        {
            //nop 
        }

        void Instance_OnSelectedResourceChanged(SourceWindow node)
        {
            if (node != null)
            {
                //update service reference
                serviceContainer = new ServiceProvidingContainer();
                serviceContainer.Add(new ResourceProvider(this));
                foreach (var r in resourceNodes.Keys)
                {
                    serviceContainer.Add(r);
                }

                serviceContainer.Add(node.Window.Source.ResourceDescriptor);
                serviceContainer.Add(node.Window.Source);
            }
        }

        void PresentationNotifier_OnResourceAdded(object sender, NotifierEventArg<ResourceDescriptor> e)
        {
            if (resourceNodes.Keys.Any(r => ResourceDescriptorEquals(e.Data, r))) return;
            if (localResourceCopying) return;

            if (!e.Data.IsLocal || (e.Data.IsLocal && e.Data.PresentationUniqueName == m_presentation.UniqueName))
            {
                ResourceDescriptor rd = e.Data;

                if (!(rd is BackgroundImageDescriptor) && !(rd is INonVisibleResource))
                {
                    SourceCategory cat = (rd.IsLocal ? local_categories : global_categories)[rd.ResourceInfo.Type];
                    ISourceNode node = new SourceWindow(rd) { SourceType = cat.Type };
                    cat.Resources.Add(node);
                    _view.AddResourceToCategory(cat, node, !rd.IsLocal);
                    resourceNodes.Add(rd, node);
                }
                else
                {
                    if (rd.ResourceInfo is INonVisibleResource)
                    {
                        nonVisibleResources.Add(rd);
                    }
                }
            }
        }

        void PresentationNotifier_OnResourceDeleted(object sender, NotifierEventArg<ResourceDescriptor> e)
        {
            if ((nodeToRemove != null && nodeToRemove.Mapping != e.Data) || nodeToRemove == null)
            {
                if (resourceNodes.Keys.Any(k => k.Equals(e.Data)))
                {
                    foreach (var n in resourceNodes)
                    {
                        if (n.Key.Equals(e.Data))
                        {
                            bool Global = !e.Data.IsLocal;

                            ISourceNode node = n.Value;
                            node.Mapping.Removed = true;
                            _view.RemoveSourceFromCategory(Global ? global_categories[node.Mapping.ResourceInfo.Type] : local_categories[node.Mapping.ResourceInfo.Type], node, Global);

                            if (serviceContainer != null)
                                serviceContainer.Remove(node.Mapping);

                            resourceNodes.Remove(node.Mapping);
                            nodeToRemove = null;

                            break;
                        }
                    }
                }
            }
        }

        void Instance_OnHistoryChanged(object Target)
        {
            ResourceInfo info = Target as ResourceInfo;
            if (info != null)
            {
                var rd = resourceNodes.Where(res => res.Key.ResourceInfo == info).Select(res => res.Key);
                if (rd.Count() > 0)
                    _view.RefreshSourceInfo(new List<ISourceNode> { resourceNodes[rd.First()] });
            }
        }

        void _resourceCRUD_OnPartTransmit(object sender, PartSendEventArgs e)
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

        //FileSaveStatus lastSaveStatus = FileSaveStatus.Ok;
        ResourceDescriptor lastResourceDescr = null;

        Thread createSourceThread;

        public void CreateSourceAsync(ResourceDescriptor r)
        {
            createSourceThread = new Thread(new ThreadStart(new MethodInvoker(() =>
            {
                string otherResourceId;
                lastResourceDescr = r;
                ClientResourceCRUDForSave.CreateSource(r, out otherResourceId);
            })));

            createSourceThread.SetApartmentState(ApartmentState.STA);
            createSourceThread.Start();
        }

        void clientResourceCRUDForGet_OnComplete(object sender, OperationStatusEventArgs<ResourceDescriptor> e)
        {
            CloseSplash();
            FileSaveStatus status = e.Status;

            if (status == FileSaveStatus.Abort)
            {
                MessageBoxExt.Show("Передача ресурса прервана", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void _resourceCRUD_OnComplete(object sender, OperationStatusEventArgs<ResourceDescriptor> e)
        {
            CloseSplash();
            FileSaveStatus status = e.Status;
            lastResourceDescr = e.Resource;
            if (status == FileSaveStatus.Exists || status == FileSaveStatus.ExistsWithSameName)
            {
                ResourceInfo resource = lastResourceDescr.ResourceInfo;
                lastResourceDescr.Created = false;

                DialogResult r = DialogResult.Yes;
                PresentationDesignerForm.ActiveForm.Invoke(new MethodInvoker(() =>
                    {
                        r = ResourceExistsDialog.Show(resource.Name);
                    }));

                if (r == DialogResult.Cancel)
                    return;

                if (r == DialogResult.Yes)
                {
                    string newName = resource.Name = "Копия " + resource.Name;
                    string name = newName;
                    int itCount = 1;
                    while (true)
                    {
                        resource.Name = newName;
                        lastResourceDescr.ResourceInfo = resource;
                        if (!FindDuplicate(lastResourceDescr)) break;
                        newName = String.Format("{0}_{1}", name, itCount);
                        itCount++;
                        if (itCount == Int32.MaxValue - 1)
                            throw new OverflowException("Слишком много имен");
                    }

                    CreateSourceAsync(lastResourceDescr);
                }
                else
                {
                    //заменить ресурс
                    // если мы заменяем ресурс, то у него может поменяться айдишник на тот - который есть на сервере
                    // это может оказаться важным
                    // если ресурс заменяем, то ему нужно присвоить айдишник существующего ресурса
                    if (!string.IsNullOrEmpty(e.OtherResourceId))
                    {
                        resource.Id = e.OtherResourceId;
                    }
                    string otherResourceId;
                    ClientResourceCRUDForSave.SaveSource(lastResourceDescr, out otherResourceId);
                }
            }

            if (status == FileSaveStatus.Ok && lastResourceDescr != null)
            {
                SourceCategory cat = (lastResourceDescr.IsLocal ? local_categories : global_categories)[lastResourceDescr.ResourceInfo.Type];
                SourceWindow node = new SourceWindow(lastResourceDescr) { SourceType = cat.Type };
                if (!resourceNodes.ContainsKey(lastResourceDescr))
                {
                    //  resourceNodes[lastResourceDescr] = node;
                    //else
                    resourceNodes.Add(lastResourceDescr, node);
                    _view.AddResourceToCategory(cat, node, !lastResourceDescr.IsLocal);
                }

                lastResourceDescr = null;
            }

            if (status == FileSaveStatus.LoadInProgress)
            {
                return;
            }

            if (status == FileSaveStatus.Abort)
            {
                MessageBoxExt.Show("Передача ресурса прервана", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void _resourceCRUD_OnTerminate(object sender, EventArgs e)
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

        void splash_OnCancel(object sender, EventArgs e)
        {
            if (ClientResourceCRUDForSave != null)
                ClientResourceCRUDForSave.Terminate();
            if (ClientResourceCRUDForGet != null)
            {
                ClientResourceCRUDForGet.Terminate();
            }
        }

        #endregion

        #region commands
        public void PreviewSource(ISourceNode node)
        {
            IDesignerModule module = DesignerClient.Instance.GetDesignerModule(node.SourceType.GetType());
            String file = String.Empty;
            if (ClientResourceCRUDForGet.GetSource(node.Mapping, true))
                file = DesignerClient.Instance.SourceDAL.GetResourceFileName(node.Mapping);
            if (!String.IsNullOrEmpty(file) && module != null && node.Mapping.ResourceInfo is ResourceFileInfo)
            {
                try
                {
                    string newName = Path.ChangeExtension(file, Path.GetExtension((node.Mapping.ResourceInfo as ResourceFileInfo).MasterResourceProperty.ResourceFileName));   //ResourceFullFileName
                    // Удаляем существующую копию, иначе File.Move вылетит
                    try
                    {
                        if (File.Exists(newName)) File.Delete(newName);
                    }
                    catch (IOException ex)
                    {
                        _config.EventLog.WriteError(ex.ToString());
                        throw;
                    }
                    File.Copy(file, newName);
                    //// если автономный режим, то надо файл копировать
                    //if (DesignerClient.Instance.IsStandAlone)
                    //else
                    //    File.Move(file, newName);
                    module.Preview(newName);
                }
                catch (FileNotFoundException /*ex*/)
                {
                    MessageBoxExt.Show(
                        String.Format("Просмотр источника {0} невозможен: в хранилище нет файла", node.Mapping.ResourceInfo.Name),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBoxExt.Show(ex.Message, "Ошибка просмотра", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void CreateSourceInstance(string categoryName, bool Global)
        {
            var categories = (!Global ? local_categories : global_categories);
            SourceCategory cat = categories[categoryName];

            int id = FindUniqueId(cat, Global);

            ResourceInfo resource = cat.Type.CreateNewResourceInfoNumbered(id);
            ResourceFileInfo fileResource = resource as ResourceFileInfo;
            ResourceDescriptor descriptor = new ResourceDescriptor(!Global, Global ? String.Empty : m_presentation.UniqueName, resource) { Created = true };

            //указать файл для файлового ресурса и он не поддерживает доп.свойств
            if (fileResource != null && !(fileResource is ISupportCustomSaveState))
            {
                string result = ResourceFileInfoEditor.ShowFileDialog(fileResource);
                if (result == null)
                {
                    return; //ресурс не создан!
                }
                else
                {
                    try
                    {
                        fileResource.SetMasterResource(result);
                    }
                    catch (Exception ex)
                    {
                        TargetInvocationException targetInvocationException = ex as TargetInvocationException;
                        InvalidResourceException invalidResourceException;
                        if (targetInvocationException != null &&
                            targetInvocationException.InnerException != null)
                        {
                            invalidResourceException = targetInvocationException.InnerException as InvalidResourceException;
                        }
                        else
                        {
                            invalidResourceException = ex as InvalidResourceException;
                        }
                        if (invalidResourceException != null)
                        {
                            MessageBoxExt.Show(invalidResourceException.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        InvalidParameterException invalidParameterException;
                        if (targetInvocationException != null &&
                            targetInvocationException.InnerException != null)
                        {
                            invalidParameterException = targetInvocationException.InnerException as InvalidParameterException;
                        }
                        else
                        {
                            invalidParameterException = ex as InvalidParameterException;
                        }
                        if (invalidParameterException != null)
                        {
                            MessageBoxExt.Show(invalidParameterException.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        throw;
                    }
                }
            }
            else
            {
                //открыть окно свойств для не-файлового ресурса
                using (SourcePropertiesForm sf = new SourcePropertiesForm(descriptor, true))
                {
                    if (sf.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

            if (resource != null)
            {
                CreateSourceAsync(descriptor);
            }
        }

        int FindUniqueId(SourceCategory cat, bool Global)
        {
            int id = 1;
            while (cat.Resources.Any(r => r.Mapping.ResourceInfo.Name == cat.Type.CreateNewResourceInfoNumbered(id).Name)) ++id;
            return id;
        }

        public void RemoveResource(ISourceNode node, bool Global)
        {
            nodeToRemove = node;
            //проверим, не использован ли источник в несохраненном сценарии
            // сначала надо проверять локально потом на сервере
            //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1041
            bool weContains = PresentationController.Instance.Presentation.SlideList.Any(s => s.SourceList.Any(n => n.ResourceDescriptor.ResourceInfo.Equals(node.Mapping.ResourceInfo)));

            RemoveResult removeResult = RemoveResult.LinkedToPresentation;
            if (!weContains)
            {
                removeResult =
                    DesignerClient.Instance.PresentationWorker.DeleteSource(node.Mapping);
            }

            if (RemoveResult.Ok == removeResult && !weContains)
            {
                node.Mapping.Removed = true;
                _view.RemoveSourceFromCategory(Global ? global_categories[node.Mapping.ResourceInfo.Type] : local_categories[node.Mapping.ResourceInfo.Type], node, Global);

                if (serviceContainer != null)
                    serviceContainer.Remove(node.Mapping);

                //if (Global)
                //{
                resourceNodes.Remove(node.Mapping);
                //}
                //else
                //    PresentationController.Instance.PresentationChanged = true;
                nodeToRemove = null;
            }
            else
            {
                string message = (removeResult == RemoveResult.LinkedToPresentation || weContains)
                                     ?
                                         "Удаление источника невозможно. Он используется в сценариях"
                                     :
                                         "Удаление источника невозможно. Он уже удален";
                MessageBoxExt.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CopySourceToGlobal(ISourceNode node)
        {
            MakeResourceCopy(node, false);
        }

        public void CopySourceToPresentation(ISourceNode node)
        {
            MakeResourceCopy(node, true);
        }

        #endregion

        #region helpers

        public IEnumerable<DeviceResourceDescriptor> GetDeviceResources()
        {
            return from t in DesignerClient.Instance.PresentationWorker.GetGlobalDeviceSources() from res in t.Value select res;
        }

        public IEnumerable<ResourceDescriptor> GetResources(bool IsLocal)
        {
            var res = resourceNodes.Where(r => r.Key.IsLocal == IsLocal);
            if (res.Count() > 0)
            {
                return res.Select(r => r.Key);
            }
            return new List<ResourceDescriptor>();
        }

        bool FindDuplicate(ResourceDescriptor descriptor)
        {
            bool result = resourceNodes.Any(r => ResourceDescriptorEquals(r.Key, descriptor) && !r.Key.Removed);
            return result;
        }

        bool ResourceDescriptorEquals(ResourceDescriptor rd1, ResourceDescriptor rd2)
        {
            return rd1.IsLocal == rd2.IsLocal &&
                   ResourceInfoEquals(rd1.ResourceInfo, rd2.ResourceInfo) &&
                   (!rd1.IsLocal || rd1.PresentationUniqueName.Equals(rd2.PresentationUniqueName, StringComparison.InvariantCultureIgnoreCase));
        }

        bool ResourceInfoEquals(ResourceInfo ri1, ResourceInfo ri2)
        {
            return ri1.Name == ri2.Name && ri1.Type == ri2.Type;
        }

        void MakeResourceCopy(ISourceNode node, bool Local)
        {
            localResourceCopying = true;
            try
            {
                ResourceDescriptor descriptor = null;
                if (Local)
                    descriptor = DesignerClient.Instance.PresentationWorker.CopySourceFromGlobalToLocal(node.Mapping, m_presentation.UniqueName);
                else
                {
                    bool isUnique = true;
                    foreach (var resource in resourceNodes)
                    {
                        if (!resource.Key.IsLocal && ResourceInfoEquals(resource.Key.ResourceInfo, node.Mapping.ResourceInfo))
                        {
                            isUnique = false;
                            break;
                        }
                    }

                    if (isUnique)
                    {
                        //SourcesController.Instance.SavePresentationSources();
                        descriptor = DesignerClient.Instance.PresentationWorker.CopySourceFromLocalToGlobal(node.Mapping);
                    }
                }

                if (descriptor != null)
                {
                    descriptor.Created = true;
                    ResourceInfo resource = descriptor.ResourceInfo;
                    SourceWindow clone = new SourceWindow(descriptor) { SourceType = node.SourceType };
                    resourceNodes.Add(descriptor, clone);
                    _view.AddResourceToCategory(Local ? local_categories[resource.Type] : global_categories[resource.Type], clone, !Local);

                }
                else
                {
                    StringBuilder sb = new StringBuilder("Источник уже добавлен к ");

                    if (Local) sb.Append("источникам сценария"); else sb.Append("общим источникам");
                    MessageBoxExt.Show(sb.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                localResourceCopying = false;
            }
        }

        public ResourceDescriptor[] GetResourcesByType(string Type, bool checkMapping)
        {
            ResourceDescriptor[] result = resourceNodes.Where(rd => rd.Key.ResourceInfo.Type == Type).Select(r => r.Key).ToArray();
            if (checkMapping && DisplayController.Instance != null && DisplayController.Instance.CurrentDisplay != null && LayoutController.Instance != null)
            {
                result = DisplayController.Instance.CurrentDisplay.ApplyAdditionalFilter(result, LayoutController.Instance.SelectedWindow == null ? null : LayoutController.Instance.SelectedWindow.Window);
            }
            if (result != null && result.Length > 0) return result;
            return nonVisibleResources.Where(r => r.ResourceInfo.Type == Type).ToArray();
        }

        public Device GetDeviceByName(DeviceType deviceType)
        {
            Device device = PresentationController.Instance.SelectedSlide.DeviceList.Where(s => s.Name == deviceType.Name).FirstOrDefault();
            if (device != null) return device;

            device = deviceType.CreateNewDevice();
            PresentationController.Instance.SelectedSlide.DeviceList.Add(device);
            return device;
        }

        public SourceCategory GetSourceCategory(string text, bool IsLocal)
        {
            var cats = IsLocal ? local_categories : global_categories;
            if (cats.ContainsKey(text))
                return cats[text];
            else
                return null;
        }

        #endregion

        #region save

        public void SaveSource(ResourceDescriptor r)
        {
            string otherResourceId;
            ClientResourceCRUDForSave.SaveSource(r, out otherResourceId);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                UndoService.Instance.OnHistoryChanged -= new HistoryChanged(Instance_OnHistoryChanged);
                DesignerClient.Instance.PresentationNotifier.OnResourceAdded -= new EventHandler<NotifierEventArg<ResourceDescriptor>>(PresentationNotifier_OnResourceAdded);
                PresentationController.Instance.OnSelectedResourceChanged -= new SelectedResourceChanged(Instance_OnSelectedResourceChanged);
                PresentationController.Instance.OnPresentationChangedExternally -= new PresentationDataChanged(Instance_OnPresentationChangedExternally);
                PresentationController.Instance.OnHardwareStateChanged -= new Action<EquipmentType, bool?>(Instance_OnHardwareStateChanged);
                PresentationController.Instance.OnSlideSelectionChanged -= new SlideSelectionChanged(Instance_OnSlideSelectionChanged);
            }
        }

        #endregion

    }
}
