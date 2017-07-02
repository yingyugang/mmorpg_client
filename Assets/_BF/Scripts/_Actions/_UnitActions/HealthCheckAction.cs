using UnityEngine;
using System.Collections;
using StatusMachines;

public class HealthCheckAction : UnitBaseAction {

	public override void OnUpdate ()
	{
		base.OnUpdate ();
//		if (hero.heroAttribute.calculateHP <= 0) {
//			statusMachine.ChangeStatus (_UnitMachineStatus.Death.ToString ());
//		}
	}
}
