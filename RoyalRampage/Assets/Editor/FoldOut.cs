using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Testeditor))]
public class FoldOut : Editor {

	private SerializedProperty _dashPlayer;
	private SerializedObject _object;

	private static bool ExpandProperties = true;
	private static AudioManager _target;

	public void OnEnable(){
		_object = new SerializedObject (target);
		_dashPlayer = _object.FindProperty ("test");
	}

	public override void OnInspectorGUI(){
		_object.Update ();
		EditorGUILayout.PropertyField (_dashPlayer);
		_object.ApplyModifiedProperties ();
		/*_target = target as AudioManager;
		ExpandProperties = EditorGUILayout.Foldout (ExpandProperties, "Fields");*/
	}
}
