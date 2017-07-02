using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public enum _AttackTurn
{
Begin,
Player,
PlayerToEnmey,
Enemy,
EnemyToPlayer,
Waving,
Failure,
Success
}

public class BattleController : MonoBehaviour
{

	public List<Transform> LeftPoints;
	public List<Transform> RightPoints;
	public List<Transform> TopPoints;
	public List<Transform> CenterPoints;

	public List<Hero> LeftHeroes;
	public List<Hero> RightHeroes;
	public List<Hero> CurrentAttackers;
	public List<Hero> DeadHeroes;
	public List<Hero> DeadingHeros;

	public Transform rightPosT;
	public Transform leftPosT;
	public Transform rightSkillMoveTarget;
	public Transform leftSkillMoveTarget;

	public int CurrentWave;
	public List<Wave> Waves;

	public _AttackTurn CurrentStatus = _AttackTurn.Player;
	public GameObject[] ToBodyEffects;
	public Camera BattleCamera;
	public Camera UICamera;
	public List<Drop> Drops;
	public List<HeroBtn> PlayerBattleButtons;
	public float btnEffectDelay;
	public float btnEffectInterval;

	public GameObject TargetMark;
	public Hero HandleTarget;
	public Hero CurrentAttackTarget;
	public GameObject SkillHint;

	public int Coins;
	public int Soals;
	public int Cards;
	public List<int> catchIds;

	public UILabel CoinText;
	public UILabel SoulText;
	public UILabel CardText;
	public UIButton EnableAutoBtn;
	public UIButton DisableAutoBtn;
	public UISprite GlobalMask;
	public UISlider CurrentEnemyHeroHeal;
	public UISprite CurrentEnemyHeroType;
	public UISprite catchSprite;
	public UISprite fingerSprite;
//used to Cross Screen (from left to right)
	public UISprite fingerSprite1;
//used to Cross Screen (from right to left)

	public UISlider bossHealthBar;
	public UILabel bossHealthText;
	public GameObject waveGizmos;
	public Hero bossHero;

	public BoxCollider2D globalControllBtnCollider2D;

	public GameObject AutoMask;
	public Potion[] Potions;

	public GameObject BattleWinEffect;
	public GameObject BattleLoseEffect;
	public GameObject HeroEnterEffect;
	public GameObject BattleWarningEffect;
	public GameObject BattleCatchEffect;

	//	public List<Chest> Chests = new List<Chest>();
	public List<GameObject> EnemyGravestones;
	public List<GameObject> PlayerGravestones;
	public List<GameObject> CatchMonsterEffects;
	public UILabel CatchMonsterAmountLbl;
	public int CatchMonsterAmount;
	public Color defenseColor = new Color (0, 68.0f / 255, 229.0f / 255, 79.0f / 255);

	public GameObject ScreenBlack;
	public GlobalSkillEffect GlobalSkill;
	public AudioClip GlobalSkillAudioClip;
	//	public bool IsToggleTurn;

	public bool IsAuto;
	public bool IsPotioning;
	public bool IsPotionAble = true;
	public bool IsPlaying = true;
	//	public bool IsTesting = false;
	//	public GameObject TestScene;

	public CameraSkillEffectController cameraSkillEffectController;
	public SceneType CurrSceneType;
	public GameObject CurrScene;
	public bool SceneScrolling;

	public SpawnManager spawnManager;

	public int currRoundIndex;
	public BattleState battleState;

	public GameObject summonMask;
	public GameObject SummonBG;
	private int summoncount;
	private UILabel summoncountLbl;
	private float summonSliderValue;
	private GameObject btnSummon;
	private GameObject summonMonsterItemList;
	private SummonMonsterItemController summonItemCtl;
	public bool summoned;
	public bool isSummoning;
	private GameObject ControllBtns;
	private GameObject DismissSummonBtn;
	private GameObject summonPanel;
	private GameObject summonReady;

	private GameObject summonLeftBar;
	private GameObject summonRightBar;
	private GameObject summonBarMask;
	private GameObject summonBarAmount1;
	private GameObject summonBarAmount2;
	private GameObject summonBarAmount3;
	private UISprite summonLeftBarSprite;
	private UISprite summonRightBarSprite;
	private UISprite summonBarAmountSprite1;
	private UISprite summonBarAmountSprite2;
	private UISprite summonBarAmountSprite3;
	private GameObject battleBg;
	private UISprite battleBgSprite;

	public UILabel waveNumberLbl;
	public float moveBackSpeed = 40;
	public float activeGroupAttackLength = 100;

	static BattleController instance;

	public static BattleController SingleTon ()
	{
		return instance;
	}

	void Awake ()
	{
		if (Chest.chests != null)
			Chest.chests.Clear ();
		Application.targetFrameRate = 60;
//		Screen.SetResolution(640, 960, false);
		Screen.SetResolution (960, 640, false);
		if (instance == null)
			instance = this;
		spawnManager = SpawnManager.SingleTon ();
		Potion.Clear ();
		HeroBtn.Clear ();
		UI.PanelStack.me.root = GameObject.Find ("Camera");
		ControllBtns = UI.PanelStack.me.FindPanel ("BattleUI/ControllBtns");	
		if (UI.PanelStack.me.FindPanel ("BattleUI/SmallGizmos/Monster/MonsterLabel") != null)
			CatchMonsterAmountLbl = UI.PanelStack.me.FindPanel ("BattleUI/SmallGizmos/Monster/MonsterLabel").GetComponent<UILabel> ();	
		CatchMonsterEffects = new List<GameObject> ();
		summonPanel = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonPanel");
		summonReady = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonBtn/Summon_Ready");
		DismissSummonBtn = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonPanel/DismissSummonBtn");	
		if (DismissSummonBtn != null)
			UIEventListener.Get (DismissSummonBtn).onClick = onBtnDismissSummonClick;
		summonMonsterItemList = UI.PanelStack.me.FindPanel ("BattleUI/summonMonsterItemList");
		if(summonMonsterItemList!=null)
		summonItemCtl = summonMonsterItemList.GetComponent<SummonMonsterItemController> ();
		summonLeftBar = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonLeftBar");
		summonRightBar = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonRightBar");
		if(summonLeftBar!=null)
		summonLeftBarSprite = summonLeftBar.GetComponent<UISprite> ();
		if(summonRightBar!=null)
		summonRightBarSprite = summonRightBar.GetComponent<UISprite> ();
		summonBarMask = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonBarMask");
		summonBarAmount1 = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonBarAmount1");
		summonBarAmount2 = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonBarAmount2");
		summonBarAmount3 = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonBarAmount3");
		if(summonBarAmount1!=null)summonBarAmountSprite1 = summonBarAmount1.GetComponent<UISprite> ();
		if(summonBarAmount2!=null)summonBarAmountSprite2 = summonBarAmount2.GetComponent<UISprite> ();
		if(summonBarAmount3!=null)summonBarAmountSprite3 = summonBarAmount3.GetComponent<UISprite> ();
		btnSummon = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonBtn");
		if (btnSummon != null)
			UIEventListener.Get (btnSummon).onClick = onBtnSummonClick;
		GameObject waveNumber = UI.PanelStack.me.FindPanel ("BattleUI/SmallGizmos/Wave/WaveNumber");
		if (waveNumberLbl != null)
			waveNumberLbl = waveNumber.GetComponent<UILabel> ();
		summoned = false;
		isSummoning = false;
		summonSliderValue = 0;
		if (AudioManager.SingleTon () != null)
			AudioManager.SingleTon ().PlayMusic (AudioManager.SingleTon ().MusicBattleClip);
		defaultBattleCameraPos = BattleCamera.transform.position;
	}

	void Start ()
	{
		Init ();
		GlobalMask.gameObject.SetActive (false);
		GlobalMask.color = new Color (0, 0, 0, 0);
	}

	public float mCurrentDelay;
	public bool mIsBeginning;
	public bool mIsEnemyTurning;
	public bool mIsPlayerToEnmeyTurning;
	public bool mIsEnemyToPlayerTurning;
	public bool mIsWaving;
	public bool mIsFailure;
	public bool mIsSuccess;
	public bool mIsEnableGlobalControll;
	bool mIsDownloadDone;
	Vector3 mStartMousePos;
	Vector3 mEndMousePos;

	void Update ()
	{
#region 1. State Machine
		//在新控制器下阻止BattleController状态机
		if (BattleManager.GetInstance () != null) {
			return;
		}
		switch (CurrentStatus) {
		case _AttackTurn.Begin:
			OnBegin ();
			break;
		case _AttackTurn.Player:
			OnPlayerTurn ();
			break;
		case _AttackTurn.PlayerToEnmey:
			OnPlayerToEnmey ();
			break;
		case _AttackTurn.Enemy:
			OnEnemyTurn ();
			break;
		case _AttackTurn.EnemyToPlayer:
			OnEnemyToPlayer ();
			break;
		case _AttackTurn.Waving:
			OnWaving ();
			break;
		case _AttackTurn.Failure:
			OnFailure ();
			break;
		case _AttackTurn.Success:
			OnSuccess ();
			break;
		default:
			break;
		}
#endregion
#region 2. Boss healthBar
		if (HandleTarget != null && HandleTarget.IsBoss) {
			bossHero = HandleTarget;
		}
		if (bossHero != null && bossHealthBar != null) {
			bossHealthBar.value = (float)bossHero.heroAttribute.currentHP / bossHero.heroAttribute.maxHP;
		}
#endregion
#region 3. Global Btn Controll
		if (CurrentStatus == _AttackTurn.Player && mIsEnableGlobalControll) {
			if (Input.GetMouseButtonDown (0)) {
				mStartMousePos = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp (0)) {
				mEndMousePos = Input.mousePosition;
				GlobalSkillControll ();
			}
		}
#endregion
#if UNITY_STANDALONE
#region 4. Controll Buttons
		if (Input.GetMouseButtonDown (0)) {
			if (CurrentStatus == _AttackTurn.Player) {
				MarkTarget ();
			}
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			AudioManager.SingleTon ().PlayMusic (AudioManager.SingleTon ().MusicMainClip);
			BaseLib.EventSystem.sendEvent ((int)DataCenter.EVENT_GLOBAL.sys_chgScene, LoadSceneMgr.SCENE_LOGIN);
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			int fieldID = DataManager.getModule<DataTask> (DATA_MODULE.Data_Task).currFieldID;
			DataManager.getModule<DataBattle> (DATA_MODULE.Data_Battle).battleEnd ((uint)fieldID, 0, 0, 0);
			BaseLib.EventSystem.sendEvent ((int)DataCenter.EVENT_GLOBAL.sys_chgScene, LoadSceneMgr.SCENE_MAIN);
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
#endregion
#endif
	}

	public bool isGlobalSkill;

	void GlobalSkillControll ()
	{
		Vector3 screenToWorldPos = UICamera.ScreenToWorldPoint (mStartMousePos);
		Collider2D[] startColls = Physics2D.OverlapPointAll (screenToWorldPos);
		bool isStartAtControllBtns = false;
		foreach (Collider2D coll in startColls) {
			if (coll == globalControllBtnCollider2D) {
				isStartAtControllBtns = true;
				break;
			}
		}
		if (isStartAtControllBtns) {
			float offsetX = mEndMousePos.x - mStartMousePos.x;
			float absOffsetX = Mathf.Abs (offsetX);
			float offsetY = mEndMousePos.y - mStartMousePos.y;
			float absOffsetY = Mathf.Abs (offsetY);
			if (absOffsetX > activeGroupAttackLength && absOffsetX > absOffsetY) {
				isGlobalSkill = true;
				if (offsetX > activeGroupAttackLength) {
					StartCoroutine (AttackQueue (PlayerBattleButtons, false));
				} else {
					StartCoroutine (AttackQueue (PlayerBattleButtons, true));
				}
			}
		}
	}

	IEnumerator AttackQueue (List<HeroBtn> heroBtns, bool skillAble)
	{
		mIsEnableGlobalControll = false;
		BattleController.SingleTon ().AutoMask.SetActive (true);
		List<Hero> heros = new List<Hero> ();
		if (skillAble) {
			if (heroBtns.Count > 1)
				BattleUtility.ShowFinger1 ();
		} else {
			if (heroBtns.Count > 1)
				BattleUtility.ShowFinger ();
		}
		foreach (HeroBtn btn in heroBtns) {
			heros.Add (btn.hero);
			if (heroBtns.Count > 1)
				StartCoroutine (ShowFingerEffect (btn, skillAble));
		}
		List<Hero> skillAbleHeros = new List<Hero> ();
		BattleController.SingleTon ().AutoMask.SetActive (true);
		yield return new WaitForSeconds (0.05f);
		if (skillAble) {
			foreach (Hero hero in heros) {
				//少于6个英雄上场的情况下，hero有可能为空，
				if (hero != null && hero.Status == _HeroStatus.BeforeTurn && hero.gameObject.activeSelf) {
					if (BattleUtility.SkillAble (hero.heroAttribute)) {
						skillAbleHeros.Add (hero);
					}
				}
			}
			if (skillAbleHeros.Count > 0) {
				BattleController.SingleTon ().GlobalSkill.Play (skillAbleHeros);
			}
			while (Time.timeScale == 0) {
				yield return null;
			}
		}
		foreach (Hero hero in heros) {
			if (hero != null && hero.Status == _HeroStatus.BeforeTurn && hero.gameObject != null && hero.gameObject.activeSelf) {
				if (BattleUtility.SkillAble (hero.heroAttribute) && skillAble) {
					hero.heroAttack.Attack (1);
				} else {
					hero.heroAttack.Attack ();
				}
			}
		}
		isGlobalSkill = false;
		BattleController.SingleTon ().AutoMask.SetActive (false);
	}

	IEnumerator ShowFingerEffect (HeroBtn heroBtn, bool skillAble = false)
	{
		int index = BattleController.SingleTon ().PlayerBattleButtons.IndexOf (heroBtn);
		if (skillAble) {
			index = BattleController.SingleTon ().PlayerBattleButtons.Count - index - 1;
		}
		float delay = BattleController.SingleTon ().btnEffectDelay;
		float interval = BattleController.SingleTon ().btnEffectInterval;
		float totalDelay = delay + interval * index;
		float t = 0;
		while (t < totalDelay) {
			t += RealTime.deltaTime;
			yield return null;
		}
		if (AudioManager.me != null)
			AudioManager.me.PlayBtnActionClip ();
		heroBtn.ShowLinerEffect ();
	}

	void Init ()
	{
//		PreparePools ();
//		GlobalSkillAudioClip = Resources.Load ("bf233_se_skill_action") as AudioClip;
//		HeroEnterEffect = Resources.Load ("Effect/Effect_Hero_Coming") as GameObject;
//		BattleCatchEffect = Resources.Load ("Effect/Effect_Catch") as GameObject;
//		EnableAutoBtn.onClick.Add (new EventDelegate (EnableAuto));
//		DisableAutoBtn.onClick.Add (new EventDelegate (DisableAuto));
//		CurrentWave = 0;
//		Waves = spawnManager.waves;
//		RightHeroes = spawnManager.playerHeros;
		BattleInfo battleInfo = SpawnUtility.getBattleInfo ();
//		cameraSkillEffectController = Camera.main.transform.gameObject.GetComponent<CameraSkillEffectController> ();
		LoadScene (battleInfo);
//
////		PotionUtility.InitBattlePotions(Potions);
//		CurrentStatus = _AttackTurn.Begin;
//		IsPlaying = true;
		BattleUtility.InitBattlePlayerHeros ();
		BattleUtility.InitHeroHealthBars ();
//		HeroBtn.InActiveAll ();
//		Potion.InActiveAll ();
	}

	void LoadScene (BattleInfo battleInfo)
	{
		CurrScene = spawnManager.sceneObject;
		SceneScrolling = false;
		if (battleInfo.SCENETYPE == SceneType.scroll) {
			if (CurrScene.GetComponent<ScrollingScript> () != null || CurrScene.GetComponentsInChildren<ScrollingScript> () != null) {
				CurrSceneType = battleInfo.SCENETYPE;
				//StartCoroutine(ScrollScene(CurrScene));
				StartCoroutine (ScrollScene (false));
			}
		}
	}

	IEnumerator ScrollScene (bool sceneRoll)
	{

		if (sceneRoll == SceneScrolling)
			yield return null;
		else
			SceneScrolling = sceneRoll;

		for (int i = 0; i < CurrScene.transform.childCount; i++) {
			Transform child = CurrScene.transform.GetChild (i);

			for (int j = 0; j < child.childCount; j++) {
				if (child.GetChild (j) != null && child.GetChild (j).gameObject.activeSelf && child.GetChild (j).GetComponent<ScrollingScript> () != null)
					child.GetChild (j).GetComponent<ScrollingScript> ().enabled = sceneRoll;
			}

			//if (CurrScene.transform.GetChild(i) != null && CurrScene.transform.GetChild(i).gameObject.activeSelf && CurrScene.transform.GetChild(i).GetComponent<ScrollingScript>() != null)
			//	CurrScene.transform.GetChild(i).GetComponent<ScrollingScript>().enabled = sceneRoll;
		}

		if (sceneRoll == false) {
			foreach (Hero hero in RightHeroes) {
				if (hero.heroAnimation.heroRes.CurrentAm.clipName != Hero.ACTION_STANDBY)
					hero.heroAnimation.Play (Hero.ACTION_STANDBY);

				//Debug.Log("Hero.ACTION_STANDBY..............................................................." + hero.name);
			}
		} else if (sceneRoll == true) {
			foreach (Hero hero in RightHeroes) {
				if (hero.heroAnimation.heroRes.CurrentAm.clipName != Hero.ACTION_RUN)
					hero.heroAnimation.Play (Hero.ACTION_RUN);
				hero.Status = _HeroStatus.BeforeTurn;
				//Debug.Log("Hero.ACTION_RUN..............................................................." + hero.name);
			}
		}
		yield return null;

	}

	IEnumerator ScrollScene (GameObject scene)
	{
		for (int i = 0; i < scene.transform.childCount; i++) {

			if (scene.transform.GetChild (i) != null && scene.transform.GetChild (i).gameObject.activeSelf && scene.transform.GetChild (i).GetComponent<ScrollingScript> () != null)
				scene.transform.GetChild (i).GetComponent<ScrollingScript> ().enabled = true;
		}

		/*foreach(Hero hero in RightHeroes)
		{
			hero.heroAnimation.Play(Hero.ACTION_RUN);
		}*/
		SceneScrolling = true;
		yield return new WaitForSeconds (4);
		SceneScrolling = false;

		for (int i = 0; i < scene.transform.childCount; i++) {
			if (scene.transform.GetChild (i) != null && scene.transform.GetChild (i).gameObject.activeSelf && scene.transform.GetChild (i).GetComponent<ScrollingScript> () != null)
				scene.transform.GetChild (i).GetComponent<ScrollingScript> ().enabled = false;
		}

		foreach (Hero hero in RightHeroes) {
			hero.heroAnimation.Play ("StandBy");
		}


		/*foreach(Hero hero in RightHeroes)
		{
			hero.heroAnimation.Play(Hero.ACTION_STANDBY);
		}*/
	}

	void ClearChest ()
	{
		foreach (Chest chest in Chest.chests) {
			PoolManager.SingleTon ().UnSpawn (chest.gameObject);
		}
		Chest.chests.Clear ();
	}

	void ClearDeathObjects ()
	{
		foreach (GameObject obj in EnemyGravestones) {
			PoolManager.SingleTon ().UnSpawn (obj);
		}
		EnemyGravestones.Clear ();
		if (CurrSceneType == SceneType.scroll) {
			foreach (GameObject obj in PlayerGravestones) {
				PoolManager.SingleTon ().UnSpawn (obj);
			}
			PlayerGravestones.Clear ();
		}
	}

	public IEnumerator ExtraEffectForHeroEnter (Hero hero, Vector3 targetPos)
	{
		Debug.Log ("ExtraEffectForHeroEnter");
		for (int i = 0; i < 20; i++) {
			GameObject extraEffectPrefab = Resources.Load ("Effect/Meteorolite") as GameObject;
			//float x = Random.Range(-3,11);
			float x = Random.Range (0, -11);
			float y = Random.Range (0, -9);
			//Debug.Log("extraEffect.....................x..." + x + "......y...." + y);
			GameObject extraEffect = Instantiate (extraEffectPrefab, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
			extraEffect.transform.localEulerAngles = new Vector3 (0, -180, 0);
			extraEffect.SetActive (true);
			//float time = Random.Range(0.05f,0.2f);
			yield return new WaitForSeconds (0.1f);
		}
		for (int i = 0; i < 20; i++) {
			ShakeCamera (0.5f, 0);
			float time = Random.Range (0.05f, 0.1f);
			yield return new WaitForSeconds (time);
		}
		yield return new WaitForSeconds (1f);
		hero.gameObject.SetActive (true);

		//StartCoroutine(HeroEnter(hero,targetPos));
	}

	IEnumerator HeroEnter (Hero hero, Vector3 targetPos)
	{
		float t = 0;
		Vector3 startPos = hero.transform.position;

//		if (CurrSceneType == SceneType.scroll)
//			hero.heroRes.Play(HeroRes.ANIMATION_CLIP_STANDBY);
//		else
		hero.heroAnimation.Play (Hero.ACTION_RUN);

		/*if (hero.IsBoss)
		{
			Debug.Log("ExtraEffectForDragonEnter");
			StartCoroutine("ExtraEffectForDragonEnter");
		}*/

		//float length = hero.heroAnimation.GetAnimClipLength(Hero.ACTION_RUN);

		if (hero.Side == _Side.Enemy && CurrSceneType == SceneType.scroll) {
			//yield return new WaitForSeconds(3.5f);
		}

		Vector3 offset = startPos - targetPos;
		float sqrLen = offset.sqrMagnitude;
		float m = sqrLen;

		while (sqrLen > 1) {
			//t += Time.deltaTime * 0.5f;
			t += Time.deltaTime * (1 / hero.heroAttribute.speed) * 5f;
			hero.transform.position = Vector3.Lerp (startPos, targetPos, t);				
			offset = hero.transform.position - targetPos;
			sqrLen = offset.sqrMagnitude;
			//Debug.Log("sqrLen..................................................." + sqrLen + "....................." + hero.name + ".........." + t);
			yield return null;
		}
		
		/*while(t < length)
		{
			t += Time.deltaTime * 5;
			hero.transform.position = Vector3.Lerp(startPos,targetPos,t);	
			Debug.Log("t..................................................." + t);

			yield return null;
		}*/
		

		if (hero.Side == _Side.Player) {
			StartCoroutine (ScrollScene (true));
		} else {
			hero.Status = _HeroStatus.BeforeTurn;
			hero.heroAnimation.Play (Hero.ACTION_STANDBY);
		}

	}

	IEnumerator ExtraEffectForDragonEnter ()
	{
		GameObject extraEffectPrefab = Resources.Load ("Effect/DragonLandSmoke") as GameObject;
//		GameObject extraEffect = Instantiate(extraEffectPrefab,new Vector3(-8.56f,-4.1f,0),Quaternion.identity) as GameObject;
		Instantiate (extraEffectPrefab, new Vector3 (-8.56f, -4.1f, 0), Quaternion.identity);
		yield return null;
	}

	public void EnableAuto ()
	{
		IsAuto = true;
		DisableAutoBtn.gameObject.SetActive (true);
		EnableAutoBtn.gameObject.SetActive (false);
		AutoMask.SetActive (true);
//		AutoPlayerTurn();
	}

	public void DisableAuto ()
	{
		IsAuto = false;
		DisableAutoBtn.gameObject.SetActive (false);
		EnableAutoBtn.gameObject.SetActive (true);
		AutoMask.SetActive (false);
	}

	void Clear ()
	{
		HandleTarget = null;
		CurrentAttackers.Clear ();
	}

	void OnBegin ()
	{
		if (!mIsBeginning) {
			Debug.Log ("OnBegin");
			mIsBeginning = true;
			StartCoroutine (_BattleBegin ());
		}
	}

	void OnPlayerTurn ()
	{
		if (RightHeroes.Count == 0) {
			CurrentStatus = _AttackTurn.Failure;
		} else if (LeftHeroes.Count == 0) {

			if (CurrentWave >= Waves.Count) {
				CurrentStatus = _AttackTurn.Success;
			} else {
				CurrentStatus = _AttackTurn.Waving;
				//doBuffs();
			}
		} else {
			if (IsAuto) {
				if (!mIsInAutoAttack) {
					mIsInAutoAttack = true;
//					foreach(Hero hero in RightHeroes){
//						hero.sm.ChangeStatus (_HeroMachineStatus.MoveToAttack.ToString());
//					}
					StartCoroutine ("AutoAttack");
				}
			}
			bool changeAble = true;
			for (int i = 0; i < RightHeroes.Count; i++) {
				if (RightHeroes [i] != null && RightHeroes [i].gameObject != null && RightHeroes [i].gameObject.activeSelf && RightHeroes [i].Status != _HeroStatus.AfterTurn) {
					changeAble = false;
					break;
				}
			}
			if (changeAble) {
				CurrentStatus = _AttackTurn.PlayerToEnmey;
				mIsEnableGlobalControll = true;
			}
		}
	}

	bool mIsInAutoAttack;

	IEnumerator AutoAttack ()
	{
		List<Hero> skillAbleHeros = new List<Hero> ();
		BattleController.SingleTon ().AutoMask.SetActive (true);
		foreach (Hero hero in RightHeroes) {
			if (hero.Status == _HeroStatus.BeforeTurn && hero.gameObject.activeSelf) {
				if (BattleUtility.SkillAble (hero.heroAttribute)) {
					skillAbleHeros.Add (hero);
				}
			}
		}
		if (skillAbleHeros.Count > 0) {
			BattleController.SingleTon ().GlobalSkill.Play (skillAbleHeros);
		}
		while (Time.timeScale == 0) {
			yield return null;
		}
		foreach (Hero hero in RightHeroes) {
			if (hero.Status == _HeroStatus.BeforeTurn && hero.gameObject.activeSelf) {
				if (BattleUtility.SkillAble (hero.heroAttribute)) {
					hero.heroAttack.Attack (1);
				} else {
					hero.heroAttack.Attack ();
				}
//				hero.sm.ChangeStatus(_HeroMachineStatus.Scan.ToString());
			}
			Debug.Log ("<color=yellow>" + hero.Status + "</color>");
			while (hero.Status != _HeroStatus.AfterTurn) {
				yield return new WaitForSeconds (2);
			}
		}
		mIsInAutoAttack = false;
	}

	void OnPlayerToEnmey ()
	{
		if (!mIsPlayerToEnmeyTurning) {
			Debug.Log ("OnPlayerToEnmey");
			mIsPlayerToEnmeyTurning = true;
			StartCoroutine (_PlayerToEnmey ());
		}
	}

	float enemyTurnDuration = 0;

	void OnEnemyTurn ()
	{
		if (RightHeroes.Count == 0) {
			CurrentStatus = _AttackTurn.Failure;
		} else if (LeftHeroes.Count == 0) {
			if (CurrentWave >= Waves.Count) {
				CurrentStatus = _AttackTurn.Success;
			} else {
				CurrentStatus = _AttackTurn.Waving;
			}
		} else {
			bool changeAble = true;
			for (int i = 0; i < LeftHeroes.Count; i++) {
				if (LeftHeroes [i].Status != _HeroStatus.AfterTurn) {
					changeAble = false;
					break;
				}
			}
			enemyTurnDuration += Time.deltaTime;
			if (enemyTurnDuration > 10) {
				Debug.LogError ("Enemy turn has error!");
			}
			if (changeAble || enemyTurnDuration > 10) {
				enemyTurnDuration = 0;
				CurrentStatus = _AttackTurn.EnemyToPlayer;
				mIsEnemyTurning = false;
			} else {
				if (!mIsEnemyTurning) {
					mIsEnemyTurning = true;
					StartCoroutine ("_EnemyTurn");
				}
			}
		}
	}

	void OnEnemyToPlayer ()
	{
		if (!mIsEnemyToPlayerTurning) {
			Debug.Log ("OnEnemyToPlayer");
			mIsEnemyToPlayerTurning = true;
			StartCoroutine (_EnemyToPlayer ());
		}
	}

	void OnWaving ()
	{
		if (!mIsWaving) {
			Debug.Log ("OnWaving");
			mIsWaving = true;
			StartCoroutine ("_Waving");
		}
	}

	void OnFailure ()
	{
		if (!mIsFailure) {
			Debug.Log ("OnFailure");
			mIsFailure = true;
			StartCoroutine ("_BattleFailure");
		}
	}

	void OnSuccess ()
	{
		if (!mIsSuccess) {
			Debug.Log ("OnSuccess");
			mIsSuccess = true;
			StartCoroutine ("_BattleWin");
		}
	}

	float battleBeginStartDelay = 0.2f;
	float battleBeginFinishDelay = 0.5f;
	float battleBeginDelay = 0.2f;
	float battleBeginDelay1 = 0.5f;

	IEnumerator _BattleBegin ()
	{
		Debug.Log ("_BattleBegin");
		yield return new WaitForSeconds (battleBeginStartDelay);
		if (CatchMonsterEffects != null)
			CatchMonsterEffects.Clear ();
		CatchMonsterAmount = 0;
		TargetMark.SetActive (false);
		/*EnterNextWave();
		bool startable = false;
		yield return null;
		while(!startable)
		{
			foreach(Hero hero in LeftHeroes)
			{
				if(hero.Status != _HeroStatus.BeforeTurn)
				{
					startable = false;
					break;
				}
				startable = true;
			}
			yield return null;
		}
		yield return new WaitForSeconds(battleBeginDelay);*/


		foreach (Hero hero in RightHeroes) {
			StartCoroutine (PlayerEnter (hero));
			yield return new WaitForSeconds (0.3f);
		}
		yield return new WaitForSeconds (battleBeginDelay1);
		/*Potion.ActiveAll();
		HeroBtn.ActiveAll();
		yield return new WaitForSeconds(battleBeginFinishDelay);*/

		//mIsBeginning = false;
		//Turn = _AttackTurn.Player;



		StartCoroutine (ScrollScene (true));
		yield return new WaitForSeconds (0.5f);
		StartCoroutine (ScrollScene (false));
		//yield return new WaitForSeconds(battleBeginDelay);


		EnterNextWave ();
		bool startable = false;
		yield return null;
		while (!startable) {
			foreach (Hero hero in LeftHeroes) {
				if (hero.Status != _HeroStatus.BeforeTurn) {
					startable = false;
					break;
				}
				startable = true;
			}
			yield return null;
		}
		mIsBeginning = false;
		CurrentStatus = _AttackTurn.Player;
		mIsEnableGlobalControll = true;
		Potion.ActiveAll ();
		HeroBtn.ActiveAll ();
		yield return new WaitForSeconds (battleBeginFinishDelay);
		battleState = BattleState.BattleBegin;
		doSkill ();
	}

	float mCollectDelay = 2;
	float mBattleWinDelay = 1;
	float mBattleFailureDelay = 1;

	IEnumerator _BattleWin ()
	{
		bool collectAble = false;

		while (!collectAble) {
			foreach (Hero hero in RightHeroes) {
				if (hero.Status != _HeroStatus.AfterTurn && hero.Status != _HeroStatus.BeforeTurn) {
					collectAble = false;
					break;
				}
				collectAble = true;
			}
			yield return null;
		}

		DropUtility.Collect (Drops, RightHeroes);
		while (Drops.Count > 0) {
			Debug.Log ("Collect");
			yield return null;
		}

		//捕获怪物特效移动效果
		CatchMonsterEffect ();
		foreach (Hero hero in RightHeroes) {
			

			hero.heroRes.CurrentAm.anim.wrapMode = WrapMode.Loop;
			hero.heroAnimation.Play (Hero.ACTION_CHEER);
//			hero.heroRes.CurrentAm.anim.PlayQueued("Cheer");
//			hero.heroRes.CurrentAm.anim.PlayQueued("Cheer");
//			hero.heroRes.CurrentAm.anim.PlayQueued("Cheer");
//			hero.heroRes.CurrentAm.anim.wrapMode = WrapMode.Loop;
//			hero.heroAnimation.Play (Hero.ACTION_STANDBY);
//			hero.heroRes.CurrentAm.anim.PlayQueued("StandBy");
		}
		TargetMark.SetActive (false);
		yield return new WaitForSeconds (mBattleWinDelay);//TODO maybe need config able
		AudioManager.SingleTon ().PlayBattleWinClip ();
		BattleWinEffect.SetActive (true);
//		TweenPosition tp = BattleWinEffect.GetComponent<TweenPosition>();
//		tp.ResetToBeginning();
//		tp.PlayForward();
		yield return new WaitForSeconds (1);
//		iTween.ShakePosition(BattleWinEffect,new Vector3(0.05f,0.01f,0),0.5f);
		DataCenter.DataManager.me.CurrentPanelIndex = 5;
		StartCoroutine (_BackToMain (true));
	}

	IEnumerator _BattleFailure ()
	{
		yield return new WaitForSeconds (mBattleFailureDelay);//TODO maybe need config able
		TargetMark.SetActive (false);
		BattleLoseEffect.SetActive (true);
//		TweenPosition tp = BattleLoseEffect.GetComponent<TweenPosition>();
//		tp.ResetToBeginning();
//		tp.PlayForward();
		yield return new WaitForSeconds (1);
//		iTween.ShakePosition(BattleLoseEffect,new Vector3(0.05f,0.01f,0),0.5f);
		StartCoroutine (_BackToMain (false));
	}

	float mWavingStartDelay = 0;
	float mWavingFinishDelay = 1;
	float mBackToPlayerDelay = 2;

	IEnumerator _Waving ()
	{
		Debug.Log ("In _Waving!");
		AutoMask.SetActive (true);
		yield return new WaitForSeconds (mCollectDelay);
		TargetMark.SetActive (false);

		foreach (Hero hero in RightHeroes) {
			hero.heroAttack.UnDefense ();
		}

		bool collectAble = false;
		if (Chest.chests.Count > 0) {
			foreach (Chest chest in Chest.chests) {
				if (!chest.isOpening && !chest.isOpened)
					ShowChestFinger (chest);
			}
			yield return new WaitForSeconds (1);
		}
		while (!collectAble && Chest.chests.Count > 0) {
			foreach (Chest chest in Chest.chests) {
				if (!chest.isOpened) {
					collectAble = false;
					if (IsAuto && !chest.IsFalling ()) {
						chest.Open ();
					}
					break;
				}
				collectAble = true;
			}
			Debug.Log ("collectAble 1");
			yield return new WaitForSeconds (0.1f);
		}

		collectAble = false;
		while (!collectAble) {
			foreach (Drop drop in Drops) {
				if (drop.isFalling) {
					collectAble = false;
					break;
				}
				collectAble = true;
			}
			if (Drops.Count == 0) {
				collectAble = true;
			}
			yield return null;
		}
		DropUtility.Collect (Drops, RightHeroes);
		while (Drops.Count > 0) {
			Debug.Log ("collectAble 4");
			yield return null;
		}
		yield return new WaitForEndOfFrame ();
		Debug.Log ("Checking deading heros!");
		while (DeadingHeros.Count > 0) {
			yield return null;
		}
		CatchMonsterEffect ();
		Debug.Log ("Waiting catch effects!");
		while (CatchMonsterEffects.Count > 0) {
			yield return null;
		}

		if (LeftHeroes.Count > 0) {
			yield return new WaitForSeconds (mBackToPlayerDelay);
			CurrentStatus = _AttackTurn.EnemyToPlayer;
			Debug.Log ("Stop _Waving");
			AutoMask.SetActive (false);
			mIsWaving = false;
			StopCoroutine ("_Waving");
		}
		collectAble = false;
		while (!collectAble) {
			foreach (Hero hero in RightHeroes) {
				if (hero.Status != _HeroStatus.AfterTurn && hero.Status != _HeroStatus.BeforeTurn) {
					collectAble = false;
					break;
				}
				collectAble = true;
				//能量加成
				if (hero.heroAttribute.additionEnergy > 0) {
					hero.heroAttribute.currentEnergy += Mathf.FloorToInt (hero.heroAttribute.additionEnergy);
					hero.Btn.RequireUpdate = true;
				}
			}
			Debug.Log ("collectAble 2");
			yield return null;
		}
		yield return new WaitForSeconds (mWavingStartDelay);
		if (CurrSceneType == SceneType.scroll) {
			ClearDeathObjects ();
			ClearChest ();
			//yield return new WaitForSeconds(1);
			foreach (Hero hero in RightHeroes) {
				hero.heroAnimation.Play ("Run");
			}
			StartCoroutine (ScrollScene (true));
			yield return new WaitForSeconds (2);
			StartCoroutine (ScrollScene (false));
		} else {
			UITweener tw = ScreenBlack.GetComponent<UITweener> ();
			tw.delay = 3.0f;
			tw.PlayForward ();
//			foreach(Hero hero in RightHeroes)
//			{
//				hero.updateHealbar = false;
//				hero.healthBar.gameObject.SetActive(false);
//			}
			yield return new WaitForSeconds (tw.delay + tw.duration);
			Clear ();
			ClearChest ();
			ClearDeathObjects ();
			HideRelationHints ();
			ScreenBlack.GetComponent<UITweener> ().delay = 0;
			ScreenBlack.GetComponent<UITweener> ().PlayReverse ();
			yield return new WaitForSeconds (tw.delay + tw.duration);
//			foreach(Hero hero in RightHeroes)
//			{
//				hero.updateHealbar = true;
//				hero.healthBar.gameObject.SetActive(true);
//			}
		}
		//boss关的警告提示
		if (CurrentWave == Waves.Count - 1) {
			yield return new WaitForSeconds (0.5f);
			float warningDuraiton = 2;
			BattleWarningEffect.SetActive (true);
			yield return new WaitForSeconds (warningDuraiton);
			BattleWarningEffect.SetActive (false);
			if (bossHealthBar != null) {
				foreach (Hero hero in Waves[CurrentWave].heros) {
					//if(hero.Index == 1)
					if (hero.IsBoss) {
						bossHero = hero;
						break;
					}
				}


				if (bossHero == null) {
					Debug.LogError ("There is no boss hero!");
					bossHero = Waves [CurrentWave].heros [0];
				}
				if (bossHero != null) {
					if (bossHealthBar != null) {
						bossHealthBar.gameObject.SetActive (true);
						waveGizmos.SetActive (false);
					}
				}
			}

		}
		EnterNextWave ();
		bool startable = false;
		while (!startable) {
			foreach (Hero hero in LeftHeroes) {
				if (hero.Status != _HeroStatus.BeforeTurn) {
					startable = false;
					break;
				}
				startable = true;
			}
			yield return null;
		}
		foreach (Hero hero in RightHeroes) {
			if (hero.heroAttribute.currentHP > 0) {
				if (hero.Btn != null)
					hero.Btn.ActiveButton ();
				hero.Status = _HeroStatus.BeforeTurn;
			}
		}	
		Potion.ActiveAll ();
		yield return new WaitForSeconds (mWavingFinishDelay);
		CurrentStatus = _AttackTurn.Player;
		mIsEnableGlobalControll = true;
		mIsWaving = false;
		AutoMask.SetActive (false);
	}

	IEnumerator _EnemyTurn ()
	{
		bool isPalsy = false;
		if (HandleTarget != null)
			TargetMark.SetActive (false);
		for (int i = 0; i < LeftHeroes.Count; i++) {

			foreach (AnnormalState state in LeftHeroes[i].heroAttribute.annormalStateList) {
				if (state.type == AnnormalType.palsy) {
					isPalsy = true;
					break;
				}
			}

//			if(LeftHeroes[i].Status == _HeroStatus.BeforeTurn  && isPalsy == false)
//            if(LeftHeroes[i].Status == _HeroStatus.BeforeTurn)
//			{
			if (LeftHeroes [i].IsBoss == true) {
				int ran = Random.Range (1, 10);
				if (ran >= 1 && ran <= 3)
					LeftHeroes [i].heroAttack.Attack (1);
				else
					LeftHeroes [i].heroAttack.Attack ();
			} else
				LeftHeroes [i].heroAttack.Attack ();
			//yield return new WaitForSeconds(0.5f);
//                yield return new WaitForSeconds(0.5f);
//			}
//			else
//				LeftHeroes[i].Status = _HeroStatus.AfterTurn;
			yield return new WaitForSeconds (0.5f);
		}
	}

	float mCommonTurningDur = 1.5f;

	IEnumerator _PlayerToEnmey ()
	{
		bool collectAble = false;
		while (!collectAble) {
			foreach (Hero hero in RightHeroes) {
				if (hero != null && hero.gameObject != null && hero.gameObject.activeSelf && hero.Status != _HeroStatus.AfterTurn && hero.Status != _HeroStatus.BeforeTurn) {
					collectAble = false;
					break;
				}
				collectAble = true;
			}
			yield return null;
		}
		DropUtility.Collect (Drops, RightHeroes);
		while (Drops.Count > 0) {
			//Debug.Log("Collect");
			yield return null;
		}
//		InitLeftTurn();
		if (HandleTarget != null) {
			TargetMark.SetActive (true);
		}
		bool isPalsy = false;
		foreach (Hero hero in LeftHeroes) {
			foreach (AnnormalState state in hero.heroAttribute.annormalStateList) {
				if (state.type == AnnormalType.palsy) {
					isPalsy = true;
					break;
				}
			}
			if (hero.heroAttribute.currentHP > 0 && isPalsy == false) {
				hero.Status = _HeroStatus.BeforeTurn;
			}
		}
		battleState = BattleState.RoundEnd;
		//doSkill();
		doBuffs ();
		CurrentStatus = _AttackTurn.Enemy;
		mIsPlayerToEnmeyTurning = false;

	}

	IEnumerator _EnemyToPlayer ()
	{

		yield return new WaitForSeconds (mCommonTurningDur);
		bool isPalsy = false;
		foreach (Hero hero in RightHeroes) {
			if (hero.gameObject.activeSelf) {
				foreach (AnnormalState state in hero.heroAttribute.annormalStateList) {
					if (state.type == AnnormalType.palsy) {
						isPalsy = true;
						break;
					}
				}
				if (hero.heroAttribute.currentHP > 0 && isPalsy == false) {
					if (hero.Btn != null)
						hero.Btn.ActiveButton ();
					hero.Status = _HeroStatus.BeforeTurn;
				}
			}
		}
		Potion.ActiveAll ();
		foreach (Hero hero in RightHeroes) {
			hero.heroAttack.UnDefense ();
		}
		//doSkill();
		CurrentStatus = _AttackTurn.Player;
		mIsEnemyToPlayerTurning = false;
		if (HandleTarget != null)
			ShowTargetMark (HandleTarget);
	}

	void EnterNextWave ()
	{

		waveNumberLbl.text = (CurrentWave + 1) + " / " + Waves.Count;
		List<Hero> waveHeros = Waves [CurrentWave].heros;
		LeftHeroes = new List<Hero> ();
		foreach (Hero hero in waveHeros) {
			hero.gameObject.SetActive (true);
			hero.heroAttack.defaultPos = LeftPoints [hero.Index].position;
			//If hero has run anim,let him run into scene
			if (hero.IsBoss) {
				Debug.Log (hero.IsBoss);
				//hero.transform.position = LeftPoints[hero.Index].position - new Vector3(3,0,0);
				hero.Status = _HeroStatus.BeforeTurn;
				hero.transform.position = LeftPoints [hero.Index].position;
				hero.HeroBody.localPosition = new Vector3 (4.4f, -1f, 0);
				//StartCoroutine(ExtraEffectForHeroEnter(hero,LeftPoints[hero.Index].position));
			} else if (hero.heroAnimation.HasAnimClip (Hero.ACTION_RUN)) {
				hero.transform.position = LeftPoints [hero.Index].position - new Vector3 (13, 0, 0);
				hero.Status = _HeroStatus.Entering;
				StartCoroutine (HeroEnter (hero, LeftPoints [hero.Index].position));
			} else {
				hero.Status = _HeroStatus.BeforeTurn;
				hero.transform.position = LeftPoints [hero.Index].position;
			}
			LeftHeroes.Add (hero);
		}
		CurrentWave++;
	}

	IEnumerator PlayerEnter (Hero hero)
	{
		/*hero.Status = _HeroStatus.Entering;
		PoolManager.SingleTon().Spawn(HeroEnterEffect,RightPoints[hero.Index].position,Quaternion.identity,0.1f,3);
		yield return new WaitForSeconds(0.5f);
		hero.transform.position = RightPoints[hero.Index].position;
		iTween.ShakePosition(BattleCamera.gameObject,Vector3.one * 0.2f,0.1f);
		yield return new WaitForSeconds(0.5f);
		hero.Status = _HeroStatus.BeforeTurn;
		hero.heroAnimation.Play("StandBy");*/


		if (hero.heroAnimation.HasAnimClip (Hero.ACTION_RUN)) {
			hero.transform.position = RightPoints [hero.Index].position + new Vector3 (12, 0, 0);
			hero.Status = _HeroStatus.Entering;
			StartCoroutine (HeroEnter (hero, RightPoints [hero.Index].position));
		}
		yield return null;


		/*if (hero.IsBoss)
		{
			//hero.transform.position = RightPoints[hero.Index].position - new Vector3(12,0,0);
			StartCoroutine(ExtraEffectForHeroEnter(hero,LeftPoints[hero.Index].position));
		}*/
	}

	public void NextHandleTarget ()
	{
		Debug.Log ("NextHandleTarget");
		bool changed = false;
		foreach (Hero hero in LeftHeroes) {
			if (hero != HandleTarget) {
				HandleTarget = hero;
				changed = true;
				break;
			}
		}
		if (!changed) {
			HandleTarget = null;
		}
		if (HandleTarget != null) {
			ShowTargetMark (HandleTarget);
		}
	}

	void MarkTarget ()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Collider2D coll = Physics2D.OverlapPoint (pos);
		if (coll != null && coll.GetComponent<HeroRes> () != null) {
			Hero hero = coll.GetComponent<HeroRes> ().hero;
			if (hero != null && hero.Side == _Side.Enemy) {
				if (HandleTarget == hero) {
					HideTargetMark ();
				} else {
					ShowTargetMark (hero);
				}
				StartCoroutine (ScaleHero (hero));
			}
		}
	}

	public float heroScaleDuration = 0.5f;
	public Vector3 heroTargetScale = new Vector3 (2, 2, 1);
	public AnimationCurve scaleCurve = AnimationCurve.EaseInOut (0, 0, 1, 1);
	HashSet<Hero> mScalingHeros = new HashSet<Hero> ();
	public Vector3 heroDefaultScale = Vector3.one;

	IEnumerator ScaleHero (Hero hero)
	{
		float t = 0;
		float value = 0;
		if (!mScalingHeros.Contains (hero)) {
			mScalingHeros.Add (hero);
			while (t < 1) {
				t += Time.deltaTime / heroScaleDuration;
				value = scaleCurve.Evaluate (t);
				hero.transform.localScale = Vector3.Lerp (heroDefaultScale, heroTargetScale, value);
				yield return null;
			}
			t = 1;
			value = 0;
			while (t > 0) {
				t -= Time.deltaTime / heroScaleDuration;
				value = scaleCurve.Evaluate (t);
				hero.transform.localScale = Vector3.Lerp (heroDefaultScale, heroTargetScale, value);
				yield return null;
			}
			mScalingHeros.Remove (hero);
			hero.transform.localScale = heroDefaultScale;
		}
	}

	void ShowTargetMark (Hero hero)
	{
		HandleTarget = hero;
		TargetMark.transform.position = BattleUtility.GetHeroHeadPos (hero.heroRes);
		if (!TargetMark.activeInHierarchy)
			TargetMark.SetActive (true);
		BattleUtility.ShowRelationHints ();//TODO
	}

	void HideTargetMark ()
	{
		HandleTarget = null;
		HideRelationHints ();
		if (TargetMark.activeInHierarchy)
			TargetMark.SetActive (false);
	}

	//TODO
	IEnumerator _BackToMain (bool isWin)
	{
		yield return null;
		BaseLib.EventSystem.sendEvent ((int)DataCenter.EVENT_GLOBAL.sys_chgScene, LoadSceneMgr.SCENE_BATTLE);
	}

	IEnumerator _BackToMain1 (bool isWin)
	{
		yield return new WaitForSeconds (1);
		float t = 0;
		foreach (HeroBtn heroBtn in PlayerBattleButtons) {
			heroBtn.gameObject.SetActive (false);
			yield return null;
		}
		GlobalMask.gameObject.SetActive (true);
		while (t < 1) {
			t += Time.deltaTime / 2;
			GlobalMask.alpha = t;
			yield return null;
		}
		if (AudioManager.SingleTon () != null)
			AudioManager.SingleTon ().PlayMusic (AudioManager.SingleTon ().MusicMainClip);
		//Application.LoadLevel("Main");
		BattleResultInfo bri = new BattleResultInfo ();
		FieldInfo fieldInfo = DataManager.getModule<DataTask> (DATA_MODULE.Data_Task).getFieldDataByFieldID (DataManager.getModule<DataTask> (DATA_MODULE.Data_Task).currFieldID);
		BattleInfo battleInfo = DataManager.getModule<DataTask> (DATA_MODULE.Data_Task).getBattledatabyBattleID (DataManager.getModule<DataTask> (DATA_MODULE.Data_Task).currBattleID);
		if (!isWin) {
			bri.remainHp = 0;
		} else {
			foreach (Hero hero in RightHeroes) {
				bri.remainHp += hero.heroAttribute.currentHP;
			}
		}
		bri.BattleName = battleInfo.NAME;
		bri.FieldName = fieldInfo.NAME;
		bri.gold = Coins;
		bri.soul = Soals;
		bri.exp = 3000;
		bri.isWin = isWin;

		BattleDropItem[] dropItems = DataManager.getModule<DataBattle> (DATA_MODULE.Data_Battle).curBattleInfo.getDropitemList ();
		for (int i = 0; i < dropItems.Length; i++) {
			BattleMaterial bm = new BattleMaterial ();
			bm.id = (int)dropItems [i].typeid;
			bm.count = (int)dropItems [i].count;
			bri.materialList.Add (bm);
		}
		
		
		/*for (int i = 0; i < 10; ++i)
        {
            BattleMaterial bm = new BattleMaterial();
            bm.id = 41001;
            bm.count = i;
            bri.materialList.Add(bm);
            if (i < 9)
            {
                bri.partnerList.Add(10007 + i);
            }
        }*/

		bri.partnerList.AddRange (BattleController.SingleTon ().catchIds);

		DataManager.getModule<DataBattle> (DATA_MODULE.Data_Battle).SetBattleResult (bri);
		BaseLib.EventSystem.sendEvent ((int)DataCenter.EVENT_GLOBAL.sys_chgScene, LoadSceneMgr.SCENE_BATTLE);
	}

	public int CurrentTurnAttackers;

	public void OnHeroAttackDone (Hero hero)
	{
		if (HandleTarget != null && HandleTarget == hero) {
			TargetMark.transform.position = HandleTarget.transform.position + HandleTarget.heroRes.CenterOffset;
			TargetMark.SetActive (true);
		}
		hero.heroRes.Shadow.localPosition = Vector3.zero;
	}

	public GameObject prefabBB;
	public GameObject prefabHC;
	public GameObject prefabSoul;
	public GameObject prefabCoin;
	public GameObject prefabUIDamage;
	public GameObject prefabUICrit;
	public GameObject prefabUIMul;

	public GameObject prefabDeathBase;
	public GameObject prefabAfterDeath;
	public GameObject prefabCatch;

	public GameObject prefabToBody;
	public GameObject prefabToBodyGreen;
	public GameObject prefabChest;
	public GameObject prefabChestFinger;
	public GameObject prefabAddBuff;
	public GameObject prefabMaterial;

	public GameObject Effect_HitComman_Fire;
	public GameObject Effect_HitComman_Light;
	public GameObject Effect_HitComman_Magic;
	public GameObject Effect_HitComman_Water;
	public GameObject Effect_HitComman_Wind;
	public GameObject Effect_HitComman_Wood;
	public GameObject prefabDefense;
	public GameObject prefabDefenseHit;

	public void PreparePools ()
	{
		prefabBB = Resources.Load ("Effect/Effect_Blue_D") as GameObject;
		if (prefabBB != null)
			PoolManager.SingleTon ().AddPool (prefabBB, 30);
		prefabHC = Resources.Load ("Effect/Effect_RED_D") as GameObject;
		if (prefabHC != null)
			PoolManager.SingleTon ().AddPool (prefabHC, 30);
		prefabSoul = Resources.Load ("Effect/Effect_Hun") as GameObject;
		if (prefabSoul != null)
			PoolManager.SingleTon ().AddPool (prefabSoul, 30);
		prefabCoin = Resources.Load ("Effect/GoldCoin") as GameObject;
		if (prefabCoin != null)
			PoolManager.SingleTon ().AddPool (prefabCoin, 30);
		prefabUIDamage = Resources.Load ("UIDamage") as GameObject;
		if (prefabUIDamage != null)
			PoolManager.SingleTon ().AddPool (prefabUIDamage, 30, UICamera.transform);
		prefabUICrit = Resources.Load ("Effect/UI_Effect/Critical_Effect") as GameObject;
		if (prefabUICrit != null)
			PoolManager.SingleTon ().AddPool (prefabUICrit, 30);
		prefabUIMul = Resources.Load ("Effect/UI_Effect/Spark_Effect") as GameObject;
		if (prefabUIMul != null)
			PoolManager.SingleTon ().AddPool (prefabUIMul, 30);
		prefabDeathBase = Resources.Load ("Effect/Common_Effect2/Effect_DeathBase") as GameObject;
		if (prefabDeathBase != null)
			PoolManager.SingleTon ().AddPool (prefabDeathBase, 10);
		prefabAfterDeath = Resources.Load ("Effect/Common_Effect2/Effect_AfterDeath") as GameObject;
		if (prefabAfterDeath != null)
			PoolManager.SingleTon ().AddPool (prefabAfterDeath, 10);
		prefabCatch = Resources.Load ("Effect/Common_Effect2/Effect_Catch") as GameObject;
		if (prefabCatch != null)
			PoolManager.SingleTon ().AddPool (prefabCatch, 10);
		prefabToBody = Resources.Load ("Effect/Effect_Shuijing_ToBody") as GameObject;
		if (prefabToBody != null)
			PoolManager.SingleTon ().AddPool (prefabToBody, 30);
		prefabToBodyGreen = Resources.Load ("Effect/Effect_Shuijing_ToBody_Green") as GameObject;
		if (prefabToBodyGreen != null)
			PoolManager.SingleTon ().AddPool (prefabToBodyGreen, 30);
		prefabChest = Resources.Load ("Effect/Common_Effect2/Chest") as GameObject;
		if (prefabChest != null)
			PoolManager.SingleTon ().AddPool (prefabChest, 10);
		prefabChestFinger = Resources.Load ("ChestFinger") as GameObject;
		if (prefabChestFinger != null)
			PoolManager.SingleTon ().AddPool (prefabChestFinger, 10, UICamera.transform);
		prefabAddBuff = Resources.Load ("Effect/Effect_AddBuff") as GameObject;
		if (prefabAddBuff != null)
			PoolManager.SingleTon ().AddPool (prefabAddBuff, 10);
		prefabMaterial = Resources.Load ("Effect/Effect_Material") as GameObject;
		if (prefabMaterial != null)
			PoolManager.SingleTon ().AddPool (prefabMaterial, 10);
		Effect_HitComman_Fire = Resources.Load ("Effect/Effect_HitComman_Fire") as GameObject;
		if (Effect_HitComman_Fire != null)
			PoolManager.SingleTon ().AddPool (Effect_HitComman_Fire, 10);
		Effect_HitComman_Light = Resources.Load ("Effect/Effect_HitComman_Light") as GameObject;
		if (Effect_HitComman_Light != null)
			PoolManager.SingleTon ().AddPool (Effect_HitComman_Light, 10);
		Effect_HitComman_Magic = Resources.Load ("Effect/Effect_HitComman_Magic") as GameObject;
		if (Effect_HitComman_Magic != null)
			PoolManager.SingleTon ().AddPool (Effect_HitComman_Magic, 10);
		Effect_HitComman_Water = Resources.Load ("Effect/Effect_HitComman_Water") as GameObject;
		if (Effect_HitComman_Water != null)
			PoolManager.SingleTon ().AddPool (Effect_HitComman_Water, 10);
		Effect_HitComman_Wind = Resources.Load ("Effect/Effect_HitComman_Wind") as GameObject;
		if (Effect_HitComman_Wind != null)
			PoolManager.SingleTon ().AddPool (Effect_HitComman_Wind, 10);
		Effect_HitComman_Wood = Resources.Load ("Effect/Effect_HitComman_Wood") as GameObject;
		if (Effect_HitComman_Wood != null)
			PoolManager.SingleTon ().AddPool (Effect_HitComman_Wood, 10);
		prefabDefense = Resources.Load ("Effect/Common_Effect2/Effect_GuardShield") as GameObject;
		if (prefabDefense != null)
			PoolManager.SingleTon ().AddPool (prefabDefense, 10);
		prefabDefenseHit = Resources.Load ("Effect/Common_Effect2/Effect_GuardShield_Hitted") as GameObject;
		if (prefabDefenseHit != null)
			PoolManager.SingleTon ().AddPool (prefabDefenseHit, 10);
	}

	public void HideRelationHints ()
	{
		foreach (Hero hero in RightHeroes) {
			hero.Btn.HideHintUp ();
			hero.Btn.HideHintDown ();
		}
	}

	public void ShowSkillHint (Vector3 pos, bool show = true)
	{
		if (SkillHint != null) {
			if (show) {
				SkillHint.transform.position = pos;
				if (!SkillHint.activeInHierarchy)
					SkillHint.SetActive (true);
			} else {
				if (SkillHint.activeInHierarchy)
					SkillHint.SetActive (false);
			}
		}
	}

	public void AddCoin (int num)
	{
		Coins += num;
		CoinText.text = "X " + Coins.ToString ();
		iTween.ShakePosition (CoinText.gameObject, new Vector3 (0.02f, 0, 0), 0.3f);
	}

	public void AddSoul (int num)
	{
		Soals += num;
		SoulText.text = "X " + Soals.ToString ();
		iTween.ShakeScale (SoulText.gameObject, new Vector3 (0.2f, 0, 0), 0.3f);
	}

	Vector3 defaultBattleCameraPos;
	public void ShakeCamera (float dur, float delay)
	{
		StartCoroutine (_ShakeCamera (dur, delay));
	}

	IEnumerator _ShakeCamera (float dur, float delay)
	{
		yield return new WaitForSeconds (delay);
		iTween.ShakePosition (BattleCamera.gameObject, Vector3.one * 0.5f, dur);
		yield return new WaitForSeconds (dur);
		BattleCamera.transform.position = defaultBattleCameraPos;
	}

	void OnDrawGizmos ()
	{
		if (LeftPoints != null) {
			foreach (Transform t in LeftPoints) {
				Gizmos.DrawCube (t.position, Vector3.one * 0.1f);
			}
		}
		if (RightPoints != null) {
			foreach (Transform t in RightPoints) {
				Gizmos.DrawCube (t.position, Vector3.one * 0.1f);
			}
		}
	}

	public List<Hero> GetAlignments (_Side side)
	{
		return side == _Side.Enemy ? LeftHeroes : RightHeroes;
	}

	public List<Hero> GetEnemys (_Side side)
	{
		return side == _Side.Player ? RightHeroes : LeftHeroes;
	}

	public void ShowCatchSprite ()
	{
		if (catchSprite != null) {
			catchSprite.gameObject.SetActive (true);
			UITweener[] tws = catchSprite.GetComponents<UITweener> ();
			foreach (UITweener tw in tws) {
				tw.ResetToBeginning ();
				tw.PlayForward ();
			}
		}
	}

	public void ShowBuffEffect (Hero hero, Color c)
	{
		Debug.Log ("ShowBuffEffect");
		Vector3 pos = BattleUtility.GetCenterPos (hero);
		GameObject go = PoolManager.SingleTon ().Spawn (prefabAddBuff, pos, Quaternion.identity);
		PoolManager.SingleTon ().UnSpawn (2, go);
	}

	public void ShowChestFinger (Chest chest)
	{
		Vector3 pos = BattleUtility.GetNGUIPosFromWorldPos (chest.transform.position);
		GameObject fingerGo = PoolManager.SingleTon ().Spawn (prefabChestFinger, pos, Quaternion.identity);
		fingerGo.transform.localScale = Vector3.one;
		chest.fingerGo = fingerGo;
		UITweener[] tws = fingerGo.GetComponents<UITweener> ();
		foreach (UITweener tw in tws) {
			tw.ResetToBeginning ();
			tw.PlayForward ();
		}
	}

	public void HideChestFinger (Chest chest)
	{
		if (chest.fingerGo != null)
			PoolManager.SingleTon ().UnSpawn (chest.fingerGo);
		chest.fingerGo = null;
	}

	IEnumerator UnSpawnToBody (GameObject go, float t)
	{
		yield return new WaitForSeconds (t);
		go.transform.position = Vector3.one * 100;
	}

	public int getHeroTypeCount ()
	{
		List<_ElementType> elementType = new List<_ElementType> ();
		foreach (Hero hero in RightHeroes) {
			if (elementType.IndexOf (hero.heroAttribute.elementType) == -1)
				elementType.Add (hero.heroAttribute.elementType);			
		}

		return elementType.Count;
		
	}

	public void doSkill ()
	{
		//Debug.Log("............doSkill....................");
		foreach (Hero hero in RightHeroes) {
			SkillManager skillMgr = hero.GetComponent<SkillManager> ();
			skillMgr.doPassivitySkill ();
		}

		foreach (Hero hero in LeftHeroes) {
			SkillManager skillMgr = hero.GetComponent<SkillManager> ();
			skillMgr.doPassivitySkill ();
		}
	}

	public void doBuffs ()
	{
		foreach (Hero hero in RightHeroes) {
			SkillManager skillMgr = hero.GetComponent<SkillManager> ();
			skillMgr.doBuffs ();
		}
		
		foreach (Hero hero in LeftHeroes) {
			SkillManager skillMgr = hero.GetComponent<SkillManager> ();
			skillMgr.doBuffs ();
		}
	}


	public void setSummonBar (int energy)
	{

		if (summoned == false)
			summonSliderValue += 0.8f;
	}


	void FixedUpdate ()
	{

		if (summonSliderValue == 0)
			return;

		if (summonSliderValue <= 1) {
			summonLeftBarSprite.fillAmount = Mathf.Lerp (summonLeftBarSprite.fillAmount, summonSliderValue, Time.deltaTime * 3);
			summonRightBarSprite.fillAmount = Mathf.Lerp (summonRightBarSprite.fillAmount, summonSliderValue, Time.deltaTime * 3);
		} else {
			summonLeftBarSprite.fillAmount = Mathf.Lerp (summonLeftBarSprite.fillAmount, 1.1f, Time.deltaTime * 3);
			summonRightBarSprite.fillAmount = Mathf.Lerp (summonRightBarSprite.fillAmount, 1.1f, Time.deltaTime * 3);
		}

		if (summonSliderValue >= 1) {
			if (summonBarMask.activeSelf != null && summonBarMask.activeSelf) {
				GameObject SkullExplosionPrefab = Resources.Load ("Effect/UI_Effect/SkullExplosion_1") as GameObject;
				if (SkullExplosionPrefab != null) {
					GameObject SkullExplosion = GameObject.Instantiate (SkullExplosionPrefab) as GameObject;
					//SkullExplosion.transform.parent = btnSummon.transform;
					SkullExplosion.transform.position = btnSummon.transform.position;
					SkullExplosion.transform.localScale = new Vector3 (0.07f, 0.08f, 100);
					SkullExplosion.SetActive (true);
				}
				summonBarMask.SetActive (false);

			
				/*GameObject Summon_ReadyPrefab = Resources.Load("Effect/Common_Effect2/Summon_Ready") as GameObject;
				//GameObject Summon_ReadyPrefab = Resources.Load("Effect/Hero_Effect2/BossHumanHunter1/BossHumanHunterBreath") as GameObject;
				GameObject Summon_Ready = Instantiate(Summon_ReadyPrefab) as GameObject;
				Summon_Ready.transform.parent = btnSummon.transform;
				Summon_Ready.transform.localPosition = Vector3.zero;
				Summon_Ready.transform.localScale = new Vector3(0.7f,0.7f,1);
				Summon_Ready.SetActive(true);*/

				summonReady.SetActive (true);


			}

			summonLeftBarSprite.fillAmount = 0;
			summonRightBarSprite.fillAmount = 0;

			int count = Mathf.FloorToInt (summonSliderValue / 1);
			summonSliderValue = summonSliderValue - count;

			summoncount += count; 		
			StartCoroutine (setSummonCountSprite ());

		}
		
	}

	public void onBtnSummonClick (GameObject go)
	{
		bool changeAble = true;
		for (int i = 0; i < RightHeroes.Count; i++) {
			if (RightHeroes [i].gameObject.activeSelf && RightHeroes [i].Status != _HeroStatus.BeforeTurn) {
				changeAble = false;
				break;
			}
		}
		if (changeAble && summoned == false && mIsWaving == false && mIsBeginning == false && summoncount > 0)
 {		//if(changeAble && summoned == false && mIsWaving == false && mIsBeginning == false)
			SpawnManager.SingleTon ().SpawnForSummonMonster ();
		}
	}

	IEnumerator setSummonCountSprite ()
	{
		if (summoncount < 10) {
			summonBarAmountSprite1.spriteName = "FgtNo" + summoncount.ToString ();		
			summonBarAmount1.SetActive (true);
			yield return new WaitForSeconds (1.5f);
			TweenScale ts = summonBarAmount1.GetComponent<TweenScale> ();
			ts.PlayForward ();

			summonBarAmount2.SetActive (false);
			summonBarAmount3.SetActive (false);
		} else {
			summonBarAmount1.SetActive (false);
			summonBarAmount2.SetActive (true);
			summonBarAmount3.SetActive (true);


			TweenScale ts2 = summonBarAmount2.GetComponent<TweenScale> ();
			//ts2.AddOnFinished(setAmountEffect(ts2));
			ts2.PlayForward ();

			TweenScale ts3 = summonBarAmount3.GetComponent<TweenScale> ();
			//ts3.AddOnFinished(setAmountEffect(ts3));
			ts3.PlayForward ();

			int Sprite2 = summoncount / 10;
			summonBarAmountSprite2.spriteName = "FgtNo" + Sprite2.ToString ();

			int Sprite3 = summoncount % 10;
			summonBarAmountSprite3.spriteName = "FgtNo" + Sprite3.ToString ();
		}


	}


	public void setSummonMonster ()
	{
		summoned = true;
		//召唤怪兽
		summonMonsterItemList.SetActive (summoned);
		DismissSummonBtn.SetActive (summoned);
		summonItemCtl.onGetSummonMonsterList ();
		//summonBar.SetActive(!summoned);
		//ControllBtns.SetActive(!summoned);		
	}

	public void onBtnDismissSummonClick (GameObject go)
	{
		StartCoroutine (DismissSummon ());
	}

	IEnumerator DismissSummon ()
	{	
		//判断当前能否退出召唤兽
		bool changeAble = true;
		for (int i = 0; i < RightHeroes.Count; i++) {
			if (RightHeroes [i].gameObject.activeSelf && RightHeroes [i].Status != _HeroStatus.BeforeTurn) {
				changeAble = false;
				break;
			}
		}

		/*foreach(Hero hero in RightHeroes)
		{
			if (hero.isSummonMonster == true && (hero.Status != _HeroStatus.AfterTurn || hero.Status == _HeroStatus.BeforeTurn))
			{
				changeAble = false;
				break;
			}
		}*/



		if (changeAble && summoned && mIsWaving == false && mIsBeginning == false) {

			StartCoroutine (summonItemCtl.ExitSkillEffect ());

			if (battleBgSprite == null) {
				battleBg = UI.PanelStack.me.FindPanel ("BattleUI/Bg");
				battleBgSprite = battleBg.GetComponent<UISprite> ();
				battleBgSprite.spriteName = "FgtBtmBarBg";
			}								

			summoned = false;
			//summonMonsterItemList.SetActive(summoned);
			DismissSummonBtn.SetActive (summoned);


			//ControllBtns.SetActive(!summoned);
			TweenPosition tp = ControllBtns.GetComponent<TweenPosition> ();
			tp.from = new Vector3 (-1, tp.from.y, tp.from.z);
			tp.to = new Vector3 (0, tp.from.y, tp.from.z);
			tp.ResetToBeginning ();
			tp.PlayForward ();

			TweenPosition summontp = summonMonsterItemList.GetComponent<TweenPosition> ();
			summontp.from = new Vector3 (0, summontp.from.y, summontp.from.z);
			summontp.to = new Vector3 (1, summontp.from.y, summontp.from.z);
			summontp.ResetToBeginning ();
			summontp.PlayForward ();
			summontp.AddOnFinished (DestorySummonItem);
			//summonBar.SetActive(!summoned);		

			if (summonPanel == null)
				summonPanel = UI.PanelStack.me.FindPanel ("BattleUI/Bg/summonPanel");

			summonPanel.SetActive (false);
			summonReady.SetActive (true);


			//普通英雄重置
			for (int i = RightHeroes.Count - 1; i >= 0; i--) {
				if (RightHeroes [i].isSummonMonster == false) {
					RightHeroes [i].gameObject.SetActive (true);
					RightHeroes [i].HeroBody.gameObject.SetActive (true);
				}
			}


			//移除召唤兽
			Transform players = GameObject.Find ("Players").transform;
			for (int i = players.childCount - 1; i >= 0; i--) {
				if (players.GetChild (i).GetComponent<Hero> ().isSummonMonster)
					DestroyObject (players.GetChild (i).gameObject);
				
			}

			summonItemCtl.selectedSummon = -1;

			//移除召唤兽场景
			BattleController.SingleTon ().SummonBG.transform.localPosition = new Vector3 (100, 0, 0);	
			StartCoroutine (summonItemCtl.setSummonScreenMask ());			
			yield return new WaitForSeconds (0.5f);



			//移除召唤兽
			for (int i = RightHeroes.Count - 1; i >= 0; i--) {
				if (RightHeroes [i].isSummonMonster == true) {
					RightHeroes.Remove (RightHeroes [i]);
				}
			}

			//召唤兽离场后，状态重置为玩家攻击状态，NPC为已经攻击状态
			CurrentStatus = _AttackTurn.Player;
			mIsEnableGlobalControll = true;
			foreach (Hero hero in LeftHeroes) {
				//hero.Status = _HeroStatus.BeforeTurn;
				hero.Status = _HeroStatus.AfterTurn;
				mIsEnemyTurning = false;
			}

			yield return new WaitForSeconds (0.5f);
			//普通英雄离场或进场特效
			StartCoroutine (PlayersEnterOrExit (true));
			yield return new WaitForSeconds (0.5f);

			//离场特效
			StartCoroutine (summonItemCtl.ExitSkillEffect2 ());
			
		}
	}

	public void ScrollSummonItemList (int offset)
	{
		summonItemCtl.scrollSummonItem (offset);
	}

	public void DestorySummonItem ()
	{
		if (summoned == false) {
			for (int i = summonItemCtl.itemList.Count - 1; i >= 0; i--) {
				Destroy (summonItemCtl.itemList [i]);
			}
		}
	}

	public void CatchMonsterEffect ()
	{
		foreach (GameObject catchEffect in CatchMonsterEffects) {
			if (catchEffect.activeSelf)
				StartCoroutine (catchMonster (catchEffect));
		}
	}

	float mCatchSpeed = 3f;

	IEnumerator catchMonster (GameObject catchEffect)
	{
		float t = 0;
		Vector3 world = BattleController.SingleTon ().UICamera.WorldToScreenPoint (CatchMonsterAmountLbl.transform.position);
		Vector3 lblPos = BattleController.SingleTon ().BattleCamera.ScreenToWorldPoint (world);
		Vector3 startPos = catchEffect.transform.position;
		while (t < 1) {
			t += Time.deltaTime * mCatchSpeed;
			catchEffect.transform.position = Vector3.Lerp (startPos, lblPos, t);
			yield return null;
		}
		CatchMonsterAmount += 1;
		CatchMonsterAmountLbl.text = "X" + CatchMonsterAmount.ToString ();
		//显示捕获
		ShowCatchSprite ();
		catchEffect.SetActive (false);
		CatchMonsterEffects.Remove (catchEffect);
	}


	public IEnumerator PlayersEnterOrExit (bool isEnter)
	{
		foreach (Hero hero in RightHeroes) {
			if (hero.isSummonMonster == false) {
				StartCoroutine (PlayersEnterOrExitEffect (hero, isEnter));
				yield return new WaitForSeconds (0.3f);
			}
		}
	}

	IEnumerator PlayersEnterOrExitEffect (Hero hero, bool isEnter)
	{
		PoolManager.SingleTon ().Spawn (HeroEnterEffect, RightPoints [hero.Index].position, Quaternion.identity, 0.1f, 3);
		yield return new WaitForSeconds (0.3f);
		if (isEnter)
			hero.transform.position = RightPoints [hero.Index].position;
		else
			hero.transform.position = new Vector3 (RightPoints [hero.Index].position.x, RightPoints [hero.Index].position.y + 20, RightPoints [hero.Index].position.z);
		iTween.ShakePosition (BattleCamera.gameObject, Vector3.one * 0.2f, 0.1f);
		//yield return new WaitForSeconds(0.5f);
		hero.Status = _HeroStatus.BeforeTurn;
		hero.heroAnimation.Play ("StandBy");
	}
	

}







