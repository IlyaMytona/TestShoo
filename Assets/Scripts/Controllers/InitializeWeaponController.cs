using Test.AmmunitionBullets;
using Test.Enum;
using Test.GameServices;
using Test.Interface;
using Test.Weapons;


namespace Test.Controllers
{
    public class InitializeWeaponController : IInitialization
    {
        public void Initialization()
        {
            var weaponFactory = new WeaponFactory();

            var poolBulletAmmunition = new PoolObjectAmmunition(AmmunitionType.Bullet);
            var gun = weaponFactory.CreateGun(poolBulletAmmunition);
            gun.IsVisible = false;
            Services.Instance.LevelService.Weapons[0] = gun;
            var bulletDictionary = poolBulletAmmunition.PoolObjectsAmmunition;
            Services.Instance.LevelService.BulletContext = bulletDictionary;            
            var bulletAmmunition = poolBulletAmmunition.Ammunitions;
            Services.Instance.LevelService.BulletAmmunitions = bulletAmmunition;

            var poolRpgBulletAmmunition = new PoolObjectAmmunition(AmmunitionType.FractionBullet);
            var shotgun = weaponFactory.CreateShotgun(poolRpgBulletAmmunition);
            shotgun.IsVisible = false;
            Services.Instance.LevelService.Weapons[1] = shotgun;
            var fractionBulletDictionary = poolRpgBulletAmmunition.PoolObjectsAmmunition;
            Services.Instance.LevelService.FractionBulletContext = fractionBulletDictionary;            
            var bulletRpgAmmunition = poolRpgBulletAmmunition.Ammunitions;
            Services.Instance.LevelService.FractionBulletAmmunitions = bulletRpgAmmunition;

            Services.Instance.LevelService.WeaponController = new WeaponController();
        }
    }
}
