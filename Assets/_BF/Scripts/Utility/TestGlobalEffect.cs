using UnityEngine;
using System.Collections;

public class TestGlobalEffect : MonoBehaviour {

	public SpriteRenderer globalMask;
	public GameObject SkilledHero;
	public float from = 0;
	public float to = 0.8f;
	public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
	public float delay = 0.5f;
	public float duration = 1;

	public float effectDelay = 0.5f;
	public GameObject effectPrefab;

	Hero mHero;
	HeroRes mHeroRes;
	HeroAnimation mHeroAnimation;
	string previousLayerName;

	void Start()
	{
		mHero = SkilledHero.GetComponent<Hero>();
		mHeroRes = SkilledHero.GetComponent<HeroRes>();
		if(mHeroRes==null)
		{
			mHeroRes = SkilledHero.GetComponentInChildren<HeroRes>();
		}
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,100,30),"Test"))
		{
			if(globalMask!=null)
			{
				StopCoroutine("ShowEffect");
				StopCoroutine("TweenAlpha");

				StartCoroutine("ShowEffect");
				StartCoroutine("TweenAlpha");
			}
		}
	}

	void OnTweenDone()
	{

	}

	void Reset()
	{
		if(SkilledHero!=null)
		{
			mHeroRes = SkilledHero.GetComponent<HeroRes>();
			if(mHeroRes==null)
			{
				mHeroRes = SkilledHero.GetComponentInChildren<HeroRes>();
				if(previousLayerName!=null)
					CommonUtility.SetOrderLayer(previousLayerName,mHeroRes);
			}
		}
	}

	IEnumerator ShowEffect()
	{
		yield return new WaitForSeconds(effectDelay);
		GameObject go = Instantiate(effectPrefab,SkilledHero.transform.position,Quaternion.identity) as GameObject;
		Renderer[] rs = go.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs)
		{
			r.sortingLayerName = "SuperLayer2";
		}
	}

	IEnumerator TweenAlpha()
	{
		if(mHero !=null)
		{
			previousLayerName = mHero.OrderLayerName;
		}
		CommonUtility.SetOrderLayer("SuperLayer1",mHeroRes);
		yield return new WaitForSeconds(delay);
		float t =0;
		while(t<1)
		{
			t += Time.deltaTime / duration;
			float value = curve.Evaluate(t);
			float alpha = Mathf.Lerp(from,to,value);
			globalMask.color = new Color(globalMask.color.r,globalMask.color.g,globalMask.color.b,alpha);
			SkilledHero.transform.localScale = Vector3.Lerp(new Vector3(0.5f,0.5f,0.5f),Vector3.one,value);
			yield return null;
		}
		OnTweenDone();
	}

}
