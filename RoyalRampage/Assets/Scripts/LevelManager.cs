using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    GameObject ReplayBtn;
    GameObject NewLevelBtn;
    GameObject IntroTapPanel;
    GameObject goalPanel;
    GameObject scorePanel;
    GameObject rageMeter;
    GameObject panel;
    GameObject[] gems;
    private GameObject continueButton;
    [HideInInspector]
    public int multiplier;
    int tempMulti;
    [HideInInspector]
    //You'll regret using this boolean
    public bool spawnedObject = false;
    [Header("Tutorial Object")]
    public GameObject tutorialPrefab;
    public GameObject tutorialPrefab2;
    public GameObject tutorialPrefab3;
    private ObjectBehavior objBehavior;

    GameObject tutorialBarrel;
    GameObject tutorialBarrel2;
    GameObject tutorialBarrel3;
    GameObject tutorialBarrel4;
    GameObject tutorialBarrel5;
    GameObject tutorialBarrel6;
    bool smashed;
    bool completed;
    bool startTimer;
    float timer, timer2, timer3;
    int tutorialState;

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
        MultiplierText = GameObject.Find("Multiplier").GetComponent<Text>();
        completed = false;
        tutorialState = 0;
        smashed = false;
        startTimer = false;
        timer = 0;
        timer2 = 0;
        playerPos = GameManager.instance.player.transform.position;
        multiplier = 1;
        ObjectManagerV2.instance.countMultiTime = 0;
        tempMulti = 1;
        amountOfObjects = 1;
        switch (GameManager.instance.CurrentScene())
        {
            case GameManager.Scene.GAME:
                gems = GameObject.FindGameObjectsWithTag("Gem");
                scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
                scoreText.text = score.ToString() + " $"; // in game score
                minScoreText = GameObject.Find("MinScoreText").GetComponent<Text>();
                minScoreText.text = LanguageManager.instance.ReturnWord("InGameGoal") + " " + scoreToCompleteLevel + " $";
                guideText = GameObject.FindGameObjectWithTag("GuideText").GetComponent<Text>();
                SetLevelTextScript.instance.SetText(GameManager.instance.currentLevel);
                SetReachGoalScript.instance.SetText(scoreToCompleteLevel);
                guideText = GameObject.Find("GuideText").GetComponent<Text>();
                guideText.text = "";

                ReplayPanel = GameObject.FindGameObjectWithTag("ReplayPanel");
                InGamePanel = GameObject.FindGameObjectWithTag("InGamePanel");
                IntroTapPanel = GameObject.FindGameObjectWithTag("IntroTapPanel");
                ReplayBtn = GameObject.Find("ReplayButton");
                NewLevelBtn = GameObject.Find("NewLevelButton");
                continueButton = GameObject.Find("ContinueButton");
                //starText = GameObject.Find("stars").GetComponent<Text>();
                replayScoreText = GameObject.FindGameObjectWithTag("GOscore").GetComponent<Text>();
                ReplayPanel.SetActive(false);
                continueButton.SetActive(false);
                InGamePanel.SetActive(false);
                break;
            case GameManager.Scene.TUTORIAL:
                gems = GameObject.FindGameObjectsWithTag("Gem");
                // print("Tut");
                guideText = GameObject.FindGameObjectWithTag("GuideText").GetComponent<Text>();
                guideText.text = "Swipe the left side of the screen to move";
                ReplayPanel = GameObject.FindGameObjectWithTag("ReplayPanel");
                InGamePanel = GameObject.FindGameObjectWithTag("InGamePanel");
                IntroTapPanel = GameObject.FindGameObjectWithTag("IntroTapPanel");
                ReplayBtn = GameObject.Find("ReplayButton");
                NewLevelBtn = GameObject.Find("NewLevelButton");
                continueButton = GameObject.Find("ContinueButton");
                //starText = GameObject.Find("stars").GetComponent<Text>();
                replayScoreText = GameObject.FindGameObjectWithTag("GOscore").GetComponent<Text>();
                goalPanel = GameObject.Find("InGameGUI/GoalPanel");
                scorePanel = GameObject.Find("InGameGUI/ScorePanel");
                rageMeter = GameObject.Find("InGameGUI/RageMeter");
                panel = GameObject.Find("InGameGUI/Panel");
                goalPanel.SetActive(false);
                scorePanel.SetActive(false);
                rageMeter.SetActive(false);
                panel.SetActive(false);
                ReplayPanel.SetActive(false);
                continueButton.SetActive(false);
                InGamePanel.SetActive(false);
                break;
                // GameManager.instance.levelLoad(); // FOR AUDIO
        }
    }

    private void IncreaseScore(GameObject destructedObj)
    {
        if (GameManager.instance.canPlayerDestroy && GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
        {
            int points = destructedObj.GetComponent<ObjectBehavior>().score;
            scoreText.text = score.ToString() + " $"; // in game score
            while (amountOfObjects <= ObjectManagerV2.instance.countObjects)
            {
                //Timer shouldn't change during combo.
                ObjectManagerV2.instance.countObjects = Mathf.Abs(amountOfObjects - ObjectManagerV2.instance.countObjects);
                multiplier++;
                amountOfObjects++;
            }
            score += points * multiplier;
            scoreText.text = score.ToString() + " $"; // in game score
            GameManager.instance.score = score;
            //GameManager.instance.player.GetComponent<StampBar>().tempScore += points;
            StampBar.increaseFill = true;
            //GameManager.instance.player.GetComponent<StampBar>().timeToLowRage = 0f;
        }
    }

    private void StartLevel()
    {
        //print ("started");
        //guideText.gameObject.SetActive(false);

        IntroTapPanel.SetActive(false);
        InGamePanel.SetActive(true);
        if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
        {
            guideText.text = "";
        }

    }

    //after the timer is out (wait for animation?)
    private void ShowEnding()
    {
        // print("ended");
        // GameManager.instance.levelUnLoad(); // FOR AUDIO

        if (score >= scoreToCompleteLevel)
        {
            guideText.text = "Level completed!";
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
        print(GameManager.instance.TutorialState());
        switch (GameManager.instance.CurrentScene())
        {
            case GameManager.Scene.GAME:
                //print(countMultiTime);
                if (ObjectManagerV2.instance.countMultiTime > ObjectManagerV2.instance.multiplierTimer)
                {
                    // print("I am in");
                    MultiplierText.transform.localScale = new Vector3(1f, 1f, 1f);
                    multiplier = 1;
                    amountOfObjects = 1;
                    ObjectManagerV2.instance.countObjects = 0;
                    tempMulti = 1;
                    t = 0f;
                    ObjectManagerV2.instance.countMultiTime = 0;
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

                //print(GameManager.instance.allStars);
                break;
            case GameManager.Scene.TUTORIAL:
                switch (GameManager.instance.tutorial)
                {
                    case GameManager.Tutorial.MOVEMENT:
                        if (SwipeHalf.startTutTimer == true)
                        {
                            //COLORED PANEL FOR COMMUNICATING MOVING
                            guideText.text = "Reach the finish-line before the timer runs out!";
                            //print((int)GameManager.instance.tutorial);
                        }
                        if (GameManager.instance.levelManager.targetReached == true)
                        {
                            guideText.text = "Great job!!!";
                        }
                        break;
                    case GameManager.Tutorial.ATTACK:
                        if (spawnedObject == false)
                        {
                            tutorialBarrel = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                            guideText.text = "Swipe the right side of the screen to hit the crate";
                            GameManager.instance.player.GetComponent<SwipeHalf>().swirlEnded = false;
                            spawnedObject = true;
                            smashed = false;
                        }
                        if (tutorialBarrel != null && (tutorialBarrel.GetComponent<ObjectBehavior>().hit == true || smashed == true))
                        {
                            //COLORED PANEL FOR COMMUNICATING ATTACK
                            //WE NEED TO DESTROY THE OBJECT (CHUNKS)!
                            print(tutorialBarrel.GetComponent<ObjectBehavior>().life);
                            smashed = true;
                            guideText.text = "You're amazing!!!";
                            //timer += Time.deltaTime;
                        }

                        if (tutorialBarrel == null)
                        {
                            timer += Time.deltaTime;
                            smashed = false;
                            if (timer > 1f)
                            {
                                timer = 0;
                                GameManager.instance.tutorial = GameManager.Tutorial.CHAIN;
                            }
                        }
                        break;
                    case GameManager.Tutorial.CHAIN:
                        if (spawnedObject == true)
                        {
                            tutorialBarrel = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                            tutorialBarrel2 = (GameObject)Instantiate(tutorialPrefab2, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 3.5f)), Quaternion.identity);
                            guideText.text = "Hit the service cart with the crate";
                            //ObjectManagerV2.instance.canDamage = false; // WE ARE HERE WE HAVE TO NOT DESTROY THE OBJECT BEFORE THE CONDITION IS TRUE
                            completed = false;
                            spawnedObject = false;
                            startTimer = false;
                        }
                        if (tutorialBarrel != null && tutorialBarrel.GetComponent<ObjectBehavior>().hit == true)
                        {
                            startTimer = true;
                        }

                        if (tutorialBarrel2 == null)
                        {
                            guideText.text = "Bullseye!";
                            completed = true;
                            timer += Time.deltaTime;
                            if (timer > 1f)
                            {
                                //COLORED PANEL FOR COMMUNICATING ATTACK
                                //WE NEED TO DESTROY THE OBJECT (CHUNKS)
                                GameManager.instance.tutorial = GameManager.Tutorial.SWIRL;
                                timer = 0;
                            }
                        }
                        else if (startTimer == true)
                        {
                            timer2 += Time.deltaTime;
                            if (timer2 > 1f && tutorialBarrel != null)
                            {
                                for (int p = 0; p < tutorialBarrel.transform.childCount; p++)
                                {
                                    if (tutorialBarrel.transform.GetChild(p).GetComponent<FracturedChunk>() != null)
                                    {
                                        tutorialBarrel.transform.GetChild(p).gameObject.SetActive(true);
                                        tutorialBarrel.transform.GetChild(p).GetComponent<MeshCollider>().enabled = true;
                                    }
                                }
                                tutorialBarrel.GetComponent<FracturedObject>().CollapseChunks();
                                GameManager.instance.objectDestructed(tutorialBarrel);
                                Destroy(tutorialBarrel);
                                startTimer = false;
                                timer2 = 0;
                                tutorialBarrel = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                            }
                            else if (tutorialBarrel == null && completed == false)
                            {
                                tutorialBarrel = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                                startTimer = false;
                            }
                        }
                        break;
                    case GameManager.Tutorial.SWIRL:
                        if (spawnedObject == false)
                        {
                            tutorialBarrel = (GameObject)Instantiate(tutorialPrefab3, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                            tutorialBarrel2 = (GameObject)Instantiate(tutorialPrefab3, (GameManager.instance.player.transform.position - new Vector3(-0.5f, 0, 1)), Quaternion.identity);
                            tutorialBarrel3 = (GameObject)Instantiate(tutorialPrefab3, (GameManager.instance.player.transform.position - new Vector3(1f, 0, 0)), Quaternion.identity);
                            tutorialBarrel4 = (GameObject)Instantiate(tutorialPrefab3, (GameManager.instance.player.transform.position - new Vector3(-1f, 0, 0)), Quaternion.identity);
                            tutorialBarrel5 = (GameObject)Instantiate(tutorialPrefab3, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, -1)), Quaternion.identity);
                            tutorialBarrel6 = (GameObject)Instantiate(tutorialPrefab3, (GameManager.instance.player.transform.position - new Vector3(-0.5f, 0, -1)), Quaternion.identity);
                            ObjectManagerV2.instance.canDamage = false;
                            GameManager.instance.player.GetComponent<SwipeHalf>().swirlEnded = true;
                            guideText.text = "Draw a circle rapidly on the right side to spin attack";

                            startTimer = false;
                            spawnedObject = true;
                        }
                        if (PlayerStates.swiped == true)
                        {
                            guideText.text = "Please draw a circle rapidly on the right side to spin attack";
                            startTimer = true;
                        }

                        if (startTimer == true)
                        {
                            timer += Time.deltaTime;
                            if (timer > 1f)
                            {
                                TutObj(tutorialBarrel, new Vector3(0.5f, 0, 1));
                                TutObj(tutorialBarrel2, new Vector3(-0.5f, 0, 1));
                                TutObj(tutorialBarrel3, new Vector3(1f, 0, 0));
                                TutObj(tutorialBarrel4, new Vector3(-1f, 0, 0));
                                TutObj(tutorialBarrel5, new Vector3(0.5f, 0, -1));
                                TutObj(tutorialBarrel6, new Vector3(-0.5f, 0, -1));
                                timer = 0;
                                startTimer = false;
                            }
                        }
                        if (GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut == true)
                        {
                            guideText.text = "Great job!";
                            ObjectManagerV2.instance.canDamage = true;
                            timer2 += Time.deltaTime;
                            if (timer2 > 1f)
                            {
                                GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut = false;
                                timer2 = 0;
                                GameManager.instance.tutorial = GameManager.Tutorial.STOMP;
                            }
                        }
                        break;
                    case GameManager.Tutorial.STOMP:
                        //Show the bar with animation
                        if (spawnedObject == true)
                        {
                            tutorialBarrel = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, 1)), Quaternion.identity);
                            tutorialBarrel2 = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(-0.5f, 0, 1)), Quaternion.identity);
                            tutorialBarrel3 = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(1f, 0, 0)), Quaternion.identity);
                            tutorialBarrel4 = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(-1f, 0, 0)), Quaternion.identity);
                            tutorialBarrel5 = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(0.5f, 0, -1)), Quaternion.identity);
                            tutorialBarrel6 = (GameObject)Instantiate(tutorialPrefab, (GameManager.instance.player.transform.position - new Vector3(-0.5f, 0, -1)), Quaternion.identity);
                            completed = false;
                            ObjectManagerV2.instance.canDamage = false;

                            rageMeter.SetActive(true);
                            GameManager.instance.player.GetComponent<PlayerStates>().rageObjects = 0;
                            GameManager.instance.player.GetComponent<StampBar>().slider.value = 1f;
                            GameManager.instance.player.GetComponent<SwipeHalf>().swirlEnded = true;

                            guideText.text = "You're enraged! Tap on both sides at the same time to stomp";

                            spawnedObject = false;
                        }

                        if (GameManager.instance.player.GetComponent<SwipeHalf>().stompTut == true && completed == false)
                        {
                            guideText.text = "Attack the crates in the air";
                            if (PlayerStates.swiped == true || GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut == true)
                            {
                                ObjectManagerV2.instance.canDamage = true;
                                guideText.text = "OHHHH YEAH YOU WIN - TUTORIAL IS OVER";
                                completed = true;
                                StartCoroutine(Delay());
                            }
                            else if (ObjectManagerV2.instance.isGrounded == true && completed == false)
                            {
                                guideText.text = "Try Again. Tap on both sides at the same time to stomp";
                                GameManager.instance.player.GetComponent<PlayerStates>().rageObjects = 0;
                                GameManager.instance.player.GetComponent<StampBar>().slider.value = 1f;
                                GameManager.instance.player.GetComponent<SwipeHalf>().stompTut = false;
                                ObjectManagerV2.instance.isGrounded = false;
                            }
                        }
                        else if (PlayerStates.swiped == true && completed == false)
                        {
                            startTimer = true;
                        }
                        else if (GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut == true && completed == false)
                        {
                            timer2 += Time.deltaTime;
                            if (timer2 > 1f)
                            {
                                TutObj(tutorialBarrel, new Vector3(0.5f, 0, 1));
                                TutObj(tutorialBarrel2, new Vector3(-0.5f, 0, 1));
                                TutObj(tutorialBarrel3, new Vector3(1f, 0, 0));
                                TutObj(tutorialBarrel4, new Vector3(-1f, 0, 0));
                                TutObj(tutorialBarrel5, new Vector3(0.5f, 0, -1));
                                TutObj(tutorialBarrel6, new Vector3(-0.5f, 0, -1));

                                timer2 = 0;
                                GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut = false;
                            }
                        }
                        if (startTimer == true && completed == false)
                        {
                            timer3 += Time.deltaTime;
                            if (timer3 > 1f)
                            {
                                TutObj(tutorialBarrel, new Vector3(0.5f, 0, 1));
                                TutObj(tutorialBarrel2, new Vector3(-0.5f, 0, 1));
                                TutObj(tutorialBarrel3, new Vector3(1f, 0, 0));
                                TutObj(tutorialBarrel4, new Vector3(-1f, 0, 0));
                                TutObj(tutorialBarrel5, new Vector3(0.5f, 0, -1));
                                TutObj(tutorialBarrel6, new Vector3(-0.5f, 0, -1));

                                timer3 = 0;
                                startTimer = false;
                            }
                        }
                        break;
                }
                break;
        }



    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.player.GetComponent<SwipeHalf>().stompTut = false;
        GameManager.instance.player.GetComponent<SwipeHalf>().swirlTut = false;
        GameManager.instance.tutorial = GameManager.Tutorial.DEFAULT;
        GameManager.instance.isInstructed = true;
        GameManager.instance.currentScene = GameManager.Scene.GAME;
    }

    public void TutObj(GameObject obj, Vector3 place)
    {
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.transform.position = GameManager.instance.player.transform.position - place;
        obj.transform.rotation = Quaternion.identity;
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


    public void CalculateCurrency()
    {
        GameManager.instance.currency = stars * currencyPerStar;
    }

    //show replay screen after animation is done
    private IEnumerator ShowContinueScreen(string levelResult)
    {
        yield return new WaitForSeconds(1f);

        // FOR AUDIO
        GameManager.instance.scoreScreenOpen();
        GameManager.instance.changeMusicState(AudioManager.IN_SCORE_SCREEN);  // FOR AUDIO

        if (GameManager.instance.currentLevel != 0)
        {
            Stars();
            
            if (GameManager.instance.stars != null)
            {
                if (stars > GameManager.instance.stars[GameManager.instance.currentLevel - 1])
                {
                    CalculateCurrency();
                    GameManager.instance.stars[GameManager.instance.currentLevel - 1] = stars;
                }
            }
            replayScoreText.text = "Score:\n" + "0" + " $"; //will be updated in counting loop
        }
        for(int i = 0; i < gems.Length; i++)
        {
            gems[i].SetActive(false);
        }
        for (int i = 0; i < stars; i++)
        {
            gems[i].SetActive(true);
        }
        InGamePanel.SetActive(false);
        ReplayPanel.SetActive(true);
        // NewLevelBtn.SetActive(true);


        Text levelNum = GameObject.Find("levelnumber").GetComponent<Text>();
        if (GameManager.instance.currentLevel < GameManager.instance.NUM_OF_LEVELS_IN_GAME)
        {
            levelNum.text = LanguageManager.instance.ReturnWord("CurrentLevel") + " " + (GameManager.instance.currentLevel).ToString();
        }
        else
        {
            levelNum.text = LanguageManager.instance.ReturnWord("CurrentLevel") + " " + GameManager.instance.currentLevel.ToString() + "*";
        }

        switch (levelResult)
        {
            case "Level completed!":
                GameManager.instance.levelWon = true;
                //ReplayBtn.SetActive(true);
                //NewLevelBtn.SetActive(true);

                if (GameManager.instance.levelsUnlocked < GameManager.instance.NUM_OF_LEVELS_IN_GAME && GameManager.instance.currentLevel == GameManager.instance.levelsUnlocked)
                {
                    GameManager.instance.levelsUnlocked++;

                }

                break;

            case "Game over":
                GameManager.instance.levelWon = false;
                //ReplayBtn.SetActive(true);
                NewLevelBtn.SetActive(false);
                //starText.text = stars.ToString();
                break;
        }

        GameManager.instance.Save();
        StartCoroutine(CountPointsTo(score)); // show counting score
    }

    //counting score "animation"
    IEnumerator CountPointsTo(int new_score)
    {
        if (new_score > 0)
        {
            yield return new WaitForSeconds(1f);
            GameManager.instance.startCountingPoints();
            int start = 0;
            float duration = Mathf.Clamp((float)new_score * (1f / 1f), 0f, 2f); //show with speed of 100 points per second, max duration 2 seconds
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
            {
                float progress = timer / duration;
                int temp_score = (int)Mathf.Lerp(start, new_score, progress);
                replayScoreText.text = "Score:\n" + temp_score + " $";
                GameManager.instance.audioManager.UpdatePointCounter(temp_score);
                yield return null;
            }
            GameManager.instance.audioManager.UpdatePointCounter(new_score);
            GameManager.instance.finishedCountingPoints();
        }
        replayScoreText.text = "Score:\n" + new_score + " $";

    }
}
