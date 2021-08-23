using UnityEngine;


namespace Test.Throw
{
    [System.Serializable]
    public class GrenadeDamage
    {
        public int DamageValue = 50;        
        [HideInInspector] public Transform Sender;
        [HideInInspector] public Transform Receiver;
        [HideInInspector] public Vector3 HitPosition;
        public bool HitReaction = true;

        public GrenadeDamage(GrenadeDamage damage)
        {
            DamageValue = damage.DamageValue;
            Sender = damage.Sender;
            Receiver = damage.Receiver;            
            HitPosition = damage.HitPosition;            
        }        
    }
}