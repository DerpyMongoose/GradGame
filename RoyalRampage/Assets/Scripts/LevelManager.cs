using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 level and score manager
 Used for public variables to be tweaked by level designer 
 */
public class LevelManager : MonoBehaviour {

    [SerializeField]
    int scoreToCompleteLevel = 10;
    public int timeToCompleteLevel = 10;
    private int amountOfObjects;
    private Text MultiplierText;
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

    float t;
    int stars = 0;
    int score = 0;
    Text starText;
    Text scoreText;
    Text minScoreText;
    Text guideText;
    GameObject InGamePanel;
    GameObject ReplayPanel;
    Text replayScoreText;
    GameObject ReplayBtn;
    GameObject NewLevelBtn;
    GameObject IntroTapPanel;
    private GameObject continueButton;
    public int multiplier;
    int tempMulti;

    void OnEnable() {
        GameManager.instance.OnObjectDestructed += IncreaseScore;
        GameManager.instance.OnLevelLoad += StartLevel;
        GameManager.instance.OnTimerOut += ShowEnding;
    }

    void OnDisable() {
        GameManager.instance.OnObjectDestructed -= IncreaseScore;
        GameManager.instance.OnLevelLoad -= StartLevel;
        GameManager.instance.OnTimerOut -= ShowEnding;
    }

    void Start() {
		MultiplierText = GameObject.Find ("Multiplier").GetComponent<Text> ();
        multiplier = 1;
        ObjectManagerV2.instance.countMultiTime = 0;
        tempMulti = 1;
        amountOfObjects = 1;
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        scoreText.text = score.ToString(); // in game score
        minScoreText = GameObject.Find("MinScoreText").GetComponent<Text>();
        minScoreText.text = "Reach " + scoreToCompleteLevel;
        SetLevelTextScript.instance.SetText(GameManager.instance.currentLevel);
        SetReachGoalScript.instance.SetText(scoreToCompleteLevel);
        guideText = GameObject.Find("GuideText").GetComponent<Text>();
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
        // GameManager.instance.levelLoad(); // FOR AUDIO
        //print("level set up");
    }

    private void IncreaseScore(GameObject destructedObj) {
        if (GameManager.instance.canPlayerDestroy) {
            int points = destructedObj.GetComponent<ObjectBehavior>().score;
            scoreText.text = score.ToString(); // in game score
            while(amountOfObjects <= ObjectManagerV2.instance.countObjects)
            {
                //Timer shouldn't change during combo.
                ObjectManagerV2.instance.countObjects = Mathf.Abs(amountOfObjects-ObjectManagerV2.instance.countObjects);
                multiplier++;
                amountOfObjects++;
            }
            score += points * multiplier;
            scoreText.text = score.ToString(); // in game score
            GameManager.instance.score = score;
            GameManager.instance.player.GetComponent<StampBar>().tempScore += points;
            StampBar.increaseFill = true;
            //GameManager.instance.player.GetComponent<StampBar>().timeToLowRage = 0f;
        }
    }

    private void StartLevel() {
        //print ("started");
        //guideText.gameObject.SetActive(false);
        IntroTapPanel.SetActive(false);
        InGamePanel.SetActive(true);
        guideText.text = "";
    }

    //after the timer is out (wait for animation?)
    private void ShowEnding() {
        // print("ended");
       // GameManager.instance.levelUnLoad(); // FOR AUDIO

        if (score >= scoreToCompleteLevel) {
            guideText.text = "Level completed!";
        } else {
            guideText.text = "Game over";
        }

        guideText.gameObject.SetActive(true);
        StartCoroutine(ShowContinueScreen(guideText.text));
    }

    void Update() {
        //print(ObjectManagerV2.instance.countObjects);

        ObjectManagerV2.instance.countMultiTime += Time.deltaTime;
        //print(countMultiTime);
        if(ObjectManagerV2.instance.countMultiTime > ObjectManagerV2.instance.multiplierTimer){
            // print("I am in");
            MultiplierText.transform.localScale = new Vector3(1f, 1f, 1f);
            multiplier = 1;
            amountOfObjects = 1;
            ObjectManagerV2.instance.countObjects = 0;
            tempMulti = 1;
            t = 0f;
            ObjectManagerV2.instance.countMultiTime = 0;
        }

        if (score >= scoreToCompleteLevel) {
            continueButton.SetActive(true);
        } else continueButton.SetActive(false);

        if (ObjectManagerV2.instance.countMultiTime < ObjectManagerV2.instance.multiplierTimer) {
            t += Time.deltaTime / ObjectManagerV2.instance.multiplierTimer;
            MultiplierText.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0.5f, 0.5f, 0.5f), t);
        }

        if (multiplier != tempMulti && multiplier != 1) {
            MultiplierText.transform.localScale = new Vector3(1f, 1f, 1f);
            tempMulti = multiplier;
            t = 0f;
        }

        if (multiplier == 1) {
            MultiplierText.text = "";
        } else {
            MultiplierText.text = "x" + multiplier.ToString();
        }

        //print(GameManager.instance.allStars);
    }


    public void Continue() {
        guideText.text = "Level completed!";
        guideText.gameObject.SetActive(true);
        StartCoroutine(ShowContinueScreen(guideText.text));
    }
    public void Stars() {
        if (score / maxScore > star1 && score / maxScore < star2) {
            stars = 1;
        }
        if (score / maxScore >= star2 && score / maxScore < star3) {
            stars = 2;
        }
        if (score / maxScore >= star3 && score / maxScore < star4) {
            stars = 3;
        }
        if (score / maxScore >= star4 && score / maxScore < star5) {
            stars = 4;
        }
        if (score / maxScore >= star5) {
            stars = 5;
        }
    }

    public void CalculateCurrency() {
        GameManager.instance.currency = stars * currencyPerStar;
    }

    //show replay screen after animation is done
    private IEnumerator ShowContinueScreen(string levelResult) {
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

        if (GameManager.instance.stars != null) {
            if (stars > GameManager.instance.stars[GameManager.instance.currentLevel - 1]) {
                GameManager.instance.allStars -= GameManager.instance.stars[GameManager.instance.currentLevel - 1];
                GameManager.instance.allStars += stars;
            }
        }
        switch (levelResult) {
            case "Level completed!":
            GameManager.instance.levelWon = true;
            ReplayBtn.SetActive(false);
            NewLevelBtn.SetActive(true);
            Text levelNum = NewLevelBtn.GetComponentInChildren<Text>();
            if (GameManager.instance.levelsUnlocked < GameManager.instance.NUM_OF_LEVELS_IN_GAME && GameManager.instance.currentLevel == GameManager.instance.levelsUnlocked) {
                GameManager.instance.levelsUnlocked++;

            }
            if (GameManager.instance.currentLevel < GameManager.instance.NUM_OF_LEVELS_IN_GAME) {
                levelNum.text = (GameManager.instance.currentLevel + 1).ToString();
            } else {
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
    IEnumerator CountPointsTo(int new_score) {
        if (new_score > 0) {
            yield return new WaitForSeconds(1f);
            GameManager.instance.startCountingPoints();
            int start = 0;
			float duration = Mathf.Clamp((float)new_score * (1f / 1f),0f,2f); //show with speed of 100 points per second, max duration 2 seconds
            for (float timer = 0; timer < duration; timer += Time.deltaTime) {
                float progress = timer / duration;
                int temp_score = (int)Mathf.Lerp(start, new_score, progress);
                replayScoreText.text = "Score: " + temp_score;
                GameManager.instance.audioManager.UpdatePointCounter(temp_score);
                yield return null;
            }
			GameManager.instance.audioManager.UpdatePointCounter(new_score);
			GameManager.instance.finishedCountingPoints();
        }
        replayScoreText.text = "Score: " + new_score;
        
    }
}
