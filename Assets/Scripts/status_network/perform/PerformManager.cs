using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MMO
{
	public class PerformManager : SingleMonoBehaviour<PerformManager>
	{

		public GameObject hitUITextPrefab;
		public List<GameObject> hitPrefabs;

		//TODO 表示する場合が確認する
		public void ShowHitInfo (HitInfo hitInfo, Dictionary<int,GameObject> unitDic)
		{
			ShowHitEffects (hitInfo);
			ShowHitUIInfos (hitInfo, unitDic);
		}

		void ShowHitEffects (HitInfo hitInfo)
		{
			for (int i = 0; i < hitInfo.hitObjectIds.Length; i++) {
				ShowHitEffect (hitInfo.hitObjectIds [i], hitInfo.hitPositions [i]);
			}
		}

		void ShowHitEffect (int objectId, IntVector3 pos)
		{
			GameObject prefab = this.hitPrefabs [objectId];
			GameObject go = Instantiater.Spawn (false, prefab, IntVector3.ToVector3 (pos), Quaternion.identity);
			Destroy (go, 10);
		}

		void ShowHitUIInfos (HitInfo hitInfo, Dictionary<int,GameObject> unitDic)
		{
			for (int j = 0; j < hitInfo.hitIds.Length; j++) {
				if (unitDic.ContainsKey (hitInfo.hitIds [j])) {
					GameObject go = unitDic [hitInfo.hitIds [j]];
					ShowHitUIInfo (hitInfo.skillId , go.GetComponent<MMOUnit> (), hitInfo.nums [j]);
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
				textMeshPro.color = Color.red;
			} else {
				textMeshPro.text =  string.Format("{0} {1:G}",effectName,-val );
				textMeshPro.color = Color.green;
			}
			uiGo.SetActive (true);
		}

		void ShowHealUI(string effectName,MMOUnit mmoUnit, int val){
			GameObject uiGo = Instantiater.Spawn (false, this.hitUITextPrefab, mmoUnit.GetHeadPos () , Quaternion.identity);
			TextMeshPro textMeshPro = uiGo.GetComponentInChildren<TextMeshPro> (true);
			textMeshPro.text =  string.Format("{0} {1:G}",effectName,val );
			textMeshPro.color = Color.green;
			uiGo.SetActive (true);
		}

		void ShowBuffUI(string effectName,MMOUnit mmoUnit, int val){
			GameObject uiGo = Instantiater.Spawn (false, this.hitUITextPrefab, mmoUnit.GetHeadPos () , Quaternion.identity);
			TextMeshPro textMeshPro = uiGo.GetComponentInChildren<TextMeshPro> (true);
			textMeshPro.text = string.Format("{0} {1:P}",effectName,val / 10000f);
			textMeshPro.color = Color.yellow;
			uiGo.SetActive (true);
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
