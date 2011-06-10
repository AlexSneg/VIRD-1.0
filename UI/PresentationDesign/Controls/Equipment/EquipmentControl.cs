using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TechnicalServices.Persistence.CommonPersistence.Resource;
using UI.PresentationDesign.DesignUI.Controllers;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Controls.Equipment;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Common.Utils;
using System.Collections.Specialized;
using Domain.PresentationDesign.Client;
using TechnicalServices.Persistence.SystemPersistence.Resource;

using Syncfusion.Windows.Forms.Tools;
using TechnicalServices.Interfaces;
using UI.PresentationDesign.DesignUI.History;
using UI.PresentationDesign.DesignUI.Properties;
using TechnicalServices.Common.Classes;
using UI.PresentationDesign.DesignUI.Services;

namespace UI.PresentationDesign.DesignUI.Controls
{
    /// <summary>
    /// Смена устройства
    /// </summary>
    /// <param name="NewDevice">Новое устройство - может быть Null, если ничего не выбрано</param>
    public delegate void DeviceSelected(Device NewDevice);

    public partial class EquipmentControl : UserControl, IDisposable
    {
        SortedDictionary<DeviceType, DeviceNode> nodesByDevice;
        DeviceNode SelectedNode;
        Slide CurrentSlide;

        Identity devIdentity = new Identity(0);

        public event DeviceSelected OnDeviceSelected;

        public Device SelectedDevice
        {
            get
            {
                if (SelectedNode != null)
                    return SelectedNode.Device;
                return null;
            }
        }

        public EquipmentControl()
        {
            InitializeComponent();

            propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid_PropertyValueChanged);

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.equipmentList.AfterSelect += new EventHandler(equipmentList_Selected);
                UndoService.Instance.OnHistoryChanged += new HistoryChanged(Instance_OnHistoryChanged);
                Init();
            }
            propertyGrid.PropertyGridValidating += new CancelEventHandler(propertyGrid_PropertyGridValidating);
        }

        void Instance_OnHistoryChanged(object Target)
        {
            if (Target is Device)
                propertyGrid.AssignedObject = Target;
            SlideDeviceHistoryCommand entry = Target as SlideDeviceHistoryCommand;
            if (entry != null)
            {
                if (entry.SlideId == CurrentSlide.Id)
                {
                    //refreshStripButton_Click(this, EventArgs.Empty);
                    DeviceNode deviceNode =
                        equipmentList.Nodes.OfType<DeviceNode>().FirstOrDefault(
                            dn => dn.DeviceType.Equals(entry.Device.Type));
                    if (deviceNode != null)
                    {
                        equipmentList.SelectedNode = deviceNode;
                        equipmentList.SelectedNode.Checked = CurrentSlide.DeviceList.Any(dev=>dev.Equals(entry.Device));
                        //if (entry.WasAdded)
                        //{
                        //    equipmentList.SelectedNode = deviceNode;
                        //    propertyGrid.AssignedObject = entry.DeviceNode.Device;
                        //}
                        //else
                        //{
                        //    nodesByDevice.Remove(deviceNode.Device.Type);
                        //    deviceNode.Device =
                        //        deviceNode.Device.Type.CreateNewDevice(
                        //            DesignerClient.Instance.PresentationWorker.GetGlobalDeviceSources());
                        //    nodesByDevice.Add(deviceNode.Device.Type, deviceNode);
                        //    equipmentList.SelectedNode = deviceNode;
                        //}
                        Refresh();
                    }
                }
            }
        }

        void propertyGrid_PropertyGridValidating(object sender, CancelEventArgs e)
        {
            ToolTipInfo t_info = new ToolTipInfo();
            t_info.Body.Image = Resources.error;
            t_info.Header.Text = "Ошибка";

            Point p = this.PointToScreen(propertyGrid.Location);
            p.Offset(-propertyGrid.Width, 0);

            ISupportValidation validation = propertyGrid.SelectedObject as ISupportValidation;
            string message;
            if (validation != null)
            {
                if (!validation.EnsureValidate(out message))
                {
                    t_info.Body.Text = message;
                    superToolTip2.Show(t_info, p);
                    propertyGrid.Focus();
                    throw new Exception();
                }
            }
        }

        void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            object target = propertyGrid.AssignedObject;
            List<PropertyDescriptor> props = new List<PropertyDescriptor>();
            props.Add(e.ChangedItem.PropertyDescriptor);

            if (e.ChangedItem.Parent.Value != null) //nested?
            {
                GridItem parent = e.ChangedItem;
                for (; ; )
                {
                    parent = parent.Parent;
                    if (parent.Value == null)
                        break;

                    props.Add(parent.PropertyDescriptor);
                }
            }
            if (null != e.ChangedItem.PropertyDescriptor.Attributes[typeof (ForceDeviceResourceSaveAttribute)])
            {
                // надо сохранить DeviceResourcedescriptor
                DeviceNode node = equipmentList.SelectedNode as DeviceNode;
                if (node != null && node.Device != null && node.Device.DeviceResourceDescriptor != null)
                {
                    PresentationController.Instance.SaveDeviceResourceDescriptor(node.Device.DeviceResourceDescriptor);
                }

                // UndoService не юзать, так как сохранения уходят на сервер
            }
            else
            {
                UndoService.Instance.PushAction(new PropertyChangedHistoryEntry(target, props, e.OldValue));
            }
            PresentationController.Instance.PresentationChanged = true;
        }

        public void Init()
        {
            EquipmentController.CreateEquipmentController();
            EquipmentController.Instance.Control = this;
            nodesByDevice = new SortedDictionary<DeviceType, DeviceNode>();
            CurrentSlide = PresentationController.Instance.SelectedSlide;

            FillDeviceTypeNodes();
            //EquipmentController.Instance.DeviceTypes.ForEach(AddNodeVoid);

            PresentationController.Instance.OnSlideSelectionChanged += new SlideSelectionChanged(Instance_OnSlideSelectionChanged);
            PresentationController.Instance.OnHardwareStateChanged += new Action<EquipmentType, bool?>(Instance_OnHardwareStateChanged);
            PresentationController.Instance.OnSlideLockChanged += new SlideLockChanged(Instance_OnSlideLockChanged);
            if (equipmentList.Nodes.Count > 0)
            {
                SelectedNode = equipmentList.Nodes[0] as DeviceNode;
                equipmentList.SelectedNode = SelectedNode;
            }
        }

        void Instance_OnSlideLockChanged(Slide slide, bool IsLocked, TechnicalServices.Entity.LockingInfo info)
        {
            if (PresentationController.Instance.SelectedSlide.Id == slide.Id)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    propertyGrid.IsEnabled = IsLocked && PresentationController.Instance.CanUnlockSlide(slide) && (equipmentList.SelectedNode != null && equipmentList.SelectedNode.Checked);
                }));
            }
        }

        void Instance_OnHardwareStateChanged(EquipmentType arg1, bool? arg2)
        {
            if (arg1 is DeviceType && nodesByDevice.Keys.Any(k => k.Equals(arg1)))
            {
                DeviceNode node = nodesByDevice[(DeviceType)arg1];
                this.Invoke(new MethodInvoker(() =>
                {
                    node.TextColor = arg2.HasValue ? (arg2.Value ? Color.Green : Color.Red) : Color.Black;
                }));
            }
        }

        public void SelectDevice(DeviceType dev)
        {
            for (int i = 0; i < equipmentList.Nodes.Count; i++)
                if (equipmentList.Nodes[i].Text == dev.Name)
                {
                    equipmentList.SelectedNode = equipmentList.Nodes[i];
                    return;
                }
        }

        void Instance_OnSlideSelectionChanged(IEnumerable<Slide> NewSelection)
        {
            string currentNodeText = null;
            if(equipmentList.SelectedNode != null) currentNodeText=equipmentList.SelectedNode.Text;
            CurrentSlide = NewSelection.FirstOrDefault();
            nodesByDevice.Clear();
            equipmentList.Nodes.Clear();
            if (CurrentSlide != null)
            {
                FillDeviceTypeNodes();
                //EquipmentController.Instance.DeviceTypes.ForEach(AddNodeVoid);
                if (equipmentList.Nodes.Count > 0)
                {
                    SelectedNode = equipmentList.Nodes[0] as DeviceNode;
                    equipmentList.SelectedNode = SelectedNode;
                }
            }
            else
            {
                propertyGrid.SelectedObject = null;
            }
            
            for (int i = 0; i < equipmentList.Nodes.Count; i++) // Перейдем к тому же оборудованию, что было выделено ранее
            {
                if (equipmentList.Nodes[i].Text == currentNodeText)
                    equipmentList.SelectedNode = equipmentList.Nodes[i];
            }

            return;

            if (NewSelection != null && NewSelection.Count() > 0)
            {
                CurrentSlide = NewSelection.First();
                LoadDeviceProperties(this.SelectedDevice);
            }
            else
            {
                CurrentSlide = null;
                ClearDeviceProperties();
            }
        }

        void AddNodeVoid(DeviceType d)
        {
            AddNewNode(d);
        }

        Device AddNewNode(DeviceType d)
        {
            if (!d.Visible)
                return null;
            DeviceNode node = new DeviceNode(d);
            Device dev;
            if (CurrentSlide.DeviceList.Any(x => x.Type.Equals(d)))
            {
                node.Checked = true;
                dev = CurrentSlide.DeviceList.Find(x => x.Type.Equals(d));
            }
            else
                dev = d.CreateNewDevice(DesignerClient.Instance.PresentationWorker.GetGlobalDeviceSources());
            if (nodesByDevice.ContainsKey(d))
            {
                equipmentList.Nodes.Remove(nodesByDevice[d]);
                nodesByDevice[d] = node;
            }
            else
                nodesByDevice.Add(d, node);
            bool? isOnline = EquipmentController.Instance.IsOnline(dev);
            node.TextColor = isOnline.HasValue ? (isOnline.Value ? Color.Green : Color.Red) : Color.Black;
            node.Device = dev;
            equipmentList.Nodes.Add(node);
            return dev;
        }

        void equipmentList_Selected(object sender, EventArgs e)
        {
            if (equipmentList.SelectedNode != null)
            {
                DeviceNode newNode = equipmentList.SelectedNode as DeviceNode;
                NodeChanging(newNode);
                SelectedNode = newNode;
            }
        }

        private void NodeChanging(DeviceNode NewNode)
        {
            if (OnDeviceSelected != null)
            {
                if (NewNode != null && CurrentSlide != null)
                    OnDeviceSelected(NewNode.Device);
                else
                    OnDeviceSelected(null);
            }

            if (NewNode != null)
            {
                LoadDeviceProperties(NewNode.Device);
            }
        }

        private void LoadDeviceProperties(Device device)
        {
            if (CurrentSlide != null && device != null)
            {
                Device selectedDevice = CurrentSlide.DeviceList.FirstOrDefault(d => d.Type.Name == device.Type.Name);
                if (selectedDevice != null)
                {
                    propertyGrid.IsEnabled = CurrentSlide.IsLocked && PresentationController.Instance.CanUnlockSlide(CurrentSlide);
                    propertyGrid.AssignedObject = selectedDevice;
                }
                else
                {
                    propertyGrid.IsEnabled = false;
                    //создаем новое устройство для данного слайда
                    selectedDevice = device;
                    propertyGrid.AssignedObject = selectedDevice;
                    //PresentationController.Instance.PresentationChanged = true;
                }
            }
            else
                ClearDeviceProperties();
        }


        private void ClearDeviceProperties()
        {
            propertyGrid.AssignedObject = null;
        }


        #region IDisposable Members

        void IDisposable.Dispose()
        {
            PresentationController.Instance.OnSlideSelectionChanged -= new SlideSelectionChanged(Instance_OnSlideSelectionChanged);
            PresentationController.Instance.OnHardwareStateChanged -= new Action<EquipmentType, bool?>(Instance_OnHardwareStateChanged);
            PresentationController.Instance.OnSlideLockChanged -= new SlideLockChanged(Instance_OnSlideLockChanged);
            DesignerClient.Instance.PresentationNotifier.OnDeviceResourceUpdated += PresentationNotifier_OnDeviceResourceUpdated;
        }

        void PresentationNotifier_OnDeviceResourceUpdated(object sender, NotifierEventArg<DeviceResourceDescriptor> e)
        {
            //e.Data.ResourceInfo.DeviceType.Type;
        }

        #endregion

        private void refreshStripButton_Click(object sender, EventArgs e)
        {
            CurrentSlide = PresentationController.Instance.SelectedSlide;
            nodesByDevice.Clear();
            equipmentList.Nodes.Clear();
            FillDeviceTypeNodes();
            SelectedNode = nodesByDevice.First().Value;
            equipmentList.SelectedNode = SelectedNode;

        }

        private void FillDeviceTypeNodes()
        {
            var query = from d in EquipmentController.Instance.DeviceTypes
                        join ord in Domain.PresentationDesign.Client.DesignerClient.Instance.ClientConfiguration.DevicePositions
                        on d.Name equals ord.Key
                        into d_ord
                        from sub in d_ord.DefaultIfEmpty()
                        orderby (sub.Key == null ? -1 : sub.Value), d.Name
                        select d;
            foreach (var v in query)
            {
                AddNodeVoid(v);
            }
            //EquipmentController.Instance.DeviceTypes.ForEach(AddNodeVoid);
        }

        private void equipmentList_AfterCheck(object sender, Syncfusion.Windows.Forms.Tools.TreeNodeAdvEventArgs e)
        {
            DeviceNode node = e.Node as DeviceNode;
            CurrentSlide.DeviceList.RemoveAll(x => x.Type.Equals(node.Device.Type));
            PresentationController.Instance.PresentationChanged = true;
            equipmentList.SelectedNode = null;
            if (node.Checked)
            {
                CurrentSlide.DeviceList.Add(node.Device);
                equipmentList.SelectedNode = node;
            }
            else
            {
                nodesByDevice.Remove(node.Device.Type);
                node.Device = node.Device.Type.CreateNewDevice(DesignerClient.Instance.PresentationWorker.GetGlobalDeviceSources());
                nodesByDevice.Add(node.Device.Type, node);
                equipmentList.SelectedNode = node;
            }
            if (e.Action == TreeViewAdvAction.ByMouse)
                UndoService.Instance.PushAction(
                    new SlideDeviceHistoryCommand(EquipmentController.Instance, CurrentSlide.Id, node.Device, node.Checked));
        }

        private void equipmentList_BeforeCheck(object sender, Syncfusion.Windows.Forms.Tools.TreeNodeAdvBeforeCheckEventArgs e)
        {
            if (!DesignerClient.Instance.IsStandAlone)
                if (!(CurrentSlide.IsLocked && PresentationController.Instance.CanUnlockSlide(CurrentSlide)))
                    e.Cancel = true;
        }

        private void findStripButton_Click(object sender, EventArgs e)
        {
            FindItemController.Instance.ShowSearchForm(ItemToSearch.Device);
        }
    }
}
