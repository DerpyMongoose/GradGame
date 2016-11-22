using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour {

	public void ChangeLanguageFunction (int index) {
        LanguageManager.instance.languageSelect = index;
        LanguageManager.instance.changeText();
    }
}
