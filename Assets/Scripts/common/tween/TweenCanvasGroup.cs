using UnityEngine;
using System.Collections;

namespace Tween{
	public class TweenCanvasGroup : TweenBase {

		public float from = 0;
		public float to = 1;
		public CanvasGroup target;

		protected override void Awake(){
			base.Awake ();
			if(target == null)
				target = GetComponent<CanvasGroup> ();
		}

		protected override void DoTween (float evaluate)
		{
			base.DoTween (evaluate);
			target.alpha = Mathf.Lerp (from,to,evaluate);
		}

		public override bool Play(){
			if (!base.Play ())
				return false;
			if(target == null)
				target = GetComponent<CanvasGroup> ();
			target.alpha = from;
			return true;
		}

		public override void ResetToBegin(){
			base.ResetToBegin ();
			target.alpha = from;
		}
	}
}