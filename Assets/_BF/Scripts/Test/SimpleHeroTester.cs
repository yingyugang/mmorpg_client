using UnityEngine;
using System.Collections;

public class SimpleHeroTester : MonoBehaviour {

	public GameObject currentHero;

	public HeroAnimation mHeroAnimation;
	public HeroEffect mHeroEffect;
	public HeroRes mHeroRes;
	public HeroResEffect mHeroResEffect;

	public void SetCurrentHero(GameObject obj)
	{
		mHeroRes = obj.GetComponent<HeroRes>();;
		mHeroAnimation.heroRes = mHeroRes;
		mHeroResEffect = obj.GetComponent<HeroResEffect>();
		mHeroAnimation.heroResEffect = mHeroResEffect;
		mHeroEffect.heroResEffect = mHeroResEffect;
		mHeroEffect.InitPlayMeshRenders();
		currentHero = obj;
	}

	static SimpleHeroTester instance;
	static public SimpleHeroTester SingleTon()
	{
		SimpleHeroTester[] shts = FindObjectsOfType<SimpleHeroTester>();
		if(shts[0] != null)
		{
			instance = shts[0];
		}
		if(instance == null)
		{
			GameObject obj = new GameObject();
			obj.name = "_Controller";
			instance = obj.AddComponent<SimpleHeroTester>();
			instance.mHeroAnimation = obj.AddComponent<HeroAnimation>();
			instance.mHeroEffect = obj.AddComponent<HeroEffect>();
		}
		return instance;
	}

	void Awake()
	{
		if(instance==null)
		{
			instance = this;
		}
	}

	int mIndexX = 0;

	void OnGUI()
	{
		mIndexX = 0;
		foreach(AnimMapping am in mHeroRes.bodyAnims)
		{
			if(GUI.Button(new Rect(10,10 + 40 * mIndexX,100,30),am.clipName))
			{
				mHeroAnimation.PlayByFrags(am.clipName);
				_AnimType type = CommonUtility.AnimCilpNameStringToEnum(am.clipName);
				Debug.Log("type:" + type);
				mHeroEffect.PlayEffects(type);
			}
			mIndexX ++;
		}
	}



}
