using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class liftMassTest : MonoBehaviour {

	private bool hasStomped = false;

	void Update () {
		if (hasStomped != true){
			StompTest ();
			hasStomped = true;
		}
	}

	void StompTest(){
		print ("run corous");
		GameObject.Find("InLevelCanvas").SetActive(false);

		List<Collider> colliders = new List<Collider>();
		List<Rigidbody> rbs = new List<Rigidbody>();
		List<float> masses = new List<float>();

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, GameManager.instance.player.GetComponent<PlayerStates>().liftRadius);
		//Lift(hitColliders); //RUN FROM ANIMATION EVENT
		for (int i = 0; i < hitColliders.Length; i++) {
			if (hitColliders[i].tag == "Destructable")
			{
				colliders.Add(hitColliders[i]);
			}
		}
		GameManager.instance.player.GetComponent<SwipeHalf> ().tempColliders = colliders;
		for(int i = 0 ; i < colliders.Count; i++){
			rbs.Add(colliders [i].gameObject.GetComponent<Rigidbody> ());
			masses.Add (rbs [i].mass);
		}

		GameManager.instance.player.GetComponent<SwipeHalf> ().Lift ();

		for(int i = 0 ; i < colliders.Count; i++){
			if(colliders[i].GetComponent<Rigidbody>().mass != 1.0f){
				GetComponent<liftMassTest> ().enabled = false;
				break;
			}
		}

		//vfall
		GameManager.instance.player.GetComponent<SwipeHalf> ().Reverse (rbs,masses);

		for(int i = 0 ; i < colliders.Count; i++){
			if(colliders[i].GetComponent<Rigidbody>().mass == 1.0f){
				GetComponent<liftMassTest> ().enabled = false;
				break;
			}
		}
	}
}
