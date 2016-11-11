using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StampBar : MonoBehaviour {

    private float fillBar;
    private Color initialColor;
    //private float fillAmount;

    public GameObject slider;
    

    [HideInInspector]
    public bool increaseFill;
    [HideInInspector]
    public float tempScore;



    void Start () {
        slider.GetComponent<Image>().fillAmount = 0;
        initialColor = slider.GetComponent<Image>().color;
    }
	

	void Update () {
	
        if(increaseFill)
        {
            //print(GameManager.instance.score);
            fillBar = ((tempScore / 30f) * 10)/10;
            //print(fillBar);
            slider.GetComponent<Image>().fillAmount = fillBar;
        }

        if(tempScore >= 30f)
        {
            GameManager.instance.player.GetComponent<PhysicalMovement>().ableToLift = true;
            slider.GetComponent<Image>().color = Color.red;
            if(GameManager.instance.player.GetComponent<PlayerStates>().lifted)
            {
                tempScore = 0f;
                fillBar = 0f;
                slider.GetComponent<Image>().fillAmount = fillBar;
                slider.GetComponent<Image>().color = initialColor;
                increaseFill = false;
            }
        }

        if (slider.GetComponent<Image>().fillAmount == 0f)
        {
            //print("came");
            increaseFill = true;
        }
    }
}
