namespace Test.Helper
{
    public struct InfoCollision
    {
        private readonly int _idAttacker;
        private readonly int _idWeapon;
        private readonly float _damage;

        public InfoCollision(int idAttacker, int idWeapon, float damage)
        {            
            _idAttacker = idAttacker;
            _idWeapon = idWeapon;
            _damage = damage;
        }

        public int IdAttacker => _idAttacker;
        public int IdWeapon => _idWeapon;
        public float Damage => _damage;
    }
}
