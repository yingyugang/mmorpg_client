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
	public UnityAction onIdle;

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
		mAnimator.SetFloat ("MoveSpeed",speed);
	}

	public bool IsRun(){
		return mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("run");
	}

	public bool IsInState(string state){
		return mAnimator.GetCurrentAnimatorStateInfo (0).IsName (state);
	}

	bool mIsIdle;
	void CheckOnIdle(){
		if (!mIsIdle && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			mIsIdle = true;
			if (onIdle != null) {
				onIdle ();
			}
		} else if(mIsIdle && !mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("idle")){
			mIsIdle = false;
		}
	}

	public bool GetTrigger(string trigger){
		return mAnimator.GetBool (trigger);
	}

	public void SetTrigger(string trigger){
		mAnimator.SetTrigger (trigger);
	}

	public void Play(string clip){
		if (mAnimation != null) {
			mAnimation.Play (clip);
		} else {
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