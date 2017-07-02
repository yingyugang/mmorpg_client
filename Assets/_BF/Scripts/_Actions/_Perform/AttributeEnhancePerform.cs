using UnityEngine;
using System.Collections;

public class AttributeEnhancePerform : BasePerform {

	protected override void OnPerformEnter ()
	{
		base.OnPerformEnter ();
		this.performDuration = 2;
		mShowEffectTime = Time.time + mEffectDelay;
		unit.GetComponent<Hero> ().Play (Hero.ACTION_CHEER);
	}

	float mEffectDelay = 0.5f;
	float mShowEffectTime =0;
	protected override void OnPerformUpdate ()
	{
		base.OnPerformUpdate ();
		if (mShowEffectTime < Time.time) {
			Color color = new Color (Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
			for(int i=0;i<unit.targetUnits.Count;i++){
//				PerformManager.GetInstance ().ShowBuff (unit.targetUnits[i],color);
			}
		}
	}

}
