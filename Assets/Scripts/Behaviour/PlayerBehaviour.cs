using System;
using System.Collections;
using Test.Controllers.TimeRemaining;
using Test.Enum;
using Test.GameServices;
using Test.Helper;
using Test.Interface;
using Test.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Test.Behaviour
{
    public sealed class PlayerBehaviour : Unit, IPlayerMovement, ISetDamage
    {
        #region Private Data

        private const float _gravityCoefficient = -4.0f;
        private const int _xRotationAngle = 0;
        private const int _yRotationAngle = 0;

        [SerializeField] private PlayerSoData _playerSoData;
        [SerializeField] private Slider _hpSlider;

        private Animator _animator;
        private CharacterController _characterController;
        private Vector3 _respawnPosition;
        private ITimeRemaining _timeRemaining;
        private int _jumpCount = 0;
        private int _maxJumps = 2;
        private int _reviveCounts = 5;
        private float _reviveDelay = 5f;
        private float _playerHp;
        private float _speed;
        private float _jumpForce;
        private float _gravity;
        private float _yVelocity;
        private float _maxHp;

        #endregion


        #region Fields

        public Transform GunContainer;        
        public event Action<InfoCollision> OnApplyDamageChange;
        #endregion

        #region Properties

        public Transform Transform { get; set; }

        #endregion


        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();           
            Services.Instance.UnitsHolderService.OnDestroyUnitEvent += RespawnAfterKillBot;
            
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _playerHp = _playerSoData.PlayerHp;
            _maxHp = _playerHp;
            _hpSlider.minValue = 0;
            _hpSlider.maxValue = _maxHp;
            _hpSlider.value = _maxHp;
            _speed = _playerSoData.Speed;
            _jumpForce = _playerSoData.JumpForce;
            _gravity = _jumpForce * _gravityCoefficient;
            _yVelocity = _playerSoData.YVelocity;
            _respawnPosition = Services.Instance.LevelService.SpawnPoints.RandomObject();

            _timeRemaining = new TimeRemaining(() => Revive(), _reviveDelay);
        }

        #endregion


        #region Methods

        private void LookRotation()
        {
            Vector3 MousePos = Input.mousePosition;
            //Convert the player’s coordinates from world to screen
            Vector3 CharacterPos = Camera.main.WorldToScreenPoint(transform.position);
            var VectorToTarget = MousePos - CharacterPos;
            float angleToTarget = Mathf.Atan2(VectorToTarget.x, VectorToTarget.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(_xRotationAngle, angleToTarget, _yRotationAngle);
        }

        public void Walk(float deltaX, float deltaZ)
        {
            if (_animator == null) return;
            _animator.SetFloat("xVel", deltaX);
            _animator.SetFloat("zVel", deltaZ);
            Vector3 movement = new Vector3(deltaX, 0, deltaZ) * _speed;
            movement = transform.TransformDirection(movement);

            if (_characterController.isGrounded)
            {
                _yVelocity = 0.0f;
                _jumpCount = 0;
                _animator.ResetTrigger("jump");
            }

            if (_jumpCount < _maxJumps)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    _yVelocity = _jumpForce;
                    _jumpCount += 1;
                    if (_jumpCount == 1)
                    {
                        _animator.SetTrigger("jump");
                    }
                    else if (_jumpCount == 2)
                    {
                        _animator.SetTrigger("flip");
                    }
                }
            }

            if (deltaX != 0 || deltaZ != 0 || _yVelocity > 0.01)
            {
                _animator.SetBool("isMoving", true);
            }
            else
            {
                _animator.SetBool("isMoving", false);
            }
            _yVelocity += _gravity * Time.deltaTime;
            movement.y = _yVelocity;
            movement *= Time.deltaTime;
            _characterController.Move(movement);
            LookRotation();
        }

        public void SetDamage(InfoCollision info)
        {
            OnApplyDamageChange?.Invoke(info);
            if (_playerHp > 0)
            {
                _playerHp -= info.Damage;
                _hpSlider.value = _playerHp;
            }

            if (_playerHp <= 0)
            {
                _hpSlider.value = _hpSlider.minValue;
                Die();
                _timeRemaining.AddTimeRemaining(_reviveDelay);                
            }
        }

        protected override void Die()
        {
            base.Die();
            _animator.SetTrigger("Die");
            Services.Instance.LevelService.UiInterface.WeaponUiText.gameObject.SetActive(false);
            Services.Instance.LevelService.SetPanelEndLevelActive(true);
            _characterController.enabled = false;
            
            _reviveCounts--;
        }

        public void ResetHP()
        {
            _hpSlider.value = _maxHp;
        }

        protected override void Revive()
        {
            if (_reviveCounts == 0)
            {
                Destroy(gameObject);
                return;
            }
            base.Revive();
            Services.Instance.LevelService.SetPanelEndLevelActive(false);
            Services.Instance.LevelService.UiInterface.WeaponUiText.gameObject.SetActive(true);
            _playerHp = _maxHp;
            ResetHP();
            DoRevive();
        }

        private void DoRevive()
        {
            transform.position = new Vector3(_respawnPosition.x, _respawnPosition.y, _respawnPosition.z);
            if (_animator != null) _animator.SetTrigger("Revive");
            _characterController.enabled = true;            
        }

        private void RespawnAfterKillBot()
        {
            _characterController.enabled = false;
            Invoke(nameof(DoRevive), _reviveDelay);            
        }
        
        private void OnDestroy()
        {
            Services.Instance.UnitsHolderService.OnDestroyUnitEvent -= RespawnAfterKillBot;
            Services.Instance.UnitsHolderService.OnDestroyUnit(this);
        }       

        #endregion
    }
}
