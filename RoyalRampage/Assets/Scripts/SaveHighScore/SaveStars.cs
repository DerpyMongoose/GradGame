using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

public class SaveStars {

    private static SaveStars _instance;

    public string saveNamePath = "/savestars.xml";
    private string saveName = "savedata";

    bool iFoundIt = false;

    public static SaveStars instance {
        get {
            if (_instance == null) {
                _instance = new SaveStars();
            }
            return _instance;
        }
    }

    List<LevelAndStars> listForStars = new List<LevelAndStars>();

    //Create the xml language
    private void CreateXml(string level, int starse) {
        if (listForStars.Count > 0) {
            XDocument doc = new XDocument(new XElement(saveName,
                from obj in listForStars
                select new XElement("Object",
                new XElement("Level", obj.Level),
                new XElement("Stars", obj.Stars))));

            doc.Save(Application.persistentDataPath + saveNamePath);
        }
        listForStars = new List<LevelAndStars>();
    }

    public void saveData(string level, int stars) {
        listForStars.Add(new LevelAndStars { Level = level, Stars = stars });

        try {
            XElement doc = XElement.Load(Application.persistentDataPath + saveNamePath);

            var element = from p in doc.Elements("Object")
                          where p.Element("Level").Value == level
                          select p;

            foreach (XElement ele in element) {
                if (ele.Element("Level").Value == level) {
                    iFoundIt = true;
                } else {
                    iFoundIt = false;
                }
            }

            if (iFoundIt) {
                foreach (XElement ele in element) {
                    var temp = ele.Element("Stars");
                    temp.ReplaceNodes(stars);
                }
            } else {
                foreach (LevelAndStars obj in listForStars) {
                    doc.Add(new XElement("Object",
                        new XElement("Level", obj.Level),
                        new XElement("Stars", obj.Stars)));
                }
            }

            doc.Save(Application.persistentDataPath + saveNamePath);
        } catch {
            CreateXml(level, stars);
        }
        listForStars = new List<LevelAndStars>();
        iFoundIt = false;
    }

    public List<LevelAndStars> ReturnListWithObjects(string key) {
        List<LevelAndStars> result = new List<LevelAndStars>();
        bool wasFound = false;
        try {
            XElement e = XElement.Load(Application.persistentDataPath + saveNamePath);
            IEnumerable<XElement> var = e.Elements();

            //find the word
            foreach (XElement node in var) {
                if (node.Element("Level").Value == key) {
                    result.Add(new LevelAndStars {
                        Level = key,
                        Stars = (int)node.Element("Stars")
                    });
                    wasFound = true;
                }
            }
            if (wasFound) {
                wasFound = false;
                return result;
            } else {
                result.Add(new LevelAndStars { Level = key, Stars = 0 });
                return result;
            }

        } catch {
            result.Add(new LevelAndStars {
                Level = key,
                Stars = 0
            });
            CreateXml(key, 0);
            return result;
        }

    }
}
