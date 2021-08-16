using System.Collections;
using UnityEngine;


namespace Test
{
    public sealed class WeaponBehaviour : MonoBehaviour
    {
        public Transform Barrel;
        public float Force = 999;
        public float RechergeTime = 0.2f;
        public int CountClip = 5;
        public AmmunitionType AmmunitionType;
        public Ray ShootRay = new Ray();
        public RaycastHit ShootHit;
        public int ShootableMask;
        public float Range = 100f;
        public ParticleSystem GunParticles;
        public LineRenderer GunLine;
        public Light GunLight;
        public Animator Animator;
        public AudioSource AudioSource;

        private void Awake()
        {
            ShootableMask = LayerMask.GetMask("Shootable");

            GunParticles = Barrel.GetComponent<ParticleSystem>();
            GunLine = Barrel.GetComponent<LineRenderer>();
            GunLight = Barrel.GetComponent<Light>();
            Animator = Barrel.GetComponentInParent<Animator>();
        }
    }
}
