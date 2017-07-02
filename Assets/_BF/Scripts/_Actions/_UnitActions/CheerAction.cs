using UnityEngine;
using System.Collections;
using StatusMachines;

public class CheerAction : UnitBaseAction {

	public override void OnEnter ()
	{
		base.OnEnter ();
		unit.Play (_UnitArtActionType.cmn_0001);
	}

}
