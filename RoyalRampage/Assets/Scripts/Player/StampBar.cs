using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StampBar : MonoBehaviour
{

    private float fillBar;
    private bool ready, gradually;
    private Color initialColor;
    private float countSecond;
    private ColorBlock sliderColors;
    private float timer;

    private Slider slider;
    private Image fillColor;
    public float reachScore, looseRageAfter, percentLoose, loosePerSecond;


    [HideInInspector]
    public static bool increaseFill;
    [HideInInspector]
    public float tempScore;
    [HideInInspector]
    public float timeToLowRage;


	void Awake(){
		slider = GameObject.FindGameObjectWithTag ("rage_slider").GetComponent<Slider>();
		fillColor = GameObject.FindGameObjectWithTag ("rage_fill").GetComponent<Image>();
	}

    void Start()
    {
		slider.value = 0f;
        fillBar = 0f;
        initialColor = fillColor.color;
        increaseFill = true;
        countSecond = 0f;
    }


    void Update()
    {

        if (increaseFill)
        {
            fillBar = ((tempScore / reachScore) * 10) / 10;
            slider.value = fillBar;
        }

        if (tempScore >= reachScore)
        {
            increaseFill = false;
            //timeToLowRage += Time.deltaTime;
            tempScore = reachScore;
            if (!ready)
            {
                //PhysicalMovement.ableToLift = true;
                SwipeHalf.ableToLift = true;
                ready = true;
            }
            fillColor.color = Color.red;
            //if (PhysicalMovement.intoAir)
            if (SwipeHalf.intoAir)
            {
                tempScore = 0f;
                fillBar = 0f;
                gradually = true;
                //slider.GetComponent<Image>().fillAmount = fillBar;
                fillColor.color = initialColor;
                //increaseFill = false;
                ready = false;
                //PhysicalMovement.intoAir = false;
                SwipeHalf.intoAir = false;
            }
        }

        if (gradually)
        {
            timer += Time.deltaTime;
            slider.value = Mathf.Lerp(1f, tempScore, timer * 2f);
            if (slider.value == tempScore)
            {
                timer = 0;
                gradually = false;
            }
        }


        //if (timeToLowRage > looseRageAfter)
        //{
        //    countSecond += 0.01f;
        //    if (countSecond >= loosePerSecond)
        //    {
        //        slider.GetComponent<Image>().fillAmount -= percentLoose / 100;
        //        //PhysicalMovement.ableToLift = false;
        //        SwipeHalf.ableToLift = false;
        //        ready = false;
        //        slider.GetComponent<Image>().color = initialColor;
        //        tempScore -= reachScore * (percentLoose / 100);
        //        countSecond = 0f;
        //    }
        //    if (slider.GetComponent<Image>().fillAmount == 0f)
        //    {
        //        timeToLowRage = 0f;
        //        countSecond = 0f;
        //    }
        //}
    }
}
