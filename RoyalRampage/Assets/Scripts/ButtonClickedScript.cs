using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonClickedScript : MonoBehaviour {

    Color pressed = new Color(1f, 1f,1f);
    Color notPressed = new Color(0.39f, 0.39f, 0.39f);
    public int index;
	// Use this for initialization
	void Start () {
	    if(index == LanguageManager.instance.languageSelect) {
            GetComponent<Image>().color = pressed;
        } else {
            GetComponent<Image>().color = notPressed;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (index == LanguageManager.instance.languageSelect) {
            GetComponent<Image>().color = pressed;
        } else {
            GetComponent<Image>().color = notPressed;
        }
    }
}
