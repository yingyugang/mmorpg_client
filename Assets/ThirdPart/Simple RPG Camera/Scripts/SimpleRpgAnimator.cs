using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SimpleRpgAnimator : MonoBehaviour
{
	public GameObject model;

	private bool _active = true;
	private string _action = string.Empty;
	private string _animation = string.Empty;

	Animation mAnimation;
	public Animator mAnimator;

	public string Action
	{
		get { return _action; }
		set { _action = value; }
	}

	void Start()
	{
		mAnimation = model.GetComponentInChildren<Animation> (true);
		mAnimator = model.GetComponent<Animator> ();
		// Check to make sure the model is selected and has animation
		if(!model)
		{
			Debug.LogWarning("SimpleRpgAnimator: No model selected");
			_active = false;
		}
		else
		{
			if(!mAnimation && !mAnimator)
			{
				Debug.LogWarning("SimpleRpgAnimator: Selected model has no animation");
				_active = false;
			}
		}
	}
	
	void Update()
	{
		if(_active)
		{
			// CrossFade the animation to match the action
			if(_animation != _action)
			{
				Play (_action);
			}
		}
		CheckOnIdle ();
	}

	public void SetMoveSpeed(float speed){
		if(mAnimator!=null)
			mAnimator.SetFloat ("MoveSpeed",speed);
	}

	public bool IsRun(){
		if(mAnimator!=null)
			return mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("run");
		return false;
	}

	public bool IsInState(string state){
		if(mAnimator!=null)
			return mAnimator.GetCurrentAnimatorStateInfo (0).IsName (state);
		return false;
	}

	public bool IsIdle(){
		return mIsIdle;
	}

	bool mIsIdle;
	void CheckOnIdle(){
		if (mAnimator!=null && !mIsIdle && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			mIsIdle = true;
		} else if(mIsIdle && !mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("idle")){
			mIsIdle = false;
		}
	}

	public bool GetTrigger(string trigger){
		if(mAnimator!=null)
			return mAnimator.GetBool (trigger);
		return false;
	}

	public void SetTrigger(string trigger){
		if(mAnimator!=null)
			mAnimator.SetTrigger (trigger);
	}

	public void RemoveAllAttackTriggers(){
		mAnimator.SetBool ("attack1",false);
		mAnimator.SetBool ("attack2",false);
		mAnimator.SetBool ("attack3",false);
		mAnimator.SetBool ("attack4",false);
		mAnimator.SetBool ("cast",false);
	}

	public void Play(string clip){
		if (mAnimation != null) {
			mAnimation.Play (clip);
		} else {
			if(mAnimator!=null)
				mAnimator.Play (clip);
		}
		_action = clip;
		_animation = _action;
	}

	public void SetSpeed(float n)
	{
		if(_active)
		{
			// Set the current animation's speed
			if (mAnimation != null ) {
				if (mAnimation [_animation]) {
					if (mAnimation [_animation].speed != n) {
						mAnimation [_animation].speed = n;
					}
				}
			} else {
				if(mAnimator!=null && mAnimator.speed != n){
					mAnimator.speed = n;
				}
			}
		}
	}
}