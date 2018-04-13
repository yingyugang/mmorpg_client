using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MMO
{
	public class PerformManager : SingleMonoBehaviour<PerformManager>
	{

		public GameObject hitUITextPrefab;

		//TODO 表示する場合が確認する
		public void ShowHitInfo (HitInfo hitInfo, Dictionary<int,GameObject> unitDic)
		{
			ShowHitEffects (hitInfo);
			ShowHitUIInfos (hitInfo, unitDic);
		}

		void ShowHitEffects (HitInfo hitInfo)
		{
			if (CSVManager.Instance.unitSkillDic.ContainsKey (hitInfo.unitSkillId)) {
				MUnitSkill mUnitSkill = CSVManager.Instance.unitSkillDic [hitInfo.unitSkillId];
				for (int i = 0; i < hitInfo.hitPositions.Length; i++) {
					Vector3 hitPos = IntVector3.ToVector3 (hitInfo.hitPositions[i]);
					ShowHitEffect (mUnitSkill.main_hit_object_id, hitPos);
				}
				for (int i = 0; i < hitInfo.hitIds.Length; i++) {
					MMOUnit mmoUnit = MMOController.Instance.GetUnitByUnitId (hitInfo.hitIds[i]);
					Vector3 hitPos;
					if (mmoUnit != null){
						hitPos = mmoUnit.GetBodyPos ();
						ShowHitEffect (mUnitSkill.sub_hit_object_id, hitPos);
					}
				}
			} else {
				Debug.LogError (string.Format("hitInfo.unitSkillId:{0} is not exiting.",hitInfo.unitSkillId));
			}
		}

		public void ShowRespawnEffect(Vector3 pos){
			GameObject prefab = ResourcesManager.Instance.GetEffect (BattleConst.RESPAWN_EFFECT_ID);
			GameObject go = Instantiater.Spawn (false, prefab, pos, Quaternion.identity);
			Destroy (go, 10);
		}

		void ShowHitEffect (int objectId,Vector3 pos)//   int hitId, IntVector3 pos)
		{
			if (objectId <= 0)
				return;
			GameObject prefab = ResourcesManager.Instance.GetEffect (objectId);
//			MMOUnit mmoUnit = MMOController.Instance.GetUnitByUnitId (hitId);
//			Vector3 hitPos = IntVector3.ToVector3 (pos);
//			if (mmoUnit != null)
//				hitPos = mmoUnit.GetBodyPos ();
			GameObject go = Instantiater.Spawn (false, prefab, pos, Quaternion.identity);
			Destroy (go, 10);
		}

		void ShowHitUIInfos (HitInfo hitInfo, Dictionary<int,GameObject> unitDic)
		{
			MMOUnit caster =  unitDic [hitInfo.casterId].GetComponent<MMOUnit>();
			for (int j = 0; j < hitInfo.hitIds.Length; j++) {
				if (unitDic.ContainsKey (hitInfo.hitIds [j])) {
					GameObject go = unitDic [hitInfo.hitIds [j]];
					MMOUnit mmoUnit = go.GetComponent<MMOUnit> ();
					mmoUnit.unitAnimator.PlayHit ();
					if (CSVManager.Instance.unitSkillDic.ContainsKey (hitInfo.unitSkillId)) {
						MUnitSkill mUnitSkill = CSVManager.Instance.unitSkillDic [hitInfo.unitSkillId];
						if (MMOController.Instance.IsPlayer (caster) || MMOController.Instance.IsPlayer (mmoUnit) || MMOController.Instance.isDebug) {
							ShowHitUIInfo (mUnitSkill.skill_id, go.GetComponent<MMOUnit> (), hitInfo.nums [j]);
						}
					} else {
						Debug.LogError (string.Format("hitInfo.unitSkillId:{0} is not exiting.",hitInfo.unitSkillId));
					}
				}
			}
		}

		//TODO change hit ui info color.
		void ShowHitUIInfo (int skillId,MMOUnit mmoUnit, int val)
		{
			if (CSVManager.Instance.skillDic.ContainsKey (skillId)) {
				MSkill skill = CSVManager.Instance.skillDic [skillId];
				MSkillEffectBaseCSVStructure skillEffectBase = skill.GetMSkillEffectBaseCSVStructure ();
				if(skillEffectBase!=null){
					string effectName = skillEffectBase.name;
					if (val != 0) {
						switch (skill.effect_type) {
						case BattleConst.BattleSkillEffectTypeConst.HP:
							ShowDamageUI (effectName,mmoUnit, val);
							break;
						case BattleConst.BattleSkillEffectTypeConst.Heal:
							ShowHealUI (effectName,mmoUnit, val);
							break;
						case BattleConst.BattleSkillEffectTypeConst.DebuffClear:
							break;
						case BattleConst.BattleSkillEffectTypeConst.BuffClear:
							break;
						case BattleConst.BattleSkillEffectTypeConst.StatusClear:
							break;
						default:
							ShowBuffUI (effectName, mmoUnit, val);
							break;
						}
					}
				}
			}
		}

		void ShowDamageUI(string effectName,MMOUnit mmoUnit, int val){
			GameObject uiGo = Instantiater.Spawn (false, this.hitUITextPrefab, mmoUnit.GetHeadPos () , Quaternion.identity);
			TextMeshPro textMeshPro = uiGo.GetComponentInChildren<TextMeshPro> (true);
			if (val > 0) {
				textMeshPro.text =  string.Format("{0} {1:G}",effectName,val );
				textMeshPro.color = Color.white;
			} else {
				textMeshPro.text =  string.Format("{0} {1:G}",effectName,-val );
				textMeshPro.color = Color.green;
			}
			uiGo.SetActive (true);
			Destroy (uiGo,5);
		}

		void ShowHealUI(string effectName,MMOUnit mmoUnit, int val){
			GameObject uiGo = Instantiater.Spawn (false, this.hitUITextPrefab, mmoUnit.GetHeadPos () , Quaternion.identity);
			TextMeshPro textMeshPro = uiGo.GetComponentInChildren<TextMeshPro> (true);
			textMeshPro.text =  string.Format("{0} {1:G}",effectName,val );
			textMeshPro.color = Color.green;
			uiGo.SetActive (true);
			Destroy (uiGo,5);
		}

		void ShowBuffUI(string effectName,MMOUnit mmoUnit, int val){
			GameObject uiGo = Instantiater.Spawn (false, this.hitUITextPrefab, mmoUnit.GetHeadPos () , Quaternion.identity);
			TextMeshPro textMeshPro = uiGo.GetComponentInChildren<TextMeshPro> (true);
			textMeshPro.text = string.Format("{0} {1:P}",effectName,val / 10000f);
			textMeshPro.color = Color.yellow;
			uiGo.SetActive (true);
			Destroy (uiGo,5);
		}

		public void ShowCurrentPlayerDeathEffect (MMOUnit playerUnit)
		{
			ImageEffectManager.Instance.ShowGray ();
		}

		public void HideCurrentPlayerDeathEffect ()
		{
			ImageEffectManager.Instance.HideGray ();
		}

	}
}
