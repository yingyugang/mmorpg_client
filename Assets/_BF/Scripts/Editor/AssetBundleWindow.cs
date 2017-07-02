using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
//using (new NetworkConnection(@"\\server\read", readCredentials));

public class AssetBundleWindow : EditorWindow {

	[MenuItem("Edit/Clean Cache")]
	static void CleanCache()
	{
		Caching.CleanCache();
	}

	[MenuItem("Window/AssetBundle Window")]
	static void Init()
	{
		AssetBundleWindow assetBundleWindow = EditorWindow.GetWindow<AssetBundleWindow>();
		assetBundleWindow.maximized = true;
	}

	//string heroPath = "/_BF/Resources/Hero";
	string heroPath = "/_BF/Prefabs/Hero";
//	string exportHeroPath = "D://BF_EXPORT/Heros";
	string exportHeroPath = "//192.168.1.152/htdocs/Hero";

	string scenePath = "/ArtDate/Prefeb/World";
	//	string exportHeroPath = "D://BF_EXPORT/Scenes";
	string exportScenePath = "//192.168.1.152/htdocs/Scene";

	string versionFilePath = "//192.168.1.152/htdocs";

	string heroStreamAssetPath = "/StreamingAssets/Hero";
	string sceneStreamAssetPath = "/StreamingAssets/Scene";

	Vector2 scrollPos = new Vector2(10,10);
	List<ExportAsset> exportHeros = new List<ExportAsset>();
	List<ExportAsset> exportScenes = new List<ExportAsset>();
	string versionName = "V_0_1";

    //add by hsw
    int _selectPage = 0;
    List<string> _pageIndex = new List<string>();
    Dictionary<int, List<ExportAsset>> _pageList = new Dictionary<int, List<ExportAsset>>();
    int _addCurPage = 0;

	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Hero Resource Path:",GUILayout.Width(150));
		EditorGUILayout.LabelField(heroPath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Hero Export Path:",GUILayout.Width(150));
		EditorGUILayout.LabelField(exportHeroPath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Hero Stream Export Path:",GUILayout.Width(150));
		EditorGUILayout.LabelField(heroStreamAssetPath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Scene Resource Path:",GUILayout.Width(150));
		EditorGUILayout.LabelField(scenePath);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Scene Export Path:",GUILayout.Width(150));
		EditorGUILayout.LabelField(exportScenePath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Scene Stream Export Path:",GUILayout.Width(150));
		EditorGUILayout.LabelField(sceneStreamAssetPath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Version File Path:",GUILayout.Width(150));
		EditorGUILayout.LabelField(versionFilePath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		string[] paths = null;
		if(GUILayout.Button("Load",GUILayout.Width(80)))
		{
            _selectPage = 0;
            _pageList.Clear();
            _addCurPage = 0;
            _pageIndex.Add("0");

            this.getServVersion();

			exportHeros = new List<ExportAsset>();
			paths = CommonUtility.GetFileByDirectory(heroPath);
			foreach(string str in paths)
			{
				string assetPath = "Assets" + str.Replace(Application.dataPath, "").Replace('\\', '/');
				GameObject go = AssetDatabase.LoadAssetAtPath(assetPath,typeof(GameObject)) as GameObject;
				Editor gameObjectEditor = Editor.CreateEditor(go);
				ExportAsset exportHero = new ExportAsset();
				exportHero.prefab = go;
				exportHero.editor = gameObjectEditor;
				exportHero.path = str;
				exportHeros.Add(exportHero);
                this.add2Page(exportHero);
			}
			try
			{
				paths = CommonUtility.GetFileByRemoveHeroDirectory();
				foreach(string str in paths)
				{
					foreach(ExportAsset ea in exportHeros)
					{
						if(str == ea.prefab.name)
						{
							ea.isExported = true;
							break;
						}
					}
				}
			}
			catch(Exception e)
			{
				Debug.LogError(e.Message);
			}

			exportScenes = new List<ExportAsset>();
			paths = CommonUtility.GetFileByDirectory(scenePath);
			foreach(string str in paths)
			{
				string assetPath = "Assets" + str.Replace(Application.dataPath, "").Replace('\\', '/');
				GameObject go = AssetDatabase.LoadAssetAtPath(assetPath,typeof(GameObject)) as GameObject;
				Editor gameObjectEditor = Editor.CreateEditor(go);
				ExportAsset exportScene = new ExportAsset();
				exportScene.prefab = go;
				exportScene.editor = gameObjectEditor;
				exportScene.path = str;
				exportScenes.Add(exportScene);
                this.add2Page(exportScene);
			}
		}
		if(GUILayout.Button("Export",GUILayout.Width(80)))
		{
			if(!Directory.Exists(exportHeroPath))
			{
				Directory.CreateDirectory(exportHeroPath);
			}
			ExportHeros();
			if(!Directory.Exists(exportScenePath))
			{
				Directory.CreateDirectory(exportScenePath);
			}
			ExportScenes();
			ExportVersionFile();
			EditorUtility.DisplayDialog("Message","Export Done","OK");
		}
		if(GUILayout.Button("Export Stream",GUILayout.Width(120)))
		{
			string pathHero = Application.dataPath + heroStreamAssetPath;
			if(!Directory.Exists(pathHero))
			{
				Directory.CreateDirectory(pathHero);
			}
			ExportStreamHeros();
			string pathScene = Application.dataPath + sceneStreamAssetPath;
			if(!Directory.Exists(pathScene))
			{
				Directory.CreateDirectory(pathScene);
			}
			ExportStreamScenes();
		}


		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Version:",GUILayout.Width(50));
		versionName = EditorGUILayout.TextField(versionName,GUILayout.Width(80));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		if(exportHeros.Count > 0)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Select All Heros",GUILayout.Width(150)))
			{
				foreach(ExportAsset ea in exportHeros)
				{
					ea.isSelected = true;
				}
			}
			if(GUILayout.Button("UnSelect All Heros",GUILayout.Width(150)))
			{
				foreach(ExportAsset ea in exportHeros)
				{
					ea.isSelected = false;
				}
			}

            if (GUILayout.Button("PreviousPage", GUILayout.Width(150)))
            {
                if (_selectPage > 0)
                    _selectPage--;
            }
            if (GUILayout.Button("NextPage", GUILayout.Width(150)))
            {
                if (_selectPage < _addCurPage)
                    _selectPage++;
            }

            EditorGUILayout.LabelField("Select Page:", GUILayout.Width(150));
            _selectPage = EditorGUI.Popup(new Rect(700, 0, 50, 20), _selectPage, _pageIndex.ToArray());
			EditorGUILayout.EndHorizontal();
            onPage(_selectPage);
        }
        /*
		int columnCount = 10;
		int rowCount = exportHeros.Count / columnCount + (exportHeros.Count % columnCount > 0 ? 1 : 0);

		for(int i =0;i < rowCount;i ++)
		{	
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			for(int j=0;j < columnCount;j ++)
			{
				int index = i * columnCount + j;
				if(index <  exportHeros.Count)
				{
					if(exportHeros[index].isExported)
					{
						GUI.color = Color.blue;
					}else{
						GUI.color = Color.white;
					}
					NGUIEditorTools.BeginContents();
					EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
					GUI.color = Color.white;
					EditorGUILayout.TextField(exportHeros[index].prefab.name,GUILayout.Width(70));
					exportHeros[index].isSelected = EditorGUILayout.Toggle(exportHeros[index].isSelected,GUILayout.Width(30));
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
					exportHeros[index].editor.OnPreviewGUI(GUILayoutUtility.GetRect(50, 100),GUIStyle.none);
					NGUIEditorTools.EndContents();
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		columnCount = 3;
		EditorGUILayout.Space();
		if(exportScenes.Count > 0)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Select All Scene",GUILayout.Width(150)))
			{
				foreach(ExportAsset ea in exportScenes)
				{
					ea.isSelected = true;
				}
			}
			if(GUILayout.Button("UnSelect All Scene",GUILayout.Width(150)))
			{
				foreach(ExportAsset ea in exportScenes)
				{
					ea.isSelected = false;
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		for(int i =0;i < rowCount;i ++)
		{	
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			for(int j=0;j < columnCount;j ++)
			{
				int index = i * columnCount + j;
				if(index <  exportScenes.Count)
				{
					EditorGUILayout.BeginVertical(GUILayout.Width(40));
					EditorGUILayout.LabelField(exportScenes[index].prefab.name,GUILayout.Width(80));
					exportScenes[index].isSelected = EditorGUILayout.Toggle(exportScenes[index].isSelected,GUILayout.Width(30));
					EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical(GUILayout.Width(400));
					exportScenes[index].editor.OnPreviewGUI(GUILayoutUtility.GetRect(200, 400),GUIStyle.none);
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndHorizontal();
		}
         */
		EditorGUILayout.EndScrollView();
	}

    void getServVersion()
    {
        string[] files = Directory.GetFiles(@"\\192.168.1.152\htdocs", "*.unity3d");
        foreach (string file in files)
        {
            string strVer;
            int index = file.LastIndexOf('V');
            if (index > 0)
            {
                versionName = "";
                strVer = file.Substring(index);
                index = strVer.LastIndexOf('.');
                if (index > 0)
                    this.versionName = strVer.Substring(0,index);
            }
        }
    }

    void onPage(int page)
    {
        List<ExportAsset> curPage = getPage(page);
        if (curPage==null)
            return;

        int columnCount = 7;
        int rowCount = curPage.Count / columnCount + (curPage.Count % columnCount > 0 ? 1 : 0);
        for (int i = 0; i < rowCount; i++)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < columnCount; j++)
            {
                int index = i * columnCount + j;
                if (index < curPage.Count)
                {
                    EditorGUILayout.BeginVertical();
                    if(curPage[index].isExported) 
                        GUI.color = Color.blue;
                    else
                        GUI.color = Color.white;
                    NGUIEditorTools.BeginContents();
                    EditorGUILayout.BeginHorizontal();
                    GUI.color = Color.white;
                    NGUIEditorTools.BeginContents();
                    EditorGUILayout.LabelField(curPage[index].prefab.name, GUILayout.Width(80));
                    NGUIEditorTools.EndContents();
                    curPage[index].isSelected = EditorGUILayout.Toggle(curPage[index].isSelected, GUILayout.Width(30));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginVertical(GUILayout.Width(100));
                    GUI.color = Color.white;
                    curPage[index].editor.OnPreviewGUI(GUILayoutUtility.GetRect(50, 100), GUIStyle.none);
                    EditorGUILayout.EndVertical();
                    NGUIEditorTools.EndContents();
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void add2Page(ExportAsset obj)
    {
        this._pageIndex.Clear();
        for(int index =0;index<=this._addCurPage;index++)
            this._pageIndex.Add(index.ToString());
        List<ExportAsset> curPage = this.getPage(this._addCurPage);
        if (curPage.Count >= 28)//最多30个
        {
            this._addCurPage++;
            add2Page(obj);
        }
        else
        {
            curPage.Add(obj);
        }
    }

    List<ExportAsset> getPage(int page)
    {
        if (this._pageList.ContainsKey(page))
        {
            return this._pageList[page];
        }
        else
        {
            this._pageList[page] = new List<ExportAsset>();
            return this._pageList[page];
        }
    }

	void ExportStreamHeros()
	{
		for(int i=0;i<exportHeros.Count;i++)
		{
			if(exportHeros[i].isSelected)
			{
				Export(exportHeros[i].prefab,heroStreamAssetPath);
			}
		}
	}

	void ExportStreamScenes()
	{
		for(int i=0;i<exportScenes.Count;i++)
		{
			if(exportScenes[i].isSelected)
			{
				Export(exportScenes[i].prefab,sceneStreamAssetPath);
			}
		}
	}

	void ExportHeros()
	{
		for(int i=0;i<exportHeros.Count;i++)
		{
			if(exportHeros[i].isSelected)
			{
				Export(exportHeros[i].prefab,exportHeroPath);
			}
		}
	}

	void ExportScenes()
	{
		for(int i=0;i<exportScenes.Count;i++)
		{
			if(exportScenes[i].isSelected)
			{
				Export(exportScenes[i].prefab,exportScenePath);
			}
		}
	}

	void ExportVersionFile()
	{
		GameObject go = new GameObject();
		VersionInfo versionInfo = go.AddComponent<VersionInfo>();
		versionInfo.heros = new List<string>();
		for(int i=0;i<exportHeros.Count;i++)
		{
			if(exportHeros[i].isSelected)
			{
				versionInfo.heros.Add(exportHeros[i].prefab.name); 
			}
		}
		versionInfo.scenes = new List<string>();
		for(int i=0;i<exportScenes.Count;i++)
		{
			if(exportScenes[i].isSelected)
			{
				versionInfo.scenes.Add(exportScenes[i].prefab.name); 
			}
		}
		go.name = versionName;
		GameObject prefab = PrefabUtility.CreatePrefab("Assets/_BF/Prefabs/" + go.name + ".prefab",go);
		prefab.name = versionName;
		Export(prefab,versionFilePath);
	}

//	void ExportList(Object[] objs)
//	{
//#if UNITY_STANDALONE || UNITY_WEBPLAYER
//		string path = Application.dataPath.Replace("/Assets","") + "/" + versionName + ".unity3d";
//		BuildPipeline.BuildAssetBundle(objs[0], objs, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
//#elif UNITY_ANDROID 
//		string path = Application.dataPath.Replace("/Assets","") + "/" + versionName + "_Android" + ".unity3d";
//		BuildPipeline.BuildAssetBundle(objs[0], objs, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.Android);
//#elif UNITY_IPHONE 
//		string path = Application.dataPath.Replace("/Assets","") + "/" + versionName + "_IPhone" + ".unity3d";
//		BuildPipeline.BuildAssetBundle(objs[0], objs, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.iPhone);
//#endif
//		Caching.CleanCache();
//	}

	void Export(UnityEngine.Object obj,string basePath)
	{
#if UNITY_STANDALONE || UNITY_WEBPLAYER
		string path = basePath + "/" + obj.name + ".unity3d";
		BuildPipeline.BuildAssetBundle(obj, null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.StandaloneWindows);
#elif UNITY_ANDROID 
		string path = basePath + "/" + obj.name + "_Android" + ".unity3d";
		BuildPipeline.BuildAssetBundle(obj, null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.Android);
#elif UNITY_IPHONE 
		string path = basePath + "/" + obj.name + "_IPhone" + ".unity3d";
		BuildPipeline.BuildAssetBundle(obj, null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.iPhone);
#endif
		Caching.CleanCache();
	}

	class ExportAsset
	{
		public GameObject prefab;
		public Editor editor;
		public string path;
		public bool isSelected;
		public bool isExported;
	}

}
