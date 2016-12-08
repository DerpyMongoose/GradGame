using UnityEngine;
using System.Collections;

public class ChangeRageFaces : MonoBehaviour {

    public GameObject[] faces;

    private float percentage;
    // Use this for initialization

    void Start() {
        faces[1].SetActive(false);
        faces[2].SetActive(false);
        percentage = 0f;
    }

    // Update is called once per frame
    void Update() {
        percentage = GameManager.instance.player.GetComponent<StampBar>().slider.value * 100f;

        if (percentage >= 0 && percentage < 50f) {
            faces[0].SetActive(true);
            faces[1].SetActive(false);
            faces[2].SetActive(false);
        } else if (percentage >= 50f && percentage < 99f) {
            faces[0].SetActive(false);
            faces[1].SetActive(true);
            faces[2].SetActive(false);
        } else {
            faces[0].SetActive(false);
            faces[1].SetActive(false);
            faces[2].SetActive(true);
        }
    }
}
