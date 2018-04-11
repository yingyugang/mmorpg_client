using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class MiniMapManager  : SingleMonoBehaviour<MiniMapManager>
	{

		Dictionary<string,GameObject> iconPrefabs;
		const string PlayerMiniIcon = "MiniMap/KGFMapIcon_air_friend";
		const string MonsterMiniIcon = "MiniMap/KGFMapIcon_air_hostile";

		public void SetMiniIcon(MMOUnit mmoUnit){
			UnitInfo playerUnit = MMOController.Instance.playerInfo.unitInfo;
			string prefabPath = PlayerMiniIcon;
			if (mmoUnit.unitInfo.camp == playerUnit.camp) {
				prefabPath = PlayerMiniIcon;
			} else {
				prefabPath = MonsterMiniIcon;
			}
			GameObject prefab = LoadIcon (prefabPath);
			GameObject go = Instantiater.Spawn (false,prefab,Vector3.zero,Quaternion.identity);
			go.transform.SetParent (mmoUnit.transform);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
		}

		GameObject LoadIcon(string prefabName){
			if(iconPrefabs==null){
				iconPrefabs = new Dictionary<string, GameObject> ();
			}
			if (iconPrefabs.ContainsKey (prefabName)) {
				return iconPrefabs [prefabName];
			} else {
				GameObject prefab = Resources.Load<GameObject> (prefabName);
				iconPrefabs.Add (prefabName,prefab);
				return prefab;
			}
		}

	}
}
