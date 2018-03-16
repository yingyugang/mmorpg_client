using UnityEngine;
using System.Collections;

namespace Tween{
	public class TweenPosition : TweenBase {

		public Vector3 from;
		public Vector3 to;

		protected override void DoTween (float evaluate)
		{
			base.DoTween (evaluate);
			mTrans.localPosition = Vector3.Lerp (from,to,evaluate);
		}

		public override bool Play(){
			if (!base.Play ())
				return false;
			mTrans.localPosition = from;
			return true;
		}

		public override void ResetToBegin(){
			base.ResetToBegin ();
			mTrans.localPosition = from;
		}
	}
}
