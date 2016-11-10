using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor {
	//private static bool showHexTypes = true;

	public override void OnInspectorGUI(){
		serializedObject.Update ();
		EditorList.Show (serializedObject.FindProperty ("test"));
		EditorList.Show (serializedObject.FindProperty ("hello"));
		serializedObject.ApplyModifiedProperties ();
	}
}
