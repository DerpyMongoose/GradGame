using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Animatic : MonoBehaviour {

	void Start ()
    {
        Handheld.PlayFullScreenMovie("Cutscene.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        //SceneManager.LoadScene("MainMenu");
		GameManager.instance.GoToMainMenu();
    }
}
