using UnityEngine;
using System.Collections;

public class MoveAttack : MonoBehaviour {

    private float touches;
    private Rigidbody playerRG;
    private Vector3 direction;
    private Vector3 startPoint,endPoint;
    private float magnitude;

    private Vector3 temp;

    public Collider floor, Nwall,Ewall,Wwall,Swall;
    public float force;
    public float hitForce;

    // Use this for initialization
    void Start () {
        playerRG = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        touches = Input.touchCount;

        if(touches > 1)
        {
            touches = 1;
        }

        for(int i=0; i<touches; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began /*|| Input.GetTouch(i).phase == TouchPhase.Moved || Input.GetTouch(i).phase == TouchPhase.Stationary*/)
            {
                //Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.farClipPlane));
                //Debug.DrawLine(Camera.main.transform.position, point, Color.red);
                temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.farClipPlane));

                startPoint = new Vector3(temp.x, 0, temp.z);

                //direction = point - transform.position;
                //playerRG.AddForce(direction.normalized * force);
            }

            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                //Vector3 endPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.farClipPlane));
                temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.farClipPlane));

                endPoint = new Vector3(temp.x, 0, temp.z);

                //float distance = Vector3.Distance(endPoint,startPoint);
                direction = endPoint - startPoint;
                magnitude = direction.magnitude;
                print(magnitude);
                playerRG.AddForce(direction.normalized * magnitude * force);
                transform.rotation = Quaternion.LookRotation(direction);
            }

            
        }
    }


   /* void OnCollisionEnter(Collision col)
    {
        if (col.collider != floor && col.collider != Nwall && col.collider != Ewall && col.collider != Wwall && col.collider != Swall)
        {
            Rigidbody rig = col.collider.GetComponent<Rigidbody>();

            rig.AddForce(direction.normalized * hitForce);
        }
    }*/

}
