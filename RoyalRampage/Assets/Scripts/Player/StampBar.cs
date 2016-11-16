using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StampBar : MonoBehaviour
{

    private float fillBar;
    private bool ready;
    private Color initialColor;
    private float countSecond;

    public GameObject slider;
    public float reachScore, looseRageAfter, percentLoose, loosePerSecond;


    [HideInInspector]
    public static bool increaseFill;
    [HideInInspector]
    public float tempScore;
    [HideInInspector]
    public float timeToLowRage;



    void Start()
    {
        slider.GetComponent<Image>().fillAmount = 0f;
        initialColor = slider.GetComponent<Image>().color;
        increaseFill = true;
        countSecond = 0f;
    }


    void Update()
    {

        if (increaseFill)
        {
            fillBar = ((tempScore / reachScore) * 10) / 10;
            slider.GetComponent<Image>().fillAmount = fillBar;
        }

        if (tempScore >= reachScore)
        {
            increaseFill = false;
            timeToLowRage += Time.deltaTime;
            tempScore = reachScore;
            if (!ready)
            {
                PhysicalMovement.ableToLift = true;
                ready = true;
            }
            slider.GetComponent<Image>().color = Color.red;
            if (PhysicalMovement.intoAir)
            {
                tempScore = 0f;
                fillBar = 0f;
                slider.GetComponent<Image>().fillAmount = fillBar;
                slider.GetComponent<Image>().color = initialColor;
                increaseFill = false;
                ready = false;
                PhysicalMovement.intoAir = false;
            }
        }

        if (timeToLowRage > looseRageAfter)
        {
            countSecond += 0.01f;
            if (countSecond >= loosePerSecond)
            {
                slider.GetComponent<Image>().fillAmount -= percentLoose / 100;
                PhysicalMovement.ableToLift = false;
                ready = false;
                slider.GetComponent<Image>().color = initialColor;
                tempScore -= reachScore * (percentLoose / 100);
                countSecond = 0f;
            }
            if (slider.GetComponent<Image>().fillAmount == 0f)
            {
                timeToLowRage = 0f;
                countSecond = 0f;
            }
        }
    }
}
