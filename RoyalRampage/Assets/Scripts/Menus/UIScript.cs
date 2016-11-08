using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {

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
        GameManager.instance.currentLevel = level;
        GameManager.instance.StartLevel(level);
    }

    public void GoToStore() {
        GameManager.instance.GoToStore();
    }

    public void GoToLevelOverview() {
        GameManager.instance.GoTolevelOverview();
    }

    public void GoToHelp() {
        GameManager.instance.GoToHelp();
    }

    public void GoToSettings() {
        GameManager.instance.GoToSettings();
    }
}
