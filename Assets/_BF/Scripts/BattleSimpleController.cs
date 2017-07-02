using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class BattleSimpleController : MonoBehaviour {

	public List<Transform> LeftPoints;
	public List<Transform> RightPoints;
	public List<Transform> CenterPoints;
	public Transform rightPosT;
	public Transform leftPosT;

	public List<Hero> LeftHeroes;
	public List<Hero> RightHeroes;
	public List<HeroBtn> LeftHeroBtns;
	public List<HeroBtn> RightHeroBtns;

	public List<Hero> DeadHeroes;
	public _AttackTurn Turn = _AttackTurn.Begin;
	public Camera BattleCamera;
	public Camera UICamera;
	public List<Drop> Drops;
	public GameObject BattleWinEffect;
	public GameObject BattleLoseEffect;
	public GameObject HeroEnterEffect;
	public GameObject ScreenBlack;
	public float ShakeRadius = 0.1f;
	public Vector3 BtnShakeOffset = new Vector3(0.03f,0.01f,0);
	public GameObject CurrScene;
	static BattleSimpleController instance;
	public static BattleSimpleController SingleTon(){
		return instance;
	}

	void Awake()
	{
		if(Chest.chests!=null)
			Chest.chests.Clear();
		Screen.SetResolution(640, 960, false);
		if (instance == null)
			instance = this;
//		spawnManager = SpawnManager.SingleTon ();
		Potion.Clear();
		HeroBtn.Clear();
		if(AudioManager.SingleTon()!=null)
			AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicBattleClip);
	}

	void Start()
	{
		Init();
	}


	bool mIsDownloadDone;
	void Update(){

		switch(Turn)
		{
			case _AttackTurn.Begin:OnBegin();break;
			case _AttackTurn.Player:OnPlayerTurn();break;
			case _AttackTurn.PlayerToEnmey:OnPlayerToEnmey();break;
			case _AttackTurn.Enemy:OnEnemyTurn();break;
			case _AttackTurn.EnemyToPlayer:OnEnemyToPlayer();break;
			case _AttackTurn.Failure:OnFailure();break;
			case _AttackTurn.Success:OnSuccess();break;
			default:break;
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
			//Application.LoadLevel(0);
			BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene, LoadSceneMgr.SCENE_LOGIN);
		}
		if(Input.GetKeyDown(KeyCode.H))
		{
			#if UNITY_EDITOR
			Application.LoadLevel("Arean");
			#else
			BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene, LoadSceneMgr.SCENE_MAIN);
			#endif
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public GameObject prefabUIDamage;
	public GameObject prefabUICrit;
	public GameObject prefabUIMul;
	public GameObject prefabBB;
	public GameObject prefabHC;
	public GameObject prefabToBody;
	public GameObject prefabToBodyGreen;

	public void PreparePools()
	{
		prefabBB = Resources.Load("Effect/Effect_Blue_D") as GameObject;
		if(prefabBB!=null)PoolManager.SingleTon().AddPool(prefabBB,30);
		prefabHC = Resources.Load("Effect/Effect_RED_D") as GameObject;
		if(prefabHC!=null)PoolManager.SingleTon().AddPool(prefabHC,30);
		prefabUIDamage = Resources.Load("UIDamage") as GameObject;
		if(prefabUIDamage!=null)PoolManager.SingleTon().AddPool(prefabUIDamage,30,UICamera.transform);
		prefabUICrit = Resources.Load("Effect/Effect_baoji") as GameObject;
		if(prefabUICrit!=null)PoolManager.SingleTon().AddPool(prefabUICrit,30,UICamera.transform);
		prefabUIMul = Resources.Load("Effect/Effect_Heji") as GameObject;
		if(prefabUIMul!=null)PoolManager.SingleTon().AddPool(prefabUIMul,30,UICamera.transform);
		prefabToBody = Resources.Load("Effect/Effect_Shuijing_ToBody") as GameObject;
		if(prefabToBody!=null)PoolManager.SingleTon().AddPool(prefabToBody,30);
		prefabToBodyGreen = Resources.Load("Effect/Effect_Shuijing_ToBody_Green") as GameObject;
		if(prefabToBodyGreen!=null)PoolManager.SingleTon().AddPool(prefabToBodyGreen,30);
	}

	void Init()
	{
		PreparePools();
		RightHeroes = SpawnManager.SingleTon().playerHeros;
		BattleUtility.ActiveAreanHeros(RightHeroes,RightHeroBtns,RightPoints);
		LeftHeroes = SpawnManager.SingleTon().enemyHeros;
		BattleUtility.ActiveAreanHeros(LeftHeroes,LeftHeroBtns,LeftPoints);
		Turn = _AttackTurn.Begin;
	}

#region OnBegin
	public bool mIsBeginning;
	void OnBegin()
	{
		if(!mIsBeginning)
		{
			mIsBeginning = true;
			StartCoroutine(_Begin());
		}
	}
	IEnumerator _Begin()
	{
		yield return new WaitForSeconds(3);
		Turn = _AttackTurn.Player;
		mIsBeginning =false;
	}
#endregion

#region OnPlayerTurn
	public bool mIsPlayerTurning;
	void OnPlayerTurn()
	{
		if(!mIsPlayerTurning)
		{
			mIsPlayerTurning = true;
			StartCoroutine(_PlayerTurn());
		}
	}
	IEnumerator _PlayerTurn()
	{
		Debug.Log("_PlayerTurn");
		yield return new WaitForSeconds(1);
		foreach(Hero hero in RightHeroes)
		{
			if(hero.Status==_HeroStatus.BeforeTurn)
			{
				if(hero.heroAttribute.currentEnergy == hero.heroAttribute.maxEnergy)
				{
					hero.heroAttack.Attack(1);
				}
				else
				{
					hero.heroAttack.Attack();
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
		bool changeAble =false;
		while(!changeAble)
		{
			changeAble = true;
			for(int i = 0;i < RightHeroes.Count;i++)
			{
				if(RightHeroes[i].Status != _HeroStatus.AfterTurn)
				{
					changeAble = false;
					break;
				}
			}
			yield return null;
		}
		mIsPlayerTurning = false;
		Turn = _AttackTurn.PlayerToEnmey;
	}
#endregion

#region OnPlayerToEnmey
	public bool mIsPlayerToEnmeyTurning;
	void OnPlayerToEnmey()
	{
		if(!mIsPlayerToEnmeyTurning)
		{
			mIsPlayerToEnmeyTurning = true;
			StartCoroutine(_PlayerToEnemy());
		}
	}
	IEnumerator _PlayerToEnemy()
	{
		yield return new WaitForSeconds(2);
		bool collectAble = false;
		while(!collectAble)
		{
			foreach(Drop drop in Drops)
			{
				if(drop.isFalling)
				{
					collectAble = false;
					break;
				}
				collectAble = true;
			}
//			Debug.Log("collectAble 0");
			yield return null;
		}
		DropUtility.Collect(Drops,RightHeroes);
		while(Drops.Count>0)
		{
			//			Debug.Log("Collect");
//			Debug.Log("collectAble 4");
			yield return null;
		}
		yield return null;
		foreach(Hero hero in RightHeroes)
		{
			hero.Status = _HeroStatus.BeforeTurn;
		}
		mIsPlayerToEnmeyTurning = false;
		Turn = _AttackTurn.Enemy;
	}
#endregion

#region OnEnemyTurn
	public bool mIsEnemyTurning;
	void OnEnemyTurn()
	{
		if(!mIsEnemyTurning)
		{
			mIsEnemyTurning = true;
			StartCoroutine(_EnemyTurn());
		}
	}
	IEnumerator _EnemyTurn()
	{
		yield return new WaitForSeconds(1);
		foreach(Hero hero in LeftHeroes)
		{
			if(hero.Status==_HeroStatus.BeforeTurn)
			{
				if(hero.heroAttribute.currentEnergy == hero.heroAttribute.maxEnergy)
				{
					hero.heroAttack.Attack(1);
				}
				else
				{
					hero.heroAttack.Attack();
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
		bool changeAble =false;
		while(!changeAble)
		{
			changeAble = true;
			for(int i = 0;i < LeftHeroes.Count;i++)
			{
				if(LeftHeroes[i].Status != _HeroStatus.AfterTurn)
				{
					changeAble = false;
					break;
				}
			}
			yield return null;
		}
		mIsEnemyTurning =false;
		Turn = _AttackTurn.EnemyToPlayer;
	}
#endregion

#region
	public bool mIsEnemyToPlayerTurning;

	void OnEnemyToPlayer()
	{
		if(!mIsEnemyToPlayerTurning)
		{
			mIsEnemyToPlayerTurning = true;
			StartCoroutine(_EnemyToPlayer());
		}
	}

	IEnumerator _EnemyToPlayer()
	{
		yield return new WaitForSeconds(2);
		bool collectAble = false;
		while(!collectAble)
		{
			foreach(Drop drop in Drops)
			{
				if(drop.isFalling)
				{
					collectAble = false;
					break;
				}
				collectAble = true;
			}
			if(Drops.Count == 0)
			{
				collectAble = true;
			}
			Debug.Log("collectAble 0");
			yield return null;
		}
		DropUtility.Collect(Drops,LeftHeroes);
		while(Drops.Count>0)
		{
			Debug.Log("collectAble 4");
			yield return null;
		}
		yield return null;
		foreach(Hero hero in LeftHeroes)
		{
			hero.Status = _HeroStatus.BeforeTurn;
		}
		mIsEnemyToPlayerTurning = false;
		Turn = _AttackTurn.Player;
	}
#endregion

	bool mIsFailure;
	bool mIsSuccess;

	void OnFailure()
	{

	}

	void OnSuccess()
	{

	}


}







