using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	//不管有没有animator这里的脚本都有效果
	//TODO 服务器端模拟这个transformation过程，实现完全同步（检讨）
	public class UnitAnimator : MonoBehaviour
	{

		public MMOUnit mmoUnit;
		public Animator animator;
		HashSet<string> mParameters;

		void Awake ()
		{
			if (mmoUnit == null)
				mmoUnit = GetComponent<MMOUnit> ();
			if (animator == null)
				animator = GetComponentInChildren<Animator> (true);
			mParameters = new HashSet<string> ();
			if(animator!=null){
				for(int i=0;i<animator.parameters.Length;i++){
					if(!mParameters.Contains(animator.parameters[i].name)){
						mParameters.Add (animator.parameters[i].name);
					}
				}				
			}
		}

		void Update ()
		{
			CheckOnIdle ();
		}

		bool ContainParameter(string paramter){
			return this.mParameters.Contains (paramter);
		}

		public void SetMoveSpeed (float speed)
		{
			if (animator != null && ContainParameter(AnimationConstant.UNIT_ANIMATION_PARAMETER_MOVESPEED))
				animator.SetFloat (AnimationConstant.UNIT_ANIMATION_PARAMETER_MOVESPEED, speed);
		}

		public bool IsRun ()
		{
			if (animator != null)
				return animator.GetCurrentAnimatorStateInfo (0).IsName (AnimationConstant.UNIT_ANIMATION_CLIP_RUN);
			return false;
		}

		public bool IsInState (string state)
		{
			if (animator != null)
				return animator.GetCurrentAnimatorStateInfo (0).IsName (state);
			return false;
		}

		public bool IsIdle ()
		{
			return mIsIdle;
		}

		bool mIsIdle;

		void CheckOnIdle ()
		{
			if (animator != null && !mIsIdle && animator.GetCurrentAnimatorStateInfo (0).IsName (AnimationConstant.UNIT_ANIMATION_CLIP_IDEL)) {
				mIsIdle = true;
			} else if (mIsIdle && !animator.GetCurrentAnimatorStateInfo (0).IsName (AnimationConstant.UNIT_ANIMATION_CLIP_IDEL)) {
				mIsIdle = false;
			}
		}

		public bool GetTrigger (string trigger)
		{
			if (animator != null)
				return animator.GetBool (trigger);
			return false;
		}

		public void SetTrigger (string trigger)
		{
			if (animator != null && ContainParameter(trigger))
				animator.SetTrigger (trigger);
		}

		public void RemoveAllAttackTriggers ()
		{
			animator.SetBool (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK1, false);
			animator.SetBool (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK2, false);
			animator.SetBool (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK3, false);
			animator.SetBool (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK4, false);
			animator.SetBool (AnimationConstant.UNIT_ANIMATION_PARAMETER_CAST, false);
		}

		public void RemoveAllNormalAttackTriggers ()
		{
			animator.SetBool (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK1, false);
			animator.SetBool (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK2, false);
			animator.SetBool (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK3, false);
			animator.SetBool (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK4, false);
		}

		public void Play (string clip)
		{
			if (animator != null)
				animator.Play (clip);
		}

		public void SetSpeed (float n)
		{
			if (animator != null && animator.speed != n) {
				animator.speed = n;
			}
		}


	}
}
