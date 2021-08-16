using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


namespace Test
{
    public sealed class LevelService : Service
    {
        #region Fields

        private Weapon[] _weapons;
        public Dictionary<int, Ammunition> BulletContext;
        public Dictionary<int, Ammunition> FractionBulletContext;        
        public HashSet<Ammunition> BulletAmmunitions;
        public HashSet<Ammunition> FractionBulletAmmunitions;

        #endregion


        #region Properties

        public PlayerBehaviour PlayerBehaviour { get;  set; }
        public Weapon[] Weapons => _weapons;
        public WeaponController WeaponController { get; set; }
        
        #endregion


        #region ClassLifeCycles

        public LevelService()
        {
            _weapons = new Weapon[9];
        }

        #endregion        
    }
}
