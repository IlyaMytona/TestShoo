using System;
using Test.Behaviour;
using Test.Enum;
using Test.GameServices;
using Test.Helper;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


namespace Test.Model.Ai
{
    public class Bot : Unit
    {
        #region Private Data

        private const float _chaseDistance = 7.0f;
        private const float _attackRadius = 1.0f;
        private const float _turnAngle = 110.0f;
        [SerializeField] private GameObject _enemyBulletPrefab;
        private EnemyStates _state;
        private GameObject _enemyBullet;
        private Animator _animator;
        private Rigidbody _rigidbody;
        private float _fireRate = 2.0f;
        private float _nextFire = 0.0f;
        private float _enemySpeed = 8.0f;
        private float _obstacleRange = 5.0f;
        private int _idAttacker;
        private int _idWeapon;
        
        #endregion


        #region Fields

        public event Action<Bot> OnDestroyBotEvent;
        public float MaxHps = 100;
        public float speed = 30.0f;
        public float Damage = 25.0f;

        #endregion


        #region Properties

        public NavMeshAgent Agent { get; private set; }
        public Transform Target { get; set; }

        #endregion


        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            _maxHp = MaxHps;
            Fraction = Enum.Fraction.Blue;
            
            _idAttacker = InstanceID;
            _idWeapon = _enemyBulletPrefab.GetInstanceID();
            _state = EnemyStates.Wander;
            _animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            _rigidbody = GetComponent<Rigidbody>();            
        }        

        private void Update()
        {
            if (Agent == null) return;
                
            if (Target==null)
            {
                Target = Services.Instance.LevelService.PlayerBehaviour.transform;
            }

            var distance = transform.position.CalculateDistance(Target.position);

            if (distance < _chaseDistance)
            {
                _state = EnemyStates.Chase;
            }
            else
            {
                _state = EnemyStates.Wander;
            }

            Ray ray = new Ray(transform.position, transform.forward);
            // spherecst and determine if enemy needs to turn
            RaycastHit hit;

            if (_state == EnemyStates.Wander)
            {
                // Move enemy and generate Ray
                transform.Translate(0, 0, _enemySpeed * Time.deltaTime);

                if (Physics.SphereCast(ray, 1.0f, out hit))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    if (hitObject.GetComponent<PlayerBehaviour>())
                    {
                        //spherecast hits player, fire laser!
                        if (_enemyBullet == null && Time.time > _nextFire)
                        {
                            Attack();
                        }
                    }

                    else if (hit.distance < _obstacleRange)
                    {
                        float turnAngle = Random.Range(-_turnAngle, _turnAngle);
                        transform.Rotate(0, turnAngle, 0);
                    }
                }
            }

            else if (_state == EnemyStates.Chase)
            {
                if (Agent == null) 
                {
                    return;
                } 
                if (Agent!= null)
                {
                    Agent.SetDestination(Target.position);
                }
                if (Physics.SphereCast(ray, _attackRadius, out hit))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    if (hitObject.GetComponent<PlayerBehaviour>())
                    {
                        //spherecast hits player, fire laser!
                        if (Time.time > _nextFire)
                        {
                            Attack();
                        }
                    }
                }
            }

            else
            {
                _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionZ;

            }
        }
        #endregion


        #region Methods

        private void Attack()
        {
            _animator.SetTrigger("Attack");

            _nextFire = Time.time + _fireRate;
            _enemyBullet = Instantiate(_enemyBulletPrefab) as GameObject;
            _enemyBullet.transform.position = transform.TransformPoint(0, 1.5f, 1.5f);
            _enemyBullet.transform.rotation = transform.rotation;
            _enemyBullet.transform.Translate(Vector3.forward * (Time.deltaTime * Time.timeScale * speed));
        }

        private void OnCollisionEnter(Collision collision)
        {
            var tempObj = collision.gameObject.GetComponent<ISetDamage>();

            if (tempObj != null)
            {
                tempObj.SetDamage(new InfoCollision(_idAttacker, _idWeapon, Damage));
            }
            if (_enemyBullet!=null) Destroy(_enemyBullet.gameObject);       
        }

        public override void Die()
        {
            base.Die();
            _state = EnemyStates.Dead;
            Agent.enabled = false;
            this.IsVisible = false;
            OnDestroyBotEvent.Invoke(this);            
        }

        public override void Revive()
        {
            base.Revive();
        }
        
        #endregion
    }
}
