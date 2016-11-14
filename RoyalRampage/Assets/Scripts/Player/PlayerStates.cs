using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 Player manager with states and level timer, input should connect here
 */
public class PlayerStates : MonoBehaviour
{

    [HideInInspector]
    public bool imInSlowMotion, lifted, hitObject;
    [Header("ApplyClamp")]
    public bool clamped = true;
    [Header("Forces")]
    public float maxMoveForce;
    public float torgueForce;
    public float hitForce;
    public float swirlForce;
    public float liftForce;
    [Header("Times")]
    public float timeForSwipe;
    public float timeForCircle;
    public float SameTapTime;
    public float doubleTapTime;
    public float gravityTimer;
    [Header("Radius")]
    public float swirlRadius;
    public float liftRadius;
    [Header("Mixed")]
    public float distSwipe;
    public float rotationSpeed;
    public float degreesInAir;
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



    private enum PlayerState
    {
        READY, IDLE, WALKING, ATTACKING, ENDING
    }

    PlayerState state = PlayerState.READY;
    float timeLeftInLevel = 0f; //timeleft to complete the level
    Text timerText;


    void Awake()
    {
        //DontDestroyOnLoad (gameObject);
        //update timer
        timerText = GameObject.Find("TimeLeftText").GetComponent<Text>();
        timeLeftInLevel = GameManager.instance.levelManager.timeToCompleteLevel;
        timerText.text = "Timer: " + timeLeftInLevel.ToString("F1");
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
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Startlevel();
                }

                if (Input.GetKey(KeyCode.R))
                {
                    Startlevel();
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
                if (!imInSlowMotion)
                {
                    timeLeftInLevel -= Time.deltaTime;
                }
                else
                {
                    timeLeftInLevel -= 0.005f;
                }
                timerText.text = "Timer: " + timeLeftInLevel.ToString("F1");

                //when timer runs out:
                if (timeLeftInLevel <= 0f)
                {
                    timerText.text = "Timer: 0";
                    timerText.color = Color.red;
                    state = PlayerState.ENDING;
                    GameManager.instance.timerOut();
                    GameManager.instance.canPlayerDestroy = false;
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

    private void Move()
    {

    }
}
