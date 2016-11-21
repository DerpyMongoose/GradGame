using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CurrencyUIScript : MonoBehaviour {

    public string key = "";

    void OnEnable() {
        LanguageManager.instance.ChangeText += changeText;
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key) + " " + GameManager.instance.currency;
    }

    void OnDisable() {
        LanguageManager.instance.ChangeText -= changeText;
    }

    public void changeText() {
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key) + " " + GameManager.instance.currency;
    }
}
