using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MMO
{
	public class ActionManager : SingleMonoBehaviour<ActionManager>
	{
		public void DoAction (MMOAction action)
		{
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

		void DoSkill (MMOAction action)
		{
			//TODO need to get the real skill information from csv.
			//now the action.actionId means the skill id.
			switch (action.actionId) {
			case 1:
				CreateProjectileShoot (action);
				break;
			case 2:

				break;

			case 3:

				break;

			default:

				break;
			}
		}


		//may CreateProjectile and CreateNormalShoot merge ?
		void CreateProjectileShoot (MMOAction action)
		{
			Debug.Log ("PlaySkillEffects");
			MMOUnit caster = MMOController.Instance.GetUnitByUnitId (action.casterId);
			GameObject shootPrefab = MMOController.Instance.shootPrefabs [0].gameObject;
			//TODO 
			GameObject shootGo = Instantiater.Spawn (false, shootPrefab,caster.GetBodyPos(), caster.transform.rotation * Quaternion.Euler (60, 0, 0));
			ShootProjectileObject so = shootGo.GetComponent<ShootProjectileObject> ();
			if (action.targetId > 0) {
				MMOUnit target = MMOController.Instance.GetUnitByUnitId (action.targetId);
				so.Shoot (caster, target.GetBodyPos(), Vector3.zero);
			} else {
				so.Shoot (caster,IntVector3.ToVector3(action.targetPos), Vector3.zero);
			}
		}

		void CreateNormalShoot(MMOAction action){
		
		}


	}
}
