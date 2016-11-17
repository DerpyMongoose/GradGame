using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

public class AddToLanguage : EditorWindow {

    public static AddToLanguage window;
   
    bool hasStarted = false;

    string word = "";
    string key = "";
    string language = "";
    int index = 0;
    int defaultIndex = 0;
    int changedIndex = 0;
    Vector2 scrollPos;
    string showText = "What words are you adding";

    List<KeyAndWord> keysAndWords = new List<KeyAndWord>();
    List<KeyAndWord> alreadyAdded;

    [MenuItem("LanguageManager/Add Language")]
    public static void AddWordToLanguage() {
        window = (AddToLanguage)EditorWindow.GetWindow(typeof(AddToLanguage)); //create a window
        window.titleContent.text = "Add Language"; //set a window title
        window.minSize = new Vector2(1024, 768);

    }

    void Update() {
        if (index > defaultIndex || index < defaultIndex) {
            Debug.Log("I change");
            showText = "What words are you adding";
            Repaint();
            ShowXml();
            defaultIndex = index;
        }

        Debug.Log(index);
    }

    void OnGUI() {
        if (window != null) {
            AddWordToLanguage();
        }

        index = EditorGUILayout.Popup(index, LanguageManager.instance.languages.ToArray());
        key = EditorGUI.TextField(new Rect(0, 220, 300, 20), "Key Word", key);
        word = EditorGUI.TextField(new Rect(300, 220, 300, 20), "Word to Add:", word);

        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(new Vector2(0, 40), GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.Label(showText);
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        if(hasStarted == false) {
            ShowXml();
            hasStarted = true;
        }
       
        

        if (GUI.Button(new Rect(0, 350, position.width, 50), "Add New Word")) {
            if (key != "" && word != "") {
                keysAndWords.Add(new KeyAndWord { Key = key, Word = word });
                showText += "\n" + key + " : " + word;
                key = "";
                word = "";
                DebugFunction();
            }
            Repaint();
        }

        if (GUI.Button(new Rect(0, 400, position.width, 50), "Add to language")) {
            AddToXml(LanguageManager.instance.languages[index]);
            keysAndWords = new List<KeyAndWord>();
        }


    }

    private void CreateXml(string language) {
        if (keysAndWords.Count > 0) {
            XDocument doc = new XDocument(new XElement(language,
                from obj in keysAndWords
                select new XElement("Word",
                new XElement("Key", obj.Key),
                new XElement("Value", obj.Word))));

            doc.Save(language + ".xml");
        }
        
    }

    private void AddToXml(string language) {

        try {
            XElement doc = XElement.Load(language + ".xml");
            foreach (KeyAndWord obj in keysAndWords) {
                doc.Add(new XElement("Word",
                    new XElement("Key", obj.Key),
                    new XElement("Value", obj.Word)));
            }
            doc.Save(language + ".xml");
        } catch {
            CreateXml(language);
        }

    }

    private void ShowXml () {
        try {
            alreadyAdded = new List<KeyAndWord>();
            alreadyAdded = LanguageManager.instance.ReturnSet(index);
            Debug.Log(alreadyAdded.Count);
            if (alreadyAdded.Count != 0) {
                foreach (KeyAndWord set in alreadyAdded) {
                    showText += "\n" + set.Key + " : " + set.Word;
                }
            }

        } catch {
            showText = "What words are you adding";
        }
    }

    void DebugFunction() {
        if (keysAndWords.Count != 0) {
            for (int i = 0; i < keysAndWords.Count; i++) {
                Debug.Log(keysAndWords[i].Key + " : " + keysAndWords[i].Word);
            }
        }
    }
}
