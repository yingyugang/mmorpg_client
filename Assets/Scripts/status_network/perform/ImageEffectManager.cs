using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace MMO
{
	public class ImageEffectManager  : SingleMonoBehaviour<ImageEffectManager>
	{

		public Camera imageEffectCamera;

		protected override void Awake ()
		{
			base.Awake ();
			mGrayscale = imageEffectCamera.gameObject.GetOrAddComponent<Grayscale> (); 
		}

		float mGrayT = 0;
		float mGrayTarget = 0;
		float mGrayDuration = 1;
		bool mGray = false;
		bool mShowGrayWhenCompleted;
		Grayscale mGrayscale;
		public void ShowGray(){
			mGray = true;
			mGrayTarget = 1;
			mShowGrayWhenCompleted = true;
		}

		public void HideGray(){
			mGray = true;
			mGrayTarget = 0;
			mShowGrayWhenCompleted = false;
		}

		void UpdateGray(){
			if (mGray) {
				if (mGrayTarget == mGrayT) {
					mGray = false;
					mGrayscale.enabled = mShowGrayWhenCompleted;
				}else if (mGrayTarget < mGrayT) {
					mGrayT = Mathf.Max (mGrayTarget, mGrayT - Time.deltaTime / mGrayDuration);
					mGrayscale.enabled = true;
				} else {
					mGrayT = Mathf.Min (mGrayTarget, mGrayT + Time.deltaTime / mGrayDuration);
					mGrayscale.enabled = true;
				}
				mGrayscale.lerp = mGrayT;
			}
		}

		void Update(){
			UpdateGray ();
   		}

	}

}