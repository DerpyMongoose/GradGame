using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour {

    public static StoreManager _instance;

    public List<StoreObject> objectsInStore = new List<StoreObject>();

    void Awake() {
        if(_instance == null) {
            _instance = this;
        }
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BuyItem () {
        GameManager.instance.currency -= 10;
    }
}
