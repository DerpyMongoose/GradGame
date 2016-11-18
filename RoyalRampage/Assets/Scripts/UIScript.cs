using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    private float waitTimeMB = .13f;
    private float waitTimeSB = .3f;

	GameObject pause_menu;
	GameObject settings_menu;
	GameObject levels_menu;
	GameObject levels;
	GameObject play_menu;

    Text starTotal;

    void Start()
    {
       // GameManager.instance.Load();

        //set up the scene when opened
        switch (GameManager.instance.CurrentScene())
        {
            /*case GameManager.Scene.INTRO:

            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO

            starTotal = GameObject.Find("starTotal").GetComponent<Text>();
            starTotal.text = "Stars:" + GameManager.instance.allStars.ToString();
            GameObject replayPanel = GameObject.Find("replayPanel");
            replayPanel.SetActive(false);
            UpdateMenuBG();
            break;*/

		case GameManager.Scene.PLAY_MENU:
			GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO

			//update level on play icon
			Text levelNum = GameObject.FindGameObjectWithTag("level_number").GetComponentInChildren<Text>();
			levelNum.text = "Level " + (GameManager.instance.levelsUnlocked).ToString();

			settings_menu = GameObject.FindGameObjectWithTag ("SettingPanel");
			settings_menu.SetActive(false);
			levels_menu = GameObject.FindGameObjectWithTag ("LevelPanel");
			levels = GameObject.FindGameObjectWithTag ("Levels");
			levels_menu.SetActive(false);
			play_menu = GameObject.FindGameObjectWithTag ("PlayPanel");

			UpdateMenuBG();
			break;

		case GameManager.Scene.LEVELS_OVERVIEW:

			GameManager.instance.changeMusicState (AudioManager.IN_MAIN_MENU);  // FOR AUDIO

			//update level on play icon
			levelNum = GameObject.FindGameObjectWithTag("level_number").GetComponentInChildren<Text>();
			levelNum.text = "Level " + (GameManager.instance.levelsUnlocked).ToString();

			settings_menu = GameObject.FindGameObjectWithTag ("SettingPanel");
			settings_menu.SetActive (false);
			levels_menu = GameObject.FindGameObjectWithTag ("LevelPanel");
			levels = GameObject.FindGameObjectWithTag ("Levels");
			play_menu = GameObject.FindGameObjectWithTag ("PlayPanel");
			play_menu.SetActive (false);
			UpdateLevelOverview ();
            break;

        case GameManager.Scene.STORE:
            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO
            break;

		case GameManager.Scene.GAME:
			pause_menu = GameObject.FindGameObjectWithTag ("PausePanel");
			pause_menu.SetActive(false);
			settings_menu = GameObject.FindGameObjectWithTag ("SettingPanel");
			settings_menu.SetActive(false);
			break;

        }
			
    }

    public void BackToGame()
    {
        PlayStartButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeSB, "BackToGame"));
    }

    public void BackToPreviousScreen()
    {

        // *** FOR AUDIO
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "BackToPreviousScreen"));

    }

    public void ToNextLevel()
    {

        //***** FOR AUDIO
        PlayStartButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeSB, "ToNextLevel"));

    }

    public void ToLevel(int level)
    {

        //***** FOR AUDIO
        if (level <= GameManager.instance.levelsUnlocked)
        {
            PlayStartButtonSound();
            StartCoroutine(WaitButtonFinish(waitTimeSB, "ToLevel", level));
        }
        else
        {

            PlayMenuButtonSound();
            StartCoroutine(WaitButtonFinish(waitTimeMB, "ToLevel", level));
        }

    }

    public void GoToStore()
    {
        //***** FOR AUDIO
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToStore"));
    }

    public void GoToLevelOverview()
    {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToLevelOverview"));
    }

	public void CloseLevelOverview()
	{

		//***** FOR AUDIO
		PlayMenuButtonSound();
		StartCoroutine(WaitButtonFinish(waitTimeMB, "CloseLevelOverview"));
	}

    public void GoToInfo()
    {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        //StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToInfo"));

    }

    public void GoToSettings()
    {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        //StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToSettings"));
		settings_menu.SetActive (true);

    }

	public void CloseSettings()
	{

		//***** FOR AUDIO
		PlayMenuButtonSound();
		//StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToSettings"));
		settings_menu.SetActive (false);

	}

	public void PauseGame(){

		//***** FOR AUDIO
		PlayMenuButtonSound();
		//StartCoroutine(WaitButtonFinish(waitTimeMB, "PauseGame"));
		GameManager.instance.isPaused = true;
		pause_menu.SetActive (true);
		GameManager.instance.PauseGame();

	}
	public void UnPauseGame(){
		
		//***** FOR AUDIO
		PlayMenuButtonSound();
		StartCoroutine(WaitButtonFinish(waitTimeMB, "UnPauseGame"));
		GameManager.instance.isPaused = false;
		GameManager.instance.PauseGame();
		print ("unpausing");
	}

	public void RestartGame(){

		//***** FOR AUDIO
		PlayMenuButtonSound();
		GameManager.instance.isPaused = false;
		GameManager.instance.BackToGame();
	}

	public void GoToMainMenu(){
		PlayMenuButtonSound();
		StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToMainMenu"));
	}

    private IEnumerator WaitButtonFinish(float waitTime, string btnAction, int level = default(int))
    {
        yield return new WaitForSeconds(waitTime);
        switch (btnAction)
        {
            case "BackToGame":
            GameManager.instance.BackToGame();
            break;

            case "BackToPreviousScreen":
            GameManager.instance.BackToPreviousScene();
            break;

            case "ToNextLevel":
            int next_level;
            if (GameManager.instance.currentLevel < GameManager.instance.NUM_OF_LEVELS_IN_GAME)
            {
                next_level = GameManager.instance.currentLevel + 1;
            }
            else
            {
                next_level = GameManager.instance.currentLevel;
            }
            print(GameManager.instance.currentLevel);
            GameManager.instance.currentLevel = next_level;
            GameManager.instance.StartLevel(next_level);
            break;

            case "ToLevel":
            if (level <= GameManager.instance.levelsUnlocked)
            {
                GameManager.instance.currentLevel = level;
                GameManager.instance.StartLevel(level);
            }
            break;

            case "GoToStore":
            GameManager.instance.GoToStore();
            break;

		case "GoToLevelOverview":
			play_menu.SetActive (false);
			levels_menu.SetActive (true);
			UpdateLevelOverview ();
            GameManager.instance.GoToLevelOverview();
            break;

		case "CloseLevelOverview":
			levels_menu.SetActive (false);
			play_menu.SetActive (true);
			GameManager.instance.CloseLevelOverview();
			break;

		case "UnPauseGame":
			pause_menu.SetActive (false);
			break;

		case "GoToMainMenu":
			GameManager.instance.GoToMainMenu ();
			break;
        }
    }



    public void PlayMenuButtonSound()
    {
        GameManager.instance.menuButtonClicked();
    }

    public void PlayStartButtonSound()
    {
        GameManager.instance.startButtonClicked();
    }

    private void UpdateMenuBG()
    {
        if (GameManager.instance.menu_bg_sprite != null)
        {
            Image bg = GameObject.FindGameObjectWithTag("menuBG").GetComponent<Image>();
            bg.sprite = GameManager.instance.menu_bg_sprite;
        }
    }

	private void UpdateLevelOverview(){
		//set the correct sprite on level icon
		Sprite unlockedSprite = GetComponent<MenuPublics>().unlockedSprite;
		Sprite lockedSprite = GetComponent<MenuPublics>().lockedSprite;
		for (int i = 0; i < 6; i++)
		{
			Image levelIcon = levels.transform.GetChild(i).GetComponent<Image>();
			if (i < GameManager.instance.levelsUnlocked)
				levelIcon.sprite = unlockedSprite;
			else
				levelIcon.sprite = lockedSprite;
		}
	}

	public void LoadGame(){
		GameManager.instance.LoadGame ();
	}

	/*private IEnumerator SplashScreen(){
		GameObject dadiu = GameObject.FindGameObjectWithTag ("DadiuSplash");
		GameObject unity = GameObject.FindGameObjectWithTag ("UnitySplash");
		GameObject game = GameObject.FindGameObjectWithTag ("GameSplash");

		unity.SetActive (false);
		game.SetActive (false);

		yield return new WaitForSeconds (1.5f);
		unity.SetActive (true);
		dadiu.SetActive (false);

		yield return new WaitForSeconds (1.5f);
		game.SetActive (true);
		unity.SetActive (false);

		yield return new WaitForSeconds (2f);
		GameManager.instance.LoadGame ();
	}*/

}
