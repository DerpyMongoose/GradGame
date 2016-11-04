using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System;

public class ProjectBuilder {

	static string[] SCENES = FindEnabledEditorScenes();
	//static string path = "C:/Builds";

	static string APP_NAME = "T1DestructionGame";
	static string baseFolder = "C:/Users/dadiu/Google Drive/QA - TEAM1/GGBuilds";
	static string buildFolder = System.DateTime.Now.ToString ("dd-MM-yy HH.mm");


	public static void BuildProject(){

		try {
			float version = float.Parse(PlayerSettings.bundleVersion);

			buildFolder = "V" + version + "_" + buildFolder;
			Directory.CreateDirectory(baseFolder + "/" + buildFolder);

			//build apk
			string target_dir = buildFolder + "/" + APP_NAME + "_V" + version + ".apk";
			GenericBuild(SCENES, baseFolder + "/" + target_dir, BuildTarget.Android,BuildOptions.None);
			//copy unity's log file
			FileUtil.CopyFileOrDirectory("C:/Users/dadiu/AppData/Local/Unity/Editor/Editor.log", baseFolder + "/" + buildFolder + "/log.txt");

			version++;
			PlayerSettings.bundleVersion = version.ToString();

		} catch (UnityException e){
			StreamWriter fil = new StreamWriter (baseFolder + "/" + buildFolder + "/unity_errors.txt", true);
			fil.Write (e.Message);
			fil.Close ();
		} catch (Exception e){
			StreamWriter fil = new StreamWriter (baseFolder + "/" + buildFolder + "/general_errors.txt", true);
			fil.Write (e.Message);
			fil.Close ();
		}
	}


	private static string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}

	static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);
		if (res.Length > 0) {
			throw new Exception("BuildPlayer failure: " + res);
		}
	}

}