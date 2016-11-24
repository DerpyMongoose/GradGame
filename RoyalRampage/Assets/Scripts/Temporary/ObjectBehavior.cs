using UnityEngine;
using System.Collections;

public class ObjectBehavior : MonoBehaviour
{
    private GameObject rubblePrefab;
    [HideInInspector]
    public int life, initialLife;
    private int rubbleAmount;

    public string soundSwitch; // FOR AUDIO

    private Rigidbody objRB;
    private GameObject player;
    private IEnumerator coroutine;

    [HideInInspector]
    public ParticleSystem particleSys;
    [HideInInspector]
    public bool hit = false;
    [HideInInspector]
    public bool slowed;
    [HideInInspector]
    public bool lifted;

    private bool readyToCheck;

    public bool hasLanded = true;

    private float checkHeight, initialMass;

    [HideInInspector]
    public int score;

    [Range(0.0f, 1.0f)]
    public float currencySpawnChance = 0.1f;



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
        initialLife = life;
        objRB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        //particleSys = GetComponent<ParticleSystem>();
        slowed = false;
        lifted = false;
        //ObjectManagerV2.instance.maxScore += score;  

        //Disable all the children.
        if (gameObject.tag != "UniqueObjs")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<FracturedChunk>() != null)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }


    void Update()
    {
        CheckDamage();
        CheckVelocity();

        if (lifted)
        {
            //APPLY TRANSFORM ROTATION
            transform.Rotate(Vector3.up, GameManager.instance.player.GetComponent<PlayerStates>().torgueForce, Space.World);

            if (Mathf.Round(objRB.velocity.y * 10) / 10 < 0 && !slowed) //Checking if the object reached its peak and if it has already been slowed down from slowMotion.
            {
                objRB.isKinematic = true;
                slowed = true;
            }

            if (PlayerStates.imInSlowMotion)
            {
                if (!hasLanded)
                {
                    transform.position -= Vector3.up * 0.003f;
                }
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

            try
            {
                for (int p = 0; p < transform.childCount; p++)
                {
                    if (transform.GetChild(p).GetComponent<FracturedChunk>() != null)
                    {
                        transform.GetChild(p).gameObject.SetActive(true);
                        transform.GetChild(p).GetComponent<MeshCollider>().enabled = true;
                    }
                }
                GetComponent<FracturedObject>().CollapseChunks();
                DestroyObj(gameObject);
                if (currencySpawnChance > 0.0f)
                {
                    SpawnCurrency();
                }
            }
            catch
            {
                DestroyObj(gameObject);
                if (currencySpawnChance > 0.0f)
                {
                    SpawnCurrency();
                }
            }
        }
    }

    void CheckVelocity()
    {
        if (Mathf.Round(objRB.velocity.magnitude) == 0)
        {
            //print("Hit becomes false now");
            hit = false;

        }
    }


    void SpawnCurrency()
    {
        float randomVal = Random.value;

        if (randomVal < currencySpawnChance)
        {//Optimise!
            Instantiate((GameObject)Resources.Load("Collectibles/Currency", typeof(GameObject)), new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
        }
    }


    void OnCollisionEnter(Collision col)
    {
        //print(col.relativeVelocity.magnitude);
        if (col.relativeVelocity.magnitude > ObjectManagerV2.instance.colImpact)
        {
            if (col.collider.gameObject == player)
            {
                GameManager.instance.objectHit(gameObject);
            }

            if ((col.collider.tag == "Destructable" || col.collider.tag == "UniqueObjs") && hit == true)
            {
                col.collider.GetComponent<ObjectBehavior>().hit = true;
                // PLAY DAMAGE PARTICLE
                //particleSys.Play();
                col.collider.GetComponent<ObjectBehavior>().particleSys.Play(); /////////IT WILL GIVE AN ERROR IN THE LEVELS WITHOUT THE FRACTURED OBJECTS
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
                    //GetComponent<Rigidbody>().velocity = Vector3.zero;
                    life -= ObjectManagerV2.instance.wallDamage;
                }

                //********** 4 AUDIO and ANIMATION

                if (col.collider.tag == "Floor" || Mathf.Round(objRB.velocity.magnitude) == 0)
                {
                    if (hasLanded == false)
                    {
                        GameManager.instance.objectLanding(gameObject);
                    }
                    hasLanded = true;
                }
            }
        }
    }
}
