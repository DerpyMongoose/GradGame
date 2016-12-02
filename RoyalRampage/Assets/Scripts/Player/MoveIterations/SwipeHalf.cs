﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwipeHalf : MonoBehaviour
{
    public static bool startTutTimer = false;

    private int touches, countTaps;
    private float distance, attackDist, moveTimer, circleTimer, speed, acc, force, powerTime, tapsTimer, time, startingMass;
    private bool newSwipe, applyMove, startTimer, doingCircle, rotationTime, rightOk, leftOk;
    private bool newDash = false;   // for dashsound
    private Vector3 temp, startPoint, startPointAtt, dragPoint, dragPointAtt, direction, firstTouch, secondTouch;
    private Rigidbody playerRig;

    [HideInInspector]
    public bool swirlEnded;
    [HideInInspector]
    public static bool ableToLift, intoAir;
    [HideInInspector]
    public List<Rigidbody> objRB = new List<Rigidbody>();
    [HideInInspector]
    public List<float> initialMass = new List<float>();
    [HideInInspector]
    public static Vector3 attackDir;
    [HideInInspector]
    public IEnumerator coroutine;
    [HideInInspector]
    public List<Collider> tempColliders = new List<Collider>();
    [HideInInspector]
    public bool swirlTut, stompTut;

    private bool spinningAnim = false;
    private bool swipeToHit = false;

    void Start()
    {
        swirlTut = false;
        playerRig = GetComponent<Rigidbody>();
        startingMass = playerRig.mass;
        touches = 0;
        moveTimer = 0;
        newSwipe = false;
        applyMove = false;
        //inAir = false;
        direction = -Vector3.forward;
        swirlEnded = true;
        PlayerStates.swiped = false;
    }


    void FixedUpdate()
    {
        if (applyMove)
        {
            float highForce = GameManager.instance.player.GetComponent<PlayerStates>().maxVelocity;

            //force = force * CubicBezier(moveTimer);
            float playerVelocity = playerRig.mass * (playerRig.velocity.magnitude * playerRig.velocity.magnitude) / 2;

            //print("force: " + force);
            playerRig.AddForce(direction.normalized * force);
            playerRig.velocity = Vector3.zero;
            //transform.rotation = Quaternion.LookRotation(direction);
            float maxForce = (playerRig.mass * (highForce * highForce)) / 2;

            //print("player velocity: " + playerVelocity);
            //print("max: " + maxForce);

            //if (playerVelocity > maxForce)
            //{

            //    var difForce = force - maxForce;
            //    //print("diff: " + difForce);
            //    playerRig.AddForce(-direction.normalized * difForce);
            //}

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
        print(playerRig.mass);
        Debug.DrawRay(transform.position, -transform.right * 1f, Color.red, 2f);
        //print(powerTime);
        //print(rightOk);
        //print(leftOk);
        moveTimer += Time.deltaTime;
        circleTimer += Time.deltaTime;

        touches = Input.touchCount;

        if (touches > 2)
        {
            touches = 2;
        }

        for (int i = 0; i < touches; i++)
        {
            powerTime += Time.deltaTime;

            if (Input.GetTouch(i).position.x <= Screen.width / 2)
            {
                if (GameManager.instance.TutorialState() == GameManager.Tutorial.STOMP && GameManager.instance.CurrentScene() == GameManager.Scene.TUTORIAL)
                {
                    rightOk = true;
                }
                //HERE MOVING////////////////////////
                if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME || GameManager.instance.TutorialState() == GameManager.Tutorial.MOVEMENT)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        rightOk = true;
                        newSwipe = true;
                        newDash = true;
                        moveTimer = 0;
                        temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                        startPoint = new Vector3(temp.x, 0, temp.z);
                    }

                    else if (Input.GetTouch(i).phase == TouchPhase.Moved && newSwipe)
                    {
                        if (coroutine != null)
                        {
                            StopCoroutine(coroutine);
                            Reverse(objRB, initialMass);
                        }
                        startTutTimer = true;
                        PlayerStates.imInSlowMotion = false;
                        temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                        dragPoint = new Vector3(temp.x, 0, temp.z);
                        distance = Vector3.Distance(dragPoint, startPoint);
                        //print(distance);
                        direction = dragPoint - startPoint;
                        //speed = distance / moveTimer;
                        speed = distance;
                        //acc = speed / moveTimer;
                        //force = playerRig.mass * acc;
                        force = playerRig.mass * (speed * GetComponent<PlayerStates>().moveForce);
                    if (distance > GetComponent<PlayerStates>().distSwipe && distance <= GetComponent<PlayerStates>().maxDistSwipe && playerRig.mass <= startingMass)
                        {
                            //if (Mathf.Round(playerRig.velocity.magnitude) == 0)
                            //{
                            applyMove = true;
                            //rotationTime = true;
                            //}
                        }

                        if (moveTimer >= GetComponent<PlayerStates>().timeForSwipe && newSwipe)
                        {
                            applyMove = false;
                            newSwipe = false;
                        }
                    }
                    else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        rightOk = false;
                    }
                }
            }
            else if (Input.GetTouch(i).position.x > Screen.width / 2)
            {
                if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME || GameManager.instance.TutorialState() != GameManager.Tutorial.MOVEMENT)
                {
                    //HERE ATTACKING////////////////////
                    if (swirlEnded)
                    {
                        if (isGestureDone())
                        {
                            swirlTut = true;
                            swirlEnded = false;
                            //IF WE NEED TO SEE SWIRLING ANIMATION WHEN YOU DO A CIRCLE GESTURE EVEN IF WE ARE NOT ABLE TO HIT SOMETHING, THEN NEEDS TO BE HERE.
                            GameManager.instance.playerSwirl();
                            spinningAnim = true;
                            //Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().swirlRadius);
                            //for (int k = 0; k < hitColliders.Length; k++)
                            //{
                            //    if (hitColliders[k].tag == "Destructable")
                            //    {
                            //        tempColliders.Add(hitColliders[k]);
                            //    }
                            //}
                            //Swirling(hitColliders);
                        }
                    }

                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        spinningAnim = false;
                        leftOk = true;
                        temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                        startPointAtt = new Vector3(temp.x, 0, temp.z);
                        swipeToHit = false; //cannot hit on click only!!!!
                    }
                    else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        leftOk = false;
                        temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                        dragPointAtt = new Vector3(temp.x, 0, temp.z);
                        attackDist = Vector3.Distance(startPointAtt, dragPointAtt);
                        //print(attackDist);
                        if (attackDist > 1f)
                        {
                            attackDir = dragPointAtt - startPointAtt;
                            var localDir = -transform.InverseTransformDirection(attackDir.normalized);
                            var angle = Vector3.Angle(-Vector3.right, localDir);
                            var cross = Vector3.Cross(-Vector3.right, localDir).normalized;
                            if(cross.y < 0)
                            {
                                angle = -angle;
                            }
                            //print(360 - Vector3.Angle(transform.forward, localDir));
                            //print(angle);
                            Debug.DrawRay(transform.position, attackDir * 1f, Color.blue, 5f);
                            Debug.DrawRay(transform.position, cross * 5f, Color.green, 5f);
                            if (spinningAnim == false && angle < 0)
                            {
                                //transform.rotation = Quaternion.LookRotation(attackDir);
                                PlayerStates.swiped = true;
                                StartCoroutine("SwipeTimer");
                            }

                            ///HIT ANIMATION
                            if (spinningAnim == false && swipeToHit == true && angle < 0)
                            {
                                GameManager.instance.playerHitObject();
                            }
                        }
                    }

                    else if (Input.GetTouch(i).phase == TouchPhase.Moved)
                    {
                        swipeToHit = true; //only hit if player move finger to swipe
                    }
                }
            }
        }
        if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME || GameManager.instance.TutorialState() == GameManager.Tutorial.STOMP)
        {
            if (Input.touchCount == 2)
            {
                print("Same touch");
                print("RightOk:" + rightOk + " " + "LeftOk:" + leftOk + " " + "ableToLift:" + ableToLift);
                //print(powerTime);

                if (powerTime < GetComponent<PlayerStates>().SameTapTime && ableToLift && rightOk && leftOk)
                {
                    ableToLift = false;
                    intoAir = true;
                    stompTut = true;
                    if(GameManager.instance.TutorialState() == GameManager.Tutorial.STOMP)
                    {
                        GetComponent<PlayerStates>().rageObjects = 50;
                        GetComponent<StampBar>().slider.value = 0f;
                    }
                    PlayerStates.imInSlowMotion = true;
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().liftRadius);
                    //Lift(hitColliders); //RUN FROM ANIMATION EVENT
                    {
                        for (int i = 0; i < hitColliders.Length; i++)
                        {
                            if (hitColliders[i].tag == "Destructable")
                            {
                                tempColliders.Add(hitColliders[i]);
                            }
                        }
                        // SOUND AND ANIMATION FOR STOMP
                        GameManager.instance.playerStomp();
				GameManager.instance.changeMusicState(AudioManager.IN_STOMP);  // FOR AUDIO
                    }
                }
            }
        }

        if (Input.touchCount == 0 || Input.touchCount > 2)
        {
            powerTime = 0;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * GetComponent<PlayerStates>().rotationSpeed);
    }

    IEnumerator SwipeTimer()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        PlayerStates.swiped = false;
    }

    public void Swirling() // hit needs to become true here
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().swirlRadius);
        for (int k = 0; k < hitColliders.Length; k++)
        {
            if (hitColliders[k].tag == "Destructable")
            {
                tempColliders.Add(hitColliders[k]);
            }
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            Reverse(objRB, initialMass);
        }
        for (int i = 0; i < tempColliders.Count; i++)
        {
            if (tempColliders[i].tag == "Destructable" && tempColliders[i] != null)
            {
                //HERE, DECTED THAT CAN HIT SOMETHING WITH SWIRLING, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RESTRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
                Rigidbody rig = tempColliders[i].GetComponent<Rigidbody>();
                Vector3 dir = tempColliders[i].transform.position - transform.position;
                tempColliders[i].GetComponent<ObjectBehavior>().hit = true;

                // SOUND OBJECT HIT
                GameManager.instance.objectHit(tempColliders[i].gameObject);
                // ANIMATION OBJECT HIT
                GameManager.instance.playerHitObject();

                // PLAY DAMAGE PARTICLE
                //rig.GetComponent<ObjectBehavior>().particleSys.Play(); /////////IT WILL GIVE AN ERROR IN THE LEVELS WITHOUT THE FRACTURED OBJECTS

                if (rig.GetComponent<ObjectBehavior>().lifted)
                {
                    var tempDir = new Vector3(dir.x, 0.0f, dir.z);
                    rig.AddForce((tempDir.normalized) * GetComponent<PlayerStates>().swirlForce, ForceMode.Impulse);
                }
                else
                {
                    rig.AddForce(dir.normalized * GetComponent<PlayerStates>().swirlForce, ForceMode.Impulse);
                }
                if (ObjectManagerV2.instance.canDamage == true)
                {
                    tempColliders[i].gameObject.GetComponent<ObjectBehavior>().life -= ObjectManagerV2.instance.swirlDamage;
                }
            }
        }

    }

    public void Lift()
    {
        //inAir = true;
        for (int i = 0; i < tempColliders.Count; i++)
        {
            if (tempColliders[i].tag == "Destructable" && tempColliders[i] != null)
            {

                tempColliders[i].GetComponent<ObjectBehavior>().lifted = true;
                tempColliders[i].GetComponent<ObjectBehavior>().flying = true;
                //HERE, DECTED THAT CAN HIT SOMETHING WITH LIFT, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RESTRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
                objRB.Add(tempColliders[i].GetComponent<Rigidbody>());
                initialMass.Add(tempColliders[i].GetComponent<Rigidbody>().mass);
                tempColliders[i].GetComponent<Rigidbody>().mass = 10f;
                tempColliders[i].GetComponent<Rigidbody>().AddForce(Vector3.up * GetComponent<PlayerStates>().liftForce);
                tempColliders[i].gameObject.GetComponent<ObjectBehavior>().hasLanded = false; //THIS HAS AN ERROR
            }
        }



        coroutine = ReturnGravity(objRB, initialMass);
        StartCoroutine(coroutine);
    }

    IEnumerator ReturnGravity(List<Rigidbody> rig, List<float> mass)
    {
        yield return new WaitForSeconds(GetComponent<PlayerStates>().gravityTimer);
        Reverse(rig, mass);
    }

    public void Reverse(List<Rigidbody> rig, List<float> mass)
    {
        PlayerStates.imInSlowMotion = false;
        StampBar.increaseFill = true;
        //inAir = false;
        for (int i = 0; i < rig.Count; i++)
        {
            if (rig[i] != null)
            {
                //[i].mass = mass[i];    //This needs to happen after a short period of time.
                rig[i].isKinematic = false;
                rig[i].GetComponent<ObjectBehavior>().slowed = false;
                rig[i].GetComponent<ObjectBehavior>().flying = false;
            }
        }
        //objRB.Clear();
        //initialMass.Clear();   We need to clear the lists after we changed the mass back to normal and that happens after a short period of time.
        StartCoroutine(InitializeMass(rig, mass));
        coroutine = null;
		GameManager.instance.changeMusicState(AudioManager.IN_LEVEL);  // FOR AUDIO, reverse from stomp
    }

    IEnumerator InitializeMass(List<Rigidbody> rig, List<float> mass)
    {
        yield return new WaitForSeconds(GetComponent<PlayerStates>().resetMassTimer);
        for (int i = 0; i < rig.Count; i++)
        {
            if (rig[i] != null)
            {
                rig[i].mass = mass[i];
            }
        }
        objRB.Clear();
        initialMass.Clear();
    }

    float CubicBezier(float t)
    {
        return ((1 - t * t * t) * GetComponent<PlayerStates>().p0) + (3 * (1 - t * t) * t * GetComponent<PlayerStates>().p1) + (3 * (1 - t) * t * t * GetComponent<PlayerStates>().p2) + (t * t * t * GetComponent<PlayerStates>().p3);
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
                if (gestureDetector.Count == 0 || (p - gestureDetector[gestureDetector.Count - 1]).magnitude > 10) //Default was 10 (make it 5 for the arc)
                {
                    gestureDetector.Add(p);
                }
            }
        }

        if (gestureDetector.Count < 10) //Default was 10 (make it 5 for the arc)
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

        if (gestureLength > gestureBase && gestureSum.magnitude < gestureBase / 2) //gestureBase need to be divided by 2 for an arc
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