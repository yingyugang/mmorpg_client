using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class MMOUnitSkill : MonoBehaviour
	{

		public List<SkillBase> skillList;
		public Dictionary<int,SkillBase> skillDic;
		public MMOUnit mmoUnit;
		Transform mTrans;

		void Awake ()
		{
			mTrans = transform;
			mmoUnit = GetComponent<MMOUnit> ();
			InitSkills ();
		}

		//TODO Init Skills from config files;
		public void InitSkills ()
		{
			skillList = new List<SkillBase> ();
			skillDic = new Dictionary<int, SkillBase> ();
			for (int i = 0; i < 20; i++) {
				SkillBase skillBase = new SkillBase ();
				skillBase.skillId = i;
				skillList.Add (skillBase);
				skillDic.Add (skillBase.skillId, skillBase);
			}
		}

		//スキルidがサーバーに通信される
		public void PlayClientSkill (SkillBase skillBase)
		{
			if (skillBase.IsUseAble ()) {
				MMOController.Instance.SendUseSkill (skillBase.skillId);


			}
		}

		//サーバーから通信されたスキルidで実行する
		//这是一个演出，没有具体的伤害逻辑，只需要使用跟服务器一样的判断逻辑，不需要非常精准。
		//必须要服务器验证过返回过后才能调用，防止延迟，否则需要在冷却部分加上逻辑。
		public void PlayServerSkill (int skillId)
		{
			SkillBase skillBase = skillDic [skillId];
			skillBase.Play ();
		}

		//表现层
		public void PlaySkillEffects (int skillType)
		{
			GameObject shootPrefab = MMOController.Instance.shootPrefabs [skillType].gameObject;
			GameObject shootGo = Instantiater.Spawn (false, shootPrefab, mTrans.position + new Vector3 (0, 1, 0), mTrans.rotation * Quaternion.Euler (60, 0, 0));
			ShootObject so = shootGo.GetComponent<ShootObject> ();
			so.Shoot (mmoUnit, mmoUnit.unitInfo.action.targetPos, Vector3.zero);
		}




	}
}
