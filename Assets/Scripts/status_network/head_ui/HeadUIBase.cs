using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MMO
{
	public class HeadUIBase : MonoBehaviour
	{
		public TextMeshPro text;
		public SpriteRenderer healthBar;
		float mDefaultHealthSize;
		public MMOUnit mmoUnit;
		Transform mTrans;

		void Awake ()
		{
			if (healthBar != null)
				mDefaultHealthSize = healthBar.size.x;
			mTrans = transform;
		}

		void Update ()
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
			Debug.Log(mmoUnit.unitInfo.attribute.unitName);
			this.mmoUnit = mmoUnit;
			text.text = mmoUnit.unitInfo.attribute.unitName;
			CapsuleCollider capsuleCollider = mmoUnit.GetComponent<CapsuleCollider> ();
			transform.SetParent (mmoUnit.transform);
			transform.localScale = Vector3.one;
			transform.localPosition = new Vector3 (0, capsuleCollider.height, 0);
		}
	}
}
