using UnityEngine;
using System.Collections;

public class CameraController7 : MonoBehaviour
{

    [Tooltip("The smoothness of the camera movement")]
    [SerializeField]
    private int smoothness = 3;
    [Tooltip("The smoothness of the camera rotation")]
    [SerializeField]
    private int rotSmooth = 3;

    private enum state { WALL, FREE }
    private state state_;

    private GameObject player;
    private GameObject tempTrans;

    public GameObject dummy;
    public GameObject pivot;

    private Vector3 offset;
    private Vector3 dummyOffset;
    private Quaternion offsetRotation;

    private RaycastHit hit;
    private RaycastHit hit2;

    void Start()
    {
        state_ = state.FREE;
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
        dummyOffset = dummy.transform.position - player.transform.position;
        offsetRotation = transform.rotation;

    }

    void FixedUpdate()
    {
        switch (state_)
        {
            case state.FREE:
                Vector3 targetCamPos = player.transform.position + offset;
                transform.position = Vector3.Lerp(transform.position, targetCamPos, Time.deltaTime * smoothness);
                transform.rotation = offsetRotation;
                Debug.DrawLine(player.transform.position, dummy.transform.position, Color.blue);
                Physics.Linecast(player.transform.position, pivot.transform.position, out hit, 1 << 8);
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Wall")
                    {
                        state_ = state.WALL;
                    }
                }
                print("free");
                break;
            case state.WALL:
                Vector3 direction = player.transform.position - pivot.transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, rotation, Time.deltaTime * rotSmooth);
                //transform.RotateAround();
                pivot.transform.position = new Vector3(player.transform.position.x, pivot.transform.position.y, hit.point.z - 0.1f);
                dummy.transform.position = player.transform.position + dummyOffset;
                Debug.DrawLine(player.transform.position, dummy.transform.position, Color.red);
                Physics.Linecast(player.transform.position, dummy.transform.position, out hit, 1 << 8);
                if (hit.transform != null)
                {
                    if (hit.transform.gameObject == dummy)
                    {
                        print("Heyooo");
                        state_ = state.FREE;
                    }
                }
                print("wall");
                break;
            default:
                break;
        }
    }
}
