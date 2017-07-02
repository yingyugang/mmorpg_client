using UnityEngine;
using System.Collections;

public class SkillAttackWithPosChangePerform : SkillAttackPerform {

	public override void OnPartmentEnter (SkillEffectPartment partment, int index = 0)
	{
		base.OnPartmentEnter (partment, index);
		Vector2 targetPos = MoveToAttackAction.GetMoveTargetPos (unit);
		unit.transform.position = targetPos;
	}

}