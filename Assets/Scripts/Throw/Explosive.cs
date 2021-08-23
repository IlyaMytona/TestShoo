using System.Collections;
using Test.Helper;
using UnityEngine;


namespace Test.Throw
{
    public class Explosive : MonoBehaviour
    {
        [SerializeField] private GrenadeDamage _damage;
        [SerializeField] private Transform _explosion;
        [SerializeField] private LayerMask _applyDamageLayer;
        [SerializeField] private LayerMask _applyForceLayer;

        private float _explosionForce = 4500.0f;
        private float _minExplosionRadius = 1.0f;
        private float _maxExplosionRadius = 2.0f;
        private float _upwardsModifier = 1.0f;
        private float _timeToExplode = 1.0f;
        private bool _inTimer;
        private ArrayList _collidersReached;

        private void Start()
        {
            _collidersReached = new ArrayList();            
        }

        private IEnumerator StartTimer()
        {
            if (!_inTimer)
            {                
                _inTimer = true;
                var startTime = Time.time;
                var time = 0f;
                while (time < _timeToExplode)
                {
                    yield return new WaitForEndOfFrame();
                    time = Time.time - startTime;                    
                }
                if (gameObject)
                {                    
                    Explode();
                }
            }
        }

        private IEnumerator DestroyBomb()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            StartCoroutine(StartTimer());
        }

        private void Explode()
        {
            RemoveParentOfOther(_explosion);
            _explosion.gameObject.SetActive(true);
            var colliders = Physics.OverlapSphere(transform.position, _maxExplosionRadius, _applyDamageLayer);

            for (int i = 0; i < colliders.Length; ++i)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, colliders[i].transform.position - transform.position, out hit, 100.0f))
                {
                    if (hit.collider == colliders[i])
                    {
                        if (!_collidersReached.Contains(colliders[i].gameObject))
                        {
                            _collidersReached.Add(colliders[i].gameObject);
                            var damage = new GrenadeDamage(_damage);
                            damage.Sender = transform;
                            damage.HitPosition = colliders[i].ClosestPointOnBounds(transform.position);
                            damage.Receiver = colliders[i].transform;
                            var distance = transform.position.CalculateDistance(damage.HitPosition);
                            var damageValue = distance <= _minExplosionRadius ? _damage.DamageValue : GetPercentageForce(distance, _damage.DamageValue);
                            damage.DamageValue = (int)damageValue;
                            colliders[i].gameObject.ApplyDamage(new InfoCollision(damage.HitPosition.GetHashCode(), transform.GetInstanceID(), _damage.DamageValue));
                        }
                    }
                }                
            }
            StartCoroutine(ApplyExplosionForce());
            StartCoroutine(DestroyBomb());
        }
        private void RemoveParentOfOther(Transform other)
        {
            other.parent = null;
        }

        private IEnumerator ApplyExplosionForce()
        {
            yield return new WaitForSeconds(0.1f);

            var colliders = Physics.OverlapSphere(transform.position, _maxExplosionRadius, _applyForceLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                var _rigdbody = colliders[i].GetComponent<Rigidbody>();
                var distance = Vector3.Distance(transform.position, colliders[i].ClosestPointOnBounds(transform.position));
                var force = distance <= _minExplosionRadius ? _explosionForce : GetPercentageForce(distance, _explosionForce);
                if (_rigdbody)
                {
                    _rigdbody.AddExplosionForce(force, transform.position, _maxExplosionRadius, _upwardsModifier, ForceMode.Force);
                }
            }
        }

        private float GetPercentageForce(float distance, float value)
        {
            if (distance > _maxExplosionRadius) distance = _maxExplosionRadius;

            var distanceLimit = _maxExplosionRadius - _minExplosionRadius;
            var distanceCalc = Mathf.Clamp(distance - _minExplosionRadius, 0, distanceLimit);
            var distanceResult = Mathf.Clamp(distanceLimit - (distanceCalc), 0, distanceLimit);
            var multiple = ((distanceResult / distanceLimit) * 100f) * 0.01f;
            return value * multiple;
        }        
    }
}