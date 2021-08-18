using Test.AmmunitionBullets;
using Test.Behaviour;
using Test.Model;
using UnityEngine;


namespace Test.Weapons
{
    public sealed class WeaponFactory
    {
        public Gun CreateGun(PoolObjectAmmunition poolObject)
        {
            var gun = Resources.Load<WeaponBehaviour>("Prefabs/Gun");
            var player = Object.FindObjectOfType<PlayerBehaviour>();
            var gameObject = Object.Instantiate(gun, player.GunContainer.position, gun.transform.rotation, player.GunContainer).gameObject;
            
            var result = new Gun(gameObject, poolObject);
            return result;
        }

        public Shotgun CreateShotgun(PoolObjectAmmunition poolObject)
        {
            var gun = Resources.Load<WeaponBehaviour>("Prefabs/Shotgun");
            var player = Object.FindObjectOfType<PlayerBehaviour>();
            var gameObject = Object.Instantiate(gun, player.GunContainer.position, gun.transform.rotation, player.GunContainer).gameObject;

            var result = new Shotgun(gameObject, poolObject);
            return result;
        }
    }
}
