using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class SkillBase
	{
		public float coolDown = 5f;

		public MSkill mSkill;
		public MUnitSkill mUnitSkill;

		float mNextActiveTime;
		MMOUnit mMMOUnit;

		public SkillBase(int unitSkillId,MMOUnit mmoUnit){
			this.mMMOUnit = mmoUnit;
			MUnitSkill unitSkill = CSVManager.Instance.unitSkillDic [unitSkillId];
			MSkill skill = CSVManager.Instance.skillDic[unitSkill.skill_id];
			this.coolDown = skill.cooldown + unitSkill.anim_length;
			this.mSkill = skill;
			this.mUnitSkill = unitSkill;
		}

		public virtual bool IsUseAble ()
		{
			return !IsInCooldown();
		}

		bool IsInCooldown(){
			if (mNextActiveTime < Time.time) {
				return false;
			} else {
				Debug.Log (string.Format("skill {0} is in cooldown.{1}",mSkill.id,coolDown));
				return true;
			}
		}

		public virtual bool Play ()
		{
			if (IsUseAble ()) {
				MMOController.Instance.SendPlayerAction (BattleConst.UnitMachineStatus.CAST,mUnitSkill.id);
				StatusInfo statusInfo = new StatusInfo ();
				statusInfo.casterId = mMMOUnit.unitInfo.attribute.unitId;
				statusInfo.actionId = mUnitSkill.id;
				if (MMOController.Instance.selectedUnit != null)
					statusInfo.targetId = MMOController.Instance.selectedUnit.unitInfo.attribute.unitId;
				ActionManager.Instance.DoSkill (statusInfo);
				mNextActiveTime = Time.time + coolDown;
				return true;
			} 
			return false;
		}

		public float GetCooldown(){
			return (mNextActiveTime - Time.time) / coolDown;
		}
	}
}
