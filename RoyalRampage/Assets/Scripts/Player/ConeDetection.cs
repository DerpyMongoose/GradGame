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
                var script = GetComponentInParent<SwipeHalf>();
                if (script.coroutine != null)
                {
                    script.StopCoroutine(script.coroutine);
                    script.Reverse(script.objRB, script.initialMass);
                }

                Rigidbody rig = col.GetComponent<Rigidbody>();
                col.GetComponent<ObjectBehavior>().hit = true;

				///SOUND PLAYER HIT OBJECT
				GameManager.instance.objectHit (col.gameObject);

                // PLAY DAMAGE PARTICLE
                //col.GetComponent<ObjectBehavior>().particleSys.Play(); /////////IT WILL GIVE AN ERROR IN THE LEVELS WITHOUT THE FRACTURED OBJECTS

                if (col.GetComponent<ObjectBehavior>().lifted)
                {
                    rig.AddForce((SwipeHalf.attackDir.normalized + new Vector3(0, GameManager.instance.player.GetComponent<PlayerStates>().degreesInAir / 90, 0)) * GameManager.instance.player.GetComponent<PlayerStates>().hitForce); // Error here
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
