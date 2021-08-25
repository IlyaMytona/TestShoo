using UnityEngine;
using Test.AmmunitionBullets;
using Test.Controllers.TimeRemainings;


namespace Test.Model
{
    public sealed class Shotgun : Weapon
    {
        private Vector3[] _rayDirections = new Vector3[6];
        private float _dispersionValue = 0.06f;
        public Shotgun(GameObject gameObject, PoolObjectAmmunition poolObject) : base(gameObject, poolObject) { }
        public override void Fire()
        {
            if (!_isReadyToShoot) return;
            if (_isReloading) return;
            if (Clip.CountAmmunition <= 0) return;            
            _isReadyToShoot = false;
            _gunAudioSource.Play();
            _gunLight.enabled = true;
            _gunParticles.Stop();
            _gunParticles.Play();            
            _shootRay.origin = _weaponBehaviour.Barrel.position;
            _shootRay.direction = _weaponBehaviour.Barrel.forward;

            //build an array of rays
            Ray[] rays = new Ray[_pelletsCount];
            for (int i = 0; i < _pelletsCount; i++)
            {
                //generate a random spread for the hitscan shotgun blast
                float randomSpreadX = UnityEngine.Random.Range(-_dispersionValue, _dispersionValue);
                float randomSpreadY = UnityEngine.Random.Range(-_dispersionValue, _dispersionValue);
                Vector3 rayDirection = _weaponBehaviour.Barrel.TransformDirection(new Vector3(randomSpreadX, randomSpreadY, 1));
                _rayDirections[i] = rayDirection;
                Ray ray = new Ray(_weaponBehaviour.Barrel.position, rayDirection);
                rays[i] = ray;

                _shotGunLine[i].enabled = true;
                _shotGunLine[i].SetPosition(0, _weaponBehaviour.Barrel.position);
            }
            
            // for each ray, handle hits            
            int j = 0;
            foreach (var ray in rays)
            {
                var tempAmmunition = _poolObject.GetObject(_weaponBehaviour.Barrel.position, Quaternion.LookRotation(_rayDirections[j]));
                tempAmmunition.AddForce(_weaponBehaviour.Force);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _range, _shootableMask))
                {
                    Debug.DrawLine(_weaponBehaviour.Barrel.position, hit.point, Color.red, _range);
                    //damage
                    AmmunitionApplyDamage(tempAmmunition, hit.collider);
                   _shotGunLine[j].SetPosition(1, hit.point);
                }
                else
                {
                    _shotGunLine[j].SetPosition(1, ray.origin + ray.direction * _range);
                }
                j++;
            }
            Clip.CountAmmunition--;
            _isReadyToShoot = false;
            _timeRemaining.AddTimeRemaining(_weaponBehaviour.RechergeTime);
        }
    }
}
