using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DropUtility
{
	public static void Collect(List<Drop> drops,List<Hero> heros)
	{
		List<Hero> tmpheros = new List<Hero>();
		for (int i = 0; i < heros.Count; i++)
		{
			if (heros[i] != null && heros[i].gameObject != null && heros[i].gameObject.activeSelf)
				tmpheros.Add(heros[i]);
		}

		if (tmpheros.Count == 0)
			return;

		foreach(Drop drop in drops)
		{
			switch(drop.type)
			{
				case _DropType.BC:
					//drop.TargetT=heros[Random.Range(0,heros.Count)].transform;
					drop.TargetT=tmpheros[Random.Range(0,tmpheros.Count)].transform;
					break;
				case _DropType.HC:
					//drop.TargetT=heros[Random.Range(0,heros.Count)].transform;
					drop.TargetT=tmpheros[Random.Range(0,tmpheros.Count)].transform;
					break;
				case _DropType.Soul:
					drop.TargetT=BattleController.SingleTon().TopPoints[1];
					break;
				case _DropType.Coin:
					drop.TargetT=BattleController.SingleTon().TopPoints[0];
					break;
				case _DropType.BattleMaterial:
					drop.TargetT = null;
					drop.targetPos = drop.transform.position + new Vector3(0,5,0);
//					drop.TargetT=tmpheros[Random.Range(0,tmpheros.Count)].transform;
					break;
			}

			float dur = Random.Range(0.5f,1.5f);
			drop.Collect(dur);
		}
//		drops.Clear();
	}
}


