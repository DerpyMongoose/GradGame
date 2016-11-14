using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralBlackMarbleTwo : MonoBehaviour {

    public int res = 256;

    private Texture2D newTex;
    private Texture2D noiseTex;
    public float frequency = 1f;

    [Range(1, 8)]
    public int octaves = 1;

    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Range(0f, 1f)]
    public float persistence = 0.5f;

    [Range(1, 3)]
    public int dimensions = 3;

    public Gradient coloring;

    public int xTile = 10;
    public int yTile = 10;
    public int offset = 3;
    private const int hashMask = 255;

    private static List<int> hashList = new List<int>();

    private static float Smooth(float t) {
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }

    public static float Value2D(Vector3 point, float frequency) {
        point *= frequency;
        int ix0 = Mathf.FloorToInt(point.x);
        int iy0 = Mathf.FloorToInt(point.y);
        float tx = point.x - ix0;
        float ty = point.y - iy0;
        ix0 &= hashMask;
        iy0 &= hashMask;
        int ix1 = ix0 + 1;
        int iy1 = iy0 + 1;

        int h0 = hashList[ix0];
        int h1 = hashList[ix1];
        int h00 = hashList[h0 + iy0];
        int h10 = hashList[h1 + iy0];
        int h01 = hashList[h0 + iy1];
        int h11 = hashList[h1 + iy1];

        tx = Smooth(tx);
        ty = Smooth(ty);
        return Mathf.Lerp(
            Mathf.Lerp(h00, h10, tx),
            Mathf.Lerp(h01, h11, tx),
            ty) * (1f / hashMask);
    }

    void OnEnable() {
        if (newTex == null) {
            newTex = new Texture2D(res, res, TextureFormat.ARGB32, true);

            newTex.name = "Procedural Texture";
            newTex.wrapMode = TextureWrapMode.Repeat;
            newTex.filterMode = FilterMode.Trilinear;

            GetComponent<Renderer>().material.mainTexture = newTex;
            GetComponent<Renderer>().material.mainTextureScale = new Vector2(xTile, yTile);
        }
        GenerateList();
        CreateMarble();

    }

    void CreateMarble() {

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        float stepSize = 1f / res;

        for (int y = 0; y < res; y++) {

            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);

            for (int x = 0; x < res; x++) {
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                //Random white dots on a black texture
                /*newTex.SetPixel(x, y, Color.black);
                newTex.SetPixel(x, (int)(Mathf.Cos(x) * res), Color.white);*/

                float sample = Sum(point, frequency, octaves, lacunarity, persistence);

                newTex.SetPixel(x, y, coloring.Evaluate(sample));
                if (y >= res - offset || (y >= 0 && y <= offset) ||
                    x >= res - offset || (x >= 0 && x <= offset)) {
                    newTex.SetPixel(x, y, new Color (0.3f, 0.3f, 0.3f));
                }
            }
        }
        newTex.Apply();
    }


    float Sum (Vector3 point, float frequency, int octaves, float lacunarity, float persistence) {
        float sum = Value2D(point, frequency);
        float amplitude = 1f;
        float range = 1f;

        for (int i = 0; i < octaves; i++) {
            frequency *= lacunarity;
            amplitude *= persistence;
            range += amplitude;
            sum += Value2D(point, frequency) * amplitude;
        }
        return sum / range;
    }

    private void GenerateList () {
        for(int i = 0; i < hashMask + 1; i++) {
            hashList.Add(Random.Range(0, 256));
        }
        int size = hashList.Count;
        for (int i = 0; i < size; i++) {
            hashList.Add(hashList[i]);
        }
    }

}
