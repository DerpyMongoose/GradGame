using UnityEngine;
using System.Collections;

public class AutoPowerupScript : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		var scale = other.transform.localScale;
		other.transform.localScale = scale * 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
