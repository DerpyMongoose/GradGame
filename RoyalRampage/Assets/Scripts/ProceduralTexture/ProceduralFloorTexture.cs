using UnityEngine;
using System.Collections;

public class ProceduralFloorTexture : MonoBehaviour {

    private Texture2D newTex;
    public int res = 256;
    private Color[] floorColor = new Color[] {
        new Color(0.627f,0.322f,0.176f) * 0.33f,new Color(0.545f,0.271f,0.075f) * 0.5f,
    new Color(0.627f,0.322f,0.176f) * 0.5f, new Color(0.627f,0.322f,0.176f) * 0.25f};

    public int xTile = 10;
    public int yTile = 10;
    void OnEnable () {
        newTex = new Texture2D(res, res, TextureFormat.ARGB32, true);

        newTex.wrapMode = TextureWrapMode.Repeat;
        newTex.filterMode = FilterMode.Trilinear;
        GetComponent<Renderer>().material.mainTexture = newTex;
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(xTile,yTile);
        FloorPlanks();
    }

    public void FloorPlanks() {
        int[] xSection = new int[] { res / 4, (res / 4) * 2, ((res / 4) * 2) + (res / 4), res };
        int offset = 3;

        for (int i = 0; i < xSection.Length; i++) {
            int startX;
            if (i == 0) {
                startX = 0;
            } else {
                startX = xSection[i - 1];
            }
            int startY = Random.Range(120, 150);
            //int remainY = 256 - startY;
            Color sectionOneCol = floorColor[Random.Range(0, 4)];
            Color sectionTwoCol = floorColor[Random.Range(0, 4)];
            if (i == 0) {
                for (int x = startX; x < xSection[i]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= startY) {
                            newTex.SetPixel(x, y, sectionOneCol);
                          if (y > startY - offset) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                        } else {
                            newTex.SetPixel(x, y, sectionTwoCol);                            
                            if (y > res - offset) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                        }
                    }
                }
            }
            if (i < 3 && i > 0) {
                for (int x = startX; x < xSection[i + 1]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= startY) {
                            newTex.SetPixel(x, y, sectionOneCol);
                            if (x < (startX + offset) || x > (xSection[i + 1] - offset)) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                            if (y > startY - offset) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                        } else {
                            newTex.SetPixel(x, y, sectionTwoCol);
                            if (x < (startX + offset) || x > (xSection[i + 1] - offset)) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                            if (y > res - offset) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                        }
                    }
                }
            }
            if (i == 3) {
                for (int x = startX; x < xSection[i]; x++) {
                    for (int y = 0; y < res; y++) {
                        if (y <= startY) {
                            newTex.SetPixel(x, y, sectionOneCol);
                            if (x < (startX + offset) || x > (xSection[i] - offset)) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                            if (y > startY - offset) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                        } else {
                            newTex.SetPixel(x, y, sectionTwoCol);
                            if (x < (startX + offset) || x > (xSection[i] - offset)) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                            if (y > res - offset) {
                                newTex.SetPixel(x, y, Color.black);
                            }
                        }
                    }
                }
            }
        }
        newTex.Apply();
    }
}
