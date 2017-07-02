using UnityEngine;
using System.Collections;

public class DefaultAttackWithPosChangePerform : DefaultAttackPerform {

	public override void OnPartmentEnter (SkillEffectPartment partment, int index = 0)
	{
		base.OnPartmentEnter (partment, index);
//		Vector2 targetPos = MoveToAttackAction.GetMoveTargetPos (unit);
//		unit.transform.position = partment.partmentTargets [0].transform.position;
		unit.transform.position = (Vector2)partment.partmentTargets [0].transform.position - partment.partmentTargets [0].unitRes.GetWidthOffset() + unit.unitRes.GetWidthOffset();
	}

}
