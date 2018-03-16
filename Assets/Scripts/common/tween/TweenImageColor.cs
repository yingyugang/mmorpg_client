using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tween{
	public class TweenImageColor : TweenBase {

		public Color from = new Color(1,1,1,0);
		public Color to = Color.white;
		public Image targetImage;

		protected override void Awake(){
			base.Awake ();
			if(targetImage == null)
				targetImage = GetComponent<Image> ();
		}

		protected override void DoTween (float evaluate)
		{
			base.DoTween (evaluate);
			targetImage.color = Color.Lerp (from,to,evaluate);
		}

		public override bool Play(){
			if (!base.Play ())
				return false;
			if(targetImage == null)
				targetImage = GetComponent<Image> ();
			targetImage.color = from;
			return true;
		}

		public override void ResetToBegin(){
			base.ResetToBegin ();
			targetImage.color = from;
		}
	}
}