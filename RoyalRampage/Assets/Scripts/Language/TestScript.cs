using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord("Timer");
        //GetComponentInChildren<Text>().text = "Hej";
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
