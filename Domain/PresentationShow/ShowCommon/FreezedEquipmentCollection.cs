using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace Domain.PresentationShow.ShowCommon
{
    public class FreezedEquipmentCollection : IEnumerable<EquipmentType>
    {
        private readonly HashSet<EquipmentType> _freezedEquipment = new HashSet<EquipmentType>();
        private bool _isInit = false;
        
        public void Init(EquipmentType[] equipmentTypes)
        {
            foreach (EquipmentType item in equipmentTypes)
                _freezedEquipment.Add(item);
            _isInit = true;
        }
        public void Reset()
        {
            Clear();
            _isInit = false;
        }
        public void Set(EquipmentType equipmentType, FreezeStatus status)
        {
            if (!equipmentType.IsHardware) return;
            if (FreezeStatus.UnFreeze == status)
                _freezedEquipment.Remove(equipmentType);
            else
                _freezedEquipment.Add(equipmentType);
        }

        public EquipmentType[] GetFreezedEquipment()
        {
            return _freezedEquipment.ToArray();
        }

        public FreezeStatus Exists(EquipmentType equipmentType)
        {
            if (_freezedEquipment.Contains(equipmentType))
                return FreezeStatus.Freeze;
            else
                return FreezeStatus.UnFreeze;
        }

        public void Clear()
        {
            _freezedEquipment.Clear();
        }

        public bool IsInit { get { return _isInit; } }

        #region Implementation of IEnumerable

        public IEnumerator<EquipmentType> GetEnumerator()
        {
            return _freezedEquipment.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
