using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Test.AmmunitionBullets;
using Test.Behaviour;
using Test.Controllers.TimeRemainings;
using Test.Helper;
using Test.Interface;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Test.Model
{
    public abstract class Weapon : IModel
    {
        #region Private Data
        private Queue<Clip> _clips = new Queue<Clip>();
        private int _maxCountAmmunition = 40;
        private int _minCountAmmunition = 20;
        protected readonly ITimeRemaining _timeRemaining;
        protected readonly WeaponBehaviour _weaponBehaviour;
        protected Ray _shootRay = new Ray();
        protected RaycastHit _shootHit;
        protected int _shootableMask;
        protected ParticleSystem _gunParticles;
        protected float _range;
        protected int _pelletsCount;
        protected LineRenderer _gunLine;
        protected LineRenderer[] _shotGunLine;
        protected Light _gunLight;
        protected AudioSource _gunAudioSource;
        protected PoolObjectAmmunition _poolObject;
        protected Transform source;
        protected float _raycastDistance;
        protected bool _isReadyToShoot = true;
        protected float _reloadTime = 3.0f;
        protected bool _isReloading;
        protected Transform _playerTransform;
        private int _idAttacker;
        private int _idWeapon;

        #endregion


        #region Fields

        public event Action CalculateBulletsAndClipsEvent;
        public Clip Clip; 
        public Sprite WeaponIcon;

        #endregion


        #region Properties

        public int CountClip => _clips.Count;
        public Transform Transform { get; }
        public GameObject GameObject { get; }
        

        public bool IsVisible
        {
            set
            {
                foreach (Transform d in Transform)
                {
                    var tempRenderer = d.GetComponent<Renderer>();
                    if (tempRenderer)
                    {
                        tempRenderer.enabled = value;
                    }
                }
            }
        }

        #endregion


        #region Class LifeCycle

        protected Weapon(GameObject weaponObject, PoolObjectAmmunition poolObject)
        {
            GameObject = weaponObject;
            _idWeapon = GameObject.GetInstanceID();
            Transform = weaponObject.transform;
            _weaponBehaviour = GameObject.GetComponent<WeaponBehaviour>();

            WeaponIcon = _weaponBehaviour.WeaponIcon;
            _shootableMask = _weaponBehaviour.ShootableMask;
            _gunParticles = _weaponBehaviour.GunParticles;
            _range = _weaponBehaviour.Range;
            _pelletsCount = _weaponBehaviour.PelletsCount;
            _gunLine = _weaponBehaviour.GunLine;
            _shotGunLine = _weaponBehaviour.ShotGunLine;
            _gunLight = _weaponBehaviour.GunLight;
            _gunAudioSource = _weaponBehaviour.AudioSource;
            _playerTransform = _weaponBehaviour.PlayerTransform;
            
            _poolObject = poolObject;
            //_timeRemaining = new TimeRemaining(ReadyShoot, _weaponBehaviour.RechergeTime);
            _timeRemaining = new TimeRemaining(() => ReadyShoot(_shotGunLine.Length), _weaponBehaviour.RechergeTime);

            Initialization();
        }

        #endregion


        #region Methods
        public void Initialization()
        {
            for (var i = 0; i <= _weaponBehaviour.CountClip; i++)
            {
                AddClip(new Clip { CountAmmunition = Random.Range(_minCountAmmunition, _maxCountAmmunition) });
            }
            ReloadClip();
        }

        public abstract void Fire();

        protected void AddClip(Clip clip) => _clips.Enqueue(clip);

        public void ReloadClip()
        {
            if (CountClip <= 0) return;
            _isReloading = true;
            //_weaponBehaviour.Animator.SetTrigger("Reload");
            DoReload();            
        }

        async void DoReload()
        {
            await WaitForReloading();
            _isReloading = false;
            Clip = _clips.Dequeue();
            CalculateBulletsAndClipsEvent?.Invoke();
        }

        private IEnumerator WaitForReloading()
        {
            yield return UniTask.Delay(TimeSpan.FromSeconds(_reloadTime)).ToCoroutine();
        }

        protected void ReadyShoot(int length)
        {
            _gunLine.enabled = false;
            _gunLight.enabled = false;
            _isReadyToShoot = true;
            for (int i = 0; i < length; i++)
            {
                _shotGunLine[i].enabled = false;
            }
            CalculateBulletsAndClipsEvent?.Invoke();
        }

        protected void AmmunitionApplyDamage(Ammunition ammunition, Collider collision)
        {
            _idAttacker = _weaponBehaviour.Unit.InstanceID;
            var tempObj = collision.gameObject.GetComponent<ISetDamage>();

            if (tempObj != null)
            {
                tempObj.SetDamage(new InfoCollision(_idAttacker, _idWeapon, ammunition.CurDamage));
            }

            ammunition.DestroyAmmunition();
        }
        #endregion
    }
}
