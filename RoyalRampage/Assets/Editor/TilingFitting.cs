using UnityEngine;
using UnityEditor;
using System.Collections;
namespace Assets.Editor
{
	public class TilingFitting : EditorWindow
	{
		[MenuItem("Fitting/Fit texture to GO #&f")]
		static void TestFunc()
		{
			if (Selection.transforms.Length == 0)
			{
				MonoBehaviour.print("Must Have a selection to perform Fitting");
				return;
			}
			GameObject selectedGO = Selection.activeTransform.gameObject;


			Material GOMat = selectedGO.GetComponent<Renderer>().material;

			GOMat.mainTextureScale = new Vector2(selectedGO.transform.localScale.x, -1);


		}

	}
}