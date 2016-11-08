using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [Tooltip("Offset in the Y")]
    [SerializeField]
    private int cameraHeight = 5;
    [Tooltip("Offset in the Z")]
    [SerializeField]
    private int cameraLength = 8;

    private Vector3 direction;

    private Quaternion lookDirection;

    private GameObject player;

    private NavMeshAgent nav;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position - new Vector3(0, 0, cameraLength);
        nav = GetComponent<NavMeshAgent>();
        nav.baseOffset = cameraHeight;
        nav.updateRotation = false;
    }
	
	void Update ()
    {
        if (nav.isOnNavMesh == true)
        {
            if (VectorXZDistance(player.transform.position, transform.position) <= cameraLength)
            {
                nav.Stop();
                nav.updatePosition = false;
                nav.velocity = Vector3.zero;
                transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            }
            else
            {
                nav.Resume();
                nav.updatePosition = true;
                nav.SetDestination(player.transform.position);
                transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            }
         
        }
    }
    private int VectorXZDistance(Vector3 v1, Vector3 v2)
    {
        float xDiff = v1.x - v2.x;
        float zDiff = v1.z - v2.z;
        return (int)Mathf.Sqrt((xDiff * xDiff) + (zDiff * zDiff));
    }
}