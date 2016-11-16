using UnityEngine;
using System.Collections;

public class WhiteAndGreenMarble : MonoBehaviour {

    public int res = 256;

    public Color edges;

    private Texture2D newTex;

    public float frequency = 1f;

    [Range(1, 8)]
    public int octaves = 1;

    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Range(0f, 1f)]
    public float persistence = 0.5f;


    public Gradient coloring;

    public int xTile = 10;
    public int yTile = 10;
    private int offset;


    void OnEnable() {
        offset = res / 8;
        if (newTex == null) {

            //Create the texture
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

        //Visualize world coordinates
        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        float stepSize = 1f / res;

        for (int y = 0; y < res; y++) {
            //interpolate between two points
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);

            for (int x = 0; x < res; x++) {
                //Interpolate again to get bilinear interpolation
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);

                //Calculate the noise
                float sample = ProceduralTextureManager.instance.Sum(point, frequency, octaves, lacunarity, persistence);

                //Set the color with a gradient
                newTex.SetPixel(x, y, coloring.Evaluate(sample));
                if (y >= res - offset || (y >= 0 && y <= offset) ||
                   x >= res - offset || (x >= 0 && x <= offset)) {

                    //Set a whiteish color at the edges
                    newTex.SetPixel(x, y, edges);
                }
            }
        }
        newTex.Apply();
    }
}
