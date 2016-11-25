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
    public float MultiplierTime;
    public int amountOfObjects;
    public Text MultiplierText;
    public int maxScore = 57;
    public int currencyPerStar = 50;
    [Range(0, 1)]
    public float star1;
    [Range(0, 1)]
    public float star2;
    [Range(0, 1)]
    public float star3;
    [Range(0, 1)]
    public float star4;
    [Range(0, 1)]
    public float star5;

    public Vector3 playerPos;
    [HideInInspector]
    public bool targetReached = false;

    int stars = 0;
    int score = 0;
    Text starText;
    Text scoreText;
    Text minScoreText;
    public Text guideText;
    GameObject InGamePanel;
    GameObject ReplayPanel;
    Text replayScoreText;
    GameObject ReplayBtn;
    GameObject NewLevelBtn;
    GameObject IntroTapPanel;
    private GameObject continueButton;
    public int multiplier;
    private float countMultiTime;
    private int countObjects;

    

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
        playerPos = GameManager.instance.player.transform.position;
        multiplier = 1;
        countMultiTime = 0;
        amountOfObjects = 5;
        MultiplierTime = 5;
        switch (GameManager.instance.CurrentScene())
        {
            case GameManager.Scene.GAME:
                scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
                scoreText.text = score.ToString(); // in game score
                minScoreText = GameObject.Find("MinScoreText").GetComponent<Text>();
                minScoreText.text = "Reach " + scoreToCompleteLevel;
                guideText = GameObject.FindGameObjectWithTag("GuideText").GetComponent<Text>();
                guideText.text = "Swipe and destroy objects";

                ReplayPanel = GameObject.FindGameObjectWithTag("ReplayPanel");
                InGamePanel = GameObject.FindGameObjectWithTag("InGamePanel");
                IntroTapPanel = GameObject.FindGameObjectWithTag("IntroTapPanel");
                ReplayBtn = GameObject.Find("ReplayButton");
                NewLevelBtn = GameObject.Find("NewLevelButton");
                continueButton = GameObject.Find("ContinueButton");
                starText = GameObject.Find("stars").GetComponent<Text>();
                replayScoreText = GameObject.FindGameObjectWithTag("GOscore").GetComponent<Text>();
                ReplayPanel.SetActive(false);
                continueButton.SetActive(false);
                InGamePanel.SetActive(false);
                break;
            case GameManager.Scene.TUTORIAL:
                guideText = GameObject.FindGameObjectWithTag("GuideText").GetComponent<Text>();
                guideText.text = "Swipe the left side of the screen to move";
                ReplayPanel = GameObject.FindGameObjectWithTag("ReplayPanel");
                InGamePanel = GameObject.FindGameObjectWithTag("InGamePanel");
                IntroTapPanel = GameObject.FindGameObjectWithTag("IntroTapPanel");
                ReplayBtn = GameObject.Find("ReplayButton");
                NewLevelBtn = GameObject.Find("NewLevelButton");
                continueButton = GameObject.Find("ContinueButton");
                starText = GameObject.Find("stars").GetComponent<Text>();
                replayScoreText = GameObject.FindGameObjectWithTag("GOscore").GetComponent<Text>();
                ReplayPanel.SetActive(false);
                continueButton.SetActive(false);
                InGamePanel.SetActive(false);
                break;
                // GameManager.instance.levelLoad(); // FOR AUDIO
        }
    }

    private void IncreaseScore(GameObject destructedObj)
    {
        if (GameManager.instance.canPlayerDestroy)
        {
            int points = destructedObj.GetComponent<ObjectBehavior>().score;
            countObjects++;
            scoreText.text = score.ToString(); // in game score
            countMultiTime = 0;
            if (countObjects == amountOfObjects)
            {
                //Timer shouldn't change during combo.
                multiplier++;
                countObjects = 0;
            }
            score += points * multiplier;
            scoreText.text = score.ToString(); // in game score
            GameManager.instance.score = score;
            GameManager.instance.player.GetComponent<StampBar>().tempScore += points;
            StampBar.increaseFill = true;
            GameManager.instance.player.GetComponent<StampBar>().timeToLowRage = 0f;
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
        GameManager.instance.levelUnLoad(); // FOR AUDIO

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
        print(GameManager.instance.CurrentScene());
        print(GameManager.instance.TutorialState());
        switch (GameManager.instance.CurrentScene())
        {
            case GameManager.Scene.GAME:
                countMultiTime += Time.deltaTime;
                //print(countMultiTime);
                if (countMultiTime > MultiplierTime)
                {
                    // print("I am in");
                    multiplier = 1;
                    countObjects = 0;
                }

                if (score >= scoreToCompleteLevel)
                {
                    continueButton.SetActive(true);
                }
                else continueButton.SetActive(false);

                MultiplierText.text = "x" + multiplier.ToString();

                //print(GameManager.instance.allStars);
                break;
            case GameManager.Scene.TUTORIAL:
                switch (GameManager.instance.TutorialState())
                {
                    case GameManager.Tutorial.MOVEMENT:
                        if(SwipeHalf.startTutTimer == true)
                        {
                            guideText.text = "Reach the finish-line before the timer runs out!";
                        }
                        if(GameManager.instance.levelManager.targetReached == true)
                        {
                            guideText.text = "Great job!";
                        }
                        break;
                    case GameManager.Tutorial.ATTACk:

                        guideText.text = "Swipe the right side of the screen to attack";
                        break;
                    case GameManager.Tutorial.CHAIN:

                        guideText.text = "placeholder";
                        break;
                    case GameManager.Tutorial.SWIRL:

                        guideText.text = "Make a circle on the right side of the screen to do a swirl attack";
                        break;
                    case GameManager.Tutorial.STOMP:

                        guideText.text = "Double tap ";
                        break;
                }
                break;
        }



    }

    public void Continue()
    {
        guideText.text = "Level completed!";
        guideText.gameObject.SetActive(true);
        StartCoroutine(ShowContinueScreen(guideText.text));
    }
    public void Stars()
    {
        if (score / maxScore > star1 && score / maxScore < star2)
        {
            stars = 1;
        }
        if (score / maxScore >= star2 && score / maxScore < star3)
        {
            stars = 2;
        }
        if (score / maxScore >= star3 && score / maxScore < star4)
        {
            stars = 3;
        }
        if (score / maxScore >= star4 && score / maxScore < star5)
        {
            stars = 4;
        }
        if (score / maxScore >= star5)
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

        Stars();
        CalculateCurrency();

        InGamePanel.SetActive(false);
        replayScoreText.text = "Score: " + "0"; //will be updated in counting loop

        starText.text = stars.ToString();

        ReplayPanel.SetActive(true);

        if (GameManager.instance.stars != null)
        {
            if (stars > GameManager.instance.stars[GameManager.instance.currentLevel - 1])
            {
                GameManager.instance.allStars -= GameManager.instance.stars[GameManager.instance.currentLevel - 1];
                GameManager.instance.allStars += stars;
            }
        }
        switch (levelResult)
        {
            case "Level completed!":
                GameManager.instance.levelWon = true;
                ReplayBtn.SetActive(false);
                NewLevelBtn.SetActive(true);
                Text levelNum = NewLevelBtn.GetComponentInChildren<Text>();
                if (GameManager.instance.levelsUnlocked < GameManager.instance.NUM_OF_LEVELS_IN_GAME && GameManager.instance.currentLevel == GameManager.instance.levelsUnlocked)
                {
                    GameManager.instance.levelsUnlocked++;

                }
                if (GameManager.instance.currentLevel < GameManager.instance.NUM_OF_LEVELS_IN_GAME)
                {
                    levelNum.text = (GameManager.instance.currentLevel + 1).ToString();
                }
                else
                {
                    levelNum.text = GameManager.instance.currentLevel.ToString() + "*";
                }

                break;

            case "Game over":
                GameManager.instance.levelWon = false;
                ReplayBtn.SetActive(true);
                NewLevelBtn.SetActive(false);
                starText.text = stars.ToString();
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
            float duration = 2f; //(float)new_score * (1f / 100f); //show with speed of 100 points per second
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
            {
                float progress = timer / duration;
                int temp_score = (int)Mathf.Lerp(start, new_score, progress);
                replayScoreText.text = "Score: " + temp_score;
                GameManager.instance.audioManager.UpdatePointCounter(temp_score);
                yield return null;
            }
        }
        replayScoreText.text = "Score: " + new_score;
        GameManager.instance.audioManager.UpdatePointCounter(new_score);
        GameManager.instance.finishedCountingPoints();
    }
}
