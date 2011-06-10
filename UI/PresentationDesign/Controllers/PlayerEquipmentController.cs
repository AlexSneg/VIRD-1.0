using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using Domain.PresentationShow.ShowClient;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Entity;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public class PlayerEquipmentController : IDisposable
    {
        private static PlayerEquipmentController _instance = null;
        private List<ValueThree<Device, bool, FreezeStatus>> _devices = new List<ValueThree<Device, bool, FreezeStatus>>();
        private Slide _selectedSlide = null;

        public event Action<string, bool?> OnDeviceStateChanged;
        public event Action<string, FreezeStatus> OnEquipmentFreezeChanged;
        public event Action OnDeviceListChanged;

        public static void CreateController()
        {
            _instance = new PlayerEquipmentController();
        }

        public static PlayerEquipmentController Instance
        {
            get { return _instance; }
        }
        public List<ValueThree<Device, bool, FreezeStatus>> Items
        {
            get { return _devices; }
        }

        public void ChangeSelectedItem(Device item)
        {
            PresentationController.Instance.NotifyCurrentDeviceChanged(item);
        }

        private PlayerEquipmentController()
        {
            fillDeviceItems();
            ShowClient.Instance.OnEquipmentStateChanged += Instance_OnEquipmentStateChanged;
            ShowClient.Instance.OnEquipmentFreezeChanged += Instance_OnEquipmentFreezeChanged;
            PresentationController.Instance.OnSlideSelectionChanged += Instance_OnSlideSelectionChanged;
            PresentationController.Instance.OnPlaySelectionChanged += Instance_OnPlaySelectionChanged;
        }
        
        private void fillDeviceItems()
        {
            _devices.Clear();
            foreach (DeviceType devType in PresentationController.Configuration.ModuleConfiguration.DeviceList.Where(dT => dT.Visible))
            {
                Device dev;
                bool isSelected;
                if ((_selectedSlide != null) && 
                    (_selectedSlide.DeviceList.Find(d => d.Type.Equals(devType)) != null))
                {
                    dev = _selectedSlide.DeviceList.Find(d => d.Type.Equals(devType));
                    isSelected = true;
                }
                else
                {
                    dev = devType.CreateNewDevice();
                    isSelected = false;
                }
                FreezeStatus fStatus = ShowClient.Instance.GetFreezedEquipment(devType);
                _devices.Add(new ValueThree<Device, bool, FreezeStatus>(dev, isSelected, fStatus));
            }
            _devices.Sort((d1,d2) =>{ return d1.Value1.Type.Name.CompareTo(d2.Value1.Type.Name); });
            
            if (OnDeviceListChanged != null)
                OnDeviceListChanged();

            foreach (ValueThree<Device, bool, FreezeStatus> d in _devices)
                //if (!ShowClient.Instance.IsOnLine(d.Value1.Type))
                Instance_OnEquipmentStateChanged(d.Value1.Type, ShowClient.Instance.IsOnLine(d.Value1.Type)); //false
        }

        private void Instance_OnSlideSelectionChanged(IEnumerable<Slide> NewSelection)
        {
            if (PlayerController.Instance.CanPlay)
                return;
            _selectedSlide = NewSelection.FirstOrDefault();
            fillDeviceItems();
        }

        private void Instance_OnPlaySelectionChanged(Slide slide)
        {
            if (!PlayerController.Instance.CanPlay)
                return;
            _selectedSlide = slide;
            fillDeviceItems();
        }

        private void Instance_OnEquipmentStateChanged(EquipmentType equipmentType, bool? isOnLine)
        {
            // https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1046
            String device = PresentationController.Configuration.ModuleConfiguration.DeviceList.Where(x => x.Equals(equipmentType)).Select(x => x.Name).FirstOrDefault();
            if (device == null)
                return;
            if (OnDeviceStateChanged != null)
                OnDeviceStateChanged(device, isOnLine);
        }

        private void Instance_OnEquipmentFreezeChanged(EquipmentType equipmentType, FreezeStatus state)
        {
            String device = PresentationController.Configuration.ModuleConfiguration.DeviceList.Where(x => x.Equals(equipmentType)).Select(x => x.Name).FirstOrDefault();
            if (device == null)
                return;
            if (OnEquipmentFreezeChanged != null)
            {
                OnEquipmentFreezeChanged(equipmentType.Name, state);
            }
        }

        public void Dispose()
        {
            if (ShowClient.Instance != null)
            {
                ShowClient.Instance.OnEquipmentStateChanged -= Instance_OnEquipmentStateChanged;
                ShowClient.Instance.OnEquipmentFreezeChanged -= Instance_OnEquipmentFreezeChanged;
            }
            if (PresentationController.Instance != null)
            {
                PresentationController.Instance.OnSlideSelectionChanged -= Instance_OnSlideSelectionChanged;
                PresentationController.Instance.OnPlaySelectionChanged -= Instance_OnPlaySelectionChanged;
            }
            _instance = null;
        }
    }
}
