using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager {

	private static GameManager _instance;
	private GameObject _player;
	private LevelManager _levelManager;
	private static string[] GAME_SCENES = {"GameScene1","GameScene2","GameScene1"};

	public int currentLevel = 1;
    public int levelsUnlocked = 1;
    public int NUM_OF_LEVELS_IN_GAME = GAME_SCENES.Length;

    public int score = 0;

	//getters:
	public static GameManager instance{
		get {
			if (_instance == null)
				_instance = new GameManager ();
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

	//scene management
	public void StartLevel(int level){
		//_instance = null;
		SceneManager.LoadScene (GAME_SCENES[level - 1]); //UPDATE FOR MORE LEVELS
		Time.timeScale = 1;
	}

	public void GoToStore(){
		//_instance = null;
		SceneManager.LoadScene ("Shop");
		Time.timeScale = 1;
	}

    public void GoTolevelOverview() {
        //_instance = null;
        SceneManager.LoadScene("GameLevelsGUI");
        Time.timeScale = 1;
    }

    public void GoToHelp() {
        //_instance = null;
        //SceneManager.LoadScene("Help");
        Time.timeScale = 1;
    }

    public void GoToSettings() {
        //_instance = null;
        //SceneManager.LoadScene("Settings");
        Time.timeScale = 1;
    }

    public void BackToGame(){
		//_instance = null;
		SceneManager.LoadScene (GAME_SCENES[currentLevel - 1]); //UPDATE FOR MORE LEVELS
		Time.timeScale = 1;
	}

	//delegates
	public delegate void PlayerAction(GameObject obj);
	public event PlayerAction OnObjectDestructed;
	public void objectDestructed(GameObject obj) {
		if (OnObjectDestructed != null)
			OnObjectDestructed (obj);
	}

	public delegate void GameAction();
	public event GameAction OnTimerStart;
	public event GameAction OnTimerOut;
    public static event GameAction stampPower;
    public void timerStart() {
		if (OnTimerStart != null)
			OnTimerStart ();
	}
	public void timerOut() {
		if (OnTimerOut != null)
			OnTimerOut ();
	}

    public void TimeToLift()
    {
        ////////we could use this: stampPower?.Invoke(); which is the same thing and simplier and 
        //The new way is thread-safe because the compiler generates code to evaluate PropertyChanged one time only, 
        //keeping the result in temporary variable. But it needs C# 6 or greater.
        if (stampPower != null)
        {
            stampPower();
        }
    }

}
