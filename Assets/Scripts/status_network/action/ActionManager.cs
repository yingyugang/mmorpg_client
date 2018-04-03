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
				unit.SetAnimation (AnimationConstant.UNIT_ANIMATION_CLIP_IDEL, 1);
				if(unit.mSimpleRpgAnimator!=null)
					unit.mSimpleRpgAnimator.SetMoveSpeed (0);
				break;
			case BattleConst.UnitMachineStatus.MOVE:
				unit.SetTrigger (AnimationConstant.UNIT_ANIMATION_CLIP_RUN);
				if(unit.mSimpleRpgAnimator!=null)
					unit.mSimpleRpgAnimator.SetMoveSpeed (3.0f);
				break;
			case BattleConst.UnitMachineStatus.CAST:
				DoSkill(action);
				break;
			case BattleConst.UnitMachineStatus.DEATH:
				unit.Death ();
				unit.SetAnimation (AnimationConstant.UNIT_ANIMATION_CLIP_DEAD,1);
				break;
			default:
				unit.SetAnimation (AnimationConstant.UNIT_ANIMATION_CLIP_IDEL,1);
				break;
			}
		}
		//Do Skill.
		public void DoSkill (StatusInfo action)
		{
			//now the action.actionId means the skill id.
			StartCoroutine (_Cast(action));
		}
		//Do Unit Actions.
//		void DoUnitAction(StatusInfo action){
//			MMOUnit unit = MMOController.Instance.GetUnitByUnitId (action.casterId);
//			switch(action.actionId){
//			case 1:
//				unit.SetAnimation (AnimationConstant.UNIT_ANIMATION_CLIP_IDEL,1);
//				break;
//			case 2:
//				unit.SetAnimation (AnimationConstant.UNIT_ANIMATION_CLIP_WALK,1);
//				break;
//			case 3:
//				unit.SetAnimation (AnimationConstant.UNIT_ANIMATION_CLIP_RUN,1);
//				break;
//			case 4:
//				unit.Death ();
//				unit.SetAnimation (AnimationConstant.UNIT_ANIMATION_CLIP_DEAD,1);
//				break;
//			default:
//				unit.SetAnimation (AnimationConstant.UNIT_ANIMATION_CLIP_IDEL,1);
//				break;
//			}
//		}
		//TODO use trigger to controll the cast clip;
		//TODO the end time point need to same as the animclip end time point.
		//TODO need to get the real skill information from csv.
		IEnumerator _Cast(StatusInfo action){
//			MSkill mSkill = CSVManager.Instance.skillDic [action.actionId];
			MUnitSkill mUnitSkill = CSVManager.Instance.unitSkillDic[action.actionId];
			MSkill mSkill = CSVManager.Instance.skillDic [mUnitSkill.skill_id];
			MMOUnit caster = MMOController.Instance.GetUnitByUnitId (action.casterId);
			//TODO need know the cast anim name from csv or from other area.
			caster.SetTrigger(mUnitSkill.anim_name);
			MMOUnit target = null;
			if (action.targetId > 0) {
				target = MMOController.Instance.GetUnitByUnitId (action.targetId);
			}
			yield return new WaitForSeconds (mSkill.active);
			if (mSkill.is_remote > 0) {
				//Remote attack;
				ShootObject shootObj = null;
				switch(mSkill.shoot_move_type){
				case 1:
					Shoot (MMOController.Instance.shootPrefabs [0].gameObject,mSkill.shoot_move_speed,caster,target,mSkill.range);
					break;
				default:
					Shoot (MMOController.Instance.shootPrefabs [1].gameObject,mSkill.shoot_move_speed,caster,target,mSkill.range);
					break;
				}
			}
			yield return null;
		}
		//Shoot Action.
		public void Shoot(GameObject shootPrefab,float speed,MMOUnit caster,MMOUnit target,float range){
			GameObject shootGo = Instantiater.Spawn (false, shootPrefab, caster.GetBodyPos (), caster.transform.rotation * Quaternion.Euler (60, 0, 0));
			ShootObject shootObj = shootGo.GetComponent<ShootObject> ();
			shootObj.speed = speed;
			if (target != null) {
				shootObj.Shoot (caster, target, new Vector3 (0, target.GetBodyHeight () / 2f, 0));// (caster, new Vector3(0,target.GetBodyHeight() / 2f, Vector3.zero));
			} else {
				Vector3 targetPos = MMOController.Instance.GetTerrainPos (caster.transform.position + caster.transform.forward * range);
				shootObj.Shoot (caster,targetPos,Vector3.zero);
			}
		}

	}
}
