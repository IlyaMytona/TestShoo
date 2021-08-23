using Test.Enum;
using UnityEngine;


namespace Test 
{
    public abstract class DamagedObject : MonoBehaviour
    {
        #region Properties

        public Fraction fraction {get; protected set;}        
        public int instanceID { get; protected set; }

        #endregion


        #region Unity Methods

        protected virtual void Awake()
        {
            instanceID = GetInstanceID();            
        }

        protected virtual void OnDestroy()
        {

        }

        #endregion


        #region Methods
        public bool IsEqual(DamagedObject other)
        {
            if (other != null)
                return this.instanceID == other.instanceID;
            else
                return false;
        }
        public abstract void ReceiveDamage(string jsonStr);
        public abstract void TakeDamage(GameObject user, int damage);

        #endregion
    }
}

