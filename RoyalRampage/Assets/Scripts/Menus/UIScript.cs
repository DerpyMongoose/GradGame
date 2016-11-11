using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    void Awake() {
        GameManager.instance.Load();
        //set up the scene when opened
        switch (GameManager.instance.CurrentScene()) {
            case GameManager.Scene.INTRO:
                GameObject replayPanel = GameObject.Find("replayPanel");
                replayPanel.SetActive(false);
            break;

            case GameManager.Scene.GAME_OVER_NEXT_LEVEL:
                GameObject playBtn = GameObject.Find("PlayButton");
                playBtn.SetActive(false);
                GameObject ReplayBTN = GameObject.Find("replayPanel/ReplayButton");
                ReplayBTN.SetActive(false);
                Text scoreText = GameObject.Find("replayPanel/score").GetComponent<Text>();
                scoreText.text = "Score: " + GameManager.instance.score;

                Text levelNum = GameObject.Find("replayPanel/NewLevelButton/levelnumber").GetComponentInChildren<Text>();
                if (GameManager.instance.levelsUnlocked < GameManager.instance.NUM_OF_LEVELS_IN_GAME) {
                    GameManager.instance.levelsUnlocked++;
                }
                if (GameManager.instance.currentLevel < GameManager.instance.NUM_OF_LEVELS_IN_GAME) {
                    levelNum.text = (GameManager.instance.currentLevel + 1).ToString();
                } else {
                    levelNum.text = GameManager.instance.currentLevel.ToString() + "*";
                }
            break;

            case GameManager.Scene.GAME_OVER_REPLAY:
                playBtn = GameObject.Find("PlayButton");
                playBtn.SetActive(false);
                GameObject NextLevelBTN = GameObject.Find("replayPanel/NewLevelButton");
                NextLevelBTN.SetActive(false);
                scoreText = GameObject.Find("replayPanel/score").GetComponent<Text>();
                scoreText.text = "Score: " + GameManager.instance.score;
            break;

            case GameManager.Scene.LEVELS_OVERVIEW:
                //set the correct sprite on level icon
                Sprite unlockedSprite = GetComponent<MenuPublics>().unlockedSprite;
                Sprite lockedSprite = GetComponent<MenuPublics>().lockedSprite;
                for(int i = 1; i <= 6; i++) {
                    Image levelIcon = GameObject.Find("LevelInGame/Level" + i).GetComponent<Image>();
                    if (i <= GameManager.instance.levelsUnlocked)
                        levelIcon.sprite = unlockedSprite;
                    else
                        levelIcon.sprite = lockedSprite;
                }
            break;
        }
    }

	public void BackToGame(){
		GameManager.instance.BackToGame ();
	}

    public void BackToPreviousScreen() {
        GameManager.instance.BackToPreviousScene();
    }

    public void ToNextLevel() {
        int next_level;
        if (GameManager.instance.currentLevel < GameManager.instance.NUM_OF_LEVELS_IN_GAME) {
            next_level = GameManager.instance.currentLevel + 1;
        }else {
            next_level = GameManager.instance.currentLevel;
        }
        print(GameManager.instance.currentLevel);
		GameManager.instance.currentLevel = next_level;    
		GameManager.instance.StartLevel(next_level);
    }

    public void ToLevel(int level) {
        if (level <= GameManager.instance.levelsUnlocked) {
            GameManager.instance.currentLevel = level;
            GameManager.instance.StartLevel(level);
        }
    }

    public void GoToStore() {
        GameManager.instance.GoToStore();
    }

    public void GoToLevelOverview() {
        GameManager.instance.GoTolevelOverview();
    }

    public void GoToInfo() {
        GameManager.instance.GoToInfo();
    }

    public void GoToSettings() {
        GameManager.instance.GoToSettings();
    }

}
