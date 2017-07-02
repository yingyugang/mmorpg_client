using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class ArtDataMgrWindow : EditorWindow {

	[MenuItem("Window/ArtData Window")]
	static void Init()
	{
		string path = "Assets/ArtDate/Prefeb/Hero";
		Object  pathObject;
		EditorWindow.GetWindow<ArtDataMgrWindow>();
		pathObject = AssetDatabase.LoadAssetAtPath(path,typeof(Object));
		EditorGUIUtility.PingObject(pathObject);
	}
	string heroResPath = "/_BF/Prefabs/Hero";
	string heroPath = "/ArtDate/Prefeb/Hero";
	string outputPath = "Assets/_BF/Prefabs/Hero";
	List<ExportAsset> artHeros = new List<ExportAsset>();
	Dictionary<GameObject,GameObject> InitPrefabDic = new Dictionary<GameObject, GameObject>();
	Vector2 scrollPos;
	Object selectObj;

	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Hero Resource Path:",GUILayout.Width(150));
		EditorGUILayout.LabelField(heroPath);
		EditorGUILayout.EndHorizontal();
	
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Load",GUILayout.Width(80)))
		{
			artHeros = new List<ExportAsset>();
			string[] paths = CommonUtility.GetFileByDirectory(heroPath);
			GameObject tempGoP = new GameObject();
			tempGoP.name = "TempGameObject";
			foreach(string str in paths)
			{
				string assetPath = "Assets" + str.Replace(Application.dataPath, "").Replace('\\', '/');
				GameObject go = AssetDatabase.LoadAssetAtPath(assetPath,typeof(GameObject)) as GameObject;
//				go.transform.eulerAngles = Vector3.zero;
//				GameObject tmpGo = PrefabUtility.InstantiatePrefab(go) as GameObject;
				GameObject pGo = go;
//				pGo.transform.parent = tempGoP.transform;
//				tmpGo.transform.parent = pGo.transform;
				Editor gameObjectEditor = Editor.CreateEditor(pGo);
				ExportAsset exportHero = new ExportAsset();
				exportHero.prefab = go;
				exportHero.editor = gameObjectEditor;
				exportHero.path = assetPath;
				artHeros.Add(exportHero);
			}
			List<ArtConfig> acs =  ArtConfigInfo.GetHeroList();
			foreach(ArtConfig ac in acs)
			{
				foreach(ExportAsset exportHero in artHeros)
				{
					if(exportHero.prefab.name == ac.heroName)
					{
						exportHero.heroId = ac.heroId;
					}
				}
			}
			paths = CommonUtility.GetFileByDirectory(heroResPath);
			InitPrefabDic.Clear();
			foreach(string str in paths)
			{
				string assetPath = "Assets" + str.Replace(Application.dataPath, "").Replace('\\', '/');
				GameObject go = AssetDatabase.LoadAssetAtPath(assetPath,typeof(GameObject)) as GameObject;
				if(go.GetComponent<HeroRes>()!=null)
				{
					foreach(ExportAsset exportHero in artHeros)
					{
						if(go.name == exportHero.heroId)
						{
							exportHero.initedPrefab = go;
							exportHero.isCreated = true;
						}
					}
				}
			}
			InitTable();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Create",GUILayout.Width(80)))
		{
			bool createable = true;
			foreach(ExportAsset ea in artHeros)
			{
				if(ea.isSelected)
				{
					if(ea.heroId == null || ea.heroId.Trim() == "")
					{
						createable = false;
						EditorUtility.DisplayDialog("Warning","Please enter the heroId for " + ea.prefab.name,"OK");
						break;
					}
				}
			}
			if(createable)
			{
				foreach(ExportAsset ea in artHeros)
				{
					if(ea.isSelected)
					{
						GameObject go = null;
						if(ea.isCreated)
						{
							go = Instantiate(ea.initedPrefab) as GameObject;
							go.name = ea.initedPrefab.name;
							HeroRes heroRes = go.GetComponent<HeroRes>();
							heroRes.BodyPrefabs = new List<GameObject>();
							heroRes.BodyPrefabs.Add(ea.prefab);
							HeroResEditor.LoadHeroRes(heroRes);
						}else{
							go = new GameObject();
							go.name = ea.heroId;
							go.AddComponent<AudioSource>();
							HeroRes heroRes = go.AddComponent<HeroRes>();
							heroRes.BodyPrefabs = new List<GameObject>();
							heroRes.HitPoints = new List<Transform>();
							heroRes.BodyPrefabs.Add(ea.prefab);
							HeroResEditor.LoadHeroRes(heroRes);
							BoxCollider2D c2d = go.AddComponent<BoxCollider2D>();
							c2d.size = new Vector2(2,3);
							c2d.offset = new Vector2(0,3);
							HeroResEffect heroResEffect = go.AddComponent<HeroResEffect>();
							BattleUtility.SetHeroResEffectInitValue(heroResEffect);
						}
						PrefabUtility.CreatePrefab(outputPath + "/" + ea.heroId + ".prefab",go,ReplacePrefabOptions.Default);
					}
				}
			}
			Caching.CleanCache();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Select All Heros",GUILayout.Width(150)))
			{
				foreach(ExportAsset ea in artHeros)
				{
					if(ea.heroId!=null && ea.heroId.Trim()!="")
					{
						ea.isSelected = true;
					}
				}
			}
			if(GUILayout.Button("UnSelect All Heros",GUILayout.Width(150)))
			{
				foreach(ExportAsset ea in artHeros)
				{
					ea.isSelected = false;
				}
			}
			if(GUILayout.Button("PreviousPage",GUILayout.Width(150)))
			{
				PreviousPage();
			}
			if(GUILayout.Button("NextPage",GUILayout.Width(150)))
			{
				NextPage();
			}
			EditorGUILayout.LabelField("Current Page:" + (mPageCount > 0 ? mCurrentPage + 1 : mPageCount) + "/" + mPageCount);

			EditorGUILayout.EndHorizontal();
		}

//		int columnCount = 10;
//		int rowCount = artHeros.Count / columnCount + (artHeros.Count % columnCount > 0 ? 1 : 0);

		if(mCurrentPageAssets!=null && mCurrentPageAssets.Count > 0)
		{
			for(int i =0;i < mRowPerPage;i ++)
			{	
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				for(int j=0;j < mColumnPerPage ;j ++)
				{
					int index = i * mColumnPerPage + j;
					if(index <  mCurrentPageAssets.Count)
					{
						if(mCurrentPageAssets[index].isCreated)
						{
							GUI.color = Color.blue;
						}
						else
						{
							GUI.color = Color.white;
						}
						EditorGUILayout.BeginVertical(GUILayout.Width(100));
						NGUIEditorTools.BeginContents();
						EditorGUILayout.BeginVertical(GUILayout.Width(100));
						GUI.color = Color.white;
						mCurrentPageAssets[index].editor.OnPreviewGUI(GUILayoutUtility.GetRect(50, 100),GUIStyle.none);
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(GUILayout.Width(40));
						EditorGUILayout.TextField(mCurrentPageAssets[index].prefab.name,GUILayout.Width(100));
						mCurrentPageAssets[index].heroId = EditorGUILayout.TextField(mCurrentPageAssets[index].heroId,GUILayout.Width(80));
						mCurrentPageAssets[index].isSelected = EditorGUILayout.Toggle(mCurrentPageAssets[index].isSelected,GUILayout.Width(30));
						if(GUILayout.Button("Select"))
						{
							selectObj = AssetDatabase.LoadAssetAtPath(mCurrentPageAssets[index].path,typeof(Object));
							EditorGUIUtility.PingObject(selectObj);
						}
						EditorGUILayout.EndVertical();
						NGUIEditorTools.EndContents();
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndScrollView();
	}

	int mPageCount;
	int mCurrentPage;
	List<ExportAsset> mCurrentPageAssets;
	int mRowPerPage = 4;
	int mColumnPerPage = 10;
	
	void InitTable()
	{
		mPageCount = artHeros.Count / (mRowPerPage * mColumnPerPage) + (artHeros.Count % (mRowPerPage * mColumnPerPage) > 0 ? 1 : 0);
		SetPage(0);
	}

	bool SetPage(int index)
	{
		Debug.Log("SetPage" + index);
		Debug.Log("artHeros:" + artHeros.Count);
		index = Mathf.Abs(index);
		if(index * mRowPerPage * mColumnPerPage < artHeros.Count)
		{
			if((index + 1) * mRowPerPage * mColumnPerPage >= artHeros.Count)
			{
				Debug.Log(index * mRowPerPage * mColumnPerPage);
				Debug.Log(artHeros.Count-1);
				mCurrentPageAssets = artHeros.GetRange(index * mRowPerPage * mColumnPerPage, artHeros.Count - index * mRowPerPage * mColumnPerPage);
			}
			else
			{
				mCurrentPageAssets = artHeros.GetRange(index * mRowPerPage * mColumnPerPage, mRowPerPage * mColumnPerPage);
			}
			mCurrentPage = index;
			Debug.Log("mCurrentPageAssets:" + mCurrentPageAssets.Count);
			return true;
		}
		else
		{
			return false;
		}
	}

	void NextPage()
	{
		SetPage(mCurrentPage + 1);
	}

	void PreviousPage()
	{
		SetPage(mCurrentPage - 1);
	}

	class ExportAsset
	{
		public GameObject prefab;
		public GameObject initedPrefab;
		public string heroId;
		public Editor editor;
		public string path;
		public bool isSelected;
		public bool isCreated;
	}

}


