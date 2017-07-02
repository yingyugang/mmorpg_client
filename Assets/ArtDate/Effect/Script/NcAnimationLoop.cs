using UnityEngine;
using System.Collections;

public class NcAnimationLoop : MonoBehaviour {
	public WrapMode wrapMode;
	// Use this for initialization
	void Start () {
		NcCurveAnimation nv = GetComponent<NcCurveAnimation>();
		for(int i=0;i < nv.GetCurveInfoCount();i++)
		{
			NcCurveAnimation.NcInfoCurve info = nv.GetCurveInfo(i);
			info.m_AniCurve.preWrapMode = wrapMode;
			info.m_AniCurve.postWrapMode = wrapMode;
		}
	}
	
	// Update is called once per frame
	void Update () {
		AnimationCurve ac;

	}

}
