using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 Player manager with states and level timer, input should connect here
 */
public class PlayerStates : MonoBehaviour
{

    [HideInInspector]
    public static bool imInSlowMotion, swiped;
    [Header("Forces")]
    public float moveForce;
    public float torgueForce;
    public float hitForce;
    public float swirlForce;
    public float liftForce;
    [Header("Times")]
    public float timeForSwipe;
    public float timeForCircle;
    public float SameTapTime;
    public float gravityTimer;
    public float resetMassTimer;
    [Header("Radius")]
    public float swirlRadius;
    public float liftRadius;
    [Header("Mixed")]
    public float becomeHeavy;
    public float distSwipe;
    public float maxDistSwipe;
    public float rotationSpeed;
    public float degreesInAir;
    public float rageObjects;
    public float smoothPick;
    public int numOfCircleToShow;

    private float timeTicker = 5;  // TIME TO START THE TICKING SOUND
    private float timeRunningOut = 10;  // TIME TO START THE RUNNING OUT SOUND
    private bool timerStart, timerStart2;
    private float timer;
    Color sliderCol;

    public enum PlayerState
    {
        READY, IDLE, WALKING, ATTACKING, ENDING
    }

    [HideInInspector]
    public PlayerState state;
    float timeLeftInLevel = 0f; //timeleft to complete the level
    Text timerText;
    GameObject timerUI;

    Slider timeSliderLeft;
    Slider timeSliderRight;
    float totalTime;

    void Start()
    {
        state = PlayerState.READY;
        SwipeHalf.startTutTimer = false;
        timerStart = false;
        timerStart2 = false;
        timer = 0;
        timeTicker = 5;
    }

    void Awake()
    {
        //DontDestroyOnLoad (gameObject);
        //update timer
        timerText = GameObject.FindGameObjectWithTag("TimeLeftText").GetComponent<Text>();
        timerUI = GameObject.Find("TimeLeftText");
        timeLeftInLevel = GameManager.instance.levelManager.timeToCompleteLevel;
        totalTime = timeLeftInLevel;
        timerText.text = timeLeftInLevel.ToString("F1"); // for the level timer
        timeSliderLeft = GameObject.Find("TimerSliderLeft").GetComponent<Slider>();
        timeSliderLeft.value = 1f;
        timeSliderRight = GameObject.Find("TimerSliderRight").GetComponent<Slider>();
        sliderCol = new Color(164f/255f, 97f/255f, 164f/255f);
        timeSliderRight.value = 1f;
        
        GameManager.instance.canPlayerMove = true;
        GameManager.instance.canPlayerDestroy = true;
    }

    void Update()
    {
        //update level timer
        UpdateLevelTimer();

        switch (state)
        {

            //until player touches the screen to start playing the level
            case PlayerState.READY:
                {
                    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        GameManager.instance.levelLoad();
                        if (GameManager.instance.CurrentScene() != GameManager.Scene.TUTORIAL)
                        {
                            Startlevel();
                        }
                    }
                    else if (SwipeHalf.startTutTimer == true && GameManager.instance.CurrentScene() == GameManager.Scene.TUTORIAL)
                    {
                        Startlevel();
                    }
                    if (Input.GetKey(KeyCode.R))
                    {
                        GameManager.instance.levelLoad();
                    }
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
                    if (GameManager.instance.levelManager.multiplier > 1 || GameManager.instance.TutorialState() == GameManager.Tutorial.MOVEMENT && (GameManager.instance.levelManager.targetReached == true || timerStart2 == true) || GameManager.instance.isPaused)
                    {
                        timeLeftInLevel -= 0;
                        timeSliderLeft.transform.Find("Fill Area/Fill").GetComponent<Image>().color = new Color(135f / 255f, 135f / 255f, 135f / 255f);
                        timeSliderRight.transform.Find("Fill Area/Fill").GetComponent<Image>().color = new Color(135f / 255f, 135f / 255f, 135f / 255f);
                    }
                    else if (!imInSlowMotion)
                    {
                        timeLeftInLevel -= Time.deltaTime;
                        timeSliderLeft.transform.Find("Fill Area/Fill").GetComponent<Image>().color = sliderCol;
                        timeSliderRight.transform.Find("Fill Area/Fill").GetComponent<Image>().color = sliderCol;
                    }
                    else
                    {
                        timeLeftInLevel -= 0.005f;
                    }
                }
                timerText.text = timeLeftInLevel.ToString("F1"); // for the level timer
                if (timeLeftInLevel <= timeTicker && GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
                {
                    GameManager.instance.timerUpdate(timeTicker);
                    timeTicker -= 1;
                }

                if (timeLeftInLevel <= timeRunningOut)
                {
                    timeRunningOut = -1;
                    GameManager.instance.changeMusicState(AudioManager.IN_LEVEL_TIME_RUNNING_OUT);  // FOR AUDIO
                }

                //when timer runs out:
                if (timeLeftInLevel <= 0f)
                { //Move stuff to events
                    if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
                    {
                        timerText.text = "0";  // for the level timer
                        timerText.color = Color.red;
                        GameManager.instance.timerOut();
                    }
                    else if (GameManager.instance.CurrentScene() == GameManager.Scene.TUTORIAL && GameManager.instance.TutorialState() == GameManager.Tutorial.MOVEMENT)
                    {
                        if (GameManager.instance.levelManager.targetReached == false)
                        {
                            transform.position = GameManager.instance.levelManager.playerPos;
                            timeLeftInLevel = GameManager.instance.levelManager.timeToCompleteLevel;
                            GameManager.instance.levelManager.guideText.text = LanguageManager.instance.ReturnWord("TryAgain");
                            SwipeHalf.startTutTimer = false;
                            timerStart2 = true;
                        }
                    }
                }
                else if (GameManager.instance.levelManager.targetReached == true)
                {
                    timer += Time.deltaTime;
                    if (timer > 2f)
                    {
                        transform.position = GameManager.instance.levelManager.playerPos;
                        GetComponent<Rigidbody>().Sleep();
                        timeLeftInLevel = 0;
                        timerUI.SetActive(false);
                        GameObject.Find("Target").SetActive(false);
                        GameManager.instance.tutorialTaskCompleted();
                        timer = 0;
                        timerStart = false;
                        GameManager.instance.tutorial = GameManager.Tutorial.ATTACK;
                    }
                }              
                else if (timerStart2 == true && timerStart == false)
                {
                    timer += Time.deltaTime;
                    if (timer > 2f)
                    {
                        GameManager.instance.levelManager.guideText.text = LanguageManager.instance.ReturnWord("Tut1.2");
                        timer = 0;
                        timerStart2 = false;
                    }
                }
                timeSliderLeft.value = timeLeftInLevel / totalTime;
                timeSliderRight.value = timeLeftInLevel / totalTime;

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

    void OnEnable()
    {
        GameManager.instance.OnTimerOut += EndLevel;
    }

    void OnDisable()
    {
        GameManager.instance.OnTimerOut -= EndLevel;
    }
}
