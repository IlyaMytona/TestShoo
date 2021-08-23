using System;
using UnityEngine;
using UnityEngine.EventSystems;
#if MOBILE_INPUT
using UnityStandardAssets.CrossPlatformInput;
#endif

namespace Test.Controllers
{
    public class CustomInput : MonoBehaviour
    {
        public event Action<InputDevice>  OnChangeInputType;
        private static CustomInput _instance;
        public static CustomInput Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<CustomInput>();
                    if (_instance == null)
                    {
                        new GameObject("InputType", typeof(CustomInput));
                        return CustomInput.Instance;
                    }
                }
                return _instance;
            }
        }       

        private InputDevice _inputType = InputDevice.MouseKeyboard;
        [HideInInspector]
        public InputDevice InputDevice
        {
            get { return _inputType; }
            set
            {
                _inputType = value;
                OnChangeInput();
            }
        }        

        private bool IsMobileInput()
        {
#if UNITY_EDITOR && UNITY_MOBILE
            if (EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                return true;
            }
		
#elif MOBILE_INPUT
            if (EventSystem.current.IsPointerOverGameObject() || (Input.touches.Length > 0))
                return true;
#endif
            return false;
        }

        private bool IsMouseKeyboard()
        {
#if MOBILE_INPUT
            return false;
#else
            // mouse & keyboard buttons
            if (Event.current.isKey || Event.current.isMouse)
                return true;
            // mouse movement
            if (Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f)
                return true;

            return false;
#endif
        }
        
        void OnChangeInput()
        {
            OnChangeInputType?.Invoke(InputDevice);
        }
    }
    
    [HideInInspector]
    public enum InputDevice
    {
        MouseKeyboard,        
        Mobile
    };

    [System.Serializable]
    public class GenericInput
    {
        protected InputDevice _inputDevice { get { return CustomInput.Instance.InputDevice; } }
        public bool UseInput = true;
        [SerializeField] private bool _isAxisInUse;
        [SerializeField] private string _keyboard;
        [SerializeField] private bool _keyboardAxis;        
        [SerializeField] private string _mobile;
        [SerializeField] private bool _mobileAxis;
        
        [SerializeField] private bool _keyboardAxisInvert;
        [SerializeField] private bool _mobileAxisInvert;

        public float TimeButtonWasPressed;
        public float LastTimeTheButtonWasPressed;
        public bool InButtomTimer;
        private float MultTapTimer;
        private int MultTapCounter;

        public bool IsAxis
        {
            get
            {
                bool value = false;
                switch (_inputDevice)
                {                    
                    case InputDevice.MouseKeyboard:
                        value = _keyboardAxis;
                        break;
                    case InputDevice.Mobile:
                        value = _mobileAxis;
                        break;
                }
                return value;
            }
        }

        public bool IsAxisInvert
        {
            get
            {
                bool value = false;
                switch (_inputDevice)
                {                    
                    case InputDevice.MouseKeyboard:
                        value = _keyboardAxisInvert;
                        break;
                    case InputDevice.Mobile:
                        value = _mobileAxisInvert;
                        break;
                }
                return value;
            }
        }
        
        public GenericInput(string keyboard, string mobile)
        {
            _keyboard = keyboard;
            _mobile = mobile;
        }
        
        public GenericInput(string keyboard, bool keyboardAxis, string mobile, bool mobileAxis)
        {
            _keyboard = keyboard;
            _keyboardAxis = keyboardAxis;            
            _mobile = mobile;
            _mobileAxis = mobileAxis;
        }

        public GenericInput(string keyboard, bool keyboardAxis, bool keyboardInvert, string mobile, bool mobileAxis, bool mobileInvert)
        {
            _keyboard = keyboard;
            _keyboardAxis = keyboardAxis;
            _keyboardAxisInvert = keyboardInvert;            
            _mobile = mobile;
            _mobileAxis = mobileAxis;
            _mobileAxisInvert = mobileInvert;
        }
        
        public string ButtonName
        {
            get
            {
                if (CustomInput.Instance != null)
                {
                    if (CustomInput.Instance.InputDevice == InputDevice.MouseKeyboard) return _keyboard.ToString();                    
                    else return _mobile;
                }
                return string.Empty;
            }
        }
        
        public bool IsKey
        {
            get
            {
                if (CustomInput.Instance != null)
                {
                    if (System.Enum.IsDefined(typeof(KeyCode), ButtonName))
                        return true;
                }
                return false;
            }
        }
        
        public KeyCode Key
        {
            get
            {
                return (KeyCode)System.Enum.Parse(typeof(KeyCode), ButtonName);
            }
        }
        
        public bool GetButton()
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;
            if (IsAxis) return GetAxisButton();

            // mobile
            if (_inputDevice == InputDevice.Mobile)
            {
#if MOBILE_INPUT
                if (CrossPlatformInputManager.GetButton(this.ButtonName))
#endif
                    return true;
            }
            // keyboard/mouse
            else if (_inputDevice == InputDevice.MouseKeyboard)
            {
                if (IsKey)
                {
                    if (Input.GetKey(Key))
                        return true;
                }
                else
                {
                    if (Input.GetButton(this.ButtonName))
                        return true;
                }
            }            
            return false;
        }
        
        public bool GetButtonDown()
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;
            if (IsAxis) return GetAxisButtonDown();
            // mobile
            if (_inputDevice == InputDevice.Mobile)
            {
#if MOBILE_INPUT
                if (CrossPlatformInputManager.GetButtonDown(this.ButtonName))
#endif
                    return true;
            }
            // keyboard/mouse
            else if (_inputDevice == InputDevice.MouseKeyboard)
            {
                if (IsKey)
                {
                    if (Input.GetKeyDown(Key))
                        return true;
                }
                else
                {
                    if (Input.GetButtonDown(this.ButtonName))
                        return true;
                }
            }
            
            return false;
        }
        
        public bool GetButtonUp()
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;
            if (IsAxis) return GetAxisButtonUp();

            // mobile
            if (_inputDevice == InputDevice.Mobile)
            {
#if MOBILE_INPUT
                if (CrossPlatformInputManager.GetButtonUp(this.ButtonName))
#endif
                    return true;
            }
            // keyboard/mouse
            else if (_inputDevice == InputDevice.MouseKeyboard)
            {
                if (IsKey)
                {
                    if (Input.GetKeyUp(Key))
                        return true;
                }
                else
                {
                    if (Input.GetButtonUp(this.ButtonName))
                        return true;
                }
            }            
            return false;
        }
        
        public float GetAxis()
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName) || IsKey) return 0;

            // mobile
            if (_inputDevice == InputDevice.Mobile)
            {
#if MOBILE_INPUT
                return CrossPlatformInputManager.GetAxis(this.ButtonName);
#endif
            }
            // keyboard/mouse
            else if (_inputDevice == InputDevice.MouseKeyboard)
            {
                return Input.GetAxis(this.ButtonName);
            }            
            return 0;
        }
        
        public float GetAxisRaw()
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName) || IsKey) return 0;

            // mobile
            if (_inputDevice == InputDevice.Mobile)
            {
#if MOBILE_INPUT
                return CrossPlatformInputManager.GetAxisRaw(this.ButtonName);
#endif
            }
            // keyboard/mouse
            else if (_inputDevice == InputDevice.MouseKeyboard)
            {
                return Input.GetAxisRaw(this.ButtonName);
            }            
            return 0;
        }
        
        public bool GetDoubleButtonDown(float inputTime = 1)
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;

            if (MultTapCounter == 0 && GetButtonDown())
            {
                MultTapTimer = Time.time;
                MultTapCounter = 1;
                return false;
            }

            if (MultTapCounter == 1 && GetButtonDown())
            {
                var time = MultTapTimer + inputTime;
                var valid = (Time.time < time);
                MultTapTimer = 0;
                MultTapCounter = 0;
                return valid;
            }
            return false;
        }
       
        public bool GetButtonTimer(float inputTime = 2)
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;
            if (GetButtonDown() && !InButtomTimer)
            {
                LastTimeTheButtonWasPressed = Time.time + 0.1f;
                TimeButtonWasPressed = Time.time;
                InButtomTimer = true;
            }
            if (InButtomTimer)
            {
                var time = TimeButtonWasPressed + inputTime;
                var valid = (time - Time.time <= 0);

                if (!GetButton() || LastTimeTheButtonWasPressed < Time.time)
                {
                    InButtomTimer = false;
                    return false;
                }
                else
                {
                    LastTimeTheButtonWasPressed = Time.time + 0.1f;
                }
                if (valid)
                {
                    InButtomTimer = false;
                }
                return valid;
            }
            return false;
        }
        
        public bool GetButtonTimer(ref float currentTimer, float inputTime = 2)
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;
            if (GetButtonDown() && !InButtomTimer)
            {
                LastTimeTheButtonWasPressed = Time.time + 0.1f;
                TimeButtonWasPressed = Time.time;
                InButtomTimer = true;
            }
            if (InButtomTimer)
            {
                var time = TimeButtonWasPressed + inputTime;
                currentTimer = time - Time.time;
                var valid = (time - Time.time <= 0);

                if (!GetButton() || LastTimeTheButtonWasPressed < Time.time)
                {
                    InButtomTimer = false;                    
                    return false;
                }
                else
                {                   
                    LastTimeTheButtonWasPressed = Time.time + 0.1f;
                }
                if (valid)
                {
                    InButtomTimer = false;
                }
                return valid;
            }
            return false;
        }
        
        public bool GetButtonTimer(ref float currentTimer, ref bool upAfterPressed, float inputTime = 2)
        {            
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;
            if (GetButtonDown())
            {
                LastTimeTheButtonWasPressed = Time.time + 0.1f;
                TimeButtonWasPressed = Time.time;
                InButtomTimer = true;
            }
            if (InButtomTimer)
            {
                var time = TimeButtonWasPressed + inputTime;
                currentTimer = (inputTime - (time - Time.time)) / inputTime;
                var valid = (time - Time.time <= 0);

                if (!GetButton() || LastTimeTheButtonWasPressed < Time.time)
                {
                    InButtomTimer = false;
                    upAfterPressed = true;
                    return false;
                }
                else
                {
                    upAfterPressed = false;
                    LastTimeTheButtonWasPressed = Time.time + 0.1f;
                }
                if (valid)
                {
                    InButtomTimer = false;
                }
                return valid;
            }
            return false;
        }
        
        public bool GetAxisButton(float value = 0.5f)
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;
            if (IsAxisInvert) value *= -1f;
            if (value > 0)
            {
                return GetAxisRaw() >= value;
            }
            else if (value < 0)
            {
                return GetAxisRaw() <= value;
            }
            return false;
        }
        
        public bool GetAxisButtonDown(float value = 0.5f)
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;
            if (IsAxisInvert) value *= -1f;
            if (value > 0)
            {
                if (!_isAxisInUse && GetAxisRaw() >= value)
                {
                    _isAxisInUse = true;
                    return true;
                }
                else if (_isAxisInUse && GetAxisRaw() == 0)
                {
                    _isAxisInUse = false;
                }
            }
            else if (value < 0)
            {
                if (!_isAxisInUse && GetAxisRaw() <= value)
                {
                    _isAxisInUse = true;
                    return true;
                }
                else if (_isAxisInUse && GetAxisRaw() == 0)
                {
                    _isAxisInUse = false;
                }
            }
            return false;
        }
        
        public bool GetAxisButtonUp()
        {
            if (string.IsNullOrEmpty(ButtonName) || !IsButtonAvailable(this.ButtonName)) return false;
            if (_isAxisInUse && GetAxisRaw() == 0)
            {
                _isAxisInUse = false;
                return true;
            }
            else if (!_isAxisInUse && GetAxisRaw() != 0)
            {
                _isAxisInUse = true;
            }
            return false;
        }

        bool IsButtonAvailable(string btnName)
        {
            if (!UseInput) return false;
            try
            {
                if (IsKey) return true;
                Input.GetButton(ButtonName);
                return true;
            }
            catch (System.Exception exc)
            {
                Debug.LogWarning(" NO access button :" + ButtonName + "\n" + exc.Message);
                return false;
            }
        }
    }
}