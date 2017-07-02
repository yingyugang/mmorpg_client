 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

static public class SpawnUtility  {

	public static Transform playerTrans;

	public static List<ChestInfo> GetDropChest()
	{
		BattleRecvData battleRecv = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).curBattleInfo;
		BattleDropChest[] dropChests = battleRecv.getDropChestList();
		List<ChestInfo> chestInfos = new List<ChestInfo>();
		foreach(BattleDropChest dropChest in dropChests)
		{
			ChestInfo chestInfo = new ChestInfo();
			chestInfo.id = (int)dropChest.id;
			chestInfo.type = dropChest.type;
			chestInfo.value = (int)dropChest.value;
			if(chestInfo.dropMonster!=null)
			{
				ChectHero chestHero = new ChectHero();
				chestHero.grown = dropChest.dropMonster.grown;
				chestHero.isCatch = dropChest.dropMonster.isCatch;
				chestHero.level = (int)dropChest.dropMonster.level;
				chestHero.typeid = (int)dropChest.dropMonster.typeid;
				chestInfo.dropMonster = chestHero;
			}
			chestInfos.Add(chestInfo);
		}
		return chestInfos;
	}

    static public List<Wave> InitializeWaves1()
    {
        List<Wave> waves = new List<Wave>();
        Transform trans = null;
        GameObject go = new GameObject();
        go.name = "_Waves";
        trans = go.transform;
        
        for(int i=0;i< 4;i++)
        {
			Dictionary<int,HeroInfo> heroInfos = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetFightHeros2();
			HeroInfo[] heroInfos0 = new HeroInfo[heroInfos.Count];
            int index = 0;
            foreach(int location in heroInfos.Keys)
            {
                heroInfos[index].location = index + 1;
                heroInfos0[index] = heroInfos[index];
                index ++;
            }
            Wave wave = InitWave(i,trans,heroInfos0);
            waves.Add(wave);
        }
        return waves;
    }

	static public List<Wave> InitializeWaves()
	{
		List<Hero> allEnemys = new List<Hero>();
		List<Wave> waves = new List<Wave>();
		BattleRecvData battleRecv = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).curBattleInfo;
		Dictionary<int, BattleStep> stepList = battleRecv._stepList;
		Transform trans = null;
		GameObject go = new GameObject();
		go.name = "_Waves";
		trans = go.transform;
		int stepIndex = 0;
		foreach(int index in stepList.Keys)
		{
			BattleStep step = stepList[index];
			List<BattleMonSter> monsterList = step._monsterList;
			HeroInfo[] heroInfos = new HeroInfo[monsterList.Count];
			int i = 0;
			for (int j = 0; j < monsterList.Count; j ++)
			{
				HeroInfo heroInfo = monsterList[j].heroInfo;
				//heroInfo.location = monsterList[j].MONSTER_TYPEID;

				heroInfo.type = monsterList[j].heroInfo.type;
				heroInfo.location = monsterList[j].MONSTER_LOCATION;
				heroInfos[i] = heroInfo;
				i ++;
			}
			Wave wave = InitWave(index,trans,heroInfos);
			waves.Add(wave);
			allEnemys.AddRange(wave.heros);

			stepIndex ++;
		}
		Debug.Log("stepList:" + stepList.Count);
		BattleDropChest[] dropChests = battleRecv.getDropChestList();
		Debug.Log("dropChests:" + dropChests.Length);
		BattleDropItem[] dropItems = battleRecv.getDropitemList();
		Debug.Log("dropItems:" + dropItems.Length);
		List<Hero> tmpHero = new List<Hero>(allEnemys);
#region remove last wave of heros
		Wave lastWave = waves[waves.Count - 1];
		foreach(Hero hero in lastWave.heros)
		{
			tmpHero.Remove(hero);
		}
#endregion
		for(int i=0;i < dropChests.Length;i++)
		{
			Hero hero = tmpHero[Random.Range(0,tmpHero.Count)];
			hero.dropChest = dropChests[i];
			tmpHero.Remove(hero);
			if(tmpHero.Count==0)
			{
				Debug.LogError("The dropChests count is more than monster count");
				break;
			}
		}
		return waves;
	}


	/// <summary>
	/// Inits the battle waves. Single mode.
	/// </summary>
	/// <returns>The waves.</returns>
	[System.Obsolete("InitWaves is Obsolete,use InitializeWaves to instead!")]
	static public List<Wave> InitWaves()
	{
		List<Wave> waves = new List<Wave>();
		Transform trans = null;
//#if UNITY_EDITOR
		GameObject go = new GameObject();
		go.name = "_Waves";
		trans = go.transform;
		if(SpawnManager.SingleTon().isDebug)
			DataManager.getModule<DataTask> (DATA_MODULE.Data_Task).getEnemyInfoList (50001);//TODO
//#endif
		List<FieldEnemy> fieldEnemy = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getEnemyInfoListByFieldID();
		Debug.Log (fieldEnemy.Count);
		for (int i = 0; i < fieldEnemy.Count; i++) {
			HeroInfo[] tmpHeros = new HeroInfo[6];
			for(int j=0;j < fieldEnemy[i].EnemyInfo.Count; j++){

				HeroInfo heroInfo = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).GetMonsterInfo(fieldEnemy[i].EnemyInfo[j].MONSTER_TYPEID);

				tmpHeros[fieldEnemy[i].EnemyInfo[j].MONSTER_LOCATION -1] = heroInfo;
			}
			waves.Add(InitWave(i,trans,tmpHeros));
		}
		return waves;
	}

	static Wave InitWave(int index,Transform parentNode,HeroInfo[] heroInfos)
	{
		Wave wave = new Wave();
		wave.heros = new List<Hero>();
		Transform trans = null;
//#if UNITY_EDITOR
		GameObject go = new GameObject();
		go.name = "Wave" + index;
		trans = go.transform;
		if(parentNode!=null)trans.parent = parentNode;
//#endif
		for(int i=0; i < heroInfos.Length; i++)
		{
			if(heroInfos[i]!=null)
			{
				Hero hero = InitEnemyHero(SpawnManager.SingleTon().battleHeroPrefab,heroInfos[i],i,trans);
				if (hero != null)
					wave.heros.Add(hero);
			}
		}
		return wave;
	}

	static Hero InitEnemyHero(GameObject heroPrefab,HeroInfo heroInfo,int location,Transform parentNode)
	{
		Hero hero = InitHero(heroPrefab,heroInfo,location,parentNode);
		if (hero.heroRes == null)
			return null;
//		#if UNITY_EDITOR
		hero.gameObject.name = "Enemy" + location;
		hero.transform.parent = parentNode;
//		#endif
		hero.OrderLayerName = hero.heroAnimation.SetOrderLayer(location);
		hero.transform.rotation = new Quaternion (0, 180, 0, 1); 
		hero.Side = _Side.Enemy;
		hero.heroAttack.maxTurn = 2;//TODO
		hero.IsBoss = heroInfo.isBoss == 1 ? true : false;
		//服务端（配置表）站位从1开始，客户端站位从0开始
		hero.Index = heroInfo.location - 1;
		hero.gameObject.SetActive(false);

		//test
		//hero.isCaptian = true;
		//test
		return hero;
	}

	public static List<Hero> InitPlayerHeros(bool useLocalData = false)
	{
		List<Hero> heros = new List<Hero>();
        Dictionary<int,HeroInfo> PlayerHeros = null;
        int captianPos = 0;
        if (useLocalData)
        {
            PlayerHeros = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetFightHeros1();
        } 
        else
        {
            PlayerHeros = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetFightHeros();
            captianPos = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetCurLeaderPos() - 1;
        }
		Debug.Log("PlayerHeros.Count:" + PlayerHeros.Count);
		int i = 0;
		Transform trans = null;
//		#if UNITY_EDITOR
		GameObject go = new GameObject();
		go.name = "Players";
		trans = go.transform;
		playerTrans = trans;
//		#endif

		//服务端发来的位置信息从1开始，本地位置从0开始
		
		foreach(int key in PlayerHeros.Keys)
		{
			if(PlayerHeros[key]!=null)
			{
				Hero hero = InitPlayerHero(SpawnManager.SingleTon().battleHeroPrefab,PlayerHeros[key],i,trans);
				//hero.isCaptian = true;

				if (key == captianPos)			
					hero.isCaptian = true;
				else
					hero.isCaptian = false;
				
				heros.Add(hero);
				i++;
			}
		}
		return heros;
	}

	static Hero InitPlayerHero(GameObject heroPrefab,HeroInfo heroInfo,int location,Transform parentNode)
	{
		Hero hero = InitHero(heroPrefab,heroInfo,location,parentNode);
//		#if UNITY_EDITOR
		hero.gameObject.name = "Player" + location;
		hero.transform.parent = parentNode;
//		#endif
		hero.OrderLayerName = hero.heroAnimation.SetOrderLayer(location);
		hero.Side = _Side.Player;
		hero.heroAttack.maxTurn = 1;//TODO
		return hero;
	}

	static public List<Hero> InitAreanPlayerHeros()
	{
		List<Hero> areanPlayers = new List<Hero>();
		Hero hero = InitTmpHero(_Side.Player,10007,SpawnManager.SingleTon().battleHeroPrefab,0);
		areanPlayers.Add(hero);
		hero = InitTmpHero(_Side.Player,10008,SpawnManager.SingleTon().battleHeroPrefab,1);
		areanPlayers.Add(hero);
		hero = InitTmpHero(_Side.Player,10009,SpawnManager.SingleTon().battleHeroPrefab,2);
		areanPlayers.Add(hero);
		hero = InitTmpHero(_Side.Player,10010,SpawnManager.SingleTon().battleHeroPrefab,3);
		areanPlayers.Add(hero);
		hero = InitTmpHero(_Side.Player,10011,SpawnManager.SingleTon().battleHeroPrefab,4);
		areanPlayers.Add(hero);
		return areanPlayers;
	}

	//TODO
	static public List<Hero> InitAreanEnemyHeros()
	{
		List<Hero> areanEnemys = new List<Hero>();
		Hero hero = InitTmpHero(_Side.Enemy,10007,SpawnManager.SingleTon().battleHeroPrefab,0);
		areanEnemys.Add(hero);
		hero = InitTmpHero(_Side.Enemy,10008,SpawnManager.SingleTon().battleHeroPrefab,1);
		areanEnemys.Add(hero);
		hero = InitTmpHero(_Side.Enemy,10009,SpawnManager.SingleTon().battleHeroPrefab,2);
		areanEnemys.Add(hero);
		hero = InitTmpHero(_Side.Enemy,10010,SpawnManager.SingleTon().battleHeroPrefab,3);
		areanEnemys.Add(hero);
		hero = InitTmpHero(_Side.Enemy,10011,SpawnManager.SingleTon().battleHeroPrefab,4);
		areanEnemys.Add(hero);
		return areanEnemys;
	}

	static public List<Hero> InitSummonMonster()
	{	


		SUMMON_INFO item = new SUMMON_INFO();
		item.idSummonType = 85001;
		SummonInfo summonInfo = new SummonInfo();
		summonInfo.init(item);
		
		List<Hero> summonPlayer = new List<Hero>();
		Hero hero = InitSummonHero(summonInfo,_Side.Player,10013,SpawnManager.SingleTon().battleHeroPrefab,4);
		hero.transform.parent = playerTrans;
		hero.isSummonMonster = true;
		hero.IsBoss = false;
		hero.gameObject.name = "Summon1";
		summonPlayer.Add(hero);	

		item.idSummonType = 85002;
		summonInfo = new SummonInfo();
		summonInfo.init(item);

		hero = InitSummonHero(summonInfo,_Side.Player,10013,SpawnManager.SingleTon().battleHeroPrefab,4);
		hero.transform.parent = playerTrans;
		hero.isSummonMonster = true;
		hero.IsBoss = false;
		hero.gameObject.name = "Summon2";
		summonPlayer.Add(hero);

		item.idSummonType = 85003;
		summonInfo = new SummonInfo();
		summonInfo.init(item);

		hero = InitSummonHero(summonInfo,_Side.Player,10013,SpawnManager.SingleTon().battleHeroPrefab,4);
		hero.transform.parent = playerTrans;
		hero.isSummonMonster = true;
		hero.IsBoss = false;
		hero.gameObject.name = "Summon3";
		summonPlayer.Add(hero);


		/*item.idSummonType = 85004;
		summonInfo = new SummonInfo();
		summonInfo.init(item);
		hero = InitSummonHero(summonInfo,_Side.Player,10013,SpawnManager.SingleTon().battleHeroPrefab,1);
		hero.transform.parent = playerTrans;
		hero.isSummonMonster = true;
		hero.IsBoss = true;
		summonPlayer.Add(hero);

		item.idSummonType = 85011;
		summonInfo = new SummonInfo();
		summonInfo.init(item);
		hero = InitSummonHero(summonInfo,_Side.Player,10013,SpawnManager.SingleTon().battleHeroPrefab,1);
		hero.transform.parent = playerTrans;
		hero.isSummonMonster = true;
		hero.IsBoss = true;
		summonPlayer.Add(hero);		*/

		return summonPlayer;
	}

	static public Hero InitSummonHero(SummonInfo summonInfo,_Side side,int heroTypeId,GameObject heroPrefab,int location)
	{			

		HeroInfo heroInfo = new HeroInfo();
		heroInfo.type = (int)summonInfo.type;
		heroInfo.initHP = summonInfo.initHp;
		heroInfo.name = summonInfo.name;
		heroInfo.moveSpeed = 6;
		heroInfo.series = summonInfo.series;
		heroInfo.initAtk = summonInfo.initAtk;
		heroInfo.initDef = summonInfo.initDef;
		heroInfo.initRecover = summonInfo.initRecover;
		heroInfo.initViolence = summonInfo.initViolence;
		heroInfo.fbxFile = summonInfo.fbxFile;
		heroInfo.spriteName = summonInfo.iconSprite;
		heroInfo.movable = 2;
		Hero hero = InitHero(heroPrefab,heroInfo,location,null);
		if(BattleController.SingleTon()!=null)
		{
			hero.transform.localPosition = new Vector3(25,-6,BattleController.SingleTon().RightPoints[location].position.z);
			hero.heroAttack.defaultPos = BattleController.SingleTon().RightPoints[location].position;
		}
		
		if (hero.heroRes == null)
			return null;
		
		//hero.gameObject.SetActive(true);
		hero.OrderLayerName = hero.heroAnimation.SetOrderLayer(location);
		hero.gameObject.SetActive(true);
		hero.HeroBody.gameObject.SetActive(true);
		//hero.Side = _Side.Player;
		hero.heroAttack.maxTurn = 1;//TODO 每次最大攻击次数
		List<AttackAttribute> attackAttributes = hero.heroAttack.GetAttackAttributes();
		if (attackAttributes.Count > 0)
			attackAttributes[0].impactNum = 6; //召唤兽的普通攻击改成群攻

		hero.Side = side;
		return hero;




		/*HeroInfo heroInfo = new HeroInfo();
		heroInfo.type = heroTypeId;
		heroInfo.initHP = 100000;
		heroInfo.name = "召唤兽";
		heroInfo.moveSpeed = 6;
		heroInfo.series = (int)_ElementType.Evil;
		heroInfo.initAtk = 100;
		heroInfo.initDef = 10;
		heroInfo.initRecover = 1000;
		heroInfo.initViolence = 2500;
		heroInfo.fbxFile = heroTypeId.ToString();
		Hero hero = InitHero(heroPrefab,heroInfo,location,null);
		if(BattleController.SingleTon()!=null)
		{
			hero.transform.position = BattleController.SingleTon().RightPoints[location].position;
			hero.heroAttack.defaultPos = BattleController.SingleTon().RightPoints[location].position;
		}
		
		if (hero.heroRes == null)
			return null;

		//hero.gameObject.SetActive(true);
		hero.OrderLayerName = hero.heroAnimation.SetOrderLayer(location);
		hero.HeroBody.gameObject.SetActive(true);
		hero.Side = _Side.Player;
		hero.heroAttack.maxTurn = 2;//TODO
		hero.Side = side;
		return hero;*/
	}



	static public Hero InitTmpHero(_Side side,int heroTypeId,GameObject heroPrefab,int location)
	{
		HeroInfo heroInfo = new HeroInfo();
		heroInfo.type = heroTypeId;
		heroInfo.initHP = 1000;
		heroInfo.name = "测试怪";
		heroInfo.moveSpeed = 6;
		heroInfo.series = (int)_ElementType.Evil;
		heroInfo.coinPerDrop = 10;
		heroInfo.soulPerDrop = 10;
		heroInfo.initAtk = 10;
		heroInfo.initDef = 10;
		heroInfo.initRecover = 1000;
		heroInfo.initViolence = 2500;
		heroInfo.fbxFile = heroTypeId.ToString();
		Hero hero = InitHero(heroPrefab,heroInfo,location,null);
		if(BattleController.SingleTon()!=null)
		{
			hero.transform.position = BattleController.SingleTon().LeftPoints[location].position;
			hero.heroAttack.defaultPos = BattleController.SingleTon().LeftPoints[location].position;
		}

		if (hero.heroRes == null)
			return null;

		hero.OrderLayerName = hero.heroAnimation.SetOrderLayer(location);

		if(side==_Side.Enemy)hero.transform.rotation = new Quaternion (0, 180, 0, 1); 
		hero.Side = _Side.Enemy;
		hero.heroAttribute.dropChest = 9999;
		hero.heroAttack.maxTurn = 2;//TODO
		hero.Side = side;
		return hero;
	}

	/// <summary>
	/// Init Hero
	/// </summary>
	static Hero InitHero(GameObject heroPrefab,HeroInfo heroInfo,int location,Transform parentNode)
	{
		if (heroInfo == null)
			return null;
		if (heroPrefab.activeInHierarchy)
			heroPrefab.SetActive (false);

		GameObject go = Object.Instantiate (heroPrefab) as GameObject;
		Hero hero = go.GetComponent<Hero>();
		if (parentNode != null)
			go.transform.parent = parentNode;
		
		HeroAttack heroAttack = go.GetComponent<HeroAttack>();
		if(heroAttack==null)heroAttack = go.AddComponent<HeroAttack>();
		HeroAttribute heroAttribute = go.GetComponent<HeroAttribute>();
		if(heroAttribute==null)heroAttribute = go.AddComponent<HeroAttribute>();
		heroAttribute.heroInfo = heroInfo;

		SkillManager skillMgr = go.GetComponent<SkillManager>();
		if (skillMgr == null) 
		{
			skillMgr = go.AddComponent<SkillManager>();
		}
		List<AttackAttribute> attacks = new List<AttackAttribute> ();
		
		//Normal attack;
		AttackAttribute attackAttr = new AttackAttribute();
		attackAttr.attackType = AttackType.Damage;//TODO
		attackAttr.impactNum = 1;//TODO
		attackAttr.heroInfo = heroInfo;
		attackAttr.attackQueue = GetAttackQueue (heroInfo.type);//TODO need to set queue id 
		attackAttr.attackFullQueue = GetAttackFullQueue(heroInfo.type);
		attackAttr.moveable = heroInfo.movable == 2 ? false : true;//TODO heroInfo.movable may base on skill and normal 
		attackAttr.isShake = false;//need to set
		attackAttr.shakeDelay = 0.5f;//need to set
		attackAttr.hitDelay = 0;//need to set
		attackAttr.hitDelay1 = 0;//need to set
		attackAttr.moveBackDelay = 0;//need to set
		attackAttr.moveBackDelay1 = 0;//need to set
		attackAttr.moveForwardDelay = 0;//need to set
		attackAttr.moveForwardDelay1 = 0;//need to set
		attackAttr.hero = hero;
		attacks.Add (attackAttr);

		//special attack;
		attackAttr = new AttackAttribute();
		attackAttr.attackType = AttackType.Damage;//TODO
		//if(heroInfo.id == 10012)attackAttr.attackType = AttackType.Health;//TODO
		attackAttr.impactNum = 6;//TODO
		attackAttr.moveable = heroInfo.movable == 2 ? false : true;//TODO heroInfo.movable may base on skill and normal 
		attackAttr.isShake = false;//need to set
		attackAttr.attackClip = Hero.ACTION_SKILL1;
		attackAttr.shakeDelay = 0.5f;//need to set
		attackAttr.hitDelay = 0;//need to set
		attackAttr.hitDelay1 = 0;//need to set
		attackAttr.moveBackDelay = 0;//need to set
		attackAttr.moveBackDelay1 = 0;//need to set
		attackAttr.moveForwardDelay = 0;//need to set
		attackAttr.moveForwardDelay1 = 0;//need to set
		attackAttr.hero = hero;
		attackAttr.targetType = 0;
		if(attackAttr.attackType != AttackType.Damage)
		{
//			List<AttackQueue> aqs = new List<AttackQueue>();
//			aqs.Add(AttackQueue.Instance(GetBattleAudioClip (defaultAudioClip0),1,0,1));
			attackAttr.attackQueue = GetAttackQueue (heroInfo.type);//TODO need to set queue id 
			attackAttr.attackFullQueue = GetAttackFullQueue(heroInfo.type);
			attackAttr.moveable = false;
		}
		else
		{
			attackAttr.attackQueue = GetAttackQueue (heroInfo.type);//TODO need to set queue id 
			attackAttr.attackFullQueue = GetAttackFullQueue(heroInfo.type);
		}
		attacks.Add (attackAttr);
		heroAttack.SetAttackAttributes (attacks);
		heroAttribute.heroTypeId = heroInfo.type;
		heroAttribute.maxHP = heroInfo.initHP;
		heroAttribute.currentHP = heroInfo.initHP;
		heroAttribute.calculateHP = heroAttribute.currentHP;
		heroAttribute.maxEnergy = 5;//TODO
		heroAttribute.heroName = heroInfo.name;//TODO heroInfo.name may counter confilect;
		heroAttribute.speed = heroInfo.moveSpeed;
		heroAttribute.elementType = (_ElementType)heroInfo.series;
		heroAttribute.spriteName = heroInfo.spriteName;
		heroAttribute.coinCount = heroInfo.isBoss==1 ? 100 : 10;
		heroAttribute.soulCount = heroInfo.isBoss==1 ? 100 : 10;
		heroAttribute.coinPerDrop = heroInfo.coinPerDrop;
		heroAttribute.soulPerDrop = heroInfo.soulPerDrop;

		heroAttribute.baseSkillDamageRatio = 1;
		heroAttribute.baseDamage = heroInfo.initAtk;
		heroAttribute.baseDefence = heroInfo.initDef;
		heroAttribute.baseRecover = heroInfo.initRecover;
		heroAttribute.baseCritical = heroInfo.initViolence;
		heroAttribute.skillCaptian = heroInfo.skillCaptian;
		heroAttribute.SkillBase = heroInfo.skillBase;		
		heroAttribute.skillLevel = heroInfo.skillLevel;
		heroAttribute.star = heroInfo.star;
		heroAttribute.ResetBuffAttributes();
		hero.Index = location;
		hero.SkillOdds = 50;
		hero.heroAttack = heroAttack;
		hero.heroAttribute = heroAttribute;
		AttachBody(hero,heroInfo);


		/*if (heroAttribute.heroTypeId == 10001 || heroAttribute.heroTypeId == 10002 || heroAttribute.heroTypeId == 10003 || heroAttribute.heroTypeId == 10004
		    || heroAttribute.heroTypeId == 10005  || heroAttribute.heroTypeId == 10006)
		{
			hero.heroAttribute.skillCaptian = 45004;
			Debug.Log("hero.heroAttribute.skillCaptia..............................." + hero.heroAttribute.skillCaptian);
		}*/
			
		if (hero.heroAttribute.skillCaptian > 0)
		{
			//被动技能
			skillMgr.initSkillbase(heroAttribute.skillCaptian);
		}

		if (hero.heroAttribute.SkillBase > 0)
		{
			//主动技能
			skillMgr.initSkillbase(heroAttribute.SkillBase);
		}

		return hero;
	}

//	static Dictionary<string,GameObject> bodys = new Dictionary<string, GameObject>();
	static void AttachBody(Hero hero,HeroInfo heroInfo)
	{
        GameObject bodyPrefab = null;
        if (SpawnManager.SingleTon().useLocalData)
        {
			bodyPrefab = Resources.Load<GameObject>("Hero/" + heroInfo.type);
			Debug.Log ("<color=yellow>" + heroInfo.fbxFile + "</color>");
			if(bodyPrefab==null){
				bodyPrefab = Resources.Load<GameObject>("Hero/10001");
			}
        } else
        {
            if (!AssetBundleMgr.SingleTon().allCachePrefabs[_AssetBundleType.Hero].ContainsKey(heroInfo.fbxFile))
            {
                
                Debug.LogError("There isn't " + heroInfo.fbxFile + ",load " + AssetBundleMgr.defaultPrefab + " to instead.");
                heroInfo.fbxFile = AssetBundleMgr.defaultPrefab;
                //TODO
            }
            bodyPrefab = AssetBundleMgr.SingleTon().allCachePrefabs[_AssetBundleType.Hero][heroInfo.fbxFile];

        }
		
		GameObject body = Object.Instantiate (bodyPrefab) as GameObject;
		body.transform.parent = hero.transform;
		body.transform.localPosition = Vector2.zero;
		body.transform.localEulerAngles = Vector3.zero;
		hero.HeroBody = body.transform;
		HeroRes res = body.GetComponent<HeroRes>();
		List<AttackAttribute> aas = hero.heroAttack.GetAttackAttributes();		
		hero.heroRes = res;
		hero.heroRes = res;
		if(hero.heroRes.HitPoints!=null && hero.heroRes.HitPoints.Count>0)
			hero.HitPoints = hero.heroRes.HitPoints;
		res.hero = hero;

		hero.heroAnimation = res.gameObject.GetOrAddComponent<HeroAnimation>();
		hero.heroAnimation.heroRes = res;
		hero.heroAnimation.heroResEffect = res.GetComponent<HeroResEffect>();
		hero.heroEffect = res.gameObject.GetOrAddComponent<HeroEffect>();
		hero.heroEffect.heroResEffect = res.GetComponent<HeroResEffect>();
	}
	
	public static Dictionary<string,AudioClip> attackAudioClips;
	public static string baseAttackAudioPath = "Audios/";
	static AudioClip GetBattleAudioClip(string clipName)
	{
		if (attackAudioClips == null)
			attackAudioClips = new Dictionary<string, AudioClip> ();
		if (attackAudioClips.ContainsKey (clipName)) 
		{
			return attackAudioClips [clipName];
		}
		else 
		{
			AudioClip audioClip = Resources.Load<AudioClip>(baseAttackAudioPath + clipName);
			if(audioClip!=null)
				attackAudioClips.Add(audioClip.name,audioClip);
			return audioClip;
		}
	}
	public static BattleInfo getBattleInfo()
	{
		int battleID = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).currBattleID;
		BattleInfo battleInfo = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getBattledatabyBattleID(battleID);
		return battleInfo;
	}
	//	public static float[] AttackQueue0 = new float[]{0.63f,0.1f,0.1f,0.1f,0.1f,0.1f,0.1f,0.1f,0.1f};
	//	public static float[] AttackQueue1 = new float[]{0.1f,0.1f,0.1f,0.1f,0.1f};
	//	public static float[] AttackQueue2 = new float[]{0.05f,0.05f,0.05f,0.05f,0.05f};
	//	public static float[] AttackQueue3 = new float[]{0.1f,0.05f,0.05f,0.05f,0.05f};
	//	public static float[] AttackQueue4 = new float[]{0.63f,0.27f,0.27f,0.27f,0.27f};
	//	public static float[] AttackQueue5 = new float[]{0.05f,0.05f,0.05f,0.05f,0.05f,0.05f};
	
	static public string defaultAudioClip0 = "bf209_se_water1";
	static public string defaultAudioClip1 = "bf210_se_water2";
	static public string defaultAudioClip2 = "bf218_se_saint1";
	static public string defaultAudioClip3 = "bf218_se_saint2";
	static public string defaultAudioClip4 = "bf311_se_battle_attack";
	static public string defaultAudioClip5 = "bf311_se_battle_attack";
//	static public string defaultAudioClip6 = "SE_skillsound_sibada01_hit";
	static public string defaultAudioClip6 = "bf209_se_water1";
	static public Dictionary<int,List<AttackQueue>> attackQueues;
	static public Dictionary<int,List<AttackQueue>> attackFullQueues;

	static public List<AttackQueue> GetAttackFullQueue(int heroId)
	{
		if(attackFullQueues==null)
		{
			InitAttackQueues();
		}
		//		Debug.Log ("heroId:" + heroId);
		if(attackFullQueues.ContainsKey(heroId))
		{
			return attackFullQueues[heroId];
		}
		else
		{
			return attackFullQueues[10001];
		}
	}


	static public List<AttackQueue> GetAttackQueue(int heroId)
	{
		if(attackQueues==null)
		{
			InitAttackQueues();
		}
//		Debug.Log ("heroId:" + heroId);
		if(attackQueues.ContainsKey(heroId))
		{
			return attackQueues[heroId];
		}
		else
		{
			return attackQueues[10001];
		}
	}

	static public float delayPerPart = 0.09f;
	public static void InitAttackQueues()
	{
		attackQueues = new Dictionary<int,List<AttackQueue>> ();
		attackFullQueues = new Dictionary<int, List<AttackQueue>> ();
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.ATTACKPART);
	
		if(table!=null)
		{
			ConfigRow[] rows = table.rows;
			for (int i = 0; i < rows.Length; i ++)
			{
				List<AttackQueue> queue = new List<AttackQueue>();
				List<AttackQueue> queue1 = new List<AttackQueue>();

                int heroId = rows[i].getIntValue(ATTACKPART.HERO_TYPEID);
                float power = rows[i].getFloatValue(ATTACKPART.PART1);
				int delayNum = 1;
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}
                power = rows[i].getFloatValue(ATTACKPART.PART2);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART3);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART4);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART5);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART6);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART7);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART8);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART9);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART10);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART11);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART12);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART13);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART14);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART15);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART16);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART17);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART18);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART19);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}

                power = rows[i].getFloatValue(ATTACKPART.PART20);
				queue1.Add (AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
				if(power>0){
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),delayNum * delayPerPart,0,power));
					delayNum=1;
				}
				else{
					delayNum++;
				}
				if(queue.Count == 0)
				{
					queue.Add(AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip6),0.5f,0,1));
				}
				attackQueues.Add(heroId,queue);
				attackFullQueues.Add (heroId,queue1);
			}
		}


//		attackQueues.Add (10001,InitAttackQueues0());
//		attackQueues.Add (10002,InitAttackQueues0());
//		attackQueues.Add (10003,InitAttackQueues0());
//		attackQueues.Add (10004,InitAttackQueues1());
//		attackQueues.Add (10005,InitAttackQueues1());
//		attackQueues.Add (10006,InitAttackQueues1());
//		attackQueues.Add (10007,InitAttackQueues0());
//		attackQueues.Add (10008,InitAttackQueues0());
//		attackQueues.Add (10009,InitAttackQueues0());
//		attackQueues.Add (10010,InitAttackQueues1());
//		attackQueues.Add (10011,InitAttackQueues1());
//		attackQueues.Add (10012,InitAttackQueues1());
	}
	
//	static AttackQueue[] InitAttackQueues1()
//	{
//		AttackQueue[] queues = new AttackQueue[5];
//		AttackQueue queue = new AttackQueue ();
//		queues [0] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip4),0.1f,0,0.2f);
//		queues [1] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip4),0.1f,0,0.2f);
//		queues [2] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip4),0.1f,0,0.2f);
//		queues [3] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip4),0.1f,0,0.2f);
//		queues [4] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip4),0.1f,0,0.2f);
//		return queues;
//	}
	
//	static AttackQueue[] InitAttackQueues0()
//	{
//		AttackQueue[] queues = new AttackQueue[9];
//		AttackQueue queue = new AttackQueue ();
//		queues [0] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip0),0.0f,0.53f,0.3f);
//		queues [1] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip1),0.1f,0,0.1f);
//		queues [2] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip1),0.1f,0,0.1f);
//		queues [3] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip1),0.1f,0,0.1f);
//		queues [4] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip1),0.1f,0,0.1f);
//		queues [5] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip1),0.1f,0,0.1f);
//		queues [6] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip1),0.1f,0,0.1f);
//		queues [7] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip1),0.1f,0,0.1f);
//		queues [8] = AttackQueue.Instance (GetBattleAudioClip (defaultAudioClip1),0.1f,0,0.1f);
//		return queues;
//	}

}
