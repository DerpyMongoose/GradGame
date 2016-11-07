using UnityEngine;
using System.Collections;

/*
 * defines the destructable object properties, should be extended to create breakable, rollable...
 * */

public class Destructable : MonoBehaviour {

    //object name to be used for Quest system??
    public enum DestructableObject {
        BAREL, CHAIR, TABLE, WARDROBE, BED, BOX
    }

    public DestructableObject objType;
    public int pointsForDestruction;
}
