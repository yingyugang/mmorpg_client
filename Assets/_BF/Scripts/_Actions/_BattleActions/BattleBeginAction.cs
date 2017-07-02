using StatusMachines;
using System.Collections.Generic;
using UnityEngine;
using DataCenter;

public class BattleBeginAction : BattleBaseAction
{

	public override void OnEnter ()
	{
		base.OnEnter ();
		mBattleManager.allUnits = new List<Unit> ();
		mBattleManager.rightUnits = new List<Unit> ();
		mBattleManager.leftUnits = new List<Unit> ();
		List<GameObject> rightUnits = BattleSpawnManager.GetInstance ().rightUnits;
		List<GameObject> leftUnits = BattleSpawnManager.GetInstance ().leftUnits;
		for (int i=0;i < BattleSpawnManager.GetInstance ().rightUnits.Count;i++) {
			rightUnits[i].gameObject.SetActive (true);
			rightUnits[i].transform.position = BattleManager.GetInstance ().rightPoints [i].position;
			Unit unit = rightUnits [i].GetComponent<Unit> ();
			unit.defaultPos = rightUnits[i].transform.position;
			unit.side = _Side.Player;
			mBattleManager.rightUnits.Add (unit);
			mBattleManager.allUnits.Add (unit);
		}
		for (int i=0;i<leftUnits.Count;i++) {
			leftUnits[i].gameObject.SetActive (true);
			leftUnits[i].transform.position = BattleManager.GetInstance ().leftPoints [i].transform.position;
			leftUnits [i].transform.localEulerAngles = new Vector3 (0,180,0);
			Unit unit = leftUnits [i].GetComponent<Unit> ();
			unit.side = _Side.Enemy;
			unit.defaultPos = leftUnits[i].transform.position;
			mBattleManager.leftUnits.Add (unit);
			mBattleManager.allUnits.Add (unit);
		}
		for (int i = 0; i < mBattleManager.allUnits.Count; i++) {
			mBattleManager.allUnits [i].Play (_UnitArtActionType.cmn_0001);
			mBattleManager.allUnits [i].unitSkill.InitUnitSkills ();
		}
		//AI强制使用优先度为-1的技能，如果没有则跳过此轮。
		for (int i = 0; i < mBattleManager.allUnits.Count; i++) {
			List<BaseSkill> handledSkills = new List<BaseSkill> ();
			foreach (BaseSkill baseSkill in mBattleManager.allUnits [i].activeSkills) {
				if (baseSkill.castPriority == -1) {
					handledSkills.Add (baseSkill);
				}
			}
			mBattleManager.allUnits [i].unitSkill.handledSkills = handledSkills;
		}
		BattleUIManager.GetInstance ().btn_battle_start.gameObject.SetActive (true);
		if (AudioManager.SingleTon () != null)
			AudioManager.SingleTon ().PlayMusic (AudioManager.SingleTon ().MusicBattleClip);
	}

	//显示技能效果
	public override void OnUpdate ()
	{
		base.OnUpdate ();
	}
}