using Test.Enum;
using Test.Interface;
using UnityEngine;
using Test.GameServices;
using Test.Helper;


namespace Test.Controllers
{
    public sealed class InputController :  IExecute
    {
        #region PrivateData

        [Header("Move Inputs")]
        private readonly GenericInput _horizontalInput = new GenericInput("Horizontal", "Horizontal");
        private readonly GenericInput _verticallInput = new GenericInput("Vertical", "Vertical");
        private readonly GenericInput _jumpInput = new GenericInput("Space", "X");        
        [Header("Shooter Inputs")]
        private readonly GenericInput _shotInput = new GenericInput("Mouse0", false, "RT", false);
        private readonly GenericInput _reloadInput = new GenericInput("R", "LB");
        private readonly GenericInput _rotateCameraXInput = new GenericInput("Mouse X", "Mouse X");
        private readonly GenericInput _rotateCameraYInput = new GenericInput("Mouse Y", "Mouse Y");
        private readonly GenericInput _weaponChangeInput = new GenericInput("Mouse ScrollWheel", "");
        [Header("Throw Inputs")]
        private readonly GenericInput _throwGranadeInput = new GenericInput("Mouse1", false, "LT", false);
        private readonly GenericInput _aimThrowInput = new GenericInput("G", "LB");

        private readonly KeyCode _reloadClip = KeyCode.R;
        private readonly int _mouseButton = (int)MouseButton.LeftButton;        

        #endregion
               

        #region IExecuteController

        public void Execute()
        {
            float deltaX = Input.GetAxis("Horizontal"); //TODO //_horizontalInput.GetAxis();
            float deltaZ = Input.GetAxis("Vertical"); //TODO //_verticallInput.GetAxis();
            Services.Instance.LevelService.PlayerBehaviour.Walk(deltaX, deltaZ);
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Services.Instance.LevelService.WeaponController.SelectWithKeyWeapon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Services.Instance.LevelService.WeaponController.SelectWithKeyWeapon(1);
            }            

            if (Input.GetKeyDown(_reloadClip)) //TODO //_reloadInput.GetKeyDown()
            {
                Services.Instance.LevelService.WeaponController.ReloadClip();
            }

            if (Input.GetMouseButton(_mouseButton)) //TODO //_shotInput.GetButtonDown()
            {
                Services.Instance.LevelService.WeaponController.Fire();
            }
                       
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) //TODO //_weaponChangeInput.GetAxis();
            {
                Services.Instance.LevelService.WeaponController.MouseScroll(MouseScrollWheel.Up);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f) //TODO //_weaponChangeInput.GetAxis();
            {
                Services.Instance.LevelService.WeaponController.MouseScroll(MouseScrollWheel.Down);
            }

            
            if (Input.GetKeyDown(KeyCode.G) && !Services.Instance.LevelService.ThrowGrenadesManager.IsAiming 
                && !Services.Instance.LevelService.ThrowGrenadesManager.InThrow) //TODO //_aimThrowInput.GetButtonDown()
            {
                Services.Instance.LevelService.ThrowGrenadesManager.PrepareToThrow(true);
            }

            if (Input.GetKeyUp(KeyCode.G)) //TODO //aimThrowInput.GetButtonUp()
            {
                Services.Instance.LevelService.ThrowGrenadesManager.PrepareToThrow(false);
            }

            if (Input.GetMouseButtonDown((int)MouseButton.RightButton) && Services.Instance.LevelService.ThrowGrenadesManager.IsAiming 
                && !Services.Instance.LevelService.ThrowGrenadesManager.InThrow) //TODO //_throwGranadeInput.GetButtonDown()
            {
                Services.Instance.LevelService.ThrowGrenadesManager.IsAiming = false;
                Services.Instance.LevelService.ThrowGrenadesManager.IsThrowInput = true;
            }

            if (Services.Instance.LevelService.ThrowGrenadesManager != null)
            {
                Services.Instance.LevelService.ThrowGrenadesManager.RotationToSpawnGrenadeZone();
            }

            if (Input.GetButtonDown("Jump")) //TODO //_jumpInput.GetButtonDown()
            {
                Services.Instance.LevelService.PlayerBehaviour.GetComponent<UnitHealth>().SetDamage(new InfoCollision(1, 100, 25));
            }
        }
        
        #endregion
    }
}
