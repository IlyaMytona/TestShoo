using Test.Enum;
using Test.Model;
using UnityEngine;


namespace Test.Behaviour
{
    public sealed class WeaponBehaviour : MonoBehaviour
    {
        [SerializeField] private WeaponSoData _weaponData;
        public AmmunitionType AmmunitionType;

        public Sprite WeaponIcon;
        public float Force = 400.0f;
        public float RechergeTime = 0.2f;
        public int CountClip = 99999;
        public int PelletsCount = 6;
        public float Range = 100f;
        public Transform Barrel;

        public int ShootableMask;
        public ParticleSystem GunParticles;
        public LineRenderer GunLine;
        public LineRenderer[] ShotGunLine;
        public Light GunLight;
        public AudioSource AudioSource;
        public Transform PlayerTransform;
        public Unit Unit;

        private void Awake()
        {
            _weaponData.SetData(AmmunitionType);
            WeaponIcon = _weaponData.WeaponIcon;
            Force = _weaponData.Force;
            RechergeTime = _weaponData.RechergeTime;
            CountClip = _weaponData.CountClip;
            PelletsCount = _weaponData.PelletsCount;
            Range = _weaponData.Range;

            ShootableMask = LayerMask.GetMask("Shootable");

            GunParticles = Barrel.GetComponent<ParticleSystem>();
            GunLine = Barrel.GetComponent<LineRenderer>();
            ShotGunLine = GetComponentsInChildren<LineRenderer>();
            GunLight = Barrel.GetComponent<Light>();
            PlayerTransform = GetComponentInParent<Transform>();
            Unit = GetComponentInParent<Unit>();
        }
    }
}
