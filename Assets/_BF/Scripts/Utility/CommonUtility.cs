using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Xft;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEditorInternal;
using System.Reflection;
#endif

public static class CommonUtility
{
	public static Texture2D InitTexture2D(int width,int height,Color c)
	{
		Texture2D _black = new Texture2D(width,height);
		for(int i=0;i<width;i++)
		{
			for(int j=0;j<height;j++)
			{
				_black.SetPixel(i,j,c);
			}
		}
		_black.Apply();
		return _black;
	}

	public static Sprite InitSprite(int width,int height,Color c)
	{
		Texture2D tex = InitTexture2D(width,height,c);
		Sprite sprite = Sprite.Create(tex,new Rect(0,0,width,height),new Vector2(width/2,height/2));
		return sprite;
	}

	public static List<T> GetAllComponents<T>(this GameObject go) where T : Component
	{
		List<T> puList = new List<T>();
//		T[] pus0 = go.GetComponents<T>();
//		if(pus0!=null && pus0.Length>0)
//			puList.AddRange(pus0);
		T[] pus = go.GetComponentsInChildren<T>();
		if(pus!=null && pus.Length>0)
			puList.AddRange(pus);
		return puList;
	}

	public static T GetOrAddComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if(t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	public static _AnimType AnimCilpNameStringToEnum(string clipName)
	{
		_AnimType animType = _AnimType.StandBy;
		switch(clipName)
		{
			case "Attack":animType = _AnimType.Attack;break;
			case "Cheer":animType = _AnimType.Cheer;break;
			case "Death":animType = _AnimType.Death;break;
			case "Hit":animType = _AnimType.Hit;break;
			case "Power":animType = _AnimType.Power;break;
			case "Run":animType = _AnimType.Run;break;
			case "Skill1":animType = _AnimType.Skill1;break;
			case "StandBy":animType = _AnimType.StandBy;break;
			case "Sprint":animType = _AnimType.Sprint;break;
			default :animType = _AnimType.None;break;
		}
		return animType;
	}

	public static string AnimCilpNameEnumToString(_AnimType animType)
	{
		string clipName = "StandBy";
		switch(animType)
		{
		case _AnimType.Attack:clipName =  "Attack";break;
		case _AnimType.Cheer:clipName =  "Cheer";break;
		case _AnimType.Death:clipName =  "Death";break;
		case _AnimType.Hit:clipName = "Hit";break;
		case _AnimType.Power:clipName =  "Power";break;
		case _AnimType.Run:clipName =  "Run";break;
		case _AnimType.Skill1:clipName = "Skill1";break;
		case _AnimType.StandBy:clipName =  "StandBy";break;
		case _AnimType.Sprint:clipName =  "Sprint";break;
		}
		return clipName;
	}

	public static float GetFragLength(AnimationFrag frag)
	{
		float fragLength = 0;
		fragLength += (frag.stopTime - frag.startTime) / frag.speed * frag.loopCount;
		if(frag.rewind)
		{
			fragLength += (frag.stopTime - frag.startTime) / frag.rewindSpeed * frag.loopCount;
		}
		return fragLength;
	}

	public static float GetFragsLength(List<AnimationFrag> frags)
	{
		float fragLength = 0;
		foreach(AnimationFrag frag in frags)
		{
			fragLength += GetFragLength(frag);
		}
		return fragLength;
	}

	public static float GetRealAnimLength(AnimMapping am)
	{
		float length = GetFragsLength(am.frags);
		if(length==0)
		{
			length = am.clip.length;
		}
		return length;
	}

	public static float GetRealClipPoint(List<AnimationFrag> frags,float t)
	{
		float realTime = t;
		foreach(AnimationFrag frag in frags)
		{
			float tmpT = frag.stopTime - frag.startTime;
			float tmpFragT = (frag.stopTime - frag.startTime) * frag.loopCount;
			if(realTime > tmpFragT)
			{
				realTime -=tmpFragT;
			}else{
				for(int i=0;i < frag.loopCount;i++)
				{
					if(realTime <= tmpT)
					{
						realTime += frag.startTime ;
						return realTime;
					}else{
						realTime -=tmpT;
					}
				}
			}
		}
		Debug.Log ("realTime:" + realTime);
		return realTime;
	}

	static public void ChangeMaterialWithMainTexture(List<Renderer> rss,Material mat)
	{
		foreach(Renderer sr in rss)
		{
			if (sr == null)
				continue;
			Texture tex = sr.material.mainTexture;
			sr.material = new Material(mat);
			if(sr.material.HasProperty("_MainTex"))
			{
				sr.material.SetTexture("_MainTex",tex);
			}
		}
	}

	static Shader edgeShader;
	static public void ChangeEdgeColor(List<SkinnedMeshRenderer> skinnedRs,List<SpriteRenderer> spriteRs,Color c,float edgeSize)
	{
		if(edgeShader==null)
		{
			edgeShader = Shader.Find("Custom/Edges");
		}
		foreach(SkinnedMeshRenderer sr in skinnedRs)
		{
			if(sr.name!="Character_Shadow" && sr.tag != "shadow")
			{
				Texture tex = sr.material.mainTexture;
				if(sr.material.shader != edgeShader)
				{
					sr.material = new Material(edgeShader);
					sr.material.mainTexture = tex;
				}
				sr.material.SetColor("_Color",c);
				sr.material.SetVector("_TexSize",new Vector4(tex.width/edgeSize,tex.height/edgeSize,0,0));
			}
		}
		foreach(SpriteRenderer sr in spriteRs)
		{
			if(sr.name!="Character_Shadow" && sr.tag != "shadow")
			{
				Texture tex = sr.sprite.texture;
				if(sr.material.shader != edgeShader)
				{
					sr.material = new Material(edgeShader);
				}
				sr.material.SetColor("_Color",c);
				sr.material.SetVector("_TexSize",new Vector4(tex.width/edgeSize,tex.height/edgeSize,0,0));
			}
		}
	}

	static Shader multiColorShader;
	static public void ChangeColor(List<Renderer> rss,Color c)
	{
		if(multiColorShader==null)
		{
			multiColorShader = Shader.Find("Custom/ChangeColor");
		}
		foreach(Renderer sr in rss)
		{
            if(sr!=null)
            {
                Texture tex = sr.material.mainTexture;
                if(sr.material.shader != multiColorShader)
                {
                    sr.material = new Material(multiColorShader);
                    sr.material.mainTexture = tex;
                }
                sr.material.SetColor("_Color",c);
            }
		}
	}
	
	static public List<string> InitClipTypes()
	{
		List<string> types = new List<string>();
		types.Add("Attack");
		types.Add("Skill1");
		types.Add("StandBy");
		types.Add("Run");
		types.Add("Death");
		types.Add("Hit");
		types.Add("Cheer");
		types.Add("Power");
		types.Add("Sprint");
		return types;
	}

	static public _AnimType[] GetAnimTypes()
	{
		_AnimType[] types = new _AnimType[9];
		types[0] = _AnimType.Attack;
		types[1] = _AnimType.Skill1;
		types[2] = _AnimType.StandBy;
		types[3] = _AnimType.Run;
		types[4] = _AnimType.Death;
		types[5] = _AnimType.Hit;
		types[6] = _AnimType.Cheer;
		types[7] = _AnimType.Power;
		types[8] = _AnimType.Sprint;
		return types;
	}

	static public GUIStyle GetErrorGUIStyle()
	{
		GUIStyle errorLabelStyle = new GUIStyle();
		errorLabelStyle.normal.textColor = Color.red;
		errorLabelStyle.fontStyle = FontStyle.Bold;
		errorLabelStyle.richText = true;
		return errorLabelStyle;
	}

	static public float GetColliderTop(BoxCollider2D collider)
	{
		return collider.offset.y + collider.size.y / 2;
	}

	static public Transform FindChild(Transform t,string childName)
	{
		return _FindChild(t,childName);
	}

	static Transform _FindChild(Transform t,string childName)
	{
		for(int i=0;i<t.childCount;i++)
		{
			if(t.GetChild(i).name == childName)
			{
				return t.GetChild(i);
			}
			else
			{
				Transform t0 = _FindChild(t.GetChild(i),childName);
				if(t0!=null)
				{
					return t0;
				}
			}
		}
		return null;
	}

	static public void SetOrderLayer(string orderLayer,HeroRes heroRes)
	{
		foreach(SkinnedMeshRenderer smr in heroRes.GetComponentsInChildren<SkinnedMeshRenderer>(true))
		{
			smr.sortingLayerName = orderLayer;
		}
		foreach(SpriteRenderer sr in heroRes.GetComponentsInChildren<SpriteRenderer>(true))
		{
			sr.sortingLayerName = orderLayer;
		}
		if(heroRes.Shadow!=null && heroRes.Shadow.GetComponent<SpriteRenderer>()!=null)
		{
			heroRes.Shadow.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
		}
	}

	static public void SetSortingLayerWithChildren(GameObject go,string layerName)
	{
		Renderer[] rs = go.GetComponentsInChildren<Renderer>(true);
		foreach(Renderer r in rs)
		{
			r.sortingLayerName = layerName;
		}
		XffectComponent[] xfts = go.GetComponentsInChildren<XffectComponent>(true);
		foreach(XffectComponent xft in xfts)
		{
			xft.LayerName = layerName;
		}
	}

	public static  string GetBundleTypeByDevice()
	{
		string bundleType = "";
#if UNITY_ANDROID
		bundleType = "_Android";
#elif UNITY_IPHONE
		bundleType = "_IPhone";
#else
		bundleType = "";
#endif
		return bundleType;
	}

#if UNITY_EDITOR

	// Get the sorting layer names
	static public string[] GetSortingLayerNames() {
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
	
	// Get the unique sorting layer IDs -- tossed this in for good measure
	static public int[] GetSortingLayerUniqueIDs() {
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
	}

	public static string[] GetFileByRemoveHeroDirectory()
	{
#if UNITY_WEBPLAYER
        string[] paths = null;
        Debug.LogError("Can't get files on WebPlayer");
#else
        string[] paths = Directory.GetFiles(@"\\192.168.1.152\htdocs\Hero\","*.unity3d",SearchOption.AllDirectories);
#endif
        string deviceType = CommonUtility.GetBundleTypeByDevice();
		for(int i=0;i<paths.Length;i++)
		{
			string heroId = paths[i].Replace(deviceType + ".unity3d","");
			heroId = heroId.Replace(@"\\192.168.1.152\htdocs\Hero\","");
			paths[i] = heroId;
		}
		return paths;
	}

	static string localHeroPath = "/_BF/Prefabs/Hero";
	public static List<GameObject> GetAllHeros()
	{
		string[] paths = CommonUtility.GetFileByDirectory(localHeroPath);
		List<GameObject> hre = new List<GameObject>();
		foreach(string str in paths)
		{
			string assetPath = "Assets" + str.Replace(Application.dataPath, "").Replace('\\', '/');
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath,typeof(GameObject));
			if(prefab!=null)
			{
				hre.Add(prefab);
			}
		}
		return hre;
	}

	public static string[] GetFileByDirectory(string path)
	{
#if UNITY_WEBPLAYER
        string[] paths = null;
        Debug.LogError("Can't get file on WebPlayer");
#else
        string[] paths = Directory.GetFiles(Application.dataPath + path,"*.prefab",SearchOption.AllDirectories);
#endif
		return paths;
	}

	public static string[] GetSpriteFolderByDirectory(string path)
	{
#if UNITY_WEBPLAYER
        string[] paths = null;
        Debug.LogError("Can't get file on WebPlayer");
#else
        string[] paths = Directory.GetFiles(Application.dataPath + path,"*.png",SearchOption.TopDirectoryOnly);
#endif
		return paths;
	}
#endif



}


