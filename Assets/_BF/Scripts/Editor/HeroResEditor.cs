using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(HeroRes))]
public class HeroResEditor : Editor {

	HeroRes mTarget;
	public void OnEnable()
	{
		mTarget = (HeroRes)target;
		if(mTarget.gameObject.activeInHierarchy)
		{
			TestEffectController controller = FindObjectOfType<TestEffectController>();
			if(controller!=null)SetTestHeroRes(controller);
		}
		errorLabelStyle = CommonUtility.GetErrorGUIStyle();
	}

	void SetTestHeroRes(TestEffectController controller )
	{
		if(mTarget.transform.parent == null)
		{
			HeroRes[] hes = FindObjectsOfType<HeroRes>();
			foreach(HeroRes he in hes)
			{
				if(he.gameObject != mTarget.gameObject && he.transform.parent == null)
				{
					DestroyImmediate(he.gameObject);
				}
			}
			mTarget.transform.position = controller.playerPos.position;
			controller.Init(mTarget.gameObject);
		}
		if(!Application.isPlaying)
		{
			Caching.CleanCache();
		}
	}

	float clipLength = 0;
	float fragLength = 0;
	float previousSampleTime = 0;
	float previousSampleTime1 = 0;
	AnimMapping currentAm;
	GUIStyle errorLabelStyle;
	public override void OnInspectorGUI()
	{
		serializedObject.Update ();
		Undo.RecordObject(mTarget,"HeroRes");

		if(NGUIEditorTools.DrawHeader("Animations"))
		{
			EditorGUILayout.BeginHorizontal();
			NGUIEditorTools.BeginContents();

			for(int i =0;i < mTarget.bodyAnims.Count;i++)
			{
				currentAm = mTarget.bodyAnims[i];
				if(mTarget.bodyAnims[i].anim==null)
				{
					string text = "  \u25BA <b><size=11>Animation " + currentAm.clipName + "</size></b>";
					GUILayout.Label(text,errorLabelStyle);
				}
				else if (NGUIEditorTools.DrawHeader("Animation " + currentAm.clipName))
				{
					NGUIEditorTools.BeginContents();
					NGUIEditorTools.SetLabelWidth(70f);
					EditorGUILayout.BeginHorizontal();
					{
						if(GUILayout.Button("Add",GUILayout.Width(50f),GUILayout.Height(25f)))
						{
							currentAm.frags.Add(new AnimationFrag());
						}
						if(GUILayout.Button("Clear",GUILayout.Width(50f),GUILayout.Height(25f)))
						{
							currentAm.frags.Clear();
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Default Clip:");
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Clip Length:",GUILayout.Width(100));
					clipLength = currentAm.clip.length;
					EditorGUILayout.FloatField(clipLength,GUILayout.Width(80));
					EditorGUILayout.LabelField("Current Time:",GUILayout.Width(100));
					EditorGUILayout.FloatField(currentAm.sampleTime,GUILayout.Width(80));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					previousSampleTime = currentAm.sampleTime;
					currentAm.sampleTime = GUILayout.HorizontalSlider(currentAm.sampleTime,0,clipLength,GUILayout.Width(300));
					if(previousSampleTime != currentAm.sampleTime)
					{
						if(!Application.isPlaying)
						{
							AnimMapping am = currentAm;
							am.clip.SampleAnimation(am.body, currentAm.sampleTime);
							if(am.sampleType==0)
							{
								am.sampleFrag.startTime = currentAm.sampleTime;

							}
							else if(am.sampleType==1)
							{
								am.sampleFrag.stopTime = currentAm.sampleTime;
							}
						}
					}
					EditorGUILayout.EndHorizontal();

					if(currentAm.frags!=null && currentAm.frags.Count>0)
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Custom Clip:");
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Clip Length:",GUILayout.Width(100));
						clipLength =CommonUtility.GetFragsLength(currentAm.frags);
						EditorGUILayout.FloatField(clipLength,GUILayout.Width(80));
						EditorGUILayout.LabelField("Current Time:",GUILayout.Width(100));
						EditorGUILayout.FloatField(currentAm.sampleTime1,GUILayout.Width(80));
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal();
						previousSampleTime1 = currentAm.sampleTime1;
						currentAm.sampleTime1 = GUILayout.HorizontalSlider(currentAm.sampleTime1,0,clipLength,GUILayout.Width(300));
						if(previousSampleTime1 != currentAm.sampleTime1)
						{
							if(!Application.isPlaying)
							{
								AnimMapping am = currentAm;
								am.clip.SampleAnimation(am.body,CommonUtility.GetRealClipPoint(currentAm.frags,currentAm.sampleTime1));
							}
						}
						EditorGUILayout.EndHorizontal();
					}

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Global Speed:",GUILayout.Width(100));
					currentAm.globalSpeed = EditorGUILayout.FloatField(currentAm.globalSpeed,GUILayout.Width(80));
					EditorGUILayout.LabelField("Really Time:",GUILayout.Width(120));
					EditorGUILayout.LabelField((clipLength / currentAm.globalSpeed).ToString(),GUILayout.Width(80));
					EditorGUILayout.EndHorizontal();

					foreach(AnimationFrag frag in currentAm.frags)
					{
						NGUIEditorTools.BeginContents();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Frag Lenght:",GUILayout.Width(100));
						fragLength = CommonUtility.GetFragLength(frag);
						EditorGUILayout.LabelField(fragLength.ToString() + " s",GUILayout.Width(80));
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Start Time:",GUILayout.Width(100));
						frag.startTime = EditorGUILayout.FloatField(frag.startTime,GUILayout.Width(55));
						frag.startTime = Mathf.Clamp(frag.startTime,0,currentAm.clip.length);
						if(currentAm.sampleFrag == frag && currentAm.sampleType == 0)
						{
							GUI.color = Color.green;
						}
						if(GUILayout.Button("S",GUILayout.Width(20)))
						{
							if(currentAm.sampleFrag == frag && currentAm.sampleType == 0)
							{
								currentAm.sampleFrag = null;
							}
							else
							{
								currentAm.clip.SampleAnimation(currentAm.body,frag.startTime);
								currentAm.sampleTime = frag.startTime;
								currentAm.sampleFrag = frag;
								currentAm.sampleType = 0;
							}
						}
						GUI.color = Color.white;

						EditorGUILayout.LabelField("Stop Time:",GUILayout.Width(100));
						frag.stopTime = EditorGUILayout.FloatField(frag.stopTime,GUILayout.Width(55));
						frag.stopTime = Mathf.Clamp(frag.stopTime,frag.startTime,currentAm.clip.length);
						if(currentAm.sampleFrag == frag && currentAm.sampleType == 1)
						{
							GUI.color = Color.green;
						}
						if(GUILayout.Button("S",GUILayout.Width(20)))
						{
							if(currentAm.sampleFrag == frag && currentAm.sampleType == 1)
							{
								currentAm.sampleFrag = null;
							}
							else
							{
								currentAm.clip.SampleAnimation(currentAm.body,frag.stopTime);
								currentAm.sampleTime = frag.stopTime;
								currentAm.sampleFrag = frag;
								currentAm.sampleType = 1;
							}
						}
						EditorGUILayout.EndHorizontal();
						GUI.color = Color.white;

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Speed:",GUILayout.Width(100));
						frag.speed = EditorGUILayout.FloatField(frag.speed,GUILayout.Width(80));
						frag.speed = Mathf.Max(0,frag.speed);
						EditorGUILayout.LabelField("Loop Count:",GUILayout.Width(100));
						frag.loopCount = EditorGUILayout.IntField(frag.loopCount,GUILayout.Width(80));
						frag.loopCount = Mathf.Max(1,frag.loopCount);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Rewind:",GUILayout.Width(100));
						frag.rewind = EditorGUILayout.Toggle(frag.rewind,GUILayout.Width(80));
						if(frag.rewind)
						{
							EditorGUILayout.LabelField("Rewind Speed:",GUILayout.Width(100));
							frag.rewindSpeed = EditorGUILayout.FloatField(frag.rewindSpeed,GUILayout.Width(80));
							frag.rewindSpeed = Mathf.Max(0,frag.rewindSpeed);
						}
						EditorGUILayout.EndHorizontal();
						if(GUILayout.Button("Delete",GUILayout.Width(50)))
						{
							currentAm.frags.Remove(frag);
							return;
						}
						NGUIEditorTools.EndContents();
					}
					NGUIEditorTools.EndContents();
				}
			}
			NGUIEditorTools.EndContents();
			EditorGUILayout.EndHorizontal();
		}
		if(NGUIEditorTools.DrawHeader("Other Setting"))
		{
		EditorGUILayout.BeginHorizontal();
		NGUIEditorTools.BeginContents();

			//				NGUIEditorTools.BeginContents();
			int maxCount = mTarget.HitPoints.Count;
			maxCount = Mathf.Max(maxCount,mTarget.ShootPoints.Count);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("HitPoints:",GUILayout.Width(200));
			EditorGUILayout.LabelField("ShootPoints:",GUILayout.Width(200));
			EditorGUILayout.EndHorizontal();
			for(int i=0;i < maxCount;i ++)
			{
				EditorGUILayout.BeginHorizontal();
				if(mTarget.HitPoints.Count > i )
				{
					EditorGUILayout.ObjectField(mTarget.HitPoints[i],typeof(Transform),true,GUILayout.Width(100));
					if(mTarget.HitPoints[i]!=null)EditorGUILayout.LabelField(mTarget.HitPoints[i].localPosition.ToString(),GUILayout.Width(100));
				}
				else
				{
					EditorGUILayout.LabelField("",GUILayout.Width(200));
				}
				
				if(mTarget.ShootPoints.Count > i )
				{
					EditorGUILayout.ObjectField(mTarget.ShootPoints[i],typeof(Transform),true,GUILayout.Width(100));
					if(mTarget.ShootPoints[i]!=null)EditorGUILayout.LabelField(mTarget.ShootPoints[i].localPosition.ToString(),GUILayout.Width(100));
				}
				else
				{
					EditorGUILayout.LabelField("",GUILayout.Width(200));
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.BeginHorizontal();
			mTarget.CenterOffset = EditorGUILayout.Vector3Field("Center Offset:",mTarget.CenterOffset);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.ObjectField("Head:",mTarget.HeadTrans,typeof(Transform),true);
			EditorGUILayout.EndHorizontal();

		NGUIEditorTools.EndContents();
		EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Test"))
		{
			ActiveCurrentHeroRes();
		}
		if(GUILayout.Button("Reload Animation"))
		{
			InitBodyAnimation(mTarget);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Reset Hitpoints"))
		{
			ResetHitPoints(mTarget);
		}
		if(GUILayout.Button("Reset Shootpoints"))
		{
			ResetShootPoints(mTarget);
		}
		EditorGUILayout.EndHorizontal();

		EditorUtility.SetDirty (mTarget);
		serializedObject.ApplyModifiedProperties ();
	}

	public static void LoadHeroRes(HeroRes heroRes)
	{
		Debug.Log("LoadHeroRes");
#region 1.Remove previous prefab.
		int count = heroRes.transform.childCount;
		int x = 0;
		for(int i = 0; i < count; i++)
		{
			if(heroRes.transform.GetChild(x) != heroRes.Shadow)
			{
				DestroyImmediate(heroRes.transform.GetChild(x).gameObject);
			}else{
				x ++;
			}
		}
#endregion

#region 2.Init ShootPoints
		if(heroRes.ShootPoints==null || heroRes.ShootPoints.Count==0)
			InitShootPoints(heroRes);
#endregion

#region 3.Init HitPoints
//		if(heroRes.HitPoints==null || heroRes.HitPoints.Count == 0)
//		{
		if(heroRes.HitPoints==null || heroRes.HitPoints.Count==0)
			InitHitPoints(heroRes);
//		}
#endregion

#region 4.Init AnimationArray
		heroRes.bodyAnimMapping = new Dictionary<string, AnimMapping>();
		heroRes.CurrentAm = null;
//		List<AnimMapping> previousAnims = heroRes.bodyAnims;
//		heroRes.bodyAnims = new List<AnimMapping>();
		heroRes.bodyObjects = new List<GameObject>();
		foreach(GameObject bodyPrefab in heroRes.BodyPrefabs)
		{
			GameObject realGo = Instantiate(bodyPrefab) as GameObject;
			realGo.transform.parent = heroRes.transform;
			realGo.name = bodyPrefab.name;
			heroRes.bodyObjects.Add(realGo);
		}
		InitBodyAnimation(heroRes);
#endregion

#region 5.Init Shadow
		string basePath = "Assets/ArtDate/Prefeb/";
		string path = basePath + "Effect/Character_Shadow.prefab";
		if(heroRes.Shadow==null)
		{
			GameObject shadowPrefab = AssetDatabase.LoadAssetAtPath(path,typeof(GameObject)) as GameObject;
			GameObject tmp = Instantiate(shadowPrefab) as GameObject;
			heroRes.Shadow = tmp.transform;
			heroRes.Shadow.parent = heroRes.transform;
			heroRes.Shadow.localScale = new Vector3(4,3,1);
			heroRes.Shadow.name = shadowPrefab.name;
		}
#endregion

#region 6.Init Center Offset
		heroRes.CenterOffset = new Vector3(0,3,0);
#endregion

#region 7.Init Head Position
		heroRes.HeadTrans = CommonUtility.FindChild(heroRes.transform,"Head");
#endregion

	}

	static string hitPointsName = "HitPoints";

	static public void ResetHitPoints(HeroRes heroRes)
	{
		ClearHitPoints(heroRes);
		InitHitPoints(heroRes);
	}

	static public void ClearHitPoints(HeroRes heroRes)
	{
		List<Transform> hitPoints = heroRes.HitPoints;
		Transform hitParent = heroRes.transform.FindChild(hitPointsName);
		if(hitParent!=null)DestroyImmediate(hitParent.gameObject);
		if(hitPoints!=null)hitPoints.Clear();
	}

	static public void InitHitPoints(HeroRes heroRes)
	{
		int maxHitPointCount = 6;
		GameObject parentHitGo = null;
		if(heroRes.HitPoints == null || heroRes.HitPoints.Count == 0)
		{
			heroRes.HitPoints = new List<Transform>();
			parentHitGo = new GameObject();
			parentHitGo.transform.parent = heroRes.transform;
			parentHitGo.transform.localPosition = Vector3.zero;
			parentHitGo.name = "HitPoints";
			for(int i = 0;i < maxHitPointCount; i ++)
			{
				GameObject go = new GameObject();
				go.name = "HitPoint" + i;
				go.transform.parent = parentHitGo.transform;
				heroRes.HitPoints.Add(go.transform);
			}
		}
		else
		{
			parentHitGo = heroRes.HitPoints[0].parent.gameObject;
		}
		for(int i = 0;i < maxHitPointCount; i ++)
		{
			if(i==0)heroRes.HitPoints[i].localPosition = new Vector3(-3,0.5f,0);
			if(i==1)heroRes.HitPoints[i].localPosition = new Vector3(-3,1.5f,0);
			if(i==2)heroRes.HitPoints[i].localPosition = new Vector3(-3,-0.5f,0);
			if(i==3)heroRes.HitPoints[i].localPosition = new Vector3(-4,0.5f,0);
			if(i==4)heroRes.HitPoints[i].localPosition = new Vector3(-4,1.5f,0);
			if(i==5)heroRes.HitPoints[i].localPosition = new Vector3(-4,-0.5f,0);
		}
	}

	static string shootPointsName = "ShootPoints";

	static public void ResetShootPoints(HeroRes heroRes)
	{
		ClearShootPoints(heroRes);
		InitShootPoints(heroRes);
	}
	
	static public void ClearShootPoints(HeroRes heroRes)
	{
		List<Transform> shootPoints = heroRes.ShootPoints;
		Transform shootParent = heroRes.transform.FindChild(shootPointsName);
		if(shootParent!=null)DestroyImmediate(shootParent.gameObject);
		if(shootPoints!=null)shootPoints.Clear();
	}

	static public void InitShootPoints(HeroRes heroRes)
	{
		GameObject parentGo = new GameObject();
		parentGo.transform.parent = heroRes.transform;
		parentGo.transform.localPosition = Vector3.zero;
		int maxShootPointCount = 5;
		heroRes.ShootPoints = new List<Transform>();
		parentGo.name = "ShootPoints";
		for(int i = 0;i < maxShootPointCount; i ++)
		{
			GameObject go = new GameObject();
			go.name = "ShootPoint" + i;
			go.transform.parent = parentGo.transform;
			heroRes.ShootPoints.Add(go.transform);
		}
		for(int i = 0;i < maxShootPointCount; i ++)
		{
			if(i==0)heroRes.ShootPoints[i].localPosition = new Vector3(0,0,0);
			if(i==1)heroRes.ShootPoints[i].localPosition = new Vector3(0,2,0);
			if(i==2)heroRes.ShootPoints[i].localPosition = new Vector3(0,4,0);
			if(i==3)heroRes.ShootPoints[i].localPosition = new Vector3(-1.5f,2,0);
			if(i==4)heroRes.ShootPoints[i].localPosition = new Vector3(1.5f,2,0);
		}
	}

	static public void ResetAnimationClipToStandBy(GameObject go)
	{
		foreach(Transform t in go.transform)
		{
			if(t.GetComponent<Animation>()!=null)
			{
				Animation anim = t.GetComponent<Animation>();
				foreach(AnimationState state in anim)
				{
					string clipName = state.name;
					if(clipName.Contains("_StandBy"))
					{
						anim.clip = state.clip;
						break;
					}
				}
			}
		}
	}

	static List<AnimMapping> GetBodyAnimation(HeroRes heroRes)
	{
		List<AnimMapping> ams = new List<AnimMapping>();
		List<string> types = CommonUtility.InitClipTypes();
		for(int i=0;i<types.Count;i++)
		{
			AnimMapping animMap = new AnimMapping();
			animMap.clipName = types[i];
			ams.Add(animMap);
		}

		Dictionary<string,AnimMapping> amDic = new Dictionary<string, AnimMapping>();
		foreach(GameObject realGo in heroRes.bodyObjects)
		{
			Animation anim = realGo.GetComponent<Animation>();
			realGo.SetActive(false);
			if(anim!=null)
			{
				foreach(AnimationState state in anim)
				{
					AnimMapping animMap = new AnimMapping();
					string aName = state.clip.name;
					if(aName.LastIndexOf("_")!=-1)
					{
						aName = aName.Substring(aName.LastIndexOf("_") + 1);
					}
					if(!types.Contains(aName))
					{
						Debug.LogError("Clip :" + aName + " is a wrong name!");
					}
					else
					{
						animMap.clipName = aName;
						animMap.clip = state.clip;
						animMap.anim = anim;
						animMap.body = realGo;
						animMap.skinRs = realGo.GetComponentsInChildren<SkinnedMeshRenderer>();
						animMap.spriteRs = realGo.GetComponentsInChildren<SpriteRenderer>();
						amDic.Add(animMap.clipName,animMap);
					}
				}
			}
		}

		List<AnimMapping> previousAms = heroRes.bodyAnims;
		Dictionary<string,AnimMapping> previousAmDic = new Dictionary<string, AnimMapping>();
		if(previousAms!=null)
		{
			foreach(AnimMapping am in previousAms)
			{
				previousAmDic.Add(am.clipName,am);
			}
		}

		for(int i=0;i<ams.Count;i++)
		{
			if(amDic.ContainsKey(ams[i].clipName))
			{
				ams[i] = amDic[ams[i].clipName];
			}
			if(previousAmDic.ContainsKey(ams[i].clipName))
			{
				ams[i].frags = previousAmDic[ams[i].clipName].frags;
			}
		}
		return ams;
	}

	static public void InitBodyAnimation(HeroRes heroRes)
	{
		List<AnimMapping> ams = GetBodyAnimation(heroRes);
		foreach(AnimMapping am in ams)
		{
			if(am.clipName == "StandBy")
			{
				am.anim.clip = am.clip;
				am.body.SetActive(true);
				break;
			}
		}
		heroRes.bodyAnims = ams;
	}

	void ActiveCurrentHeroRes()
	{
		SimpleHeroTester.SingleTon().SetCurrentHero(mTarget.gameObject);
	}


}
