using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Test.UI
{
	public class WeaponUiText : MonoBehaviour
	{
        #region Private Data

        private TMP_Text _text;
		private Image _iconImage;
        private TMP_Text _textEndGame;
        #endregion


        #region Unity Methods

        private void Start()
		{
			_text = GetComponent<TMP_Text>();
			_iconImage = GetComponentInChildren<Image>();
		}

        #endregion


        #region Methods

        public void ShowData(int countAmmunition, int countClip)
		{
			_text.text = $"{countAmmunition}/{countClip}";
		}

		public void SetActive(bool value, Sprite icon)
		{
			_text.gameObject.SetActive(value);
            if (value)
            {
				_iconImage.sprite = icon;
			}
            else
            {
				_iconImage.sprite = null;
			}
		}

        public void SetPanelEndLevelActive(bool isActive)
        {
            var panel = GameObject.FindWithTag("PanelEndLevel");
            if (panel == null)
                return;
            panel.transform.GetChild(0).gameObject.SetActive(isActive);
            _textEndGame = panel.transform.GetChild(1).GetComponent<TMP_Text>();
            _textEndGame.gameObject.SetActive(isActive);
            if (isActive)
            {                
                _textEndGame.text = "Congratulations! You Lose!";
            }
        }
        
        #endregion
    }
}