using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EffectWindow : EditorWindow {

	[MenuItem("Window/Temporary Editor Window")]
	static void Init()
	{
		EditorWindow.GetWindow<EffectWindow>();
	}

//	string myString = "Hello World";
//	bool groupEnabled = false;
//	bool myBool = true;
//	float myFloat = 1.23f;
	string tag = "shadow";

	void OnGUI () {

		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Change shadow Tag:",GUILayout.Width(120f));
		EditorGUILayout.TextField(tag);
		if(GUILayout.Button("OK",GUILayout.Width(50f),GUILayout.Height(25f)))
		{
			GetAllHeros();

			for(int i=0;i<hre.Count;i++)
			{
				if(hre[i].GetComponent<HeroRes>()!=null && hre[i].GetComponent<HeroRes>().Shadow !=null)
				{
					hre[i].GetComponent<HeroRes>().Shadow.tag = tag;
				}
			}
			Caching.CleanCache();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Check HeroResEffect",GUILayout.Width(120f));
//		EditorGUILayout.TextField(tag);
		if(GUILayout.Button("OK",GUILayout.Width(50f),GUILayout.Height(25f)))
		{
			GetAllHeros();
			for(int i=0;i<hre.Count;i++)
			{
				if(hre[i].GetComponent<HeroResEffect>()==null)
				{
					HeroResEffect heroResEffect = hre[i].AddComponent<HeroResEffect>();
					BattleUtility.SetHeroResEffectInitValue(heroResEffect);
				}
			}
			Caching.CleanCache();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Reload Hero Anim",GUILayout.Width(120f));
		if(GUILayout.Button("OK",GUILayout.Width(50f),GUILayout.Height(25f)))
		{
			GetAllHeros();
//			Undo.RecordObjects(hre,"HeroRes");
			for(int i=0;i<hre.Count;i++)
			{
				GameObject go = Instantiate(hre[i]) as GameObject;
				go.name = hre[i].name;

				HeroRes heroRes = go.GetComponent<HeroRes>();
				if(heroRes==null)
				{
					heroRes = hre[i].AddComponent<HeroRes>();
				}
				if(heroRes.bodyObjects==null || heroRes.bodyObjects.Count==0)
				{
					heroRes.bodyObjects = new List<GameObject>();
					for(int j= 0;j<heroRes.transform.childCount;j++ )
					{
						Transform child = heroRes.transform.GetChild(j);
						if(child.GetComponent<Animation>()!=null)
						{
							heroRes.bodyObjects.Add(child.gameObject);
						}
					}
				}
				HeroResEditor.InitBodyAnimation(heroRes);
				PrefabUtility.ReplacePrefab(go,hre[i]);
				DestroyImmediate(go);
//				EditorUtility.SetDirty(hre[i]);
			}
			Caching.CleanCache();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Reset default animClip",GUILayout.Width(120f));
		if(GUILayout.Button("OK",GUILayout.Width(50f),GUILayout.Height(25f)))
		{
			ResetDefaultAnimClip();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Reset Hit animClip",GUILayout.Width(120f));
		startColor = EditorGUILayout.ColorField(startColor);
		endColor = EditorGUILayout.ColorField(endColor);
		if(GUILayout.Button("OK",GUILayout.Width(50f),GUILayout.Height(25f)))
		{
			ResetHitEffectValue();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Modify Hero SmallHead",GUILayout.Width(120f));
		if(GUILayout.Button("OK",GUILayout.Width(50f),GUILayout.Height(25f)))
		{
			ModifyHeroSmallHead();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Reset Hero Head",GUILayout.Width(120f));
		if(GUILayout.Button("OK",GUILayout.Width(50f),GUILayout.Height(25f)))
		{
			SetHeroHeadTrans();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Reset Hero HitPoint",GUILayout.Width(120f));
		if(GUILayout.Button("OK",GUILayout.Width(50f),GUILayout.Height(25f)))
		{
			SetHitPoints();

		}
		EditorGUILayout.EndHorizontal();


	}

	void SetHitPoints()
	{
		GetAllHeros();
		foreach(GameObject go in hre)
		{
			HeroRes heroRes = go.GetComponent<HeroRes>();
//			DestroyImmediate(heroRes.transform.FindChild("HitPoints"),true);
			HeroResEditor.InitHitPoints(go.GetComponent<HeroRes>());
			EditorUtility.SetDirty (go);
		}
	}

	Material mMat;
	void SetHeroDeathMaterial(Material mat)
	{
		GetAllHeros();
		foreach(GameObject go in hre)
		{
			HeroResEffect heroResEffect = go.GetComponent<HeroResEffect>();
//			HeroRes heroRes = go.GetComponent<HeroRes>();
//			float clipLength = 0;
//			foreach(AnimMapping am in heroRes.bodyAnims)
//			{
//				if(am.clipName == "Death")
//				{
//					clipLength = CommonUtility.GetFragsLength(am.frags);
//					if(clipLength == 0)
//					{
//						clipLength = am.clip.length;
//					}
//					break;
//				}
//			}
			List<EffectAttr> heroResEffects = heroResEffect.deathEffectAttrList;
			bool has = false;
			foreach(EffectAttr hr in heroResEffects)
			{
				if(hr.effectType == _EffectType.ChangeMaterial)
				{
					hr.material = mat;
					hr.isPlayAfterAnim = true;
//					hr.delayTime = clipLength;
					has = true;
					break;
				}
			}
			if(!has)
			{
				EffectAttr tempHre = new EffectAttr();
				tempHre.material = mat;
//				tempHre.delayTime = clipLength;
				tempHre.isPlayAfterAnim = true;
				tempHre.effectType = _EffectType.ChangeMaterial;
				heroResEffect.deathEffectAttrList.Add(tempHre);
			}
			EditorUtility.SetDirty (go);
		}
	}

	void SetHeroHeadTrans()
	{
		GetAllHeros();
		foreach(GameObject go in hre)
		{
			HeroRes heroRes = go.GetComponent<HeroRes>();
			heroRes.HeadTrans = CommonUtility.FindChild(heroRes.transform,"Head");
			EditorUtility.SetDirty (go);
		}
		Caching.CleanCache();
	}

	void ModifyHeroSmallHead()
	{
		GameObject atlas = AssetDatabase.LoadAssetAtPath("Assets/_BF/UIAtlas/heroPortaraitSmall/heroPortaraitAtlas.prefab", typeof(GameObject)) as GameObject;
		UIAtlas uiAtlas = atlas.GetComponent<UIAtlas>();
		List<UISpriteData> datas = uiAtlas.spriteList;
		foreach(UISpriteData spriteData in datas)
		{
			spriteData.height = 108;
		}
		EditorUtility.SetDirty(atlas);
	}


	Color startColor;
	Color endColor;

	void ResetHitEffectValue()
	{
		GetAllHeros();
		foreach(GameObject go in hre)
		{
			ResetHitEffect(go,startColor,endColor);
			EditorUtility.SetDirty (go);
		}
	}
	
	public void ResetHitEffect(GameObject go,Color color,Color colorTo)
	{
		HeroResEffect heroResEffect = go.GetComponent<HeroResEffect>();
		Debug.Log(go);
		foreach(EffectAttr ae in heroResEffect.hitEffectAttrList)
		{
			if(ae.effectType == _EffectType.ChangeColor)
			{
				ae.color = color;
				ae.toColor = colorTo;
				ae.interval = 0.1f;
			}
		}
	}

	public static void SetHeroResEffectInitValue(HeroResEffect heroResEffect)
	{
		EffectAttr ea = new EffectAttr();
		ea.effectType = _EffectType.ChangeColor;
		ea.delayTime = 0;
		ea.loopDuration = 0.1f;
		ea.interval = 0.2f;
		ea.color = Color.white;
		ea.toColor = HeroResEffect.hitColor;
		heroResEffect.hitEffectAttrList.Add(ea);
	}


	string path = "/_BF/Prefabs/Hero";

	void ResetDefaultAnimClip()
	{
		GetAllHeros();
		Debug.Log(hre.Count);
		foreach(GameObject go in hre)
		{
			HeroResEditor.ResetAnimationClipToStandBy(go);
		}
	}

	List<GameObject> hre = null;
	void GetAllHeros()
	{
		string[] paths = CommonUtility.GetFileByDirectory(path);
		hre = new List<GameObject>();
		foreach(string str in paths)
		{
			string assetPath = "Assets" + str.Replace(Application.dataPath, "").Replace('\\', '/');
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath,typeof(GameObject));
			if(prefab!=null)
			{
				hre.Add(prefab);
			}
		}
	}


}
