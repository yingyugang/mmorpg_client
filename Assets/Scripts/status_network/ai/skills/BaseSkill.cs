using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class SkillBase
	{
		public int skillId;
		public int unitSkillId;
		public float coolDown = 5f;

		public MSkill mSkill;
		public MUnitSkill mUnitSkill;

		float mNextActiveTime;
		MMOUnit mMMOUnit;

		public SkillBase(int unitSkillId,MMOUnit mmoUnit){
			this.mMMOUnit = mmoUnit;
			MUnitSkill unitSkill = CSVManager.Instance.unitSkillDic [unitSkillId];
			MSkill skill = CSVManager.Instance.skillDic[mUnitSkill.skill_id];
			this.coolDown = skill.cooldown;
			this.mMMOUnit = mmoUnit;
			this.mSkill = skill;
			this.mUnitSkill = unitSkill;
		}

		public virtual bool IsUseAble ()
		{
			return mNextActiveTime < Time.time;
		}

		public virtual bool Play ()
		{
			if(IsUseAble()){
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
