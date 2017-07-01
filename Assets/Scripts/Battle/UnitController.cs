using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class UnitController : MonoBehaviour {

	public Animator animator;
	public PlayMakerFSM pm;
	public FsmBool[] fssBool;

	protected virtual void Awake(){
		animator = GetComponent<Animator> ();
		pm = GetComponent<PlayMakerFSM>();
		fssBool = pm.FsmVariables.BoolVariables;
	}

#region Animator Event
	public void OnFallDone()
	{
		pm.FsmVariables.FindFsmBool("isFall").Value = false;
	}
	
	public void OnLieDone()
	{
		pm.FsmVariables.FindFsmBool("isLie").Value = false;
	}
	
	public void OnAdjuredDone()
	{
		pm.FsmVariables.FindFsmBool("isInjured").Value = false;
	}
	
	public void OnAttackXDone()
	{
		pm.FsmVariables.FindFsmBool("isAttackX").Value = false;
	}
	
	public void OnAttackADone()
	{
		pm.FsmVariables.FindFsmBool("isAttackA").Value = false;
		animator.SetInteger ("ActionCMD",0);
	}
	
	public void OnAttackAADone()
	{
		pm.FsmVariables.FindFsmBool("isAttackAA").Value = false;
		animator.SetInteger ("ActionCMD",0);
	}
	
	public void OnAttackAAADone()
	{
		pm.FsmVariables.FindFsmBool("isAttackAAA").Value = false;
		animator.SetInteger ("ActionCMD",0);
	}
	
	public void OnRollDone()
	{
		Debug.Log ("OnRollDone");
		pm.FsmVariables.FindFsmBool("isRoll").Value = false;
	}

	public void ResetState(string stateName)
	{
		pm.FsmVariables.FindFsmBool(stateName).Value = false;
	}

#endregion Animator Event


#region Playmaker Event
	public void ReSetAnimatorBool(string animName)
	{
		if(animName!=null && animator!=null)animator.SetBool (animName,false);
	}
	
	public void SetAnimatorBool(string animName)
	{
		animator.SetBool (animName,true);
	}
	
	public void AdjustAnimatorSpeed(float speed)
	{
//		Debug.Log ("AdjustAnimatorSpeed");
		animator.speed = speed;
	}
	
	public void SetAnimatorTrigger(string triggerName)
	{
		animator.SetTrigger (triggerName);
	}
	
	public void SetActionCMD(int cmd)
	{
//		Debug.Log ("SetActionCMD");
		animator.SetInteger ("ActionCMD",cmd);
	}

	public void ResetBoolVariablesExceptBattle()
	{
	}

#endregion




	public virtual void OnHit(int hitType,float damage,UnitController attacker,bool isRepel = false)
	{

	}

	protected virtual bool CheckControllAble()
	{
		string currentState = pm.ActiveStateName;
		if(currentState == "Injured" || currentState == "Lie" || currentState == "Fall")
		{
			return false;
		}
		return true;
	}


}
