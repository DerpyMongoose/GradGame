using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProceduralObjectives : MonoBehaviour {

    private GameObject guideText, largeObjs, mediumObjs, smallObjs;
    private Text text;
    private int amountLargeObj, amountMediumObj, amountSmallObj, killerChoice, victimChoice;

    [HideInInspector]
    public bool finishedGuide, completeObjective;
    [HideInInspector]
    public GameObject killerObj, victimObj;


	void Awake () {
        guideText = GameObject.FindGameObjectWithTag("GuideText");
        largeObjs = GameObject.FindGameObjectWithTag("LargeObjects");
        mediumObjs = GameObject.FindGameObjectWithTag("MediumObjects");
        smallObjs = GameObject.FindGameObjectWithTag("SmallObjects");
        text = guideText.GetComponent<Text>();
        finishedGuide = false;
    }
	

	void Update () {
        amountLargeObj = largeObjs.transform.childCount;
        amountMediumObj = mediumObjs.transform.childCount;
        amountSmallObj = smallObjs.transform.childCount;
        for(int i=0; i<amountLargeObj; i++)
        {
            if(largeObjs.transform.GetChild(i).childCount == 0)
            {
                Destroy(largeObjs.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < amountMediumObj; i++)
        {
            if (mediumObjs.transform.GetChild(i).childCount == 0)
            {
                Destroy(mediumObjs.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < amountSmallObj; i++)
        {
            if (smallObjs.transform.GetChild(i).childCount == 0)
            {
                Destroy(smallObjs.transform.GetChild(i).gameObject);
            }
        }

        if (finishedGuide)
        {
            ChooseType();
            text.fontSize = text.fontSize / 2;
			text.text = "Destroy " + victimObj.name.ToString().Replace("_", " ") + " with " + killerObj.name.ToString().Replace("_", " ");
            finishedGuide = false;
			//GameManager.instance.announcedObjective ();
        }
        
        if(completeObjective)
        {
			//GameManager.instance.completedObjective ();
			ChooseType();
			text.text = "Destroy " + victimObj.name.ToString().Replace("_", " ")  + " with " + killerObj.name.ToString().Replace("_", " ");
            completeObjective = false;
			//GameManager.instance.announcedObjective ();
        }

    }

    void ChooseType()
    {
        killerChoice = Random.Range(0, 1);
        victimChoice = Random.Range(0, 2);

        if(killerChoice == 0)
        {
            killerObj = largeObjs.transform.GetChild(Random.Range(0, amountLargeObj)).gameObject;
        }
        else
        {
            killerObj = mediumObjs.transform.GetChild(Random.Range(0, amountMediumObj)).gameObject;
        }

        if(victimChoice == 0)
        {
            victimObj = largeObjs.transform.GetChild(Random.Range(0, amountLargeObj)).gameObject;
        }
        else if(victimChoice == 1)
        {
            victimObj = mediumObjs.transform.GetChild(Random.Range(0, amountMediumObj)).gameObject;
        }
        else
        {
            victimObj = smallObjs.transform.GetChild(Random.Range(0, amountSmallObj)).gameObject;
        }
    }

}
