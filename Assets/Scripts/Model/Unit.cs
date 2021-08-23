using System;
using Test.Enum;
using Test.Interface;
using UnityEngine;


namespace Test.Model
{
    public class Unit : MonoBehaviour, IUnit
    {
        #region Private Data

        protected bool _isDie;
        protected float _maxHp;        

        #endregion


        #region Fields

        public event Action EventOnDie;
        public event Action EventOnRevive;

        #endregion


        #region Properties

        public float MaxHp => _maxHp;
        public Fraction Fraction { get; protected set; }
        public int InstanceID { get; protected set; }
        public Transform Transform { get; protected set; }
        public bool IsVisible
        {
            set
            {
                Transform = GetComponent<Transform>();
                foreach (Transform d in Transform)
                {
                    var tempRenderer = d.GetComponent<Renderer>();
                    if (tempRenderer)
                    {
                        tempRenderer.enabled = value;
                    }
                }
            }
        }

        #endregion


        #region Unity Methods

        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();            
        }

        #endregion


        #region Methods
        public virtual void Die()
        {
            _isDie = true;
            EventOnDie?.Invoke();            
        }

        public virtual void Revive()
        {
            _isDie = false;
            EventOnRevive?.Invoke();            
        }

        #endregion
    }
}
