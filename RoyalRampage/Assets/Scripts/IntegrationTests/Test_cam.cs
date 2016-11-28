using UnityEngine;
using System.Collections;

public class Test_cam : MonoBehaviour {
	
	Transform Player;

	void Start(){
		Player = GameManager.instance.player.transform;
	}
	void Update () {
		if(Player.position.x >= -5f){
			Player.position += new Vector3(-0.8f*Time.deltaTime,0,0);
		}
	}
}
