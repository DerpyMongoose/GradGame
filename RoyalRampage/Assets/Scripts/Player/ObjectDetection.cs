using UnityEngine;
using System.Collections;

public class ObjectDetection : MonoBehaviour {

    bool prevent;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.GetComponent<ObjectBehavior>() != null)
        {
            var obj = col.gameObject.GetComponent<ObjectBehavior>().hit;
            if (obj)
            {
                //GetComponent<Rigidbody>().Sleep();               
                //StartCoroutine(TweakRigibody());
                prevent = true;
            }
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.GetComponent<ObjectBehavior>() != null)
        {
            var obj = col.gameObject.GetComponent<ObjectBehavior>().hit;
            if (obj)
            {
                //GetComponent<Rigidbody>().Sleep();               
                //StartCoroutine(TweakRigibody());
                prevent = true;
            }
        }
    }

    void LateUpdate()
    {
        if(prevent)
        {
            //StartCoroutine(TweakRigibody());
            GetComponent<Rigidbody>().Sleep();
        }
    }

    void Start()
    {
        prevent = false;
    }

    //IEnumerator TweakRigibody()
    //{
    //    GetComponent<Rigidbody>().isKinematic = true;
    //    yield return new WaitForSeconds(Time.deltaTime);
    //    GetComponent<Rigidbody>().isKinematic = false;
    //    prevent = false;
    //}
}
