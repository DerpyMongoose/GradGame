using UnityEngine;
using UnityEditor;

public class EditorList {

	public static void Show(SerializedProperty list) {
		EditorGUILayout.PropertyField (list);
			Debug.Log ("in editor");
		for(int i = 0; i < list.arraySize; i++){
			Debug.Log("reached editor");
			EditorGUILayout.PropertyField (list.GetArrayElementAtIndex(i));
		}

	}
}
