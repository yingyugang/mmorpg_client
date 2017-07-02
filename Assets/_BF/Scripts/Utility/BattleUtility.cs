using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public static class BattleUtility {

	public static Dictionary<_ElementType,RestrainRelation> Relations;

	public static List<HeroAttr> GlobalTempHeroData;
//	public static Dictionary<string,HeroAttr> GlobalTempHeroDataMappings;
	public static Dictionary<int,HeroAttr> GlobalTempHeroDic;

	static public float commonRowDelay = 0.01f;
	static public float commonColumnDelay = 0.18f;
	static public Vector3 BtnShakeOffset = new Vector3(0.03f,0.01f,0);


	public static bool CheckBattleFailure(){
		bool isFailure = true;
		for(int i=0;i< BattleManager.GetInstance().rightUnits.Count;i++){
			if (BattleManager.GetInstance ().rightUnits [i].GetAttribute(EffectType.HP) > 0) {
				isFailure = false;
				break;
			}
		}
		return isFailure;
	}

	public static bool CheckBattleSuccess(){
		bool isSuccess = true;
		for(int i=0;i< BattleManager.GetInstance().leftUnits.Count;i++){
			if (BattleManager.GetInstance ().leftUnits [i].GetAttribute(EffectType.HP)  > 0) {
				isSuccess = false;
				break;
			}
		}
		return isSuccess;
	}

	public static void Drop(AttackAttribute attackAttribute,Hero attacker,Hero attackTarget,int i)
	{
		int dropNum; 
		if(attackTarget.Side == _Side.Enemy || BattleSimpleController.SingleTon()!=null)
		{
			//Debug.Log("attackAttribute.impactNum:" + attackAttribute.impactNum);
			if(attackAttribute.impactNum==1)
			{
				if(i==0)
					dropNum = Random.Range(0,2) == 0 ? 1 : 0;
				else
					dropNum = Random.Range(0,10) == 0 ? 1 : 0;
				if(dropNum==1)
					_DropOut(attackTarget,_DropType.HC);//HC

				if(i==0)
					dropNum = Random.Range(0,2) == 0 ? 2 : 1;
				else
					dropNum = Random.Range(0,10) < 4 ? 1 : 0;

				dropNum = Mathf.FloorToInt(dropNum * attacker.heroAttribute.additionBBDrop);
				for(int z = 0;z < dropNum;z++)
				{
					//Debug.Log("BC.......................NUM...................." + dropNum);

					_DropOut(attackTarget,_DropType.BC);//BC
				}

				if (i == 0)
					dropNum = Random.Range(0,2) == 0 ? 1 : 0;
				else
					dropNum = Random.Range(0,10) == 0 ? 1 : 0;
				if (dropNum == 1)
					_DropOut(attackTarget,_DropType.BattleMaterial);

			}
			else if(attackAttribute.impactNum > 1)
			{
				if(i==0)
					dropNum = Random.Range(0,4) == 0 ? 1 : 0;
				else
					dropNum = Random.Range(0,20) == 0 ? 1 : 0;
				if(dropNum==1)
					_DropOut(attackTarget,_DropType.HC);//HC

				if(i==0)
					dropNum = Random.Range(0,4) == 0 ? 2 : 1;
				else
					dropNum = Random.Range(0,10) < 2 ? 1 : 0;

				dropNum = Mathf.FloorToInt(dropNum * attacker.heroAttribute.additionBBDrop);
				for(int z = 0;z < dropNum;z++)
				{
					//Debug.Log("BC.......................NUM...................." + dropNum);
					_DropOut(attackTarget,_DropType.BC);//BC
				}
			}
			//TODO should amend the drop of coin and soul
			if(attackTarget.heroAttribute.coinCount > 0)
			{
				bool drop = Random.Range(0,2) == 0 ? true : false;
				if(drop)
				{
					_DropOut(attackTarget,_DropType.Coin,attackTarget.heroAttribute.coinPerDrop);//Coin
					attackTarget.heroAttribute.coinCount --;
				}
			}
			if(attackTarget.heroAttribute.soulCount > 0)
			{
				bool drop = Random.Range(0,2) == 0 ? true : false;
				if(drop)
				{
					_DropOut(attackTarget,_DropType.Soul,attackTarget.heroAttribute.soulPerDrop);//Soul
					attackTarget.heroAttribute.soulCount --;
				}
			}
		}				
	}

	static void _DropOut(Hero target,_DropType type,int dropValue = 0)
	{
		Vector3 pos = BattleUtility.GetCenterPos (target);
		Vector3 offset =  new Vector3(Random.Range(-1f,1f),Random.Range(-2.0f,2.0f),0);
		pos = pos + offset;
		GameObject go = null;
		Drop drop = null;
		if(type == _DropType.BC)
		{
//			GameObject prefabBB = BattleController.SingleTon()!=null ? BattleController.SingleTon().prefabBB : BattleSimpleController.SingleTon().prefabBB;
			GameObject prefabBB = CommonEffectManager.GetInstance().prefabBB;
			go = PoolManager.SingleTon().Spawn(prefabBB,pos,Quaternion.identity);
			drop = go.GetComponent<Drop>();
		}
		else if(type == _DropType.HC)
		{
//			GameObject prefabHC = BattleController.SingleTon()!=null ? BattleController.SingleTon().prefabHC : BattleSimpleController.SingleTon().prefabHC;
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
			drop.ChangeOrderLayer(target.OrderLayerName,(int)(go.transform.position.y * -100));
			drop.OnSpawn(target.transform.position,offset);
			drop.type = type;
			if(BattleController.SingleTon()!=null)
			{
				BattleController.SingleTon().Drops.Add(drop);
			}
			else
			{
				BattleSimpleController.SingleTon().Drops.Add(drop);
			}
		}
	}

	static public float GetDelayByLocation(Hero attacker)
	{
		float delayRow = attacker.Index * commonRowDelay;
		float delayColumn = attacker.Index / 3 > 0 ? commonColumnDelay : 0;
		return delayRow + delayColumn ;
	}

	static public GameObject healthBarPrefab;
	static public void InitHeroHealthBars()
	{
		for(int i=0;i < BattleController.SingleTon().Waves.Count;i++)
		{
			foreach(Hero hero in BattleController.SingleTon().Waves[i].heros)
			{
                hero.healthBar = InitHeroHealthBar(hero);
			}
		}
	}

	static public UISlider InitHeroHealthBar(Hero hero)
	{
		if(healthBarPrefab==null)
		{
			healthBarPrefab = Resources.Load<GameObject>("HealthBar") as GameObject;
		}
		GameObject healthBarGo = GameObject.Instantiate(healthBarPrefab) as GameObject;
		UISlider uiSlider = healthBarGo.GetComponent<UISlider>();
		healthBarGo.transform.parent = BattleController.SingleTon().UICamera.transform;
		Transform eleType = healthBarGo.transform.FindChild("ElementType");
		UISprite eleSprite = eleType.GetComponent<UISprite>();
		eleSprite.spriteName = BattleUtility.GetElementSpriteName(hero.heroAttribute.elementType);
		return uiSlider;
	}

	static public void InitBattlePlayerHeros()
	{
		for(int i = 0; i < BattleController.SingleTon().RightHeroes.Count ;i ++)
		{
			Hero hero = BattleController.SingleTon().RightHeroes[i];
			hero.gameObject.SetActive(true);
			hero.SetControllBtn(BattleController.SingleTon().PlayerBattleButtons[i]);
				hero.transform.position = BattleController.SingleTon().RightPoints[hero.Index].position + new Vector3(20,0,0); 
			hero.heroAttack.defaultPos = BattleController.SingleTon().RightPoints[hero.Index].position; 
			hero.Status = _HeroStatus.BeforeTurn;
		}

		for (int i = BattleController.SingleTon().RightHeroes.Count; i < BattleController.SingleTon().PlayerBattleButtons.Count; i ++)
		{
			HeroBtn heroBtn = BattleController.SingleTon().PlayerBattleButtons[i];
			heroBtn.gameObject.SetActive(false);

		}
	}

	static public void ShowRelationHints()
	{
		foreach(Hero hero in BattleController.SingleTon().RightHeroes)
		{
			float relation = BattleUtility.GetRestrainRelation(hero.heroAttribute.elementType,BattleController.SingleTon().HandleTarget.heroAttribute.elementType);
			if(relation > 1)
			{
				hero.Btn.ShowHintUp();
			}
			else if(relation < 1)
			{
				hero.Btn.ShowHintDown();
			}
			else
			{
				hero.Btn.HideHintUp();
				hero.Btn.HideHintDown();
			}
		}
	}

	public static Dictionary<string,Sprite> BuffSprites = new Dictionary<string, Sprite>();

	public static bool IsAttackable()
	{
		if(BattleController.SingleTon().GlobalSkill.IsPlaying || BattleController.SingleTon().IsPotioning)
		{
			return false;
		}
		return true;
	}
	
	static public int GetHCPoint(Hero hero)
	{
		float point =  0.35f * hero.heroAttribute.baseRecover;
//		point = point * Random.Range(0.9f,1.1f);
		//Debug.Log("GetHCPoint......................................." + point);
		point = point * hero.heroAttribute.additionHCRevert;
		//Debug.Log("GetHCPoint......................................." + point);
		//Debug.Log("hero.heroAttribute.additionHCRevert......................................." + hero.heroAttribute.additionHCRevert);
		return (int)point;
	}

	static public int GetHealPotionPoint(Hero hero,PotionAttr potAttr)
	{
		float point = potAttr.Power + 1.1f * hero.BaseRevert;
        Debug.Log("Potion heal:" + point);
//		point = point * Random.Range(0.9f,1.1f);
		return (int)point;
	}

	static public int GetHealBuffPoint(Hero hero,BuffAttribute ba)
	{
		float point = ba.baseNum + ba.power * hero.heroAttribute.currentRecover;
		return (int)point;
	}

	static public int GetRealDamage(Hero atk,Hero def,bool isCrit,float relation,bool isCombo = false)
	{
		float critRatio = isCrit ? 0.5f : 0;
		float floatRatio = Random.Range(1,1);
		float comboRatio = isCombo ? 1.5f + atk.heroAttribute.additionCombo : 1;
		//relation 属性相克关系 , floatRatio 浮动系数, critRatio 暴击系数, additionCritical 技能暴击加成,currentSkillDamageRatio技能伤害系数
		float realDamage = (atk.heroAttribute.currentDamage - def.heroAttribute.currentDefence / 3.0f) * 
			atk.heroAttribute.currentSkillDamageRatio * relation * (1 + critRatio * atk.heroAttribute.additionCritical)
			* comboRatio * floatRatio; 
		//降低伤害
		realDamage = realDamage * def.heroAttribute.decreaseDamage;
		return Mathf.Max(1,Mathf.RoundToInt(realDamage));
	}

	static public float GetElementRelation(int atkType,int defType)
	{
		return atkType == defType ? 1.2f : 1;
	}

	static public float GetRestrainRelation(_ElementType atkType,_ElementType defType)
	{
		if(Relations==null)
		{
			InitRelations();
		}
		float relation = 1;
		RestrainRelation r = Relations[atkType];
		if(r.NextEle == defType)
		{
			relation = 1.5f;
		}
		else if(r.PreEle == defType)
		{
			relation = 0.67f;
		}
		return relation;
	}

	static public float GetRestrainRelationBySkill(List<_ElementType> atkTypeList,_ElementType defType)
	{
		if(Relations==null)
		{
			InitRelations();
		}

		float relation = 1;
		foreach(_ElementType atkType in atkTypeList)
		{
			RestrainRelation r = Relations[atkType];
			if(r.NextEle == defType)
			{
				relation = 1.5f;
				return relation;
			}
			else if(r.PreEle == defType)
			{
				relation = 0.67f;
			}
		}

		//Debug.Log("......................relation............................." + relation);
		return relation;
	}

	static public Vector3 GetNGUIPosFromWorldPos(Vector3 worldPos)
	{
		BattleSimpleController simpleController = BattleSimpleController.SingleTon();
		BattleController controller = BattleController.SingleTon();
		if(simpleController!=null){
			Vector3 pos = simpleController.BattleCamera.WorldToScreenPoint(worldPos);
			pos = simpleController.UICamera.ScreenToWorldPoint(pos);
			return pos;
		}else{
			Vector3 pos = controller.BattleCamera.WorldToScreenPoint(worldPos);
			pos = controller.UICamera.ScreenToWorldPoint(pos);
			return pos;
		}
	}

	static Dictionary<_ElementType, string> elementTypeSprite;
	static Dictionary<_ElementType,string> elementBigTypeSprite;

	static public string GetElementBigSpriteName(_ElementType eleType)
	{
		//Debug.Log(eleType);
		if(elementBigTypeSprite==null)
		{
			InitBigHeroType();
		}
		if(elementBigTypeSprite.ContainsKey(eleType))
		{
			return elementBigTypeSprite[eleType];
		}
		else
		{
			return "";
		}
	}

	static public string GetElementSpriteName(_ElementType eleType)
	{
		//Debug.Log(eleType);
		if(elementTypeSprite==null)
		{
			InitHeroType();
		}
		if(elementTypeSprite.ContainsKey(eleType))
		{
			return elementTypeSprite[eleType];
		}
		else
		{
			return "";
		}
	}

	static Dictionary<int, string> starSprite;
	static public string GetStarSpriteName(int star)
	{
		//Debug.Log(eleType);
		if(starSprite==null)
		{
			InitStarSprites();
		}
		if(starSprite.ContainsKey(star))
		{
			return starSprite[star];
		}
		else
		{
			return "";
		}
	}

	static void InitStarSprites()
	{
		starSprite = new Dictionary<int, string>();
		starSprite.Add(1,"FgtSummonStarBar1");
		starSprite.Add(2,"FgtSummonStarBar2");
		starSprite.Add(3,"FgtSummonStarBar3");
		starSprite.Add(4,"FgtSummonStarBar4");
		starSprite.Add(5,"FgtSummonStarBar5");
		starSprite.Add(6,"FgtSummonStarBar6");
	}

	static void InitBigHeroType()
	{
		elementBigTypeSprite = new Dictionary<_ElementType, string>();
		elementBigTypeSprite.Add(_ElementType.Wind,"Air3");
		elementBigTypeSprite.Add(_ElementType.Wood,"Earth3");
		elementBigTypeSprite.Add(_ElementType.Water,"Water3");
		elementBigTypeSprite.Add(_ElementType.Fire,"Fire3");
		elementBigTypeSprite.Add(_ElementType.Holy,"Light3");
		elementBigTypeSprite.Add(_ElementType.Evil,"Shadow3");
	}

	static void InitHeroType()
	{
		elementTypeSprite = new Dictionary<_ElementType, string>();
		elementTypeSprite.Add(_ElementType.Wind,"Air2");
		elementTypeSprite.Add(_ElementType.Wood,"Earth2");
		elementTypeSprite.Add(_ElementType.Water,"Water2");
		elementTypeSprite.Add(_ElementType.Fire,"Fire2");
		elementTypeSprite.Add(_ElementType.Holy,"Light2");
		elementTypeSprite.Add(_ElementType.Evil,"Shadow2");
	}

	static public void InitRelations()
	{
		Relations = new Dictionary<_ElementType, RestrainRelation>();
		RestrainRelation rel = new RestrainRelation();
		rel.Ele = _ElementType.Fire;
		rel.PreEle = _ElementType.Water;
		rel.NextEle = _ElementType.Wood;
		Relations.Add(rel.Ele,rel);
		
		rel = new RestrainRelation();
		rel.Ele = _ElementType.Water;
		rel.PreEle = _ElementType.Wind;
		rel.NextEle = _ElementType.Fire;
		Relations.Add(rel.Ele,rel);
		
		rel = new RestrainRelation();
		rel.Ele = _ElementType.Wind;
		rel.PreEle = _ElementType.Wood;
		rel.NextEle = _ElementType.Water;
		Relations.Add(rel.Ele,rel);
		
		rel = new RestrainRelation();
		rel.Ele = _ElementType.Wood;
		rel.PreEle = _ElementType.Fire;
		rel.NextEle = _ElementType.Wind;
		Relations.Add(rel.Ele,rel);
		
		rel = new RestrainRelation();
		rel.Ele = _ElementType.Holy;
		rel.PreEle = _ElementType.Evil;
		rel.NextEle = _ElementType.Evil;
		Relations.Add(rel.Ele,rel);
		
		rel = new RestrainRelation();
		rel.Ele = _ElementType.Evil;
		rel.PreEle = _ElementType.Holy;
		rel.NextEle = _ElementType.Holy;
		Relations.Add(rel.Ele,rel);
	}

	static public List<Hero> GetCalculateTargets(List<Hero> heros)
	{
		List<Hero> result = new List<Hero>();
		result.AddRange(heros);
		Hero tmp = null;
		for(int i = 0;i < result.Count;i ++)
		{
			tmp = result[i];
			if(tmp.heroAttribute.calculateHP<=0)
			{
				result.Remove(tmp);
				result.Add(tmp);
			}
//			int index = Random.Range(0,result.Count);
//			tmp = result[i];
//			result[i] = result[index];
//			result[index] = tmp;
		}
		return result;
	}

	static public List<Hero> GetRandomTargets(List<Hero> heros)
	{
		List<Hero> result = new List<Hero>();
		result.AddRange(heros);
		Hero tmp = null;
		for(int i = 0;i < result.Count;i ++)
		{
			int index = Random.Range(0,result.Count);
			tmp = result[i];
			result[i] = result[index];
			result[index] = tmp;
		}
		return result;
	}

	static public List<int> GetEnemyIds(int currentId)
	{
		List<int> list = new List<int>();
		GetPow(currentId,list);
		return list;
	}
	
	static void GetPow(int value,List<int> list)
	{
		int i = 0;
		while(value > 0)
		{
			int test =  1 << i;
			if(test < value)
			{
				i ++;
			}else if(test == value)
			{
				list.Add(test);
				break;
			}
			else
			{
				list.Add(1<<(i-1));
				value -= 1<<(i-1);
				if(value>1)
				{
					GetPow(value,list);
				}else{
					list.Add(value);
				}
				break;
			}
		}
	}

	static public Vector3 GetScale(Transform t)
	{
		Vector3 scale = t.localScale;
		if(t.parent!=null)
		{
			Vector3 parentScale = GetScale(t.parent);
			scale =  new Vector3(scale.x * parentScale.x,scale.y * parentScale.y,1);
		}
		return scale;
	}

	static public void ShowCritAndRelationDamageBeat(float shakeRadius, int damage,Vector3 pos,float scale){
		string text = "Crit && Relation ";
		ShowDamageBeat (shakeRadius, damage, pos, scale, new Color(0.8f,0,0.2f), text);
	}

	static public void ShowRelationDamageBeat(float shakeRadius, int damage,Vector3 pos,float scale){
		string text = "Relation ";
		ShowDamageBeat (shakeRadius, damage, pos, scale, Color.yellow, text);
	}

	static public void ShowCritDamageBeat(float shakeRadius, int damage,Vector3 pos,float scale){
		string text = "Crit ";
		ShowDamageBeat (shakeRadius, damage, pos, scale, Color.red, text);
	}

	static public void ShowDamageBeat(float shakeRadius, int damage,Vector3 pos,float scale)
	{
		ShowDamageBeat (shakeRadius,damage,pos,scale,Color.white,"");
	}

	static public void ShowHealthBeat(float shakeRadius, int damage,Vector3 pos,float scale)
	{
		ShowDamageBeat (shakeRadius,damage,pos,scale,Color.green,"");
	}

	static public void ShowDamageBeat(float shakeRadius, int damage,Vector3 pos,float scale,Color color,string str)
	{
		GameObject prefab = BattleController.SingleTon()!=null ? BattleController.SingleTon().prefabUIDamage : BattleSimpleController.SingleTon().prefabUIDamage;
		GameObject go = PoolManager.SingleTon().Spawn(prefab,pos,Quaternion.identity);
//		GameObject go = GameObject.Instantiate(prefab,pos,Quaternion.identity);
		go.transform.localScale = Vector3.one;
		UILabel label = go.GetComponentInChildren<UILabel>(true);
		label.fontSize = (int)(15 * scale);
		label.text = str + (Mathf.Abs(damage)).ToString();
//		iTween.ShakePosition(go,new Vector3(shakeRadius,0,0) ,0.3f);
		label.color = color;
	}

	static public void ShowEnhaceEffect(Hero hero,int point)
	{
		Vector3 offset;
		Vector3 hitPos = BattleUtility.GetHitPos(hero);
		offset = new Vector3(Random.Range(-0.1f,0.1f),Random.Range(1f,1f),0);
		Vector3 pos = BattleUtility.GetNGUIPosFromWorldPos(hitPos + offset);
		BattleUtility.ShowDamageBeat(HeroAttribute.shakeRadius,point,pos,1);
	}

	static public void ShowCritEffect(Vector3 pos)
	{
		GameObject prefab = BattleController.SingleTon()!=null ? BattleController.SingleTon().prefabUICrit : BattleSimpleController.SingleTon().prefabUICrit;
		GameObject go = PoolManager.SingleTon().Spawn(prefab,pos,Quaternion.identity);
		go.transform.localScale = Vector3.one;
		PoolManager.SingleTon().UnSpawn(2,go);
	}
	
	static public void ShowMultiEffect(Vector3 pos)
	{
		GameObject prefab = BattleController.SingleTon()!=null ? BattleController.SingleTon().prefabUIMul : BattleSimpleController.SingleTon().prefabUIMul;
		GameObject go =  PoolManager.SingleTon().Spawn(prefab,pos,Quaternion.identity);
		go.transform.localScale = Vector3.one;
		PoolManager.SingleTon().UnSpawn(2,go);
	}
	
	static public void ActiveAreanHeros(List<Hero> heros,List<HeroBtn> heroBtns,List<Transform> heroPoints)
	{
		for(int i=0;i<heros.Count;i++)
		{
			int index = heros[i].Index;
			Hero hero = heros[i];
			hero.transform.position = heroPoints[index].position;
			hero.heroAttribute.coinCount = 0;
			hero.heroAttribute.soulCount = 0;
			hero.SetControllBtn(heroBtns[index]);
			hero.heroAttack.defaultPos = heroPoints[index].position;
			hero.Status = _HeroStatus.BeforeTurn;
			hero.gameObject.SetActive(true);
		}
	}

	static public Transform GetRandomShootTarget(HeroRes heroRes)
	{
		if(heroRes.ShootPoints!=null && heroRes.ShootPoints.Count>0)
		{
			int index = Random.Range(0,heroRes.ShootPoints.Count);
			return heroRes.ShootPoints[index];
		}
		else
		{
			return heroRes.transform;
		}
	}

	static public Transform GetGlobalPos(_Side side)
	{
		if(BattleController.SingleTon()!=null)
		{
			return GetBattleControllerGlobalPos(side);
		}
		else if(BattleSimpleController.SingleTon()!=null)
		{
			return GetBattleSimpleControllerGlobalPos(side);
		}
		else if(BattleManager.GetInstance()!=null)
		{
			return GetBattleManagerGlobalPos(side);
		}
		return null;
	}

	static public Transform GetBattleManagerGlobalPos(_Side side)
	{
		Transform posT = null;
		switch(side)
		{
		case _Side.Player: posT = BattleManager.GetInstance().rightPosT;break;
		case _Side.Enemy: posT = BattleManager.GetInstance().leftPosT;break;
		}
		return posT;
	}

	static public Transform GetBattleControllerGlobalPos(_Side side)
	{
		Transform posT = null;
		switch(side)
		{
			case _Side.Player: posT = BattleController.SingleTon().rightPosT;break;
			case _Side.Enemy: posT = BattleController.SingleTon().leftPosT;break;
		}
		return posT;
	}
	
	static public Transform GetBattleSimpleControllerGlobalPos(_Side side)
	{
		Transform posT = null;
		switch(side)
		{
			case _Side.Player: posT = BattleSimpleController.SingleTon().rightPosT;break;
			case _Side.Enemy: posT = BattleSimpleController.SingleTon().leftPosT;break;
		}
		return posT;
	}
	
	static public Transform GetTestEffectContollerGlobalPos(_Side side)
	{
		Transform posT = null;
		switch(side)
		{
			case _Side.Player: posT = TestEffectController.SingleTon().rightEffectPos;break;
			case _Side.Enemy: posT = TestEffectController.SingleTon().leftEffectPos;break;
		}
		return posT;
	}

	static public void InitEffectDictionary(out Dictionary<_AnimType,List<EffectAttr>> effectMaps,HeroResEffect heroResEffect)
	{
		effectMaps = new Dictionary<_AnimType,List<EffectAttr>>();
		effectMaps.Add(_AnimType.Attack,heroResEffect.attackEffectAttrList);
		effectMaps.Add(_AnimType.Skill1,heroResEffect.skillEffeectAttrList);
		effectMaps.Add(_AnimType.StandBy,heroResEffect.standbyEffectAttrList);
		effectMaps.Add(_AnimType.Run,heroResEffect.runEffectAttrList);
		effectMaps.Add(_AnimType.Death,heroResEffect.deathEffectAttrList);
		effectMaps.Add(_AnimType.Hit,heroResEffect.hitEffectAttrList);
		effectMaps.Add(_AnimType.Cheer,heroResEffect.cheerEffectAttrList);
		effectMaps.Add(_AnimType.Power,heroResEffect.powerEffectAttrList);
		effectMaps.Add(_AnimType.Sprint,heroResEffect.sprintEffectAttrList);
	}

	public static void SetHeroResEffectInitValue(HeroResEffect heroResEffect)
	{
		EffectAttr ea = new EffectAttr();
		ea.effectType = _EffectType.ChangeColor;
		ea.delayTime = 0;
		ea.loopDuration = 0.1f;
		ea.frequency = 1;
		ea.interval = 0.1f;
		ea.color = new Color(1,0,0,80.0f/255);
		ea.toColor = new Color(1,0,0,128.0f/255);
		heroResEffect.hitEffectAttrList.Add(ea);
	}

	public static Transform GetHeroHead(HeroRes heroRes)
	{
		return heroRes.HeadTrans == null ? heroRes.transform : heroRes.HeadTrans;
	}

	public static Vector3 GetHeroHeadPos(HeroRes heroRes)
	{
		Vector3 pos = Vector3.zero;
		if(heroRes.HeadTrans == null)
		{
			pos = GetHeroCenterPos(heroRes);
		}
		else
		{
			pos = heroRes.HeadTrans.position;
		}
		return pos;
	}

	static public Vector3 GetHeroCenterPos(HeroRes heroRes)
	{
		return heroRes.transform.position + heroRes.CenterOffset;
	}

	static public Vector3 GetCenterPos(Hero hero)
	{
		Vector3 centerPos = hero.heroRes.transform.position + hero.heroRes.CenterOffset;
		return centerPos;
	}
	
	static public Vector3 GetDefaultCenterPos(Hero hero)
	{
		Vector3 centerPos = hero.heroAttack.defaultPos + hero.heroRes.CenterOffset;
		return centerPos;
	}
	
	static public Vector3 GetHitPos(Hero hero)
	{
		Vector3 hitPos = hero.heroRes.transform.position + hero.heroRes.CenterOffset + new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
		return hitPos;
	}

	static public void HideBattleHeroHealthBar()
	{
		List<Hero> rightHeros = BattleController.SingleTon().RightHeroes;
		List<Hero> leftHeros = BattleController.SingleTon().LeftHeroes;
		foreach(Hero hero in rightHeros)
		{
			if(hero.healthBar!=null)
			{
				hero.updateHealbar = false;
				hero.healthBar.gameObject.SetActive(false);
			}
		}
		foreach(Hero hero in leftHeros)
		{
			if(hero.healthBar!=null)
			{
				hero.updateHealbar = false;
				hero.healthBar.gameObject.SetActive(false);
			}
		}
	}

	static public void ShowBattleHeroHealthBar()
	{
		List<Hero> rightHeros = BattleController.SingleTon().RightHeroes;
		List<Hero> leftHeros = BattleController.SingleTon().LeftHeroes;
		foreach(Hero hero in rightHeros)
		{
			hero.updateHealbar = true;
		}
		foreach(Hero hero in leftHeros)
		{
			hero.updateHealbar = true;
		}
	}

	static public Dictionary<Transform,int> InitHitPointUsedCount(HeroRes heroRes)
	{
		Dictionary<Transform,int> hps = new Dictionary<Transform, int>();
		if(heroRes.HitPoints==null || heroRes.HitPoints.Count==0)
		{
			Debug.LogError("heroRes.HitPoints is empty!");
		}
		else
		{
			foreach(Transform t in heroRes.HitPoints)
			{
				if(t!=null && !hps.ContainsKey(t))
				{
					hps.Add(t,0);
				}
			}
		}
		return hps;
	}

	static public Transform GetLeastUsedHitPoint(Hero hero)
	{
		Dictionary<Transform,int> hitPointMap = hero.hitPointUseedCount;
		Transform t0 = null;
		if(hitPointMap==null || hitPointMap.Count == 0)
		{
			return t0;
		}
		foreach(Transform t in hitPointMap.Keys)
		{
			if(t0 == null)
			{
				t0 = t;
			}
			if(hitPointMap[t0] > hitPointMap[t])
			{
				t0 = t;
			}
		}
		hitPointMap[t0] = hitPointMap[t0] + 1; 
		return t0;
	}

	static public bool SkillAble(HeroAttribute heroAttribute)
	{
		if(heroAttribute.currentEnergy >= heroAttribute.maxEnergy)
		{
			return true;
		}
		return false;
	}

	static public void ShowElementHitEffect(Hero hero,Vector3 pos)
	{
		GameObject prefab = GetHitEffectPrefab(hero);
		PoolManager.SingleTon().Spawn(prefab,pos,Quaternion.identity);
	}

	static public void ShowDefenseHitEffect(Hero hero,Vector3 pos)
	{
		PoolManager.SingleTon().Spawn(BattleController.SingleTon().prefabDefenseHit,pos,Quaternion.identity,0,2);
	}

	static public GameObject GetHitEffectPrefab(Hero hero)
	{
		if(BattleController.SingleTon()!=null)
		{
			if(hero.heroAttribute.elementType == _ElementType.Water)
			{
				return BattleController.SingleTon().Effect_HitComman_Water;
			}
			else if(hero.heroAttribute.elementType == _ElementType.Wind)
			{
				return BattleController.SingleTon().Effect_HitComman_Wind;
			}
			else if(hero.heroAttribute.elementType == _ElementType.Fire)
			{
				return BattleController.SingleTon().Effect_HitComman_Fire;
			}
			else if(hero.heroAttribute.elementType == _ElementType.Wood)
			{
				return BattleController.SingleTon().Effect_HitComman_Wood;
			}
			else if(hero.heroAttribute.elementType == _ElementType.Evil)
			{
				return BattleController.SingleTon().Effect_HitComman_Magic;
			}
			else if(hero.heroAttribute.elementType == _ElementType.Holy)
			{
				return BattleController.SingleTon().Effect_HitComman_Light;
			}
		}
		return null;
	}

	static public Vector3 GetLeftSkillTargetPos()
	{
		Vector3 leftSkillTargetPos = Vector3.zero;
		if(BattleController.SingleTon()!=null)
		{
			leftSkillTargetPos = BattleController.SingleTon().leftSkillMoveTarget.position;
//			float x = leftSkillTargetPos.x + Random.Range(-1.0f,1.0f);
//			float y = leftSkillTargetPos.y + Random.Range(-3.0f,3.0f);
//			float z = leftSkillTargetPos.z;
//			leftSkillTargetPos = new Vector3(x,y,z);
		}
		return leftSkillTargetPos;
	}

	static public Vector3 GetRightSkillTarget()
	{
		Vector3 rightSkillTargetPos = Vector3.zero;
		if(BattleController.SingleTon()!=null)
		{
			rightSkillTargetPos = BattleController.SingleTon().rightSkillMoveTarget.position;
			float x = rightSkillTargetPos.x + Random.Range(-1.0f,1.0f);
			float y = rightSkillTargetPos.y + Random.Range(-3.0f,3.0f);
			float z = rightSkillTargetPos.z;
			rightSkillTargetPos = new Vector3(x,y,z);
		}
		return rightSkillTargetPos;
	}

	static public void ShowFinger1()
	{
		UISprite finger = BattleController.SingleTon().fingerSprite1;
		UITweener[] tws = finger.GetComponentsInChildren<UITweener>();
		foreach(UITweener tw in tws)
		{
			tw.ResetToBeginning();
			tw.PlayForward();
		}
	}

	static public void ShowFinger()
	{
		UISprite finger = BattleController.SingleTon().fingerSprite;
		UITweener[] tws = finger.GetComponentsInChildren<UITweener>();
		foreach(UITweener tw in tws)
		{
			tw.ResetToBeginning();
			tw.PlayForward();
		}
	}

}

[System.Serializable]
public class RestrainRelation
{
	public _ElementType Ele;
	public _ElementType PreEle;
	public _ElementType NextEle;
}

[System.Serializable]
public class Wave
{
	public List<Hero> heros;
}

//public enum _ElementType{Thunder=1,Wood=2,Water=3,Fire=4,Holy=15,Evil=16}
public enum _ElementType{Wind=4,Wood=3,Water=1,Fire=2,Holy=5,Evil=6,None=7}
[System.Serializable]
public class HeroAttr
{
	public int Id;
	public int Side;
	public int Location;
	public float Speed;
	public string Name;
	public string BodyPrefabName;
	public int HP;
	public int Energy;
	public _ElementType HeroType = _ElementType.Wood;//0 is Thunder,1 is wood,2 is water,3 is fire,4 is Holy,5 is Evil
	public int BaseATK;
	public int BaseDEF;
	public int BaseATKAbility;
	public int BaseRevert;
	public float[] ATKQueue;
	public string ATKSound;
	public string ATKEffect;
	public Skill BaseSkill;
	public bool Movable = true;
	public bool IsFlash = false;
	public bool Shakable = false;
	public int SkillOdds = 0;
	public float HitDelay = 0.5f;
	public bool IsBoss = false;
}

public enum _Combo{}
public enum _SkillType{Heal,Damage,Buff,Debuff}//如果只有4种类型就用1248
public enum _SubSkillType{Single,Multi,All}
[System.Serializable]
public class Skill
{
	public _SkillType SkillType = _SkillType.Damage;
	public _SubSkillType SubSkillType = _SubSkillType.All;
	public int Effect;
	public float[] AttackQueue;
	public float Rate = 1;//Base on Hero normal attack 
}

public enum BattleState
{
	noCondition = 0,
	RoundCount = 1,
	RoundStart = 2,
	RoundEnd = 3,
	IsCombo = 4,
	IsCritical = 5,
	HeroType = 6,
	Attacking = 7,
	BattleBegin = 8
}