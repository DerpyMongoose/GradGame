using UnityEngine;
using System.Collections;

public class ObjectDetection : MonoBehaviour {

    Rigidbody playerRig;
    float initialMass;

    void Start()
    {
        obj = false;
        playerRig = GetComponent<Rigidbody>();
        initialMass = playerRig.mass;

    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.GetComponent<ObjectBehavior>() != null)
        {
            if (col.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 5f)
            {
                playerRig.mass = GetComponent<PlayerStates>().becomeHeavy;
                StartCoroutine(ReverseMass());
            }
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.GetComponent<ObjectBehavior>() != null)
        {
            if (col.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 5f)
            {
                playerRig.mass = GetComponent<PlayerStates>().becomeHeavy;
                StartCoroutine(ReverseMass());
            }
        }
    }

    IEnumerator ReverseMass()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        playerRig.mass = initialMass;
    }

}
