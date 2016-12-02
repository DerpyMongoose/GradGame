using UnityEngine;
using System.Collections;

public class ObjectDetection : MonoBehaviour {

    bool prevent;
    bool obj;
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
        if(col.gameObject.GetComponent<ObjectBehavior>() != null /*|| col.gameObject.GetComponent<FracturedChunk>() != null*/)
        {
            //if (col.gameObject.GetComponent<ObjectBehavior>() != null)
            //{
                obj = col.gameObject.GetComponent<ObjectBehavior>().hit;
            //}

            if (obj)
            {
                playerRig.mass = GetComponent<PlayerStates>().becomeHeavy;
                StartCoroutine(ReverseMass());
                //print(col.relativeVelocity.magnitude * col.gameObject.GetComponent<Rigidbody>().mass);
                //var force = col.relativeVelocity.magnitude * col.gameObject.GetComponent<Rigidbody>().mass;
                //var direction = transform.position - col.transform.position;
                //playerRig.AddForce(-direction.normalized * force);
            }
            //else if(col.gameObject.GetComponent<FracturedChunk>() != null)
            //{
            //    playerRig.mass = 200f;
            //    StartCoroutine(ReverseMass());
            //    //print(col.relativeVelocity.magnitude * col.gameObject.GetComponent<Rigidbody>().mass);
            //    //var force = col.relativeVelocity.magnitude * col.gameObject.GetComponent<Rigidbody>().mass;
            //    //var direction = transform.position - col.transform.position;
            //    //playerRig.AddForce(-direction.normalized * force);
            //}
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.GetComponent<ObjectBehavior>() != null /*|| col.gameObject.GetComponent<FracturedChunk>() != null*/)
        {
            //if (col.gameObject.GetComponent<ObjectBehavior>() != null)
            //{
                obj = col.gameObject.GetComponent<ObjectBehavior>().hit;
            //}

            if (obj)
            {
                playerRig.mass = GetComponent<PlayerStates>().becomeHeavy;
                StartCoroutine(ReverseMass());
                //print(col.relativeVelocity.magnitude * col.gameObject.GetComponent<Rigidbody>().mass);
                //var force = col.relativeVelocity.magnitude * col.gameObject.GetComponent<Rigidbody>().mass;
                //var direction = transform.position - col.transform.position;
                //playerRig.AddForce(-direction.normalized * force);
            }
            //else if (col.gameObject.GetComponent<FracturedChunk>() != null)
            //{
            //    playerRig.mass = 200f;
            //    StartCoroutine(ReverseMass());
            //    //print(col.relativeVelocity.magnitude * col.gameObject.GetComponent<Rigidbody>().mass);
            //    //var force = col.relativeVelocity.magnitude * col.gameObject.GetComponent<Rigidbody>().mass;
            //    //var direction = transform.position - col.transform.position;
            //    //playerRig.AddForce(-direction.normalized * force);
            //}
        }
    }

    IEnumerator ReverseMass()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        playerRig.mass = initialMass;
    }

    //void LateUpdate()
    //{
    //    if(prevent)
    //    {
    //        //StartCoroutine(TweakRigibody());
    //        GetComponent<Rigidbody>().Sleep();
    //        prevent = false;
    //    }
    //}

    //void Start()
    //{
    //    prevent = false;
    //}

    //IEnumerator TweakRigibody()
    //{
    //    GetComponent<Rigidbody>().isKinematic = true;
    //    yield return new WaitForSeconds(Time.deltaTime);
    //    GetComponent<Rigidbody>().isKinematic = false;
    //    prevent = false;
    //}
}
