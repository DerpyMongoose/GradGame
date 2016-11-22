using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreButtonScript : MonoBehaviour {

    public int price;
    public int amountOfCurrency;

    string currency = "DKK";
    // Use this for initialization
    void Awake() {
            GetComponentInChildren<Text>().text = amountOfCurrency + "\n" + price + " " + currency;

    }

    public void clickBuyButton() {
        StoreManager.instance.BuyCurrency(amountOfCurrency);
    }

}
