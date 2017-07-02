using UnityEngine;
using System.Collections;
using Spine.Unity;

[ExecuteInEditMode]
public class UnitTest : MonoBehaviour {

	SkeletonAnimation mSkeletonAnimation;
	public float delay = 0.4f;

	void Awake(){
		mSkeletonAnimation = GetComponent<SkeletonAnimation> ();
		mSkeletonAnimation.AnimationName = _UnitArtActionType.atk_0101.ToString ();
	}

	void Start () {
	
	}

	public bool load;
	void Update () {
		if(load){
			load = false;
		}
	}
}
