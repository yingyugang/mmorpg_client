using UnityEngine;
using System.Collections;
using StatusMachines;

public enum MoveType{
	TargetHero,
	BattleFieldCenter,
	None
};
public class MoveToAttackAction : UnitBaseAction
{
	Vector2 mMoveTarget;
	public Vector2 direct;
	public float maxT;
	Transform mTran;
	Vector2 mStartPos;
	public override void OnEnter ()
	{
		if (unit.currentSkill.impactCount > 1)
			mMoveTarget = (Vector2)BattleUtility.GetLeftSkillTargetPos ();
		else {
			mMoveTarget = GetMoveTargetPos (unit);
		}
		direct = (mMoveTarget - (Vector2)unit.transform.position).normalized;
		mTran = unit.transform;
		mStartPos = mTran.position;
		t = 0;
		maxT = unit.unitAnimation.GetAnimationClipLenth (_UnitArtActionType.cmn_0003.ToString());
		maxT = 0.2f;
	}
		
	float t;
	public override void OnUpdate ()
	{
		t += Time.deltaTime/maxT;
		t = Mathf.Min(1,t);
		mTran.position = Vector3.Lerp (mStartPos,mMoveTarget,t);//   Curve.Bezier2(startPos,controllPos,targetPos,t);
		if(t >= 1){
			statusMachine.ChangeStatus (_UnitMachineStatus.Cast.ToString());
		}
	}

	public override void OnExit ()
	{
		
	}

	public static Vector2 GetMoveTargetPos(Unit unit){
		Vector2 offset = Vector2.zero;
		Vector2 moveTarget = Vector2.zero;
		moveTarget = (Vector2)unit.targetUnits [0].transform.position - unit.targetUnits [0].unitRes.GetWidthOffset() + unit.unitRes.GetWidthOffset();
		return moveTarget;
	}

}
