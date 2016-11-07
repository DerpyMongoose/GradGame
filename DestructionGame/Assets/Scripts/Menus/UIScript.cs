using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {

	public void BackToGame(){
		GameManager.instance.BackToGame ();
	}
}
