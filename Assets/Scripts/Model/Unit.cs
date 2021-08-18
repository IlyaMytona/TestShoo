using System;
using Test.Data;
using Test.Enum;
using Test.Interface;
using UnityEngine;


namespace Test.Model
{
    public class Unit : MonoBehaviour, IUnit
    {
        #region Private Data

        private IPlayerMovement _movement;
        protected bool _isDie;

        #endregion
        

        #region Fields

        public event Action EventOnDie;
        public event Action EventOnRevive;
        
        #endregion


        #region Properties

        public IPlayerMovement mover { get { return _movement;} set { _movement = value; }}
        public Fraction fraction { get; protected set; }
        public int InstanceID { get; protected set; }

        #endregion


        #region Unity Methods

        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
        }

        #endregion


        #region Methods
        protected virtual void Die()
        {
            _isDie = true;
            EventOnDie?.Invoke();            
        }

        protected virtual void Revive()
        {
            _isDie = false;
            EventOnRevive?.Invoke();            
        }

        #endregion
    }
}
