using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultAttackPerform : BasePerform {

	const string realDamageShowed = "RealDamageShowed";

	public override void OnEnter ()
	{
		base.OnEnter ();
		OnDefaultAttackEnter ();
	}

	protected virtual void OnDefaultAttackEnter(){
		unit.Play (_UnitArtActionType.atk_0101,baseSkill.partments[0].partmentTargets);
	}

	//30FPS情况下1帧0.033s。相当于3帧
	const float attckQueueInterval = 0.033f;
	//默认数字震动范围
	const float shakeRadius = 0.1f;
	public override void OnPartmentByCalculation (SkillEffectPartment partment, Unit target, Hashtable parameters, int index = 0)
	{
		base.OnPartmentByCalculation (partment, target, parameters, index);
		CoroutineManager.GetInstance ().StartCoroutine (_HitPartment(partment,target,parameters,index,null));
	}

	IEnumerator _HitPartment(SkillEffectPartment partment,Unit attackTarget,Hashtable parameters,int index,AudioClip audioClip){
		int damage = (int)parameters [CalculationResultType.RealDamage];
		bool isCrit = (bool)parameters [CalculationResultType.IsCrit];
		bool isDefence = (bool)parameters[CalculationResultType.IsDefence];
		bool isDodge = (bool)parameters[CalculationResultType.IsDodge];
		bool isRelation = (bool)parameters[CalculationResultType.IsRelation];
		int health = (int)parameters [CalculationResultType.RealHealth];
		if (!parameters.ContainsKey (realDamageShowed)) {
			parameters.Add (realDamageShowed,0);
		}
		int mCurrentDamaged = (int)parameters [realDamageShowed];

		int attackCount = partment.partCount;
		if (isDodge) {
			if (index == 0)
				PerformManager.GetInstance ().ShowDodge (attackTarget);
		} else {
			if (isDefence) {
				if (index == 0)
					PerformManager.GetInstance ().ShowDefence (attackTarget);
			}
			int partmentDamage = Mathf.Max(1,Mathf.RoundToInt (partment.effectValue * damage));
			partmentDamage = Mathf.Min (partmentDamage,damage - mCurrentDamaged);
			int partmentHealth = Mathf.Max(1,Mathf.RoundToInt (partment.effectValue * health));
			if (partmentDamage > 0) {
				parameters [realDamageShowed] = (int)parameters [realDamageShowed] + partmentDamage;
			}
			int textCount = Mathf.Min (attackCount, partmentDamage);
			if(textCount==0 ){
				Debug.Log ("textCount:" + textCount + "||" + "partmentDamage:" + partmentDamage + "||" );
			}
			Vector3 hitPos = attackTarget.unitRes.GetCenterPos ();// .transform.position;
//			BattleUtility.Drop (unit.hero.currentAttackAttribute, unit.hero, attackTarget.hero, index);
			if (attackTarget.side == _Side.Enemy) {
				if (index == 0)
					DropManager.GetInstance ().Drop (attackTarget, _DropType.Soul, 1);
			}
			Vector3 posOffset = new Vector3 (0.01f, 0, 0);
			partmentDamage = partmentDamage / attackCount;
			partmentHealth = partmentHealth / attackCount;
			for (int i = 0; i < attackCount; i++) {
				iTween.ShakePosition (attackTarget.gameObject, new Vector3 (0.01f, 0.5f, 0), 0.1f);
//				attackTarget.Play (UnitAnimationClipName.hit.ToString());
				attackTarget.sm.ChangeStatus (_UnitMachineStatus.Hit.ToString());
				if (partmentDamage > 0 && textCount > i) {
					if (isCrit && isRelation)
						PerformManager.GetInstance().ShowCritAndRelationDamageBeat (shakeRadius, partmentDamage, hitPos + posOffset * i, isRelation ? 1.5f : 1);
					else if (isCrit)
						PerformManager.GetInstance().ShowCritDamageBeat (shakeRadius, partmentDamage, hitPos + posOffset * i, isRelation ? 1.5f : 1);
					else if (isRelation)
						PerformManager.GetInstance().ShowRelationDamageBeat (shakeRadius, partmentDamage, hitPos + posOffset * i, isRelation ? 1.5f : 1);
					else
						PerformManager.GetInstance().ShowDamageBeat (shakeRadius, partmentDamage, hitPos + posOffset * i, isRelation ? 1.5f : 1);
				}
				if (partmentHealth > 0) {
					PerformManager.GetInstance().ShowHealthBeat (shakeRadius, partmentHealth, unit.transform.position + posOffset * i, 1.5f);
				}

				PerformManager.GetInstance().ShowElementHitEffect (unit, hitPos + new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0));

				AudioManager.SingleTon ().PlayHitClip (AudioManager.SingleTon ().hitElement3);
				yield return new WaitForSeconds (attckQueueInterval);
			}
		}
	}
}
