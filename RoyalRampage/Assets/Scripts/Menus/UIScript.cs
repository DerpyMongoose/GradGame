using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    void Awake() {
        if (GameManager.instance.CurrentScene() == "GameLevelsGUI") {
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
        }
    }

	public void BackToGame(){
		GameManager.instance.BackToGame ();
	}

    public void ToNextLevel() {
        int next_level;
        if (GameManager.instance.currentLevel < GameManager.instance.NUM_OF_LEVELS_IN_GAME) {
            next_level = GameManager.instance.currentLevel + 1;
        }else {
            next_level = GameManager.instance.currentLevel;
        }
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
