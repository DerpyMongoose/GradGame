using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StampBar : MonoBehaviour
{
    [HideInInspector]
    public float fillBar;
    [HideInInspector]
    public bool ready;
    private Color initialColor;
    private float timer;

    [HideInInspector]
    public Slider slider;
    private Image fillColor;


    [HideInInspector]
    public static bool increaseFill;
    [HideInInspector]
    public float tempScore;
    [HideInInspector]
    public bool gradually;


	void Awake(){
		slider = GameObject.FindGameObjectWithTag ("rage_slider").GetComponent<Slider>();
		fillColor = GameObject.FindGameObjectWithTag ("rage_fill").GetComponent<Image>();
	}
    void Start()
    {
        if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
        {
            slider.value = 0f;
            fillBar = 0f;
            initialColor = fillColor.color;
            increaseFill = true;
            tempScore = 0f;
            ready = true;
            gradually = false;
            timer = 0f;
        }
    }


    void Update()
    {
        if (increaseFill && GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
        {
            fillBar = ((tempScore / GetComponent<PlayerStates>().rageObjects) * 10) / 10;
            slider.value = fillBar;
        }

        if (tempScore >= GetComponent<PlayerStates>().rageObjects)
        {
            increaseFill = false;
            tempScore = GetComponent<PlayerStates>().rageObjects;
            if (ready == true)
            {
                SwipeHalf.ableToLift = true;
                ready = false;
            }
            fillColor.color = Color.red;
            if (SwipeHalf.intoAir)
            {
                if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME)
                {
                    tempScore = 0f;
                    fillBar = 0f;
                    gradually = true;
                    fillColor.color = initialColor;
                }
                ready = true;
                SwipeHalf.intoAir = false;
            }
        }

        if (gradually)
        {
            timer += Time.deltaTime;
            slider.value = Mathf.Lerp(1f, 0f, timer * 2f);
            if (slider.value == 0f)
            {
                timer = 0;
                increaseFill = true;
                gradually = false;
            }
        }
    }
}

