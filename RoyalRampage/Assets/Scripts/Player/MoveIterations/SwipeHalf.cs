using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwipeHalf : MonoBehaviour
{

    private List<float> initialMass = new List<float>();
    private int touches, countTaps;
    private float distance, attackDist, moveTimer, circleTimer, speed, acc, force, powerTime, tapsTimer, time;
    private bool newSwipe, applyMove, startTimer, doingCircle, rotationTime;
    private bool newDash = false;   // for dashsound
    private Vector3 temp, startPoint, startPointAtt, dragPoint, dragPointAtt, direction, firstTouch, secondTouch;
    private Rigidbody playerRig;

    [HideInInspector]
    public static bool ableToLift, intoAir;
    [HideInInspector]
    public List<Rigidbody> objRB = new List<Rigidbody>();
    [HideInInspector]
    public static Vector3 attackDir;


    void Start()
    {

        playerRig = GetComponent<Rigidbody>();
        touches = 0;
        moveTimer = 0;
        newSwipe = false;
        applyMove = false;
        direction = -Vector3.forward;
    }


    void FixedUpdate()
    {
        if (applyMove)
        {
            float highForce = GameManager.instance.player.GetComponent<PlayerStates>().maxVelocity;

            force = force * CubicBezier(moveTimer);
            float playerVelocity = playerRig.mass * (playerRig.velocity.magnitude * playerRig.velocity.magnitude) / 2;

            //print("force: " + force);
            playerRig.AddForce(direction.normalized * force);
            transform.rotation = Quaternion.LookRotation(direction);
            float maxForce = (playerRig.mass * (highForce * highForce)) / 2;

            //print("player velocity: " + playerVelocity);
            //print("max: " + maxForce);

            if (playerVelocity > maxForce)
            {

                var difForce = force - maxForce;
                //print("diff: " + difForce);
                playerRig.AddForce(-direction.normalized * difForce);
            }

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
        //print(Mathf.Round(playerRig.velocity.magnitude));

        moveTimer += Time.deltaTime;
        circleTimer += Time.deltaTime;

        touches = Input.touchCount;

        if (touches > 1)
        {
            touches = 1;
        }

        for (int i = 0; i < touches; i++)
        {
            powerTime += Time.deltaTime;

            if (Input.GetTouch(i).position.x <= Screen.width / 2)
            {
                //HERE MOVING////////////////////////
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    newSwipe = true;
                    newDash = true;
                    moveTimer = 0;
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    startPoint = new Vector3(temp.x, 0, temp.z);
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Moved && newSwipe)
                {
                    PlayerStates.imInSlowMotion = false;
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    dragPoint = new Vector3(temp.x, 0, temp.z);
                    distance = Vector3.Distance(dragPoint, startPoint);
                    direction = dragPoint - startPoint;
                    speed = distance / moveTimer;
                    acc = speed / moveTimer;
                    force = playerRig.mass * acc;
                    if (distance > GetComponent<PlayerStates>().distSwipe)
                    {
                        if (Mathf.Round(playerRig.velocity.magnitude) == 0)
                        {
                            applyMove = true;
                            //rotationTime = true;
                        }
                    }

                    if (moveTimer >= GetComponent<PlayerStates>().timeForSwipe && newSwipe)
                    {
                        applyMove = false;
                        newSwipe = false;
                    }
                }
            }
            else if (Input.GetTouch(i).position.x > Screen.width / 2)
            {
                //HERE ATTACKING////////////////////
                if (isGestureDone())
                {
                    //IF WE NEED TO SEE SWIRLING ANIMATION WHEN YOU DO A CIRCLE GESTURE EVEN IF WE ARE NOT ABLE TO HIT SOMETHING, THEN NEEDS TO BE HERE.
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().swirlRadius);
                    Swirling(hitColliders);
                }

                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    startPointAtt = new Vector3(temp.x, 0, temp.z);
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.farClipPlane));
                    dragPointAtt = new Vector3(temp.x, 0, temp.z);
                    attackDist = Vector3.Distance(startPointAtt, dragPointAtt);
                    attackDir = dragPointAtt - startPointAtt;
                    transform.rotation = Quaternion.LookRotation(attackDir);
                    PlayerStates.swiped = true;
                    StartCoroutine("SwipeTimer");
                    //Dash(attackDir);
                    //Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().dashRadius);
                    //Dash2(hitColliders);
                }

            }
        }

        if (Input.touchCount == 2)
        {
            if (powerTime < GetComponent<PlayerStates>().SameTapTime && ableToLift)
            {
                //print("I am here");
                ableToLift = false;
                PlayerStates.lifted = true;
                intoAir = true;
                PlayerStates.imInSlowMotion = true;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<PlayerStates>().liftRadius);
                Lift(hitColliders);
            }
        }

        if (Input.touchCount == 0 || Input.touchCount > 2)
        {
            powerTime = 0;
        }

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * GetComponent<PlayerStates>().rotationSpeed);
    }


    public void RotateObjs(List<Rigidbody> rig)
    {
        for (int i = 0; i < rig.Count; i++)
        {
            if (rig[i] != null)
            {
                rig[i].transform.Rotate(Vector3.up, GetComponent<PlayerStates>().torgueForce);
            }
        }
    }

    IEnumerator SwipeTimer()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        PlayerStates.swiped = false;
    }

    //void Dash2(Collider[] col)
    //{
    //    for (int i = 0; i < col.Length; i++)
    //    {
    //        if (col[i].tag == "Destructable")
    //        {

    //        }
    //    }
    //}

    //void Dash(Vector3 direction) //Hit needs to become true here
    //{
    //    bool decreaseHp = true;
    //    for (float i = transform.position.x - 1f; i < transform.position.x + 1f; i += 0.1f)
    //    {
    //        Ray ray = new Ray(new Vector3(i, transform.position.y, transform.position.z), attackDir.normalized);
    //        Debug.DrawRay(new Vector3(i, transform.position.y, transform.position.z), attackDir.normalized * GetComponent<PlayerStates>().attackRange, Color.red, 2f);
    //        RaycastHit hit;
    //        Physics.Raycast(ray, out hit, GetComponent<PlayerStates>().attackRange);
    //        if (hit.collider != null && hit.collider.tag == "Destructable")
    //        {
    //            Rigidbody rig = hit.collider.GetComponent<Rigidbody>();
    //            rig.isKinematic = false;
    //            hit.collider.GetComponent<ObjectBehavior>().hit = true;
    //            if (PlayerStates.lifted)
    //            {
    //                rig.AddForce((attackDir.normalized + new Vector3(0, GetComponent<PlayerStates>().degreesInAir / 90, 0)) * GetComponent<PlayerStates>().hitForce);
    //            }
    //            else
    //            {
    //                rig.AddForce(attackDir.normalized * GetComponent<PlayerStates>().hitForce);
    //            }
    //            if (decreaseHp)
    //            {
    //                hit.collider.GetComponent<ObjectBehavior>().life -= ObjectManagerV2.instance.dashDamage;
    //                decreaseHp = false;
    //            }
    //        }
    //    }
    //}

    void Swirling(Collider[] col) // hit needs to become true here
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].tag == "Destructable")
            {
                //HERE, DECTED THAT CAN HIT SOMETHING WITH SWIRLING, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RESTRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
                Rigidbody rig = col[i].GetComponent<Rigidbody>();
                Vector3 dir = col[i].transform.position - transform.position;
                col[i].GetComponent<ObjectBehavior>().hit = true;

                // SOUND OBJECT HIT
                GameManager.instance.objectHit(col[i].gameObject);
                // ANIMATION OBJECT HIT
                GameManager.instance.playerHitObject();

                //rig.useGravity = true;
                rig.isKinematic = false;
                if (PlayerStates.lifted)
                {
                    rig.AddForce((dir.normalized + new Vector3(0, GetComponent<PlayerStates>().degreesInAir / 90, 0)) * GetComponent<PlayerStates>().swirlForce);
                }
                else
                {
                    rig.AddForce(dir.normalized * GetComponent<PlayerStates>().swirlForce);
                }
                col[i].gameObject.GetComponent<ObjectBehavior>().life -= ObjectManagerV2.instance.swirlDamage;
            }
        }
        GameManager.instance.playerSwirl();
    }


    void Lift(Collider[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].tag == "Destructable")
            {
                //HERE, DECTED THAT CAN HIT SOMETHING WITH LIFT, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RESTRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
                objRB.Add(col[i].GetComponent<Rigidbody>());
                initialMass.Add(col[i].GetComponent<Rigidbody>().mass);
                col[i].GetComponent<Rigidbody>().mass = 0.1f;
                col[i].GetComponent<Rigidbody>().AddForce(Vector3.up * GetComponent<PlayerStates>().liftForce);
                //rig.AddTorque(Vector3.up * GetComponent<PlayerStates>().torgueForce);
                col[i].gameObject.GetComponent<ObjectBehavior>().hasLanded = false; //THIS HAS AN ERROR
            }
        }

        // SOUND AND ANIMATION FOR STOMP
        GameManager.instance.playerStomp();
        StartCoroutine(ReturnGravity(objRB, initialMass));
    }

    IEnumerator ReturnGravity(List<Rigidbody> rig, List<float> mass)
    {
        //print("I am into Coroutine");
        yield return new WaitForSeconds(GetComponent<PlayerStates>().gravityTimer);
        PlayerStates.lifted = false;
        PlayerStates.imInSlowMotion = false;
        StampBar.increaseFill = true;
        for (int i = 0; i < rig.Count; i++)
        {
            if (rig[i] != null)
            {
                rig[i].mass = mass[i];
                //objRB.useGravity = true;
                rig[i].isKinematic = false;
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
                if (gestureDetector.Count == 0 || (p - gestureDetector[gestureDetector.Count - 1]).magnitude > 10) //Default was 10
                {
                    gestureDetector.Add(p);
                }
            }
        }

        if (gestureDetector.Count < 10) //Default was 10
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

        if (gestureLength > gestureBase && gestureSum.magnitude < gestureBase / 2) //gestureBase divided by 2 for a half arc
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