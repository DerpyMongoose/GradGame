using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour
{
    public enum type
    {
        BARREL, BED, BOX
    }

    public type type_;

    public List<DestructableObject> Objects = new List<DestructableObject>(0);

    void Start()
    {

    }
}

