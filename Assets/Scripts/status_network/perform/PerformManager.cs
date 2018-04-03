using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MMO
{
	public class PerformManager : SingleMonoBehaviour<PerformManager>
	{

		public GameObject hitUITextPrefab;
		public List<GameObject> hitPrefabs;

		//TODO 表示する場合が確認する
		public void ShowHitInfo (HitInfo hitInfo, Dictionary<int,GameObject> unitDic)
		{
			ShowHitEffects (hitInfo);
			ShowHitUIInfos (hitInfo, unitDic);
		}

		void ShowHitEffects (HitInfo hitInfo)
		{
			for (int i = 0; i < hitInfo.hitObjectIds.Length; i++) {
				ShowHitEffect (hitInfo.hitObjectIds [i], hitInfo.hitPositions [i]);
			}
		}

		void ShowHitEffect (int objectId, IntVector3 pos)
		{
			GameObject prefab = this.hitPrefabs [objectId];
			GameObject go = Instantiater.Spawn (false, prefab, IntVector3.ToVector3 (pos), Quaternion.identity);
			Destroy (go, 10);
		}

		void ShowHitUIInfos (HitInfo hitInfo, Dictionary<int,GameObject> unitDic)
		{
			for (int j = 0; j < hitInfo.hitIds.Length; j++) {
				if (unitDic.ContainsKey (hitInfo.hitIds [j])) {
					GameObject go = unitDic [hitInfo.hitIds [j]];
					ShowHitUIInfo (go.GetComponent<MMOUnit> (), hitInfo.nums [j]);
				}
			}
		}

		//TODO change hit ui info color.
		void ShowHitUIInfo (MMOUnit mmoUnit, int damage)
		{
			if (damage != 0) {
				GameObject uiGo = Instantiater.Spawn (false, this.hitUITextPrefab, mmoUnit.GetHeadPos () , Quaternion.identity);
				TextMeshPro textMeshPro = uiGo.GetComponentInChildren<TextMeshPro> (true);
				if (damage > 0) {
					textMeshPro.text = damage.ToString ();
					textMeshPro.color = Color.red;
				} else {
					textMeshPro.text = (-damage).ToString ();
					textMeshPro.color = Color.green;
				}
				uiGo.SetActive (true);
			}
		}

		public void ShowCurrentPlayerDeathEffect (MMOUnit playerUnit)
		{
			ImageEffectManager.Instance.ShowGray ();
		}

		public void HideCurrentPlayerDeathEffect ()
		{
			ImageEffectManager.Instance.HideGray ();
		}

	}
}
