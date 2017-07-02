using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum _DropType{BC=0,HC=1,Soul=2,Coin=3,BattleMaterial=4}
public class Drop : MonoBehaviour {

	public static GameObject CollectEffect;
	public static Pool EffectPool;

	public static GameObject CollectEffectGreen;
	public static Pool EffectGreenPool;
	public Transform TargetT;
	public Vector3 targetPos;
	public Hero hero;
//	public Pool ThisPool;
	public _DropType type;
	public int battleMaterialId;
	public int Val;
	public SpriteRenderer sr;
	public TweenPosition wp;
	public bool isFalling;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	void Update(){
		ChangeOrderLayer("UnitLayer0",(-(int)(transform.position.y * 10))) ;
	}

	public void Collect(float dur)
	{
//		if(!IsMoving)
//		{
//			IsMoving = true;
		if(wp==null)
		{
			wp = GetComponent<TweenPosition>();
			EventDelegate ev = new EventDelegate(MoveDone);
			wp.onFinished.Add(ev);
		}
		wp.from = transform.position;
//		if(type == _DropType.HC || type == _DropType.BC)
//		{
//			Hero h = TargetT.GetComponent<Hero>();
//			if(h != null)
//			{
//				ChangeOrderLayer("");
//				wp.to = h.heroRes.transform.position + h.heroRes.CenterOffset;
//			}
//			else
//			{
//				wp.to = TargetT.position;
//			}
//		}
//		else
//		{
			if(TargetT!=null)
			{
				wp.to = TargetT.position;
			}
			else
			{
				wp.to = targetPos;
			}
//		}
		//Debug.Log("Collect!");
		ChangeOrderLayer("SuperLayer");
		wp.duration = Random.Range(0.8f,1.5f);

		wp.ResetToBeginning();
		wp.PlayForward();
//		wp.SetOnFinished(BattleManager.SingleTon ().AddEnergy);
		if(type == _DropType.BattleMaterial)
		{
			StartCoroutine("TweentAlpha");
		}
//		StartCoroutine(_Move(dur));
//		}
	}

	IEnumerator TweentAlpha()
	{
		SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime;
			foreach(SpriteRenderer sr in srs)
			{
				sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,1-t);
			}
			yield return null;
		}
	}

	public void OnSpawn(Vector3 heroPos,Vector3 pos)
	{
		isFalling = true;
		StartCoroutine(_Drop(heroPos,pos));
	}


//	Renderer[] mRenders;
	List<Renderer> mRenders;
	public void ChangeOrderLayer(string layer,float order)
	{
		if(mRenders==null)
		{
			mRenders = new List<Renderer>();
			mRenders.AddRange(GetComponentsInChildren<Renderer>());
			if(GetComponent<Renderer>()!=null)
			{
				mRenders.Add(GetComponent<Renderer>());
			}
		}
		foreach(Renderer rr in mRenders)
		{
			rr.sortingLayerName = layer;
			rr.sortingOrder = (int)(order * 100);
		}
	}

	public void ChangeOrderLayer(string layer)
	{
		if(mRenders==null)
		{
			mRenders = new List<Renderer>();
			mRenders.AddRange(GetComponentsInChildren<Renderer>());
			if(GetComponent<Renderer>()!=null)
			{
				mRenders.Add(GetComponent<Renderer>());
			}
		}
		foreach(Renderer rr in mRenders)
		{
			rr.sortingLayerName = layer;
		}
	}

	public SpriteRenderer GetSpriteRenderer()
	{
		if(sr==null)
			sr = GetComponent<SpriteRenderer>();
		return sr;
	}

	IEnumerator _Drop(Vector3 heroPos,Vector3 pos)
	{
		ChangeOrderLayer("SuperLayer",-transform.position.y);
		float x = 0;
		if(pos.x > 0)
			x = Random.Range(0,2.5f);
		else
			x =  Random.Range(-2.5f,0);
		if(x<0)x-=1.5f;
		if(x>0)x+=1.5f;
		Vector2 startPos = transform.position;
		Vector2 direction = new Vector2(x,Random.Range(0.0f,1.0f));
		Vector2 targetPos = (Vector2)heroPos + direction ;
//		Debug.Log("targetPos:" + targetPos);
		Vector2 controlPos = (startPos + targetPos) / 2;
		controlPos = new Vector2(controlPos.x,controlPos.y + Random.Range(2.5f,3.0f));
		float t = 0;
//		int nagative = -1;
//		if(targetPos.x < startPos.x)
//		{
//			nagative = 1;
//		}
//		Vector3 startEuler =  new Vector3(0,0,0 * nagative);
//		Vector3 endEnler =  Random.Range(0,2) == 0 ? new Vector3(0,0,360 * nagative) : Vector3.zero;

		while(t<1)
		{
			t += Time.deltaTime / 0.5f;
			transform.position = Curve.Bezier2(startPos ,controlPos,targetPos,t);
//			transform.eulerAngles = Vector3.Lerp(startEuler,endEnler,t);
			yield return null;
		}
//		ChangeOrderLayer("UI",-transform.position.y);
		isFalling = false;
//		GetSpriteRenderer().sortingLayerName = "UI";
	}

	void MoveDone()
	{
		if(type == _DropType.BC)
		{
			if(AudioManager.SingleTon()!=null)AudioManager.SingleTon().PlayBCClip();
			hero = TargetT.GetComponent<Hero>();
			hero.AdjustEnergy(Random.Range(0,2));
			ShowCollectEffect(hero);
		}
		else if(type == _DropType.HC)
		{
			if(AudioManager.SingleTon()!=null)AudioManager.SingleTon().PlayHCClip();
			hero = TargetT.GetComponent<Hero>();
			int point = BattleUtility.GetHCPoint(hero);
			hero.OnHeal(point);
			//hero.OnHeal(BattleUtility.GetHCPoint(hero));
				
			ShowCollectEffectGreen(hero,point);
		}
		else if(type == _DropType.Soul)
		{
			if(AudioManager.SingleTon()!=null)AudioManager.SingleTon().PlaySoulClip();
			if(BattleController.SingleTon()!=null)
			BattleController.SingleTon().AddSoul(Val);
		}
		else if(type == _DropType.Coin)
		{
			if(AudioManager.SingleTon()!=null)AudioManager.SingleTon().PlayCoinClip();
			if(BattleController.SingleTon()!=null)
			BattleController.SingleTon().AddCoin(Val);
		}
		PoolManager.SingleTon().UnSpawn(gameObject);
		if(BattleController.SingleTon()!=null)
		{
			BattleController.SingleTon().Drops.Remove(this);
		}
		else
		{
			if(BattleSimpleController.SingleTon()!=null)
			BattleSimpleController.SingleTon().Drops.Remove(this);
		}
		BattleManager.GetInstance ().AddEnergy ();
//		ThisPool.Unspawn(ThisGo);
	}

	void ShowCollectEffect(Hero hero)
	{
		Vector3 pos = hero.heroRes.transform.position + hero.heroRes.CenterOffset;
		GameObject prefab = BattleController.SingleTon() != null ? BattleController.SingleTon().prefabToBody : BattleSimpleController.SingleTon().prefabToBody;
		GameObject go = PoolManager.SingleTon().Spawn(prefab,pos,Quaternion.identity);
		PoolManager.SingleTon().UnSpawn(2,go);
	}

	void ShowCollectEffectGreen(Hero hero,int point)
	{
		Vector3 pos = hero.heroRes.transform.position + hero.heroRes.CenterOffset;
		GameObject prefab = BattleController.SingleTon() != null ? BattleController.SingleTon().prefabToBodyGreen : BattleSimpleController.SingleTon().prefabToBodyGreen;
		GameObject go = PoolManager.SingleTon().Spawn(prefab,pos,Quaternion.identity);
		PoolManager.SingleTon().UnSpawn(2,go);


		/*Vector3 offset;
		Vector3 hitPos = BattleUtility.GetHitPos(hero);
		offset = new Vector3(Random.Range(-0.1f,0.1f),Random.Range(1f,1f),0);
		pos = BattleUtility.GetNGUIPosFromWorldPos(hitPos + offset);
		BattleUtility.ShowDamageBeat(HeroAttribute.shakeRadius,point,pos,1,false);*/

		BattleUtility.ShowEnhaceEffect(hero,point);
	}

	IEnumerator UnSpawn(float delay,Pool pool,GameObject go)
	{
		yield return new WaitForSeconds(delay);
		pool.Unspawn(go);
	}
}


