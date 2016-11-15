using UnityEngine;
using System.Collections;

public class ObjectBehavior : MonoBehaviour
{
    private GameObject rubblePrefab;
    private int life;
    private int rubbleAmount;
    private int state;
    [HideInInspector]
    public string soundSwitch; // FOR AUDIO

    private Vector3 initialPos;

    private Rigidbody objRB;
    private GameObject player;
    private ParticleSystem particleSys;
    private IEnumerator coroutine;

    [HideInInspector]
    public bool isGrounded = true;
    private bool hit = false;
    private bool readyToCheck;
    private bool lifted;

    public bool hasLanded = true;

    private float checkHeight, initialMass;

    [HideInInspector]
    public int score;


    //object name to be used for Quest system??
    public enum DestructableObject
    {
        BARREL, CHAIR, TABLE, WARDROBE, BED, BOX
    }

    public DestructableObject objType;

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

        switch (objType)
        {
            case DestructableObject.BARREL:
                score = ObjectManager.instance.barrelScore;
                life = ObjectManager.instance.barrelLife;
                rubbleAmount = ObjectManager.instance.barrelRubbleAmount;
                rubblePrefab = ObjectManager.instance.barrelRubblePrefab;
                soundSwitch = ObjectManager.instance.barrelSwitch;
                break;
            case DestructableObject.BED:
                score = ObjectManager.instance.bedScore;
                life = ObjectManager.instance.bedLife;
                rubbleAmount = ObjectManager.instance.bedRubbleAmount;
                rubblePrefab = ObjectManager.instance.bedRubblePrefab;
                soundSwitch = ObjectManager.instance.bedSwitch;
                break;
            case DestructableObject.BOX:
                score = ObjectManager.instance.boxScore;
                life = ObjectManager.instance.boxLife;
                rubbleAmount = ObjectManager.instance.boxRubbleAmount;
                rubblePrefab = ObjectManager.instance.boxRubblePrefab;
                soundSwitch = ObjectManager.instance.boxSwitch;
                break;
            case DestructableObject.CHAIR:
                score = ObjectManager.instance.chairScore;
                life = ObjectManager.instance.chairLife;
                rubbleAmount = ObjectManager.instance.chairRubbleAmount;
                rubblePrefab = ObjectManager.instance.chairRubblePrefab;
                soundSwitch = ObjectManager.instance.chairSwitch;
                break;
            case DestructableObject.TABLE:
                score = ObjectManager.instance.tableScore;
                life = ObjectManager.instance.tableLife;
                rubbleAmount = ObjectManager.instance.tableRubbleAmount;
                rubblePrefab = ObjectManager.instance.tableRubblePrefab;
                soundSwitch = ObjectManager.instance.tableSwitch;
                break;
            case DestructableObject.WARDROBE:
                score = ObjectManager.instance.wardrobeScore;
                life = ObjectManager.instance.wardrobeLife;
                rubbleAmount = ObjectManager.instance.wardrobeRubbleAmount;
                rubblePrefab = ObjectManager.instance.wardrobeRubblePrefab;
                soundSwitch = ObjectManager.instance.wardrobeSwitch;
                break;
            default:
                break;

        }
        state = 1;
        particleSys = GetComponent<ParticleSystem>();
        objRB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        initialPos = transform.position;
    }


    void Update()
    {
        //print(PlayerStates.lifted);
        if (gameObject.tag != "UniqueObjs")
        {
            if (PlayerStates.lifted)
            {
                //APPLY TRANSFORM ROTATION
                GameManager.instance.player.GetComponent<PhysicalMovement>().RotateObjs(GameManager.instance.player.GetComponent<PhysicalMovement>().objRB);

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


    void OnCollisionEnter(Collision col)
    {

        if (col.collider.gameObject == player)
        {
            hit = true;

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
            if (isGrounded == false && col.gameObject.GetComponent<ObjectBehavior>().isGrounded == false)
            {
                DestroyObj(gameObject);
            }
            if (isGrounded == false)
            {
                DestroyObj(gameObject);
            }
            if (isGrounded == true)
            {
                if(GameManager.instance.levelManager.gameObject.GetComponent<ProceduralObjectives>().killerObj.name == gameObject.transform.parent.name && GameManager.instance.levelManager.gameObject.GetComponent<ProceduralObjectives>().victimObj.name == col.gameObject.transform.parent.name)
                {
                    GameManager.instance.levelManager.gameObject.GetComponent<ProceduralObjectives>().completeObjective = true;
                }
                DestroyObj(gameObject);
                DestroyObj(col.gameObject);
            }
        }

        if (gameObject.tag != "UniqueObjs")
        {
            if (col.collider.tag == "Wall")
            {
                //DestroyObj(gameObject);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
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
