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
		//if hasn't run the stomp, run now
		if (hasStomped != true){
			StompTest ();
			hasStomped = true;
		}
	}

	void StompTest(){
		//disable level canvas to see the level
		GameObject.Find("InLevelCanvas").SetActive(false);

		//prepare arrays to save masse, colliders, rigidbodies
		List<Collider> colliders = new List<Collider>();
		List<Rigidbody> rbs = new List<Rigidbody>();
		List<float> masses = new List<float>();

		//find all objects to lifted on stomp (code copied from original stomp function)
		Collider[] hitColliders = null;
		hitColliders = Physics.OverlapSphere(transform.position, player.GetComponent<PlayerStates>().liftRadius);
		//Lift(hitColliders); //RUN FROM ANIMATION EVENT
		for (int i = 0; i < hitColliders.Length; i++) {
			if (hitColliders[i].tag == "Destructable")
			{
				colliders.Add(hitColliders[i]);
			}
		}

		//update tempcolliders (objects to lift) in alex's swipe half script 
		player.GetComponent<SwipeHalf> ().tempColliders = colliders;
		//save initial rigidbody masses for later check
		for(int i = 0 ; i < colliders.Count; i++){
			rbs.Add(colliders [i].gameObject.GetComponent<Rigidbody> ());
			masses.Add (rbs [i].mass);
		}
		//lift objects
		player.GetComponent<SwipeHalf> ().Lift ();
		//check whether masses changed to 1
		for(int i = 0 ; i < colliders.Count; i++){
			if(colliders[i].GetComponent<Rigidbody>().mass != 1.0f){
				GetComponent<liftMassTest> ().enabled = false;
				break;
			}
		}

		//call reverse function to let objects fall down back
		player.GetComponent<SwipeHalf> ().Reverse (rbs,masses);

		//check whether mases reset to initial, compare to masses array that we saved earlier
		for(int i = 0 ; i < colliders.Count; i++){
			if(colliders[i].GetComponent<Rigidbody>().mass == 1.0f){
				GetComponent<liftMassTest> ().enabled = false;
				break;
			}
		}
	}
}
