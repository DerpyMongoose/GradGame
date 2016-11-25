using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PointObjectPool : MonoBehaviour {

    //Singleton
    public static PointObjectPool instance;

    //List for the points (pool)
    public List<GameObject> points;

    //The Gameobject points
    public GameObject pointObject;

    //How many in the pool
    public int amountInPool;

    //Is the pool too small?
    bool willGrow = true;

    //Create Singleton
    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

	// Create pool on start
	void Start () {
        points = new List<GameObject>();

        for (int i = 0; i < amountInPool; i++) {
            GameObject obj = (GameObject)Instantiate(pointObject);
            obj.SetActive(false);
            points.Add(obj);
        }
	}
	
    //Get Object From pool
    public GameObject FindObjectInPool() {
        for (int i = 0; i < points.Count; i++) {
            if (!points[i].activeInHierarchy) {
                return points[i];
            }
        }

        if (willGrow) {
            for (int i = 0; i < 4; i++) {
                GameObject obj = (GameObject)Instantiate(pointObject);
                points.Add(obj);
            }
            GameObject lastObj = (GameObject)Instantiate(pointObject);
            points.Add(lastObj);
            return lastObj;
        }

        return null;
    }
}
