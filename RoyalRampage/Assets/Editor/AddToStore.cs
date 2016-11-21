using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class AddToStore : EditorWindow {

    public static AddToStore window;

    string objectName = "";
    string price = "";

    bool hasStarted = false;

    Vector2 scrollPos;
    string showText = "What items are you adding";

    List<StoreObject> storeItems = new List<StoreObject>();
    List<StoreObject> alreadyAdded;

    [MenuItem("Store Manager/Add To Store")]
    public static void AddItemToStore() {
        window = (AddToStore)EditorWindow.GetWindow(typeof(AddToStore));
        window.titleContent.text = "Add item to Store";
        window.minSize = new Vector2(1024, 768);
    }

    void OnGUI() {
        if (window != null) {
            AddItemToStore();
        }

        objectName = EditorGUI.TextField(new Rect(0, 220, 300, 20), "Object Name", objectName);
        price = EditorGUI.TextField(new Rect(300, 220, 300, 20), "Price of Object", price);

        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(new Vector2(0, 40), GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.Label(showText);
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        if (hasStarted == false) {
            Debug.Log("Hello");
            ShowXml();
            hasStarted = true;
        }

        if (GUI.Button(new Rect(0, 350, position.width, 50), "Add New Item")) {
            if (objectName != "" && price != "") {
                storeItems.Add(new StoreObject { Object = objectName, Price = int.Parse(price) });
                showText += "\n" + objectName + " : " + price;
                objectName = "";
                price = "";
            }
            Repaint();
        }

        if (GUI.Button(new Rect(0, 400, position.width, 50), "Add to XML")) {
            AddToXml("StoreItems");
            storeItems = new List<StoreObject>();
        }
    }

            //Add the word to the xml document
    private void AddToXml(string language) {

        try {
            XElement doc = XElement.Load("Assets/Resources/" + "StoreItems" + ".xml");
            foreach (StoreObject obj in storeItems) {
                doc.Add(new XElement("Item",
                    new XElement("Object", obj.Object),
                    new XElement("Price", obj.Price)));
            }
            doc.Save("Assets/Resources/" + "StoreItems" + ".xml");
        } catch {
            CreateXml(language);
        }

    }

    //Create the xml language
    private void CreateXml(string language) {
        if (storeItems.Count > 0) {
            XDocument doc = new XDocument(new XElement(language,
                from obj in storeItems
                select new XElement("Item",
                new XElement("Object", obj.Object),
                new XElement("Price", obj.Price))));

            doc.Save("Assets/Resources/" + "StoreItems" + ".xml");
        }

    }

    private void ShowXml() {
        try {            
            alreadyAdded = new List<StoreObject>();            
            alreadyAdded = ReturnSet();
            Debug.Log("I got in here");
            if (alreadyAdded.Count != 0) {
                foreach (StoreObject set in alreadyAdded) {
                    showText += "\n" + set.Object + " : " + set.Price.ToString();
                }
            }

        } catch {
            showText = "What items are you adding";
        }
    }

    //Function to display the current words
    //in the unity editor
    public List<StoreObject> ReturnSet() {
        Debug.Log("Adding to list");
        List<StoreObject> result = new List<StoreObject>();

        XElement doc = XElement.Load("Assets/Resources/" + "StoreItems" + ".xml");

        IEnumerable<XElement> var = doc.Elements();

        foreach (XElement node in var) {
            result.Add(new StoreObject {
                Object = node.Element("Object").Value,
                Price = int.Parse(node.Element("Price").Value)
            });

        }

        return result;
    }

}




