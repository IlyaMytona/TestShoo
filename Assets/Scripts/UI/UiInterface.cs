using UnityEngine;


namespace Test.UI
{    
	public class UiInterface
	{		
		private WeaponUiText _weaponUiText;        
		public WeaponUiText WeaponUiText
		{
			get
			{
				if (!_weaponUiText)
					_weaponUiText = Object.FindObjectOfType<WeaponUiText>();
				return _weaponUiText;
			}
		}		
	}
}