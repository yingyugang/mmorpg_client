using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class SkillShoot : SkillBase
	{

		public override bool Play ()
		{
			return base.Play ();
		}

		protected override void OnActive ()
		{
			base.OnActive ();
			PlaySkillEffects ();
		}

		//interface layer.
		public void PlaySkillEffects ()
		{
			GameObject shootPrefab = MMOController.Instance.shootPrefabs [0].gameObject;
			GameObject shootGo = Instantiater.Spawn (false, shootPrefab, mmoUnit.transform.position + new Vector3 (0, 1, 0), mmoUnit.transform.rotation * Quaternion.Euler (60, 0, 0));
			ShootObject so = shootGo.GetComponent<ShootObject> ();
			so.Shoot (mmoUnit, mmoUnit.unitInfo.action.targetPos, Vector3.zero);
		}
	}
}