using UnityEngine;
using System.Collections;
using StatusMachines;

public class BattleBaseAction : StatusAction {

	protected BattleManager mBattleManager;

	public override void OnAwake ()
	{
		base.OnAwake ();
		mBattleManager = GO.GetComponent<BattleManager> ();
	}

}
