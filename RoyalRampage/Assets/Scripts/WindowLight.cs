using UnityEngine;
using System.Collections;

public class WindowLight : MonoBehaviour {

	GameObject window;
	GameObject lightWhole;
	GameObject lightBroken;

	void Start () {
		window = transform.parent.GetChild (0).gameObject;
		if (window == gameObject) {
			window = transform.parent.GetChild (1).gameObject;
		}
		lightWhole = transform.FindChild ("windowSpotlightWhole").gameObject;
		lightBroken = transform.FindChild ("windowSpotlightBroken").gameObject;

		lightBroken.SetActive (false);
	}

	void ChangeLightToBroken(GameObject destructedObj){
		if (destructedObj == window) {
			lightBroken.SetActive (true);
			lightWhole.SetActive (false);
		}
	}
	
	void OnEnable(){
		GameManager.instance.OnObjectDestructed += ChangeLightToBroken;
	}

	void OnDisable(){
		GameManager.instance.OnObjectDestructed -= ChangeLightToBroken;
	}
}
