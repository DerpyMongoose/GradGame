using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    void Awake()
    {
        instance = this;
    }

    [HideInInspector]
    public List<GameObject> objectList = new List<GameObject>();

    [Header("Barrel")]
    [Tooltip("The amount of points this object should award")]
    public int barrelScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int barrelLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int barrelRubbleAmount;
    [Tooltip("The prefab that should spawn when this object is destroyed")]
    public GameObject barrelRubblePrefab;

    [Header("Bed")]
    [Tooltip("The amount of points this object should award")]
    public int bedScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int bedLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int bedRubbleAmount;
    [Tooltip("The prefab that should spawn when this object is destroyed")]
    public GameObject bedRubblePrefab;

    [Header("Box")]
    [Tooltip("The amount of points this object should award")]
    public int boxScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int boxLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int boxRubbleAmount;
    [Tooltip("The prefab that should spawn when this object is destroyed")]
    public GameObject boxRubblePrefab;

    [Header("Chair")]
    [Tooltip("The amount of points this object should award")]
    public int chairScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int chairLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int chairRubbleAmount;
    [Tooltip("The prefab that should spawn when this object is destroyed")]
    public GameObject chairRubblePrefab;

    [Header("Table")]
    [Tooltip("The amount of points this object should award")]
    public int tableScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int tableLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int tableRubbleAmount;
    [Tooltip("The prefab that should spawn when this object is destroyed")]
    public GameObject tableRubblePrefab;

    [Header("Wardrobe")]
    [Tooltip("The amount of points this object should award")]
    public int wardrobeScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int wardrobeLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int wardrobeRubbleAmount;
    [Tooltip("The prefab that should spawn when this object is destroyed")]
    public GameObject wardrobeRubblePrefab;


    // Use this for initialization
    void Start()
    {
        objectList.AddRange(GameObject.FindGameObjectsWithTag("Destructable"));
    }
}
