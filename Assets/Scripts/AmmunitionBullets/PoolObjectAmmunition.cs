using System;
using System.Collections.Generic;
using System.Linq;
using Test.Enum;
using Test.Interface;
using Test.Model;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Test.AmmunitionBullets
{
    public sealed class PoolObjectAmmunition : IPoolObject<Ammunition>, IDisposable
    {        
        private readonly AmmunitionType _ammunitionType;
        private readonly AmmunitionFactory _ammunitionFactory;
        private readonly int _sizePool;
        private readonly Transform _rootFolderInScene;

        public Dictionary<int, Ammunition> PoolObjectsAmmunition;
        public HashSet<Ammunition> Ammunitions;
                
        public PoolObjectAmmunition(AmmunitionType ammunitionType)
        {
            PoolObjectsAmmunition = new Dictionary<int, Ammunition>(10);            
            Ammunitions = new HashSet<Ammunition>();

            _ammunitionType = ammunitionType;
            _ammunitionFactory = new AmmunitionFactory();
            _sizePool = 50;
            _rootFolderInScene = new GameObject($"Root PoolObject of {ammunitionType} Ammunition").transform;
            InitializePool();
        }

        private void InitializePool()
        {
            for (var i = 0; i < _sizePool; i++)
            {               
                var ammunition = CreateAmmunition();
                
                PoolObjectsAmmunition[ammunition.GetHashCode()] = ammunition;
                AddAmmunition(ammunition);
                ReturnToPool(ammunition.GetHashCode());
            }
        }

        public Ammunition GetObject(Vector3 position, Quaternion rotation) //, float time
        {
            var result = PoolObjectsAmmunition.Values.FirstOrDefault(ammunition => !ammunition.GameObject.activeSelf);
            if (result == null)
            {
                InitializePool();
                GetObject(position, rotation);
                return null;
            }            
            result.Transform.position = position;
            result.Transform.rotation = rotation;
            result.SetActive(true);
            return result;
        }

        public void ReturnToPool(int hash)
        {            
            var ammunition = PoolObjectsAmmunition[hash];
            ammunition.SetActive(false);
            ammunition.Transform.SetParent(_rootFolderInScene);
        }
        
        private Ammunition CreateAmmunition()
        {
            Ammunition ammunition = null;
            switch (_ammunitionType)
            {
                case AmmunitionType.Bullet:
                    ammunition = _ammunitionFactory.CreateBullet(this);
                    break;
                case AmmunitionType.FractionBullet:
                    ammunition = _ammunitionFactory.CreateFractionBullet(this);
                    break;
                default:
                    break;
            }
            return ammunition;
        }

        public void Dispose()
        {
            foreach (var t in PoolObjectsAmmunition.Values)
            {
                Object.Destroy(t.GameObject);
            }
            Object.Destroy(_rootFolderInScene.gameObject);
        }       
                
        public void AddAmmunition(Ammunition ammunition)
        {
            Ammunitions.Add(ammunition);
        }
    }
}
