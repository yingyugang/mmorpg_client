using UnityEngine;
using System.Collections;

public class UIDamage : MonoBehaviour {

	UITweener[] tweeners;

	void Awake () {
		tweeners = GetComponents<UITweener>();
	}

	void OnEnable()
	{
		Play();
	}

	public void Play()
	{
		PoolManager.SingleTon().UnSpawn(1,gameObject);
		foreach(UITweener tw in tweeners)
		{
			tw.ResetToBeginning();
			tw.PlayForward();
		}
	}

}
