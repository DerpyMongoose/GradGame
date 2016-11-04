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
        player = GameObject.FindGameObjectWithTag("Player");
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
        Debug.DrawLine(transform.position, player.transform.position);
        if (hit.transform != null)
        {
            if (hit.transform.gameObject != player)
            {
                tempTrans = hit.transform.gameObject;
                print(tempTrans);
                hit.transform.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (hit.transform.gameObject == player && tempTrans != null)
            {
                print(tempTrans);
                tempTrans.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}
