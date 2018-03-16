using UnityEngine;
using System.Collections;

namespace Tween{
	public class TweenRotate : TweenBase {

		public Vector3 from;
		public Vector3 to;

		Quaternion mFromQua ;
		Quaternion mToQua;

		public override bool Play(){
			if (!base.Play ())
				return false;
			mFromQua = Quaternion.Euler (from);
			mToQua = Quaternion.Euler (to);
			mTrans.rotation = mFromQua;
			return true;
		}

		protected override void DoTween(float evaluate){
			base.DoTween (evaluate);
			mTrans.rotation = Quaternion.Lerp (mFromQua,mToQua,evaluate);
		}

		public override void ResetToBegin(){
			base.ResetToBegin ();
			mFromQua = Quaternion.Euler (from);
			mToQua = Quaternion.Euler (to);
			mTrans.rotation = mFromQua;
		}
	}
}
