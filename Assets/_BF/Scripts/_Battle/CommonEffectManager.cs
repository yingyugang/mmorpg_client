using UnityEngine;
using System.Collections;

public class CommonEffectManager :SingleMonoBehaviour<CommonEffectManager>
{

	public GameObject prefabBB;
	public GameObject prefabHC;
	public GameObject prefabSoul;
	public GameObject prefabCoin;
	public GameObject prefabDeathBase;
	public GameObject prefabAfterDeath;
	public GameObject prefabCatch;
	public GameObject prefabToBody;
	public GameObject prefabToBodyGreen;
	public GameObject prefabAddBuff;
	public GameObject prefabMaterial;
	public GameObject prefabHitCommanFire;
	public GameObject prefabHitCommanLight;
	public GameObject prefabHitCommanMagic;
	public GameObject prefabHitCommanWater;
	public GameObject prefabHitCommanWind;
	public GameObject prefabHitCommanWood;

	public GameObject prefabHitCommon;


	public GameObject prefabDefense;
	public GameObject prefabDefenseHit;

	void LoadPrefabs ()
	{
		prefabBB = Resources.Load ("Effect/Effect_Blue_D") as GameObject;
		prefabHC = Resources.Load ("Effect/Effect_RED_D") as GameObject;
		prefabSoul = Resources.Load ("Effect/Effect_Hun") as GameObject;
		prefabCoin = Resources.Load ("Effect/GoldCoin") as GameObject;
		prefabDeathBase = Resources.Load ("Effect/Common_Effect2/Effect_DeathBase") as GameObject;
		prefabAfterDeath = Resources.Load ("Effect/Common_Effect2/Effect_AfterDeath") as GameObject;
		prefabCatch = Resources.Load ("Effect/Common_Effect2/Effect_Catch") as GameObject;
		prefabToBody = Resources.Load ("Effect/Effect_Shuijing_ToBody") as GameObject;
		prefabToBodyGreen = Resources.Load ("Effect/Effect_Shuijing_ToBody_Green") as GameObject;
		prefabAddBuff = Resources.Load ("Effect/Effect_AddBuff") as GameObject;
		prefabMaterial = Resources.Load ("Effect/Effect_Material") as GameObject;
		prefabHitCommanFire = Resources.Load ("Effect/Effect_HitComman_Fire") as GameObject;
		prefabHitCommanLight = Resources.Load ("Effect/Effect_HitComman_Light") as GameObject;
		prefabHitCommanMagic = Resources.Load ("Effect/Effect_HitComman_Magic") as GameObject;
		prefabHitCommanWater = Resources.Load ("Effect/Effect_HitComman_Water") as GameObject;
		prefabHitCommanWind = Resources.Load ("Effect/Effect_HitComman_Wind") as GameObject;
		prefabHitCommanWood = Resources.Load ("Effect/Effect_HitComman_Wood") as GameObject;
		prefabDefense = Resources.Load ("Effect/Common_Effect2/Effect_GuardShield") as GameObject;
		prefabDefenseHit = Resources.Load ("Effect/Common_Effect2/Effect_GuardShield_Hitted") as GameObject;
	}

	public void PreparePools ()
	{
		if (prefabBB != null)
			PoolManager.SingleTon ().AddPool (prefabBB, 30);
		if (prefabHC != null)
			PoolManager.SingleTon ().AddPool (prefabHC, 30);
		if (prefabSoul != null)
			PoolManager.SingleTon ().AddPool (prefabSoul, 30);
		if (prefabCoin != null)
			PoolManager.SingleTon ().AddPool (prefabCoin, 30);
		if (prefabDeathBase != null)
			PoolManager.SingleTon ().AddPool (prefabDeathBase, 10);
		if (prefabAfterDeath != null)
			PoolManager.SingleTon ().AddPool (prefabAfterDeath, 10);
		if (prefabCatch != null)
			PoolManager.SingleTon ().AddPool (prefabCatch, 10);
		if (prefabToBody != null)
			PoolManager.SingleTon ().AddPool (prefabToBody, 30);
		if (prefabToBodyGreen != null)
			PoolManager.SingleTon ().AddPool (prefabToBodyGreen, 30);
		if (prefabAddBuff != null)
			PoolManager.SingleTon ().AddPool (prefabAddBuff, 10);
		if (prefabMaterial != null)
			PoolManager.SingleTon ().AddPool (prefabMaterial, 10);
		if (prefabHitCommanFire != null)
			PoolManager.SingleTon ().AddPool (prefabHitCommanFire, 10);
		if (prefabHitCommanLight != null)
			PoolManager.SingleTon ().AddPool (prefabHitCommanLight, 10);
		if (prefabHitCommanMagic != null)
			PoolManager.SingleTon ().AddPool (prefabHitCommanMagic, 10);
		if (prefabHitCommanWater != null)
			PoolManager.SingleTon ().AddPool (prefabHitCommanWater, 10);
		if (prefabHitCommanWind != null)
			PoolManager.SingleTon ().AddPool (prefabHitCommanWind, 10);
		if (prefabHitCommanWood != null)
			PoolManager.SingleTon ().AddPool (prefabHitCommanWood, 10);
		if (prefabDefense != null)
			PoolManager.SingleTon ().AddPool (prefabDefense, 10);
		if (prefabDefenseHit != null)
			PoolManager.SingleTon ().AddPool (prefabDefenseHit, 10);
		if (prefabHitCommon != null)
			PoolManager.SingleTon ().AddPool (prefabHitCommon, 10);
	}

}
