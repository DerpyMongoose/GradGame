using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralFloorTexture : MonoBehaviour {

    private Texture2D newTex;
    private Texture2D normalMap;
    public int res = 256;
    public int offset = 1;

    public int amountOfSectionsX = 8;
    public int amountOfSectionsY = 3;
    private Color[] floorColor = new Color[] {
        new Color(0.627f,0.322f,0.176f) * 0.33f,new Color(0.545f,0.271f,0.075f) * 0.5f,
    new Color(0.627f,0.322f,0.176f) * 0.5f, new Color(0.627f,0.322f,0.176f) * 0.25f,
    new Color(0.627f,0.322f,0.176f) * 0.78f,new Color(0.627f,0.322f,0.176f) * 0.17f};
    List<int> sectionListX;
    public int xTile = 10;
    public int yTile = 10;
    void OnEnable() {

        GetComponent<Renderer>().material.shaderKeywords = new string[1] { "_NORMALMAP" };
        newTex = new Texture2D(res, res, TextureFormat.ARGB32, false);
        //normalMap = (Texture2D)Resources.Load("woodNormalMap");
        normalMap = new Texture2D(res, res, TextureFormat.ARGB32, false);

        normalMap.wrapMode = TextureWrapMode.Repeat;
        normalMap.filterMode = FilterMode.Trilinear;

        newTex.wrapMode = TextureWrapMode.Repeat;
        newTex.filterMode = FilterMode.Trilinear;

        GetComponent<Renderer>().material.mainTexture = newTex;
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(xTile, yTile);
        GetComponent<Renderer>().material.SetTexture("_BumpMap", normalMap);
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

            int section1 = Random.Range(70, 140);
            int section2 = Random.Range(160, 210);
            Color sectionOneCol = floorColor[Random.Range(0, 6)];
            Color sectionTwoCol = floorColor[Random.Range(0, 6)];
            Color sectionThreeCol = floorColor[Random.Range(0, 6)];
            if (i == 0) {
                for (int x = startX; x < sectionListX[i]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= section1) {
                            newTex.SetPixel(x, y, sectionOneCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y > section1 - offset || y < 0 + offset) {
                                newTex.SetPixel(x, y, Color.black);
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else if (y > section1 && y <= section2) {
                            newTex.SetPixel(x, y, sectionTwoCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y > section2 - offset) {
                                newTex.SetPixel(x, y, Color.black);
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else {
                            newTex.SetPixel(x, y, sectionThreeCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y > res - offset) {
                                newTex.SetPixel(x, y, Color.black);
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        }
                        if(x < 0 + offset) {
                            newTex.SetPixel(x, y, Color.black);
                            normalMap.SetPixel(x, y, Color.black);
                        }
                    }
                }
            }
            if (i < sectionListX.Count - 1 && i > 0) {
                for (int x = startX; x < sectionListX[i + 1]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= section1) {
                            newTex.SetPixel(x, y, sectionOneCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y > section1 - offset || y < 0 + offset) {
                                newTex.SetPixel(x, y, Color.black);
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else if (y > section1 && y <= section2) {
                            newTex.SetPixel(x, y, sectionTwoCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y > section2 - offset) {
                                newTex.SetPixel(x, y, Color.black);
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else {
                            newTex.SetPixel(x, y, sectionThreeCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y > res - offset) {
                                newTex.SetPixel(x, y, Color.black);
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        }
                        if (x < (startX + offset) || x > (sectionListX[i + 1] - offset)) {
                            newTex.SetPixel(x, y, Color.black);
                            normalMap.SetPixel(x, y, Color.black);
                        }
                    }
                }
            }
            if (i == sectionListX.Count-1) {
                for (int x = startX; x < sectionListX[i]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= section1) {
                            newTex.SetPixel(x, y, sectionOneCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y > section1 - offset || y < 0 + offset) {
                                newTex.SetPixel(x, y, Color.black);
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else if (y > section1 && y <= section2) {
                            newTex.SetPixel(x, y, sectionTwoCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y > section2 - offset) {
                                newTex.SetPixel(x, y, Color.black);
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else {
                            newTex.SetPixel(x, y, sectionThreeCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y > res - offset) {
                                newTex.SetPixel(x, y, Color.black);
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        }
                        if (x < (startX + offset) || x > (sectionListX[i] - offset)) {
                            newTex.SetPixel(x, y, Color.black);
                            normalMap.SetPixel(x, y, Color.black);
                        }
                    }
                }
            }
        }
        newTex.Apply();
        normalMap.Apply();
    }

    void CreateSectionList() {
        sectionListX = new List<int>();
        for (int i = 0; i < amountOfSectionsX; i++) {
            sectionListX.Add((res / amountOfSectionsX) * (i + 1));
        }
    }



}
