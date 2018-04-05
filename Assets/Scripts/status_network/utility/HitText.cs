using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace MMO
{
	public class HitText : MonoBehaviour
	{
		
		Transform mTrans;
		public float moveHeight = 10;
		public float moveDuration = 2;
		TextMeshPro mTextMeshPro;

		void Awake(){
			mTrans = transform;
			mTextMeshPro = transform.GetComponentInChildren<TextMeshPro> (true);
//			mTrans.DOMove (mTrans.position + new Vector3(0,moveHeight,0),moveDuration ).OnComplete(()=>{
//				Destroy(gameObject);
//			});
//			mTextMeshPro.DOColor (new Color(1,0,0,0),moveDuration);
		}

		void OnEnable(){
			mTrans.forward = Camera.main.transform.forward;
		}

		void Update ()
		{
			mTrans.forward = Camera.main.transform.forward;
		}
	}
}
