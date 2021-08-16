using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Collections;


namespace Test
{
    public sealed class Shotgun : Weapon
    {
        private int pellets = 6;
        private float dispersionValue = 0.06f;
        public Shotgun(GameObject gameObject, PoolObjectAmmunition poolObject) : base(gameObject, poolObject) { }
        public override void Fire()
        {
            if (!_isReadyToShoot) return;
            if (_isReloading) return;
            if (Clip.CountAmmunition <= 0) return;
            _weaponBehaviour.Animator.SetTrigger("FireShotgun");
            DoFire();
        }        

        async void DoFire()
        {
            await WaitUntilMiddleOfAnimation();

            var temAmmunition = _poolObject.GetObject(_weaponBehaviour.Barrel.position, _weaponBehaviour.Barrel.rotation);
            _isReadyToShoot = false;

            if (temAmmunition == null) return;
            
            _gunAudioSource.Play();
            _gunLight.enabled = true;

            _gunParticles.Stop();
            _gunParticles.Play();

            _gunLine.enabled = true;
            _gunLine.SetPosition(0, _weaponBehaviour.Barrel.position);

            _shootRay.origin = _weaponBehaviour.Barrel.position;
            _shootRay.direction = _weaponBehaviour.Barrel.forward;

            //build an array of rays
            Ray[] rays = new Ray[pellets];
            for (int i = 0; i < pellets; i++)
            {
                //generate a random spread for the hitscan shotgun blast
                float randomSpreadX = UnityEngine.Random.Range(-dispersionValue, dispersionValue);
                float randomSpreadY = UnityEngine.Random.Range(-dispersionValue, dispersionValue);
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f + randomSpreadX, 0.5f + randomSpreadY, 0));
                rays[i] = ray;
            }
            _shootRay.origin = _weaponBehaviour.Barrel.position;
            _shootRay.direction = _weaponBehaviour.Barrel.forward;
            // for each ray, handle hits
            foreach (var ray in rays)
            {
                //create pellets

                /*Vector3 currentBulletPoint = _weaponBehaviour.Barrel.transform.position +
                    new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), UnityEngine.Random.Range(-0.3f, 0.3f), UnityEngine.Random.Range(-0.3f, 0.3f));
                Quaternion currentBulletRotation = _weaponBehaviour.Barrel.transform.rotation;
                Instantiate(_projectile, currentBulletPoint,
                    currentBulletRotation);*/

                if (Physics.Raycast(_shootRay, out _shootHit, _range, _shootableMask))
                {
                    //damage
                    _gunLine.SetPosition(1, _shootHit.point);
                }
                else
                {
                    _gunLine.SetPosition(1, _shootRay.origin + _shootRay.direction * _range);
                }
            }
            temAmmunition.AddForce(_weaponBehaviour.Force);
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
