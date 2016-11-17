﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralFloorTexture : MonoBehaviour {

    private Texture2D newTex;
    private Texture2D normalMap;
    public int res = 256;
    public int offset = 1;
    public float normalMapScale = 0.3f;

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
        Debug.Log(res);
        GetComponent<Renderer>().material.shaderKeywords = new string[1] { "_NORMALMAP" };
        newTex = new Texture2D(res, res, TextureFormat.ARGB32, false);
        normalMap = new Texture2D(res, res, TextureFormat.ARGB32, false);

        normalMap.wrapMode = TextureWrapMode.Repeat;
        //normalMap.filterMode = FilterMode.Trilinear;

        newTex.wrapMode = TextureWrapMode.Repeat;
       // newTex.filterMode = FilterMode.Trilinear;

        GetComponent<Renderer>().material.mainTexture = newTex;
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(xTile, yTile);
        GetComponent<Renderer>().material.SetTexture("_BumpMap", normalMap);
        GetComponent<Renderer>().material.SetFloat("_BumpScale", normalMapScale);
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

            int section1 = Random.Range(res/4, (res/2) - (int)(res*0.1f));
            int section2 = Random.Range((res/2) + (int)(res * 0.1f), (int)(res*0.75f));
            Color sectionOneCol = floorColor[Random.Range(0, 6)];
            Color sectionTwoCol = floorColor[Random.Range(0, 6)];
            Color sectionThreeCol = floorColor[Random.Range(0, 6)];
            if (i == 0) {
                for (int x = startX; x < sectionListX[i]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= section1) {
                            newTex.SetPixel(x, y, sectionOneCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y >= section1 - offset || y <= 0 + offset) {
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else if (y > section1 && y <= section2) {
                            newTex.SetPixel(x, y, sectionTwoCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y >= section2 - offset) {
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else {
                            newTex.SetPixel(x, y, sectionThreeCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y >= res - offset) {
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        }
                        if(x <= 0 + offset) {
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
                            if (y >= section1 - offset || y <= 0 + offset) {
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else if (y > section1 && y <= section2) {
                            newTex.SetPixel(x, y, sectionTwoCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y >= section2 - offset) {
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else {
                            newTex.SetPixel(x, y, sectionThreeCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y >= res - offset) {
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        }
                        if (x <= (startX + offset) || x >= (sectionListX[i + 1] - offset)) {
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
                            if (y >= section1 - offset || y <= 0 + offset) {
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else if (y > section1 && y <= section2) {
                            normalMap.SetPixel(x, y, Color.white);
                            if (y >= section2 - offset) {
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        } else {
                            newTex.SetPixel(x, y, sectionThreeCol);
                            normalMap.SetPixel(x, y, Color.white);
                            if (y >= res - offset) {
                                normalMap.SetPixel(x, y, Color.black);
                            }
                        }
                        if (x <= (startX + offset) || x >= (sectionListX[i] - offset)) {
                            normalMap.SetPixel(x, y, Color.black);
                        }
                    }
                }
            }
        }
        newTex.Apply();
        normalMap.Apply();
        createBumpMap();
    }

    void CreateSectionList() {
        sectionListX = new List<int>();
        for (int i = 0; i < amountOfSectionsX; i++) {
            sectionListX.Add((res / amountOfSectionsX) * (i + 1));
        }
    }

    void createBumpMap() {
        float xLeft;
        float xRight;
        float yUp;
        float yDown;
        float yDelta;
        float xDelta;
        for(int y = 0; y < res; y++) {
            for(int x = 0; x < res; x++) {

                xLeft = normalMap.GetPixel(x - 1, y).grayscale;
                xRight = normalMap.GetPixel(x + 1, y).grayscale;
                yUp = normalMap.GetPixel(x, y - 1).grayscale;
                yDown = normalMap.GetPixel(x, y + 1).grayscale;
                xDelta = ((xLeft - xRight) + 1f) * 0.5f;
                yDelta = ((yUp - yDown) + 1f) * 0.5f;

                normalMap.SetPixel(x,y,new Color(xDelta,yDelta,1.0f,1.0f));
            }
        }
        normalMap.Apply();
    }

}
