using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

public class SaveHighScore {

    private static SaveHighScore _instance;

    public string saveNamePath = "/savehighscore.xml";
    private string saveName = "savedata";

    bool iFoundIt = false;

    public static SaveHighScore instance {
        get {
            if (_instance == null) {
                _instance = new SaveHighScore();
            }
            return _instance;
        }
    }

    List<LevelAndObjects> listforHighScore = new List<LevelAndObjects>();

    //Create the xml language
    private void CreateXml(string level, int highScore) {
        if (listforHighScore.Count > 0) {
            XDocument doc = new XDocument(new XElement(saveName,
                from obj in listforHighScore
                select new XElement("Object",
                new XElement("Level", obj.Level),
                new XElement("HighScore", obj.HighScore))));

            doc.Save(Application.persistentDataPath + saveNamePath);
        }
        listforHighScore = new List<LevelAndObjects>();
    }

    public void saveData(string level, int highScore) {
        listforHighScore.Add(new LevelAndObjects { Level = level, HighScore = highScore });
        
        try {
            XElement doc = XElement.Load(Application.persistentDataPath + saveNamePath);

            var element = from p in doc.Elements("Object")
                          where p.Element("Level").Value == level
                          select p;

            foreach(XElement ele in element) {
              if(ele.Element("Level").Value == level) {
                    iFoundIt = true;
                } else {
                    iFoundIt = false;
                }
            }

            if (iFoundIt) {
                foreach (XElement ele in element) {
                    var temp = ele.Element("HighScore");
                    temp.ReplaceNodes(highScore);
                }
            } else {
                foreach (LevelAndObjects obj in listforHighScore) {
                    Debug.Log(obj.HighScore);
                    doc.Add(new XElement("Object",
                        new XElement("Level", obj.Level),
                        new XElement("HighScore", obj.HighScore)));
                }
            }
            
            doc.Save(Application.persistentDataPath + saveNamePath);
        } catch {
            CreateXml(level, highScore);
        }
        listforHighScore = new List<LevelAndObjects>();
        iFoundIt = false;
    }

    public List<LevelAndObjects> ReturnListWithObjects(string key) {
        List<LevelAndObjects> result = new List<LevelAndObjects>();
        bool wasFound = false;
        try {
            Debug.Log("Hello From XML");
            XElement e = XElement.Load(Application.persistentDataPath + saveNamePath);
            IEnumerable<XElement> var = e.Elements();

            //find the word
            foreach (XElement node in var) {
                if (node.Element("Level").Value == key) {
                    result.Add(new LevelAndObjects {
                        Level = key,
                        HighScore = (int)node.Element("HighScore")
                    });
                    wasFound = true;
                }
            }
            if (wasFound) {
                wasFound = false;
                return result;

            } else {
                result.Add(new LevelAndObjects { Level = key, HighScore = 0 });
                return result;
            }
        } catch {
            result.Add(new LevelAndObjects {
                Level = key,
                HighScore = 0
            });
            CreateXml(key, 0);
            return result;
        }

    }
}
