using UnityEngine;
using System.Collections;

public class SetResolution : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.SetResolution((int)(Screen.width/1.5f), (int)(Screen.height/1.5f), true);
    }
	
}
