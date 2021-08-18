using System;
using UnityEngine;


namespace Test.Model
{
    public class UnitStats : MonoBehaviour
    {
        #region Fields

        public Action<int> OnHealthChanged;
        public Stat Damage;
        public Stat Armor;
        public Stat MoveSpeed;
        public int HealthMax = 100;

        #endregion


        #region Private Data

        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _curHealth;

        #endregion


        #region Properties

        public virtual int CurHealth { get { return _curHealth; } set { _curHealth = value; } }

        #endregion


        #region Methods
        public virtual void TakeDamage(int damage)
        {
            damage -= Armor.GetValue();
            if (damage > 0)
            {
                CurHealth -= damage;
                OnHealthChanged?.Invoke(_curHealth);
                if (CurHealth <= 0)
                {
                    CurHealth = 0;
                }
            }
        }

        public void AddHealth(int amount)
        {
            CurHealth += amount;
            if (CurHealth > _maxHealth)
            {
                CurHealth = _maxHealth;
            }
        }

        public void SetHealthRate(float rate)
        {

            CurHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
            OnHealthChanged?.Invoke(_curHealth);
        }
        #endregion
    }
}
