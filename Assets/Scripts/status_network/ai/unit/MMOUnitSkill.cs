using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	//player has only. 
	public class MMOUnitSkill : MonoBehaviour
	{
		//cooldown,skill,unitskill.
		public List<SkillBase> skillList;
		public Dictionary<int,SkillBase> skillDic;
		public MMOUnit mmoUnit;
		Transform mTrans;
		bool mIsInitted;

		void Awake ()
		{
			mTrans = transform;
			mmoUnit = GetComponent<MMOUnit> ();
		}

		public bool IsInitted{
			get{
				return mIsInitted;
			}
		}

		public void InitSkills ()
		{
			if (mIsInitted)
				return;
			mIsInitted = true;
			skillList = new List<SkillBase> ();
			skillDic = new Dictionary<int, SkillBase> ();
			for (int i = 0; i < mmoUnit.unitInfo.unitSkillIds.Length; i++) {
				int unitSkillId = mmoUnit.unitInfo.unitSkillIds[i];
				SkillBase skillBase = new SkillBase (unitSkillId,this.mmoUnit);
				skillList.Add (skillBase);
				if(!skillDic.ContainsKey(skillBase.mSkill.id))
					skillDic.Add (skillBase.mSkill.id,skillBase);
			}
		}

		//スキルidがサーバーに通信される
		public void PlayClientSkill (SkillBase skillBase)
		{
			Debug.Log ("PlayClientSkill");
			if (skillBase.IsUseAble ()) {
				MMOController.Instance.DoServerPlayerAction (1, skillBase.skillId);
				//TODO remove later.
				MMOController.Instance.playerInfo.skillId = skillBase.skillId;
			}
		}

	}
}
