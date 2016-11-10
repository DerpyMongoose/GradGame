using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveAttackV2 : MonoBehaviour
{

    private Rigidbody playerRG;
    private Vector3 direction, startPoint, endPoint, dragpoint, temp;
    private float magnitude, powerTime, swipeTimer;
    private int amountOfTaps;
    private bool dragged, notStartAndEnd, beginCount, onlyOneTouch, ableToLift, circleTime, applyMove;

    private float distance, speed, acc, force, rotateTime, tempVariable;
    private bool canRotate, boolForRotation, newSwipe;
    //private string place;

    public Collider floor, Nwall, Ewall, Wwall, Swall;
    public float hitForce, swirlForce, cdLift, doubleTapTime, collisionRadius, scaleTime;
    public int numOfCircleToShow;
    public AnimationCurve curve;


    void Start()
    {
        playerRG = GetComponent<Rigidbody>();
        amountOfTaps = 0;
        swipeTimer = 0;
        rotateTime = 0;
        ableToLift = true;
        notStartAndEnd = false;
        applyMove = false;
        canRotate = false;
        boolForRotation = true;
        newSwipe = false;
    }


    void FixedUpdate()
    {

        //if (playerRG.isKinematic)
        //{
        //    playerRG.isKinematic = false;
        //}

        if (applyMove && newSwipe)
        {
            //print("I came");
            //print(force * 2f);
            //float tempForce = Mathf.Clamp(force, 200000f, 500000f);
            playerRG.AddForce(direction.normalized * force * tempVariable * 2f);
            //transform.rotation = Quaternion.LookRotation(direction);
            newSwipe = false;
            //canRotate = true;
            //transform.rotation = Quaternion.LookRotation(direction);
            //playerRG.AddForce(direction.normalized * 100000f);
            applyMove = false;
        }
    }

    /// <summary>
    /// THERE IS AN ISSUE BY IMPLEMENTING BOTH THE DOUBLE TAP WITH ONE FINGER AND THE TWO TAPS AT THE SAME TIME.
    /// THEY NEED DIFFERENT TIMER. YOU CAN PERFORM THE TAPS AT THE SAME TIME WITHIN ONE FRAME (0.02) BUT NOT THE DOUBLE TAP.
    /// THEN IF YOU INCREACE THE TIME, THEY BOTH WORK BUT NOW YOU ARE ABLE ALSO TO DO A TAP AND THEN ANOTHER TAP WITHOUT CONSIDERING THEM TO BE A DOUBLE TAP
    /// IN THE SAME POINT.
    /// </summary>

    void Update()
    {


        //print(dragged);
        //print(powerTime);
        //print(amountOfTaps);
        powerTime += Time.deltaTime;


        if (powerTime >= doubleTapTime || amountOfTaps >= 2)
        {
            amountOfTaps = 0;
            powerTime = 0;
            dragged = false;
        }

        if (Input.touchCount > 0 && Input.touchCount < 3)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (isGestureDone())
                {
                    //print("I made a circle");
                    //////////////PLAY ANIMATION OF SWIRLING////////////////////
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRadius);
                    Swirling(hitColliders);
                }

                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    //print("I am here");
                    amountOfTaps++;
                    newSwipe = true;
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    startPoint = new Vector3(temp.x, 0, temp.z);

                    if (amountOfTaps == 2 && powerTime < doubleTapTime && !dragged && ableToLift)
                    {
                        //print("Lift");
                       //GameManager.instance.TimeToLift();
                    }
                }

                if (Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    swipeTimer += Time.deltaTime;

                    //print(swipeTimer);
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    dragpoint = new Vector3(temp.x, 0, temp.z);
                    Vector3 dir = dragpoint - startPoint;
                    distance = Vector3.Distance(dragpoint, startPoint);

                    //print(distance);
                    if (distance > 4f && swipeTimer > 0.05f && newSwipe == true)
                    {
                        //print("Dragged");
                        //notStartAndEnd = true;
                        //print(distance);
                        direction = dir;
                        speed = distance / (swipeTimer * scaleTime);
                        acc = speed / (swipeTimer * scaleTime);
                        force = playerRG.mass * acc;

                        tempVariable = Mathf.Clamp(force, 50000f, 1000000f);
                        tempVariable = (tempVariable - 50000f) / (50000f);
                        tempVariable = curve.Evaluate(tempVariable);

                        applyMove = true;
                        if (boolForRotation)
                        {
                            //print("I am here");
                            boolForRotation = false;
                            canRotate = true;
                        }
                        //transform.rotation = Quaternion.LookRotation(direction);
                        //transform.LookAt(direction);
                    }
                    //else
                    //{
                    //    applyMove = false;
                    //}

                }


                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.farClipPlane));
                    endPoint = new Vector3(temp.x, 0, temp.z);


                    //print(swipeTimer);
                    //if (notStartAndEnd && swipeTimer <= 0.4f)
                    //{
                    //    print("dragged");
                    //    dragged = true;
                    //    powerTime = 0;
                    //    amountOfTaps = 0;
                    //    direction = endPoint - startPoint;
                    //    distance = Vector3.Distance(endPoint, startPoint);
                    //    //print(direction.magnitude);
                    //    //print(swipeTimer);
                    //    //speed = direction.magnitude / (swipeTimer);
                    //    speed = distance / (swipeTimer * scaleTime);
                    //    //print(speed);
                    //    //force = (playerRG.mass * (speed * speed)) / 2;
                    //    acc = speed / (swipeTimer * scaleTime);
                    //    //print(acc);
                    //    force = playerRG.mass * acc;
                    //    //print(force);
                    //    //magnitude = direction.magnitude;
                    //    applyMove = true;
                    //    //playerRG.AddForce(direction.normalized * force);
                    //    transform.rotation = Quaternion.LookRotation(direction);
                    //    //PLAY MOVE ANIMATION, I AM THINKING OF DIFFERENT WALK-JOKING-RUNNING ANIMATION THAT WE CAN DIFFERENTIATE ACCORDING TO DIRECTION'S MAGNITUDE.
                    //}
                    //notStartAndEnd = false;
                    swipeTimer = 0;
                    boolForRotation = true;
                    //rotateTime = 0;
                    //canRotate = true;
                }

            }
        }

        if (canRotate)
        {
            //print("I am in");
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), rotateTime * 2f);
            rotateTime += Time.deltaTime;
            if (rotateTime >= 1f)
            {
                canRotate = false;
                rotateTime = 0;
            }
            //canRotate = false;
        }
    }


    void Swirling(Collider[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i] != floor && col[i] != Nwall && col[i] != Ewall && col[i] != Wwall && col[i] != Swall && col[i] != GetComponent<Collider>())
            {
                //HERE, DECTED THAT CAN HIT SOMETHING WITH SWIRLING, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RESTRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
                Rigidbody rig = col[i].GetComponent<Rigidbody>();
                Vector3 dir = col[i].transform.position - transform.position;
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

        /// <summary>
        /// I CAN PREVENT THE PLAYER FROM CONTINUING ON RAMPAGE WHEN ACTUALLY HITS AN OBJECT. DON'T KNOW HOW IT WILL FEEL IN THE ACTUALL GAME.
        /// </summary>

        //playerRG.isKinematic = true;

        if (col.collider != floor && col.collider != Nwall && col.collider != Ewall && col.collider != Wwall && col.collider != Swall)
        {
            Rigidbody rig = col.collider.GetComponent<Rigidbody>();

            rig.AddForce(direction.normalized * hitForce);
        }
        else if (col.collider == Nwall || col.collider == Ewall || col.collider == Wwall || col.collider == Swall)
        {
            //col.collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //playerRG.velocity = Vector3.zero;
            //playerRG.isKinematic = true;
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

