using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBundleInfoWindow : EditorWindow
{

	string message = "关于自动abname设置：\n" +
		"所有需要进行ab打包的文件，放到AssetBundleResources文件夹下面。注意不会处理文件间的依赖关系。\n" +
		"AssetBundleResources下面的一级分类：Images，Sounds，Characters等等，用于文件类型分类。\n" +
		"AssetBundleResources下面的二级分类：Images/SkillIcons , Characters/Unit1等，用来存放需要打包的文件和文件夹。\n" +
		"自动abname设置的时候会以一级分类文件夹name+“/”+ 二级文件夹name + “.后缀名”来设置。这样第一规范，便于程序化设置，第二abname不会重复。\n" +
		"TODO 清除子目录的资源存在的abname。";

	static void Init ()
	{ 
//		EditorWindow.GetWindow<AssetBundleInfoWindow> (true, "AssetBundleInfo", true); 
	}

	void OnGUI ()
	{
		GUILayout.Label (message, GUILayout.Width (800),GUILayout.Height(600));
	}


}