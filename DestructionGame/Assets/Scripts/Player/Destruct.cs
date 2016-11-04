using UnityEngine;
using System.Collections;

/*
 What happens when player tries to destruct an object
 */
public class Destruct : MonoBehaviour {

	void OnTriggerEnter(Collider hitObject){
		if (hitObject.gameObject.tag == "Destructable") {
			GameManager.instance.objectDestructed (hitObject.gameObject);
			DestructObject (hitObject.gameObject);

		}
	}

	private void DestructObject(GameObject ObjToDestruct){
		Destroy (ObjToDestruct);
	}
}
