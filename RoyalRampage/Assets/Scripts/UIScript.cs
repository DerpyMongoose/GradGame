using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIScript : MonoBehaviour {

    private float waitTimeMB = .13f;
    private float waitTimeSB = .3f;

    GameObject gui_Time;
    GameObject gui_Slider;
    GameObject gui_ScorePanel;
    GameObject gui_Panel;
    GameObject background;
    GameObject behindPanelButton;
    GameObject pause_menu;
    GameObject pauseButton;
    GameObject settings_menu;
    GameObject levels_menu;
    GameObject levels;
    GameObject levels_Panel;
    GameObject play_menu;
    GameObject instr_Menu;
    GameObject instr_Slides;
    GameObject back_Button;
    GameObject skip_Button;
    GameObject twoHandsSplitScreen;
    GameObject tapPanel;
    GameObject credits;
    Transform[] instr_SlidesChildren;
    [HideInInspector]
    public int slide = 5;

    GameObject help_menu;
    //    GameObject[] slides;
    //    GameObject arrowL;
    //    GameObject arrowR;

    Text starTotal;
    Text levelNum;
    Text highScoreMenu;

    List<LevelAndObjects> highScoreList;

    int current_slide = 0;

    void OnApplicationQuit() {
        GameManager.instance.Save();
    }

    void Start() {
        //set up the scene when opened
        switch (GameManager.instance.CurrentScene()) {
            /*case GameManager.Scene.INTRO:

            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO

            starTotal = GameObject.Find("starTotal").GetComponent<Text>();
            starTotal.text = "Stars:" + GameManager.instance.allStars.ToString();
            GameObject replayPanel = GameObject.Find("replayPanel");
            replayPanel.SetActive(false);
            UpdateMenuBG();
            break;*/
            case GameManager.Scene.SPLASH:
            GameManager.instance.Load();
            GameManager.instance.currentLevel = GameManager.instance.levelsUnlocked;
            break;

            case GameManager.Scene.ANIMATIC:
            GameManager.instance.changeMusicState(AudioManager.IN_INTRO_CUTSCENE);
            break;

            case GameManager.Scene.PLAY_MENU:
            GameObject.Find("music_slider").GetComponent<Slider>().value = GameManager.instance.music_volume;
            GameObject.Find("sfx_slider").GetComponent<Slider>().value = GameManager.instance.sfx_volume;
            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO

            //update level on play icon
            highScoreList = new List<LevelAndObjects>();
            highScoreList = SaveHighScore.instance.ReturnListWithObjects((GameManager.instance.levelsUnlocked - 1).ToString());
            levelNum = GameObject.FindGameObjectWithTag("level_number").GetComponentInChildren<Text>();
            highScoreMenu = GameObject.FindGameObjectWithTag("MenuHighScore").GetComponent<Text>();
            levelNum.text = (GameManager.instance.levelsUnlocked - 1).ToString();
            highScoreMenu.text = "HighScore: " + highScoreList[0].HighScore.ToString();
            credits = GameObject.FindGameObjectWithTag("Credits");
            GameManager.instance.currentLevel = GameManager.instance.levelsUnlocked;

            background = GameObject.FindGameObjectWithTag("menuBG");

            settings_menu = GameObject.FindGameObjectWithTag("SettingPanel");
            settings_menu.SetActive(false);
            levels_menu = GameObject.FindGameObjectWithTag("LevelPanel");
            levels = GameObject.FindGameObjectWithTag("Levels");
            levels_menu.SetActive(false);
            play_menu = GameObject.FindGameObjectWithTag("PlayPanel");
            help_menu = GameObject.FindGameObjectWithTag("HelpPanel");
            help_menu.SetActive(false);
            credits.SetActive(false);

            //UpdateMenuBG();
            break;

            case GameManager.Scene.LEVELS_OVERVIEW:
            GameObject.Find("music_slider").GetComponent<Slider>().value = GameManager.instance.music_volume;
            GameObject.Find("sfx_slider").GetComponent<Slider>().value = GameManager.instance.sfx_volume;
            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO

            //update level on play icon
            levelNum = GameObject.FindGameObjectWithTag("level_number").GetComponentInChildren<Text>();
            levelNum.text = (GameManager.instance.levelsUnlocked).ToString();

            settings_menu = GameObject.FindGameObjectWithTag("SettingPanel");
            settings_menu.SetActive(false);
            levels_menu = GameObject.FindGameObjectWithTag("LevelPanel");
            levels = GameObject.FindGameObjectWithTag("Levels");
            play_menu = GameObject.FindGameObjectWithTag("PlayPanel");
            play_menu.SetActive(false);
            help_menu = GameObject.FindGameObjectWithTag("HelpPanel");
            help_menu.SetActive(false);
            credits = GameObject.FindGameObjectWithTag("Credits");
            credits.SetActive(false);
            UpdateLevelOverview();
            break;

            case GameManager.Scene.STORE:
            GameManager.instance.changeMusicState(AudioManager.IN_MAIN_MENU);  // FOR AUDIO
            break;

            case GameManager.Scene.LOADING:
            GameManager.instance.Loading();
            GameManager.instance.changeMusicState(AudioManager.IN_LOADINGSCREEN);
            break;

            case GameManager.Scene.GAME:
            GameObject.Find("music_slider").GetComponent<Slider>().value = GameManager.instance.music_volume;
            GameObject.Find("sfx_slider").GetComponent<Slider>().value = GameManager.instance.sfx_volume;
            GameManager.instance.changeMusicState(AudioManager.IN_LEVEL);  // FOR AUDIO

            help_menu = GameObject.FindGameObjectWithTag("HelpPanel");
            pause_menu = GameObject.FindGameObjectWithTag("PausePanel");
            pauseButton = gameObject.GetComponent<MenuPublics>().pauseButtonPublic;
            settings_menu = GameObject.FindGameObjectWithTag("SettingPanel");
            behindPanelButton = GameObject.FindGameObjectWithTag("BehindPanelButton");
            pause_menu.SetActive(false);
            settings_menu.SetActive(false);
            help_menu.SetActive(false);
            behindPanelButton.SetActive(false);

            break;
            case GameManager.Scene.TUTORIAL:
            help_menu = GameObject.FindGameObjectWithTag("HelpPanel");
            pause_menu = GameObject.FindGameObjectWithTag("PausePanel");

            pauseButton = gameObject.GetComponent<MenuPublics>().pauseButtonPublic;
            settings_menu = GameObject.FindGameObjectWithTag("SettingPanel");
            behindPanelButton = GameObject.FindGameObjectWithTag("BehindPanelButton");
            twoHandsSplitScreen = GameObject.Find("TwoHandsSplitScreen");
            tapPanel = GameObject.Find("IntroTapPanel");

            pause_menu.SetActive(false);
            settings_menu.SetActive(false);
            help_menu.SetActive(false);
            behindPanelButton.SetActive(false);

            break;
        }
    }


    public void BehindPanel() {
        if (Input.touchCount == 1) {
            if (pause_menu.activeInHierarchy && !settings_menu.activeInHierarchy && !help_menu.activeInHierarchy) {
                UnPauseGame();
            }
            if (settings_menu.activeInHierarchy) {
                settings_menu.SetActive(false);
            }
            if (help_menu.activeInHierarchy) {
                help_menu.SetActive(false);
            }
        }
    }

    public void CloseLevelSelect() {
        if (levels_menu.activeInHierarchy) {
            PlayMenuButtonSound();
            levels_menu.SetActive(false);
            play_menu.SetActive(true);
        }
        if (credits.activeInHierarchy) {
            PlayMenuButtonSound();
            credits.SetActive(false);
        }
        if (settings_menu.activeInHierarchy) {
            PlayMenuButtonSound();
            settings_menu.SetActive(false);
        }
        if (help_menu.activeInHierarchy) {
            PlayMenuButtonSound();
            help_menu.SetActive(false);
        }
    }

    public void InstructionsSkip() {
        PlayMenuButtonSound();
        help_menu.SetActive(false);
        //GameManager.instance.isInstructed = true;
    }

    public void CloseCredits() {
        PlayMenuButtonSound();
        credits.SetActive(false);
    }

    public void OpenCredits() {
        if (Input.touchCount == 1) {
            PlayMenuButtonSound();
            credits.SetActive(true);
        }
    }

    public void BackToGame() {
        GameManager.instance.finishedCountingPoints();
        PlayStartButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeSB, "BackToGame"));
    }

    public void BackToPreviousScreen() {

        // *** FOR AUDIO
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "BackToPreviousScreen"));

    }

    public void ToNextLevel() {

        //***** FOR AUDIO
        GameManager.instance.finishedCountingPoints();
        PlayStartButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeSB, "ToNextLevel"));

    }

    public void ToLevel(int level) {

        //***** FOR AUDIO
        if (level <= GameManager.instance.levelsUnlocked) {
            PlayStartButtonSound();
            StartCoroutine(WaitButtonFinish(waitTimeSB, "ToLevel", level));
        } else {
            PlayMenuButtonSound();
            StartCoroutine(WaitButtonFinish(waitTimeMB, "ToLevel", level));
        }

    }

    public void GoToStore() {
        //***** FOR AUDIO
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToStore"));
    }

    public void GoToLevelOverview() {

        //***** FOR AUDIO
        if (Input.touchCount == 1) {
            PlayMenuButtonSound();
            StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToLevelOverview"));
        }
    }

    public void CloseLevelOverview() {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "CloseLevelOverview"));
    }

    public void GoToInfo() {
        if (Input.touchCount == 1) {
            //***** FOR AUDIO
            PlayMenuButtonSound();
            help_menu.SetActive(true);
            //UpdateHelpSlides("open");
        }
    }

    public void CloseInfo() {
        PlayMenuButtonSound();
        help_menu.SetActive(false);

    }


    public void GoToSettings() {
        if (Input.touchCount == 1) {
            //***** FOR AUDIO
            PlayMenuButtonSound();
            //StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToSettings"));
            settings_menu.SetActive(true);
        }

    }

    public void CloseSettings() {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        //StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToSettings"));
        settings_menu.SetActive(false);

    }

    public void RemovePauseButton() {
        pauseButton.SetActive(false);
    }

    public void EnablePauseButton() {
        StartCoroutine(PauseButtonDelay());
    }

    public void PauseGame() {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        GameManager.instance.menuRolledOut();
        //StartCoroutine(WaitButtonFinish(waitTimeMB, "PauseGame"));
        GameManager.instance.changeMusicState(AudioManager.IN_GAME_MENU);
        pauseButton.SetActive(false);// FOR AUDIO
        GameManager.instance.isPaused = true;
        pause_menu.SetActive(true);
        behindPanelButton.SetActive(true);
        GameManager.instance.PauseGame();

    }
    public void UnPauseGame() {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "UnPauseGame"));
        GameManager.instance.isPaused = false;
        GameManager.instance.PauseGame();
        behindPanelButton.SetActive(false);
        StartCoroutine(PauseButtonDelay());
        GameManager.instance.menuRolledIn();
        GameManager.instance.changeMusicState(AudioManager.IN_LEVEL);  // FOR AUDIO       
    }

    IEnumerator PauseButtonDelay() {
        yield return new WaitForSeconds(0.5f);
        pauseButton.SetActive(true);
    }

    public void RestartGame() {

        //***** FOR AUDIO
        PlayMenuButtonSound();
        GameManager.instance.isPaused = false;
        GameManager.instance.BackToGame();
    }

    public void GoToMainMenu() {
        GameManager.instance.finishedCountingPoints();
        PlayMenuButtonSound();
        StartCoroutine(WaitButtonFinish(waitTimeMB, "GoToMainMenu"));
        Time.timeScale = 1;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void UpdateMusicVolume(Slider slider) {
        GameManager.instance.changeMusicVolume(slider.value);
    }

    public void UpdateSFXVolume(Slider slider) {
        GameManager.instance.changeSFXVolume(slider.value);
    }

    private IEnumerator WaitButtonFinish(float waitTime, string btnAction, int level = default(int)) {
        yield return new WaitForSeconds(waitTime);
        switch (btnAction) {
            case "BackToGame":
            GameManager.instance.BackToGame();
            break;

            case "BackToPreviousScreen":
            GameManager.instance.BackToPreviousScene();
            break;

            case "ToNextLevel":
            int next_level;
            if (GameManager.instance.currentLevel < GameManager.instance.NUM_OF_LEVELS_IN_GAME) {
                next_level = GameManager.instance.currentLevel + 1;
            } else {
                next_level = GameManager.instance.currentLevel;
            }
            GameManager.instance.currentLevel = next_level;
            GameManager.instance.StartLevel(next_level);
            break;

            case "ToLevel":
            if (level <= GameManager.instance.levelsUnlocked) {
                GameManager.instance.currentLevel = level;
                GameManager.instance.StartLevel(level);
            }
            break;

            case "GoToStore":
            GameManager.instance.GoToStore();
            break;

            case "GoToLevelOverview":
            play_menu.SetActive(false);
            levels_menu.SetActive(true);
            UpdateLevelOverview();
            GameManager.instance.GoToLevelOverview();
            GameManager.instance.letterOpen();
            break;

            case "CloseLevelOverview":
            levels_menu.SetActive(false);
            play_menu.SetActive(true);
            GameManager.instance.CloseLevelOverview();
            GameManager.instance.letterClose();
            break;

            case "UnPauseGame":
            pause_menu.SetActive(false);
            break;

            case "GoToMainMenu":
            GameManager.instance.GoToMainMenu();
            break;
        }
    }



    public void PlayMenuButtonSound() {
        GameManager.instance.menuButtonClicked();
    }

    public void PlayStartButtonSound() {
        GameManager.instance.startButtonClicked();
    }

    /* private void UpdateMenuBG()
     {
         if (GameManager.instance.menu_bg_sprite != null)
         {
             Image bg = GameObject.FindGameObjectWithTag("menuBG").GetComponent<Image>();
             bg.sprite = GameManager.instance.menu_bg_sprite;
         }
     }*/

    private void UpdateLevelOverview() {
        //set the correct sprite on level icon
        Sprite[] unlockedSprite = GetComponent<MenuPublics>().unlockedSprite;
        Sprite lockedSprite = GetComponent<MenuPublics>().lockedSprite;
        for (int i = 0; i < 6; i++) {
            Image levelIcon = levels.transform.GetChild(i).GetComponent<Image>();
            if (i < GameManager.instance.levelsUnlocked)
                levelIcon.sprite = unlockedSprite[i];
            else
                levelIcon.sprite = lockedSprite;
        }
    }

    public void LoadGame() {
        GameManager.instance.LoadGame();
    }

    public void Continue() {
        GameManager.instance.timerOut();
        if (GameManager.instance.currentLevel == 1) {
            GameManager.instance.currentScene = GameManager.Scene.GAME;
        }
    }
}
