using UnityEngine;
using System.Collections;

public class SetResolution : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.SetResolution(Screen.width / 2, Screen.height / 2, true);
    }
	
}
