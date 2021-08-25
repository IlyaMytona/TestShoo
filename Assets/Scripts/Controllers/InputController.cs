using Test.Enum;
using Test.Interface;
using UnityEngine;
using Test.GameServices;
using Test.Helper;
using UnityStandardAssets.CrossPlatformInput;
using Test.Behaviour;

namespace Test.Controllers
{
    public sealed class InputController :  IExecute
    {
        #region PrivateData

        [Header("Move Inputs")]
        private readonly GenericInput _horizontalInput = new GenericInput("Horizontal", "Horizontal");
        private readonly GenericInput _verticallInput = new GenericInput("Vertical", "Vertical");
        private readonly GenericInput _horizontalRotationInput = new GenericInput("", "RightStickHorizontal");
        private readonly GenericInput _verticallRotationInput = new GenericInput("", "RightStickVertical");
        private readonly GenericInput _jumpInput = new GenericInput("Space", "X");        
        [Header("Shooter Inputs")]
        private readonly GenericInput _shotInput = new GenericInput("Fire1", "RT");
        private readonly GenericInput _reloadInput = new GenericInput("R", "RG");
        private readonly GenericInput _weaponChangeInput = new GenericInput("Mouse ScrollWheel", "");
        private readonly GenericInput _takeGunWeaponInput = new GenericInput("1", "1");
        private readonly GenericInput _takeShotgunWeaponInput = new GenericInput("2", "2");
        [Header("Throw Inputs")]
        private readonly GenericInput _throwGranadeInput = new GenericInput("Mouse1", false, "LT", false);
        private readonly GenericInput _aimThrowInput = new GenericInput("G", "LB");

        #endregion
               

        #region IExecuteController

        public void Execute()
        {            
            float deltaX = _horizontalInput.GetAxis();
            float deltaZ = _verticallInput.GetAxis();
            Services.Instance.LevelService.PlayerBehaviour.Walk(deltaX, deltaZ);
#if UNITY_IOS || UNITY_ANDROID
            float deltaRotationX = _horizontalRotationInput.GetAxis();
            float deltaRotationZ = _verticallRotationInput.GetAxis();
            Services.Instance.LevelService.PlayerBehaviour.Rotate(deltaRotationX, deltaRotationZ);
#endif
            if (_takeGunWeaponInput.GetButtonDown())
            {
                Services.Instance.LevelService.WeaponController.SelectWithKeyWeapon(0);
            }

            if (_takeShotgunWeaponInput.GetButtonDown())
            {
                Services.Instance.LevelService.WeaponController.SelectWithKeyWeapon(1);
            }            

            if (_reloadInput.GetButtonDown())
            {
                Services.Instance.LevelService.WeaponController.ReloadClip();
            }

            if (_shotInput.GetButton())
            {
                Services.Instance.LevelService.WeaponController.Fire();
            }
                       
            if (_weaponChangeInput.GetAxis() > 0f)
            {
                Services.Instance.LevelService.WeaponController.MouseScroll(MouseScrollWheel.Up);
            }

            if (_weaponChangeInput.GetAxis() < 0f)
            {
                Services.Instance.LevelService.WeaponController.MouseScroll(MouseScrollWheel.Down);
            }
            
            if (_aimThrowInput.GetButtonDown() && !Services.Instance.LevelService.ThrowGrenadesManager.IsAiming 
                && !Services.Instance.LevelService.ThrowGrenadesManager.InThrow)
            {
                Services.Instance.LevelService.ThrowGrenadesManager.PrepareToThrow(true);
            }

            if (_aimThrowInput.GetButtonUp())
            {
                Services.Instance.LevelService.ThrowGrenadesManager.PrepareToThrow(false);
            }

            if (_throwGranadeInput.GetButtonDown() && Services.Instance.LevelService.ThrowGrenadesManager.IsAiming 
                && !Services.Instance.LevelService.ThrowGrenadesManager.InThrow)
            {
                Services.Instance.LevelService.ThrowGrenadesManager.IsAiming = false;
                Services.Instance.LevelService.ThrowGrenadesManager.IsThrowInput = true;
            }

            if (Services.Instance.LevelService.ThrowGrenadesManager != null)
            {
                Services.Instance.LevelService.ThrowGrenadesManager.RotationToSpawnGrenadeZone();
            }

            if (_jumpInput.GetButtonDown())
            {
                Services.Instance.LevelService.PlayerBehaviour.GetComponent<UnitHealth>().SetDamage(new InfoCollision(1, 100, 25));
            }
        }
        
        #endregion
    }
}
