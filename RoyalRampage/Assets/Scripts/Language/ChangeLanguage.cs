using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour {

	public void ChangeLanguageFunction (int index) {
		GameManager.instance.menuButtonClicked();
        LanguageManager.instance.languageSelect = index;
        LanguageManager.instance.changeText();
    }
}
