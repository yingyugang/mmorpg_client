using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//[CanEditMultipleObjects]
[CustomEditor(typeof(HeroResEffect))]
public class HeroResEffectEditor : Editor {

	HeroResEffect mTarget;

	void GetEffectMaps()
	{
		effectMaps = new Dictionary<_AnimType,List<EffectAttr>>();
		effectMaps.Add(_AnimType.Attack,mTarget.attackEffectAttrList);
		effectMaps.Add(_AnimType.Skill1,mTarget.skillEffeectAttrList);
		effectMaps.Add(_AnimType.StandBy,mTarget.standbyEffectAttrList);
		effectMaps.Add(_AnimType.Run,mTarget.runEffectAttrList);
		effectMaps.Add(_AnimType.Death,mTarget.deathEffectAttrList);
		effectMaps.Add(_AnimType.Hit,mTarget.hitEffectAttrList);
		effectMaps.Add(_AnimType.Cheer,mTarget.cheerEffectAttrList);
		effectMaps.Add(_AnimType.Power,mTarget.powerEffectAttrList);
		effectMaps.Add(_AnimType.Sprint,mTarget.sprintEffectAttrList);
	}

	void GetAnimMaps()
	{
		animMaps = new Dictionary<_AnimType, AnimMapping>();
		HeroRes heroRes = mTarget.GetComponent<HeroRes>();
		foreach(AnimMapping am in heroRes.bodyAnims)
		{
			if(CommonUtility.AnimCilpNameStringToEnum(am.clipName) == _AnimType.None)
			{
				Debug.LogError("am.clipName:" + am.clipName + " isn't in our list.");
			}
			else if(am.anim == null)
			{
				Debug.LogError("am.clipName:" + am.clipName + " is empty!");
			}
			else
			{
				animMaps.Add(CommonUtility.AnimCilpNameStringToEnum(am.clipName),am);
			}
		}
	}

	Dictionary<_AnimType,List<EffectAttr>> effectMaps = new Dictionary<_AnimType,List<EffectAttr>>();
	Dictionary<_AnimType,AnimMapping> animMaps = new Dictionary<_AnimType,AnimMapping>();
	_AnimType[] types;
	GUIStyle errorGUIStyle;

	public void OnEnable()
	{
		mTarget = (HeroResEffect)target;
		types = CommonUtility.GetAnimTypes();
		GetEffectMaps();
		GetAnimMaps();
		errorGUIStyle = CommonUtility.GetErrorGUIStyle();
	}

	float clipLength;
	float previousSampleTime;
	float previousSampleTime1;
	AnimMapping currentAm;
	public override void OnInspectorGUI()
	{
		serializedObject.Update ();
		Undo.RecordObject(mTarget,"EffectRes");
		if (NGUIEditorTools.DrawHeader("Effects"))
		{
			NGUIEditorTools.BeginContents();
			for(int i=0;i<types.Length;i++)
			{
				if(!animMaps.ContainsKey(types[i]) || animMaps[types[i]].anim == null)
				{
					string text = "  \u25BA <b><size=11>Animation " + types[i].ToString() + "</size></b>";
					GUILayout.Label(text,errorGUIStyle);
				}
				else if (NGUIEditorTools.DrawHeader("Effect " + types[i].ToString()))
				{
					NGUIEditorTools.BeginContents();
					NGUIEditorTools.SetLabelWidth(70f);
					EditorGUILayout.BeginHorizontal();
					{
						if(GUILayout.Button("Add",GUILayout.Width(50f),GUILayout.Height(25f)))
						{
							effectMaps[types[i]].Add(new EffectAttr());
						}
						if(GUILayout.Button("Clear",GUILayout.Width(50f),GUILayout.Height(25f)))
						{
							effectMaps[types[i]].Clear();
						}
						if(types[i] == _AnimType.Hit)
							mTarget.enableHitEffect = GUILayout.Toggle(mTarget.enableHitEffect,"Enable",GUILayout.Width(50f),GUILayout.Height(25f));
					}
					EditorGUILayout.EndHorizontal();
					if(animMaps.ContainsKey(types[i]))
					{
						currentAm = animMaps[types[i]];
						if(currentAm.frags!=null && currentAm.frags.Count>0)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Custom Clip:");
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Clip Length:",GUILayout.Width(100));
							clipLength = CommonUtility.GetFragsLength(currentAm.frags);
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
						else
						{
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
								}
							}
							EditorGUILayout.EndHorizontal();
						}
					}
					if(types[i] == _AnimType.Hit && !mTarget.enableHitEffect)
					{

					}
					else
					{
						for(int j = 0;j < effectMaps[types[i]].Count;j++)
						{
							NGUIEditorTools.BeginContents();
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Effect Type",GUILayout.Width(80f));
							effectMaps[types[i]][j].effectType = (_EffectType)EditorGUILayout.EnumPopup(effectMaps[types[i]][j].effectType,GUILayout.Width(80f));
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Ignore TimeScale",GUILayout.Width(80f));
							effectMaps[types[i]][j].ignoreTimeScale = EditorGUILayout.Toggle(effectMaps[types[i]][j].ignoreTimeScale, GUILayout.Width(80f));
							EditorGUILayout.EndHorizontal();

							if(effectMaps[types[i]][j].effectType == _EffectType.Cast)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Cast At",GUILayout.Width(80f));
								effectMaps[types[i]][j].effectTargetType = (_EffectTargetType)EditorGUILayout.EnumPopup(effectMaps[types[i]][j].effectTargetType,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Scope",GUILayout.Width(80f));
								effectMaps[types[i]][j].effectScopeType = (_EffectScopeType)EditorGUILayout.EnumPopup(effectMaps[types[i]][j].effectScopeType,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Effect Prefab",GUILayout.Width(80f));
								Object go = null;
								go = EditorGUILayout.ObjectField(go,typeof(GameObject),false,GUILayout.Width(80f));
								if(go!=null)
								{
									string path = AssetDatabase.GetAssetPath(go);
									int index = path.IndexOf("Resources/");
									if(index!=-1)
									{
										path = path.Replace(path.Substring(0,index +"Resources/".Length),"");
										path = path.Replace(".prefab","");
									}
									effectMaps[types[i]][j].effectName = path;
									go = null;
								}
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Effect Path",GUILayout.Width(80f));
								EditorGUILayout.LabelField(effectMaps[types[i]][j].effectName, GUILayout.Width(300f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
								effectMaps[types[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[types[i]][j].delayTime, GUILayout.Width(80f));
								effectMaps[types[i]][j].delayTime = Mathf.Max(0,effectMaps[types[i]][j].delayTime);
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Frequency",GUILayout.Width(80f));
								effectMaps[types[i]][j].frequency = EditorGUILayout.IntField(effectMaps[types[i]][j].frequency, GUILayout.Width(80f));
								effectMaps[types[i]][j].frequency = Mathf.Max(1,effectMaps[types[i]][j].frequency);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Interval",GUILayout.Width(80f));
								effectMaps[types[i]][j].interval = EditorGUILayout.FloatField(effectMaps[types[i]][j].interval, GUILayout.Width(80f));
								effectMaps[types[i]][j].interval = Mathf.Max(0,effectMaps[types[i]][j].interval);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Follow",GUILayout.Width(80f));
								effectMaps[types[i]][j].follow = (Transform)EditorGUILayout.ObjectField(effectMaps[types[i]][j].follow,typeof(Transform),true,GUILayout.Width(200f));
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								effectMaps[types[i]][j].position = EditorGUILayout.Vector3Field("Start Offset",effectMaps[types[i]][j].position, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								effectMaps[types[i]][j].rotation = EditorGUILayout.Vector3Field("Start Rotate",effectMaps[types[i]][j].rotation, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								effectMaps[types[i]][j].scale = EditorGUILayout.Vector3Field("Start Scale",effectMaps[types[i]][j].scale, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();

								if(effectMaps[types[i]][j].effectName != null && effectMaps[types[i]][j].effectName != "")
								{
									if(GUILayout.Button("Show",GUILayout.Width(50f),GUILayout.Height(25f)))
									{
										mTarget.GenerateTempResource(effectMaps[types[i]][j],true);
									}
								}
							}
							if(effectMaps[types[i]][j].effectType == _EffectType.Shoot)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Shoot From",GUILayout.Width(80f));
								effectMaps[types[i]][j].effectTargetType = (_EffectTargetType)EditorGUILayout.EnumPopup(effectMaps[types[i]][j].effectTargetType,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Scope",GUILayout.Width(80f));
								effectMaps[types[i]][j].effectScopeType = (_EffectScopeType)EditorGUILayout.EnumPopup(effectMaps[types[i]][j].effectScopeType,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Shoot Track",GUILayout.Width(80f));
								effectMaps[types[i]][j].effectShootTrack = (_EffectShootTrack)EditorGUILayout.EnumPopup(effectMaps[types[i]][j].effectShootTrack,GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Effect Prefab",GUILayout.Width(80f));
								Object go = null;
								go = EditorGUILayout.ObjectField(go,typeof(GameObject),false,GUILayout.Width(80f));
								if(go!=null)
								{
									string path = AssetDatabase.GetAssetPath(go);
									int index = path.IndexOf("Resources/");
									if(index!=-1)
									{
										path = path.Replace(path.Substring(0,index +"Resources/".Length),"");
										path = path.Replace(".prefab","");
									}
									effectMaps[types[i]][j].effectName = path;
									go = null;
								}
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Effect Path",GUILayout.Width(80f));
								EditorGUILayout.LabelField(effectMaps[types[i]][j].effectName, GUILayout.Width(300f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Hit Effect Prefab",GUILayout.Width(80f));
								go = EditorGUILayout.ObjectField(go,typeof(GameObject),false,GUILayout.Width(80f));
								if(go!=null)
								{
									string path = AssetDatabase.GetAssetPath(go);
									int index = path.IndexOf("Resources/");
									if(index!=-1)
									{
										path = path.Replace(path.Substring(0,index +"Resources/".Length),"");
										path = path.Replace(".prefab","");
									}
									effectMaps[types[i]][j].hitPrefabName = path;
									go = null;
								}
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Hit Effect Path",GUILayout.Width(80f));
								EditorGUILayout.LabelField(effectMaps[types[i]][j].hitPrefabName, GUILayout.Width(300f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Fly Duration",GUILayout.Width(80f));
								effectMaps[types[i]][j].shootDurtion = EditorGUILayout.FloatField(effectMaps[types[i]][j].shootDurtion, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
								effectMaps[types[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[types[i]][j].delayTime, GUILayout.Width(80f));
								effectMaps[types[i]][j].delayTime = Mathf.Max(0,effectMaps[types[i]][j].delayTime);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Frequency",GUILayout.Width(80f));
								effectMaps[types[i]][j].frequency = EditorGUILayout.IntField(effectMaps[types[i]][j].frequency, GUILayout.Width(80f));
								effectMaps[types[i]][j].frequency = Mathf.Max(1,effectMaps[types[i]][j].frequency);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Interval",GUILayout.Width(80f));
								effectMaps[types[i]][j].interval = EditorGUILayout.FloatField(effectMaps[types[i]][j].interval, GUILayout.Width(80f));
								effectMaps[types[i]][j].interval = Mathf.Max(0,effectMaps[types[i]][j].interval);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Follow",GUILayout.Width(80f));
								effectMaps[types[i]][j].follow = (Transform)EditorGUILayout.ObjectField(effectMaps[types[i]][j].follow,typeof(Transform),true,GUILayout.Width(200f));
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								effectMaps[types[i]][j].position = EditorGUILayout.Vector3Field("Start Offset",effectMaps[types[i]][j].position, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								effectMaps[types[i]][j].rotation = EditorGUILayout.Vector3Field("Start Rotate",effectMaps[types[i]][j].rotation, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								effectMaps[types[i]][j].scale = EditorGUILayout.Vector3Field("Start Scale",effectMaps[types[i]][j].scale, GUILayout.Width(280f));
								EditorGUILayout.EndHorizontal();
							}
							if(effectMaps[types[i]][j].effectType == _EffectType.ChangeColor || effectMaps[types[i]][j].effectType == _EffectType.EdgeColor)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Color",GUILayout.Width(80f));
								Color previouColor = effectMaps[types[i]][j].color;
								effectMaps[types[i]][j].color = EditorGUILayout.ColorField(effectMaps[types[i]][j].color, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("To Color",GUILayout.Width(80f));
								Color previouToColor = effectMaps[types[i]][j].toColor;
								effectMaps[types[i]][j].toColor = EditorGUILayout.ColorField(effectMaps[types[i]][j].toColor, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Loop Type",GUILayout.Width(80f));
								effectMaps[types[i]][j].effectLoopType = (_EffectLoopType)EditorGUILayout.EnumPopup(effectMaps[types[i]][j].effectLoopType, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Anim Curve",GUILayout.Width(80f));
								effectMaps[types[i]][j].curve = EditorGUILayout.CurveField(effectMaps[types[i]][j].curve,GUILayout.Height(80f), GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
								effectMaps[types[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[types[i]][j].delayTime, GUILayout.Width(80f));
								effectMaps[types[i]][j].delayTime = Mathf.Max(0,effectMaps[types[i]][j].delayTime);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Frequency",GUILayout.Width(80f));
								effectMaps[types[i]][j].frequency = EditorGUILayout.IntField(effectMaps[types[i]][j].frequency, GUILayout.Width(80f));
								effectMaps[types[i]][j].frequency = Mathf.Max(1,effectMaps[types[i]][j].frequency);
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Interval",GUILayout.Width(80f));
								effectMaps[types[i]][j].interval = EditorGUILayout.FloatField(effectMaps[types[i]][j].interval, GUILayout.Width(80f));
								effectMaps[types[i]][j].interval = Mathf.Max(0,effectMaps[types[i]][j].interval);
								EditorGUILayout.EndHorizontal();

								if( effectMaps[types[i]][j].effectType == _EffectType.EdgeColor)
								{
									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.LabelField("Edge Size",GUILayout.Width(80f));
									effectMaps[types[i]][j].edgeSize = EditorGUILayout.FloatField(effectMaps[types[i]][j].edgeSize, GUILayout.Width(80f));
									effectMaps[types[i]][j].edgeSize = Mathf.Max(0,effectMaps[types[i]][j].edgeSize);
									EditorGUILayout.EndHorizontal();

									if(Application.isPlaying )
									{
										if(previouColor != effectMaps[types[i]][j].color)
										{
											mTarget.ChangeEdgeColor(effectMaps[types[i]][j].color,effectMaps[types[i]][j].edgeSize);
										}
										else if(previouToColor != effectMaps[types[i]][j].toColor)
										{
											mTarget.ChangeEdgeColor(effectMaps[types[i]][j].toColor,effectMaps[types[i]][j].edgeSize);
										}
									}
								}
							}
		//
							if(effectMaps[types[i]][j].effectType == _EffectType.Ghost)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
								effectMaps[types[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[types[i]][j].delayTime, GUILayout.Width(80f));
								effectMaps[types[i]][j].delayTime = Mathf.Max(0,effectMaps[types[i]][j].delayTime);
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Frequency",GUILayout.Width(80f));
								effectMaps[types[i]][j].frequency = EditorGUILayout.IntField(effectMaps[types[i]][j].frequency, GUILayout.Width(80f));
								effectMaps[types[i]][j].frequency = Mathf.Max(1,effectMaps[types[i]][j].frequency);
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Interval",GUILayout.Width(80f));
								effectMaps[types[i]][j].interval = EditorGUILayout.FloatField(effectMaps[types[i]][j].interval, GUILayout.Width(80f));
								effectMaps[types[i]][j].interval = Mathf.Max(0,effectMaps[types[i]][j].interval);
								EditorGUILayout.EndHorizontal();
							}

							if(effectMaps[types[i]][j].effectType == _EffectType.ChangeMaterial)
							{
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("PlayAfterAnim",GUILayout.Width(80f));
								effectMaps[types[i]][j].isPlayAfterAnim = EditorGUILayout.Toggle(effectMaps[types[i]][j].isPlayAfterAnim, GUILayout.Width(80f));
								EditorGUILayout.EndHorizontal();

								if(!effectMaps[types[i]][j].isPlayAfterAnim)
								{
									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.LabelField("Delay Time",GUILayout.Width(80f));
									effectMaps[types[i]][j].delayTime = EditorGUILayout.FloatField(effectMaps[types[i]][j].delayTime, GUILayout.Width(80f));
									effectMaps[types[i]][j].delayTime = Mathf.Max(0,effectMaps[types[i]][j].delayTime);
									EditorGUILayout.EndHorizontal();
								}

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Material",GUILayout.Width(80f));
								effectMaps[types[i]][j].material = EditorGUILayout.ObjectField(effectMaps[types[i]][j].material,typeof(Material),true, GUILayout.Width(80f)) as Material;
								EditorGUILayout.EndHorizontal();

							}

							if(GUILayout.Button("Del",GUILayout.Width(50f),GUILayout.Height(25f)))
							{
								effectMaps[types[i]].Remove(effectMaps[types[i]][j]);
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
                    mTarget.deathEffectAttrList.Add(ea);
                }
            }
            EditorGUILayout.EndHorizontal();
			NGUIEditorTools.EndContents();
		}
		EditorUtility.SetDirty (mTarget);
		serializedObject.ApplyModifiedProperties ();
	}

	void Clone(_EffectType type,EffectAttr ea)
	{
		GetAnimMaps();
	}

}
