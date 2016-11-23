using UnityEngine;
using System.Collections;

public class QueenAnim : MonoBehaviour {

	public void PlaySpinParticle(){
		GameManager.instance.animationManager.PlaySpinParticle ();
	}
	public void PlayStompParticle(){
		GameManager.instance.animationManager.PlayStompParticle ();
	}
}
