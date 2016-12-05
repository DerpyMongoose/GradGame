using UnityEngine;
using System.Collections;

public class ObjectDetection : MonoBehaviour {

    //Rigidbody playerRig;
    //float initialMass;

    //void Start()
    //{
    //    playerRig = GetComponent<Rigidbody>();
    //    initialMass = playerRig.mass;
    //}

    //void OnCollisionEnter(Collision col)
    //{
    //    if(col.gameObject.GetComponent<ObjectBehavior>() != null || col.gameObject.GetComponent<FracturedChunk>() != null)
    //    {
    //        if (col.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
    //        {
    //            playerRig.mass = GetComponent<PlayerStates>().becomeHeavy;
    //            //StartCoroutine(ReverseMass());
    //        }
    //    }
    //}

    //void OnCollisionStay(Collision col)
    //{
    //    if (col.gameObject.GetComponent<ObjectBehavior>() != null || col.gameObject.GetComponent<FracturedChunk>() != null)
    //    {
    //        if (col.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
    //        {
    //            playerRig.mass = GetComponent<PlayerStates>().becomeHeavy;
    //            //StartCoroutine(ReverseMass());
    //        }
    //    }
    //}

    //void OnCollisionExit(Collision col)
    //{
    //    if (col.gameObject.GetComponent<ObjectBehavior>() != null || col.gameObject.GetComponent<FracturedChunk>() != null)
    //    {
    //        if(playerRig.mass != initialMass)
    //        {
    //            playerRig.mass = initialMass;
    //        }
    //    }
    //}

    //IEnumerator ReverseMass()
    //{
    //    yield return new WaitForSeconds(Time.deltaTime);
    //    playerRig.mass = initialMass;
    //}

}
