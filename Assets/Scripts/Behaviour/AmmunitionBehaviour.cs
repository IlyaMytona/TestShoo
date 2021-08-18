using Test.Enum;
using UnityEngine;


namespace Test.Behaviour
{
    public sealed class AmmunitionBehaviour : MonoBehaviour
    {
        public AmmunitionSoData AmmunitionSoData;                

        private void Start()
        {
            AmmunitionSoData.CurDamage = AmmunitionSoData.BaseDamage;            
        }        
    }
}
