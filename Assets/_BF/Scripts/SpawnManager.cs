using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

//[ExecuteInEditMode]
public enum _SpawnMode{Battle,Arean}
public class SpawnManager : MonoBehaviour {
	public List<GameObject> heroPrefabs;
	public List<Hero> summonMonsters;
	public List<Hero> playerHeros;
	public List<Hero> enemyHeros;//just for arean;
	public List<Wave> waves;
	public List<ChestInfo> battleChests;
	public GameObject battleHeroPrefab;
	public GameObject sceneObject;
	public _SpawnMode spawnMode = _SpawnMode.Battle;
	public Vector2 defaultSceneScale = new Vector2(1.5f,1.6f);

	static public readonly int currentVersion = 1;
	static readonly public string baseAssetBundlePath = "http://192.168.4.4/";
	static readonly public string baseSceneAssetPath = "Scene/";
	static readonly public string baseHeroAssetPath = "Hero/";
	public string pathURL = "";
//	static Vector2 deflautSceneScale = new Vector2(1.5f,1.5f);
	
	BattleController controller;
	BattleSimpleController simpleController;

    public bool useLocalData;

	static SpawnManager instance;
	public static SpawnManager SingleTon(){
		return instance;
	}

	void Awake () {
		if (instance == null)
			instance = this;
		LoadResources ();
//		heroFbxs.Add("10014");//TODO this is for box monster

		controller = GetComponent<BattleController>();
		simpleController = GetComponent<BattleSimpleController>();

//		ConfigMgr.initPath(true,path);
//		DataManager.getModule<DataHero> (DATA_MODULE.Data_Hero).TextDate();
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
		if (controller != null) {
			controller.GlobalMask.color = Color.black;
			controller.GlobalMask.gameObject.SetActive (true);
		}
	}

	void Start()
	{
		if(spawnMode==_SpawnMode.Battle)
		{
			Spawn();
		}
		else if(spawnMode==_SpawnMode.Arean)
		{
			SpawnForArean();
		}
	}

	void LoadResources(){
		if (battleHeroPrefab == null)
			battleHeroPrefab = Resources.Load<GameObject> ("Hero");
	}

	void SpawnForArean()
	{
		List<string> fbxStrings = new List<string>();
		fbxStrings.Add("10001");
		fbxStrings.Add("10002");
		fbxStrings.Add("10003");
		fbxStrings.Add("10004");
		fbxStrings.Add("10005");
		fbxStrings.Add("10006");
		fbxStrings.Add("10007");
		fbxStrings.Add("10008");
		fbxStrings.Add("10009");
		fbxStrings.Add("10010");
		fbxStrings.Add("10011");
		AssetBundleMgr.SingleTon().CacheOrDownloadHeros(fbxStrings,Init,true);
	}

	public void Init(Dictionary<_AssetBundleType,Dictionary<string,GameObject>> allPrefabs)
	{
		playerHeros = SpawnUtility.InitAreanPlayerHeros();
		enemyHeros = SpawnUtility.InitAreanEnemyHeros();
		simpleController.enabled = true;
	}

	public void SpawnForSummonMonster()
	{

		List<string> fbxStrings = new List<string>();
		fbxStrings.Add("10013");	
		AssetBundleMgr.SingleTon().CacheOrDownloadHeros(fbxStrings,InitSummonMonster,true);
	}

	public void InitSummonMonster(Dictionary<_AssetBundleType,Dictionary<string,GameObject>> allPrefabs)
	{
		/*for (int i = playerHeros.Count - 1; i >= 0; i --)
		{
			if (playerHeros[i].gameObject.GetComponent<Hero>().isSummonMonster)
			{
				DestroyObject(playerHeros[i].gameObject);
			}
		}*/

		/*Transform players = GameObject.Find("Players").transform;
		for (int i = players.childCount -1; i >=0; i--)
		{
			if (players.GetChild(i).GetComponent<Hero>().isSummonMonster)
				DestroyObject(players.GetChild(i).gameObject);
				
		}*/

		List<Hero> summons = SpawnUtility.InitSummonMonster();
		if (summonMonsters == null)
			summonMonsters = new List<Hero>();
		else
			summonMonsters.Clear();

		summonMonsters.AddRange(summons);
		//playerHeros.AddRange(summons);
		//controller.RightHeroes.AddRange(summons);
		if(controller!=null)
		controller.setSummonMonster();
	}

	void Spawn()
	{
		StartCoroutine("_Load");
	}

	IEnumerator _Load()
	{
		BattleInfo battleInfo = SpawnUtility.getBattleInfo();
		Debug.Log("battleInfo.SCENE:" + battleInfo.SCENE);
        if (useLocalData)
        {
//            sceneObject = Instantiate(Resources.Load<GameObject>("World/" + battleInfo.SCENE)) as GameObject;
			sceneObject = Instantiate(Resources.Load<GameObject>("World/" + "GobiGrassland1")) as GameObject;
        }
        else
        {
            sceneObject = Instantiate(AssetBundleMgr.SingleTon().allCachePrefabs[_AssetBundleType.Scene][battleInfo.SCENE]) as GameObject;
        }
		//sceneObject.transform.localScale = defaultSceneScale;
        sceneObject.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
		yield return null;
		//PrepareHeroPrefabs();
        InitHeros(null);

	}

	void PrepareHeroPrefabs()
	{
		List<string> fbxStrings = new List<string>();
		Dictionary<int,HeroInfo> PlayerHeros = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetFightHeros();
		foreach(int index in PlayerHeros.Keys)
		{
			HeroInfo heroInfo = PlayerHeros[index];
			fbxStrings.Add(heroInfo.type.ToString());
		}
		BattleRecvData battleRecv = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).curBattleInfo;
		Dictionary<int, BattleStep> stepList = battleRecv._stepList;
		foreach(int index in stepList.Keys)
		{
			BattleStep step = stepList[index];


			/*Dictionary<int, BattleMonSter> monsterList = step._monsterList;
			foreach(int index0 in monsterList.Keys)
			{
				HeroInfo heroInfo = monsterList[index0].heroInfo;
				fbxStrings.Add(heroInfo.type.ToString());
			}*/

			List<BattleMonSter> monsterList = step._monsterList;
			for (int i = 0; i < monsterList.Count; i ++)
			{
				HeroInfo heroInfo = monsterList[i].heroInfo;
				fbxStrings.Add(heroInfo.type.ToString());

			}		

		}
#region insert chest heros
		fbxStrings.Add("10062");
		fbxStrings.Add("10063");
		fbxStrings.Add("10064");
#endregion
		AssetBundleMgr.SingleTon().CacheOrDownloadHeros(fbxStrings,InitHeros,true);
	}

	public void InitHeros(Dictionary<_AssetBundleType,Dictionary<string,GameObject>> prefabs)
	{
		Debug.Log("InitHeros");
		playerHeros = SpawnUtility.InitPlayerHeros (useLocalData);
        if (useLocalData)
        {
            waves = SpawnUtility.InitializeWaves1();
        }
        else
        {
            waves = SpawnUtility.InitializeWaves ();
            battleChests = SpawnUtility.GetDropChest();
        }
		if(controller!=null)
		controller.enabled = true;

	}

	string path = "D:/Config";
	public int comboCount = 0;
	public float delay;
	public bool isDebug = true;
	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,200,100),"ReStart")){
			UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}
		if(isDebug)
		{
			GUI.color = Color.green;
			GUI.Label(new Rect(10,145,100,30),"Combo:");
			GUI.Label(new Rect(110,145,50,30),comboCount.ToString());
			GUI.Label(new Rect(10,175,100,30),"DelayBeforeAttack:");
			GUI.Label(new Rect(110,175,100,30),delay.ToString());
			SpawnUtility.delayPerPart = float.Parse(GUI.TextField(new Rect(10,215,100,30),SpawnUtility.delayPerPart.ToString()));
			BattleUtility.commonColumnDelay = float.Parse(GUI.TextField(new Rect(10,255,100,30),BattleUtility.commonColumnDelay.ToString()));
			BattleUtility.commonRowDelay = float.Parse(GUI.TextField(new Rect(10,295,100,30),BattleUtility.commonRowDelay.ToString()));
			HeroAttribute.commonHitInterval = float.Parse(GUI.TextField(new Rect(10,335,100,30),HeroAttribute.commonHitInterval.ToString()));
			if(GUI.Button(new Rect(10,110,100,30),"Load!"))
			{
				ConfigMgr.initPath(true,path);
				SpawnUtility.InitAttackQueues();
				Application.LoadLevel("Battle");
			}
		}
	}

	public bool isPressed;
	public Hero curHero;

	void Update()
	{
		if(isDebug)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Collider2D col = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				if(col!=null)
				{
					HeroRes hr = col.GetComponent<HeroRes>();
					if(hr!=null && hr.hero.Side == _Side.Player)
					{
						if(curHero!=null)
						{
							Vector3 pos0 = curHero.heroAttack.defaultPos;
							int index = curHero.Index;
							curHero.heroAttack.defaultPos = hr.hero.heroAttack.defaultPos;
							curHero.Index = hr.hero.Index;
							curHero.transform.position = hr.hero.heroAttack.defaultPos;

							hr.hero.heroAttack.defaultPos = pos0;
							hr.hero.Index = index;
							hr.hero.transform.position = pos0;
							curHero = null;
						}
						else
						{
							curHero = hr.hero;
						}
					}
					else
					{
						curHero = null;
					}
				}
			}
		}
	}
}

[System.Serializable]
public class ChestInfo
{
	public CHEST_TYPE type;
	public int id;
	public int value;
	public ChectHero dropMonster;
}

[System.Serializable]
public class ChectHero
{
	public HERO_GROWUP grown;
	public int level;
	public int typeid;
	public bool isCatch;
}


