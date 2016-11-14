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

    int score = 0;
    Text scoreText;
    Text minScoreText;
    Text guideText;
    GameObject InGamePanel;
    GameObject ReplayPanel;
    Text replayScoreText;
    GameObject ReplayBtn;
    GameObject NewLevelBtn;
    private GameObject continueButton;
    private int multiplier;
    private float countMultiTime;
    private int countObjects;

    void OnEnable()
    {
        GameManager.instance.OnObjectDestructed += IncreaseScore;
        GameManager.instance.OnTimerStart += StartLevel;
        GameManager.instance.OnTimerOut += ShowEnding;
    }

    void OnDisable()
    {
        GameManager.instance.OnObjectDestructed -= IncreaseScore;
        GameManager.instance.OnTimerStart -= StartLevel;
        GameManager.instance.OnTimerOut -= ShowEnding;
    }

    void Start(){
        multiplier = 1;
        countMultiTime = 0;
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		scoreText.text = "Score: " + "$" + score;
        minScoreText = GameObject.Find("MinScoreText").GetComponent<Text>();
        minScoreText.text = "Reach " + scoreToCompleteLevel + " to Win";
        guideText = GameObject.Find("GuideText").GetComponent<Text>();
        guideText.text = "Swipe and destroy objects";

        ReplayPanel = GameObject.Find("replayPanel");
        InGamePanel = GameObject.Find("InGameGUI");
        ReplayBtn = GameObject.Find("ReplayButton");
        NewLevelBtn = GameObject.Find("NewLevelButton");
        continueButton = GameObject.Find("ContinueButton");
        replayScoreText = GameObject.FindGameObjectWithTag("GOscore").GetComponent<Text>();
        ReplayPanel.SetActive(false);
        continueButton.SetActive(false);
        GameManager.instance.levelLoad(); // FOR AUDIO
    }

    private void IncreaseScore(GameObject destructedObj)
    {
        if (GameManager.instance.canPlayerDestroy)
        {
            int points = destructedObj.GetComponent<ObjectBehavior>().score;
            countObjects++;
		scoreText.text = "Score: " + "$" + score;
            countMultiTime = 0;
            if(countObjects == amountOfObjects)
            {
                multiplier++;
                countObjects = 0;
            }
            score += points * multiplier;
            scoreText.text = "Score: " + "$" + score;
            GameManager.instance.score = score;
            GameManager.instance.player.GetComponent<StampBar>().tempScore += points;
            GameManager.instance.player.GetComponent<StampBar>().increaseFill = true;
            GameManager.instance.player.GetComponent<StampBar>().timeToLowRage = 0f;
        }
    }

    private void StartLevel()
    {
        //print ("started");
        guideText.gameObject.SetActive(false);
    }

    //after the timer is out (wait for animation?)
    private void ShowEnding()
    {

       // print("ended");
        GameManager.instance.levelUnLoad(); // FOR AUDIO

        if (score >= scoreToCompleteLevel || ObjectManager.instance.objectList.Count <= 1)
            guideText.text = "Level completed!";
        else
            guideText.text = "Game over";

        guideText.gameObject.SetActive(true);
        StartCoroutine(ShowContinueScreen(guideText.text));
    }

    void Update()
    {
        countMultiTime += Time.deltaTime;
        //print(countMultiTime);
        if(countMultiTime > MultiplierTime)
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

        MultiplierText.text = multiplier.ToString() + "x";
    }

    public void Continue()
    {
        guideText.text = "Level completed!";
        guideText.gameObject.SetActive(true);
        StartCoroutine(ShowContinueScreen(guideText.text));
    }

    //show replay screen after animation is done
    private IEnumerator ShowContinueScreen(string levelResult)
    {
        yield return new WaitForSeconds(0.5f);

        // FOR AUDIO
        GameManager.instance.scoreScreenOpen();

        InGamePanel.SetActive(false);
        replayScoreText.text = "Score: " + "$" + score;

        ReplayPanel.SetActive(true);
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
                    GameManager.instance.Save();
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
                break;
        }
    }
}
