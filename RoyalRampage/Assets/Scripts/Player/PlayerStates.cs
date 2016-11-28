﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 Player manager with states and level timer, input should connect here
 */
public class PlayerStates : MonoBehaviour
{

    [HideInInspector]
    public static bool imInSlowMotion, hitObject, swiped;
    [Header("Forces")]
    public float moveForce;
    public float torgueForce;
    public float hitForce;
    public float swirlForce;
    public float liftForce;
    public float maxVelocity;
    [Header("Times")]
    public float timeForSwipe;
    public float timeForCircle;
    public float SameTapTime;
    public float gravityTimer;
    [Header("Radius")]
    //public float dashRadius;
    public float swirlRadius;
    public float liftRadius;
    [Header("Mixed")]
    public float distSwipe;
    public float rotationSpeed;
    public float degreesInAir;
    public float smoothPick;
    public int numOfCircleToShow;
    [Header("Cubic Bezier")]
    [Tooltip("The four points indicate the percentage of the force that you need to apply within a period of 1 second. For the record, the force starts really high and becomes lower")]
    public float p0;
    [Tooltip("The four points indicate the percentage of the force that you need to apply within a period of 1 second. For the record, the force starts really high and becomes lower")]
    public float p1;
    [Tooltip("The four points indicate the percentage of the force that you need to apply within a period of 1 second. For the record, the force starts really high and becomes lower")]
    public float p2;
    [Tooltip("The four points indicate the percentage of the force that you need to apply within a period of 1 second. For the record, the force starts really high and becomes lower")]
    public float p3;

    private float timeTicker = 5;  // TIME TO START THE TICKING SOUND
    private float timeRunningOut = 10;  // TIME TO START THE RUNNING OUT SOUND



    private enum PlayerState
    {
        READY, IDLE, WALKING, ATTACKING, ENDING
    }

    PlayerState state;
    float timeLeftInLevel = 0f; //timeleft to complete the level
    Text timerText;
    GameObject timerUI;


    void Start()
    {
        hitObject = false;
        state = PlayerState.READY;
    }

    void Awake()
    {
        //DontDestroyOnLoad (gameObject);
        //update timer
        timerText = GameObject.Find("TimeLeftText").GetComponent<Text>();
        timerUI = GameObject.Find("TimeLeftText");
        timeLeftInLevel = GameManager.instance.levelManager.timeToCompleteLevel;
        timerText.text = timeLeftInLevel.ToString("F1"); // for the level timer
        GameManager.instance.canPlayerMove = true;
        GameManager.instance.canPlayerDestroy = true;
        GameManager.instance.changeMusicState(AudioManager.IN_LEVEL);  // FOR AUDIO
    }

    void Update()
    {
        //print(state);
        //update level timer
        UpdateLevelTimer();

        switch (state)
        {

            //until player touches the screen to start playing the level
            case PlayerState.READY:
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    GameManager.instance.levelLoad();
                    print("LoadLevelStart");
                }
                if (SwipeHalf.startTutTimer == true)
                {
                    Startlevel();
                }
                if (Input.GetKey(KeyCode.R))
                {
                    GameManager.instance.levelLoad();
                }
                break;

            case PlayerState.IDLE:
                break;

            case PlayerState.WALKING:
                break;

            case PlayerState.ATTACKING:
                break;

            //after timer is out - losing/winning ending
            case PlayerState.ENDING:
                break;
        }
    }


    private void UpdateLevelTimer()
    {
        switch (state)
        {
            case PlayerState.IDLE:
            case PlayerState.WALKING:
            case PlayerState.ATTACKING:
                if (GameManager.instance.TutorialState() == GameManager.Tutorial.MOVEMENT || GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
                {
                    if (GameManager.instance.levelManager.multiplier > 1)
                    {
                        timeLeftInLevel -= 0;
                    }
                    else if (!imInSlowMotion)
                    {
                        timeLeftInLevel -= Time.deltaTime;
                    }
                    else
                    {
                        timeLeftInLevel -= 0.005f;
                    }
                }
                timerText.text = timeLeftInLevel.ToString("F1"); // for the level timer
                if (timeLeftInLevel <= timeTicker)
                {
                    timeTicker -= 1;
                    GameManager.instance.timerUpdate(timeTicker);
                }

                if (timeLeftInLevel <= timeRunningOut)
                {
                    timeRunningOut = -1;
                    GameManager.instance.changeMusicState(AudioManager.IN_LEVEL_TIME_RUNNING_OUT);  // FOR AUDIO
                }

                //when timer runs out:
                if (timeLeftInLevel <= 0f)
                { //Move stuff to events
                    if (GameManager.instance.TutorialState() == GameManager.Tutorial.MOVEMENT && GameManager.instance.CurrentScene() == GameManager.Scene.TUTORIAL && GameManager.instance.levelManager.targetReached == false)
                    {
                        GameManager.instance.player.transform.position = GameManager.instance.levelManager.playerPos;
                        timerText.text = "0";  // for the level timer
                        timeLeftInLevel = GameManager.instance.levelManager.timeToCompleteLevel;
                        SwipeHalf.startTutTimer = false;
                    }
                    else if (GameManager.instance.TutorialState() == GameManager.Tutorial.MOVEMENT && GameManager.instance.CurrentScene() == GameManager.Scene.TUTORIAL && GameManager.instance.levelManager.targetReached == true)
                    {
                        GameManager.instance.player.transform.position = GameManager.instance.levelManager.playerPos;
                        GameManager.instance.player.GetComponent<Rigidbody>().Sleep();
                        timeLeftInLevel = 0;
                        timerUI.SetActive(false);
                        GameObject.Find("Target").SetActive(false);
                        GameManager.instance.tutorial = GameManager.Tutorial.ATTACK;
                    }
                    else if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
                    {
                        timerText.text = "0";  // for the level timer
                        timerText.color = Color.red;
                        GameManager.instance.timerOut();
                    }
                }
                break;
        }
    }

    private void Startlevel()
    {
        timeLeftInLevel = GameManager.instance.levelManager.timeToCompleteLevel;
        state = PlayerState.IDLE;
        GameManager.instance.timerStart();
    }

    private void EndLevel()
    {
        state = PlayerState.ENDING;
        GameManager.instance.canPlayerDestroy = false;
        GameManager.instance.changeMusicState(AudioManager.IN_LEVEL_TIMES_UP);  // FOR AUDIO
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.transform.tag == "Destructable")
        {
            if (GameManager.instance.TutorialState() == GameManager.Tutorial.MOVEMENT)
            {
                SwipeHalf.startTutTimer = true;
            }
            else hitObject = true;
        }
    }

    void OnEnable()
    {
        GameManager.instance.OnTimerOut += EndLevel;
    }

    void OnDisable()
    {
        GameManager.instance.OnTimerOut -= EndLevel;
    }
}
