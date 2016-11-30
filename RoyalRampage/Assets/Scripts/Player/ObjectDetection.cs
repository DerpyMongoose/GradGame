using UnityEngine;
using System.Collections;

public class ObjectDetection : MonoBehaviour {


    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.GetComponent<ObjectBehavior>() != null)
        {
            var obj = col.gameObject.GetComponent<ObjectBehavior>().hit;
            if (obj)
            {
                GetComponent<Rigidbody>().Sleep();               
                //StartCoroutine(TweakRigibody());
            }
        }
    }

    //IEnumerator TweakRigibody()
    //{
    //    GetComponent<Rigidbody>().isKinematic = true;
    //    yield return new WaitForSeconds(Time.deltaTime);
    //    GetComponent<Rigidbody>().isKinematic = false;
    //}
}
