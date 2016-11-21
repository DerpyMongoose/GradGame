using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

public class LanguageManager {

    private static LanguageManager _instance;
    

    public List<string> languages = new List<string>() { "Danish", "English" };

    public int languageSelect = 0; // 0 for danish, 1 for english

    public static LanguageManager instance {
        get {
            if (_instance == null) {
                _instance = new LanguageManager();
            }
            return _instance;
        }
    }
    //Function to get a word from the xml file
    public string ReturnWord(string key) {
        //Load xml as textasset
        TextAsset textAsset = (TextAsset)Resources.Load(languages[languageSelect],typeof(TextAsset));
        string result = "";
        //Create xml document
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        //Convert to Xelement
        XElement e = XElement.Load(new XmlNodeReader(doc));
        IEnumerable<XElement> var = e.Elements();

        //find the word
        foreach (XElement node in var) {
            if (node.Element("Key").Value == key) {
                result += node.Element("Value").Value;
            }
        }
        return result;
    }

    //Function to display the current words
    //in the unity editor
    public List<KeyAndWord> ReturnSet(int index) {
        List<KeyAndWord> result = new List<KeyAndWord>();

        XElement doc = XElement.Load("Assets/Resources/" + languages[index] + ".xml");

        IEnumerable<XElement> var = doc.Elements();

        foreach (XElement node in var) {
            result.Add(new KeyAndWord {Key = node.Element("Key").Value,
            Word = node.Element("Value").Value});
        }
        return result;
    }

    //Delegate to change the language in the entire scene
    public delegate void ChangeLanguage();

    public event ChangeLanguage ChangeText;

    public void changeText () {
        if(ChangeText != null) {
            ChangeText();
        }
    }
}
