using UnityEngine;
using System.Collections;

public class HitAction : UnitBaseAction {

//	float hitDuration = 0.
	float mExitTime = 0;
	public override void OnEnter ()
	{
		base.OnEnter ();
		unit.Play (_UnitArtActionType.cmn_0006 );
		mExitTime = Time.time + unit.GetAnimationClipLenth (_UnitArtActionType.cmn_0006);
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		if(mExitTime < Time.time){
			statusMachine.ChangeStatus (_UnitMachineStatus.StandBy.ToString ());
			return;
		}
	}

}
