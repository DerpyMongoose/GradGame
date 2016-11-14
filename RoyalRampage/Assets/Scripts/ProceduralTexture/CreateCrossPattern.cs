using UnityEngine;
using System.Collections;

public class CreateCrossPattern : MonoBehaviour {

    public int res = 256;

    private Texture2D newTex;

    public int xTile = 10;
    public int yTile = 10;

    void OnEnable() {
        if (newTex == null) {
            newTex = new Texture2D(res, res, TextureFormat.ARGB32, true);

            newTex.name = "Procedural Texture";
            newTex.wrapMode = TextureWrapMode.Repeat;
            newTex.filterMode = FilterMode.Trilinear;

            GetComponent<Renderer>().material.mainTexture = newTex;
            GetComponent<Renderer>().material.mainTextureScale = new Vector2(xTile, yTile);
        }

    }

    void CrossPattern() {
        for (int y = 0; y < res; y++) {
            for (int x = 0; x < res; x++) {

            }
        }
    }
}
