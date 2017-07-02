using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using StatusMachines;

public enum _BattleMachineStatus
{
	//开始攻击
	Begin,
	//等待
	Waiting,
	//战斗开始前前演出
	Perform,
	//己方攻击
	PlayerAttack,
	//切换到地方攻击
	ToggleToEnemy,
	//切换到己方攻击
	ToggleToPlayer,
	//敌方攻击
	EnemyAttack,
	//攻击
	Attack,
	//攻击结束，进入下一回合之前
	AttackDone,
	Failure,
	Success}
;

//战斗
public class BattleManager : SingleMonoBehaviour<BattleManager>
{
	#region 1.战斗场景相关
	public List<Transform> leftPoints;
	public List<Transform> rightPoints;
	public List<Transform> topPoints;
	public List<Transform> centerPoints;
	public Transform rightPosT;
	public Transform leftPosT;
	public Transform rightSkillMoveTarget;
	public Transform leftSkillMoveTarget;
	public Camera battleCamera;
	public Camera uiCamera;

//	public UISprite GlobalMask;
	#endregion

	public StatusMachine sm;
	public List<Unit> rightUnits;
	public List<Unit> leftUnits;
	public List<Unit> allUnits;

	[System.NonSerialized]
	public bool hasPerformSKill = true;
	[System.NonSerialized]
	public bool isPerformed = false;

	public int quickBattleTimeScale = 1;
	public int currentEnergy = 0;
	float maxEnergy = 100f;

	protected override void Awake ()
	{
		if (AudioManager.SingleTon () != null)
			AudioManager.SingleTon ().PlayMusic (AudioManager.SingleTon ().MusicBattleClip);
		Application.targetFrameRate = 60;
		Screen.SetResolution (1136, 640, false);
	}

	void Start(){
		Init ();
		quickBattleTimeScale = 1;
		StartCoroutine (_Start ());
	}

	void Init ()
	{
		sm = gameObject.AddComponent<StatusMachine> ();
		sm.AddAction (_BattleMachineStatus.Begin.ToString (), new BattleBeginAction ());
		sm.AddAction (_BattleMachineStatus.Perform.ToString (), new BattlePerformAction ());
//		sm.AddAction (_BattleMachineStatus.PlayerAttack.ToString (), new BattlePlayerAttackAction ());
//		sm.AddAction (_BattleMachineStatus.ToggleToEnemy.ToString (), new BattleToggleToEnemyAction ());
//		sm.AddAction (_BattleMachineStatus.ToggleToPlayer.ToString (), new BattleToggleToPlayerAction ());
//		sm.AddAction (_BattleMachineStatus.EnemyAttack.ToString (), new BattleEnemyAttackAction ());
		sm.AddAction (_BattleMachineStatus.Waiting.ToString (), new BattleWaitingAction ());
		sm.AddAction (_BattleMachineStatus.Failure.ToString (), new BattleFailureAction ());
		sm.AddAction (_BattleMachineStatus.Success.ToString (), new BattleSuccessAction ());
		sm.AddAction (_BattleMachineStatus.Attack.ToString (), new BattleAttackAction ());
		sm.AddAction (_BattleMachineStatus.AttackDone.ToString (), new BattleAttackDoneAction ());


	}

	IEnumerator _Start ()
	{
		yield return new WaitForSeconds (0.1f);
		sm.ChangeStatus (_BattleMachineStatus.Begin.ToString ());
	}

	public void AddEnergy ()
	{
		currentEnergy += 5;
		BattleUIManager.GetInstance ().SetEnergy (currentEnergy / maxEnergy);
	}

	public void PlayTeamSkill ()
	{
		currentEnergy = 0;
		PerformManager.GetInstance ().PlayTeamSkillEffect ();
	}

	public List<Unit> GetRandomAlivePlayerUnitsWithTargetUnit (Unit selfUnit, int num)
	{
		num--;
		List<Unit> aliveUnits = GetAlivePlayerUnits ();
		aliveUnits.Remove (selfUnit);
		List<Unit> randomUnits = GetRandomAliveUnits (aliveUnits, num);
		randomUnits.Add (selfUnit);
		return randomUnits;
	}

	public List<Unit> GetRandomAliveEnemyUnitsWithTargetUnit (Unit selfUnit, int num)
	{
		num--;
		List<Unit> aliveUnits = GetAliveEnemyUnits ();
		aliveUnits.Remove (selfUnit);
		List<Unit> randomUnits = GetRandomAliveUnits (aliveUnits, num);
		randomUnits.Add (selfUnit);
		return randomUnits;
	}

	public List<Unit> GetRandomAliveAllUnits (int num)
	{
		List<Unit> aliveUnits = GetAliveEnemyUnits ();
		List<Unit> aliveUnits1 = GetAlivePlayerUnits ();
		aliveUnits.AddRange (aliveUnits1);
		return GetRandomAliveUnits (aliveUnits, num);
	}

	public List<Unit> GetRandomAliveEnemyUnits (int num)
	{
		List<Unit> aliveUnits = GetAliveEnemyUnits ();
		return GetRandomAliveUnits (aliveUnits, num);
	}

	public List<Unit> GetRandomAlivePlayerUnits (int num)
	{
		List<Unit> aliveUnits = GetAlivePlayerUnits ();
		return GetRandomAliveUnits (aliveUnits, num);
	}

	List<Unit> GetRandomAliveUnits (List<Unit> aliveUnits, int num)
	{
		List<Unit> units = new List<Unit> ();
		int curNum = 0;
		while (aliveUnits.Count > 0 && curNum < num) {
			Unit unit = aliveUnits [Random.Range (0, aliveUnits.Count)];
			aliveUnits.Remove (unit);
			units.Add (unit);
			curNum++;
		}
		return units;
	}

	List<Unit> GetAliveEnemyUnits ()
	{
		return GetAliveUnits (leftUnits);
	}

	List<Unit> GetAlivePlayerUnits ()
	{
		return GetAliveUnits (rightUnits);
	}

	List<Unit> GetAliveUnits (List<Unit> units)
	{
		List<Unit> aliveUnits = new List<Unit> ();
		for (int i = 0; i < units.Count; i++) {
			if (units [i].GetAttribute(EffectType.HP) > 0) {
				aliveUnits.Add (units [i]);
			}
		}
		return aliveUnits;
	}

	void InitGlobalSkill ()
	{
//		BaseSkill baseSkill = UnitSkill.InitBaseSkill
	}

	void OnGUI(){
		Time.timeScale = GUI.HorizontalSlider (new Rect (Screen.width - 210, 10, 200, 20), Time.timeScale, 0, 1);
		GUI.color = Color.red;
		GUI.Label (new Rect (Screen.width - 210, 35, 200, 20), Time.timeScale.ToString ());
		GUI.color = Color.white;
	}

}





