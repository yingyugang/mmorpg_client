using UnityEngine;
using System.Collections;
using StatusMachines;

public class BattleFailureAction:BattleBaseAction
{

	public override void OnAwake ()
	{
		base.OnAwake ();
	}

	public override void OnEnter ()
	{
		if (AudioManager.SingleTon () != null)
			AudioManager.SingleTon ().PlayMusic (AudioManager.SingleTon ().MusicMainClip);
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
	}

}

