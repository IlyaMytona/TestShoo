using System;
using UnityEngine;

namespace Test
{
    public class Unit : DamagedObject, IUnit
    {
        #region Private Data

        private IPlayerMovement _movement;
        [SerializeField] protected UnitStats _stats;

        #endregion


        #region Fields

        public event Action EventOnDamage;
        public event Action EventOnDie;
        public event Action EventOnRevive;
        public readonly DamageReceiveData _damageReceiveData = new DamageReceiveData();

        #endregion


        #region Properties

        public IPlayerMovement mover { get { return _movement;} set { _movement = value; }}
        public DamageReceiveData damageReceiveData { get { return _damageReceiveData;}}

        #endregion


        #region Methods

        public override void TakeDamage(GameObject user, int damage)
        {
            _stats.TakeDamage(damage);
            DamageWithCombat(user);
        }

        protected virtual void DamageWithCombat(GameObject user)
        {
            EventOnDamage();
        }

        #endregion
    }

}
