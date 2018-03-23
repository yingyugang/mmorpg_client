using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MMO
{
	public class HeadUIBase : MonoBehaviour
	{
		public GameObject container_head_info;
		public SpriteRenderer healthBar;
		public TextMesh txt_name;

		public GameObject container_bubble;
		public TextMeshPro txt_bubble;

		float mDefaultHealthSize;
		public MMOUnit mmoUnit;
		Transform mTrans;

		void Awake ()
		{
			if (healthBar != null)
				mDefaultHealthSize = healthBar.size.x;
			if(txt_name==null)
				txt_name = GetComponentInChildren<TextMesh> (true);
			if (txt_bubble == null)
				txt_bubble = GetComponentInChildren<TextMeshPro> (true);
			mTrans = transform;
		}

		void LateUpdate ()
		{
			mTrans.forward = Camera.main.transform.forward;
			UpdateHealthBar ();
		}

		float mPreHP;
		float mPreMaxHP;

		void UpdateHealthBar ()
		{
			if (healthBar != null && mmoUnit.unitInfo.attribute.maxHP > 0) {
				if (mPreHP != mmoUnit.unitInfo.attribute.currentHP || mPreMaxHP != mmoUnit.unitInfo.attribute.maxHP) {
					mPreHP = mmoUnit.unitInfo.attribute.currentHP;
					mPreMaxHP = mmoUnit.unitInfo.attribute.maxHP;
					float radio = (float)mmoUnit.unitInfo.attribute.currentHP / mmoUnit.unitInfo.attribute.maxHP;
					healthBar.size = new Vector2 (mDefaultHealthSize * radio, healthBar.size.y);
				}
			}
		}

		public void SetUnit (MMOUnit mmoUnit)
		{
//			Debug.Log(mmoUnit.unitInfo.attribute.unitName);
			this.mmoUnit = mmoUnit;
			txt_name.text = mmoUnit.unitInfo.attribute.unitName;
			CapsuleCollider capsuleCollider = mmoUnit.GetComponent<CapsuleCollider> ();
			transform.SetParent (mmoUnit.transform);
			transform.localScale = Vector3.one;
			transform.localPosition = new Vector3 (0, capsuleCollider.height, 0);
		}

		public void SwitchToBubble(string text){
			container_head_info.SetActive (false);
			container_bubble.SetActive (true);
			txt_bubble.SetText (text);
		}

		public void SwitchToHeadInfo(){
			container_head_info.SetActive (true);
			container_bubble.SetActive (false);
		}

	}
}
