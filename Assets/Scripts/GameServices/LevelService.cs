using System.Collections.Generic;
using Test.Behaviour;
using Test.Controllers;
using Test.Model;
using Test.UI;
using UnityEngine;

namespace Test.GameServices
{
    public sealed class LevelService : Service
    {
        #region Fields

        private Weapon[] _weapons;
        private Vector3[] _spawnPoints;
        public Dictionary<int, Ammunition> BulletContext;
        public Dictionary<int, Ammunition> FractionBulletContext;        
        public HashSet<Ammunition> BulletAmmunitions;
        public HashSet<Ammunition> FractionBulletAmmunitions;      

        #endregion


        #region Properties

        public PlayerBehaviour PlayerBehaviour { get;  set; }
        public Weapon[] Weapons => _weapons;
        public Vector3[] SpawnPoints => _spawnPoints;
        public WeaponController WeaponController { get; set; }
        public UiInterface UiInterface { get; set; }

        #endregion


        #region ClassLifeCycles

        public LevelService()
        {
            _weapons = new Weapon[2];
            _spawnPoints = new Vector3[3];
        }

        #endregion

        public void SetPanelEndLevelActive(bool isActive) => UiInterface.WeaponUiText.SetPanelEndLevelActive(isActive);
    }
}
