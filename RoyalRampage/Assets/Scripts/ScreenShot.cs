using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour {

	private Texture2D screenShot;
	private int resWidth;
	private int resHeight;

	void OnEnable(){
		GameManager.instance.OnScoreScreenOpen += SaveBG;
	}

	void OnDisable(){
		GameManager.instance.OnScoreScreenOpen -= SaveBG;
	}

	private void SaveBG(){
		StartCoroutine (TakeScreenShot());
	}

	private IEnumerator TakeScreenShot(){
		yield return new WaitForEndOfFrame();

		resWidth = Screen.width;
		resHeight = Screen.height;

		screenShot= new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
		RenderTexture renderTexture = new RenderTexture(resWidth, resHeight, 24);

		Camera.main.targetTexture = renderTexture;
		RenderTexture.active = renderTexture;
		Camera.main.Render();

		screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
		screenShot.Apply(false);

		RenderTexture.active = null;
		Camera.main.targetTexture = null;
		Destroy(renderTexture);

		Sprite new_bgSprite = Sprite.Create(screenShot,new Rect(0,0,resWidth,resHeight),new Vector2(0,0));
		GameManager.instance.menu_bg_sprite = new_bgSprite;
	}
}
