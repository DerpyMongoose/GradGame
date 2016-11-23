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
	private ParticleSystem spinParticle, stompParticle;
    
	//dash
    void PlayerDashAnim(){
        playerAnim.SetTrigger("dash_trig");
		scepterAnim.SetTrigger("dash_trig");
    }
	void PlayerDashRecoilAnim(){
		playerAnim.SetTrigger("dash_end_trig");
		scepterAnim.SetTrigger("dash_end_trig");
	}

	// Hits
	void PlayerHitAnim(){
		playerAnim.SetTrigger ("has_hit");
		scepterAnim.SetTrigger("has_hit");
	} 

	// Spin
	void PlayerSpinAnim(){
		playerAnim.SetTrigger ("spin_trig");
		scepterAnim.SetTrigger ("spin_trig");
		PlaySpinParticle ();
	}
	public void PlaySpinParticle(){
		spinParticle.Play ();
	}
	// Stomp
	void PlayerStompAnim(){
		playerAnim.SetTrigger ("stomp_trig");
		scepterAnim.SetTrigger ("stomp_trig");
	}
	public void PlayStompParticle(){
		stompParticle.Play ();
	}

	void OnEnable(){
		playerAnim = GameObject.FindGameObjectWithTag("queen").GetComponent<Animator>();
        scepterAnim = GameObject.FindGameObjectWithTag("scepter").GetComponent<Animator>();
		spinParticle = GameObject.FindGameObjectWithTag("spinParticle").GetComponent<ParticleSystem>();
		stompParticle = GameObject.FindGameObjectWithTag("stompParticle").GetComponent<ParticleSystem>();

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
