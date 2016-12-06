using UnityEngine;
using System.Collections;

public class ObjectDetection : MonoBehaviour {

    Rigidbody playerRig;

    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "UniqueObjs")
        {
            playerRig.velocity = Vector3.zero;
        }
    }


}
