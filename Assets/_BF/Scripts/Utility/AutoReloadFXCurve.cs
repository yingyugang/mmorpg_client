using UnityEngine;
using System.Collections;

public class AutoReloadFXCurve : MonoBehaviour {

	void OnEnable()
	{
		NcCurveAnimation[] ncAnims = GetComponentsInChildren<NcCurveAnimation>();
		foreach(NcCurveAnimation ncAnim in ncAnims)
		{
//			if(ncAnim.IsEndAnimation())
			//ncAnim.isResetOnEnabel = true;
//			ncAnim.ResetAnimation();
			ncAnim.StopAllCoroutines();
		}
	}

}
