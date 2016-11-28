using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetReachGoalScript : MonoBehaviour {

    public static SetReachGoalScript instance;
    private int thisInput;
    public string key = "";

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void OnEnable() {
        LanguageManager.instance.ChangeText += changeText;
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key) + " " + thisInput.ToString() + "$";
    }

    void OnDisable() {
        LanguageManager.instance.ChangeText -= changeText;
    }


    private void changeText() {
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key) + " " + thisInput.ToString() + "$";
    }

    public void SetText(int input) {
        thisInput = input;
        GetComponent<Text>().text = LanguageManager.instance.ReturnWord(key) + " " + thisInput.ToString() + "$";
    }
}
