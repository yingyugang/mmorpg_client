using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestGlobalSkill : MonoBehaviour {

	public GlobalSkillEffect globalSkillEffect;
	public GameObject heroPrefab;
	public List<GameObject> heros;
	public GameObject hero;
	void Start()
	{
//		heros.Add(hero);
#if UNITY_EDITOR
		List<GameObject> heroPrefabs = CommonUtility.GetAllHeros();
		foreach(GameObject prefab in heroPrefabs)
		{
			GameObject heroGo = Instantiate(heroPrefab) as GameObject;
			Hero hero = heroGo.GetComponent<Hero>();
			GameObject go = Instantiate(prefab) as GameObject;
			go.name = prefab.name;
			go.transform.parent = heroGo.transform;
			go.transform.localPosition =Vector3.zero;
			HeroRes heroRes = go.GetComponent<HeroRes>();
			HeroAnimation heroAnimation = go.AddComponent<HeroAnimation>();

			HeroResEffect heroResEffect = go.GetComponent<HeroResEffect>();
			HeroEffect heroEffect = go.AddComponent<HeroEffect>();

			heroAnimation.heroRes = heroRes;
			heroAnimation.heroResEffect = heroResEffect;
			heroEffect.heroResEffect = heroResEffect;

			hero.heroRes = heroRes;
			hero.heroEffect = heroEffect;
			hero.heroResEffect = heroResEffect;
			hero.heroAnimation = heroAnimation;
			hero.heroRes = heroRes;
			heroGo.SetActive(false);
			heros.Add(heroGo);
		}
#endif
	}

	Vector2 scrollPos = new Vector2(100,500);
	void OnGUI()
	{
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		foreach(GameObject go in heros)
		{
			GUILayout.BeginHorizontal();
			Hero h = go.GetComponent<Hero>();
			if(GUILayout.Button(h.heroRes.name))
			{
				go.SetActive(true);
				globalSkillEffect.gameObject.SetActive(true);
				globalSkillEffect.Play(h);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

}
