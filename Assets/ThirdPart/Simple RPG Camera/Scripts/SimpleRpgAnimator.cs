using UnityEngine;
using System.Collections;

public class SimpleRpgAnimator : MonoBehaviour
{
	public GameObject model;

	private bool _active = true;
	private string _action = string.Empty;
	private string _animation = string.Empty;

	Animation mAnimation;
	Animator mAnimator;

	public string Action
	{
		get { return _action; }
		set { _action = value; }
	}

	void Start()
	{
		mAnimation = model.GetComponent<Animation> ();
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
				_animation = _action;
				if (mAnimation != null) {
					mAnimation.Play (_animation);
				} else {
					mAnimator.Play (_animation);
				}
			}
		}
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