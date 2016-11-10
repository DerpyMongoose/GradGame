using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicalMovement : MonoBehaviour {

    private int touches;
    private float distance, moveTimer, speed, acc, force, powerTime;
    private bool newSwipe, applyMove;
    private Vector3 temp, startPoint, dragPoint, direction;
    private Rigidbody playerRig;

    [HideInInspector]
    public bool ableToLift;
    public float timeForSwipe = 1f;
    public float hitForce, swirlForce, cdLift, doubleTapTime, collisionRadius, liftRadius;
    public int numOfCircleToShow;


    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        touches = 0;
        moveTimer = 0;
        newSwipe = false;
        applyMove = false;
        ableToLift = true;

    }


    void FixedUpdate()
    {
        if (applyMove)
        {
            playerRig.AddForce(direction.normalized * force);
            playerRig.velocity = Vector3.zero;
            applyMove = false;
        }
    }


    void Update()
    {

        moveTimer += Time.deltaTime;

        touches = Input.touchCount;

        if (touches > 1)
        {
            touches = 1;
        }

        for (int i = 0; i < touches; i++)
        {
            powerTime += Time.deltaTime;

            if (isGestureDone())
            {
                //IF WE NEED TO SEE SWIRLING ANIMATION WHEN YOU DO A CIRCLE GESTURE EVEN IF WE ARE NOT ABLE TO HIT SOMETHING, THEN NEEDS TO BE HERE.
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRadius);
                Swirling(hitColliders);
            }
            else
            {

                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    newSwipe = true;
                    moveTimer = 0;
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    startPoint = new Vector3(temp.x, 0, temp.z);
                }

                if (Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    GameManager.instance.player.GetComponent<PlayerStates>().imInSlowMotion = false;
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    dragPoint = new Vector3(temp.x, 0, temp.z);

                    distance = Vector3.Distance(dragPoint, startPoint);
                    direction = dragPoint - startPoint;
                    speed = distance / moveTimer;
                    acc = speed / moveTimer;
                    force = playerRig.mass * acc;
                    ///////////////////////////////////////////I WANT TO MAKE THE ROTATION BETTER WITHOUT RANDOM NUMBER//////////////////////////////
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), moveTimer * 10);
                    applyMove = true;

                    if (moveTimer >= timeForSwipe && newSwipe)
                    {
                        applyMove = false;
                        newSwipe = false;
                    }
                }
            }
        }

        if (Input.touchCount == 2)
        {
            if (powerTime < doubleTapTime && ableToLift)
            {
                print("Came here");
                ableToLift = false;
                StartCoroutine("Cooldown");
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, liftRadius);
                Lift(hitColliders);
            }
        }

        if (Input.touchCount == 0 || Input.touchCount > 2)
        {
            powerTime = 0;
        }

    }


    void Lift(Collider[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].tag != "Floor" && col[i].tag != "Wall" && col[i] != GetComponent<Collider>())
            {
                //HERE, DECTED THAT CAN HIT SOMETHING WITH SWIRLING, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RESTRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
                Rigidbody rig = col[i].GetComponent<Rigidbody>();
                rig.AddForce(Vector3.up * GameManager.instance.player.GetComponent<PlayerStates>().liftForce);
            }
        }
    }


    void Swirling(Collider[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].tag != "Floor" && col[i].tag != "Wall"  && col[i] != GetComponent<Collider>())
            {
                //HERE, DECTED THAT CAN HIT SOMETHING WITH SWIRLING, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RESTRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
                Rigidbody rig = col[i].GetComponent<Rigidbody>();
                Vector3 dir = col[i].transform.position - transform.position;
                rig.useGravity = true;
                rig.AddForce(dir.normalized * swirlForce);
            }
        }
    }


    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cdLift);
        ableToLift = true;
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "Floor" && col.collider.tag != "Wall")
        {
            Rigidbody rig = col.collider.GetComponent<Rigidbody>();
            rig.useGravity = true;
            rig.AddForce(direction.normalized * hitForce);
        }
    }


    List<Vector2> gestureDetector = new List<Vector2>();
    Vector2 gestureSum = Vector2.zero;
    float gestureLength = 0;
    int gestureCount = 0;

    bool isGestureDone()
    {
        if (Input.touches.Length > 2 || Input.touches[Input.touchCount - 1].phase == TouchPhase.Began)
        {
            gestureDetector.Clear();
            gestureCount = 0;
        }
        else
        {
            if (Input.touches[Input.touchCount - 1].phase == TouchPhase.Canceled || Input.touches[Input.touchCount - 1].phase == TouchPhase.Ended)
                gestureDetector.Clear();
            else if (Input.touches[Input.touchCount - 1].phase == TouchPhase.Moved)
            {
                Vector2 p = Input.touches[Input.touchCount - 1].position;
                if (gestureDetector.Count == 0 || (p - gestureDetector[gestureDetector.Count - 1]).magnitude > 10)
                    gestureDetector.Add(p);
            }
        }

        if (gestureDetector.Count < 10)
            return false;

        gestureSum = Vector2.zero;
        gestureLength = 0;
        Vector2 prevDelta = Vector2.zero;
        for (int i = 0; i < gestureDetector.Count - 2; i++)
        {

            Vector2 delta = gestureDetector[i + 1] - gestureDetector[i];
            float deltaLength = delta.magnitude;
            gestureSum += delta;
            gestureLength += deltaLength;

            float dot = Vector2.Dot(delta, prevDelta);
            if (dot < 0f)
            {
                gestureDetector.Clear();
                gestureCount = 0;
                return false;
            }

            prevDelta = delta;
        }

        int gestureBase = (Screen.width + Screen.height) / 4;

        if (gestureLength > gestureBase && gestureSum.magnitude < gestureBase / 2)
        {
            gestureDetector.Clear();
            gestureCount++;
            if (gestureCount >= numOfCircleToShow)
                return true;
        }

        return false;
    }
}
