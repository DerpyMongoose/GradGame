 using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	[Header ("-- Player --")]
	//[SerializeField]
	public string dashPlayer;
	//[SerializeField]
	public string spinPlay, 
		stompPlay;

	[Header ("-- Objects --")]
	//[SerializeField]
	public string actionPlay;
	public string landing;
	private string hit;
	private string destruction;


	//[Header ("-- Background --")]
	//[SerializeField]
	private string ambiencePlay,
		MusicSystem;


	//[Header ("-- Menu --")]
	//[SerializeField]
	private string menuButton;
	//[SerializeField]
	private string startButton;

	//[Header ("-- Score Screen --")]
	//[SerializeField]
	private string scoreScreenOpen;
	//[SerializeField]
	private string starCounting;
	private string pointsCounting;

	//[Header ("-- In Game --")]
	//[SerializeField]
	private string timerTick;
	//[SerializeField]
	private string pointsRewarded, 
		objectiveAnnounced,
		objectiveCompleted;

	//************* Player *************

	void PlayerDash(){
		PlaySound (dashPlayer, GameManager.instance.player);
	}

	void PlayerSwirl(){
		PlaySound (spinPlay, GameManager.instance.player);
	}
	void PlayerStomp(){
		PlaySound (stompPlay, GameManager.instance.player);
	}

	/*private static bool showHexTypes = true;
	public override void OnInspectorGUI () {
		showHexTypes = EditorGUILayout.Foldout(showHexTypes, "HexTypes");

		if (showHexTypes) {
			EditorGUILayout.PropertyField (landing);
			EditorGUILayout.PropertyField (hit);
			EditorGUILayout.PropertyField (destruction);
		}
	}*/

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

	private void PlaySound(string eventName, GameObject audioSource){
		if(eventName == null || eventName == "")
			return;

		AkSoundEngine.PostEvent (eventName, audioSource);
		AkSoundEngine.RenderAudio ();
	}

	//Subscribing

	void OnEnable(){
		GameManager.instance.OnPlayerDash += PlayerDash;
		GameManager.instance.OnPlayerSwirl += PlayerSwirl;
		GameManager.instance.OnPlayerStomp += PlayerStomp;
	}

	void OnDisable(){
		GameManager.instance.OnPlayerDash -= PlayerDash;
		GameManager.instance.OnPlayerSwirl -= PlayerSwirl;
		GameManager.instance.OnPlayerStomp -= PlayerStomp;
	}
}
