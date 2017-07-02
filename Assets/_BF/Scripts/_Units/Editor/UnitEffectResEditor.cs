using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

//[CanEditMultipleObjects]
[CustomEditor (typeof(UnitEffectRes))]
public class UnitEffectResEditor : Editor
{

	//	HeroResEffect mTarget;
	Dictionary<_UnitArtActionType,List<EffectAttr>> effectMaps = new Dictionary<_UnitArtActionType,List<EffectAttr>> ();
	Dictionary<string,UnitEffectResAnimAttr> animMaps = new Dictionary<string, UnitEffectResAnimAttr> ();
	List<_UnitArtActionType> artActions = new List<_UnitArtActionType> ();
	UnitEffectRes mTarget;
	SkeletonAnimation mSkeletonAnimation;

	void GetEffectMaps ()
	{
		effectMaps = new Dictionary<_UnitArtActionType, List<EffectAttr>> ();
		effectMaps.Add (_UnitArtActionType.atk_0001, mTarget.atk_0001_attrs_list);
		effectMaps.Add (_UnitArtActionType.atk_0101, mTarget.atk_0101_attrs_list);
		effectMaps.Add (_UnitArtActionType.atk_0102, mTarget.atk_0102_attrs_list);
		effectMaps.Add (_UnitArtActionType.atk_0103, mTarget.atk_0103_attrs_list);
		effectMaps.Add (_UnitArtActionType.atk_0201, mTarget.atk_0201_attrs_list);
		effectMaps.Add (_UnitArtActionType.atk_0202, mTarget.atk_0202_attrs_list);
		effectMaps.Add (_UnitArtActionType.atk_0203, mTarget.atk_0203_attrs_list);
		effectMaps.Add (_UnitArtActionType.cmn_0001, mTarget.cmn_0001_attrs_list);
		effectMaps.Add (_UnitArtActionType.cmn_0002, mTarget.cmn_0002_attrs_list);
		effectMaps.Add (_UnitArtActionType.cmn_0003, mTarget.cmn_0003_attrs_list);
		effectMaps.Add (_UnitArtActionType.cmn_0004, mTarget.cmn_0004_attrs_list);
		effectMaps.Add (_UnitArtActionType.cmn_0006, mTarget.cmn_0006_attrs_list);
		artActions = new List<_UnitArtActionType> ();
		artActions.AddRange (effectMaps.Keys);
	}

	void GetAnimMaps ()
	{
		mSkeletonAnimation = mTarget.GetComponent<SkeletonAnimation> ();
		animMaps = new Dictionary<string,UnitEffectResAnimAttr> ();
		for (int i = 0; i < mSkeletonAnimation.Skeleton.Data.Animations.Items.Length; i++) {
			UnitEffectResAnimAttr attr = new UnitEffectResAnimAttr ();
			attr.anim = mSkeletonAnimation.Skeleton.Data.Animations.Items [i];
			animMaps.Add (mSkeletonAnimation.Skeleton.Data.Animations.Items [i].Name, attr);
		}
	}

	//	void GetEffectMaps()
	//	{
	//		effectMaps = new Dictionary<_AnimType,List<EffectAttr>>();
	//		effectMaps.Add(_AnimType.Attack,mTarget.attackEffectAttrList);
	//		effectMaps.Add(_AnimType.Skill1,mTarget.skillEffeectAttrList);
	//		effectMaps.Add(_AnimType.StandBy,mTarget.standbyEffectAttrList);
	//		effectMaps.Add(_AnimType.Run,mTarget.runEffectAttrList);
	//		effectMaps.Add(_AnimType.Death,mTarget.deathEffectAttrList);
	//		effectMaps.Add(_AnimType.Hit,mTarget.hitEffectAttrList);
	//		effectMaps.Add(_AnimType.Cheer,mTarget.cheerEffectAttrList);
	//		effectMaps.Add(_AnimType.Power,mTarget.powerEffectAttrList);
	//		effectMaps.Add(_AnimType.Sprint,mTarget.sprintEffectAttrList);
	//	}
	//
	//	void GetAnimMaps()
	//	{
	//		animMaps = new Dictionary<_AnimType, AnimMapping>();
	//		HeroRes heroRes = mTarget.GetComponent<HeroRes>();
	//		foreach(AnimMapping am in heroRes.bodyAnims)
	//		{
	//			if(CommonUtility.AnimCilpNameStringToEnum(am.clipName) == _AnimType.None)
	//			{
	//				Debug.LogError("am.clipName:" + am.clipName + " isn't in our list.");
	//			}
	//			else if(am.anim == null)
	//			{
	//				Debug.LogError("am.clipName:" + am.clipName + " is empty!");
	//			}
	//			else
	//			{
	//				animMaps.Add(CommonUtility.AnimCilpNameStringToEnum(am.clipName),am);
	//			}
	//		}
	//	}
	//
	//	Dictionary<_AnimType,List<EffectAttr>> effectMaps = new Dictionary<_AnimType,List<EffectAttr>>();
	//	Dictionary<_AnimType,AnimMapping> animMaps = new Dictionary<_AnimType,AnimMapping>();
	//	_AnimType[] types;
	GUIStyle errorGUIStyle;

	public void OnEnable ()
	{
		mTarget = (UnitEffectRes)target;
		GetEffectMaps ();
		GetAnimMaps ();
//		mTarget = (HeroResEffect)target;
//		types = CommonUtility.GetAnimTypes();
//		GetEffectMaps();
//		GetAnimMaps();
		errorGUIStyle = CommonUtility.GetErrorGUIStyle ();
//		lastTime = (float)EditorApplication.timeSinceStartup;
	}

	float clipLength;
	float previousSampleTime;
	float previousSampleTime1;
	UnitEffectResAnimAttr currentAm;

//	float lastTime;
	public override void OnInspectorGUI ()
	{
//		mSkeletonAnimation.state.Update ((float)EditorApplication.timeSinceStartup - lastTime);
//		lastTime = (float)EditorApplication.timeSinceStartup;
		serializedObject.Update ();
		Undo.RecordObject (mTarget, "UnitEffectRes");
		if (NGUIEditorTools.DrawHeader ("Effects")) {
			NGUIEditorTools.BeginContents ();
			for (int i = 0; i < artActions.Count; i++) {

				if (NGUIEditorTools.DrawHeader("Effect " + artActions[i].ToString())){
					NGUIEditorTools.BeginContents();
					NGUIEditorTools.SetLabelWidth(70f);
					if(!animMaps.ContainsKey(artActions[i].ToString()))
					{
						string text = "  <b><size=11>Animation " + artActions [i].ToString () + " not exiting!</size></b>";
						GUILayout.Label (text, errorGUIStyle);
					}
					EditorGUILayout.BeginHorizontal();
					{
						if(GUILayout.Button("Add",GUILayout.Width(50f),GUILayout.Height(25f)))
						{
							effectMaps[artActions[i]].Add(new EffectAttr());
						}
						if(GUILayout.Button("Clear",GUILayout.Width(50f),GUILayout.Height(25f)))
						{
							effectMaps[artActions[i]].Clear();
						}
						if(artActions[i] == _UnitArtActionType.cmn_0006)
							mTarget.enableHitEffect = GUILayout.Toggle(mTarget.enableHitEffect,"Enable",GUILayout.Width(50f),GUILayout.Height(25f));
					}
					EditorGUILayout.EndHorizontal();
					if(animMaps.ContainsKey(artActions[i].ToString()))
					{
						currentAm = animMaps[artActions[i].ToString()];
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Custom Clip:");
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Clip Length:",GUILayout.Width(80));
						EditorGUILayout.LabelField(currentAm.anim.Duration.ToString(),GUILayout.Width(70));
						EditorGUILayout.LabelField("Current Time:",GUILayout.Width(80));
						currentAm.sampleTime = EditorGUILayout.FloatField(currentAm.sampleTime,GUILayout.Width(70));
						currentAm.sampleTime = Mathf.Min (currentAm.sampleTime,currentAm.anim.Duration);
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal();
						currentAm.sampleTime = GUILayout.HorizontalSlider(currentAm.sampleTime,0,currentAm.anim.Duration,GUILayout.Width(300));
						if(currentAm.preSampleTime != currentAm.sampleTime)
						{
							mSkeletonAnimation.AnimationName = "";
							mSkeletonAnimation.state.SetAnimation (0, currentAm.anim.Name, true);
							mSkeletonAnimation.state.Update (currentAm.sampleTime);
							currentAm.preSampleTime = currentAm.sampleTime;
						}
						EditorGUILayout.EndHorizontal();
//						if(currentAm.frags!=null && currentAm.frags.Count>0)
//						{
//							EditorGUILayout.BeginHorizontal();
//							EditorGUILayout.LabelField("Custom Clip:");
//							EditorGUILayout.EndHorizontal();
//							EditorGUILayout.BeginHorizontal();
//							EditorGUILayout.LabelField("Clip Length:",GUILayout.Width(100));
//							clipLength = CommonUtility.GetFragsLength(currentAm.frags);
//							EditorGUILayout.FloatField(clipLength,GUILayout.Width(80));
//							EditorGUILayout.LabelField("Current Time:",GUILayout.Width(100));
//							EditorGUILayout.FloatField(currentAm.sampleTime1,GUILayout.Width(80));
//							EditorGUILayout.EndHorizontal();
//							EditorGUILayout.BeginHorizontal();
//							previousSampleTime1 = currentAm.sampleTime1;
//							currentAm.sampleTime1 = GUILayout.HorizontalSlider(currentAm.sampleTime1,0,clipLength,GUILayout.Width(300));
//							if(previousSampleTime1 != currentAm.sampleTime1)
//							{
//								if(!Application.isPlaying)
//								{
//									AnimMapping am = currentAm;
//									am.clip.SampleAnimation(am.body,CommonUtility.GetRealClipPoint(currentAm.frags,currentAm.sampleTime1));
//								}
//							}
//							EditorGUILayout.EndHorizontal();
//						}
//						else
//						{
//							EditorGUILayout.BeginHorizontal();
//							EditorGUILayout.LabelField("Default Clip:");
//							EditorGUILayout.EndHorizontal();
//							EditorGUILayout.BeginHorizontal();
//							EditorGUILayout.LabelField("Clip Length:",GUILayout.Width(100));
//							clipLength = currentAm.clip.length;
//							EditorGUILayout.FloatField(clipLength,GUILayout.Width(80));
//							EditorGUILayout.LabelField("Current Time:",GUILayout.Width(100));
//							EditorGUILayout.FloatField(currentAm.sampleTime,GUILayout.Width(80));
//							EditorGUILayout.EndHorizontal();
//							EditorGUILayout.BeginHorizontal();
//							previousSampleTime = currentAm.sampleTime;
//							currentAm.sampleTime = GUILayout.HorizontalSlider(currentAm.sampleTime,0,clipLength,GUILayout.Width(300));
//							if(previousSampleTime != currentAm.sampleTime)
//							{
//								if(!Application.isPlaying)
//								{
//									AnimMapping am = currentAm;
//									am.clip.SampleAnimation(am.body, currentAm.sampleTime);
//								}
//							}
//							EditorGUILayout.EndHorizontal();
//						}
					}
					if(artActions[i] ==  _UnitArtActionType.cmn_0006 && !mTarget.enableHitEffect)
					{

					}
					else
					{
						for(int j = 0;j < effectMaps[artActions[i]].Count;j++)
						{
							NGUIEditorTools.BeginContents();
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Effect Type",GUILayout.Width(80f));
							effectMaps[artActions[i]][j].effectType = (_EffectType)EditorGUILayout.EnumPopup(effectMaps[artActions[i]][j].effectType,GUILayout.Width(80f));
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Ignore TimeScale",GUILayout.Width(80f));
							effectMaps[artActions[i]][j].ignoreTimeScale = EditorGUILayout.Toggle(effectMaps[artActions[i]][j].ignoreTimeScale, GUILayout.Width(80f));
							EditorGUILayout.EndHorizontal();

							if(effectMaps[artActions[i]][j].effectType == _EffectType.Cast)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Cast At",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].effectTargetType = (_EffectTargetType)EditorGUILayout.EnumPopup(effectMaps[artActions[i]][j].effectTargetType,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Scope",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].effectScopeType = (_EffectScopeType)EditorGUILayout.EnumPopup(effectMaps[artActions[i]][j].effectScopeType,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Effect Prefab",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].effectPrefab = (GameObject)EditorGUILayout.ObjectField(effectMaps[artActions[i]][j].effectPrefab,typeof(GameObject),false,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].delayTime, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].delayTime = Mathf.Max(0,effectMaps[artActions[i]][j].delayTime);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Frequency",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].frequency = EditorGUILayout.IntField(effectMaps[artActions[i]][j].frequency, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].frequency = Mathf.Max(1,effectMaps[artActions[i]][j].frequency);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Interval",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].interval = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].interval, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].interval = Mathf.Max(0,effectMaps[artActions[i]][j].interval);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Follow",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].follow = (Transform)EditorGUILayout.ObjectField(effectMaps[artActions[i]][j].follow,typeof(Transform),true,GUILayout.Width(200f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								effectMaps[artActions[i]][j].position = EditorGUILayout.Vector3Field("Start Offset",effectMaps[artActions[i]][j].position, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								effectMaps[artActions[i]][j].rotation = EditorGUILayout.Vector3Field("Start Rotate",effectMaps[artActions[i]][j].rotation, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								effectMaps[artActions[i]][j].scale = EditorGUILayout.Vector3Field("Start Scale",effectMaps[artActions[i]][j].scale, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();

								if(effectMaps[artActions[i]][j].effectName != null && effectMaps[artActions[i]][j].effectName != "")
								{
									if(GUILayout.Button("Show",GUILayout.Width(50f),GUILayout.Height(25f)))
									{
										mTarget.GenerateTempResource(effectMaps[artActions[i]][j],true);
									}
								}
							}
							if(effectMaps[artActions[i]][j].effectType == _EffectType.Shoot)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Shoot From",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].effectTargetType = (_EffectTargetType)EditorGUILayout.EnumPopup(effectMaps[artActions[i]][j].effectTargetType,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Scope",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].effectScopeType = (_EffectScopeType)EditorGUILayout.EnumPopup(effectMaps[artActions[i]][j].effectScopeType,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Shoot Track",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].effectShootTrack = (_EffectShootTrack)EditorGUILayout.EnumPopup(effectMaps[artActions[i]][j].effectShootTrack,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Effect Prefab",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].effectPrefab = (GameObject)EditorGUILayout.ObjectField(effectMaps[artActions[i]][j].effectPrefab,typeof(GameObject),false,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Hit Effect Prefab",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].hitEffectPrefab = (GameObject)EditorGUILayout.ObjectField(effectMaps[artActions[i]][j].hitEffectPrefab,typeof(GameObject),false,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Fly Duration",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].shootDurtion = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].shootDurtion, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].delayTime, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].delayTime = Mathf.Max(0,effectMaps[artActions[i]][j].delayTime);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Frequency",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].frequency = EditorGUILayout.IntField(effectMaps[artActions[i]][j].frequency, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].frequency = Mathf.Max(1,effectMaps[artActions[i]][j].frequency);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Interval",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].interval = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].interval, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].interval = Mathf.Max(0,effectMaps[artActions[i]][j].interval);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Follow",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].follow = (Transform)EditorGUILayout.ObjectField(effectMaps[artActions[i]][j].follow,typeof(Transform),true,GUILayout.Width(200f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								effectMaps[artActions[i]][j].position = EditorGUILayout.Vector3Field("Start Offset",effectMaps[artActions[i]][j].position, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								effectMaps[artActions[i]][j].rotation = EditorGUILayout.Vector3Field("Start Rotate",effectMaps[artActions[i]][j].rotation, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								effectMaps[artActions[i]][j].scale = EditorGUILayout.Vector3Field("Start Scale",effectMaps[artActions[i]][j].scale, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();
							}
							if(effectMaps[artActions[i]][j].effectType == _EffectType.ChangeColor || effectMaps[artActions[i]][j].effectType == _EffectType.EdgeColor)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Color",GUILayout.Width(80f));
								Color previouColor = effectMaps[artActions[i]][j].color;
								effectMaps[artActions[i]][j].color = EditorGUILayout.ColorField(effectMaps[artActions[i]][j].color, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("To Color",GUILayout.Width(80f));
								Color previouToColor = effectMaps[artActions[i]][j].toColor;
								effectMaps[artActions[i]][j].toColor = EditorGUILayout.ColorField(effectMaps[artActions[i]][j].toColor, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Loop Type",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].effectLoopType = (_EffectLoopType)EditorGUILayout.EnumPopup(effectMaps[artActions[i]][j].effectLoopType, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Anim Curve",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].curve = EditorGUILayout.CurveField(effectMaps[artActions[i]][j].curve,GUILayout.Height(80f), GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].delayTime, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].delayTime = Mathf.Max(0,effectMaps[artActions[i]][j].delayTime);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Frequency",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].frequency = EditorGUILayout.IntField(effectMaps[artActions[i]][j].frequency, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].frequency = Mathf.Max(1,effectMaps[artActions[i]][j].frequency);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Interval",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].interval = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].interval, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].interval = Mathf.Max(0,effectMaps[artActions[i]][j].interval);
								EditorGUILayout.EndHorizontal();

								if( effectMaps[artActions[i]][j].effectType == _EffectType.EdgeColor)
								{
									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.LabelField("Edge Size",GUILayout.Width(80f));
									effectMaps[artActions[i]][j].edgeSize = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].edgeSize, GUILayout.Width(80f));
									effectMaps[artActions[i]][j].edgeSize = Mathf.Max(0,effectMaps[artActions[i]][j].edgeSize);
									EditorGUILayout.EndHorizontal();

									if(Application.isPlaying )
									{
										if(previouColor != effectMaps[artActions[i]][j].color)
										{
											mTarget.ChangeEdgeColor(effectMaps[artActions[i]][j].color,effectMaps[artActions[i]][j].edgeSize);
										}
										else if(previouToColor != effectMaps[artActions[i]][j].toColor)
										{
											mTarget.ChangeEdgeColor(effectMaps[artActions[i]][j].toColor,effectMaps[artActions[i]][j].edgeSize);
										}
									}
								}
							}
							//
							if(effectMaps[artActions[i]][j].effectType == _EffectType.Ghost)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].delayTime, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].delayTime = Mathf.Max(0,effectMaps[artActions[i]][j].delayTime);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Frequency",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].frequency = EditorGUILayout.IntField(effectMaps[artActions[i]][j].frequency, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].frequency = Mathf.Max(1,effectMaps[artActions[i]][j].frequency);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Interval",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].interval = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].interval, GUILayout.Width(80f));
								effectMaps[artActions[i]][j].interval = Mathf.Max(0,effectMaps[artActions[i]][j].interval);
								EditorGUILayout.EndHorizontal();
							}

							if(effectMaps[artActions[i]][j].effectType == _EffectType.ChangeMaterial)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("PlayAfterAnim",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].isPlayAfterAnim = EditorGUILayout.Toggle(effectMaps[artActions[i]][j].isPlayAfterAnim, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								if(!effectMaps[artActions[i]][j].isPlayAfterAnim)
								{
									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
									effectMaps[artActions[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[artActions[i]][j].delayTime, GUILayout.Width(80f));
									effectMaps[artActions[i]][j].delayTime = Mathf.Max(0,effectMaps[artActions[i]][j].delayTime);
									EditorGUILayout.EndHorizontal();
								}

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Material",GUILayout.Width(80f));
								effectMaps[artActions[i]][j].material = EditorGUILayout.ObjectField(effectMaps[artActions[i]][j].material,typeof(Material),true, GUILayout.Width(80f)) as Material;
								EditorGUILayout.EndHorizontal();

							}

							if(GUILayout.Button("Del",GUILayout.Width(50f),GUILayout.Height(25f)))
							{
								effectMaps[artActions[i]].Remove(effectMaps[artActions[i]][j]);
							}
							NGUIEditorTools.EndContents();
						}
					}
					NGUIEditorTools.EndContents();
					EditorGUILayout.Space();
				}
			}
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Add Death Effect",GUILayout.Width(100f),GUILayout.Height(25f)))
			{
				Material mat = Resources.Load<Material>("DeathColorMaterials");

				if(mat == null)
				{
					Debug.LogError("DeathColorMaterials not exist!");
				}
				else
				{
					EffectAttr ea = new EffectAttr();
					ea.effectType = _EffectType.ChangeMaterial;
					ea.material = mat;
					ea.isPlayAfterAnim = true;
//					mTarget.deathEffectAttrList.Add(ea);
				}
			}
			EditorGUILayout.EndHorizontal();
			NGUIEditorTools.EndContents ();
		}
		EditorUtility.SetDirty (mTarget);
		serializedObject.ApplyModifiedProperties ();
	}
	//	void Clone(_EffectType type,EffectAttr ea)
	//	{
	//		GetAnimMaps();
	//	}
}

public class UnitEffectResAnimAttr{

	public Spine.Animation anim;
	public float sampleTime = 0;
	public float preSampleTime = 0;
}



