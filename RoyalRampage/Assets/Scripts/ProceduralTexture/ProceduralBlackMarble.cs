using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralBlackMarble : MonoBehaviour {

    public int res = 256;

    private Texture2D newTex;

    private float xPeriod = 5f;
    private float yPeriod = 10f;

    private float turbPower = 5.0f;
    private float turbSize = 32.0f;

    public int xTile = 10;
    public int yTile = 10;
    public int offset = 3;
    private const int noiseRes = 256;
    float[,] noiseArray = new float[noiseRes, noiseRes];

    void OnEnable() {
        if (newTex == null) {
            newTex = new Texture2D(res, res, TextureFormat.ARGB32, true);
            
            newTex.name = "Procedural Texture";
            newTex.wrapMode = TextureWrapMode.Repeat;
            newTex.filterMode = FilterMode.Trilinear;

            GetComponent<Renderer>().material.mainTexture = newTex;
            GetComponent<Renderer>().material.mainTextureScale = new Vector2(xTile, yTile);
        }
        CreateMarble();

    }

    void CreateMarble() {
        
        for (int y = 0; y < res; y++) {
            for (int x = 0; x < res; x++) {
                float xyVal = (x * xPeriod / noiseRes) + (y * yPeriod / noiseRes) + (turbPower * Turbulence(x, y, turbSize) / 256.0f);
                float sineVal = (256f * Mathf.Sin(xyVal * 3.14159f)) / 256.0f;

                Color col = new Color(sineVal, sineVal, sineVal);
                newTex.SetPixel(x,y,col);
                //Random white dots on a black texture
                /*newTex.SetPixel(x, y, Color.black);
                newTex.SetPixel(x, (int)(Mathf.Cos(x) * res), Color.white);*/


                if(y > res - offset || (y > 0 && y < offset) ||
                    x > res - offset || (x > 0 && x < offset)) {
                    newTex.SetPixel(x,y,Color.white);
                }
            }
        }
        newTex.Apply();
    }


    void GenerateNoise () {
        for(int y = 0; y < noiseRes; y++) {
            for(int x = 0; x < noiseRes; x++) {
                noiseArray[x, y] = (Random.Range(0, 32768)) / 32768.0f;
            }
        }
    }

    float SmoothNoise (float x, float y) {
        float fractX = x - (int)x;
        float fractY = y - (int)y;

        int x1 = ((int)x + noiseRes) % noiseRes;
        int y1 = ((int)y + noiseRes) % noiseRes;

        int x2 = (x1 + noiseRes - 1) % noiseRes;
        int y2 = (y1 + noiseRes - 1) % noiseRes;

        float val = 0f;
        val += fractX * fractY + noiseArray[y1, x1];
        val += (1-fractX) * fractY + noiseArray[y1, x2];
        val += fractX * (1-fractY) + noiseArray[y2, x1];
        val += (1-fractX) * (1-fractY) + noiseArray[y2, x2];

        return val;
    }

    float Turbulence(float x, float y, float size) {
        float val = 0f;
        float initialSize = size;

        while(size >= 1) {
            val += SmoothNoise(x / size, y / size) * size;
            size /= 2.0f;
        }

        return (128.0f * val / initialSize);
    }
}
