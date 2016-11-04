﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager {

	private static GameManager _instance;
	private GameObject _player;
	private LevelManager _levelManager;
	private static string[] GAME_SCENES = {"GameScene1","GameScene2"};

	private int currentLevel = 1;

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
	public void StartNewLevel(){
		_instance = null;
		SceneManager.LoadScene (GAME_SCENES[0]); //UPDATE FOR MORE LEVELS
		Time.timeScale = 1;
	}

	public void GoToStore(){
		_instance = null;
		SceneManager.LoadScene ("Shop");
		Time.timeScale = 1;
	}

	public void BackToGame(){
		_instance = null;
		SceneManager.LoadScene (GAME_SCENES[currentLevel-1]); //UPDATE FOR MORE LEVELS
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
	public void timerStart() {
		if (OnTimerStart != null)
			OnTimerStart ();
	}
	public void timerOut() {
		if (OnTimerOut != null)
			OnTimerOut ();
	}
}
