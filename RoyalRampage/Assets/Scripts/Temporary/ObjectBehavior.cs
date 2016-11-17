using UnityEngine;
using System.Collections;

public class ObjectBehavior : MonoBehaviour
{
    private GameObject rubblePrefab;
    //[HideInInspector]
    public int life, initialLife;
    private int rubbleAmount;
	private int state;

    public string soundSwitch; // FOR AUDIO

    private Vector3 initialPos;

    private Rigidbody objRB;
    private GameObject player;
    private ParticleSystem particleSys;
    private IEnumerator coroutine;

    [HideInInspector]
    public bool isGrounded = true;
    [HideInInspector]
    public bool hit = false;
    private bool readyToCheck;
    private bool lifted;

    public bool hasLanded = true;

    private float checkHeight, initialMass;

    [HideInInspector]
    public int score;


    //object name to be used for Quest system??
    public enum DestructableMaterial
    {
        GLASS, WOOD, STONE, METAL
    }

    public enum DestructableSize
    {
        SMALL, MEDIUM, LARGE
    }

    public DestructableMaterial objMaterial;
    public DestructableSize objSize;
    // Use this for initialization
    //void OnEnable()
    //{
    //    GameManager.stampPower += Lift;
    //}

    //void OnDisable()
    //{
    //    GameManager.stampPower -= Lift;
    //}

    void Start()
    {
        rubblePrefab = ObjectManagerV2.instance.rubblePrefab;
        switch (objMaterial)
        {
            case DestructableMaterial.GLASS:
                switch (objSize)
                {
                    case DestructableSize.SMALL:
                        score = ObjectManagerV2.instance.smallGlassScore;
                        life = ObjectManagerV2.instance.smallGlassLife;
                        rubbleAmount = ObjectManagerV2.instance.smallGlassRubbleAmount;
                        break;
                    case DestructableSize.MEDIUM:
                        score = ObjectManagerV2.instance.mediumGlassScore;
                        life = ObjectManagerV2.instance.mediumGlassLife;
                        rubbleAmount = ObjectManagerV2.instance.mediumGlassRubbleAmount;
                        break;
                    case DestructableSize.LARGE:
                        score = ObjectManagerV2.instance.largeGlassScore;
                        life = ObjectManagerV2.instance.largeGlassLife;
                        rubbleAmount = ObjectManagerV2.instance.largeGlassRubbleAmount;
                        break;
                    default:
                        break;
                }
                break;
            case DestructableMaterial.WOOD:
                switch (objSize)
                {
                    case DestructableSize.SMALL:
                        score = ObjectManagerV2.instance.smallWoodScore;
                        life = ObjectManagerV2.instance.smallWoodLife;
                        rubbleAmount = ObjectManagerV2.instance.smallWoodRubbleAmount;
                        break;
                    case DestructableSize.MEDIUM:
                        score = ObjectManagerV2.instance.mediumWoodScore;
                        life = ObjectManagerV2.instance.mediumWoodLife;
                        rubbleAmount = ObjectManagerV2.instance.mediumWoodRubbleAmount;
                        break;
                    case DestructableSize.LARGE:
                        score = ObjectManagerV2.instance.largeWoodScore;
                        life = ObjectManagerV2.instance.largeWoodLife;
                        rubbleAmount = ObjectManagerV2.instance.largeWoodRubbleAmount;
                        break;
                    default:
                        break;
                }
                break;
            case DestructableMaterial.STONE:
                switch (objSize)
                {
                    case DestructableSize.SMALL:
                        score = ObjectManagerV2.instance.smallStoneScore;
                        life = ObjectManagerV2.instance.smallStoneLife;
                        rubbleAmount = ObjectManagerV2.instance.smallStoneRubbleAmount;
                        break;
                    case DestructableSize.MEDIUM:
                        score = ObjectManagerV2.instance.mediumStoneScore;
                        life = ObjectManagerV2.instance.mediumStoneLife;
                        rubbleAmount = ObjectManagerV2.instance.mediumStoneRubbleAmount;
                        break;
                    case DestructableSize.LARGE:
                        score = ObjectManagerV2.instance.largeStoneScore;
                        life = ObjectManagerV2.instance.largeStoneLife;
                        rubbleAmount = ObjectManagerV2.instance.largeStoneRubbleAmount;
                        break;
                    default:
                        break;
                }
                break;
            case DestructableMaterial.METAL:
                switch (objSize)
                {
                    case DestructableSize.SMALL:
                        score = ObjectManagerV2.instance.smallMetalScore;
                        life = ObjectManagerV2.instance.smallMetalLife;
                        rubbleAmount = ObjectManagerV2.instance.smallMetalRubbleAmount;
                        break;
                    case DestructableSize.MEDIUM:
                        score = ObjectManagerV2.instance.mediumMetalScore;
                        life = ObjectManagerV2.instance.mediumMetalLife;
                        rubbleAmount = ObjectManagerV2.instance.mediumMetalRubbleAmount;
                        break;
                    case DestructableSize.LARGE:
                        score = ObjectManagerV2.instance.largeMetalScore;
                        life = ObjectManagerV2.instance.largeMetalLife;
                        rubbleAmount = ObjectManagerV2.instance.largeMetalRubbleAmount;
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;

        }
        state = 1;
        initialLife = life;
        //particleSys = GetComponent<ParticleSystem>(); //////////////DO WE STILL NEED THESE PARTICLES?????
        //particleSys.Stop();
        objRB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        initialPos = transform.position;
    }


    void Update()
    {
        CheckDamage();
        CheckVelocity();
        //print(PlayerStates.lifted);
        if (gameObject.tag != "UniqueObjs")
        {
            if (PlayerStates.lifted)
            {
                //APPLY TRANSFORM ROTATION
                //GameManager.instance.player.GetComponent<PhysicalMovement>().RotateObjs(GameManager.instance.player.GetComponent<PhysicalMovement>().objRB);
                GameManager.instance.player.GetComponent<SwipeHalf>().RotateObjs(GameManager.instance.player.GetComponent<SwipeHalf>().objRB);

                if (Mathf.Round(objRB.velocity.y * 10) / 10 < 0 && PlayerStates.imInSlowMotion)
                {
                    //objRB.useGravity = false;
                    objRB.isKinematic = true;
                }
                else
                {
                    //objRB.useGravity = true;
                    objRB.isKinematic = false;
                }
            }

            if (!Mathf.Approximately(initialPos.y, transform.position.y))
            {
                isGrounded = false;
            }
        }
    }

    void DestroyObj(GameObject obj)
    {
        for (int i = 0; i < obj.GetComponent<ObjectBehavior>().rubbleAmount; i++)
        {
            Instantiate(obj.GetComponent<ObjectBehavior>().rubblePrefab, obj.transform.position, Quaternion.identity);
        }
        GameManager.instance.objectDestructed(obj);
        Destroy(obj);
    }



    void CheckDamage()
    {
        if (life <= 0)
        {
            DestroyObj(gameObject);
        }
        else if (life < initialLife)
        {
            //particleSys.Play();
        }

    }

    void CheckVelocity()
    {
        if(gameObject.tag != "UniqueObjs" && objRB.velocity.magnitude == 0)
        {
            hit = false;
        }
    }



    void OnCollisionEnter(Collision col)
    {
        //print(col.relativeVelocity.magnitude);
        if (col.relativeVelocity.magnitude > ObjectManagerV2.instance.colImpact)
        {
            if (col.collider.gameObject == player)
            {
                //hit = true; //Hit true will become when you actually swiped the object.

                // SOUND OBJECT HIT
                GameManager.instance.objectHit(gameObject);

                //Damage system, it takes more hits to destroy
                /*if(state == (life - life) + state)
                {
                    if (state >= life)
                    {           
                        for (int i = 0; i < rubbleAmount; i++)
                        {
                            Instantiate(rubblePrefab,transform.position, Quaternion.identity);
                        }
                        if()
                        {

                        }
                    }*/
                if (particleSys != null)
                {
                    particleSys.startSize *= state;
                    particleSys.Play();
                }
                /*state += 1;
            }*/
            }
            if ((col.collider.tag == "Destructable" || col.collider.tag == "UniqueObjs") && hit == true)
            {
                col.collider.GetComponent<ObjectBehavior>().hit = true;
                //if (isGrounded == false && col.gameObject.GetComponent<ObjectBehavior>().isGrounded == false)
                //{
                //    //Both are in the air. Addforce to the second object to the direction you hit it + 45 degrees angle (Both objects need to be damaged). 
                //    ObjectManagerV2.instance.direction = col.transform.position - transform.position;
                //    col.gameObject.GetComponent<Rigidbody>().AddForce(ObjectManagerV2.instance.direction.normalized * ObjectManagerV2.instance.oneToAnother);  //THE ISSUE IS THAT IT IS ALSO APPLY THE SAME FORCE TO THE ACTUAL OBJECT THAT HIT THE OTHER ON THE AIR.

                //    life -= ObjectManagerV2.instance.objDamage;
                //    col.gameObject.GetComponent<ObjectBehavior>().life -= ObjectManagerV2.instance.objDamage;
                //    //col.gameObject.GetComponent<Rigidbody>().AddForce((-Vector3.forward + Vector3.up).normalized * ObjectManagerV2.instance.oneToAnother); 
                //    //DestroyObj(gameObject); //Give damage not destroy
                //}
                ////if (isGrounded == false && col.collider.tag != "UniqueObjs") //AS IT IS RIGHT NOW IT WILL PUSH THE COLLIDED OBJECTS BELOW FLOOR
                ////{
                ////    //object the player hit is on air and the object hits another obj. Before it dies needs to apply force to the other object which collided with (addForce to the object to the direction you hit it) (Both objects need to be damaged).
                ////    ObjectManagerV2.instance.direction = col.transform.position - transform.position;
                ////    col.gameObject.GetComponent<Rigidbody>().AddForce(ObjectManagerV2.instance.direction.normalized * ObjectManagerV2.instance.oneToAnother);

                ////    life -= 20;
                ////    col.gameObject.GetComponent<ObjectBehavior>().life -= 20;
                ////    //col.gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.forward.normalized * ObjectManagerV2.instance.oneToAnother);
                ////    //DestroyObj(gameObject); //Give damage not destroy
                ////}
                //if (isGrounded == true)
                //{
                //    if (GameManager.instance.levelManager.gameObject.GetComponent<ProceduralObjectives>().killerObj.name == gameObject.transform.parent.name && GameManager.instance.levelManager.gameObject.GetComponent<ProceduralObjectives>().victimObj.name == col.gameObject.transform.parent.name)
                //    {
                //        GameManager.instance.levelManager.gameObject.GetComponent<ProceduralObjectives>().completeObjective = true;
                //    }
                //    //col.gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.forward.normalized * ObjectManagerV2.instance.oneToAnother);
                //    ObjectManagerV2.instance.direction = col.transform.position - transform.position;
                //    if (col.gameObject.tag != "UniqueObjs")
                //    {
                //        col.gameObject.GetComponent<Rigidbody>().AddForce(ObjectManagerV2.instance.direction.normalized * ObjectManagerV2.instance.oneToAnother);
                //    }

                //    life -= ObjectManagerV2.instance.objDamage;
                //    col.gameObject.GetComponent<ObjectBehavior>().life -= ObjectManagerV2.instance.objDamage;
                //    //DestroyObj(gameObject); //Give damage not destroy
                //    //DestroyObj(col.gameObject); //Give damage not destroy
                //}

                ObjectManagerV2.instance.direction = col.transform.position - transform.position;
                if (col.gameObject.tag != "UniqueObjs")
                {
                    col.gameObject.GetComponent<Rigidbody>().AddForce(ObjectManagerV2.instance.direction.normalized * ObjectManagerV2.instance.oneToAnother);
                }
                life -= ObjectManagerV2.instance.objDamage;
                col.gameObject.GetComponent<ObjectBehavior>().life -= ObjectManagerV2.instance.objDamage;
            }

            if (gameObject.tag != "UniqueObjs")
            {
                if (col.collider.tag == "Wall")
                {
                    //DestroyObj(gameObject);
                    //GetComponent<Rigidbody>().velocity = Vector3.zero;
                    life -= ObjectManagerV2.instance.wallDamage;
                }

                //********** 4 AUDIO and ANIMATION

                if (col.collider.tag == "Floor" || objRB.velocity == Vector3.zero)
                {
                    if (hasLanded == false && isGrounded == false)
                    {
                        GameManager.instance.objectLanding(gameObject);
                        //print ("landing" + gameObject);
                    }
                    hasLanded = true;
                }
            }
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (gameObject.tag != "UniqueObjs")
        {
            if (col.collider.tag == "Floor" || objRB.velocity == Vector3.zero)
            {
                isGrounded = true;

                initialPos = transform.position;
            }
        }
    }
}
