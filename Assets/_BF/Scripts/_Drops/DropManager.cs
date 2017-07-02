using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropManager  : SingleMonoBehaviour<DropManager>  {

	public List<Drop> drops;

	public void Drop(Unit unit,_DropType type,int dropValue = 0)
	{
		Vector3 pos = unit.unitRes.GetCenterPos ();
		Vector3 offset =  new Vector3(Random.Range(-1f,1f),Random.Range(-2.0f,2.0f),0);
		pos = pos + offset;
		GameObject go = null;
		Drop drop = null;
		if(type == _DropType.BC)
		{
			GameObject prefabBB = CommonEffectManager.GetInstance().prefabBB;
			go = PoolManager.SingleTon().Spawn(prefabBB,pos,Quaternion.identity);
			drop = go.GetComponent<Drop>();
		}
		else if(type == _DropType.HC)
		{
			GameObject prefabHC =  CommonEffectManager.GetInstance().prefabHC;
			go = PoolManager.SingleTon().Spawn(prefabHC,pos,Quaternion.identity);
			drop = go.GetComponent<Drop>();
		}
		else if(type == _DropType.Soul)
		{
			if(dropValue>0)
			{
				go = PoolManager.SingleTon().Spawn(CommonEffectManager.GetInstance().prefabSoul,pos,Quaternion.identity);
				drop = go.GetComponent<Drop>();
				drop.Val = dropValue;
			}
		}
		else if(type == _DropType.Coin)
		{
			if(dropValue>0)
			{
				go = PoolManager.SingleTon().Spawn(CommonEffectManager.GetInstance().prefabCoin,pos,Quaternion.identity);
				drop = go.GetComponent<Drop>();
				drop.Val = dropValue;
			}
		}
		else if(type == _DropType.BattleMaterial)
		{
			if(dropValue>0)
			{
				go = PoolManager.SingleTon().Spawn(CommonEffectManager.GetInstance().prefabMaterial,pos,Quaternion.identity);
				drop = go.GetComponent<Drop>();
				drop.Val = dropValue;
			}
		}

		if(drop!=null)
		{
//			drop.ChangeOrderLayer(target.OrderLayerName,(int)(go.transform.position.y * -100));
			drop.ChangeOrderLayer("SuperLayer0",0);
			drop.OnSpawn(unit.transform.position,offset);
			drop.type = type;
			if(DropManager.GetInstance()!=null){
				DropManager.GetInstance ().drops.Add (drop);
			}
		}
		if (go != null)
			go.transform.localScale = Vector3.one * 2;
	}

	public void Collect()
	{
		foreach(Drop drop in drops)
		{
			drop.targetPos = drop.transform.position + new Vector3(0,5,0);
			float dur = Random.Range(0.5f,1.5f);
			drop.Collect(dur);
		}
	}

}
