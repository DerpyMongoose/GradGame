using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetTextPoints : MonoBehaviour {

    float counter;
    public float timeActive = 1f;
    public Color normalPointColor, bonusPointColor;

    void OnEnable() {
        counter = 0f;
    }
	
    public void SetText(int pointAmount)
    {
        GetComponentInChildren<Text>().color = normalPointColor;
        GetComponentInChildren<Text>().text = "+" + pointAmount.ToString();
    }

    public void SetBonusText(int pointAmount)
    {
        GetComponentInChildren<Text>().color = bonusPointColor;
        GetComponentInChildren<Text>().text = "+" + pointAmount.ToString();
    }

    void Update() {
        counter += Time.deltaTime;

        if(counter > timeActive) {
            gameObject.SetActive(false);
        }

        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, Time.deltaTime);
        
    }
}
