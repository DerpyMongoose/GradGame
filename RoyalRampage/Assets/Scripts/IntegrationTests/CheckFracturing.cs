using UnityEngine;
using System.Collections;

public class CheckFracturing : MonoBehaviour {

	public int num_of_fractured_objects = 0;
	private FracturedObject [] fractured;

	void Start () {
		fractured = GameObject.FindObjectsOfType<FracturedObject>();
		num_of_fractured_objects = fractured.Length;
		print("--- Number of small fractured objects: " + fractured.Length + "---");

		if(num_of_fractured_objects > 0)
			GetComponent<CheckFracturing> ().enabled = false;
	}
}
