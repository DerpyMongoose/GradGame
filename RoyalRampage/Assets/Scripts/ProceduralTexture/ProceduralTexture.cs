using UnityEngine;
using System.Collections;

public class ProceduralTexture : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

        Texture2D newTex = new Texture2D(200, 200);
        for (int i = 0; i <= 100; i++) {
            for(int j = 0; j <= 100; j++) {
                newTex.SetPixel(j, i, Color.blue);
            }
        }

        for (int i = 101; i <= 200; i++) {
            for (int j = 101; j <= 200; j++) {
                newTex.SetPixel(j, i, Color.green);
            }
        }

        newTex.Apply();

        GetComponent<Renderer>().material.mainTexture = newTex;
    }
	
}
