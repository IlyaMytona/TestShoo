using UnityEngine;


namespace Test
{
    public sealed class Gun : Weapon
    {
        public Gun(GameObject gameObject, PoolObjectAmmunition poolObject) : base(gameObject, poolObject) { }
        public override void Fire()
        {
            if (!_isReadyToShoot) return;
            if (_isReloading) return;
            if (Clip.CountAmmunition <= 0) return;
            _weaponBehaviour.Animator.SetTrigger("Fire");
            var temAmmunition = _poolObject.GetObject(_weaponBehaviour.Barrel.position, _weaponBehaviour.Barrel.rotation);            
            if (temAmmunition == null) return;            
            
            _gunAudioSource.Play(); //PlayOneShot
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
                _gunLine.SetPosition(1, _shootHit.point);
            }

            else
            {
                _gunLine.SetPosition(1, _shootRay.origin + _shootRay.direction * _range);
            }

            temAmmunition.AddForce(_weaponBehaviour.Force);
            Clip.CountAmmunition--;
            _isReadyToShoot = false;
            _timeRemaining.AddTimeRemaining(_weaponBehaviour.RechergeTime);            
            //UiInterface.WeaponUiText.ShowData(_weapon.Clip.CountAmmunition, _weapon.CountClip);
        }
    }
}
