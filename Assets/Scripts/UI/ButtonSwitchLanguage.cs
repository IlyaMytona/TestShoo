using System;
using Test.Localization;
using UnityEngine;
using UnityEngine.UI;


namespace Test.UI
{
    public class ButtonSwitchLanguage : MonoBehaviour
    {
        private Button _button;
        private Text _text;

        private void OnEnable()
        {
            LanguageManager.Instance.OnButtonSwitch += ChangeText;
            _text = GetComponentInChildren<Text>();

            LanguageManager.Instance.Init("Language");
            ChangeText();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(SwitchLanguage);
        }

        public void SwitchLanguage()
        {

            if (LanguageManager.Instance.LanguageCode == "Ru")
            {
                LanguageManager.Instance.Init("Language", "Eng");
                return;
            }
            if (LanguageManager.Instance.LanguageCode == "Eng")
            {
                LanguageManager.Instance.Init("Language", "Ru");
                return;
            }
        }

        public void ChangeText()
        {
            _text.text = LanguageManager.Instance.Text("menu_settings");
        }

        private void OnDisable()
        {
            LanguageManager.Instance.OnButtonSwitch -= ChangeText;
            _button.onClick.RemoveListener(SwitchLanguage);
        }
    }
}

