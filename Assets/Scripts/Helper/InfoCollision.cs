using UnityEngine;

namespace Test
{
    public struct InfoCollision
    {
        private readonly Vector3 _direction;
        private readonly float _damage;

        public InfoCollision(float damage, Vector3 direction = default)
        {
            _damage = damage;
            _direction = direction;
        }

        public Vector3 Direction => _direction;

        public float Damage => _damage;
    }
}
