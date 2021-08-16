using UnityEngine;


namespace Test
{
    public sealed class InputController :  IExecute
    {
        #region PrivateData
                
        private KeyCode _cancel = KeyCode.Escape;
        private KeyCode _reloadClip = KeyCode.R;
        private int _mouseButton = (int)MouseButton.LeftButton;
                
        #endregion

        #region ClassLifeCycles
                

        #endregion

        #region IExecuteController

        public void Execute()
        {
            float deltaX = Input.GetAxis("Horizontal");
            float deltaZ = Input.GetAxis("Vertical");
            Services.Instance.LevelService.PlayerBehaviour.Walk(deltaX, deltaZ);
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Services.Instance.LevelService.WeaponController.SelectWithKeyWeapon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Services.Instance.LevelService.WeaponController.SelectWithKeyWeapon(1);
            }

            if (Input.GetKeyDown(_cancel))
            {
                Services.Instance.LevelService.WeaponController.Off();
            }

            if (Input.GetKeyDown(_reloadClip))
            {
                Services.Instance.LevelService.WeaponController.ReloadClip();
            }

            if (Input.GetMouseButton(_mouseButton))
            {
                Services.Instance.LevelService.WeaponController.Fire();
            }
                       
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                Services.Instance.LevelService.WeaponController.MouseScroll(MouseScrollWheel.Up);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                Services.Instance.LevelService.WeaponController.MouseScroll(MouseScrollWheel.Down);
            }            
        }
        
        #endregion
    }
}
