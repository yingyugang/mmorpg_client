using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;

public class BuildWindow{

	[MenuItem("File/Build Exe")]
	public static void BuildGame ()
	{
		PlayerPrefs.SetInt(AssetBundleMgr.bfVersionSubName,0);
		// Get filename.
		string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
		string[] levels = new string[] {"Assets/_BF/Scenes/Login.unity", "Assets/_BF/Scenes/Main.unity", "Assets/_BF/Scenes/Battle.unity", "Assets/_BF/Scenes/Arena.unity"};
		
		// Build player.
		BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
		
		// Copy a file from the project folder to the build folder, alongside the built game.
//		FileUtil.CopyFileOrDirectory("Assets/WebPlayerTemplates/Readme.txt", path + "Readme.txt");
		
		// Run the game (Process class from System.Diagnostics).
		Process proc = new Process();
		proc.StartInfo.FileName = path + "BuiltGame.exe";
		proc.Start();
	}
}
