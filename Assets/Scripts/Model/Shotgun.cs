using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Collections;
using Test.AmmunitionBullets;
using Test.Controllers.TimeRemaining;


namespace Test.Model
{
    public sealed class Shotgun : Weapon
    {
        private int pelletsCount = 6;        
        private float dispersionValue = 0.06f;
        public Shotgun(GameObject gameObject, PoolObjectAmmunition poolObject) : base(gameObject, poolObject) { }
        public override async void Fire()
        {
            if (!_isReadyToShoot) return;
            if (_isReloading) return;
            if (Clip.CountAmmunition <= 0) return;
            _weaponBehaviour.Animator.SetTrigger("FireShotgun");
            await WaitUntilMiddleOfAnimation();
            
            var tempAmmunition = _poolObject.GetObject(_weaponBehaviour.Barrel.position, _weaponBehaviour.Barrel.rotation);
            _isReadyToShoot = false;

            if (tempAmmunition == null) return;

            _gunAudioSource.Play();
            _gunLight.enabled = true;

            _gunParticles.Stop();
            _gunParticles.Play();
            
            _shootRay.origin = _weaponBehaviour.Barrel.position;
            _shootRay.direction = _weaponBehaviour.Barrel.forward;

            //build an array of rays
            Ray[] rays = new Ray[pelletsCount];
            for (int i = 0; i < pelletsCount; i++)
            {
                //generate a random spread for the hitscan shotgun blast
                float randomSpreadX = UnityEngine.Random.Range(-dispersionValue, dispersionValue);
                float randomSpreadY = UnityEngine.Random.Range(-dispersionValue, dispersionValue);
                Ray ray = new Ray(_weaponBehaviour.Barrel.position, _weaponBehaviour.Barrel.TransformDirection(new Vector3(randomSpreadX, randomSpreadY, 1)));
                rays[i] = ray;

                _shotGunLine[i].enabled = true;
                _shotGunLine[i].SetPosition(0, _weaponBehaviour.Barrel.position);
            }
            _shootRay.origin = _weaponBehaviour.Barrel.position;
            _shootRay.direction = _weaponBehaviour.Barrel.forward;
            // for each ray, handle hits            
            int j = 0;
            foreach (var ray in rays)
            {
                //create pellets
                /*Vector3 currentBulletPoint = _weaponBehaviour.Barrel.transform.position +
                    new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), UnityEngine.Random.Range(-0.3f, 0.3f), UnityEngine.Random.Range(-0.3f, 0.3f));
                Quaternion currentBulletRotation = _weaponBehaviour.Barrel.transform.rotation;
                Instantiate(_projectile, currentBulletPoint,
                    currentBulletRotation);*/
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
            tempAmmunition.AddForce(_weaponBehaviour.Force);
            Clip.CountAmmunition--;
            _isReadyToShoot = false;
            _timeRemaining.AddTimeRemaining(_weaponBehaviour.RechergeTime);
        }

        private IEnumerator WaitUntilMiddleOfAnimation()
        {
            yield return UniTask.Delay(TimeSpan.FromSeconds(2)).ToCoroutine();
        }
        
    }
}
