using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextScript : MonoBehaviour {

    public string key = "";

    void OnEnable() {
        LanguageManager.instance.ChangeText += changeText;
    }

    void OnDisable () {
        LanguageManager.instance.ChangeText -= changeText;
    }

	// Use this for initialization
	void Start () {
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key);
    }

    private void changeText () {
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key);
    }
}
