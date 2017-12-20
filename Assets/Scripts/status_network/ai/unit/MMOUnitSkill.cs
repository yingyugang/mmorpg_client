using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class MMOUnitSkill : MonoBehaviour
	{

		public List<SkillBase> skillList;
		public Dictionary<int,SkillBase> skilDic;
		public MMOUnit mmoUnit;

		void Awake(){
			mmoUnit = GetComponent<MMOUnit> ();
			InitSkills ();
		}

		//TODO Init Skills from config files;
		public void InitSkills(){
			skillList = new List<SkillBase> ();
			skilDic = new Dictionary<int, SkillBase> ();
			SkillBase skillBase = new SkillBase ();
			skillBase.skillId = 1;
			skillList.Add (skillBase);
			skilDic.Add (skillBase.skillId,skillBase);
		}

		//スキルidがサーバーに通信される
		public void PlayClientSkill(int skillId){
			SkillBase skillBase = skilDic[skillId];
			if (skillBase.IsUseAble()) {
				MMOController.Instance.SendUseSkill (skillId);
			}
		}

		//サーバーから通信されたスキルidで実行する
		public void PlayServerSkill(){
			
		}

	}
}
