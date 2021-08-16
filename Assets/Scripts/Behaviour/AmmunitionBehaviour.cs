using UnityEngine;


namespace Test
{
    public sealed class AmmunitionBehaviour : MonoBehaviour
    {
        public float TimeToDestruct = 5;
        public float BaseDamage = 10;
        public float CurDamage;
        public AmmunitionType Type = AmmunitionType.Bullet;
        
        private void Awake()
        {
            CurDamage = BaseDamage;            
        }        
    }
}
