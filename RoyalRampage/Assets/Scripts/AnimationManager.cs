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
    
    void PlayerDashAnim()
    {

        //playerAnim.SetBool("is_dashing", true);
        playerAnim.SetTrigger("dash_trig");
        scepterAnim.SetTrigger("scepterdash_trig");

    }

	void OnEnable(){
        playerAnim = GameManager.instance.player.transform.GetChild(0).GetComponent<Animator>();
        scepterAnim = GameObject.FindGameObjectWithTag("Scepter").GetComponent<Animator>();
        print(scepterAnim);

        GameManager.instance.OnPlayerDash += PlayerDashAnim;

	}

	void OnDisable(){
        GameManager.instance.OnPlayerDash -= PlayerDashAnim;
    }

}
