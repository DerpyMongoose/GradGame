using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 Player manager with states and level timer, input should connect here
 */
public class PlayerStates : MonoBehaviour {

	private enum PlayerState {
		READY, IDLE, WALKING, ATTACKING, ENDING }

	PlayerState state = PlayerState.READY;
	float timeLeftInLevel = 0f; //timeleft to complete the level
	Text timerText;


	void Awake(){
		//DontDestroyOnLoad (gameObject);
		//update timer
		timerText = GameObject.Find ("TimeLeftText").GetComponent<Text> ();
		timeLeftInLevel = GameManager.instance.levelManager.timeToCompleteLevel;
		timerText.text = "Timer: " + timeLeftInLevel.ToString("F1");
	}

	void Update () {
		//update level timer
		switch (state) {
		case PlayerState.IDLE:
		case PlayerState.WALKING:
		case PlayerState.ATTACKING:
			timeLeftInLevel -= Time.deltaTime;
			timerText.text = "Timer: " + timeLeftInLevel.ToString("F1");

			//when timer runs out:
			if (timeLeftInLevel <= 0f) {
				timerText.text = "Timer: 0";
				timerText.color = Color.red;
				state = PlayerState.ENDING;
				GameManager.instance.timerOut ();
			}
			break;
		}

		switch (state) {
		//until player touches the screen to start playing the level
		case PlayerState.READY:
			if (Input.GetKey (KeyCode.R)) {
				timeLeftInLevel = 10f;
				state = PlayerState.IDLE;
				GameManager.instance.timerStart ();
			}
			break;

		case PlayerState.IDLE:
			break;

		case PlayerState.WALKING:
			break;

		case PlayerState.ATTACKING:
			break;

		//after timer is out - losing/winning ending
		case PlayerState.ENDING:
			break;
		}
	}
}
