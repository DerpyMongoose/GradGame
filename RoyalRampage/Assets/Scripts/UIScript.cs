using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    private float waitTimeMB = .13f;
    private float waitTimeSB = .3f;

    Text starTotal;

    void Start()
    {
        GameManager.instance.Load();

        //set up the scene when opened
        switch (GameManager.instance.CurrentScene())
        {
            case GameManager.Scene.INTRO:

            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO

            starTotal = GameObject.Find("starTotal").GetComponent<Text>();
            starTotal.text = "Stars:" + GameManager.instance.allStars.ToString();
            GameObject replayPanel = GameObject.Find("replayPanel");
            replayPanel.SetActive(false);
            UpdateMenuBG();
            break;

            case GameManager.Scene.GAME_OVER_NEXT_LEVEL:
            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO

            GameObject playBtn = GameObject.Find("PlayButton");
            playBtn.SetActive(false);
            GameObject ReplayBTN = GameObject.Find("replayPanel/ReplayButton");
            ReplayBTN.SetActive(false);
            Text scoreText = GameObject.Find("replayPanel/score").GetComponent<Text>();
            scoreText.text = "Score: " + "$" + GameManager.instance.score;
            starTotal = GameObject.Find("starTotal").GetComponent<Text>();
            starTotal.text = "Stars:" + GameManager.instance.allStars.ToString();

            Text levelNum = GameObject.Find("replayPanel/NewLevelButton/levelnumber").GetComponentInChildren<Text>();
            if (GameManager.instance.levelsUnlocked < GameManager.instance.NUM_OF_LEVELS_IN_GAME)
            {
                GameManager.instance.levelsUnlocked++;
            }
            if (GameManager.instance.currentLevel < GameManager.instance.NUM_OF_LEVELS_IN_GAME)
            {
                levelNum.text = (GameManager.instance.currentLevel + 1).ToString();
            }
            else
            {
                levelNum.text = GameManager.instance.currentLevel.ToString() + "*";
            }

            UpdateMenuBG();
            break;

            case GameManager.Scene.GAME_OVER_REPLAY:

            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO

            playBtn = GameObject.Find("PlayButton");
            playBtn.SetActive(false);
            GameObject NextLevelBTN = GameObject.Find("replayPanel/NewLevelButton");
            NextLevelBTN.SetActive(false);
            scoreText = GameObject.Find("replayPanel/score").GetComponent<Text>();
            scoreText.text = "Score: " + "$" + GameManager.instance.score;
            starTotal = GameObject.Find("starTotal").GetComponent<Text>();
            starTotal.text = "Stars:" + GameManager.instance.allStars.ToString();
            break;

            case GameManager.Scene.LEVELS_OVERVIEW:

            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO

            //set the correct sprite on level icon
            Sprite unlockedSprite = GetComponent<MenuPublics>().unlockedSprite;
            Sprite lockedSprite = GetComponent<MenuPublics>().lockedSprite;
            for (int i = 1; i <= 6; i++)
            {
                Image levelIcon = GameObject.Find("LevelInGame/Level" + i).GetComponent<Image>();
                if (i <= GameManager.instance.levelsUnlocked)
                    levelIcon.sprite = unlockedSprite;
                else
                    levelIcon.sprite = lockedSprite;
            }

            break;

            case GameManager.Scene.STORE:
            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO
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

    public void GoToInfo()
    {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToInfo"));

    }

    public void GoToSettings()
    {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToSettings"));

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
            GameManager.instance.GoTolevelOverview();
            break;

            case "GoToInfo":
            GameManager.instance.GoToInfo();
            break;

            case "GoToSettings":
            GameManager.instance.GoToSettings();
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
            Image bg = GameObject.FindGameObjectWithTag("levelsPanel").GetComponent<Image>();
            bg.sprite = GameManager.instance.menu_bg_sprite;
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
