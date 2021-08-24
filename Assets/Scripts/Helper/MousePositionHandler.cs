using Test.Controllers;
using UnityEngine;
#if UNITY_IOS || UNITY_ANDROID
using UnityStandardAssets.CrossPlatformInput;
#endif


namespace Test.Helper
{
    public class MousePositionHandler : MonoBehaviour
    {
#if UNITY_IOS || UNITY_ANDROID
        private Vector2 _joystickMousePos;
        private float _joystickSensitivity = 15.0f;
#endif
        public Camera mainCamera;
        protected static MousePositionHandler _instance;
        public static MousePositionHandler Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<MousePositionHandler>();
                if (_instance == null)
                {
                    var go = new GameObject("MousePositionHandler");
                    _instance = go.AddComponent<MousePositionHandler>();
                    _instance.mainCamera = Camera.main;
                }
                return _instance;
            }
        }
        
        public Vector2 MousePosition
        {
            get
            {
                var inputDevice = CustomInput.Instance.InputDevice;
                switch (inputDevice)
                {
                    case InputDevice.MouseKeyboard:
                        return Input.mousePosition;                    
                    case InputDevice.Mobile:
#if UNITY_IOS || UNITY_ANDROID
                        _joystickMousePos.x += CrossPlatformInputManager.GetAxis("Mouse X") * _joystickSensitivity;
                        _joystickMousePos.x = Mathf.Clamp(_joystickMousePos.x, -(Screen.width * 0.5f), (Screen.width * 0.5f));
                        _joystickMousePos.y += CrossPlatformInputManager.GetAxis("Mouse Y") * _joystickSensitivity;
                        _joystickMousePos.y = Mathf.Clamp(_joystickMousePos.y, -(Screen.height * 0.5f), (Screen.height * 0.5f));
                        var mobileScreenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                        var mobileResult = _joystickMousePos + mobileScreenCenter;
                        mobileResult.x = Mathf.Clamp(mobileResult.x, 0, Screen.width);
                        mobileResult.y = Mathf.Clamp(mobileResult.y, 0, Screen.height);
                        return mobileResult;
#else
                        return Input.GetTouch(0).deltaPosition;
#endif

                    default: return Input.mousePosition;
                }
            }
        }

        public Vector3 WorldMousePosition(LayerMask castLayer)
        {
            Ray ray = mainCamera.ScreenPointToRay(MousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, mainCamera.farClipPlane, castLayer)) return hit.point;
            else return ray.GetPoint(mainCamera.farClipPlane);
        }
    }
}