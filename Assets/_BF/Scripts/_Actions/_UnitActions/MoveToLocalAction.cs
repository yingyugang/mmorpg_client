using UnityEngine;
using System.Collections;
using StatusMachines;

public class MoveToLocalAction : UnitBaseAction {

	Vector2 mMoveTarget;
	public Vector2 direct;
	Transform mTran;
	float t = 0;
	float maxT = 0;
	Vector2 startPos;
	Vector2 targetPos;
	Vector2 controllPos;
	public override void OnEnter ()
	{
		mTran = GO.transform;
		mMoveTarget = unit.targetUnits[0].defaultPos;
		direct = (mMoveTarget - (Vector2)unit.transform.position).normalized;
//		hero.heroAnimation.Play (Hero.ACTION_STANDBY);
		t = 0;
		startPos = (Vector2)mTran.position;
		targetPos = (Vector2)unit.defaultPos;
		controllPos = (Vector2)(targetPos + startPos) / 2 + new Vector2(0,2);
		maxT = Vector2.Distance(startPos,targetPos) / 40;
		maxT = unit.unitAnimation.GetAnimationClipLenth (_UnitArtActionType.cmn_0004.ToString());

		maxT = 0.2f;
//		unit.Play (_UnitArtActionType.cmn_0004.ToString());

	}

	public override void OnUpdate ()
	{
		t += Time.deltaTime/maxT;
		t = Mathf.Min(1,t);
		mTran.position = Curve.Bezier2(startPos,controllPos,targetPos,t);
		if(t >= 1){
			statusMachine.ChangeStatus (_UnitMachineStatus.StandBy.ToString());
		}
	}

	public override void OnExit ()
	{

	}

}
