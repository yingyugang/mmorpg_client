using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class SkillBase
	{
		public int skillId;
		public float coolDown = 5f;
		float mNextActiveTime;
		public MMOUnit mmoUnit;

		public virtual void OnAwake ()
		{
		
		}

		public virtual void OnEnable ()
		{

		}

		public virtual bool IsUseAble ()
		{
			return mNextActiveTime < Time.time;
		}

		public virtual bool Play ()
		{
//			bool playAble = IsUseAble ();
//			if(playAble){
				OnActive ();
//			}
			return true;
		}

		protected virtual void OnActive(){
			mNextActiveTime = Time.time + coolDown;
		}

	}
}
