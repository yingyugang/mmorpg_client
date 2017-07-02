using UnityEngine;
using System.Collections;
using StatusMachines;
using System.Collections.Generic;

public class CastAction : UnitBaseAction
{
	public BaseSkill baseSkill;

	public override void OnAwake ()
	{
		base.OnAwake ();
	}

	public override void OnEnter ()
	{
		baseSkill = unit.currentSkill;
		baseSkill.OnEnter ();
	}

	public override void OnUpdate ()
	{
		if(baseSkill!=null)
			baseSkill.OnUpdate ();
	}

	public override void OnExit ()
	{
		if(baseSkill!=null)
			baseSkill.OnExit ();
	}

}
