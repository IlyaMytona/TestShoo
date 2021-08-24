using UnityEngine;


namespace Test.UI
{
    public class MobileControlsActivator : MonoBehaviour
    {
        public GameObject LeftAnalogButtonMove;
        public GameObject CombatButtons;
        public GameObject TakeWeaponButtons;
        public GameObject RightAnalogButtonRotate;

#if UNITY_IOS || UNITY_ANDROID
        private void Awake()
        {
            SetMobileControlsActive(true);
        }

        public virtual void SetMobileControlsActive(bool state)
        {
            LeftAnalogButtonMove.gameObject.SetActive(state);
            CombatButtons.gameObject.SetActive(state);
            TakeWeaponButtons.gameObject.SetActive(state);
            RightAnalogButtonRotate.gameObject.SetActive(state);
        }
#endif
    }

}

