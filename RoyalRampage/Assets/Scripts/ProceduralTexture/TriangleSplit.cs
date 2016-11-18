using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriangleSplit : MonoBehaviour {

    public int res = 256;
    private Texture2D newTex;
    List<Coordinates> splitList = new List<Coordinates>();
    public int sizeOfSquares = 5;

    public int xTile = 10;
    public int yTile = 10;

	public Color colorTriangle1 = new Color (242f/256f, 96f/256f, 129f/256f);
	public Color colorTriangle2 = new Color (189f/256f, 140f/256f, 191f/256f);
	public Color colorDots = new Color (248f/256f, 148f/256f, 30f/256f);



    void OnEnable() {
        if (newTex == null) {
            newTex = new Texture2D(res, res, TextureFormat.ARGB32, true);

            newTex.name = "Procedural Texture";
            newTex.wrapMode = TextureWrapMode.Repeat;
            newTex.filterMode = FilterMode.Trilinear;

            GetComponent<Renderer>().material.mainTexture = newTex;
            GetComponent<Renderer>().material.mainTextureScale = new Vector2(xTile, yTile);
        }

        CreateSplitList();
        CreateTexture();
    }

    void CreateSplitList() {
        for (int i = 0; i < res; i++) {
            int y = 1 * i;
            splitList.Add(new Coordinates { X = i, Y = y });
        }
    }

    void CreateTexture() {


        for (int y = 0; y < res; y++) {
            for (int x = 0; x < res; x++) {
                if (x <= splitList[x].X && y <= splitList[x].Y) {
					newTex.SetPixel(x, y, colorTriangle1);
                } else {
					newTex.SetPixel(x, y, colorTriangle2);
                }
                if(x < sizeOfSquares && y < sizeOfSquares) {
					newTex.SetPixel(x, y, colorDots);
                }
                if (x < sizeOfSquares && y > (res - sizeOfSquares)) {
					newTex.SetPixel(x, y, colorDots);
                }
                if (x > (res - sizeOfSquares) && y > (res - sizeOfSquares)) {
					newTex.SetPixel(x, y, colorDots);
                }
                if (x > (res - sizeOfSquares) && y < sizeOfSquares) {
					newTex.SetPixel(x, y, colorDots);
                }
            }
        }
        newTex.Apply();
    }


}
