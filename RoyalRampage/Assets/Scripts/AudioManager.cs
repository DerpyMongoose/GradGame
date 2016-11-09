 using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	[Header ("-- Player --")]
	[SerializeField]
	private string dashPlayer;
	[SerializeField]
	private string spinPlay, 
		StompPlay;

	[Header ("-- Objects --")]
	[SerializeField]
	private string actionPlay;

	[Header ("-- Background --")]
	[SerializeField]
	private string ambiencePlay,
		MusicSystem;


	[Header ("-- Menu --")]
	[SerializeField]
	private string menuButton;
	[SerializeField]
	private string startButton;

	[Header ("-- Score Screen --")]
	[SerializeField]
	private string scoreScreenOpen;
	[SerializeField]
	private string starCounting;
	private string pointsCounting;

	[Header ("-- In Game --")]
	[SerializeField]
	private string timerTick;
	[SerializeField]
	private string pointsRewarded, 
		objectiveAnnounced,
		objectiveCompleted;

	//************* Player *************

	void PlayerDash(){
		PlaySound (dashPlayer, GameManager.instance.player);
	}

	void PlaySpin(){
		PlaySound (spinPlay, GameManager.instance.player);
	}

	//************** Objects **************

	void ObjectAction(){

	}

	//**************Background **************
	void BackgroundAmbience(){

	}

	void BackgroundMusic(){

	}

	//************** Menus **************
	void MenuButtons(){

	}

	void StartButton(){

	}

	//************** Score screen **************
	void ScoreScreenOpen(){

	}

	void CountingStars(){

	}

	void CountingPoints(){

	}

	//************** In Game **************
	void TickingTime(){

	}

	void RewardingPoints(){

	}

	void AnnouncingObjective(){

	}

	void CompletedObjective(){

	}

	//****** play sound ****************

	private void PlaySound(string eventName, GameObject obj){
		if(eventName == null || eventName == "")
			return;

		AkSoundEngine.PostEvent (eventName, obj);
		AkSoundEngine.RenderAudio ();
	}

	public void UnloadSoundBank(){
		Destroy (gameObject);
	}

	/// <summary>
	/// Subscribing
	/// </summary>

	void OnEnable(){

	}

	void OnDisable(){

	}
}
