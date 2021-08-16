using Test;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { wander, chase, dead };

public class WanderingAI : MonoBehaviour
{
    #region Private Data

    private EnemyStates _state;

    [SerializeField] private GameObject _enemyBulletPrefab;
    private GameObject _enemyBullet;
    private NavMeshAgent _agent;
    private Animator _animator;
    private GameObject _player;
    private Rigidbody _rigidbody;

    private float _fireRate = 2.0f;
    private float _nextFire = 0.0f;
    private float _enemySpeed = 8.0f;
    private float _obstacleRange = 5.0f;

    #endregion


    #region Unity Methods
    private void Start()
    {
        _state = EnemyStates.wander;
        _animator = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, _player.transform.position);

        if (distance < 7.0f)
        {
            _state = EnemyStates.chase;
        } else
        {
            _state = EnemyStates.wander;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        // spherecst and determine if enemy needs to turn
        RaycastHit hit;

        if (_state == EnemyStates.wander)
        {
            // Move enemy and generate Ray
            transform.Translate(0, 0, _enemySpeed * Time.deltaTime);

            if (Physics.SphereCast(ray, 1.0f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerBehaviour>())
                {
                    //spherecast hits player, fire laser!
                    if(_enemyBullet == null && Time.time > _nextFire)
                    {
                        Attack();
                    }
                }
                else if (hit.distance < _obstacleRange)
                {
                    float turnAngle = Random.Range(-110, 110);
                    transform.Rotate(0, turnAngle, 0);
                } 
            }
        } 
        else if (_state == EnemyStates.chase)
        {
            _agent.SetDestination(_player.transform.position);

            if (Physics.SphereCast(ray, 1.0f, out hit))
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
        } else
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionZ;

        }
    }
    #endregion


    #region Methods

    public void ChangeState(EnemyStates state)
    {
        _state = state;
    }
    
    private void Attack()
    {
        _animator.SetTrigger("Attack");

        _nextFire = Time.time + _fireRate;
        _enemyBullet = Instantiate(_enemyBulletPrefab) as GameObject;
        _enemyBullet.transform.position = transform.TransformPoint(0, 1.5f, 1.5f);
        _enemyBullet.transform.rotation = transform.rotation;    
    }

    #endregion
}