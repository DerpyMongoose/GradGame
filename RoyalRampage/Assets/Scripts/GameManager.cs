using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class GameManager
{

    private static GameManager _instance;
    private GameObject _player;
    private LevelManager _levelManager;
    private AudioManager _audioManager;

    private static string[] GAME_SCENES = { "FracturedLevel", "GameScene1", "GameScene2", "GameScene3" };
    private static string MAIN_MENU = "Menu";

    // The size of the array is the total amount of levels
    public int[] stars = new int[6];

    public int allStars = 0;
    public int currentLevel = 1;
    public int levelsUnlocked = 1;
    public int currency = 200;
    public int NUM_OF_LEVELS_IN_GAME = GAME_SCENES.Length;
    public enum Scene
    {
        SPLASH, LOADING, GAME, LEVELS_OVERVIEW, STORE, PLAY_MENU
    }
    private Scene currentScene = Scene.SPLASH;
    private Scene previousScene = Scene.PLAY_MENU;
    public bool levelWon;

    public Sprite menu_bg_sprite;

    public int score = 0;
    public bool canPlayerMove = false;
    public bool canPlayerDestroy = false;
    public bool isPaused = false;
    public bool isInstructed = false;

    //getters:
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    public GameObject player
    {
        get
        {
            if (_player == null)
                _player = GameObject.FindGameObjectWithTag("Player");
            return _player;
        }
    }

    public LevelManager levelManager
    {
        get
        {
            if (_levelManager == null)
                _levelManager = GameObject.FindObjectOfType<LevelManager>();
            return _levelManager;
        }
    }

    public AudioManager audioManager
    {
        get
        {
            if (_audioManager == null)
                _audioManager = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;
            return _audioManager;
        }

    }

    //scene management
    public Scene CurrentScene()
    {
        return currentScene;
    }

    private void SetPreviousScene()
    {
        if (currentScene == Scene.LEVELS_OVERVIEW)
            previousScene = Scene.LEVELS_OVERVIEW;
        else if (currentScene == Scene.PLAY_MENU)
            previousScene = Scene.PLAY_MENU;
    }

    //after splash screen
    public void LoadGame()
    {
        SceneManager.LoadScene("Animatic");
        Time.timeScale = 1;
    }

    public void Loading()
    {
        SceneManager.LoadSceneAsync(GAME_SCENES[currentLevel - 1]); //UPDATE FOR MORE LEVELS
        Time.timeScale = 1;
        currentScene = Scene.GAME;
    }

    public void Loading(int level)
    {
        SceneManager.LoadSceneAsync(GAME_SCENES[level - 1]); //UPDATE FOR MORE LEVELS
        Time.timeScale = 1;
        currentScene = Scene.GAME;
    }
    
    public void StartLevel(int level)
    {
        SceneManager.LoadScene("Loading"); //UPDATE FOR MORE LEVELS
        currentScene = Scene.LOADING;
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else if (!isPaused)
        {
            Time.timeScale = 1;
        }
    }

    public void GoToStore()
    {
        //_instance = null;
        SceneManager.LoadScene("Shop");
        Time.timeScale = 1;
        SetPreviousScene();
        currentScene = Scene.STORE;
    }

    public void GoToLevelOverview()
    {
        SetPreviousScene();
        currentScene = Scene.LEVELS_OVERVIEW;
    }

    public void CloseLevelOverview()
    {
        SetPreviousScene();
        currentScene = Scene.PLAY_MENU;
    }

    public void BackToGame()
    {
        SceneManager.LoadScene("Loading"); 
        Time.timeScale = 1;
        currentScene = Scene.LOADING;
    }

    public void BackToPreviousScene()
    {
        SceneManager.LoadScene(MAIN_MENU);
        Time.timeScale = 1;
        currentScene = previousScene;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU);
        Time.timeScale = 1;
        currentScene = Scene.PLAY_MENU;
    }

    //delegates
    public delegate void DestructionAction(GameObject obj);
    public event DestructionAction OnObjectDestructed;
    public event DestructionAction OnObjectHit;
    public event DestructionAction OnObjectLanding;

    public void objectDestructed(GameObject obj)
    {
        if (OnObjectDestructed != null)
            OnObjectDestructed(obj);
    }
    public void objectHit(GameObject obj)
    {
        if (OnObjectHit != null)
            OnObjectHit(obj);
    }
    public void objectLanding(GameObject obj)
    {
        if (OnObjectLanding != null)
            OnObjectLanding(obj);
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
    public event GameAction OnPlayerHit;

    public void timerStart()
    {
        if (OnTimerStart != null)
            OnTimerStart();
    }
    public void timerOut()
    {
        if (OnTimerOut != null)
            OnTimerOut();
    }
    public void playerDash()
    {
        if (OnPlayerDash != null)
            OnPlayerDash();
    }
    public void playerSwirl()
    {
        if (OnPlayerSwirl != null)
            OnPlayerSwirl();
    }
    public void playerStomp()
    {
        if (OnPlayerStomp != null)
            OnPlayerStomp();
    }
    public void levelLoad()
    {
        if (OnLevelLoad != null)
            OnLevelLoad();
    }
    public void levelUnLoad()
    {
        if (OnLevelUnLoad != null)
            OnLevelUnLoad();
    }

    public void menuButtonClicked()
    {
        if (OnMenuButtonClicked != null)
            OnMenuButtonClicked();
    }
    public void startButtonClicked()
    {
        if (OnStartButtonClicked != null)
            OnStartButtonClicked();
    }
    public void scoreScreenOpen()
    {
        if (OnScoreScreenOpen != null)
            OnScoreScreenOpen();
    }
    public void announcedObjective()
    {
        if (OnObjectiveAnnounced != null)
            OnObjectiveAnnounced();
    }
    public void completedObjective()
    {
        if (OnObjectiveCompleted != null)
            OnObjectiveCompleted();
    }
    public void startCountingPoints()
    {
        if (OnPointsCountingStart != null)
            OnPointsCountingStart();
    }
    public void finishedCountingPoints()
    {
        if (OnPointsCountingFinished != null)
            OnPointsCountingFinished();
    }
    public void playerHitObject()
    {
        if (OnPlayerHit != null)
            OnPlayerHit();
    }

    public delegate void LevelAction(float val);
    public event LevelAction OnTimerUpdate;
    public event LevelAction OnMusicStateChange;
    public event LevelAction OnMusicVolumeChange;
    public event LevelAction OnSFXVolumeChange;

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
    public void changeSFXVolume(float val)
    {
        if (OnSFXVolumeChange != null)
            OnSFXVolumeChange(val);
    }
    public void changeMusicVolume(float val)
    {
        if (OnMusicVolumeChange != null)
            OnMusicVolumeChange(val);
    }

    //SAVE-LOAD
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerProgress.dat"); /////////Not dynamic saves. it is only one save file. doesn't matter. we would need more if we would like to have different save files

        PlayerData data = new PlayerData();
        //data.currentLevel = currentLevel;
        data.levelsUnlocked = levelsUnlocked;
        data.allStars = allStars;
        data.stars = stars;
        data.currency = currency;
        data.isInstructed = isInstructed;


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
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //currentLevel = data.currentLevel;
            levelsUnlocked = data.levelsUnlocked;
            allStars = data.allStars;
            stars = data.stars;
            currency = data.currency;
            isInstructed = data.isInstructed;
        }

    }
}

[Serializable]
class PlayerData
{
    //public int currentLevel;
    public int levelsUnlocked;
    public int currency;
    public int allStars;
    public int[] stars;
    public bool isInstructed;
}
