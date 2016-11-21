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
    
    void PlayerDashAnim(){
        playerAnim.SetTrigger("dash_trig");
        scepterAnim.SetTrigger("scepterdash_trig");
    }

	// Hits
	void PlayerHitAnim(){
		playerAnim.SetTrigger ("has_hit");
		scepterAnim.SetTrigger("scepter_hasHitTrig");
	} 

	// Spin
	void PlayerSpinAnim(){
		playerAnim.SetTrigger ("spin_trig");
	}
	void PlayerSpinAnimStop(){
		playerAnim.SetBool ("is_spinning", false);
	}
	// Stomp
	void PlayerStompAnim(){
		playerAnim.SetTrigger ("stomp_trig");
	}

	void OnEnable(){
        playerAnim = GameManager.instance.player.transform.GetChild(0).GetComponent<Animator>();
        scepterAnim = GameObject.FindGameObjectWithTag("scepter").GetComponent<Animator>();
        print(scepterAnim);

        GameManager.instance.OnPlayerDash += PlayerDashAnim;
		GameManager.instance.OnPlayerHit += PlayerHitAnim;
		GameManager.instance.OnPlayerSwirl += PlayerSpinAnim;

	}

	void OnDisable(){
        GameManager.instance.OnPlayerDash -= PlayerDashAnim;
		GameManager.instance.OnPlayerHit -= PlayerHitAnim;
		GameManager.instance.OnPlayerSwirl -= PlayerSpinAnim;
    }

}
