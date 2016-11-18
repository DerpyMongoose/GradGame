using UnityEngine;
using System.Collections;

public class ConeDetection : MonoBehaviour {

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Destructable")
        {
            //print("I am in collision");
            var decreaseHp = true;
            //PlayerStates.ableToSwipe = true;
            if (PlayerStates.swiped)
            {
                Rigidbody rig = col.GetComponent<Rigidbody>();
                rig.isKinematic = false;
                col.GetComponent<ObjectBehavior>().hit = true;
                if (PlayerStates.lifted)
                {
                    rig.AddForce((SwipeHalf.attackDir.normalized + new Vector3(0, GameManager.instance.player.GetComponent<PlayerStates>().degreesInAir / 90, 0)) * GetComponent<PlayerStates>().hitForce);
                }
                else
                {
                    rig.AddForce(SwipeHalf.attackDir.normalized * GameManager.instance.player.GetComponent<PlayerStates>().hitForce);
                }
                if (decreaseHp)
                {
                    col.GetComponent<ObjectBehavior>().life -= ObjectManagerV2.instance.dashDamage;
                    decreaseHp = false;
                }
                //PlayerStates.swiped = false;
            }
        }
    }

}
