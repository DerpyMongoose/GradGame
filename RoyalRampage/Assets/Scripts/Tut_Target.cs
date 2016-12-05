using UnityEngine;
using System.Collections;

public class Tut_Target : MonoBehaviour
{
    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.transform.tag == "Player")
        {
            GameManager.instance.levelManager.targetReached = true;
        }
    }
}
