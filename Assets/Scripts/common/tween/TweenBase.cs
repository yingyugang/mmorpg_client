using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Tween{
	public delegate void OnTweenDone();
	public class TweenBase : MonoBehaviour {

		public OnTweenDone onTweenDone;
		public UnityAction complete;
		public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
		public float duration = 1;
		public float delay = 0;
		public bool isLoop;
		public bool isPlayOnEnable;
		public bool isAutoHide;
		protected Transform mTrans;

		protected virtual void Awake(){
			mTrans = transform;
		}

		protected virtual void OnEnable(){
			if (isPlayOnEnable) {
				ResetToBegin ();
				Play ();
			}
		}

		public virtual bool Play(){
			if(isPlaying){
				return false;
			}
			isPlaying = true;
			mBeginTime = Time.time + delay;
			mTime = 0;
			return true;
		}

		public virtual void ResetToBegin(){
			isPlaying = false;
			mTime = 0;
		}

		bool isPlaying;
		float mTime=0;
		int dir = 1;
		float mBeginTime = 0;
		void Update(){
			if(isPlaying && mBeginTime<=Time.time){
				mTime += Time.deltaTime / duration * dir;
				DoTween (curve.Evaluate(mTime));
				if (isLoop) {
					if (mTime >= 1) {
						dir = -1;
					}
					if (mTime <= 0) {
						dir = 1;
					}
				} else {
					if (mTime >= 1 || mTime <= 0) {
						isPlaying = false;
						if (onTweenDone != null)
							onTweenDone ();
						if (complete != null)
							complete ();
						if (isAutoHide)
							gameObject.SetActive (false);
					} 
				}
			}
		}

		//tween logic
		protected virtual void DoTween(float evaluate){
		
		}
	}
}
