using UnityEngine;


namespace Test
{
    public sealed class PlayerBehaviour : Unit, IPlayerMovement
    {
        #region Private Data

        private const float _gravityCoefficient = -4.0f;
        private const int _xRotationAngle = 0;
        private const int _yRotationAngle = 0;
        private Animator _animator;
        private CharacterController _characterController;
        private int _jumpCount = 0;
        private int maxJumps = 2;

        #endregion


        #region Fields

        public Transform GunContainer;
        public float Speed = 10.0f;
        public float Gravity = -1.5f;
        public float YVelocity = 0.0f;
        public float JumpForce = 20.0f;

        #endregion

        #region Properties

        public Transform Transform { get; set; }

        #endregion
        

        #region UnityMethods

        private void Awake()
        {
            Services.Instance.LevelService.PlayerBehaviour = this;
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            Gravity = JumpForce * _gravityCoefficient;
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
            _animator.SetFloat("xVel", deltaX);
            _animator.SetFloat("zVel", deltaZ);
            Vector3 movement = new Vector3(deltaX, 0, deltaZ) * Speed;
            movement = transform.TransformDirection(movement);

            if (_characterController.isGrounded)
            {
                YVelocity = 0.0f;
                _jumpCount = 0;
                _animator.ResetTrigger("jump");
            }

            if (_jumpCount < maxJumps)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    YVelocity = JumpForce;
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

            if (deltaX != 0 || deltaZ != 0 || YVelocity > 0.01)
            {
                _animator.SetBool("isMoving", true);
            }
            else
            {
                _animator.SetBool("isMoving", false);
            }
            YVelocity += Gravity * Time.deltaTime;
            movement.y = YVelocity;
            movement *= Time.deltaTime;
            _characterController.Move(movement);
            LookRotation();
        }

        public void Turning(float mouseX)
        {
            _animator.SetFloat("mouseX", mouseX);
        }

        #endregion
    }
}
