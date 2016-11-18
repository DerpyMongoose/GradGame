using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Animatic : MonoBehaviour {

	void Start ()
    {
        Handheld.PlayFullScreenMovie("AnimaticFinal.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        SceneManager.LoadScene("MainMenu");
    }
}
