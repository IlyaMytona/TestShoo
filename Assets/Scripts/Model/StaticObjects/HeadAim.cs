using System;
using Test.Helper;
using UnityEngine;


namespace Test.Model.StaticObjects
{
    public class HeadAim : MonoBehaviour , ISetDamage
    {
        public float Hp = 100;
        public event Action<InfoCollision> OnApplyDamageChange;
        public void SetDamage(InfoCollision info)
        {
            OnApplyDamageChange?.Invoke(new InfoCollision(info.IdAttacker, info.IdWeapon, info.Damage * 100));
        }
    }
}

