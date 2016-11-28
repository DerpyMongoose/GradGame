using UnityEngine;
using System.Collections;

public class QueenAnim : MonoBehaviour {

	public void PlaySpinParticle(){
		GameManager.instance.animationManager.PlaySpinParticle ();
	}
	public void PlayStompParticle(){
		GameManager.instance.animationManager.PlayStompParticle ();
		GameManager.instance.player.GetComponent<SwipeHalf> ().Lift();
        if (GameManager.instance.player.GetComponent<SwipeHalf>().tempColliders != null)
        {
            GameManager.instance.player.GetComponent<SwipeHalf>().tempColliders.Clear();
        }
    }

    public void SwirlFinished()
    {
        GameManager.instance.player.GetComponent<SwipeHalf>().swirlEnded = true;
        GameManager.instance.player.GetComponent<SwipeHalf>().Swirling();
        if (GameManager.instance.player.GetComponent<SwipeHalf>().tempColliders != null)
        {
            GameManager.instance.player.GetComponent<SwipeHalf>().tempColliders.Clear();
        }
    }
}
