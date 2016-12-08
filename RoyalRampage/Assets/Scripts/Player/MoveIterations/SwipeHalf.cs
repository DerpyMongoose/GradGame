using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwipeHalf : MonoBehaviour
{
    public static bool startTutTimer;

    private int touches, sameTapCount;
    private float distance, attackDist, moveTimer, circleTimer, speed, force, powerTime, startingMass;
    private bool newSwipe, applyMove, rightOk, leftOk, startSameTouchTimer;
    private bool newDash;
    private Vector3 temp, startPoint, startPointAtt, dragPoint, dragPointAtt, direction;
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

    private bool spinningAnim;

    void Start()
    {
        newDash = false;
        spinningAnim = false;
        leftOk = false;
        rightOk = false;
        swirlTut = false;
        stompTut = false;
        ableToLift = false;
        intoAir = false;
        playerRig = GetComponent<Rigidbody>();
        startingMass = playerRig.mass;
        touches = 0;
        sameTapCount = 0;
        moveTimer = 0;
        circleTimer = 0;
        powerTime = 0;
        startSameTouchTimer = false;
        newSwipe = false;
        applyMove = false;
        direction = -Vector3.forward;
        swirlEnded = true;
        PlayerStates.swiped = false;
        playerRig.mass = GetComponent<PlayerStates>().becomeHeavy;
    }


    void FixedUpdate()
    {
        if (applyMove)
        {
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
        //Debug.DrawRay(transform.position, -transform.right * 1f, Color.red, 2f);
        moveTimer += Time.deltaTime;
        circleTimer += Time.deltaTime;

        if (powerTime >= GetComponent<PlayerStates>().SameTapTime || Input.touchCount > 2)
        {
            sameTapCount = 0;
            startSameTouchTimer = false;
            powerTime = 0;
        }

        touches = Input.touchCount;

        if (touches > 2)
        {
            touches = 2;
        }

        for (int i = 0; i < touches; i++)
        {
            if (Input.GetTouch(i).position.x <= Screen.width / 2)
            {
                if (GameManager.instance.TutorialState() == GameManager.Tutorial.STOMP && GameManager.instance.CurrentScene() == GameManager.Scene.TUTORIAL)
                {
                    leftOk = true;
                }
                //HERE MOVING////////////////////////
                if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME || GameManager.instance.TutorialState() == GameManager.Tutorial.MOVEMENT)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        leftOk = true;
                        newSwipe = true;
                        newDash = true;
                        moveTimer = 0;
                        playerRig.mass = startingMass;
                        StartCoroutine(BringBackMass());
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
                        direction = dragPoint - startPoint;
                        speed = distance;
                        force = playerRig.mass * (speed * GetComponent<PlayerStates>().moveForce);
                        if (distance > GetComponent<PlayerStates>().distSwipe && distance <= GetComponent<PlayerStates>().maxDistSwipe && playerRig.mass <= startingMass)
                        {
                            applyMove = true;
                        }

                        if (moveTimer >= GetComponent<PlayerStates>().timeForSwipe && newSwipe)
                        {
                            applyMove = false;
                            newSwipe = false;
                        }
                    }
                    else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        leftOk = false;
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
                            if (GameManager.instance.levelManager.wrongMove == false)
                            {
                                swirlTut = true;
                                swirlEnded = false;
                                GameManager.instance.playerSwirl();
                                spinningAnim = true;
                            }
                        }
                    }

                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        rightOk = true;
                        temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                        startPointAtt = new Vector3(temp.x, 0, temp.z);
                    }
                    else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        rightOk = false;
                        temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                        dragPointAtt = new Vector3(temp.x, 0, temp.z);
                        attackDist = Vector3.Distance(startPointAtt, dragPointAtt);
                        if (attackDist > 1f)
                        {
                            attackDir = dragPointAtt - startPointAtt;
                            var localDir = -transform.InverseTransformDirection(attackDir.normalized);
                            var angle = Vector3.Angle(-Vector3.right, localDir);
                            var cross = Vector3.Cross(-Vector3.right, localDir).normalized;
                            if (cross.y < 0)
                            {
                                angle = -angle;
                            }
                            //Debug.DrawRay(transform.position, attackDir * 1f, Color.blue, 5f);
                            //Debug.DrawRay(transform.position, cross * 5f, Color.green, 5f);
                            if (spinningAnim == false && angle < 0)
                            {
                                //transform.rotation = Quaternion.LookRotation(attackDir);
                                GameManager.instance.playerHitObject();
                                PlayerStates.swiped = true;
                                StartCoroutine("SwipeTimer");
                            }
                            spinningAnim = false;
                        }
                    }
                }
            }
        }
        if (GameManager.instance.CurrentScene() == GameManager.Scene.GAME || GameManager.instance.TutorialState() == GameManager.Tutorial.STOMP)
        {
            if (Input.touchCount == 2)
            {
                if (rightOk && leftOk)
                {
                    startSameTouchTimer = true;
                    sameTapCount++;
                    rightOk = false;
                    leftOk = false;
                }

                if (powerTime < GetComponent<PlayerStates>().SameTapTime && ableToLift && sameTapCount == 2)
                {
                    ableToLift = false;
                    intoAir = true;
                    stompTut = true;
                    if (GameManager.instance.TutorialState() == GameManager.Tutorial.STOMP)
                    {
                        GetComponent<PlayerStates>().rageObjects = 50;
                        GetComponent<StampBar>().slider.value = 0f;
                        ObjectManagerV2.instance.isGrounded = false;
                    }
                    PlayerStates.imInSlowMotion = true;
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().liftRadius);
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

        if (startSameTouchTimer)
        {
            powerTime += Time.deltaTime;
        }

        if (GameManager.instance.currentScene == GameManager.Scene.GAME || GameManager.instance.tutorial == GameManager.Tutorial.MOVEMENT)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * GetComponent<PlayerStates>().rotationSpeed);
        }


    }

    IEnumerator BringBackMass()
    {
        yield return new WaitForSeconds(GetComponent<PlayerStates>().timeForSwipe);
        playerRig.mass = GetComponent<PlayerStates>().becomeHeavy;
    }

    IEnumerator SwipeTimer()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        PlayerStates.swiped = false;
    }

    public void Swirling()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().swirlRadius);
        for (int k = 0; k < hitColliders.Length; k++)
        {
            if (hitColliders[k].tag == "Destructable" || hitColliders[k].tag == "UniqueObjs")
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
            if (tempColliders[i] != null)
            {
                Rigidbody rig = tempColliders[i].GetComponent<Rigidbody>();
                Vector3 dir = tempColliders[i].transform.position - transform.position;
                tempColliders[i].GetComponent<ObjectBehavior>().hit = true;

                // SOUND OBJECT HIT
                GameManager.instance.objectHit(tempColliders[i].gameObject);

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
                    if (tempColliders[i].gameObject.GetComponent<ObjectBehavior>().life <= 0)
                    {
                        ObjectManagerV2.instance.countObjects++;
                        //ObjectManagerV2.instance.countMultiTime = 0;
                    }
                }
            }
        }

    }

    public void Lift()
    {
        for (int i = 0; i < tempColliders.Count; i++)
        {
            if (tempColliders[i] != null)
            {

                tempColliders[i].GetComponent<ObjectBehavior>().lifted = true;
                tempColliders[i].GetComponent<ObjectBehavior>().flying = true;
                tempColliders[i].GetComponent<ObjectBehavior>().canRotate = true;
                tempColliders[i].GetComponent<ObjectBehavior>().score  = tempColliders[i].GetComponent<ObjectBehavior>().score + ObjectManagerV2.instance.bonusScore;
                objRB.Add(tempColliders[i].GetComponent<Rigidbody>());
                initialMass.Add(tempColliders[i].GetComponent<Rigidbody>().mass);
                tempColliders[i].GetComponent<Rigidbody>().mass = 10f;
                tempColliders[i].GetComponent<Rigidbody>().AddForce(Vector3.up * GetComponent<PlayerStates>().liftForce);
                tempColliders[i].gameObject.GetComponent<ObjectBehavior>().hasLanded = false;
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
        for (int i = 0; i < rig.Count; i++)
        {
            if (rig[i] != null)
            {
                rig[i].isKinematic = false;
                rig[i].GetComponent<ObjectBehavior>().slowed = false;
                rig[i].GetComponent<ObjectBehavior>().flying = false;
                rig[i].GetComponent<ObjectBehavior>().canRotate = false;
            }
        }
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
                    circleTimer = 0;
                    return true;
                }
                else
                {
                    circleTimer = 0;
                }
            }
        }

        return false;
    }
}