using UnityEngine;
using System.Collections;

public class Destruction : MonoBehaviour
{
    [SerializeField]
    private int maxStates;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int tPPrefab;

    private GameObject player;
    private ParticleSystem particleSys;

    private int state;

	//object name to be used for Quest system??
	public enum DestructableObject {
		BAREL, CHAIR, TABLE, WARDROBE, BED, BOX
	}

	public DestructableObject objType;
	public int pointsForDestruction;

    // Use this for initialization
    void Start()
    {
        state = 1;
        particleSys = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        if(maxStates <= 0)
        {
            print("Please assign maxStates a value above 0");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision col)
    {

        if(col.collider.gameObject == player)
        {
            col.collider.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if(state == (maxStates - maxStates) + state)
            {
                print(state);
                if (state >= maxStates)
                {           
                    for (int i = 0; i < tPPrefab; i++)
                    {
                        Instantiate(prefab,transform.position, Quaternion.identity);
                    }

					Destroy (gameObject);
                }
				if (particleSys != null) 
				{
					particleSys.startSize *= state; 
					particleSys.Play ();
				}
				state += state;
            }
        }
    }
}
