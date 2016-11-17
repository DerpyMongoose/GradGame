using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManagerV2 : MonoBehaviour
{
    public static ObjectManagerV2 instance;

    [Header("Forces")]
    public float oneToAnother;

    [Header("Mixed")]
    public float colImpact;

    [Header("Damages")]
    public int dashDamage;
    public int swirlDamage;
    public int wallDamage;
    public int objDamage;

    [Header("rubble after destruction")]
    public GameObject rubblePrefab;

    [HideInInspector]
    public Vector3 direction;
    //[HideInInspector]
    //public int maxScore = 0;

    void Awake()
    {
        instance = this;
    }

    [HideInInspector]
    public List<GameObject> objectList = new List<GameObject>();

    [Header("Glass")] //Barrel counts as wood material
    [Tooltip("The amount of points this object should award")]
    public int smallGlassScore;
    public int mediumGlassScore;
    public int largeGlassScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int smallGlassLife;
    public int mediumGlassLife;
    public int largeGlassLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int smallGlassRubbleAmount;
    public int mediumGlassRubbleAmount;
    public int largeGlassRubbleAmount;
    //   [Tooltip("The prefab that should spawn when this object is destroyed")]
    //   public GameObject barrelRubblePrefab;
    //[Tooltip("Write the Objects switch for sound")]
    //public string barrelSwitch;

    [Header("Wood")] // Bed counts as stone material
    [Tooltip("The amount of points this object should award")]
    public int smallWoodScore;
    public int mediumWoodScore;
    public int largeWoodScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int smallWoodLife;
    public int mediumWoodLife;
    public int largeWoodLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int smallWoodRubbleAmount;
    public int mediumWoodRubbleAmount;
    public int largeWoodRubbleAmount;
    //   [Tooltip("The prefab that should spawn when this object is destroyed")]
    //   public GameObject bedRubblePrefab;
    //[Tooltip("Write the Objects switch for sound")]
    //public string bedSwitch;

    [Header("Stone")] // Box counts as stone material
    [Tooltip("The amount of points this object should award")]
    public int smallStoneScore;
    public int mediumStoneScore;
    public int largeStoneScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int smallStoneLife;
    public int mediumStoneLife;
    public int largeStoneLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int smallStoneRubbleAmount;
    public int mediumStoneRubbleAmount;
    public int largeStoneRubbleAmount;
    //   [Tooltip("The prefab that should spawn when this object is destroyed")]
    //   public GameObject boxRubblePrefab;
    //[Tooltip("Write the Objects switch for sound")]
    //public string boxSwitch;

    [Header("Metal")] // Chair counts as glass material
    [Tooltip("The amount of points this object should award")]
    public int smallMetalScore;
    public int mediumMetalScore;
    public int largeMetalScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int smallMetalLife;
    public int mediumMetalLife;
    public int largeMetalLife;
    [Tooltip("The amount of rubble this object should spawn when destroyed")]
    public int smallMetalRubbleAmount;
    public int mediumMetalRubbleAmount;
    public int largeMetalRubbleAmount;
    //   [Tooltip("The prefab that should spawn when this object is destroyed")]
    //   public GameObject chairRubblePrefab;
    //[Tooltip("Write the Objects switch for sound")]
    //public string chairSwitch;


    void Start()
    {
        objectList.AddRange(GameObject.FindGameObjectsWithTag("Destructable"));
    }
}
