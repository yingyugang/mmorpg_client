using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class MMOUnitSkill : MonoBehaviour
	{

		public List<BaseSkill> skillList;
		public Dictionary<int,BaseSkill> skillDic;
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
			skillList = new List<BaseSkill> ();
			skillDic = new Dictionary<int, BaseSkill> ();
			for (int i = 0; i < mmoUnit.unitInfo.skillIds.Length; i++) {
				ShootSkill skillBase = new ShootSkill ();
				skillBase.mmoUnit = mmoUnit;
				skillBase.skillId = mmoUnit.unitInfo.skillIds[i];
				skillList.Add (skillBase);
				skillDic.Add (skillBase.skillId, skillBase);
			}
		}

		//スキルidがサーバーに通信される
		public void PlayClientSkill (BaseSkill skillBase)
		{
			Debug.Log ("PlayClientSkill");
			if (skillBase.IsUseAble ()) {
				MMOController.Instance.DoServerPlayerAction (1, skillBase.skillId);
				//TODO remove later.
				MMOController.Instance.playerInfo.skillId = skillBase.skillId;
			}
		}

		//サーバーから通信されたスキルidで実行する
		//这是一个演出，没有具体的伤害逻辑，只需要使用跟服务器一样的判断逻辑，不需要非常精准。
		//必须要服务器验证过返回过后才能调用，防止延迟，否则需要在冷却部分加上逻辑。
		public void PlayServerSkill (int skillId)
		{
			Debug.Log (skillId);
			BaseSkill skillBase = skillDic [skillId];
			skillBase.Play ();
		}

	}
}
