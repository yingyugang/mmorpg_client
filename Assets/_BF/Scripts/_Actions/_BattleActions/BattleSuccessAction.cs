using UnityEngine;
using System.Collections;
using StatusMachines;

public class BattleSuccessAction:BattleBaseAction
{

	public override void OnAwake ()
	{
		base.OnAwake ();
	}

	public override void OnEnter ()
	{
		foreach (Unit unit in BattleManager.GetInstance().rightUnits) {
			unit.GetComponent<Hero> ().heroRes.CurrentAm.anim.wrapMode = WrapMode.Loop;
			unit.GetComponent<Hero> ().heroAnimation.Play (Hero.ACTION_CHEER);
		}
		AudioManager.SingleTon ().PlayBattleWinClip ();
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
	}

}

