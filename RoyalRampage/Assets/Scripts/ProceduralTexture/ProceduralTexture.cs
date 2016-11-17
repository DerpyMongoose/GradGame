using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ProceduralTexture : MonoBehaviour {

    public int res = 256;

    private Texture2D newTex;
    private Texture2D noiseTex;
    private int[] sizeOfSquare = new int[] { 32, 64, 96 };
    private Color[] floorColor = new Color[] {new Color(0.545f,0.271f,0.075f),
        new Color(0.627f,0.322f,0.176f),new Color(0.545f,0.271f,0.075f) * 0.5f,
    new Color(0.627f,0.322f,0.176f) * 0.5f, new Color(0.627f,0.322f,0.176f) * 0.25f};

    public bool blackwhite = false;

    float deg;
    int xCoord;
    int yCoord;

    void OnEnable() {
        if (newTex == null) {
            newTex = new Texture2D(res, res, TextureFormat.ARGB32, true);

            //For noise
            noiseTex = new Texture2D(res, res, TextureFormat.ARGB32, false);
            //pix = new Color[noiseTex.width * noiseTex.height];

            newTex.name = "Procedural Texture";
            newTex.wrapMode = TextureWrapMode.Repeat;
            newTex.filterMode = FilterMode.Trilinear;


            GetComponent<Renderer>().material.mainTexture = newTex;
            //GetComponent<Renderer>().material.mainTextureScale = new Vector2(10,10);
        }
        //SolidColor();
        //CreatePlanks();
        //CalcNoise();
        //FillTexture();
        RandomFunctions(3);
        //DrawTriangle();

        DebugLogFunction();
    }


    public void SolidColor() {

        for (int i = 0; i < res; i++) {
            for (int j = 0; j < res; j++) {
                newTex.SetPixel(j, i, floorColor[4]);
            }
        }

        newTex.Apply();
    }

    void DrawTriangle() {
        for (int y = 0; y < res; y++) {
            for (int x = 0; x < res; x++) {
                for (int i = 0; i < y; i++) {
                    newTex.SetPixel((int)(res * 0.5f) + (i / 2), y, Color.white);
                    newTex.SetPixel((int)(res * 0.5f) - (i / 2), y, Color.white);
                }
            }
        }
        newTex.Apply();
    }

    void CreatePlanks() {
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(10, 10);
        int offset = (int)(res * 0.05f);
        for (int y = 0; y < res; y++) {
            for (int x = 0; x < res; x++) {

                newTex.SetPixel(x, y, Color.white);
                if (y > (res - offset) || y < (res - (res - offset))) {
                    newTex.SetPixel(x, y, Color.black);
                }
                if (x > (res - offset) || x < (res - (res - offset))) {
                    newTex.SetPixel(x, y, Color.black);
                }
            }
        }
        newTex.Apply();
    }

    void CalcNoise() {
        for (int y = 0; y < res; y++) {
            for (int x = 0; x < res; x++) {
                if (blackwhite) {
                    noiseTex.SetPixel(x, y, Color.white * Random.Range(0.4f, 0.8f));
                } else {
                    noiseTex.SetPixel(x, y, new Color(Random.value, Random.value, Random.value));
                }

            }
        }
        noiseTex.Apply();
    }


    public void FillTexture() {

        float stepSize = 1f / res;
        for (int y = 0; y < res; y++) {
            for (int x = 0; x < res; x++) {
                newTex.SetPixel(x, y, new Color((x + 0.5f) * stepSize, (y + 0.5f) * stepSize, 0f));
            }
        }
        newTex.Apply();
    }

    public void RandomFunctions(int caseSwitch) {

        switch (caseSwitch) {
            case 1:
            for (int y = 0; y < res; y++) {
                for (int x = 0; x < res; x++) {
                    newTex.SetPixel(x, y, Color.black);
                    newTex.SetPixel(x, (int)(Mathf.Sin(x) * res), Color.white);
                }
            }
            break;
            case 2:
            for (int y = 0; y < res; y++) {
                for (int x = 0; x < res; x++) {
                    newTex.SetPixel(x, y, Color.black);
                    newTex.SetPixel(x, (int)(Mathf.Cos(x) * res), Color.white);
                }
            }
            break;
            case 3:
            int amountOfSqr = sizeOfSquare[Random.Range(0, 3)];

            for (int y = 0; y < res; y++) {
                for (int x = 0; x < res; x++) {
                    newTex.SetPixel(x, y, Color.white);
                }
            }
            for (int i = 0; i < 4; i++) {
                int counter = 0;
                if (i % 2 == 0) {
                    yCoord = ((res / 4)) + (amountOfSqr / 2);
                } else {
                    yCoord = (res - (res / 4)) + (amountOfSqr / 2);
                }
                if (i < 2) {
                    xCoord = (res / 4);
                } else {
                    xCoord = res - (res / 4);
                }

                for (int j = 0; j < amountOfSqr + 1; j++) {
                    if (j < ((amountOfSqr) / 2)) {
                        newTex.SetPixel(xCoord + j, yCoord - j, Color.red);
                        newTex.SetPixel(xCoord - j, yCoord - j, Color.red);
                    }
                    if (j >= ((amountOfSqr) / 2)) {
                        newTex.SetPixel((xCoord + j) - counter, yCoord - j, Color.red);
                        newTex.SetPixel((xCoord - j) + counter, yCoord - j, Color.red);
                        counter += 2;
                    }

                }

            }
            break;
        }
        newTex.Apply();
    }

   

    void DebugLogFunction() {
        int[] xSection = new int[] { res / 4, (res / 4) * 2, ((res / 4) * 2) + (res / 4), res };
        for (int i = 0; i < xSection.Length; i++) {
            int startY = Random.Range(50, 170);
        }
    }
}

