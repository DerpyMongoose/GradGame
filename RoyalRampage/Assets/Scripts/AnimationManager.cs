using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {


    // animation variables
    private bool isSpinning = false;
    private bool isIdle = false;
    private bool isStomping = false;
    private bool isDashing = false;

    private Animator playerAnim,
        scepterAnim;
    
	//dash
    void PlayerDashAnim(){
        playerAnim.SetTrigger("dash_trig");
        //scepterAnim.SetTrigger("scepterdash_trig");
    }
	void PlayerDashRecoilAnim(){
		playerAnim.SetTrigger("dash_end_trig");
		//scepterAnim.SetTrigger("scepterdash_trig");
	}

	// Hits
	void PlayerHitAnim(){
		playerAnim.SetTrigger ("has_hit");
		//scepterAnim.SetTrigger("scepter_hasHitTrig");
	} 

	// Spin
	void PlayerSpinAnim(){
		playerAnim.SetTrigger ("spin_trig");
	}
	// Stomp
	void PlayerStompAnim(){
		playerAnim.SetTrigger ("stomp_trig");
	}

	void OnEnable(){
		playerAnim = GameObject.FindGameObjectWithTag("queen").GetComponent<Animator>();
        scepterAnim = GameObject.FindGameObjectWithTag("scepter").GetComponent<Animator>();
		print (playerAnim);
		print (scepterAnim);

        GameManager.instance.OnPlayerDash += PlayerDashAnim;
		GameManager.instance.OnPlayerHit += PlayerHitAnim;
		GameManager.instance.OnPlayerSwirl += PlayerSpinAnim;
		GameManager.instance.OnPlayerStomp += PlayerStompAnim;

	}

	void OnDisable(){
        GameManager.instance.OnPlayerDash -= PlayerDashAnim;
		GameManager.instance.OnPlayerHit -= PlayerHitAnim;
		GameManager.instance.OnPlayerSwirl -= PlayerSpinAnim;
		GameManager.instance.OnPlayerStomp -= PlayerStompAnim;
    }

}
