using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralFloorTexture : MonoBehaviour {

    private Texture2D newTex;
    public int res = 256;
    public int offset = 1;

    public int amountOfSectionsX = 8;
    public int amountOfSectionsY = 3;
    private Color[] floorColor = new Color[] {
        new Color(0.38f,0.247f,0.141f),new Color(0.329f,0.196f,0.090f),
    new Color(0.309f,0.196f,0.113f), new Color(0.301f,0.207f,0.113f),
    new Color(0.360f,0.227f,0.117f) ,new Color(0.341f,0.211f,0.098f)};
    List<int> sectionListX;
    public int xTile = 10;
    public int yTile = 10;

    void OnEnable() {
        newTex = new Texture2D(res, res, TextureFormat.ARGB32, false);

        newTex.wrapMode = TextureWrapMode.Repeat;

        GetComponent<Renderer>().material.mainTexture = newTex;
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(xTile, yTile);
        FloorPlanks();
    }

    public void FloorPlanks() {

        CreateSectionList();
        for (int i = 0; i < sectionListX.Count; i++) {
            int startX;
            if (i == 0) {
                startX = 0;
            } else {
                startX = sectionListX[i - 1];
            }

            int section1 = Random.Range(res / 4, (res / 2) - (int)(res * 0.1f));
            int section2 = Random.Range((res / 2) + (int)(res * 0.1f), (int)(res * 0.75f));
            Color sectionOneCol = floorColor[Random.Range(0, 6)];
            Color sectionTwoCol = floorColor[Random.Range(0, 6)];
            Color sectionThreeCol = floorColor[Random.Range(0, 6)];
            if (i == 0) {
                for (int x = startX; x < sectionListX[i]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= section1) {
                            newTex.SetPixel(x, y, sectionOneCol);
                        } else if (y > section1 && y <= section2) {
                            newTex.SetPixel(x, y, sectionTwoCol);
                        } else {
                            newTex.SetPixel(x, y, sectionThreeCol);
                        }
                    }
                }
            }
            if (i < sectionListX.Count - 1 && i > 0) {
                for (int x = startX; x < sectionListX[i + 1]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= section1) {
                            newTex.SetPixel(x, y, sectionOneCol);
                        } else if (y > section1 && y <= section2) {
                            newTex.SetPixel(x, y, sectionTwoCol);
                        } else {
                            newTex.SetPixel(x, y, sectionThreeCol);
                        }
                    }
                }
            }
            if (i == sectionListX.Count - 1) {
                for (int x = startX; x < sectionListX[i]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= section1) {
                            newTex.SetPixel(x, y, sectionOneCol);
                        } else if (y > section1 && y <= section2) {
                        } else {
                            newTex.SetPixel(x, y, sectionThreeCol);
                        }
                    }
                }
            }
        }
        newTex.Apply();
    }

    void CreateSectionList() {
        sectionListX = new List<int>();
        for (int i = 0; i < amountOfSectionsX; i++) {
            sectionListX.Add((res / amountOfSectionsX) * (i + 1));
        }
    }

}
