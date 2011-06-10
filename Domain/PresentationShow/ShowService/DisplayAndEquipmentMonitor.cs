using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Interfaces.ConfigModule.Server;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace Domain.PresentationShow.ShowService
{
    internal class DisplayAndEquipmentMonitor : IDisposable
    {
        private readonly IControllerChannel _controller;
        private readonly IConfiguration _config;
        private readonly Dictionary<Type, IModule> mappingList = new Dictionary<Type, IModule>();
        private readonly List<DisplayType> activeDisplayMappingList = new List<DisplayType>();
        private readonly object _activeDisplaySync;
        private readonly Dictionary<int, EquipmentType> uidMapping = new Dictionary<int, EquipmentType>();


        public DisplayAndEquipmentMonitor(IControllerChannel controller, IConfiguration config)
        {
            _controller = controller;
            _config = config;
            _activeDisplaySync = ((ICollection) activeDisplayMappingList).SyncRoot;
            foreach (DisplayType displayType in _config.ModuleConfiguration.DisplayList)
            {
                if (!displayType.IsHardware)
                {
                    if (!activeDisplayMappingList.Contains(displayType))
                        activeDisplayMappingList.Add(displayType);
                }
            }
            // для всех дисплейных модулей подписка на событие изменения состояния
            foreach (IModule module in _config.ModuleList)
            {
                module.ServerModule.OnStateChange += new EventHandler<EqiupmentStateChangeEventArgs>(ServerModule_OnStateChange);
                // маппинг
                foreach (Type deviceType in module.SystemModule.Configuration.GetDevice())
                    mappingList[deviceType] = module;
                foreach (Type displayType in module.SystemModule.Configuration.GetDisplay())
                    mappingList[displayType] = module;
                foreach (Type sourceType in module.SystemModule.Configuration.GetSource())
                    mappingList[sourceType] = module;
            }
            IEnumerable<EquipmentType> equipmentTypes = _config.ModuleConfiguration.DeviceList.Cast<EquipmentType>().
                Union(_config.ModuleConfiguration.DisplayList.Cast<EquipmentType>()).
                Union(_config.ModuleConfiguration.SourceList.Cast<EquipmentType>()).
                Where(et => et.IsHardware && et.UID >= 0);
            foreach (EquipmentType equipmentType in equipmentTypes)
            {
                uidMapping[equipmentType.UID] = equipmentType;
            }
            _controller.OnStatusChange += new EventHandler<DeviceStatusChangeEventArgs>(_controller_OnStatusChange);
        }

        public bool IsOnLine(EquipmentType equipmentType)
        {
            if (equipmentType.UID == Constants.ControllerUID)
                //опрос контроллера
                return IsControllerOnLine;
            IModule module;
            if (mappingList.TryGetValue(equipmentType.GetType(), out module))
            {
                return module.ServerModule.IsOnLine(equipmentType);
            }
            return false;
        }

        public bool IsControllerOnLine
        {
            get { return _controller.IsControllerOnLine; }
        }

        public int GetActiveDisplayNumber()
        {
            lock (_activeDisplaySync)
            {
                int count = 0;
                foreach (DisplayType displayType in activeDisplayMappingList)
                {
                    if (IsOnLine(displayType)) count++;
                }
                return count;
            }
        }

        public event EventHandler<EqiupmentStateChangeEventArgs> OnStateChange;

        #region private

        void _controller_OnStatusChange(object sender, DeviceStatusChangeEventArgs e)
        {
            EquipmentType equipmentType;
            if (e.UID == Constants.ControllerUID && !e.IsOnLine)
            {
                foreach (KeyValuePair<int, EquipmentType> valuePair in uidMapping)
                {
                    StateChange(sender, new EqiupmentStateChangeEventArgs(valuePair.Value, false));
                }
                return;
            }
            if (uidMapping.TryGetValue(e.UID, out equipmentType))
            {
                StateChange(sender, new EqiupmentStateChangeEventArgs(equipmentType, e.IsOnLine));
            }
        }

        private void ServerModule_OnStateChange(object sender, EqiupmentStateChangeEventArgs e)
        {
            StateChange(sender, e);
        }

        private void StateChange(object sender, EqiupmentStateChangeEventArgs e)
        {
            if (OnStateChange != null)
            {
                OnStateChange(sender, e);
            }
        }

        #endregion

        public void Dispose()
        {
        }
    }
}
