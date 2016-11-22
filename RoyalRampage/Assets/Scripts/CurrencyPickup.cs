using UnityEngine;
using System.Collections;

public class CurrencyPickup : MonoBehaviour
{
    public int coinValue = 10;
    private float smoothValue;
    // Use this for initialization
    void Start()
    {
        smoothValue = GameManager.instance.player.GetComponent<PlayerStates>().smoothPick;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, GameManager.instance.player.transform.position, Time.deltaTime * smoothValue);

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            PickupCoin();
        }
    }

    void PickupCoin()
    {
        //Play Sound here
        GameManager.instance.currency += coinValue;
        //Optimise: Dont use destroy
        Destroy(gameObject);
    }


}