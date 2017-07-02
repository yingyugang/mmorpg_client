using UnityEngine;
using System.Collections;
using StatusMachines;

public class UnitBaseAction : StatusAction {

//	public Hero hero;
	public Unit unit;

	public override void OnAwake ()
	{
		base.OnAwake ();
//		hero = GO.GetComponent<Hero> ();
		unit = GO.GetComponent<Unit> ();
	}

}
