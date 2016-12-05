using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class liftMassTest : MonoBehaviour {

	private bool hasStomped = false;
	private GameObject player;
	void Start(){
		player = GameObject.Find("Player");
	}

	void Update () {
		if (hasStomped != true){
			StompTest ();
			hasStomped = true;
		}
	}

	void StompTest(){
		GameObject.Find("InLevelCanvas").SetActive(false);

		List<Collider> colliders = new List<Collider>();
		List<Rigidbody> rbs = new List<Rigidbody>();
		List<float> masses = new List<float>();

		GameObject Player;
		Collider[] hitColliders = null;
		hitColliders = Physics.OverlapSphere(transform.position, player.GetComponent<PlayerStates>().liftRadius);
		//Lift(hitColliders); //RUN FROM ANIMATION EVENT
		for (int i = 0; i < hitColliders.Length; i++) {
			if (hitColliders[i].tag == "Destructable")
			{
				colliders.Add(hitColliders[i]);
			}
		}
		player.GetComponent<SwipeHalf> ().tempColliders = colliders;
		for(int i = 0 ; i < colliders.Count; i++){
			rbs.Add(colliders [i].gameObject.GetComponent<Rigidbody> ());
			masses.Add (rbs [i].mass);
		}

		player.GetComponent<SwipeHalf> ().Lift ();

		for(int i = 0 ; i < colliders.Count; i++){
			if(colliders[i].GetComponent<Rigidbody>().mass != 1.0f){
				GetComponent<liftMassTest> ().enabled = false;
				break;
			}
		}

		//vfall
		player.GetComponent<SwipeHalf> ().Reverse (rbs,masses);

		for(int i = 0 ; i < colliders.Count; i++){
			if(colliders[i].GetComponent<Rigidbody>().mass == 1.0f){
				GetComponent<liftMassTest> ().enabled = false;
				break;
			}
		}
	}
}
