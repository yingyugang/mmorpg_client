using UnityEngine;
using System.Collections;
using Spine.Unity;

public class UnitAnimation : MonoBehaviour {

	SkeletonAnimation mSkeletonAnimation;

	void Awake(){
		mSkeletonAnimation = GetComponent<SkeletonAnimation> ();
	}

	public float GetAnimationClipLenth(string clipName){
		return mSkeletonAnimation.Skeleton.Data.FindAnimation (clipName).Duration;
	}

	public void PlayAnimation(string clipName){
		if (mSkeletonAnimation.Skeleton.Data.FindAnimation (clipName) == null) {
			return;
		}
		if (clipName == "cmn_0001")
			mSkeletonAnimation.loop = true;
		else
			mSkeletonAnimation.loop = false;
		mSkeletonAnimation.AnimationName = clipName;
	}

}
