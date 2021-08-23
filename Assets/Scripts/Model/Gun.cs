using Test.AmmunitionBullets;
using Test.Controllers.TimeRemainings;
using UnityEngine;


namespace Test.Model
{
    public sealed class Gun : Weapon
    {
        public Gun(GameObject gameObject, PoolObjectAmmunition poolObject) : base(gameObject, poolObject) { }
        public override void Fire()
        {
            if (!_isReadyToShoot) return;
            if (_isReloading) return;
            if (Clip.CountAmmunition <= 0) return;
            var tempAmmunition = _poolObject.GetObject(_weaponBehaviour.Barrel.position, _weaponBehaviour.Barrel.rotation);            
            if (tempAmmunition == null) return;            
            
            _gunAudioSource.Play();
            _gunLight.enabled = true;

            _gunParticles.Stop();
            _gunParticles.Play();

            _gunLine.enabled = true;
            _gunLine.SetPosition(0, _weaponBehaviour.Barrel.position);

            _shootRay.origin = _weaponBehaviour.Barrel.position;
            _shootRay.direction = _weaponBehaviour.Barrel.forward;

            if (Physics.Raycast(_shootRay, out _shootHit, _range, _shootableMask))
            {
                //damage
                AmmunitionApplyDamage(tempAmmunition, _shootHit.collider);
                _gunLine.SetPosition(1, _shootHit.point);
            }

            else
            {
                _gunLine.SetPosition(1, _shootRay.origin + _shootRay.direction * _range);
            }

            tempAmmunition.AddForce(_weaponBehaviour.Force);
            Clip.CountAmmunition--;
            _isReadyToShoot = false;
            _timeRemaining.AddTimeRemaining(_weaponBehaviour.RechergeTime);            
        }
    }
}
