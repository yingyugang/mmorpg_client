using UnityEngine;
using System.Collections;

namespace Tween{
	public class TweenCurvePosition : TweenBase {

		public Vector3 from;
		public Vector3 to;
		public Vector3 middle;

		protected override void DoTween (float evaluate)
		{
			base.DoTween (evaluate);
			mTrans.localPosition = Curve (from,middle,to,evaluate);
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


		Vector2 Curve(Vector2 start,Vector2 middle,Vector2 end,float t){
			Vector2 pos1 = Vector2.Lerp (start,middle,t);
			Vector2 pos2 = Vector2.Lerp (middle,end,t);
			return Vector2.Lerp (pos1,pos2,t);
		}
	}
}
