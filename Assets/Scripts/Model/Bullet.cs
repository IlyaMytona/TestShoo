using System;
using Test.AmmunitionBullets;
using UnityEngine;


namespace Test.Model
{
    public sealed class Bullet : Ammunition, IDisposable
    {
        private readonly TrailRenderer _trailRenderer;
        private readonly float _timeTrailRenderer;
        
        public Bullet(GameObject gameObject, PoolObjectAmmunition poolObject) : base(gameObject, poolObject)
        {
            _trailRenderer = gameObject.GetComponentInChildren<TrailRenderer>();
            _timeTrailRenderer = _trailRenderer.time;            
        }

        public override void SetActive(bool value)
        {
            base.SetActive(value);
            if (value)
            {
                _trailRenderer.time = _timeTrailRenderer;
                _trailRenderer.enabled = true;
            }
            else
            {
                _trailRenderer.Clear();
                _trailRenderer.time = 0.0f;
                _trailRenderer.enabled = false;
            }
        }

        public void Dispose()
        {
            _poolObject.Dispose();            
        }
    }
}
