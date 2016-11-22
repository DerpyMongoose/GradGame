using UnityEngine;
using System.Collections;

public class ConeDetection : MonoBehaviour
{

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Destructable")
        {
            var decreaseHp = true;
            if (PlayerStates.swiped)
            {
                Rigidbody rig = col.GetComponent<Rigidbody>();
                rig.isKinematic = false;
                col.GetComponent<ObjectBehavior>().hit = true;

                // PLAY DAMAGE PARTICLE
                //print(col.bounds.extents.y * 2);
                col.GetComponent<ObjectBehavior>().particleSys.Play(); /////////IT WILL GIVE AN ERROR IN THE LEVELS WITHOUT THE FRACTURED OBJECTS

                if (PlayerStates.lifted)
                {
                    rig.AddForce((SwipeHalf.attackDir.normalized + new Vector3(0, GameManager.instance.player.GetComponent<PlayerStates>().degreesInAir / 90, 0)) * GetComponent<PlayerStates>().hitForce); // Error here
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
            }
        }
    }

}
