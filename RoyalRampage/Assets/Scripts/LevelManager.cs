using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/*
 level and score manager
 Used for public variables to be tweaked by level designer 
 */
public class LevelManager : MonoBehaviour
{

    [SerializeField]
    int scoreToCompleteLevel = 10;
    public int timeToCompleteLevel = 10;
    private int amountOfObjects;
    private float initialRageObjects;
    private Text MultiplierText;
    public int maxScore = 57;
    public int currencyPerStar = 50;
    public int star1;
    public int star2;
    public int star3;
    public int star4;
    public int star5;

    public Vector3 playerPos;
    float t;
    [HideInInspector]
    public bool targetReached = false;
    int[] starValue;
    int starIndex;
    int stars = 0;
    int score = 0;
    Text starText;
    Text scoreText;
    Text minScoreText;
    [HideInInspector]
    public Text guideText;
    GameObject InGamePanel;
    GameObject ReplayPanel;
    Text replayScoreText;
    Text highScoreText;
    GameObject replayScore;
    GameObject ReplayBtn;
    GameObject NewLevelBtn;
    GameObject IntroTapPanel;
    GameObject goalPanel;
    GameObject scorePanel;
    GameObject rageMeter;
    GameObject starPoints;
    GameObject panel;
    GameObject highscore;
    GameObject twoHandsSplitScreen;
    GameObject levelNr;
    GameObject lvlNr;
    GameObject InGameGem;
    GameObject highScoreTextForTUT;
    GameObject[] gems;
    private GameObject continueButton;
    [HideInInspector]
    public int multiplier;
    int tempMulti;
    int tempScore;
    bool updateGoal;
    [HideInInspector]
    //You'll regret using this boolean
    public bool spawnedObject = false;
    //
    [Header("Tutorial Objects")]
    public GameObject tutorial_candlestand;
    public GameObject tutorial_chair;
    public GameObject tutorial_cabinet;
    public GameObject tutorial_vaseb;
    public GameObject tutorial_vasec;

    private ObjectBehavior objBehavior;

    GameObject tutorialObj1;
    GameObject tutorialObj2;
    GameObject tutorialObj3;
    GameObject tutorialObj4;
    GameObject tutorialObj5;
    GameObject tutorialObj6;
    bool smashed;
    bool completed;
    bool startTimer, startTimer2, startTimer3;
    float timer, timer2, timer3;

    List<LevelAndObjects> highScoreList;
    List<LevelAndStars> starsList;

    void OnEnable()
    {
        GameManager.instance.OnObjectDestructed += IncreaseScore;
        GameManager.instance.OnLevelLoad += StartLevel;
        GameManager.instance.OnTimerOut += ShowEnding;
    }

    void OnDisable()
    {
        GameManager.instance.OnObjectDestructed -= IncreaseScore;
        GameManager.instance.OnLevelLoad -= StartLevel;
        GameManager.instance.OnTimerOut -= ShowEnding;
    }

    void Start()
    {
        highScoreList = new List<LevelAndObjects>();
        starsList = new List<LevelAndStars>();
        gems = new GameObject[5];
        MultiplierText = GameObject.Find("Multiplier").GetComponent<Text>();
        completed = false;
        updateGoal = false;
        smashed = false;
        startTimer = false;
        startTimer2 = false;
        timer = 0;
        timer2 = 0;
        starIndex = 0;
        starValue = new int[5] { star1, star2, star3, star4, star5 };
        playerPos = GameManager.instance.player.transform.position;
        multiplier = 1;
        ObjectManagerV2.instance.countMultiTime = 0;
        tempMulti = 1;
        tempScore = scoreToCompleteLevel;
        amountOfObjects = 1;
        initialRageObjects = GameManager.instance.player.GetComponent<PlayerStates>().rageObjects;
        switch (GameManager.instance.CurrentScene())
        {
            case GameManager.Scene.GAME:
                for (int i = 1; i <= gems.Length; i++)
                {
                    gems[i - 1] = GameObject.Find("Gem" + i.ToString());
                }
                scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
                scoreText.text = score.ToString(); // in game score
                minScoreText = GameObject.Find("MinScoreText").GetComponent<Text>();
                minScoreText.text = LanguageManager.instance.ReturnWord("InGameGoal") + " " + scoreToCompleteLevel;
                guideText = GameObject.FindGameObjectWithTag("GuideText").GetComponent<Text>();
                SetLevelTextScript.instance.SetText(GameManager.instance.currentLevel - 1);
                SetReachGoalScript.instance.SetText(scoreToCompleteLevel);
                guideText = GameObject.Find("GuideText").GetComponent<Text>();
                guideText.text = "";
                twoHandsSplitScreen = GameObject.Find("TwoHandsSplitScreen");
                highScoreTextForTUT = GameObject.FindGameObjectWithTag("HighScoreText");
                InGameGem = GameObject.FindGameObjectWithTag("InGameGem");
                ReplayPanel = GameObject.FindGameObjectWithTag("ReplayPanel");
                InGamePanel = GameObject.FindGameObjectWithTag("InGamePanel");
                IntroTapPanel = GameObject.FindGameObjectWithTag("IntroTapPanel");
                ReplayBtn = GameObject.Find("ReplayButton");
                NewLevelBtn = GameObject.Find("NewLevelButton");
                continueButton = GameObject.Find("ContinueButton");
                //starText = GameObject.Find("stars").GetComponent<Text>();
                replayScoreText = GameObject.FindGameObjectWithTag("GOscore").GetComponent<Text>();
                highScoreText = GameObject.FindGameObjectWithTag("HighScore").GetComponent<Text>();
                highScoreTextForTUT.SetActive(true);
                twoHandsSplitScreen.SetActive(false);
                ReplayPanel.SetActive(false);
                continueButton.SetActive(false);
                InGamePanel.SetActive(false);
                InGameGem.SetActive(false);
                break;
            case GameManager.Scene.TUTORIAL:
                for (int i = 1; i <= gems.Length; i++)
                {
                    gems[i - 1] = GameObject.Find("Gem" + i.ToString());
                }
                guideText = GameObject.FindGameObjectWithTag("GuideText").GetComponent<Text>();
                ReplayPanel = GameObject.FindGameObjectWithTag("ReplayPanel");
                InGamePanel = GameObject.FindGameObjectWithTag("InGamePanel");
                IntroTapPanel = GameObject.FindGameObjectWithTag("IntroTapPanel");
                ReplayBtn = GameObject.Find("ReplayButton");
                NewLevelBtn = GameObject.Find("NewLevelButton");
                continueButton = GameObject.Find("ContinueButton");
                replayScore = GameObject.FindGameObjectWithTag("GOscore");
                replayScoreText = GameObject.FindGameObjectWithTag("GOscore").GetComponent<Text>();
                highScoreText = GameObject.FindGameObjectWithTag("HighScore").GetComponent<Text>();
                highScoreTextForTUT = GameObject.FindGameObjectWithTag("HighScoreText");
                goalPanel = GameObject.Find("InGameGUI/GoalPanel");
                scorePanel = GameObject.Find("InGameGUI/ScorePanel");
                highscore = GameObject.Find("replayPanelv2/Highscore");
                rageMeter = GameObject.Find("InGameGUI/RageMeter");
                panel = GameObject.Find("InGameGUI/Panel");
                starPoints = GameObject.Find("replayPanelv2/Points");
                levelNr = GameObject.Find("replayPanelv2/levelnumber");
                lvlNr = GameObject.Find("IntroTapPanel/TitlePanelBG/BackgroundFrame/LevelNumber");
                twoHandsSplitScreen = GameObject.Find("TwoHandsSplitScreen");

                goalPanel.SetActive(false);
                scorePanel.SetActive(false);
                rageMeter.SetActive(false);
                panel.SetActive(false);
                ReplayPanel.SetActive(false);
                continueButton.SetActive(false);
                InGamePanel.SetActive(false);
                IntroTapPanel.SetActive(false);
                highScoreTextForTUT.SetActive(false);
                break;
                // GameManager.instance.levelLoad(); // FOR AUDIO
        }
    }

    private void IncreaseScore(GameObject destructedObj)
    {
        if (GameManager.instance.canPlayerDestroy && GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
        {
            int points = destructedObj.GetComponent<ObjectBehavior>().score;
            scoreText.text = score.ToString(); // in game score
            while (amountOfObjects <= ObjectManagerV2.instance.countObjects)
            {
                //Timer shouldn't change during combo.
                ObjectManagerV2.instance.countObjects = Mathf.Abs(amountOfObjects - ObjectManagerV2.instance.countObjects);
                multiplier++;
                amountOfObjects += amountOfObjects;
                ObjectManagerV2.instance.countMultiTime = 0;
            }
            score += points * multiplier;
            scoreText.text = score.ToString(); // in game score
            GameManager.instance.score = score;
            //GameManager.instance.player.GetComponent<StampBar>().tempScore += points;
            //StampBar.increaseFill = true;  ///WHY IS THIS HERE?????
            //GameManager.instance.player.GetComponent<StampBar>().timeToLowRage = 0f;
        }
    }

    private void StartLevel()
    {
        //guideText.gameObject.SetActive(false);

        InGamePanel.SetActive(true);
        if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
        {
            IntroTapPanel.SetActive(false);
            guideText.text = "";
        }
        else twoHandsSplitScreen.SetActive(false);

    }

    //after the timer is out (wait for animation?)
    private void ShowEnding()
    {
        // GameManager.instance.levelUnLoad(); // FOR AUDIO

        if (score >= scoreToCompleteLevel)
        {
            if (GameManager.instance.currentLevel != 1)
            {
                guideText.text = "Level completed!";
            }
            else guideText.text = LanguageManager.instance.ReturnWord("TutEnd");
        }
        else
        {
            guideText.text = "Game over";
        }

        guideText.gameObject.SetActive(true);
        StartCoroutine(ShowContinueScreen(guideText.text));
    }

    void Update()
    {

        ObjectManagerV2.instance.countMultiTime += Time.deltaTime;
        switch (GameManager.instance.CurrentScene())
        {
            case GameManager.Scene.GAME:
                if (ObjectManagerV2.instance.countMultiTime > ObjectManagerV2.instance.multiplierTimer)
                {
                    MultiplierText.transform.localScale = new Vector3(1f, 1f, 1f);
                    multiplier = 1;
                    amountOfObjects = 1;
                    ObjectManagerV2.instance.countObjects = 0;
                    tempMulti = 1;
                    t = 0f;
                    ObjectManagerV2.instance.countMultiTime = 0;
                }

                if (score >= tempScore && score != 0)
                {
                    updateGoal = true;
                    starIndex += 1;
                }
                
                if (updateGoal == true)
                {
                    if (starIndex <= 4)
                    {
                        tempScore = starValue[starIndex];
                        minScoreText.text = LanguageManager.instance.ReturnWord("InGameGoal") + " " + tempScore;
                    GameManager.instance.gemScoreDisplay();
                    InGameGem.SetActive(true);
                    StartCoroutine(DisplayIngameGem());
                    updateGoal = false;
                    }
                    else if (starIndex == 5 && SaveHighScore.instance.ReturnListWithObjects((GameManager.instance.currentLevel - 1).ToString())[0].HighScore != 0)
                    {
                        minScoreText.text = LanguageManager.instance.ReturnWord("InGameGoal") + " " + SaveHighScore.instance.ReturnListWithObjects((GameManager.instance.currentLevel - 1).ToString())[0].HighScore;
                    GameManager.instance.gemScoreDisplay();
                    InGameGem.SetActive(true);
                    StartCoroutine(DisplayIngameGem());
                }
                }

                if (score >= scoreToCompleteLevel)
                {
                    continueButton.SetActive(true);
                }
                else continueButton.SetActive(false);

                if (ObjectManagerV2.instance.countMultiTime < ObjectManagerV2.instance.multiplierTimer)
                {
                    t += Time.deltaTime / ObjectManagerV2.instance.multiplierTimer;
                    MultiplierText.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0.5f, 0.5f, 0.5f), t);
                }

                if (multiplier != tempMulti && multiplier != 1)
                {
                    MultiplierText.transform.localScale = new Vector3(1f, 1f, 1f);
                    tempMulti = multiplier;
                    t = 0f;
                }

                if (multiplier == 1)
                {
                    MultiplierText.text = "";
                }
                else
                {
                    MultiplierText.text = "x" + multiplier.ToString();
                }
                break;
            case GameManager.Scene.TUTORIAL:
                switch (GameManager.instance.tutorial)
                {
                    case GameManager.Tutorial.MOVEMENT:
                        guideText.text = LanguageManager.instance.ReturnWord("Tut1.1");
                        if (SwipeHalf.startTutTimer == true)
                        {
                            //COLORED PANEL FOR COMMUNICATING MOVING
                            
                            guideText.text = LanguageManager.instance.ReturnWord("Tut1.2");
                        }
                        if (GameManager.instance.levelManager.targetReached == true)
                        {
                            guideText.text = LanguageManager.instance.ReturnWord("Tut1.3");
                        }
                        break;
                    case GameManager.Tutorial.ATTACK:
                        if (spawnedObject == false)
                        {
                            tutorialObj1 = (GameObject)Instantiate(tutorial_candlestand, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 0.5f)), Quaternion.identity);
                            GameManager.instance.player.transform.LookAt(tutorialObj1.transform);
                            guideText.text = LanguageManager.instance.ReturnWord("Tut2.0");
                            GameManager.instance.player.GetComponent<SwipeHalf>().swirlEnded = false;
                            GameManager.instance.player.GetComponent<Rigidbody>().isKinematic = true;
                            startTimer = false;
                            spawnedObject = true;
                        }
                        if (tutorialObj1 != null && tutorialObj1.GetComponent<ObjectBehavior>().hit == true)
                        {
                            //COLORED PANEL FOR COMMUNICATING ATTACK
                            //WE NEED TO DESTROY THE OBJECT (CHUNKS)!
                            guideText.text = LanguageManager.instance.ReturnWord("Tut2.1");
                            startTimer = true;

                        }

                        if (startTimer == true)
                        {
                            timer += Time.deltaTime;
                            if (timer > 2f && tutorialObj1 != null)
                            {
                                for (int p = 0; p < tutorialObj1.transform.childCount; p++)
                                {
                                    if (tutorialObj1.transform.GetChild(p).GetComponent<FracturedChunk>() != null)
                                    {
                                        tutorialObj1.transform.GetChild(p).gameObject.SetActive(true);
                                        tutorialObj1.transform.GetChild(p).GetComponent<MeshCollider>().enabled = true;
                                    }
                                }
                                tutorialObj1.GetComponent<FracturedObject>().CollapseChunks();
                                GameManager.instance.objectDestructed(tutorialObj1);
                                Destroy(tutorialObj1);
                                timer = 0;
                                startTimer = false;
                            }
                        }

                        if (tutorialObj1 == null)
                        {
                            timer += Time.deltaTime;
                            if (timer > 2f)
                            {
                                GameManager.instance.tutorialTaskCompleted();
                                timer = 0;
                                GameManager.instance.tutorial = GameManager.Tutorial.CHAIN;
                            }
                        }
                        break;
                    case GameManager.Tutorial.CHAIN:
                        if (spawnedObject == true)
                        {
                            tutorialObj1 = (GameObject)Instantiate(tutorial_chair, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                            tutorialObj2 = (GameObject)Instantiate(tutorial_cabinet, (GameManager.instance.player.transform.position - new Vector3(0.5f, -1, 4)), Quaternion.identity);
                            guideText.text = LanguageManager.instance.ReturnWord("Tut3.0");
                            //ObjectManagerV2.instance.canDamage = false; // WE ARE HERE WE HAVE TO NOT DESTROY THE OBJECT BEFORE THE CONDITION IS TRUE
                            completed = false;
                            startTimer = false;
                            panel.SetActive(true);
                            MultiplierText.text = "x1";
                            spawnedObject = false;
                        }
                        if (tutorialObj1 != null && tutorialObj1.GetComponent<ObjectBehavior>().hit == true)
                        {
                            startTimer = true;
                        }

                        if (tutorialObj2 == null)
                        {
                            guideText.text = LanguageManager.instance.ReturnWord("Tut3.2");
                            MultiplierText.text = "x2";
                            completed = true;
                            timer += Time.deltaTime;
                            if (timer > 2f)
                            {
                                //COLORED PANEL FOR COMMUNICATING ATTACK
                                //WE NEED TO DESTROY THE OBJECT (CHUNKS)
                                GameManager.instance.tutorialTaskCompleted();
                                timer = 0;
                                GameManager.instance.tutorial = GameManager.Tutorial.SWIRL;
                            }
                        }
                        else if (startTimer == true)
                        {
                            if (tutorialObj1 == null)
                            {
                                guideText.text = LanguageManager.instance.ReturnWord("Tut3.3");
                            }
                            timer2 += Time.deltaTime;
                            if (timer2 > 3f)
                            {
                                if (tutorialObj1 != null)
                                {
                                    guideText.text = LanguageManager.instance.ReturnWord("Tut3.3");
                                    for (int p = 0; p < tutorialObj1.transform.childCount; p++)
                                    {
                                        if (tutorialObj1.transform.GetChild(p).GetComponent<FracturedChunk>() != null)
                                        {
                                            tutorialObj1.transform.GetChild(p).gameObject.SetActive(true);
                                            tutorialObj1.transform.GetChild(p).GetComponent<MeshCollider>().enabled = true;
                                        }
                                    }
                                    tutorialObj1.GetComponent<FracturedObject>().CollapseChunks();
                                    GameManager.instance.objectDestructed(tutorialObj1);
                                    Destroy(tutorialObj1);
                                }
                                guideText.text = LanguageManager.instance.ReturnWord("Tut3.1");
                                tutorialObj1 = (GameObject)Instantiate(tutorial_chair, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                                timer2 = 0;
                                startTimer = false;
                            }
                        }
                        break;
                    case GameManager.Tutorial.SWIRL:
                        if (spawnedObject == false)
                        {
                            tutorialObj1 = (GameObject)Instantiate(tutorial_vaseb, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                            tutorialObj2 = (GameObject)Instantiate(tutorial_vaseb, (GameManager.instance.player.transform.position - new Vector3(-0.5f, 0, 1)), Quaternion.identity);
                            tutorialObj3 = (GameObject)Instantiate(tutorial_vaseb, (GameManager.instance.player.transform.position - new Vector3(1f, 0, 0)), Quaternion.identity);
                            tutorialObj4 = (GameObject)Instantiate(tutorial_vaseb, (GameManager.instance.player.transform.position - new Vector3(-1f, 0, 0)), Quaternion.identity);
                            tutorialObj5 = (GameObject)Instantiate(tutorial_vaseb, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, -1)), Quaternion.identity);
                            tutorialObj6 = (GameObject)Instantiate(tutorial_vaseb, (GameManager.instance.player.transform.position - new Vector3(-0.5f, 0, -1)), Quaternion.identity);
                            panel.SetActive(false);
                            GameManager.instance.player.transform.rotation = Quaternion.LookRotation(Vector3.back);
                            ObjectManagerV2.instance.canDamage = false;
                            GameManager.instance.player.GetComponent<SwipeHalf>().swirlEnded = true;
                            guideText.text = LanguageManager.instance.ReturnWord("Tut4.0");
                            timer = 0f;
                            startTimer = false;
                            spawnedObject = true;
                        }
                        if (PlayerStates.swiped == true)
                        {
                            guideText.text = LanguageManager.instance.ReturnWord("TryAgain"); // FAIL MESSAGE
                            startTimer = true;
                        }

                        if (startTimer == true)
                        {
                            GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut = false;
                            timer += Time.deltaTime;
                            if (timer > 2f)
                            {
                                startTimer = false;
                                TutObj(tutorialObj1, new Vector3(0.5f, 0, 1));
                                TutObj(tutorialObj2, new Vector3(-0.5f, 0, 1));
                                TutObj(tutorialObj3, new Vector3(1f, 0, 0));
                                TutObj(tutorialObj4, new Vector3(-1f, 0, 0));
                                TutObj(tutorialObj5, new Vector3(0.5f, 0, -1));
                                TutObj(tutorialObj6, new Vector3(-0.5f, 0, -1));

                                guideText.text = LanguageManager.instance.ReturnWord("Tut4.0");

                                timer = 0;
                            }
                        }
                        else if (GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut == true) //startTimer changed, change back if broken
                        {
                            ObjectManagerV2.instance.canDamage = true;
                            guideText.text = LanguageManager.instance.ReturnWord("Tut4.1");
                            timer2 += Time.deltaTime;
                            if (timer2 > 2f)
                            {
                                GameManager.instance.tutorialTaskCompleted();
                                timer2 = 0;
                                GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut = false;
                                GameManager.instance.tutorial = GameManager.Tutorial.STOMP;
                            }
                        }
                        break;
                    case GameManager.Tutorial.STOMP:
                        //Show the bar with animation
                        if (spawnedObject == true)
                        {
                            tutorialObj1 = (GameObject)Instantiate(tutorial_vasec, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                            tutorialObj2 = (GameObject)Instantiate(tutorial_vaseb, (GameManager.instance.player.transform.position - new Vector3(-0.5f, 0, 1)), Quaternion.identity);
                            tutorialObj3 = (GameObject)Instantiate(tutorial_vaseb, (GameManager.instance.player.transform.position - new Vector3(1f, 0, 0)), Quaternion.identity);
                            tutorialObj4 = (GameObject)Instantiate(tutorial_vasec, (GameManager.instance.player.transform.position - new Vector3(-1f, 0, 0)), Quaternion.identity);
                            tutorialObj5 = (GameObject)Instantiate(tutorial_vasec, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, -1)), Quaternion.identity);
                            tutorialObj6 = (GameObject)Instantiate(tutorial_vaseb, (GameManager.instance.player.transform.position - new Vector3(-0.5f, 0, -1)), Quaternion.identity);
                            completed = false;
                            ObjectManagerV2.instance.canDamage = false;
                            GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut = false;

                            rageMeter.SetActive(true);
                            GameManager.instance.player.GetComponent<StampBar>().tempScore = 0;
                            GameManager.instance.player.GetComponent<PlayerStates>().rageObjects = 0;
                            GameManager.instance.player.GetComponent<StampBar>().slider.value = 1f;
                            GameManager.instance.player.GetComponent<StampBar>().ready = true;
                            GameManager.instance.player.GetComponent<SwipeHalf>().swirlEnded = true;

                            guideText.text = LanguageManager.instance.ReturnWord("Tut5.0");

                            startTimer3 = true;
                            ObjectManagerV2.instance.isGrounded = true;
                            spawnedObject = false;
                        }

                        if (GameManager.instance.player.GetComponent<SwipeHalf>().stompTut == true && completed == false && startTimer == false && startTimer2 == false)
                        {
                            startTimer3 = false;
                            timer3 = 0;
                            guideText.text = LanguageManager.instance.ReturnWord("Tut5.2");

                            ObjectManagerV2.instance.canDamage = true;
                            if ((PlayerStates.swiped == true || GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut == true) /*&& startTimer == false && startTimer2 == false*/)
                            {
                                completed = true;
                                GameManager.instance.player.GetComponent<StampBar>().tempScore = 0;
                                GameManager.instance.player.GetComponent<PlayerStates>().rageObjects = initialRageObjects;
                                GameManager.instance.player.GetComponent<StampBar>().slider.value = 0f;
                                GameManager.instance.player.GetComponent<StampBar>().ready = true;
                                guideText.text = LanguageManager.instance.ReturnWord("Tut5.3");

                                StartCoroutine(Delay());
                                return;
                            }
                            else if (ObjectManagerV2.instance.isGrounded == true && completed == false)
                            {
                                guideText.text = LanguageManager.instance.ReturnWord("Tut5.1");

                                GameManager.instance.player.GetComponent<StampBar>().tempScore = 0;
                                GameManager.instance.player.GetComponent<PlayerStates>().rageObjects = 0;
                                GameManager.instance.player.GetComponent<StampBar>().slider.value = 1f;
                                GameManager.instance.player.GetComponent<StampBar>().ready = true;
                                GameManager.instance.player.GetComponent<SwipeHalf>().stompTut = false;
                                ObjectManagerV2.instance.canDamage = false;
                            }
                        }
                        else if (PlayerStates.swiped == true && completed == false)
                        {
                            guideText.text = LanguageManager.instance.ReturnWord("TryAgain");

                            startTimer = true;
                            startTimer3 = false;
                            timer3 = 0;
                        }
                        else if (GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut == true && completed == false)
                        {
                            guideText.text = LanguageManager.instance.ReturnWord("TryAgain");

                            startTimer2 = true;
                            startTimer3 = false;
                            timer3 = 0;
                            GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut = false;
                        }

                        if (startTimer == true && completed == false)
                        {
                            GameManager.instance.player.GetComponent<SwipeHalf>().stompTut = false;
                            timer += Time.deltaTime;
                            if (timer > 2f)
                            {
                                TutObj(tutorialObj1, new Vector3(0.5f, 0, 1));
                                TutObj(tutorialObj2, new Vector3(-0.5f, 0, 1));
                                TutObj(tutorialObj3, new Vector3(1f, 0, 0));
                                TutObj(tutorialObj4, new Vector3(-1f, 0, 0));
                                TutObj(tutorialObj5, new Vector3(0.5f, 0, -1));
                                TutObj(tutorialObj6, new Vector3(-0.5f, 0, -1));

                                timer = 0;
                                guideText.text = LanguageManager.instance.ReturnWord("Tut5.1");

                                GameManager.instance.player.GetComponent<StampBar>().tempScore = 0;
                                GameManager.instance.player.GetComponent<PlayerStates>().rageObjects = 0;
                                GameManager.instance.player.GetComponent<StampBar>().slider.value = 1f;
                                GameManager.instance.player.GetComponent<StampBar>().ready = true;
                                startTimer = false;
                            }
                        }
                        if (startTimer2 == true && completed == false)
                        {
                            GameManager.instance.player.GetComponent<SwipeHalf>().stompTut = false;
                            timer2 += Time.deltaTime;
                            if (timer2 > 2f)
                            {
                                TutObj(tutorialObj1, new Vector3(0.5f, 0, 1));
                                TutObj(tutorialObj2, new Vector3(-0.5f, 0, 1));
                                TutObj(tutorialObj3, new Vector3(1f, 0, 0));
                                TutObj(tutorialObj4, new Vector3(-1f, 0, 0));
                                TutObj(tutorialObj5, new Vector3(0.5f, 0, -1));
                                TutObj(tutorialObj6, new Vector3(-0.5f, 0, -1));

                                timer2 = 0;
                                guideText.text = LanguageManager.instance.ReturnWord("Tut5.1");
 
                                GameManager.instance.player.GetComponent<StampBar>().tempScore = 0;
                                GameManager.instance.player.GetComponent<PlayerStates>().rageObjects = 0;
                                GameManager.instance.player.GetComponent<StampBar>().slider.value = 1f;
                                GameManager.instance.player.GetComponent<StampBar>().ready = true;
                                startTimer2 = false;
                            }
                        }
                        if (startTimer3 == true && completed == false)
                        {
                            timer3 += Time.deltaTime;
                            if (timer3 > 3f)
                            {
                                guideText.text = LanguageManager.instance.ReturnWord("Tut5.01");

                                timer3 = 0;
                                startTimer3 = false;
                            }
                        }
                        break;
                }
                break;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.player.GetComponent<SwipeHalf>().stompTut = false;
        GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut = false;
        ObjectManagerV2.instance.canDamage = true;
        GameManager.instance.tutorial = GameManager.Tutorial.DEFAULT;
        GameManager.instance.player.GetComponent<Rigidbody>().isKinematic = false;
        GameManager.instance.isInstructed = true;
        GameManager.instance.tutorialTaskCompleted();
        continueButton.SetActive(true);

        //GameManager.instance.currentScene = GameManager.Scene.GAME;
    }

    IEnumerator DisplayIngameGem() { 
        yield return new WaitForSeconds(1f);
        InGameGem.SetActive(false);
    }

    public void TutObj(GameObject obj, Vector3 place)
    {
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.transform.position = GameManager.instance.player.transform.position - place;
        obj.transform.rotation = Quaternion.identity;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<ObjectBehavior>().slowed = false;
        obj.GetComponent<ObjectBehavior>().flying = false;
        obj.GetComponent<ObjectBehavior>().canRotate = false;
    }

    public void Continue()
    {
        guideText.text = "Level completed!";
        guideText.gameObject.SetActive(true);
        StartCoroutine(ShowContinueScreen(guideText.text));
    }
    public void Stars()
    {
        if (score >= star1 && score < star2)
        {
            stars = 1;
        }
        if (score >= star2 && score < star3)
        {
            stars = 2;
        }
        if (score >= star3 && score < star4)
        {
            stars = 3;
        }
        if (score >= star4 && score < star5)
        {
            stars = 4;
        }
        if (score >= star5)
        {
            stars = 5;
        }
    }


    public void CalculateCurrency(int starsVal)
    {
        GameManager.instance.currency += starsVal * currencyPerStar;
    }

    //show replay screen after animation is done
    private IEnumerator ShowContinueScreen(string levelResult)
    {
        yield return new WaitForSeconds(1f);

        // FOR AUDIO
        GameManager.instance.scoreScreenOpen();
        GameManager.instance.changeMusicState(AudioManager.IN_SCORE_SCREEN);  // FOR AUDIO

        Stars();
        highScoreList = SaveHighScore.instance.ReturnListWithObjects((GameManager.instance.currentLevel - 1).ToString());
        starsList = SaveStars.instance.ReturnListWithObjects((GameManager.instance.currentLevel - 1).ToString());


        if (GameManager.instance.stars != null)
        {
            if (stars > starsList[0].Stars)
            {
                CalculateCurrency(stars - starsList[0].Stars);
                GameManager.instance.stars[GameManager.instance.currentLevel - 1] = stars;
                SaveStars.instance.saveData((GameManager.instance.currentLevel - 1).ToString(), stars);
            }
        }

        highScoreText.text = highScoreList[0].HighScore.ToString();
        replayScoreText.text = "0"; //will be updated in counting loop    
        if (score >= highScoreList[0].HighScore)
        {
            SaveHighScore.instance.saveData((GameManager.instance.currentLevel - 1).ToString(), score);
        }
        if (GameManager.instance.currentLevel == 1)
        {
            highscore.SetActive(false);
        }
        for (int i = 0; i < gems.Length; i++)
        {
            gems[i].SetActive(false);
        }



        InGamePanel.SetActive(false);
        ReplayPanel.SetActive(true);
        // NewLevelBtn.SetActive(true);


        Text levelNum = GameObject.Find("levelnumber").GetComponent<Text>();
        if (GameManager.instance.currentLevel != 1)
        {
            levelNum.text = LanguageManager.instance.ReturnWord("CurrentLevel") + " " + (GameManager.instance.currentLevel - 1).ToString();
        }
        else levelNum.text = LanguageManager.instance.ReturnWord("Tutorial"); //Tutorial needs to be added to language manager? I guess?

        switch (levelResult)
        {
		case "Level completed!":
			GameManager.instance.levelWon = true;
                //ReplayBtn.SetActive(true);
                //NewLevelBtn.SetActive(true);

			if (GameManager.instance.levelsUnlocked < GameManager.instance.NUM_OF_LEVELS_IN_GAME && GameManager.instance.currentLevel == GameManager.instance.levelsUnlocked) {
				GameManager.instance.levelsUnlocked++;
			}
			//disable next button after last level
			if (GameManager.instance.levelsUnlocked == GameManager.instance.NUM_OF_LEVELS_IN_GAME && GameManager.instance.currentLevel == GameManager.instance.levelsUnlocked) {
				NewLevelBtn.SetActive(false);
			}
                break;

            case "Game over":
                GameManager.instance.levelWon = false;
                //ReplayBtn.SetActive(true);
                NewLevelBtn.SetActive(false);
                //starText.text = stars.ToString();
                break;
            default:
                if (GameManager.instance.levelsUnlocked < GameManager.instance.NUM_OF_LEVELS_IN_GAME && GameManager.instance.currentLevel == GameManager.instance.levelsUnlocked)
                {
                    GameManager.instance.levelsUnlocked++;
                }
                break;
        }

        GameManager.instance.Save();
        if (GameManager.instance.currentLevel != 1)
        {
            StartCoroutine(CountPointsTo(score)); // show counting score
        }
        else StartCoroutine(CountPointsTo(2000)); // show counting score
    }

    //counting score "animation"
    IEnumerator CountPointsTo(int new_score)
    {
        highScoreList = SaveHighScore.instance.ReturnListWithObjects((GameManager.instance.currentLevel - 1).ToString());
        if (new_score > 0)
        {
            yield return new WaitForSeconds(1f);
            GameManager.instance.startCountingPoints();
            int start = 0;
            float duration = Mathf.Clamp((float)new_score * (1f / 100f), 0f, 1.5f); //show with speed of 100 points per second, max duration 1.5 seconds
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
            {
                float progress = timer / duration;
                int temp_score = (int)Mathf.Lerp(start, new_score, progress);
                replayScoreText.text = temp_score.ToString();
                GameManager.instance.audioManager.UpdatePointCounter(temp_score);
                yield return null;
            }
            GameManager.instance.audioManager.UpdatePointCounter(new_score);
            GameManager.instance.finishedCountingPoints();
        }
        replayScoreText.text = new_score.ToString();
        highScoreText.text = highScoreList[0].HighScore.ToString();

        for (int i = 0; i < stars; i++)
        {
            //Play Sound here (Add delay with coroutine)
            yield return new WaitForSeconds(0.5f);
            GameManager.instance.gemScoreDisplay(); //AUDIO FOR ONE GEM
            gems[i].SetActive(true);
        }
    }
}
