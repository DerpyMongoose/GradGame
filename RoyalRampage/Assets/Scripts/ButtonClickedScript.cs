using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonClickedScript : MonoBehaviour {

    Color pressed = new Color(0.97f, 0.76f,0.15f);
    Color notPressed = new Color(0.79f, 0.75f, 0.63f);
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
