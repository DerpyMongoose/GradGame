using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour
{
    private enum direction
    {
        forward,
        backward,
        left,
        right,
        up,
        down,
    };
    [SerializeField]
    private direction direction_;
    [SerializeField]
    private float lifeTime;

    private Vector3 dirVector;

    private Rigidbody rb;

    private float randX;
    private float randY;
    private float randZ;
    private float timer;
    // Use this for initialization

    void Start()
    {
        randX = Random.Range(1, 10);
        randY = Random.Range(1, 10);
        randZ = Random.Range(1, 10);
        rb = GetComponent<Rigidbody>();
        switch (direction_)
        {
            case direction.forward:
            dirVector = Vector3.forward;
            break;
            case direction.backward:
            dirVector = Vector3.back;
            break;
            case direction.left:
            dirVector = Vector3.left;
            break;
            case direction.right:
            dirVector = Vector3.right;
            break;
            case direction.up:
            dirVector = Vector3.up;
            break;
            case direction.down:
            dirVector = Vector3.down;
            break;
            default:
            break;
        }

        dirVector = new Vector3(randX, randY, randZ);
        transform.Rotate(dirVector);
        rb.AddForce(1,   1,  1);
        Destroy(gameObject, lifeTime);
    }
}
