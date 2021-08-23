using System;
using System.Collections.Generic;
using Test.Interface;


namespace Test.GameServices
{
    public class UnitsHolderService : Service
    {
        private Dictionary<int, IUnit> _unitsByID = new Dictionary<int, IUnit>();

        public event Action OnCreatedUnitEvent;
        public event Action OnDestroyUnitEvent;
        public Dictionary<int, IUnit> UnitsDictionary
        {
            get
            {
                return _unitsByID;
            }
        }

        public void OnCreatedUnit(IUnit unit)
        {
            var unitID = unit.InstanceID;

            if (!_unitsByID.ContainsKey(unitID))
                _unitsByID.Add(unitID, unit);
            else
                _unitsByID[unitID] = unit;
            
            OnCreatedUnitEvent?.Invoke();

        }

        public void OnDestroyUnit(IUnit unit)
        {
            if (_unitsByID.ContainsKey(unit.InstanceID))
            {
                _unitsByID.Remove(unit.InstanceID);
                OnDestroyUnitEvent?.Invoke();
            }
        }
    }

}
