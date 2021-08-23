using Test.GameServices;


namespace Test.Model.StaticObjects
{
    public sealed class Aim : Unit
    {	
        public float Hp = 30;           
        
        protected override void Awake()
		{
            base.Awake();
            _maxHp = Hp;            
            Services.Instance.UnitsHolderService.OnCreatedUnit(this);            
		}     

        public override void Die()
        {
            base.Die();
            Services.Instance.UnitsHolderService.OnDestroyUnit(this);
            Destroy(gameObject);            
        }
    }
}