using Test.Controllers.TimeRemainings;
using Test.GameServices;
using Test.Helper;
using Test.Interface;
using Test.Model;
using Test.Throw;
using UnityEngine;


namespace Test.Behaviour
{
    public sealed class PlayerBehaviour : Unit, IPlayerMovement
    {
        #region Private Data

        private const float _gravityCoefficient = -4.0f;
        private const int _xRotationAngle = 0;
        private const int _yRotationAngle = 0;
        [SerializeField] private PlayerSoData _playerSoData;
        private ThrowGrenadesManager _throwManager;        
        private CharacterController _characterController;
        private Vector3 _respawnPosition;
        private ITimeRemaining _timeRemaining;        
        private float _reviveDelay = 5.0f;
        private float _playerHp;
        private float _speed;
        private float _jumpForce;
        private float _gravity;
        private float _yVelocity;

        #endregion


        #region Fields

        public Transform GunContainer;        
        
        #endregion
        

        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            Fraction = Enum.Fraction.Red;
            _throwManager = GetComponent<ThrowGrenadesManager>();
            _characterController = GetComponent<CharacterController>();
            _playerHp = _playerSoData.PlayerHp;
            _maxHp = _playerHp;            
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
            Vector3 MousePos = MousePositionHandler.Instance.MousePosition;
            //Convert the player’s coordinates from world to screen
            Vector3 CharacterPos = Camera.main.WorldToScreenPoint(transform.position);
            var VectorToTarget = MousePos - CharacterPos;
            float angleToTarget = Mathf.Atan2(VectorToTarget.x, VectorToTarget.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(_xRotationAngle, angleToTarget, _yRotationAngle);
        }

        public void Walk(float deltaX, float deltaZ)
        {            
            Vector3 movement = new Vector3(deltaX, 0, deltaZ) * _speed;            
            _yVelocity += _gravity * Time.deltaTime;
            movement.y = _yVelocity;
            movement *= Time.deltaTime;            
            _characterController.Move(movement);
            if (!_throwManager.IsAiming)
            {
#if UNITY_STANDALONE
                LookRotation();
#endif
            }
        }

#if UNITY_IOS || UNITY_ANDROID
        public void Rotate(float x, float y)
        {
            if (!_throwManager.IsAiming)
            {
                if (x != 0f || y != 0f)
                    transform.eulerAngles = new Vector3(0, Mathf.Atan2(y, -x) * 180 / Mathf.PI - 90, 0);
            }
                
        }
#endif
        public override void Die()
        {
            base.Die();
            _timeRemaining.AddTimeRemaining(_reviveDelay);
            Services.Instance.LevelService.UiInterface.WeaponUiText.gameObject.SetActive(false);
            Services.Instance.LevelService.SetPanelEndLevelActive(true);
            _characterController.enabled = false;
        }        

        public override void Revive()
        {            
            base.Revive();
            Services.Instance.LevelService.SetPanelEndLevelActive(false);
            Services.Instance.LevelService.UiInterface.WeaponUiText.gameObject.SetActive(true);
            _playerHp = _maxHp;
            DoRevive();
        }

        private void DoRevive()
        {
            transform.position = new Vector3(_respawnPosition.x, _respawnPosition.y, _respawnPosition.z);
            _characterController.enabled = true;            
        }
        
        private void OnDestroy()
        {
            Services.Instance.UnitsHolderService.OnDestroyUnit(this);
        }       

#endregion
    }
}
