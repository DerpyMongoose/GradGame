using System;
using UnityEngine;

[Serializable]
public class DestructableObject
{
    public enum type { BARREL, BED, BOX, CHAIR, TABLE, WARDROBE }

    public type type_;
    public int score;
    public int life;
    public int rubbleAmount;
    public GameObject rubblePrefab;
}

