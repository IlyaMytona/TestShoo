namespace Test
{
    public sealed class WeaponController : BaseController
    {
        #region Private Data

        private Weapon _weapon;
        private int _selectIndexWeapon = 0;

        #endregion


        #region Methods

        public override void On(params IModel[] weapon)
        {            
            //if (IsActive) return;
            base.On(weapon[0]);
            _weapon = weapon[0] as Weapon;
            
            if (_weapon == null) return;
            _weapon.IsVisible = true;

            //UiInterface.WeaponUiText.SetActive(true);
            //UiInterface.WeaponUiText.ShowData(_weapon.Clip.CountAmmunition, _weapon.CountClip);
        }

        public override void Off()
        {
            if (!IsActive) return;
            base.Off();
            _weapon.IsVisible = false;
            _weapon = null;
            //UiInterface.WeaponUiText.SetActive(false);
        }

        public void ReloadClip()
        {
            if (_weapon == null) return;


            _weapon.ReloadClip();
            //UiInterface.WeaponUiText.ShowData(_weapon.Clip.CountAmmunition, _weapon.CountClip);
        }

        public void Fire()
        {
            if (_weapon == null) return;
            _weapon.Fire();
            //UiInterface.WeaponUiText.ShowData(_weapon.Clip.CountAmmunition, _weapon.CountClip);
        }

        public void SelectWithKeyWeapon(int i)
        {
            Off();
            var tempWeapon = Services.Instance.LevelService.Weapons[i];
            if (tempWeapon != null)
            {
                On(tempWeapon);
            }
        }

        public void MouseScroll(MouseScrollWheel value)
        {
            var tempWeapon = SelectWeapon(value);
            SelectWeapon(tempWeapon);
        }

        public void SelectWeapon(Weapon weapon)
        {
            Off();
            if (weapon != null)
            {
                On(weapon);
            }
        }

        public Weapon SelectWeapon(int weaponNumber)
        {
            if (weaponNumber < 0 || weaponNumber >= Services.Instance.LevelService.Weapons.Length) return null;
            var tempWeapon = Services.Instance.LevelService.Weapons[weaponNumber];
            return tempWeapon;
        }

        public Weapon SelectWeapon(MouseScrollWheel scrollWheel)
        {
            if (scrollWheel == MouseScrollWheel.Up)
            {
                if (_selectIndexWeapon < Services.Instance.LevelService.Weapons.Length - 1)
                {
                    _selectIndexWeapon++;
                }
                else
                {
                    _selectIndexWeapon = -1;
                }
                return SelectWeapon(_selectIndexWeapon);
            }

            if (_selectIndexWeapon <= 0)
            {
                _selectIndexWeapon = Services.Instance.LevelService.Weapons.Length;
            }
            else
            {
                _selectIndexWeapon--;
            }
            return SelectWeapon(_selectIndexWeapon);
        }

        #endregion
    }
}
