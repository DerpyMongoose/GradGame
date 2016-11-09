using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveAttack : MonoBehaviour
{

    private Rigidbody playerRG;
    private Vector3 direction, startPoint, endPoint, dragpoint, temp;
    private float magnitude, powerTime;
    private bool dragged, beginCount, onlyOneTouch, ableToLift, circleTime;
    private string place;

    public Collider floor, Nwall, Ewall, Wwall, Swall;
    public float moveForce, hitForce, swirlForce, cdLift, doubleTapTime, collisionRadius;
    public int numOfCircleToShow;


    void Start()
    {
        playerRG = GetComponent<Rigidbody>();
        ableToLift = true;
    }


    void FixedUpdate()
    {

        if (beginCount)
        {
            powerTime += Time.deltaTime;
        }

        if (Input.touchCount == 1)
        {
            place = null;

            if (isGestureDone())
            {
                print("I made a circle while touches 1");
                //IF WE NEED TO SEE SWIRLING ANIMATION WHEN YOU DO A CIRCLE GESTURE EVEN IF WE ARE NOT ABLE TO HIT SOMETHING, THEN NEEDS TO BE HERE.
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRadius);
                Swirling(hitColliders);
                circleTime = true;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                onlyOneTouch = true;
                circleTime = false;
                temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.farClipPlane));
                startPoint = new Vector3(temp.x, 0, temp.z);
            }


            if (Input.GetTouch(0).phase == TouchPhase.Moved && onlyOneTouch)
            {
                temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.farClipPlane));
                dragpoint = new Vector3(temp.x, 0, temp.z);
                Vector3 dir = dragpoint - startPoint;

                //this number 40 is for how far you need to swipe before you actually register that you swiped.
                if (dir.magnitude > 2f)
                {
                    dragged = true;
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended && !circleTime)
            {
                if (dragged)
                {
                print("I reached here");
                    temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.farClipPlane));
                    endPoint = new Vector3(temp.x, 0, temp.z);

                    direction = endPoint - startPoint;
                    magnitude = direction.magnitude;
                    playerRG.AddForce(direction.normalized * magnitude * moveForce);
                    transform.rotation = Quaternion.LookRotation(direction);
                    //PLAY MOVE ANIMATION, I AM THINKING OF DIFFERENT WALK-JOKING-RUNNING ANIMATION THAT WE CAN DIFFERENTIATE ACCORDING TO DIRECTION'S MAGNITUDE.
                    dragged = false;
                }
            }
        }
        else if (Input.touchCount == 2)
        {
            onlyOneTouch = false;

            if (isGestureDone())
            {
                print("I made a circle while touches 2");
                //IF WE NEED TO SEE SWIRLING ANIMATION WHEN YOU DO A CIRCLE GESTURE EVEN IF WE ARE NOT ABLE TO HIT SOMETHING, THEN NEEDS TO BE HERE.
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRadius);
                Swirling(hitColliders);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                powerTime = 0;
                beginCount = true;

                if (Input.GetTouch(0).position.x <= Screen.width / 2)
                {
                    place = "left";
                }
                else
                {
                    place = "right";
                }
            }


            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(1).position.x, Input.GetTouch(1).position.y, Camera.main.farClipPlane));

                startPoint = new Vector3(temp.x, 0, temp.z);

                if (powerTime <= doubleTapTime && ableToLift)
                {
                    switch (place)
                    {
                        case "left":
                            if (Input.GetTouch(1).position.x > Screen.width / 2)
                            {
                                GameManager.instance.TimeToLift();
                                /////////////////////////////////////////////////////////////////PLAY STAMP ANIMATION
                                ableToLift = false;
                                StartCoroutine("Cooldown");
                            }
                            break;

                        case "right":
                            if (Input.GetTouch(1).position.x <= Screen.width / 2)
                            {
                                GameManager.instance.TimeToLift();
                                //////////////////////////////////////////////////////////////////PLAY STAMP ANIMATION
                                ableToLift = false;
                                StartCoroutine("Cooldown");
                            }
                            break;
                    }
                }
            }
        }
        else if (Input.touchCount < 1)
        {
            beginCount = false;
            powerTime = 0;
        }
    }


    void Swirling(Collider[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i] != floor && col[i] != Nwall && col[i] != Ewall && col[i] != Wwall && col[i] != Swall && col[i] != GetComponent<Collider>())
            {
                //HERE, DECTED THAT CAN HIT SOMETHING WITH SWIRLING, SO PLAY SWIRLING ANIMATION BUT NEED TO BE RETRICTED HOW MANY TIMES TO PLAY THE ANIM BECAUSE IT IS A LOOP AND PROBABLY IT IS GOING TO OVERIDE.
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
        if (col.collider != floor && col.collider != Nwall && col.collider != Ewall && col.collider != Wwall && col.collider != Swall)
        {
            Rigidbody rig = col.collider.GetComponent<Rigidbody>();

            rig.AddForce(direction.normalized * hitForce);
        }
    }



    List<Vector2> gestureDetector = new List<Vector2>();
    Vector2 gestureSum = Vector2.zero;
    float gestureLength = 0;
    int gestureCount = 0;

    bool isGestureDone()
    {
        if (Input.touches.Length > 2)
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
