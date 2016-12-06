using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GetHighScoreForLevel : MonoBehaviour {

    List<LevelAndObjects> highScore;
    public string level;

	// Use this for initialization
	void OnEnable () {
        highScore = new List<LevelAndObjects>();
        gameObject.GetComponent<Text>().text = SaveHighScore.instance.ReturnListWithObjects(level)[0].HighScore.ToString();
	}
	
}
