using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalSkillEffect : MonoBehaviour
{

	public SpriteRenderer skillMask;
	public float alphaFrom = 0;
	public float alphaTo = 1;
	public AnimationCurve curve = AnimationCurve.Linear (0, 0, 1, 1);
	public float delay = 0;
	public float duration = 1.0f;

	public float effectDelay = 0.15f;
	public GameObject effectPrefab;

	public float scaleFrom = 1;
	public float scaleTo = 1.2f;
	public float scaleDelay = 0;
	public float scaleDuration = 1.0f;
	public _AnimType animType = _AnimType.Cheer;

	public float resetDelay = 1;

	public bool IsPlaying;

	Hero mHero;
	float mTotalTime;
	Vector3 mHeroDefaultScale;
	List<Hero> mHeros;
	List<Vector3> mDefaultScales;
	List<GameObject> mEffects = new List<GameObject> ();

	public void Play (List<Hero> heros)
	{
		duration = 0.2f;
		AudioManager.SingleTon ().PlayBattleSkillClip ();
		IsPlaying = true;
		mHeros = heros;
		BattleController.SingleTon ().AutoMask.SetActive (true);
//		Time.timeScale = 0.1f;
		BattleUtility.HideBattleHeroHealthBar ();
		mDefaultScales = new List<Vector3> ();
#region Order as default sorting
		List<Hero> tmpHeros = new List<Hero> ();
		for (int i = heros.Count - 1; i >= 0; i--) {
			tmpHeros.Add (heros [i]);
		}
		mHeros = tmpHeros;
#endregion
		for (int i = 0; i < heros.Count; i++) {
			mDefaultScales.Add (heros [i].transform.localScale);
		}
//		StartCoroutine("ShowEffect");
		StartCoroutine ("TweenForwardScales");
		StartCoroutine ("TweenForwardAlpha");
		mTotalTime = Mathf.Max (delay + duration, effectDelay);
		mTotalTime = Mathf.Max ((scaleDelay + scaleDuration) * heros.Count, mTotalTime);
		mTotalTime += resetDelay;
		StartCoroutine ("_Reset");
	}

	void Reset ()
	{
		if (skillMask != null) {
			skillMask.color = new Color (skillMask.color.r, skillMask.color.g, skillMask.color.b, 0);
		}
		for (int i = 0; i < mHeros.Count; i++) {
			mHeros [i].transform.localScale = mDefaultScales [i];
			mHeros [i].updateSelfLayerByPostion = true;
			if (mHeros [i].heroRes != null) {
				CommonUtility.SetSortingLayerWithChildren (mHeros [i].heroRes.gameObject, mHeros [i].OrderLayerName);
			}
		}
		foreach (GameObject effectGo in mEffects) {
			effectGo.SetActive (false);
		}
		mEffects.Clear ();
		Time.timeScale = 1;
		if (BattleManager.GetInstance () != null)
			Time.timeScale = BattleManager.GetInstance ().quickBattleTimeScale;
		BattleUtility.ShowBattleHeroHealthBar ();
		IsPlaying = false;
		BattleController.SingleTon ().AutoMask.SetActive (false);
	}

	IEnumerator _Reset ()
	{
		float t = 0;
		while (t < mTotalTime) {
			t += RealTime.deltaTime;
			yield return null;
		}
		Stop ();
		yield return null;
	}

	IEnumerator TweenForwardAlpha ()
	{
		float t = 0;
		float value = 0;
		while (t < 1) {
			t += RealTime.deltaTime / duration;
			value = curve == null ? t : curve.Evaluate (t);
			value = Mathf.Lerp (alphaFrom, alphaTo, value);
			if (skillMask != null)
				skillMask.color = new Color (skillMask.color.r, skillMask.color.g, skillMask.color.b, value);
			yield return null;
		}
		foreach (Hero hero in mHeros) {
			if (hero.heroAnimation != null) {
				hero.heroAnimation.StopAllCoroutines ();// .PlayWithRealTime ("Cheer");
			}
			hero.Play (Hero.ACTION_SKILL1);
		}
	}

	IEnumerator TweenForwardScales ()
	{
		Vector3 defaultScale;
		if (BattleController.SingleTon ().summoned == true)
			scaleTo = 1f;
		else
			scaleTo = 1.2f;

		for (int i = 0; i < mHeros.Count; i++) {
			if (mHeros [i] != null && mHeros [i].heroRes != null) {
				float t = 0;
				float value = 0;
				defaultScale = mHeros [i].transform.localScale;
				mHeros [i].updateSelfLayerByPostion = false;
				while (t < 1) {
					CommonUtility.SetSortingLayerWithChildren (mHeros [i].heroRes.gameObject, "SuperLayer1");
					t += RealTime.deltaTime / scaleDuration;
					mHeros [i].transform.localScale = Vector3.Lerp (defaultScale * scaleFrom, defaultScale * scaleTo, t);
					yield return null;
				}
				AudioManager.SingleTon ().PlayBattleSkillActionClip ();
				if (mHeros [i].heroAnimation != null) {
					mHeros [i].heroAnimation.PlayWithRealTime ("Cheer");
				}
				mEffects.Add (Instantiate (effectPrefab, mHeros [i].transform.position, Quaternion.identity) as GameObject);
			}
		}
	}

	public void Play (Hero hero)
	{
		mHero = hero;
		IsPlaying = true;
		if (hero.heroAnimation != null) {
			hero.heroAnimation.PlayWithRealTime ("Cheer");
		}
		BattleController.SingleTon ().AutoMask.SetActive (true);
		Time.timeScale = 0;
		BattleUtility.HideBattleHeroHealthBar ();
//		StartCoroutine("ShowEffect");
		StartCoroutine ("TweenForwardScales");
		StartCoroutine ("TweenForwardAlpha");
		mTotalTime = Mathf.Max (delay + duration, effectDelay);
		mTotalTime = Mathf.Max (scaleDelay + scaleDuration, mTotalTime);
		mTotalTime += resetDelay;
		StartCoroutine ("_Reset");
	}

	public void Stop ()
	{
//		StopCoroutine("ShowEffect");
		StopCoroutine ("TweenForwardScales");
		StopCoroutine ("TweenForwardAlpha");
		StopCoroutine ("_Reset");
		Reset ();
	}
	//	IEnumerator ShowEffect()
	//	{
	//		float t = 0 ;
	//		while(t < effectDelay)
	//		{
	//			t += RealTime.deltaTime;
	//			yield return null;
	//		}
	//		if(mHero!=null)
	//			Instantiate(effectPrefab,mHero.transform.position,Quaternion.identity);
	//	}

	IEnumerator TweenForwardScale ()
	{
		float t = 0;
		float value = 0;
		if (BattleController.SingleTon ().summoned == true)
			scaleTo = 1f;
		else
			scaleTo = 1.2f;

		mHeroDefaultScale = mHero.transform.localScale;
		while (t < 1) {
			t += RealTime.deltaTime / scaleDuration;
			if (mHero != null)
				mHero.transform.localScale = Vector3.Lerp (mHeroDefaultScale * scaleFrom, mHeroDefaultScale * scaleTo, t);
			yield return null;
		}
	}

	//	IEnumerator TweenForwardAlpha()
	//	{
	//		float t = 0;
	//		float value = 0;
	//		if(mHero!=null && mHero.heroRes!=null)
	//		{
	//			mHero.updateSelfLayerByPostion = false;
	//			CommonUtility.SetSortingLayerWithChildren(mHero.heroRes.gameObject,"SuperLayer1");
	//		}
	//		while(t < 1)
	//		{
	//			t += RealTime.deltaTime / duration;
	//			value = curve == null ? t : curve.Evaluate(t);
	//			value = Mathf.Lerp(alphaFrom,alphaTo,value);
	//			if(skillMask!=null)skillMask.color = new Color(skillMask.color.r,skillMask.color.g,skillMask.color.b,value);
	//			yield return null;
	//		}
	//	}

	//	public Transform controllTrans;
	//	public TweenPosition[] tps;
	//	public bool isPlaying;

	//	void Start () {
	//		EventDelegate ed = new EventDelegate(Done);
	//		EventDelegate ed1 = new EventDelegate(Hide);
	//		tps[1].onFinished.Add(ed1);
	//	}

	//	HeroAnimation mHeroAnimation;
	//	GameObject mBody;
	//	public void Play(HeroAnimation heroAnimation)
	//	{
	//		isPlaying = true;
	//		mHeroAnimation = heroAnimation;
	//		float childCount = controllTrans.childCount;
	//		for(int i=0;i<childCount;i++)
	//		{
	//			Transform t = controllTrans.GetChild(i);
	//			t.gameObject.SetActive(false);
	//		}
	//		gameObject.SetActive(true);
	//		mBody = heroAnimation.gameObject;
	//		heroAnimation.Play(Hero.ACTION_SPRINT);
	//		Time.timeScale = 0;
	//		Vector3 pos = heroAnimation.heroRes.HeadTrans == null ? (heroAnimation.transform.position + heroAnimation.heroRes.CenterOffset): heroAnimation.heroRes.HeadTrans.position;
	//		Vector3 offset = pos - heroAnimation.heroRes.transform.position;
	//		mBody.transform.parent = tps[0].transform;
	//		mBody.transform.localPosition = -offset;
	//		mBody.transform.localScale = Vector3.one;
	//		foreach(TweenPosition tp in tps)
	//		{
	//			tp.ResetToBeginning();
	//			tp.PlayForward();
	//		}
	//	}

	//	void Hide()
	//	{
	//		mHeroAnimation.gameObject.SetActive(false);
	//	}

	//	void Done()
	//	{
	//		mCurHero.heroAttack.Attack();
	//		gameObject.SetActive(false);
	//		Time.timeScale = 1;
	//		mBody.SetActive(false);
	//		Destroy(mBody);
	//		isPlaying =false;
	//	}

	//	void OnDrawGizmos()
	//	{
	//		Gizmos.color = Color.red;
	//		if(controllTrans!=null)
	//			Gizmos.DrawWireSphere(controllTrans.position,0.5f);
	//	}

}
