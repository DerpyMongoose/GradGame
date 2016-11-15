using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralTextureManager{

    private static ProceduralTextureManager _instance;

    private const int hashMask = 255;

    private static List<int> hashList;

    public static ProceduralTextureManager instance {
        get {
            if(_instance == null) {
                _instance = new ProceduralTextureManager();
                _instance.GenerateList();
            }
            return _instance;
        }
    }

    //Smooth function, with second derivative with 0 at gradient boundaries.
    private static float Smooth(float t) {
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }

    //2D noise to interpolate along both x and y
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

    //Function to make fractal noise
    //First the point we want to interpolate between
    //Then how many samples we want to sum (octaves)
    // Lacunarity and persistence control the frequency and amplitude respectively
    public float Sum(Vector3 point, float frequency, int octaves, float lacunarity, float persistence) {
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


    //Create a random hashlist
    private void GenerateList() {
        hashList = new List<int>();
        for (int i = 0; i < hashMask + 1; i++) {
            hashList.Add(Random.Range(0, 256));
        }
        //repeat the list
        int size = hashList.Count;
        for (int i = 0; i < size; i++) {
            hashList.Add(hashList[i]);
        }
    }
}
