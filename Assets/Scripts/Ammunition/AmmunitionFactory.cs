using UnityEngine;


namespace Test
{
    public sealed class AmmunitionFactory
    {        
        public Bullet CreateBullet(PoolObjectAmmunition poolObject)
        {
            var bullet = Resources.Load<AmmunitionBehaviour>("Prefabs/Bullet");
            var gameObject = Object.Instantiate(bullet).gameObject;
            var result = new Bullet(gameObject, poolObject);
            
            return result;
        }

        public FractionBullet CreateFractionBullet(PoolObjectAmmunition poolObject)
        {
            var bullet = Resources.Load<AmmunitionBehaviour>("Prefabs/FractionBullet");
            var gameObject = Object.Instantiate(bullet).gameObject;
            var result = new FractionBullet(gameObject, poolObject);
            return result;
        }
    }
}
