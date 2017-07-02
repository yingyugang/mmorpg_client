using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HeroPrefabAndIdMap : MonoBehaviour {

	public List<HeroPrefabAndIdMapping> heroPrefabs;

}

[System.Serializable]
public class HeroPrefabAndIdMapping
{
	public int heroPrefabId;
	public string heroPrefabName;

//	public int CompareTo(object obj) {
//		int result;
//		HeroPrefabAndIdMapping info = obj as HeroPrefabAndIdMapping;
//		if (this.heroPrefabId > info.heroPrefabId)
//        {
//           result =1;
//        }
//        else
//	       result = 0;
//        return result;
//	}
}