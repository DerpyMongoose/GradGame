using UnityEngine;
using System.Collections;

public class CameraController3 : MonoBehaviour
{
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

    private bool Clipping = false;

    private GameObject player;
    private GameObject tempTrans;

    private RaycastHit hit;

    private Quaternion currentRotation;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        if (Clipping == false)
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
    }
    void FixedUpdate()
    {
        Debug.DrawLine(player.transform.position, transform.position);
        if (Physics.Linecast(player.transform.position, transform.position, out hit) && hit.transform.gameObject != player)
        {
            Clipping = true;
            transform.position = hit.point;
            //rotationAngle = player.transform.eulerAngles.y;

            //currentRotationAngle = transform.eulerAngles.y;

            //currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, rotationAngle, Time.deltaTime * rotationSpeed);

            //currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            //transform.position -= currentRotation * Vector3.forward;
        }
        else Clipping = false;
    }
}