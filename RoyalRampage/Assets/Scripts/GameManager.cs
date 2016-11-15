﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class GameManager {

	private static GameManager _instance;
	private GameObject _player;
	private LevelManager _levelManager;
	private AudioManager _audioManager;

	private static string[] GAME_SCENES = {"GameScene1","GameScene2","GameScene3"};

	public int currentLevel = 1;
    public int levelsUnlocked = 1;
    public int NUM_OF_LEVELS_IN_GAME = GAME_SCENES.Length;
    public enum Scene {
        INTRO, GAME, GAME_OVER_REPLAY, GAME_OVER_NEXT_LEVEL, INFO, SETTINGS, LEVELS_OVERVIEW, STORE
    }
    private Scene currentScene = Scene.INTRO;
    private Scene previousScene = Scene.INTRO;
    public bool levelWon;

	public Sprite menu_bg_sprite;

    public int score = 0;
    public bool canPlayerMove = false;
    public bool canPlayerDestroy = false;

	//getters:
	public static GameManager instance{
		get {
            if (_instance == null) {
                _instance = new GameManager();
            }
			return _instance;
		}
	}

	public GameObject player{
		get {
			if (_player == null)
				_player = GameObject.FindGameObjectWithTag("Player");
			return _player;
		}
	}

	public LevelManager levelManager{
		get {
			if (_levelManager == null)
				_levelManager = GameObject.FindObjectOfType<LevelManager> ();
			return _levelManager;
		}
	}

	public AudioManager audioManager{
		get {
			if (_audioManager == null)
				_audioManager = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;
				return _audioManager;
		}

	}

    //scene management
    public Scene CurrentScene() {
        return currentScene;        
    }

    private void SetPreviousScene() {
        if (currentScene == Scene.INTRO)
            previousScene = Scene.INTRO;
        if(currentScene == Scene.GAME) {
			//levelUnLoad(); // FOR AUDIO
            if (levelWon == true)
                previousScene = Scene.GAME_OVER_NEXT_LEVEL;
            else
                previousScene = Scene.GAME_OVER_REPLAY;
        }
    }

    public void StartLevel(int level){
		//_instance = null;
		//levelUnLoad();
		SceneManager.LoadScene (GAME_SCENES[level - 1]);
		Time.timeScale = 1;
        currentScene = Scene.GAME;
		//levelLoad (); // FOR AUDIO
	}

	public void GoToStore(){
		//_instance = null;
		SceneManager.LoadScene ("Shop");
		Time.timeScale = 1;
        SetPreviousScene();
        currentScene = Scene.STORE;
    }

    public void GoTolevelOverview() {
        //_instance = null;
        SceneManager.LoadScene("GameLevelsGUI");
        Time.timeScale = 1;
        SetPreviousScene();
        currentScene = Scene.LEVELS_OVERVIEW;
    }

    public void GoToInfo() {
        //_instance = null;
        //SceneManager.LoadScene("Help");
        Time.timeScale = 1;
        SetPreviousScene();
        currentScene = Scene.INFO;
    }

    public void GoToSettings() {
        //_instance = null;
        //SceneManager.LoadScene("Settings");
        Time.timeScale = 1;
        SetPreviousScene();
        currentScene = Scene.SETTINGS;
    }

    public void BackToGame(){
		//_instance = null;
		//levelUnLoad();
		SceneManager.LoadScene (GAME_SCENES[currentLevel - 1]); //UPDATE FOR MORE LEVELS
		Time.timeScale = 1;
        currentScene = Scene.GAME;
		//levelLoad ();	// FOR AUDIO
	}

    public void BackToPreviousScene() {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        currentScene = previousScene;
    }

	//delegates
	public delegate void DestructionAction(GameObject obj);
	public event DestructionAction OnObjectDestructed;
	public event DestructionAction OnObjectHit;
	public event DestructionAction OnObjectLanding;

	public void objectDestructed(GameObject obj) {
		if (OnObjectDestructed != null)
			OnObjectDestructed (obj);
	}
	public void objectHit(GameObject obj){
		if (OnObjectHit != null)
			OnObjectHit (obj);
	}
	public void objectLanding(GameObject obj){
		if (OnObjectLanding != null)
			OnObjectLanding (obj);
	}

	public delegate void GameAction();
	public event GameAction OnTimerStart;
	public event GameAction OnTimerOut;
	public event GameAction OnPlayerDash;
	public event GameAction OnPlayerSwirl;
    public event GameAction OnPlayerStomp;
	public event GameAction OnLevelLoad;
	public event GameAction OnLevelUnLoad;
    public event GameAction OnMenuButtonClicked;
    public event GameAction OnStartButtonClicked;
    public event GameAction OnScoreScreenOpen;
	public event GameAction OnObjectiveAnnounced;
	public event GameAction OnObjectiveCompleted;
	public event GameAction OnPointsCountingStart;
	public event GameAction OnPointsCountingFinished;

    public void timerStart() {
		if (OnTimerStart != null)
			OnTimerStart ();
	}
	public void timerOut() {
		if (OnTimerOut != null)
			OnTimerOut ();
	}
	public void playerDash(){
		if (OnPlayerDash != null)
			OnPlayerDash ();
	}
	public void playerSwirl(){
		if (OnPlayerSwirl != null)
			OnPlayerSwirl ();
	}
	public void playerStomp(){
		if (OnPlayerStomp != null)
			OnPlayerStomp ();
    }
	public void levelLoad(){
		if (OnLevelLoad != null)
			OnLevelLoad ();
	}
	public void levelUnLoad(){
		if (OnLevelUnLoad != null)
			OnLevelUnLoad ();
	}

    public void menuButtonClicked(){
        if (OnMenuButtonClicked != null)
            OnMenuButtonClicked();
    }
    public void startButtonClicked(){
        if (OnStartButtonClicked != null)
            OnStartButtonClicked();
    }
    public void scoreScreenOpen(){
        if (OnScoreScreenOpen != null)
            OnScoreScreenOpen();
    }
	public void announcedObjective(){
		if (OnObjectiveAnnounced != null)
			OnObjectiveAnnounced();
	}
	public void completedObjective(){
		if (OnObjectiveCompleted != null)
			OnObjectiveCompleted();
	}
	public void startCountingPoints(){
		if (OnPointsCountingStart != null)
			OnPointsCountingStart();
	}
	public void finishedCountingPoints(){
		if (OnPointsCountingFinished != null)
			OnPointsCountingFinished();
	}

    public delegate void LevelAction(float val);
    public event LevelAction OnTimerUpdate;
    public event LevelAction OnMusicStateChange;

    public void timerUpdate(float val)
    {
        if (OnTimerUpdate != null)
            OnTimerUpdate(val);
    }
    public void changeMusicState(float val)
    {
        if (OnMusicStateChange != null)
            OnMusicStateChange(val);
    }

    //SAVE-LOAD
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerProgress.dat"); /////////Not dynamic saves. it is only one save file. doesn't matter. we would need more if we would like to have different save files

        PlayerData data = new PlayerData();
        //data.currentLevel = currentLevel;
        data.levelsUnlocked = levelsUnlocked;

        bf.Serialize(file, data);
        file.Close();
    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/playerProgress.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/playerProgress.dat", FileMode.Open);
            PlayerData data = (PlayerData) bf.Deserialize(file);
            file.Close();

            //currentLevel = data.currentLevel;
            levelsUnlocked = data.levelsUnlocked;
        }

    }
}

[Serializable]
class PlayerData
{
    //public int currentLevel;
    public int levelsUnlocked;
}
