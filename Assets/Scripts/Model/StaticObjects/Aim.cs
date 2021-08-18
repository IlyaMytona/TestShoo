using Test.GameServices;
using Test.Helper;


namespace Test.Model.StaticObjects
{
    public sealed class Aim : Unit
    {	
        public float Hp = 30;
        private bool _isDead = false;              
        
        protected override void Awake()
		{
            base.Awake();
            Services.Instance.UnitsHolderService.OnCreatedUnit(this);
            var damage = GetComponentsInChildren<ISetDamage>();
            foreach (var setDamage in damage)
            {
                setDamage.OnApplyDamageChange += SetDamage;
            }
		}     

		public void SetDamage(InfoCollision info)
		{
            if (Hp > 0)
			{
                Hp -= info.Damage;
			}

			if (Hp <= 0)
			{
                if (!_isDead)
                {
                    Services.Instance.UnitsHolderService.OnDestroyUnit(this);
                    _isDead = true;
                    Destroy(gameObject);                    
                }                                            
            }
		}

	}
}