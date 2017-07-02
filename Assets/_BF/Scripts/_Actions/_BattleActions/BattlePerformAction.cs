//因为是多个单位技能加成的总和，所以不能基于unit，而是基于Battle。
using StatusMachines;
using System.Collections.Generic;
using UnityEngine;


public class BattlePerformAction:BattleBaseAction
{

	public List<EffectType> effectTypes;

	public override void OnAwake ()
	{
		base.OnAwake ();
		effectTypes = new List<EffectType> ();
		effectTypes.Add (EffectType.CritOdds);
		effectTypes.Add (EffectType.DodgeOdds);
		effectTypes.Add (EffectType.SkillOdds);
		effectTypes.Add (EffectType.DamageMuilt);
		effectTypes.Add (EffectType.DefenceMuilt);
		effectTypes.Add (EffectType.ElementMuilt);
		effectTypes.Add (EffectType.HP);
		effectTypes.Add (EffectType.MaxHP);
	}

	public override void OnEnter ()
	{
		base.OnEnter ();
		mCurrentIndex = 0;
		mNextPerformTime = Time.time + mPerformInterval;
		mBattleManager.isPerformed = true;
	}

	const float mPerformInterval = 0.8f;
	float mNextPerformTime;
	int mCurrentIndex = 0;

	//显示技能效果
	public override void OnUpdate ()
	{
		if (mNextPerformTime < Time.time) {
			if (effectTypes.Count <= mCurrentIndex) {
				statusMachine.ChangeStatus (_BattleMachineStatus.Attack.ToString ());
				return;
			}
			EffectType effectType = effectTypes [mCurrentIndex];
			bool isShow = false;
			for (int i = 0; i < mBattleManager.rightUnits.Count; i++) {
				Unit unit = mBattleManager.rightUnits [i];
				Color color = Color.white;
				switch (effectType) {
				case EffectType.DamageMuilt:
					color = Color.red;
					break;
				case EffectType.DefenceMuilt:
					color = Color.blue;
					break;
				case EffectType.CritOdds:
					color = Color.yellow;
					break;
				}
				string str = effectType.ToString () + " " + (unit.additionUnitAttribute.attributes [effectType] / 100) + "%";
				if (unit.additionUnitAttribute.attributes [effectType] > 0) {
					PerformManager.GetInstance ().ShowAttributeEnhance (unit, str, color);
					isShow = true;
				}
			}
			mCurrentIndex++;
			if (isShow)
				mNextPerformTime = Time.time + mPerformInterval;
//			Debug.Log ("isShow:" + isShow);
		}
		base.OnUpdate ();
	}
}