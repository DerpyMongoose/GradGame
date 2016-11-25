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
                        //score = ObjectManagerV2.instance.smallGlassScore;
                        life = ObjectManagerV2.instance.smallGlassLife;
                        break;
                    case DestructableSize.MEDIUM:
                        //score = ObjectManagerV2.instance.mediumGlassScore;
                        life = ObjectManagerV2.instance.mediumGlassLife;
                        break;
                    case DestructableSize.LARGE:
                        //score = ObjectManagerV2.instance.largeGlassScore;
                        life = ObjectManagerV2.instance.largeGlassLife;
                        break;
                    default:
                        break;
                }
                break;
            case DestructableMaterial.WOOD:
                switch (objSize)
                {
                    case DestructableSize.SMALL:
                        //score = ObjectManagerV2.instance.smallWoodScore;
                        life = ObjectManagerV2.instance.smallWoodLife;
                        break;
                    case DestructableSize.MEDIUM:
                        //score = ObjectManagerV2.instance.mediumWoodScore;
                        life = ObjectManagerV2.instance.mediumWoodLife;
                        break;
                    case DestructableSize.LARGE:
                        //score = ObjectManagerV2.instance.largeWoodScore;
                        life = ObjectManagerV2.instance.largeWoodLife;
                        break;
                    default:
                        break;
                }
                break;
            case DestructableMaterial.STONE:
                switch (objSize)
                {
                    case DestructableSize.SMALL:
                        //score = ObjectManagerV2.instance.smallStoneScore;
                        life = ObjectManagerV2.instance.smallStoneLife;
                        break;
                    case DestructableSize.MEDIUM:
                        //score = ObjectManagerV2.instance.mediumStoneScore;
                        life = ObjectManagerV2.instance.mediumStoneLife;
                        break;
                    case DestructableSize.LARGE:
                        //score = ObjectManagerV2.instance.largeStoneScore;
                        life = ObjectManagerV2.instance.largeStoneLife;
                        break;
                    default:
                        break;
                }
                break;
            case DestructableMaterial.METAL:
                switch (objSize)
                {
                    case DestructableSize.SMALL:
                        //score = ObjectManagerV2.instance.smallMetalScore;
                        life = ObjectManagerV2.instance.smallMetalLife;
                        break;
                    case DestructableSize.MEDIUM:
                        //score = ObjectManagerV2.instance.mediumMetalScore;
                        life = ObjectManagerV2.instance.mediumMetalLife;
                        break;
                    case DestructableSize.LARGE:
                        //score = ObjectManagerV2.instance.largeMetalScore;
                        life = ObjectManagerV2.instance.largeMetalLife;
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
        hit = false;
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
        if (hit)
        {
            CheckDamage();
            CheckVelocity();
        }

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


    void CheckDamage()
    {
        if (life <= 0)
        {
            SpawnPoints();
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
                GameManager.instance.objectDestructed(gameObject);
                Destroy(gameObject);
                if (currencySpawnChance > 0.0f)
                {
                    SpawnCurrency();
                }
            }
            catch
            {
                GameManager.instance.objectDestructed(gameObject);
                Destroy(gameObject);
                if (currencySpawnChance > 0.0f)
                {
                    SpawnCurrency();
                }
            }
        }
    }

    void CheckVelocity()
    {
        if (objRB.velocity.magnitude <= 0.5f)
        {
            //print("Hit becomes false now");
            hit = false;

        }
    }

    void SpawnPoints () {
        GameObject obj = PointObjectPool.instance.FindObjectInPool();

        obj.transform.position = transform.position + Vector3.up;
        obj.transform.rotation = Quaternion.LookRotation(obj.transform.position - Camera.main.transform.position);
        //obj.transform.LookAt(Camera.main.transform);
        obj.SetActive(true);

        obj.GetComponent<SetTextPoints>().SetText(score*GameManager.instance.levelManager.multiplier);
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
                //col.collider.GetComponent<ObjectBehavior>().particleSys.Play(); /////////IT WILL GIVE AN ERROR IN THE LEVELS WITHOUT THE FRACTURED OBJECTS
                ObjectManagerV2.instance.direction = col.transform.position - transform.position;
                if (col.gameObject.tag != "UniqueObjs")
                {
                    col.gameObject.GetComponent<Rigidbody>().AddForce(ObjectManagerV2.instance.direction.normalized * ObjectManagerV2.instance.oneToAnother, ForceMode.Impulse);
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
