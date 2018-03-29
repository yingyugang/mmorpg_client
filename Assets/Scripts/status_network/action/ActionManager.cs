using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MMO
{
	public class ActionManager : SingleMonoBehaviour<ActionManager>
	{
		public void DoAction (ActionInfo action)
		{
			Debug.Log (JsonUtility.ToJson(action));
			switch (action.actionType) {
			case 1:
				//MMOSkillManager.Instance.DoSkill (playerUnit,targetUnit,action);
				DoSkill (action);
				break;
			case 2:
				//TODO
				//Create projectile object;
				break;
			case 3:
				//TODO
				//Create hit object 
				break;
			default:
				break;
			}
		}

		void DoSkill (ActionInfo action)
		{
			//TODO need to get the real skill information from csv.
			//now the action.actionId means the skill id.
			StartCoroutine (_Cast(action));
		}

		//TODO use trigger to controll the cast clip;
		//TODO the end time point need to same as the animclip end time point.
		IEnumerator _Cast(ActionInfo action){
			MSkill mSkill = CSVManager.Instance.skillDic [action.actionId];
			MMOUnit caster = MMOController.Instance.GetUnitByUnitId (action.casterId);
			//TODO need know the cast anim name from csv or from other area.
			caster.SetTrigger (AnimationConstant.UNIT_ANIMATION_CLIP_ATTACK3);
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
					Shoot (MMOController.Instance.shootPrefabs [0].gameObject,mSkill.shoot_move_speed,caster,target);
					break;
				default:
					Shoot (MMOController.Instance.shootPrefabs [1].gameObject,mSkill.shoot_move_speed,caster,target);
					break;
				}
			}
			yield return null;
		}

		void Shoot(GameObject shootPrefab,float speed,MMOUnit caster,MMOUnit target){
			GameObject shootGo = Instantiater.Spawn (false, shootPrefab, caster.GetBodyPos (), caster.transform.rotation * Quaternion.Euler (60, 0, 0));
			ShootObject shootObj = shootGo.GetComponent<ShootObject> ();
			shootObj.speed = speed;
			shootObj.Shoot (caster,target, Vector3.zero);
		}

	}
}
