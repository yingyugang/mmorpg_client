using UnityEngine;
using System.Collections;

public class SkillAttackPerform : DefaultAttackPerform {

	public override void OnEnter ()
	{
		base.OnEnter ();
	}

	protected override void OnDefaultAttackEnter ()
	{
		unit.Play (_UnitArtActionType.atk_0201,baseSkill.partments[0].partmentTargets);
	}
}
