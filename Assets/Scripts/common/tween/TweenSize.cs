using UnityEngine;
using System.Collections;

namespace Tween{
	public class TweenSize : TweenBase {

		public Vector3 from;
		public Vector3 to;

		protected override void DoTween (float evaluate)
		{
			base.DoTween (evaluate);
			mTrans.localScale = Vector3.Lerp (from,to,evaluate);
		}

		public override bool Play(){
			if (!base.Play ())
				return false;
			mTrans.localScale = from;
			return true;
		}

		public override void ResetToBegin(){
			base.ResetToBegin ();
			mTrans.localScale = from;
		}
	}
}