using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class BulletGroup : MonoBehaviour
	{
		public GameObject bulletPrefab;
		public int maxBullet;
		public int currentBulletIndex;
		List<GameObject> mBullets;

		void Awake ()
		{
			mBullets = new List<GameObject> ();
		}

		public void SetWeapon (int maxBullet)
		{
			this.maxBullet = maxBullet;
			Reload ();
		}

		public void Reload ()
		{
			for (int i = 0; i < maxBullet; i++) {
				if (mBullets.Count <= i) {
					GameObject go = Instantiate (bulletPrefab);
					go.transform.SetParent (transform);
					go.transform.localEulerAngles = Vector3.zero;
					go.transform.localScale = Vector3.one;
					go.SetActive (true);
					mBullets.Add (go);
				} else {
					mBullets [i].gameObject.SetActive (true);
				}
			}
			currentBulletIndex = maxBullet - 1;
		}

		public void Shoot ()
		{
			if (currentBulletIndex >= 0 && currentBulletIndex < maxBullet) {
				mBullets [currentBulletIndex].SetActive (false);
			}
		}

	}
}
