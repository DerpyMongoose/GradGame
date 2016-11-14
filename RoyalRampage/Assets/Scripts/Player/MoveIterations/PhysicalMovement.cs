using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicalMovement : MonoBehaviour
{

    private int touches, countTaps;
    private float distance, moveTimer, circleTimer, speed, acc, force, powerTime, tapsTimer, time;
    private bool newSwipe, applyMove, startTimer, doingCircle, rotationTime;
    private bool newDash = false;   // for dashsound
    private Vector3 temp, startPoint, dragPoint, direction, firstTouch, secondTouch;
    private Rigidbody playerRig;

    [HideInInspector]
    public bool ableToLift;




    float CubicBezier(float t)
    {
        return ((1 - t * t * t) * GetComponent<PlayerStates>().p0) + (3 * (1 - t * t) * t * GetComponent<PlayerStates>().p1) + (3 * (1 - t) * t * t * GetComponent<PlayerStates>().p2) + (t * t * t * GetComponent<PlayerStates>().p3);
    }

    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        touches = 0;
        moveTimer = 0;
        newSwipe = false;
        applyMove = false;
        direction = -Vector3.forward;
        //ableToLift = true;

    }

    void FixedUpdate()
    {
        if (applyMove)
        {
            if (GetComponent<PlayerStates>().clamped)
            {
                if (force > GetComponent<PlayerStates>().maxMoveForce)
                {
                    force = GetComponent<PlayerStates>().maxMoveForce;
                }
            }
            else
            {
                force = force * CubicBezier(moveTimer);
            }
            playerRig.AddForce(direction.normalized * force);
            playerRig.velocity = Vector3.zero;

            //dash sound
            if (newDash == true)
            {
                GameManager.instance.playerDash();
                newDash = false;
            }

            applyMove = false;
        }
    }


    void Update()
    {
        //print(applyMove);
        moveTimer += Time.deltaTime;
        circleTimer += Time.deltaTime;

        if (startTimer)
        {
            tapsTimer += 0.01f;
        }
        if (countTaps >= 2 || tapsTimer >= GetComponent<PlayerStates>().doubleTapTime)
        {
            countTaps = 0;
            startTimer = false;
            tapsTimer = 0;
            firstTouch = Vector3.zero;
            secondTouch = Vector3.zero;
        }

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
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().swirlRadius);
                Swirling(hitColliders);
            }
            else
            {

                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    newSwipe = true;
                    newDash = true;
                    startTimer = true;
                    moveTimer = 0;
                    countTaps++;
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    startPoint = new Vector3(temp.x, 0, temp.z);
                    if (countTaps == 1)
                    {
                        firstTouch = new Vector3(temp.x, 0, temp.z);
                    }
                    else if (countTaps == 2)
                    {
                        secondTouch = new Vector3(temp.x, 0, temp.z);
                    }
                    //print(Vector3.Distance(firstTouch, secondTouch));
                    if (countTaps == 2 && tapsTimer < GetComponent<PlayerStates>().doubleTapTime && ableToLift && Vector3.Distance(firstTouch, secondTouch) < 1)
                    {
                        //print("came here");
                        ableToLift = false;
                        GetComponent<PlayerStates>().lifted = true;
                        GetComponent<PlayerStates>().imInSlowMotion = true;
                        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().liftRadius);
                        Lift(hitColliders);
                    }
                }

                if (Input.GetTouch(i).phase == TouchPhase.Moved && newSwipe)
                {
                    countTaps = 0;
                    startTimer = false;
                    tapsTimer = 0;
                    firstTouch = Vector3.zero;
                    secondTouch = Vector3.zero;
                    GetComponent<PlayerStates>().imInSlowMotion = false;
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    dragPoint = new Vector3(temp.x, 0, temp.z);
                    distance = Vector3.Distance(dragPoint, startPoint);
                    //print(Vector3.Angle(startPoint, dragPoint));
                    //print(distance);
                    direction = dragPoint - startPoint;
                    speed = distance / moveTimer;
                    acc = speed / moveTimer;
                    force = playerRig.mass * acc;
                    ///////////////////////////////////////////I WANT TO MAKE THE ROTATION BETTER WITHOUT RANDOM NUMBER//////////////////////////////
                    //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), moveTimer * 10);
                    if (distance > GetComponent<PlayerStates>().distSwipe)
                    {
                        applyMove = true;
                        rotationTime = true;
                    }

                    if (moveTimer >= GetComponent<PlayerStates>().timeForSwipe && newSwipe)
                    {
                        applyMove = false;
                        newSwipe = false;
                    }
                }

                //if(Input.GetTouch(i).phase == TouchPhase.Ended)
                //{
                //    doingCircle = false;
                //}
            }
        }

        if (Input.touchCount == 2)
        {
            if (powerTime < GetComponent<PlayerStates>().SameTapTime && ableToLift)
            {
                ableToLift = false;
                GetComponent<PlayerStates>().lifted = true;
                GetComponent<PlayerStates>().imInSlowMotion = true;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().liftRadius);
                Lift(hitColliders);
            }
        }

        if (Input.touchCount == 0 || Input.touchCount > 2)
        {
            powerTime = 0;
        }

        //if(rotationTime)
        //{
        //print(direction);
        //time += 0.01f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * GetComponent<PlayerStates>().rotationSpeed);
        //if(time > 0.5f)
        //{
        //    rotationTime = false;
        //}

        //}

    }


    void Lift(Collider[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].tag == "Destructable")
            {
                //HERE, DECTED THAT CAN HIT SOMETHING WITH LIFT, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RESTRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
                Rigidbody rig = col[i].GetComponent<Rigidbody>();
                rig.mass = 0.5f;
                rig.AddForce(Vector3.up * GetComponent<PlayerStates>().liftForce);
                rig.AddTorque(Vector3.up * GetComponent<PlayerStates>().torgueForce);
                col[i].gameObject.GetComponent<ObjectBehavior>().hasLanded = false; //THIS HAS AN ERROR
            }
        }

        // SOUND AND ANIMATION FOR STOMP
        GameManager.instance.playerStomp();
    }


    void Swirling(Collider[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].tag == "Destructable")
            {
                //HERE, DECTED THAT CAN HIT SOMETHING WITH SWIRLING, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RESTRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
                Rigidbody rig = col[i].GetComponent<Rigidbody>();
                Vector3 dir = col[i].transform.position - transform.position;
                GetComponent<PlayerStates>().hitObject = true;

                // SOUND OBJECT HIT
                GameManager.instance.objectHit(col[i].gameObject);

                rig.useGravity = true;
                if (GetComponent<PlayerStates>().lifted)
                {
                    rig.AddForce((dir.normalized + new Vector3(0, GetComponent<PlayerStates>().degreesInAir / 90, 0)) * GetComponent<PlayerStates>().swirlForce);
                }
                else
                {
                    rig.AddForce(dir.normalized * GetComponent<PlayerStates>().swirlForce);
                }
            }
        }
        GameManager.instance.playerSwirl();
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Destructable")
        {
            Rigidbody rig = col.collider.GetComponent<Rigidbody>();
            GetComponent<PlayerStates>().hitObject = true;

            // SOUND OBJECT HIT
            GameManager.instance.objectHit(col.collider.gameObject);

            rig.useGravity = true;
            if (GetComponent<PlayerStates>().lifted)
            {
                rig.AddForce((direction.normalized + new Vector3(0, GetComponent<PlayerStates>().degreesInAir / 90, 0)) * GetComponent<PlayerStates>().hitForce);
            }
            else
            {
                rig.AddForce(direction.normalized * GetComponent<PlayerStates>().hitForce);
            }
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
            circleTimer = 0;
        }
        else
        {
            if (Input.touches[Input.touchCount - 1].phase == TouchPhase.Canceled || Input.touches[Input.touchCount - 1].phase == TouchPhase.Ended)
                gestureDetector.Clear();
            else if (Input.touches[Input.touchCount - 1].phase == TouchPhase.Moved)
            {
                Vector2 p = Input.touches[Input.touchCount - 1].position;
                if (gestureDetector.Count == 0 || (p - gestureDetector[gestureDetector.Count - 1]).magnitude > 5)
                {
                    gestureDetector.Add(p);
                }
            }
        }

        if (gestureDetector.Count < 5)
        {
            return false;
        }

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

        if (gestureLength > gestureBase / 2 && gestureSum.magnitude < gestureBase / 2) //gestureBase divided by 2 for a half arc
        {
            gestureDetector.Clear();
            gestureCount++;
            if (gestureCount >= GetComponent<PlayerStates>().numOfCircleToShow)
            {
                if (circleTimer < GetComponent<PlayerStates>().timeForCircle)
                {
                    print("I made a circle");
                    circleTimer = 0;
                    return true;
                }
                else
                {
                    print("Too slow");
                    circleTimer = 0;
                }
            }
        }

        return false;
    }
}
