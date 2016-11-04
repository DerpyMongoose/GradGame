using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

    public float playerSpeed = 1000;

    private Rigidbody playerRG;
    private Vector3 zeroVelocity;

    void Start()
    {
        playerRG = GetComponent<Rigidbody>();
        zeroVelocity = Vector3.zero;
    }


    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            playerRG.AddRelativeForce(Vector3.forward * playerSpeed);
            playerRG.velocity = zeroVelocity;
        }

        if (Input.GetKey("s"))
        {
            playerRG.AddRelativeForce(-Vector3.forward * playerSpeed);
            playerRG.velocity = zeroVelocity;
        }

        if (Input.GetKey("d"))
        {
            transform.Rotate(new Vector3(0, 5, 0));
        }

        if (Input.GetKey("a"))
        {
            transform.Rotate(new Vector3(0, -5, 0));
        }
    }
}