using UnityEngine;
using System.Collections;

// How to put the destruction in focus.
// Which movement is the least intrusive.

public class CameraController1 : MonoBehaviour {

    //Camera Shake

    [Tooltip("The rotation speed of the camera")]
    [SerializeField]
    private int rotationSpeed = 3;
    [Tooltip("Offset in the Y")]
    [SerializeField]
    private int cameraHeight = 5;
    [Tooltip("Offset in the Z")]
    [SerializeField]
    private int cameraLength = 8;

    private float rotationAngle;
    private float currentRotationAngle;

    private GameObject player;
    private GameObject tempTrans;

    private RaycastHit hit;

    private Quaternion currentRotation;



    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	void LateUpdate ()
    {
        rotationAngle = player.transform.eulerAngles.y;

        currentRotationAngle = transform.eulerAngles.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, rotationAngle, Time.deltaTime * rotationSpeed);

        currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = player.transform.position;
        transform.position -= currentRotation * Vector3.forward * cameraLength;

        transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z); //Lerp this
        transform.LookAt(player.transform.position + new Vector3(0, 3, 0));
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
                hit.transform.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (hit.transform.gameObject == player && tempTrans != null)
            {
                tempTrans.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}