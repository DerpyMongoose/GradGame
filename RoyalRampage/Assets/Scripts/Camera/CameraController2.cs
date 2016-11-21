using UnityEngine;
using System.Collections;

public class CameraController2 : MonoBehaviour
{

    [Tooltip("The smoothness of the camera movement")]
    [SerializeField]
    private int smoothness = 3;

    private GameObject player;
    private GameObject tempTrans;

    private Vector3 offset;

    private RaycastHit hit;

    void Start()
    {
        player = GameManager.instance.player;
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        Vector3 targetCamPos = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothness * Time.deltaTime);
    }
    void FixedUpdate()
    {
        Physics.Linecast(transform.position, player.transform.position, out hit);
        if (hit.transform != null)
        {
            if (hit.transform.tag == "Wall")
            {
                tempTrans = hit.transform.gameObject;
                if (hit.transform.GetComponent<MeshRenderer>() != null)
                {
                    hit.transform.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            else if (hit.transform.gameObject == player && tempTrans != null)
            {
                if (hit.transform.GetComponent<MeshRenderer>() == null)
                {
                    tempTrans.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }
}
