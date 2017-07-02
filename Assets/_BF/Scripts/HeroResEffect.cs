using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum _AnimType{Attack,Skill1,StandBy,Run,Hit,Death,Cheer,Power,Sprint,None}
public enum _EffectType{Cast,Shoot,Ghost,ChangeColor,EdgeColor,ChangeMaterial,PosChange}
public enum _EffectTargetType{Target,Self}
public enum _EffectShootTrack{Line,Curve,Projection,Missle}
public enum _EffectScopeType{Single,Scope}
public enum _EffectLoopType{Default,Loop,PingPong}
[ExecuteInEditMode]
public class HeroResEffect : MonoBehaviour {

	public const string parentPath = "Effect/Hero_Effect/";
//	public static Transform leftEffectPos;
//	public static Transform rightEffectPos;
	public float gizmosSize = 0.5f;
//	public List<EffectAttr> effectAttrList = new List<EffectAttr>();
	public List<EffectAttr> attackEffectAttrList = new List<EffectAttr>();
	public List<EffectAttr> skillEffeectAttrList = new List<EffectAttr>();
	public List<EffectAttr> standbyEffectAttrList = new List<EffectAttr>();
	public List<EffectAttr> runEffectAttrList = new List<EffectAttr>();
	public List<EffectAttr> deathEffectAttrList = new List<EffectAttr>();
	public List<EffectAttr> hitEffectAttrList = new List<EffectAttr>();
	public List<EffectAttr> cheerEffectAttrList = new List<EffectAttr>();
	public List<EffectAttr> powerEffectAttrList = new List<EffectAttr>();
	public List<EffectAttr> sprintEffectAttrList = new List<EffectAttr>();

	public bool enableHitEffect = true;

	Dictionary<_AnimType,List<EffectAttr>> effectMaps = null;
	Dictionary<SkinnedMeshRenderer,Material> mMeshRenders;
	Dictionary<SpriteRenderer,Material> mSpriteRenders;
	Dictionary<SkinnedMeshRenderer,Material> mCurMeshRenders;
	Dictionary<SpriteRenderer,Material> mCurSpriteRenders;
	SkinnedMeshRenderer[] skinnedRs;
	SpriteRenderer[] spriteRs;

	public static Color hitColor = new Color(248.0f/255,78.0f/255,117.0f/255);
	bool mIsColorChanged;
	bool mIsEdgeColorChanged;
	bool mIsMaterialChanged;
	bool mIsMeshBaking;
	Shader mShader;
	Dictionary<MeshFilter,float> meshFilters = new Dictionary<MeshFilter, float>();
	Dictionary<SpriteRenderer,float> spriteRenders = new Dictionary<SpriteRenderer, float>();
	Shader mEdgeShader;

	void Awake()
	{
		mShader = Shader.Find("Custom/UnlitTransparent");
		mEdgeShader = Shader.Find("Custom/Edges");
		skinnedRs=GetComponentsInChildren<SkinnedMeshRenderer>();
		spriteRs=GetComponentsInChildren<SpriteRenderer>();
//		if(Application.isPlaying)
//		{
//			mMeshRenders = new Dictionary<SkinnedMeshRenderer,Material>();
//			foreach(SkinnedMeshRenderer sr in skinnedRs)
//			{
//				if(sr.name!="Character_Shadow" && sr.tag != "shadow")
//				{
//					mMeshRenders.Add(sr,sr.material);
//				}
//			}
//			mSpriteRenders = new Dictionary<SpriteRenderer, Material>();
//			foreach(SpriteRenderer sr in spriteRs)
//			{
//				if(sr.name!="Character_Shadow" && sr.tag != "shadow")
//				{
//					mSpriteRenders.Add(sr,sr.material);
//				}
//			}
//		}
		effectMaps = new Dictionary<_AnimType,List<EffectAttr>>();
		effectMaps.Add(_AnimType.Attack,attackEffectAttrList);
		effectMaps.Add(_AnimType.Skill1,skillEffeectAttrList);
		effectMaps.Add(_AnimType.StandBy,standbyEffectAttrList);
		effectMaps.Add(_AnimType.Run,runEffectAttrList);
		effectMaps.Add(_AnimType.Death,deathEffectAttrList);
		effectMaps.Add(_AnimType.Hit,hitEffectAttrList);
		effectMaps.Add(_AnimType.Cheer,cheerEffectAttrList);
		effectMaps.Add(_AnimType.Power,powerEffectAttrList);
		effectMaps.Add(_AnimType.Sprint,sprintEffectAttrList);
		Clear();
//#if UNITY_EDITOR
//		TestEffectController controller = FindObjectOfType<TestEffectController>();
//		if(controller!=null && transform.parent == null)
//		{
//			HeroResEffect[] hes = FindObjectsOfType<HeroResEffect>();
//			foreach(HeroResEffect he in hes)
//			{
//				if(he != this && he.transform.parent == null)
//				{
//					DestroyImmediate(he.gameObject);
//				}
//			}
//			if(gameObject.tag != "static")
//			{
//				transform.position = controller.playerPos.position;
//				controller.prefab = gameObject;
//				controller.Init(gameObject);
//			}
//		}
//		if(!Application.isPlaying)
//		{
//			Caching.CleanCache();
//		}
//#endif
	}

//	void Start () {
//		if(leftEffectPos==null)
//			leftEffectPos = GameObject.Find("LeftEffectPos").transform;
//		if(rightEffectPos==null)
//			rightEffectPos = GameObject.Find("RightEffectPos").transform;
//	}

	void Update () {
#if UNITY_EDITOR
		if(!Application.isPlaying)
		{
			AttachPrefab(attackEffectAttrList);
			AttachPrefab(skillEffeectAttrList);
			AttachPrefab(standbyEffectAttrList);
			AttachPrefab(runEffectAttrList);
			AttachPrefab(deathEffectAttrList);
			AttachPrefab(hitEffectAttrList);
			AttachPrefab(cheerEffectAttrList);
			AttachPrefab(powerEffectAttrList);
			AttachPrefab(sprintEffectAttrList);
			Caching.CleanCache();
		}
#endif
	}

	public void ChangeMaterial(Material mat)
	{
		foreach(SkinnedMeshRenderer sr in skinnedRs)
		{
			if(sr.name!="Character_Shadow" && sr.tag != "shadow")
			{
				if(!mIsMaterialChanged)
				{
					sr.material = new Material(mat);
				}
			}
		}
		foreach(SpriteRenderer sr in spriteRs)
		{
			if(sr.name!="Character_Shadow" && sr.tag != "shadow")
			{
				if(!mIsMaterialChanged)
				{
					sr.material = new Material(mat);
				}
			}
		}
		mIsMaterialChanged = true;
		mIsColorChanged = false;
		mIsEdgeColorChanged = false;
	}

	public void ChangeColor(Color c)
	{
		foreach(SkinnedMeshRenderer sr in skinnedRs)
		{
			if(sr.name!="Character_Shadow" && sr.tag != "shadow")
			{
				if(!mIsColorChanged)
				{
					Texture tex = sr.material.mainTexture;
					sr.material = new Material(mShader);
					sr.material.mainTexture = tex;
				}
				sr.material.SetColor("_Color",c);
			}
		}
		foreach(SpriteRenderer sr in spriteRs)
		{
			if(sr.name!="Character_Shadow" && sr.tag != "shadow")
			{
				if(!mIsColorChanged)
				{
					sr.material = new Material(mShader);
				}
				sr.material.SetColor("_Color",c);
			}
		}
		mIsColorChanged = true;
		mIsEdgeColorChanged = false;
	}

	public void ChangeEdgeColor(Color c,float edgeSize)
	{
		foreach(SkinnedMeshRenderer sr in skinnedRs)
		{
			if(sr.name!="Character_Shadow" && sr.tag != "shadow")
			{
				Texture tex = sr.material.mainTexture;
				if(!mIsEdgeColorChanged)
				{
					sr.material = new Material(mEdgeShader);
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
				if(!mIsEdgeColorChanged)
				{
					sr.material = new Material(mEdgeShader);
				}
				sr.material.SetColor("_Color",c);
				sr.material.SetVector("_TexSize",new Vector4(tex.width/edgeSize,tex.height/edgeSize,0,0));
			}
		}
		mIsColorChanged = false;
		mIsEdgeColorChanged = true;
	}

	public void RevertColor()
	{
		foreach(SkinnedMeshRenderer sr in mMeshRenders.Keys)
		{
			sr.material = mMeshRenders[sr];
		}
		foreach(SpriteRenderer sr in mSpriteRenders.Keys)
		{
			sr.material = mSpriteRenders[sr];
		}
		mIsColorChanged = false;
		mIsEdgeColorChanged = false;
	}

	void Clear()
	{
		foreach(_AnimType at in effectMaps.Keys)
		{
			foreach(EffectAttr ea in effectMaps[at])
			{
#if UNITY_EDITOR
				ea.test = false;
#endif
				ea.effectPrefab = null;
				ea.effectObject = null;
			}
		}
	}

	void AttachPrefab(List<EffectAttr> effectAttrs)
	{
#if UNITY_EDITOR 
		if(!Application.isPlaying)
		{
			foreach(EffectAttr ea in effectAttrs)
			{
				if(ea.tempPrefab !=null)
				{
					ea.effectName = AssetDatabase.GetAssetPath(ea.tempPrefab);
					int index = ea.effectName.IndexOf("Resources/");
					if(index!=-1)
					{
						ea.effectName = ea.effectName.Replace(ea.effectName.Substring(0,index +"Resources/".Length),"");
						ea.effectName = ea.effectName.Replace(".prefab","");
					}
					GenerateTempResource(ea,true);
					ea.tempPrefab = null;
				}
				if(ea.tempHitPrefab != null)
				{
					ea.hitEffectName = AssetDatabase.GetAssetPath(ea.tempHitPrefab);
					int index = ea.hitEffectName.IndexOf("Resources/");
					if(index!=-1)
					{
						ea.hitEffectName = ea.hitEffectName.Replace(ea.hitEffectName.Substring(0,index +"Resources/".Length),"");
						ea.hitEffectName = ea.hitEffectName.Replace(".prefab","");
					}
					ea.tempHitPrefab = null;
				}
				if(ea.test)
				{
					GenerateTempResource(ea,false);
					ea.test = false;
				}
			}
		}
#endif
	}

#if UNITY_EDITOR
	public TestEffectController testController;
	public GameObject GenerateTempResource(EffectAttr ea,bool isDefaultScale)
	{
		Debug.Log("GenerateTempResource");
		if(ea.testObject!=null)
		{
			DestroyImmediate(ea.testObject);
		}
		GameObject prefab = Resources.Load(ea.effectName) as GameObject;
		GameObject go = Instantiate(prefab,transform.position,Quaternion.identity) as GameObject;
		ea.testObject = go;
		if(ea.testObject!=null)
		{
			if(!go.GetComponent<EffectFollow>())
			{
				EffectFollow ef = go.AddComponent<EffectFollow>();
				ef.hideFlags = HideFlags.HideInInspector;
				ef.parentGo = transform;
				ef.ea = ea;
			}
			if(isDefaultScale)
			{
				ea.scale = ea.testObject.transform.localScale ;
			}
			else
			{
				ea.testObject.transform.localScale  = ea.scale;
			}
			if(ea.follow != null)
			{
				ea.testObject.transform.position = ea.follow.position;
//				ea.testObject.transform.parent = ea.follow;
			}
			else
			{
				ea.testObject.transform.position = transform.position + ea.position;
			}
			ea.testObject.transform.localEulerAngles = ea.rotation;
			ea.testObject.transform.localScale = ea.scale;
			testController = GameObject.Find("_EffectController").GetComponent<TestEffectController>();
			foreach(GameObject go0 in testController.testObjects)
			{
				DestroyImmediate(go0);
			}
			testController.testObjects.Clear();
			testController.testObjects.Add(ea.testObject);
		}
		return ea.testObject;
	}
#endif

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		foreach(EffectAttr ea in attackEffectAttrList)
		{
			Vector3 pos = ea.follow == null ? ea.position + transform.position : ea.follow.position;
			Gizmos.DrawWireCube(pos,Vector3.one * gizmosSize);
		}
		Gizmos.color = Color.green;
		foreach(EffectAttr ea in standbyEffectAttrList)
		{
			Vector3 pos = ea.follow == null ? ea.position + transform.position : ea.follow.position;
			Gizmos.DrawWireCube(pos,Vector3.one * gizmosSize);
  		}
	}
}

[System.Serializable]
public class EffectAttr
{
	public _EffectType effectType;
	public _EffectTargetType effectTargetType;
	public _EffectShootTrack effectShootTrack;
	public _EffectScopeType effectScopeType;
	public _EffectLoopType effectLoopType = _EffectLoopType.Default;
#if UNITY_EDITOR
	public GameObject tempPrefab;
#endif
	public List<GameObject> targets;
	public string effectName;
	public string hitPrefabName;

	[HideInInspector]public GameObject effectPrefab;
	[HideInInspector]public GameObject hitEffectPrefab;
	
	public GameObject effectObject;
	public float delayTime;
	public int frequency = 1;
	public float interval;

	public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
	public float loopDuration = 1;

	public Material material;
	public float frequencySpace = 0.033f;
	public Color color = Color.white;
	public Color toColor = Color.white;
	public float edgeSize = 2;

	public float maxProjectionY;
	public float minProjectionY;

	public Vector3 rotation;
	public Vector3 position;
	public Vector3 scale = Vector3.one;
	public Transform follow;
	public bool isPlayAfterAnim;
	public float shootDurtion = 0.2f;
	public GameObject tempHitPrefab;
	public string hitEffectName;

	[HideInInspector]public bool isParticleInited;
	[HideInInspector]public List<ParticleUtility> particleUtilitys;
	[HideInInspector]public bool isNcAnimatin;
	[HideInInspector]public List<NcUvAnimation> ncUvAnimations; 
	public bool autoDestory;
	public float destoryDelay = 5;
	public bool ignoreTimeScale;
#if UNITY_EDITOR
	public bool test = false;
	[HideInInspector]public GameObject testObject;
#endif
}
