using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MMO
{
	public class ActionManager : SingleMonoBehaviour<ActionManager>
	{
		//1:unit skill,2:start auto,3:end auto(must),4,change status(actionId:1=idle,2=move,3=run,4=death,演出するしかない)。
		public void DoAction (StatusInfo action)
		{
			MMOUnit unit = MMOController.Instance.GetUnitByUnitId (action.casterId);
			if (unit == null) {
				Debug.Log ("unit is null.");
				return;
			}
			switch (action.status) {
			case BattleConst.UnitMachineStatus.STANDBY:
				unit.unitAnimator.Play (AnimationConstant.UNIT_ANIMATION_CLIP_IDEL);
				unit.unitAnimator.SetSpeed (1);
				unit.unitAnimator.SetMoveSpeed (0);
//				if(unit.mSimpleRpgAnimator!=null)
//					unit.mSimpleRpgAnimator.SetMoveSpeed (0);
				break;
			case BattleConst.UnitMachineStatus.MOVE:
				unit.unitAnimator.SetTrigger (AnimationConstant.UNIT_ANIMATION_CLIP_RUN);
//				unit.SetAnimation (AnimationConstant.UNIT_ANIMATION_CLIP_RUN,1);
//				if(unit.mSimpleRpgAnimator!=null)
				unit.unitAnimator.SetMoveSpeed (3.0f);
				break;
			case BattleConst.UnitMachineStatus.CAST:
				DoSkill(action);
				break;
			case BattleConst.UnitMachineStatus.DEATH:
				unit.Death ();
				//TODO change to parameter 
				unit.unitAnimator.Play (AnimationConstant.UNIT_ANIMATION_CLIP_DEAD);
				unit.unitAnimator.SetSpeed (1);
				break;
			case BattleConst.UnitMachineStatus.RESPAWN:
				PerformManager.Instance.ShowRespawnEffect (unit.transform.position);
				break;
			default:
				unit.unitAnimator.Play (AnimationConstant.UNIT_ANIMATION_CLIP_IDEL);
				unit.unitAnimator.SetSpeed (1);
				break;
			}
		}
		//Do Skill.
		public void DoSkill (StatusInfo action)
		{
			Debug.Log ("DoSkill");
			//now the action.actionId means the skill id.
			StartCoroutine (_Cast(action));
		}


		public void DoShoot(ShootInfo shootInfo){
			MUnitSkill mUnitSkill = CSVManager.Instance.unitSkillDic[shootInfo.unitSkillId];
			MSkill mSkill = CSVManager.Instance.skillDic [mUnitSkill.skill_id];
			MMOUnit caster = MMOController.Instance.GetUnitByUnitId (shootInfo.casterId);
			MMOUnit target = null;
			if (shootInfo.targetId >= 0) {
				target = MMOController.Instance.GetUnitByUnitId (shootInfo.targetId);
			}
			GameObject effect = ResourcesManager.Instance.GetEffect (mUnitSkill.shoot_object_id);
			Shoot (effect, mSkill,caster,target);
		}

		//TODO use trigger to controll the cast clip;
		//TODO the end time point need to same as the animclip end time point.
		//TODO need to get the real skill information from csv.
		IEnumerator _Cast(StatusInfo action){
//			MSkill mSkill = CSVManager.Instance.skillDic [action.actionId];
			MUnitSkill mUnitSkill = CSVManager.Instance.unitSkillDic[action.actionId];
//			MSkill mSkill = CSVManager.Instance.skillDic [mUnitSkill.skill_id];
			MMOUnit caster = MMOController.Instance.GetUnitByUnitId (action.casterId);
			//TODO need know the cast anim name from csv or from other area.
			caster.unitAnimator.SetTrigger(mUnitSkill.anim_name);
//			MMOUnit target = null;
//			if (action.targetId >= 0) {
//				target = MMOController.Instance.GetUnitByUnitId (action.targetId);
//			}
//			yield return new WaitForSeconds ((mUnitSkill.anim_action_point / 100f) * mUnitSkill.anim_length);
//			if (mSkill.is_remote > 0) {
//				//Remote attack;
//				GameObject effect = ResourcesManager.Instance.GetEffect (mUnitSkill.shoot_object_id);
//				Shoot (effect, mSkill,caster,target,mSkill.range);
//			}
			yield return null;
		}
		//Shoot Action.
		public void Shoot(GameObject shootPrefab,MSkill mSkill,MMOUnit caster,MMOUnit target){
			Vector3 spawnPos;
			UnitPerform unitPerform = caster.GetComponent<UnitPerform> ();
			if (unitPerform!=null && unitPerform.shootPoint != null) {
				spawnPos = caster.GetComponent<UnitPerform> ().shootPoint.position;
			} else {
				spawnPos = caster.GetBodyPos ();
			}
			GameObject shootGo = Instantiater.Spawn (false, shootPrefab, spawnPos, caster.transform.rotation * Quaternion.Euler (60, 0, 0));
			ShootObject shootObj = null;
			switch(mSkill.shoot_move_type){
			case 1:
				shootObj = shootGo.GetOrAddComponent<ShootProjectileObject> ();
				break;
			default:
				shootObj = shootGo.GetOrAddComponent<ShootLineObject> ();
				break;
			}
			shootObj.speed = mSkill.shoot_move_speed;
			if (target != null) {
				shootObj.Shoot (caster, target, new Vector3 (0, target.GetBodyHeight () / 2f, 0));
			} else {
				Vector3 targetPos = MMOController.Instance.GetTerrainPos (caster.transform.position + caster.transform.forward * mSkill.range);
				shootObj.Shoot (caster,targetPos,Vector3.zero);
			}
		}

	}
}
