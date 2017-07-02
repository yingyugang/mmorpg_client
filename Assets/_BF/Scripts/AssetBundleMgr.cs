using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum _AssetBundleType{Hero,Scene}
public class AssetBundleMgr : MonoBehaviour{

	public Dictionary<_AssetBundleType,string> assetBundlePath = new Dictionary<_AssetBundleType, string>();
	public Dictionary<_AssetBundleType,Dictionary<string,GameObject>> allCachePrefabs = new Dictionary<_AssetBundleType, Dictionary<string, GameObject>>();
	public Dictionary<_AssetBundleType,Dictionary<string,GameObject>> allCacheGameObjects = new Dictionary<_AssetBundleType, Dictionary<string, GameObject>>();
	public HashSet<Object> allSprites = new HashSet<Object>();
	public const int currentVersion = 0;//just let hardcode
	public const string bfVersionSubName = "BF_Version_Sub";
	public const int initVersionSub = 0;
	public int currentVersionSub = 0;//it used to download assetbundle
	public const string baseAssetBundlePath = "http://192.168.1.152:8111/";
	public string pathURL = "";
	static Vector2 deflautSceneScale = new Vector2(1.5f,1.5f);
	[System.NonSerialized]
	public bool isHeroPrefabLoading = false;
	
	float mDownloadProgress = 0;
	float mDownloadProgressGlobal = 0;
	string mDownloadLabel = "";
	string mDownloadLabelDetail = "";
	public bool loadStreamingAssets;
	static public string defaultPrefab = "10239";
	public delegate void OnLoadDone(Dictionary<_AssetBundleType,Dictionary<string,GameObject>> prefabs);
    public bool useLocalModel;

	static AssetBundleMgr instance;
	public static AssetBundleMgr SingleTon()
	{
		return instance;
	}

	Texture2D mTex;
	Texture2D mTexFront;
	void Awake()
	{
		if(instance==null)
			instance = this;
		pathURL =
#if UNITY_ANDROID
			"jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
		"file://" + Application.dataPath + "/StreamingAssets/";
#else
		string.Empty;
#endif 
		mTex = CommonUtility.InitTexture2D(10,10,Color.black);
		mTexFront = CommonUtility.InitTexture2D(10,10,Color.green);
		assetBundlePath.Add(_AssetBundleType.Hero,"Hero/");
		assetBundlePath.Add(_AssetBundleType.Scene,"Scene/");
		currentVersionSub = Mathf.Max(initVersionSub,PlayerPrefs.GetInt(bfVersionSubName)); 
	}

	void Start()
	{
        if(!useLocalModel)DownloadVersion(SaveVersion);
		Debug.Log("Application.temporaryCachePath:" + Application.temporaryCachePath);
	}

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width-50,20,100,30),"V" + currentVersion + "." + currentVersionSub);
		if(isHeroPrefabLoading)
		{
			GUI.backgroundColor = Color.black;
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),mTex);
			GUI.color = Color.white;
			float width = Screen.width * 0.8f * mDownloadProgress;
			float xOffset = Screen.width * 0.2f / 2;
			width = 100;
			GUI.Label(new Rect(xOffset,Screen.height/2 - 30,width,30),mDownloadLabel);
			GUI.Label(new Rect(xOffset + width / 2,Screen.height/2 - 30,width,30),mDownloadLabelDetail);
			GUI.color = Color.green;
			width = Screen.width * 0.8f * mDownloadProgress;
			xOffset = Screen.width * 0.2f / 2;
			GUI.DrawTexture(new Rect(xOffset,Screen.height/2 - 8,width,5),mTexFront);
			width = Screen.width * 0.8f * mDownloadProgressGlobal;
			xOffset = Screen.width * 0.2f / 2;
			GUI.DrawTexture(new Rect(xOffset,Screen.height/2 ,width,10),mTexFront);
		}
	}

    void Update()
    {
//        if(Input.GetKeyDown(KeyCode.Q))
//        {
//            Debug.Log("Cache has been cleared!");
//            PlayerPrefs.SetInt(bfVersionSubName,initVersionSub);
//            Caching.CleanCache();
//        }
    }

	public void CacheOrDownloadHeros(List<string> heroStrs,OnLoadDone loadDoneFunc = null,bool showProgress = false)
	{
		Reset();
#region set default hero
		if (!heroStrs.Contains(defaultPrefab))
		{
			heroStrs.Add(defaultPrefab);
		}

		if (!heroStrs.Contains("85001"))
		{
			heroStrs.Add("85001");
		}
#endregion
		isHeroPrefabLoading = showProgress;
		Dictionary<_AssetBundleType,HashSet<string>> prefabStrings = new Dictionary<_AssetBundleType, HashSet<string>>();
		HashSet<string> set = new HashSet<string>(heroStrs);
		set.Add("10062");
		set.Add("10063");
		set.Add("10064");
		prefabStrings.Add(_AssetBundleType.Hero,set);
        if(!useLocalModel)
        {
            StartCoroutine(_Download(prefabStrings,loadDoneFunc));
        }
        else
        {
            LoadLocalModels(prefabStrings,loadDoneFunc);
        }
	}

	public void CacheOrDownloadScenes(List<string> sceneStrs,OnLoadDone loadDoneFunc,bool showProgress = false)
	{
		Reset();
		isHeroPrefabLoading = showProgress;
		Dictionary<_AssetBundleType,HashSet<string>> prefabStrings = new Dictionary<_AssetBundleType, HashSet<string>>();
		HashSet<string> set = new HashSet<string>(sceneStrs);
		prefabStrings.Add(_AssetBundleType.Scene,set);
        if(!useLocalModel)
        {
            StartCoroutine(_Download(prefabStrings,loadDoneFunc));
        }
        else
        {
            LoadLocalModels(prefabStrings,loadDoneFunc);
        }
	}

	public void DownloadVersion(OnLoadDone loadDoneFunc)
	{
		Reset();
		if(loadStreamingAssets)
		{

		}
		else
		{
            StartCoroutine(_DownloadVersion(loadDoneFunc));
		}
	}

	IEnumerator _DownloadVersion(OnLoadDone loadDoneFunc)
	{
        if(WaitController.me!=null)WaitController.me.hide();
		isHeroPrefabLoading = true;
		Dictionary<_AssetBundleType,HashSet<string>> allAssets = new Dictionary<_AssetBundleType, HashSet<string>>();
		bool versionDone = false;
		if(currentVersionSub==0)
		{
			Caching.CleanCache();
		}
		while(!versionDone)
		{
			Dictionary<_AssetBundleType,HashSet<string>> prefabStrings = new Dictionary<_AssetBundleType, HashSet<string>>();
			string assetName = "V_" + currentVersion + "_" + (currentVersionSub + 1);
//			Debug.Log(baseAssetBundlePath + assetName + GetBundleTypeByDevice() + ".unity3d");
			string assetVersionName = baseAssetBundlePath + assetName + CommonUtility.GetBundleTypeByDevice() + ".unity3d";
			Debug.Log("assetVersionName:" + assetVersionName);
			WWW bundle = WWW.LoadFromCacheOrDownload(assetVersionName,currentVersion);
			yield return bundle;
			if(bundle.error == null && bundle.assetBundle!=null)
			{
				VersionInfo versionInfo = ((GameObject)bundle.assetBundle.mainAsset).GetComponent<VersionInfo>();
				string[] heroListStrs = versionInfo.heros.ToArray();
				foreach(string str in heroListStrs)
				{
					Debug.Log("Version heroId:" + str);
				}
				prefabStrings.Add(_AssetBundleType.Hero,new HashSet<string>(heroListStrs));
				string[] sceneListStrs =  versionInfo.scenes.ToArray();
				prefabStrings[_AssetBundleType.Scene] = new HashSet<string>(sceneListStrs);
				currentVersionSub ++;
				bundle.assetBundle.Unload(false);
				
				foreach(_AssetBundleType bundleType in prefabStrings.Keys)
				{
					if(!allAssets.ContainsKey(bundleType))
					{
						allAssets.Add (bundleType,new HashSet<string>());
					}
					HashSet<string> prefabStrs = prefabStrings[bundleType];
					foreach(string str in prefabStrs)
					{
						allAssets[bundleType].Add(str);
					}
				}
			}else{
				versionDone = true;
			}
		}
		Debug.Log("allAssets:" + allAssets.Count);
		isHeroPrefabLoading = false;
        if (allAssets.Count > 0)
		{
			StartCoroutine(_Download(allAssets,loadDoneFunc));
		}
	}
	
    void LoadLocalModels(Dictionary<_AssetBundleType,HashSet<string>> prefabStrings,OnLoadDone loadDoneFunc)
    {
        Debug.Log("LoadLocalModels");
        foreach(_AssetBundleType abType in prefabStrings.Keys)
        {
            if(!allCachePrefabs.ContainsKey(abType))
            {
                allCachePrefabs.Add(abType,new Dictionary<string, GameObject>());
            }
            foreach(string fbx in prefabStrings[abType])
            {
                string tempFbx = fbx;
                if(!allCachePrefabs[abType].ContainsKey(tempFbx))
                {
                    GameObject prefab = Resources.Load<GameObject>("Hero/" + fbx);
                    if(prefab==null)
                    {
                        Debug.LogError("Hero " + fbx + " is not existing!");
                        prefab = Resources.Load<GameObject>("Hero/" + defaultPrefab);
                    }
                    if(!allCachePrefabs[abType].ContainsKey(fbx))
                        allCachePrefabs[abType].Add(fbx,prefab);
                }
            }
        }
        isHeroPrefabLoading = false;
        if(loadDoneFunc!=null)
            loadDoneFunc(allCachePrefabs);
    }

	/// <summary>
	/// _s the download.
	/// </summary>
	/// <returns>The download.</returns>
	/// <param name="prefabStrings">all the prefabs need to download.</param>
	IEnumerator _Download(Dictionary<_AssetBundleType,HashSet<string>> prefabStrings,OnLoadDone loadDoneFunc)
	{
		Debug.Log("_Download");
		isHeroPrefabLoading = true;
		WWW bundle;

		int currentFbxIndex = 0;
		int totalFbxCount = 0;
		foreach(_AssetBundleType abType in prefabStrings.Keys)
		{
			totalFbxCount += prefabStrings[abType].Count;
		}
		
		foreach(_AssetBundleType abType in prefabStrings.Keys)
		{
			if(!allCachePrefabs.ContainsKey(abType))
			{
				allCachePrefabs.Add(abType,new Dictionary<string, GameObject>());
			}
			foreach(string fbx in prefabStrings[abType])
			{
				mDownloadProgress = 0;
				mDownloadLabelDetail = fbx;
				string tempFbx = fbx;
				if(!allCachePrefabs[abType].ContainsKey(tempFbx))
				{
					if(tempFbx!=null)
					{
						bundle = WWW.LoadFromCacheOrDownload(baseAssetBundlePath + assetBundlePath[abType] + tempFbx + CommonUtility.GetBundleTypeByDevice() + ".unity3d",currentVersion);
						while(!bundle.isDone)
						{
							mDownloadProgress = bundle.progress;
							yield return null;
						}
						yield return bundle;

						if((bundle.error!=null || bundle == null) && abType == _AssetBundleType.Hero)
						{
							tempFbx = defaultPrefab;
							bundle = WWW.LoadFromCacheOrDownload(baseAssetBundlePath + assetBundlePath[abType] + tempFbx + CommonUtility.GetBundleTypeByDevice() + ".unity3d",currentVersion);
							Debug.Log("tempFbx:" + tempFbx + ";fbx:" + fbx);
							while(!bundle.isDone)
							{
								mDownloadProgress = bundle.progress;
								yield return null;
							}
							yield return bundle;
						}

						if(bundle.error==null && bundle!=null)
						{
							GameObject go = (GameObject)bundle.assetBundle.LoadAsset(tempFbx,typeof(GameObject));
							if(!allCachePrefabs[abType].ContainsKey(fbx))allCachePrefabs[abType].Add(fbx,go);
							Object[] tmpObjs = bundle.assetBundle.LoadAllAssets(typeof(Sprite));
							foreach(Object obj in tmpObjs)
							{
								allSprites.Add(obj);
							}
							bundle.assetBundle.Unload(false);
						}
					}
				}
				currentFbxIndex ++;
				mDownloadProgress = 1;
				mDownloadLabel = currentFbxIndex + "/" + totalFbxCount;
				mDownloadProgressGlobal = (float)currentFbxIndex / totalFbxCount;
				yield return null;
			}
		}
		yield return null;
		isHeroPrefabLoading = false;
		if(loadDoneFunc!=null)
			loadDoneFunc(allCachePrefabs);
	}

	void SaveVersion(Dictionary<_AssetBundleType,Dictionary<string,GameObject>> allCachePrefabs)
	{
		PlayerPrefs.SetInt(bfVersionSubName,currentVersionSub);
		PlayerPrefs.Save();
	}

	void Reset()
	{
		mDownloadProgress = 0;
		mDownloadProgressGlobal = 0;
		mDownloadLabel = "";
		mDownloadLabelDetail = "";
	}

}
