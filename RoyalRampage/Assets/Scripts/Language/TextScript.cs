using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextScript : MonoBehaviour {

    public string key = "";

    void OnEnable() {
        LanguageManager.instance.ChangeText += changeText;
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key);
    }

    void OnDisable () {
        LanguageManager.instance.ChangeText -= changeText;
    }


    private void changeText () {
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key);
    }
}
