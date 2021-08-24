using Test.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Test.UI
{
    public class ButtonLoadLevel : MonoBehaviour
    {
        private const int _sceneNumber = 1;
        private Button _button;
        private Text _text;

        private void OnEnable()
        {
            LanguageManager.Instance.OnButtonSwitch += ChangeText;
            _text = GetComponentInChildren<Text>();

            _button = GetComponent<Button>();
            _button.onClick.AddListener(delegate { LoadLevel(_sceneNumber); });
        }
        private void Start()
        {
            ChangeText();
        }

        private void LoadLevel(int number)
        {
            SceneManager.LoadScene(1);
        }

        public void ChangeText()
        {
            _text.text = LanguageManager.Instance.Text("menu_play");
        }

        private void OnDisable()
        {
            LanguageManager.Instance.OnButtonSwitch -= ChangeText;
            _button.onClick.RemoveListener(delegate { LoadLevel(_sceneNumber); });
        }
    }
}
