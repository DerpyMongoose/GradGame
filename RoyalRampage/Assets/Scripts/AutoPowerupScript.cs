using UnityEngine;
using System.Collections;

public class AutoPowerupScript : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		var scale = other.transform.localScale;
		other.transform.localScale = scale * 2;
	}

	public AudioManager findSth(){
		AudioManager cam = null;
		cam = GameObject.FindObjectOfType<AudioManager>() as AudioManager;
		Debug.Log(cam);
		return cam;
	}
}
