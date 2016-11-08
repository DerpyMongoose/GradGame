using UnityEngine;
using System.Collections;

/*
 What happens when player tries to destruct an object
 */
public class Destruct : MonoBehaviour {

	void OnCollisionEnter(Collision hit){
		if (hit.gameObject.tag == "Destructable") {
			GameManager.instance.objectDestructed (hit.gameObject);
			//DestructObject (hitObject.gameObject);

		}
	}

	private void DestructObject(GameObject ObjToDestruct){
		Destroy (ObjToDestruct);
	}
}
