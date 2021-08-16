using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Test
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
        public float _range;
        protected LineRenderer _gunLine;
        protected Light _gunLight;
        protected AudioSource _gunAudioSource;
        protected PoolObjectAmmunition _poolObject;
        protected Transform source;
        protected float _raycastDistance;
        protected bool _isReadyToShoot = true;
        protected float _reloadTime = 3.0f;
        protected bool _isReloading;

        #endregion


        #region Fields

        public Clip Clip;

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
            Transform = weaponObject.transform;
            _weaponBehaviour = GameObject.GetComponent<WeaponBehaviour>();
            
            _shootableMask = _weaponBehaviour.ShootableMask;
            _gunParticles = _weaponBehaviour.GunParticles;
            _range = _weaponBehaviour.Range;
            _gunLine = _weaponBehaviour.GunLine;
            _gunLight = _weaponBehaviour.GunLight;
            _gunAudioSource = _weaponBehaviour.AudioSource;
            _poolObject = poolObject;
            _timeRemaining = new TimeRemaining(ReadyShoot, _weaponBehaviour.RechergeTime);            

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
            _weaponBehaviour.Animator.SetTrigger("Reload");
            DoReload();
            //UiInterface.WeaponUiText.ShowData(_weapon.Clip.CountAmmunition, _weapon.CountClip);
        }

        async void DoReload()
        {
            await WaitForReloading();
            _isReloading = false;
            Clip = _clips.Dequeue();
        }

        private IEnumerator WaitForReloading()
        {
            yield return UniTask.Delay(TimeSpan.FromSeconds(_reloadTime)).ToCoroutine();
        }

        protected void ReadyShoot()
        {
            _gunLine.enabled = false;
            _gunLight.enabled = false;
            _isReadyToShoot = true;
        }

        #endregion
    }
}
