using UnityEngine;
using System.Collections;

public class Test_cam : MonoBehaviour {
	
	Transform Player;

	void Start(){
		Player = GameManager.instance.player.transform;
	}
	void Update () {
		if(Player.position.x >= -3.5){
			Player.position += new Vector3(-0.3f*Time.deltaTime,0,0);
		}
	}
}
