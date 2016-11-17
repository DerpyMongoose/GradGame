using UnityEngine;
using System.Collections;
using System.Linq;
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

    public string ReturnWord(string key) {
        string result = "";
        XElement doc = XElement.Load(languages[languageSelect] + ".xml");

        IEnumerable<XElement> var = doc.Elements();

        foreach (XElement node in var) {
            if (node.Element("Key").Value == key) {
                result += node.Element("Value").Value;
            }
        }
        return result;
    }

    public List<KeyAndWord> ReturnSet(int index) {
        List<KeyAndWord> result = new List<KeyAndWord>();

        XElement doc = XElement.Load(languages[index] + ".xml");

        IEnumerable<XElement> var = doc.Elements();

        foreach (XElement node in var) {
            result.Add(new KeyAndWord {Key = node.Element("Key").Value,
            Word = node.Element("Value").Value});
        }
        return result;
    }
}
