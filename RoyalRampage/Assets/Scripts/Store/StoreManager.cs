using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour {

    public static StoreManager instance;

    public List<StoreObject> objectsInStore = new List<StoreObject>();

    public float conversion = 0.01f;

    public GameObject button;
    public RectTransform parent;
    GameObject message;
    GameObject buyCurrency;
    public GameObject confirmBox;
    GameObject tempItem;

    public bool hasLoaded = false;
    bool willBuy = false;
    bool amIbuying = false;

    int inputFieldValue;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

	// Use this for initialization
	void Start () {
        tempItem = null;
        willBuy = false;
        message = GameObject.FindGameObjectWithTag("BuyMorePop");
        buyCurrency = GameObject.FindGameObjectWithTag("BuyCurrency");
        message.SetActive(false);
        buyCurrency.SetActive(false);
        confirmBox.SetActive(false);
        objectsInStore = ReturnSet();
        createButtons();
	}

    public void BuyItem (GameObject button) {
        if (GameManager.instance.currency > 0 && (GameManager.instance.currency - button.GetComponent<StoreButtonScript>().price) >= 0 && willBuy == true) {
            GameManager.instance.currency -= button.GetComponent<StoreButtonScript>().price;
            GameObject.FindGameObjectWithTag("Currency").GetComponent<CurrencyUIScript>().changeText();
            GameManager.instance.Save();
            willBuy = false;
            tempItem = null;
        } else {           
            message.SetActive(true);
        }
        
    }
    public void ShowConfirm(GameObject itemToBuy) {
        confirmBox.SetActive(true);
        tempItem = itemToBuy;
    }

    public void ConfirmBuy () {
        willBuy = true;
        BuyItem(tempItem);
        confirmBox.SetActive(false);
    }
    public void DenyBuy() {
        willBuy = false;
        confirmBox.SetActive(false);
    }

    //Function to get a word from the xml file
    private int ReturnItemPrice(string key) {
        //Load xml as textasset
        TextAsset textAsset = (TextAsset)Resources.Load("StoreItems", typeof(TextAsset));
        int result = 0;
        //Create xml document
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        //Convert to Xelement
        XElement e = XElement.Load(new XmlNodeReader(doc));
        IEnumerable<XElement> var = e.Elements();

        //find the word
        foreach (XElement node in var) {
            if (node.Element("Object").Value == key) {
                result += int.Parse(node.Element("Price").Value);
            }
        }
        return result;
    }

    //Function to display the current words
    //in the unity editor
    private List<StoreObject> ReturnSet() {
        //Load xml as textasset
        TextAsset textAsset = (TextAsset)Resources.Load("StoreItems", typeof(TextAsset));
        //Create xml document
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        //Convert to Xelement
        XElement e = XElement.Load(new XmlNodeReader(doc));
        IEnumerable<XElement> var = e.Elements();

        List<StoreObject> result = new List<StoreObject>();

        foreach (XElement node in var) {
            result.Add(new StoreObject {
                Object = node.Element("Object").Value,
                Price = int.Parse(node.Element("Price").Value)
            });

        }

        return result;
    }

    private void createButtons () {

        for(int i = 0; i < objectsInStore.Count; i++) {
            GameObject objButton = Instantiate(button);         
            objButton.GetComponent<Button>().onClick.AddListener(() => StoreManager.instance.ShowConfirm(objButton));
            objButton.AddComponent<StoreButtonScript>();
            objButton.GetComponent<StoreButtonScript>().price = objectsInStore[i].Price;
            Component[] temp = objButton.GetComponentsInChildren<Text>();
            temp[0].GetComponent<Text>().text = objectsInStore[i].Object;
            temp[1].GetComponent<Text>().text = objectsInStore[i].Price.ToString();
            objButton.transform.SetParent(parent);
            parent.sizeDelta = parent.sizeDelta + new Vector2(0f,300f);
        }
    }

    public void AddCurrencyYes() {
        buyCurrency.SetActive(true);
        if(message.activeInHierarchy){
            message.SetActive(false);
        }      
    }

    public void AddCurrencyNo() {
        message.SetActive(false);
    }

    public void Back() {
        buyCurrency.SetActive(false);
    }

    public void BuyCurrency(int currency) {
        GameManager.instance.currency += currency;
        GameObject.FindGameObjectWithTag("Currency").GetComponent<CurrencyUIScript>().changeText();
        GameManager.instance.Save();
    }
}
