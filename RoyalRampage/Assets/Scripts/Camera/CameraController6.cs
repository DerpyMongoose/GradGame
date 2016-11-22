using UnityEngine;
using System.Collections;

public class CameraController6 : MonoBehaviour
{

    [Tooltip("The smoothness of the camera movement")]
    [SerializeField]
    private int smoothness = 3;
    [Tooltip("The smoothness of the camera rotation")]
    [SerializeField]
    private int rotSmooth = 3;

    private GameObject player;
    private GameObject tempTrans;

    private Vector3 offset;
    private Quaternion offsetRotation;

    private RaycastHit hit;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
        offsetRotation = transform.rotation;
  
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, Time.deltaTime * smoothness);
        transform.rotation = offsetRotation;

        Physics.Linecast(player.transform.position, transform.position, out hit, 1 << 8);
        if (hit.transform != null)
        {
            if (hit.transform.tag == "Wall")
            {
                Vector3 direction = player.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotSmooth);
                transform.position = new Vector3(player.transform.position.x, transform.position.y, hit.point.z);
            }
        }
    }
}
