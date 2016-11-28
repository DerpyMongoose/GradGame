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
    //[Tooltip("The amount of points this object should award")]
    //public int smallGlassScore;
    //public int mediumGlassScore;
    //public int largeGlassScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int smallGlassLife;
    public int mediumGlassLife;
    public int largeGlassLife;

    [Header("Wood")] // Bed counts as stone material
    //[Tooltip("The amount of points this object should award")]
    //public int smallWoodScore;
    //public int mediumWoodScore;
    //public int largeWoodScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int smallWoodLife;
    public int mediumWoodLife;
    public int largeWoodLife;

    [Header("Stone")] // Box counts as stone material
    //[Tooltip("The amount of points this object should award")]
    //public int smallStoneScore;
    //public int mediumStoneScore;
    //public int largeStoneScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int smallStoneLife;
    public int mediumStoneLife;
    public int largeStoneLife;

    [Header("Metal")] // Chair counts as glass material
    //[Tooltip("The amount of points this object should award")]
    //public int smallMetalScore;
    //public int mediumMetalScore;
    //public int largeMetalScore;
    [Tooltip("The amount of times you must hit this object to destroy it")]
    public int smallMetalLife;
    public int mediumMetalLife;
    public int largeMetalLife;


    void Start()
    {
        objectList.AddRange(GameObject.FindGameObjectsWithTag("Destructable"));
    }
}
