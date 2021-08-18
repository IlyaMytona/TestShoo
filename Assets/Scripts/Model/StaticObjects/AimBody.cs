using System;
using Test.Helper;
using UnityEngine;


namespace Test.Model.StaticObjects
{
    public class AimBody : MonoBehaviour, ISetDamage
{
        public event Action<InfoCollision> OnApplyDamageChange;
        public void SetDamage(InfoCollision info)
        {
            OnApplyDamageChange?.Invoke(info);
        }
    }
}

