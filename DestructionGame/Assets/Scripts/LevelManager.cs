using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 level and score manager
 Used for public variables to be tweaked by level designer 
 */
public class LevelManager : MonoBehaviour {

	[SerializeField]
	int scoreToCompleteLevel = 10;
	public int timeToCompleteLevel = 10;

	int score = 0;
	Text scoreText;
	Text minScoreText;
	Text guideText;

	void Awake(){
		GameManager.instance.OnObjectDestructed += IncreaseScore;
		GameManager.instance.OnTimerStart += StartLevel;
		GameManager.instance.OnTimerOut += ShowEnding;

		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
		scoreText.text = "Score: " + score;
		minScoreText = GameObject.Find ("MinScoreText").GetComponent<Text> ();
		minScoreText.text = "Reach " + scoreToCompleteLevel + " to Win";
		guideText = GameObject.Find ("GuideText").GetComponent<Text> ();
		guideText.text = "Press R to start the level and hit objects";
	}

	void OnDisable(){
		GameManager.instance.OnObjectDestructed -= IncreaseScore;
		GameManager.instance.OnTimerStart -= StartLevel;
		GameManager.instance.OnTimerOut -= ShowEnding;
	}

	private void IncreaseScore(GameObject destructedObj){
        int points = destructedObj.GetComponent<Destructable>().pointsForDestruction;
		score += points;
		scoreText.text = "Score: " + score;
		GameManager.instance.score = score;
	}

	private void StartLevel(){
		guideText.gameObject.SetActive (false);
	}

	private void ShowEnding(){
		if (score >= scoreToCompleteLevel)
			guideText.text = "Level completed!";
		else
			guideText.text = "Game over";

		guideText.gameObject.SetActive (true);
	}

	public void GoToStore(){
		GameManager.instance.GoToStore ();
	}
}
