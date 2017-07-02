using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitEffect : MonoBehaviour {

//	public Hero hero;
	public UnitEffectRes unitEffectRes;
	Dictionary<_UnitArtActionType,List<EffectAttr>> effect_maps = null;

	Dictionary<SkinnedMeshRenderer,Material> mMeshRenders;
	Dictionary<SpriteRenderer,Material> mSpriteRenders;
	public List<SkinnedMeshRenderer> skinnedMeshRenderers;
	public List<SpriteRenderer> spriteRenderes;
	public List<Renderer> renderers;
	public List<GameObject> excludeRenderers;
	Dictionary<_AnimType,List<EffectAttr>> effectMaps = null;
	public bool enableHitEffect = true;

	void Awake()
	{
		unitEffectRes = GetComponent<UnitEffectRes> ();
		effect_maps = unitEffectRes.GetEffectDic ();
		InitPlayMeshRenders();
	}

	public void PlayEffects(_UnitArtActionType actionType,List<SkillEffectPartment> partments){
		
	}

	public void PlayEffects(_UnitArtActionType actionType,List<Unit> targetUnits){
		List<GameObject> targets = new List<GameObject>();
		if (targetUnits != null) {
			foreach (Unit unit in targetUnits) {
				targets.Add (unit.gameObject);
			}
		}
		PlayEffects (actionType,targets);
	}

	public void PlayEffects(_UnitArtActionType actionType,List<GameObject> targets = null)
	{
		if(mCurrentEffects!=null)
			StopCurrentEffects();
		mCurrentEffects = effect_maps[actionType];
		foreach(EffectAttr ea in effect_maps[actionType])
		{
			ea.targets = targets;
			PlayEffect(ea);
		}
	}

	List<EffectAttr> mCurrentEffects;
	bool mIsColorChanged = false;
	bool mIsEdgeColorChanged = false;
	bool mIsMaterialChanged = false;

	public void PlayEffects(_AnimType aType,List<Unit> targetUnits)
	{
		List<GameObject> targets = new List<GameObject>();
		foreach(Unit unit in targetUnits){
			targets.Add (unit.gameObject);
		}
		PlayEffects (aType,targets);
	}

	public void PlayEffects(_AnimType aType,List<GameObject> targets = null)
	{
		if(mCurrentEffects!=null)
			StopCurrentEffects();
		mCurrentEffects = effectMaps[aType];
		foreach(EffectAttr ea in effectMaps[aType])
		{
			ea.targets = targets;
			PlayEffect(ea);
		}
	}

	void PlayEffect(EffectAttr ea)
	{
		if(!gameObject.activeInHierarchy){
			return;
		}
		switch(ea.effectType)
		{
		case _EffectType.Cast:StartCoroutine("Cast",ea);break;
		case _EffectType.Shoot:StartCoroutine("Shoot",ea);break;
		case _EffectType.Ghost:StartCoroutine("Ghost",ea);break;
		case _EffectType.ChangeColor:StartCoroutine("ChangeColor",ea);break;
		case _EffectType.EdgeColor:StartCoroutine("ChangeColor",ea);break;
		case _EffectType.ChangeMaterial:StartCoroutine("ChangeMaterial",ea);break;
		}
	}

	public void StopCurrentEffects()
	{
		StopCoroutine("_Ghost");
		StopCoroutine("_PlayEffect");
		foreach(EffectAttr ea in mCurrentEffects)
		{
			if(ea.effectObject!=null && ea.effectObject.activeInHierarchy)
			{
				for(int i=0;i<ea.particleUtilitys.Count;i++)
				{
					ea.particleUtilitys[i].Stop();
				}
				if(ea.particleUtilitys == null || ea.particleUtilitys.Count == 0)
				{
					ea.effectObject.SetActive(false);
				}
			}
		}
		if(mIsColorChanged || mIsEdgeColorChanged || mIsMaterialChanged)
		{
			RevertMaterial();
		}
	}

	public void InitPlayMeshRenders()
	{
//		if(heroResEffect ==null)
//			heroResEffect = GetComponent<HeroResEffect>();
//		if(heroResEffect!=null)
//		{
//			effectMaps = new Dictionary<_AnimType, List<EffectAttr>>();
//			effectMaps.Add(_AnimType.Attack,heroResEffect.attackEffectAttrList);
//			effectMaps.Add(_AnimType.Skill1,heroResEffect.skillEffeectAttrList);
//			effectMaps.Add(_AnimType.StandBy,heroResEffect.standbyEffectAttrList);
//			effectMaps.Add(_AnimType.Run,heroResEffect.runEffectAttrList);
//			effectMaps.Add(_AnimType.Death,heroResEffect.deathEffectAttrList);
//			effectMaps.Add(_AnimType.Hit,heroResEffect.hitEffectAttrList);
//			effectMaps.Add(_AnimType.Cheer,heroResEffect.cheerEffectAttrList);
//			effectMaps.Add(_AnimType.Power,heroResEffect.powerEffectAttrList);
//			effectMaps.Add(_AnimType.Sprint,heroResEffect.sprintEffectAttrList);
//			skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
//			spriteRenderes = new List<SpriteRenderer>();
//			renderers = new List<Renderer>();
//			excludeRenderers = new List<GameObject>();
//			#region 1.Init exclude renderers
//			Transform shadow = heroResEffect.transform.Find("Character_Shadow");
//			ParticleRenderer[] prs = heroResEffect.GetComponentsInChildren<ParticleRenderer>();
//			ParticleSystem [] pss = heroResEffect.GetComponentsInChildren<ParticleSystem>();
//			NcEffectAniBehaviour[] ncs = heroResEffect.GetComponentsInChildren<NcEffectAniBehaviour>();
//			TrailRenderer[] trs = heroResEffect.GetComponentsInChildren<TrailRenderer>();
//			if(shadow!=null && shadow.GetComponent<Renderer>()!=null)
//			{
//				excludeRenderers.Add(shadow.gameObject);
//			}
//			foreach(ParticleRenderer pr in prs)
//			{
//				excludeRenderers.Add(pr.gameObject);
//			}
//			foreach(ParticleSystem pr in pss)
//			{
//				excludeRenderers.Add(pr.gameObject);
//			}
//			foreach(NcEffectAniBehaviour nc in ncs)
//			{
//				excludeRenderers.Add(nc.gameObject);
//			}
//			foreach(TrailRenderer tr in trs)
//			{
//				excludeRenderers.Add(tr.gameObject);
//			}
//			#endregion
//			#region 2.Init spriterenderers and skinnedmeshrenderers except exclude renderers;
//			SpriteRenderer[] spriteRs = heroResEffect.GetComponentsInChildren<SpriteRenderer>();
//			SkinnedMeshRenderer[] skinnedRs = heroResEffect.GetComponentsInChildren<SkinnedMeshRenderer>();;
//			Renderer[] rss = heroResEffect.GetComponentsInChildren<Renderer>();
//			spriteRenderes.AddRange(spriteRs);
//			skinnedMeshRenderers.AddRange(skinnedRs);
//			renderers.AddRange(rss);
//
//			foreach(GameObject go in excludeRenderers)
//			{
//				SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
//				if(sr!=null && spriteRenderes.Contains(sr))
//				{
//					spriteRenderes.Remove(sr);
//				}
//				SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
//				if(smr!=null && skinnedMeshRenderers.Contains(smr))
//				{
//					skinnedMeshRenderers.Remove(smr);
//				}
//				Renderer rs = go.GetComponent<Renderer>();
//				if(rs!=null && renderers.Contains(rs))
//				{
//					renderers.Remove(rs);
//				}
//			}
//			#endregion
//			#region 3.Init renderer mappings;
//			mMeshRenders = new Dictionary<SkinnedMeshRenderer,Material>();
//			foreach(SkinnedMeshRenderer sr in skinnedRs)
//			{
//				mMeshRenders.Add(sr,sr.material);
//			}
//			mSpriteRenders = new Dictionary<SpriteRenderer, Material>();
//			foreach(SpriteRenderer sr in spriteRs)
//			{
//				mSpriteRenders.Add(sr,sr.material);
//			}
//			#endregion
//		}
//	
	}

	IEnumerator Cast(EffectAttr ea)
	{
		List<GameObject> targets = ea.targets;
		yield return new WaitForSeconds(ea.delayTime);
		bool follow = false;
		if(ea.effectTargetType==_EffectTargetType.Self)
		{
			if(ea.effectScopeType==_EffectScopeType.Single)
			{
				targets = new List<GameObject>();
				if(ea.follow != null)
				{
					follow = true;
					targets.Add(ea.follow.gameObject);
				}
				else
				{
					targets.Add(unitEffectRes.gameObject);
				}
			}
			else
			{
				targets = new List<GameObject>();
//				if(TestEffectController.SingleTon()!=null)
//				{
//					if(this.hero.Side == _Side.Player)
//						targets.Add(BattleUtility.GetGlobalPos(_Side.Player).gameObject);
//					else
//						targets.Add(BattleUtility.GetGlobalPos(_Side.Enemy).gameObject);
//				}
			}
		}
		else if(ea.effectTargetType==_EffectTargetType.Target)
		{
			if(ea.effectScopeType==_EffectScopeType.Scope)
			{
				targets = new List<GameObject>();
				if (TestEffectController.SingleTon () != null) {
					targets.Add (TestEffectController.SingleTon ().leftEffectPos.gameObject);
				} else {
					if (this.unitEffectRes.unit.side == _Side.Player)
						targets.Add (BattleUtility.GetGlobalPos (_Side.Enemy).gameObject);
					else
						targets.Add (BattleUtility.GetGlobalPos (_Side.Player).gameObject);
				}

			}
		}
		if(ea.effectPrefab==null)
		{
			GameObject go = Resources.Load(ea.effectName) as GameObject;
			//			Debug.Log("ea.effectName:" + ea.effectName);
			ea.effectPrefab = go;
		}
		for(int i=0;i < ea.frequency;i ++)
		{
			if (targets == null)
				continue;
			foreach(GameObject tmpGo in targets)
			{
				Vector3 castPos  = Vector3.zero;
				if(ea.effectTargetType==_EffectTargetType.Target && ea.effectScopeType == _EffectScopeType.Single)
				{
					HeroRes heroRes = tmpGo.GetComponentInChildren<HeroRes>();
					if(heroRes!=null)
					{
						castPos = heroRes.ShootPoints[Random.Range(0,heroRes.ShootPoints.Count)].TransformPoint(ea.position);
					}
				}
				else
				{
					castPos = tmpGo.transform.TransformPoint(ea.position);
				}
				if(ea.effectPrefab!=null)
				{
					GameObject castGo = PoolManager.SingleTon().Spawn(ea.effectPrefab,castPos,Quaternion.identity) as GameObject;
					//					GameObject castGo = Instantiate(ea.effectPrefab,castPos,Quaternion.identity)  as GameObject;
					//				castGo.transform.eulerAngles = tmpGo.transform.TransformDirection(ea.rotation);
					castGo.transform.eulerAngles = tmpGo.transform.eulerAngles + ea.rotation;
					castGo.transform.localScale = ea.scale;
					if(ea.effectTargetType==_EffectTargetType.Self && ea.follow != null)
					{
//						if(hero!=null)CommonUtility.SetSortingLayerWithChildren(castGo,hero.OrderLayerName);
					}
					if(follow)
					{
						castGo.transform.parent = tmpGo.transform;
					}
				}
				else
				{
					Debug.LogError("effectPrefab is null");
				}

			}
			yield return new WaitForSeconds(ea.interval);
		}
	}

	IEnumerator Shoot(EffectAttr ea)
	{
		List<GameObject> targets = ea.targets;
		if(ea.effectScopeType == _EffectScopeType.Scope)
		{
			if(TestEffectController.SingleTon()!=null)
			{
				targets = new List<GameObject>();
				targets.Add(BattleUtility.GetGlobalPos(_Side.Enemy).gameObject);
			}
		}
		yield return new WaitForSeconds(ea.delayTime);
		if(ea.effectType==_EffectType.Shoot)
		{
			for(int i=0;i<ea.frequency;i++)
			{
				if (targets != null) {
					foreach (GameObject tmpGo in targets) {
						StartCoroutine (_Shoot (tmpGo.transform, ea, Random.Range (0, ea.interval)));
					}
				}
				yield return new WaitForSeconds(ea.interval);
			}
		}
	}

	HeroAnimation heroAnimation = null;
	IEnumerator ChangeMaterial(EffectAttr ea)
	{
		float delay = 0;
		if(ea.isPlayAfterAnim)
		{
			_AnimType at = GetAnimType(ea);
			if(heroAnimation == null)
			{
				heroAnimation = GetComponent<HeroAnimation>();
			}
			string animString = CommonUtility.AnimCilpNameEnumToString(at);
			AnimMapping tempAm = null;
			foreach(AnimMapping am in heroAnimation.heroRes.bodyAnims)
			{
				if(am.clipName == animString)
				{
					tempAm = am;
					break;
				}
			}
			delay = CommonUtility.GetRealAnimLength(tempAm);
		}
		else
		{
			delay = ea.delayTime;
		}
		yield return new WaitForSeconds(delay);
		CommonUtility.ChangeMaterialWithMainTexture(renderers,ea.material);
	}

	IEnumerator Ghost(EffectAttr ea)
	{
		yield return new WaitForSeconds(ea.delayTime);
		StartCoroutine("_Ghost",ea);
	}

	IEnumerator ChangeColor(EffectAttr ea)
	{
		yield return new WaitForSeconds(ea.delayTime);
		float t = 0;
		float value = 0;
		float globalT = 0;
		bool loopAble = true;
		if(ea.effectLoopType == _EffectLoopType.Default || ea.effectLoopType == _EffectLoopType.Loop)
		{
			int frequency = ea.frequency;
			while(frequency>0)
			{
				t = 0;
				if(ea.interval<=0)
				{
					Debug.LogError("The interval can't be less than zero!");
					break;
				}
				else
				{
					while(t<1)
					{
						if(ea.ignoreTimeScale)
						{
							t += RealTime.deltaTime/ea.interval;
						}
						else
						{
							t += Time.deltaTime/ea.interval;
						}
						t = Mathf.Clamp(t,0,1);
						value = ea.curve.Evaluate(t);
						Color c = Color.Lerp(ea.color,ea.toColor,value);
						if(ea.effectType==_EffectType.EdgeColor)
						{
							CommonUtility.ChangeEdgeColor(skinnedMeshRenderers,spriteRenderes,c,ea.edgeSize);
						}
						else if(ea.effectType==_EffectType.ChangeColor)
						{
							CommonUtility.ChangeColor(renderers,c);
						}
						yield return null;
					}
				}
				RevertMaterial();
				frequency --;
			}
		}
		else if(ea.effectLoopType == _EffectLoopType.PingPong)
		{
			int frequency = ea.frequency;
			while(frequency>0)
			{
				if(ea.interval<=0)
				{
					Debug.LogError("The interval can't be less than zero!");
				}
				else
				{
					t=0;
					while(t<1)
					{
						if(ea.ignoreTimeScale)
						{
							t += RealTime.deltaTime/(ea.interval/2);
						}
						else
						{
							t += Time.deltaTime/(ea.interval/2);
						}
						t = Mathf.Clamp(t,0,1);
						value = ea.curve.Evaluate(t);
						Color c = Color.Lerp(ea.color,ea.toColor,value);
						if(ea.effectType==_EffectType.EdgeColor)
						{
							CommonUtility.ChangeEdgeColor(skinnedMeshRenderers,spriteRenderes,c,ea.edgeSize);
						}
						else if(ea.effectType==_EffectType.ChangeColor)
						{
							CommonUtility.ChangeColor(renderers,c);
						}
						yield return null;
					}
					t=0;
					while(t<1)
					{
						if(ea.ignoreTimeScale)
						{
							t += RealTime.deltaTime/(ea.interval/2);
						}
						else
						{
							t += Time.deltaTime/(ea.interval/2);
						}
						t = Mathf.Clamp(t,0,1);
						value = ea.curve.Evaluate(1-t);
						Color c = Color.Lerp(ea.color,ea.toColor,value);
						if(ea.effectType==_EffectType.EdgeColor)
						{
							CommonUtility.ChangeEdgeColor(skinnedMeshRenderers,spriteRenderes,c,ea.edgeSize);
						}
						else if(ea.effectType==_EffectType.ChangeColor)
						{
							CommonUtility.ChangeColor(renderers,c);
						}
						yield return null;
					}
				}
				RevertMaterial();
				frequency --;
			}
		}
	}

	public void _ChangeColor(Color c)
	{
		CommonUtility.ChangeColor(renderers,c);
	}

	public void RevertMaterial()
	{
		foreach(SkinnedMeshRenderer sr in mMeshRenders.Keys)
		{
			sr.material = mMeshRenders[sr];
		}
		foreach(SpriteRenderer sr in mSpriteRenders.Keys)
		{
			sr.material = mSpriteRenders[sr];
		}
	}

	IEnumerator _Shoot(Transform target,EffectAttr ea,float delay = 0)
	{
		yield return new WaitForSeconds(delay);
		//		Debug.Log("Shoot");
		Vector3 pos = ea.follow == null ? ea.position + unitEffectRes.transform.position : ea.follow.position;

//		HeroRes heroRes = target.GetComponent<HeroRes>();
//		if(heroRes == null)
//		{
//			heroRes = target.GetComponentInChildren<HeroRes>();
//		}
//		if(heroRes != null)
//		{
//			Transform shootTarget = BattleUtility.GetRandomShootTarget(heroRes);
//			target = shootTarget;
//		}
		if(ea.effectPrefab==null)
			ea.effectPrefab = Resources.Load<GameObject>(ea.effectName);
		Vector2 targetPos = target.GetComponent<Unit> ().unitRes.GetHitPos();
		if (ea.effectPrefab != null)
		{
			GameObject shootObject = Instantiate(ea.effectPrefab,pos,Quaternion.Euler(ea.rotation)) as GameObject;
			shootObject.transform.localScale = ea.scale;
			Vector2 startPos = shootObject.transform.position;
			float totalT = ea.shootDurtion <=0.01f ? 0.01f : ea.shootDurtion;
			float t = 0;
			float x = Random.Range (0,(targetPos.x - startPos.x) * 2);
			float y = Random.Range (0,(targetPos.y - startPos.y) * 2);
			Vector2 controllPos = new Vector2 (x + startPos.x,y+ startPos.y);
			if(ea.effectShootTrack==_EffectShootTrack.Projection)
			{
				controllPos = new Vector2((startPos.x + targetPos.x)/2,Mathf.Max(startPos.y,targetPos.y) + Random.Range(5,20));
			}
			if(ea.effectShootTrack==_EffectShootTrack.Missle)
			{
				while(t<1)
				{
					t += Time.deltaTime/totalT;
					shootObject.transform.position = Vector2.Lerp(startPos,targetPos,t);
					yield return null;
				}
			}
			else
			{
				while(t<1)
				{
					t += Time.deltaTime/totalT;
					if(ea.effectShootTrack==_EffectShootTrack.Line)
					{
						shootObject.transform.position = Vector2.Lerp(startPos,targetPos,t);
					}
					else if(ea.effectShootTrack==_EffectShootTrack.Curve || ea.effectShootTrack==_EffectShootTrack.Projection)
					{
						shootObject.transform.position = Curve.Bezier2(startPos,controllPos,targetPos,t);
					}
					yield return null;
				}
			}
			Destroy(shootObject);
		}
		if(ea.hitEffectPrefab==null)
			ea.hitEffectPrefab = Resources.Load<GameObject>(ea.hitPrefabName);
		if(ea.hitEffectPrefab!=null)
		{
			GameObject hitGo = PoolManager.SingleTon().Spawn(ea.hitEffectPrefab,targetPos,Quaternion.Euler(ea.rotation)) as GameObject;
			PoolManager.SingleTon().UnSpawn(3,hitGo);
		}
	}

	public _AnimType GetAnimType(EffectAttr ea)
	{
		foreach(_AnimType at in effectMaps.Keys)
		{
			if(effectMaps[at].Contains(ea))
			{
				return at;
			}
		}
		Debug.LogError("EffectAttr " + ea.ToString() + " is not exit!");
		return _AnimType.StandBy;
	}

	public void BakeRenders()
	{
		SampleRenderer(skinnedMeshRenderers,spriteRenderes);
	}

	public void SampleRenderer(List<SkinnedMeshRenderer> skinRs,List<SpriteRenderer> spriteRs)
	{
		for(int i = 0;i < skinRs.Count;i++)
		{
			GameObject frameGO = new GameObject();
			frameGO.hideFlags = HideFlags.HideInHierarchy;
			Mesh frameMesh = new Mesh();
			skinRs[i].BakeMesh(frameMesh);
			frameMesh.name = skinRs[i].gameObject.name;
			frameGO.transform.position = skinRs[i].transform.position;
			frameGO.transform.eulerAngles = skinRs[i].transform.eulerAngles;
			MeshFilter meshFilter = frameGO.AddComponent<MeshFilter>();
			meshFilter.mesh = frameMesh;
			MeshRenderer sr = frameGO.AddComponent<MeshRenderer>();
			sr.sortingLayerName = skinRs[i].sortingLayerName;
			sr.sortingOrder = skinRs[i].sortingOrder;
			sr.material = new Material(skinRs[i].material);
			StartCoroutine(_HideMesh(meshFilter));
		}
		for(int i = 0;i < spriteRs.Count ; i ++)
		{
			GameObject frameGO = new GameObject();
			frameGO.hideFlags = HideFlags.HideInHierarchy;
			frameGO.transform.position = spriteRs[i].transform.position;
			frameGO.transform.eulerAngles = spriteRs[i].transform.eulerAngles;
			SpriteRenderer rs = frameGO.AddComponent<SpriteRenderer>();
			rs.sprite = spriteRs[i].sprite;
			rs.sortingLayerName = spriteRs[i].sortingLayerName;
			rs.sortingOrder = spriteRs[i].sortingOrder;
			StartCoroutine(_HideSprite(rs));
		}
	}

	IEnumerator _HideMesh(MeshFilter meshFilter)
	{
		Mesh mesh = meshFilter.mesh;
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime * 2;
			Color[] cols = mesh.colors;
			for(int i=0;i<cols.Length;i++)
			{
				cols[i] = new Color(cols[i].r,cols[i].g,cols[i].b,1-t);
			}
			mesh.colors = cols;
			mesh.RecalculateNormals();
			yield return null;
		}
		Destroy(meshFilter.gameObject);
	}

	IEnumerator _HideSkinned(MeshRenderer sr)
	{
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime * 2;
			sr.material.SetFloat("_Alpha",1-t);
			yield return null;
		}
		Destroy(sr.gameObject);
	}

	IEnumerator _HideSprite(SpriteRenderer sr)
	{
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime * 2;
			sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,1-t);
			yield return null;
		}
		Destroy(sr.gameObject);
	}

}
