using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetLevelTextScript : MonoBehaviour
{

    public static SetLevelTextScript instance;
    private int thisInput;
    public string key = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void OnEnable()
    {
        LanguageManager.instance.ChangeText += changeText;
        if (GameManager.instance.currentLevel == 1)
        {
            key = "Tutorial";
            GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key);
        }
        else
        {
            GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key) + " " + thisInput.ToString();
        }
    }

    public void SetText(int input)
    {
        if (GameManager.instance.currentLevel == 1)
        {
            thisInput = input;
            GetComponent<Text>().text = LanguageManager.instance.ReturnWord(key) + " " + input.ToString();
        }
        thisInput = input;
        GetComponent<Text>().text = LanguageManager.instance.ReturnWord(key) + " " + input.ToString();
    }

    private void changeText()
    {
        GetComponentInChildren<Text>().text = LanguageManager.instance.ReturnWord(key) + " " + thisInput.ToString();
    }

    void OnDisable()
    {
        LanguageManager.instance.ChangeText -= changeText;
    }

}
