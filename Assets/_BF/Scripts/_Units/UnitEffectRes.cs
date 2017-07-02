using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class UnitEffectRes : MonoBehaviour
{

	public Unit unit;
//	public SkeletonRenderer mSkeletonRenderer;
	public Dictionary<_UnitArtActionType,List<EffectAttr>> effect_maps;
	public List<EffectAttr> atk_0001_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> atk_0101_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> atk_0102_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> atk_0103_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> atk_0201_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> atk_0202_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> atk_0203_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> cmn_0001_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> cmn_0002_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> cmn_0003_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> cmn_0004_attrs_list = new List<EffectAttr> ();
	public List<EffectAttr> cmn_0006_attrs_list = new List<EffectAttr> ();

	public const string parentPath = "Effect/Hero_Effect/";
	public float gizmosSize = 0.5f;
	//	public List<EffectAttr> attackEffectAttrList = new List<EffectAttr>();
	//	public List<EffectAttr> skillEffeectAttrList = new List<EffectAttr>();
	//	public List<EffectAttr> standbyEffectAttrList = new List<EffectAttr>();
	//	public List<EffectAttr> runEffectAttrList = new List<EffectAttr>();
	//	public List<EffectAttr> deathEffectAttrList = new List<EffectAttr>();
	//	public List<EffectAttr> hitEffectAttrList = new List<EffectAttr>();
	//	public List<EffectAttr> cheerEffectAttrList = new List<EffectAttr>();
	//	public List<EffectAttr> powerEffectAttrList = new List<EffectAttr>();
	//	public List<EffectAttr> sprintEffectAttrList = new List<EffectAttr>();

	public bool enableHitEffect = true;
	Dictionary<_AnimType,List<EffectAttr>> effectMaps = null;
	Dictionary<SkinnedMeshRenderer,Material> mMeshRenders;
	Dictionary<SpriteRenderer,Material> mSpriteRenders;
	Dictionary<SkinnedMeshRenderer,Material> mCurMeshRenders;
	Dictionary<SpriteRenderer,Material> mCurSpriteRenders;
	SkinnedMeshRenderer[] skinnedRs;
	SpriteRenderer[] spriteRs;
	public static Color hitColor = new Color (248.0f / 255, 78.0f / 255, 117.0f / 255);
	bool mIsColorChanged;
	bool mIsEdgeColorChanged;
	bool mIsMaterialChanged;
	bool mIsMeshBaking;
	Shader mShader;
	Dictionary<MeshFilter,float> meshFilters = new Dictionary<MeshFilter, float> ();
	Dictionary<SpriteRenderer,float> spriteRenders = new Dictionary<SpriteRenderer, float> ();
	Shader mEdgeShader;

	void Awake ()
	{
		GetEffectDic ();
		mShader = Shader.Find ("Custom/UnlitTransparent");
		mEdgeShader = Shader.Find ("Custom/Edges");
		skinnedRs = GetComponentsInChildren<SkinnedMeshRenderer> ();
		spriteRs = GetComponentsInChildren<SpriteRenderer> ();
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
//		effectMaps = new Dictionary<_AnimType,List<EffectAttr>>();
//		effectMaps.Add(_AnimType.Attack,attackEffectAttrList);
//		effectMaps.Add(_AnimType.Skill1,skillEffeectAttrList);
//		effectMaps.Add(_AnimType.StandBy,standbyEffectAttrList);
//		effectMaps.Add(_AnimType.Run,runEffectAttrList);
//		effectMaps.Add(_AnimType.Death,deathEffectAttrList);
//		effectMaps.Add(_AnimType.Hit,hitEffectAttrList);
//		effectMaps.Add(_AnimType.Cheer,cheerEffectAttrList);
//		effectMaps.Add(_AnimType.Power,powerEffectAttrList);
//		effectMaps.Add(_AnimType.Sprint,sprintEffectAttrList);
		Clear ();
	}

	public Dictionary<_UnitArtActionType,List<EffectAttr>> GetEffectDic(){
		if (effect_maps == null) {
			effect_maps = new Dictionary<_UnitArtActionType, List<EffectAttr>> ();
			effect_maps.Add (_UnitArtActionType.atk_0001, atk_0001_attrs_list);
			effect_maps.Add (_UnitArtActionType.atk_0101, atk_0101_attrs_list);
			effect_maps.Add (_UnitArtActionType.atk_0102, atk_0102_attrs_list);
			effect_maps.Add (_UnitArtActionType.atk_0103, atk_0103_attrs_list);
			effect_maps.Add (_UnitArtActionType.atk_0201, atk_0201_attrs_list);
			effect_maps.Add (_UnitArtActionType.atk_0202, atk_0202_attrs_list);
			effect_maps.Add (_UnitArtActionType.atk_0203, atk_0203_attrs_list);
			effect_maps.Add (_UnitArtActionType.cmn_0001, cmn_0001_attrs_list);
			effect_maps.Add (_UnitArtActionType.cmn_0002, cmn_0002_attrs_list);
			effect_maps.Add (_UnitArtActionType.cmn_0003, cmn_0003_attrs_list);
			effect_maps.Add (_UnitArtActionType.cmn_0004, cmn_0004_attrs_list);
			effect_maps.Add (_UnitArtActionType.cmn_0006, cmn_0006_attrs_list);
		}
		return effect_maps;
	}


	void Update ()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying) {
//			AttachPrefab (attackEffectAttrList);
//			AttachPrefab (skillEffeectAttrList);
//			AttachPrefab (standbyEffectAttrList);
//			AttachPrefab (runEffectAttrList);
//			AttachPrefab (deathEffectAttrList);
//			AttachPrefab (hitEffectAttrList);
//			AttachPrefab (cheerEffectAttrList);
//			AttachPrefab (powerEffectAttrList);
//			AttachPrefab (sprintEffectAttrList);
			Caching.CleanCache ();
		}
		#endif
	}

	public void ChangeMaterial (Material mat)
	{
		foreach (SkinnedMeshRenderer sr in skinnedRs) {
			if (sr.name != "Character_Shadow" && sr.tag != "shadow") {
				if (!mIsMaterialChanged) {
					sr.material = new Material (mat);
				}
			}
		}
		foreach (SpriteRenderer sr in spriteRs) {
			if (sr.name != "Character_Shadow" && sr.tag != "shadow") {
				if (!mIsMaterialChanged) {
					sr.material = new Material (mat);
				}
			}
		}
		mIsMaterialChanged = true;
		mIsColorChanged = false;
		mIsEdgeColorChanged = false;
	}

	public void ChangeColor (Color c)
	{
		foreach (SkinnedMeshRenderer sr in skinnedRs) {
			if (sr.name != "Character_Shadow" && sr.tag != "shadow") {
				if (!mIsColorChanged) {
					Texture tex = sr.material.mainTexture;
					sr.material = new Material (mShader);
					sr.material.mainTexture = tex;
				}
				sr.material.SetColor ("_Color", c);
			}
		}
		foreach (SpriteRenderer sr in spriteRs) {
			if (sr.name != "Character_Shadow" && sr.tag != "shadow") {
				if (!mIsColorChanged) {
					sr.material = new Material (mShader);
				}
				sr.material.SetColor ("_Color", c);
			}
		}
		mIsColorChanged = true;
		mIsEdgeColorChanged = false;
	}

	public void ChangeEdgeColor (Color c, float edgeSize)
	{
		foreach (SkinnedMeshRenderer sr in skinnedRs) {
			if (sr.name != "Character_Shadow" && sr.tag != "shadow") {
				Texture tex = sr.material.mainTexture;
				if (!mIsEdgeColorChanged) {
					sr.material = new Material (mEdgeShader);
					sr.material.mainTexture = tex;
				}
				sr.material.SetColor ("_Color", c);
				sr.material.SetVector ("_TexSize", new Vector4 (tex.width / edgeSize, tex.height / edgeSize, 0, 0));
			}
		}
		foreach (SpriteRenderer sr in spriteRs) {
			if (sr.name != "Character_Shadow" && sr.tag != "shadow") {
				Texture tex = sr.sprite.texture;
				if (!mIsEdgeColorChanged) {
					sr.material = new Material (mEdgeShader);
				}
				sr.material.SetColor ("_Color", c);
				sr.material.SetVector ("_TexSize", new Vector4 (tex.width / edgeSize, tex.height / edgeSize, 0, 0));
			}
		}
		mIsColorChanged = false;
		mIsEdgeColorChanged = true;
	}

	public void RevertColor ()
	{
		foreach (SkinnedMeshRenderer sr in mMeshRenders.Keys) {
			sr.material = mMeshRenders [sr];
		}
		foreach (SpriteRenderer sr in mSpriteRenders.Keys) {
			sr.material = mSpriteRenders [sr];
		}
		mIsColorChanged = false;
		mIsEdgeColorChanged = false;
	}

	void Clear ()
	{
//		foreach (_AnimType at in effectMaps.Keys) {
//			foreach (EffectAttr ea in effectMaps[at]) {
//				#if UNITY_EDITOR
//				ea.test = false;
//				#endif
//				ea.effectPrefab = null;
//				ea.effectObject = null;
//			}
//		}
	}

	void AttachPrefab (List<EffectAttr> effectAttrs)
	{
		#if UNITY_EDITOR 
		if (!Application.isPlaying) {
			foreach (EffectAttr ea in effectAttrs) {
				if (ea.tempPrefab != null) {
					ea.effectName = AssetDatabase.GetAssetPath (ea.tempPrefab);
					int index = ea.effectName.IndexOf ("Resources/");
					if (index != -1) {
						ea.effectName = ea.effectName.Replace (ea.effectName.Substring (0, index + "Resources/".Length), "");
						ea.effectName = ea.effectName.Replace (".prefab", "");
					}
					GenerateTempResource (ea, true);
					ea.tempPrefab = null;
				}
				if (ea.tempHitPrefab != null) {
					ea.hitEffectName = AssetDatabase.GetAssetPath (ea.tempHitPrefab);
					int index = ea.hitEffectName.IndexOf ("Resources/");
					if (index != -1) {
						ea.hitEffectName = ea.hitEffectName.Replace (ea.hitEffectName.Substring (0, index + "Resources/".Length), "");
						ea.hitEffectName = ea.hitEffectName.Replace (".prefab", "");
					}
					ea.tempHitPrefab = null;
				}
				if (ea.test) {
					GenerateTempResource (ea, false);
					ea.test = false;
				}
			}
		}
		#endif
	}

	#if UNITY_EDITOR
	public TestEffectController testController;

	public GameObject GenerateTempResource (EffectAttr ea, bool isDefaultScale)
	{
		Debug.Log ("GenerateTempResource");
		if (ea.testObject != null) {
			DestroyImmediate (ea.testObject);
		}
		GameObject prefab = Resources.Load (ea.effectName) as GameObject;
		GameObject go = Instantiate (prefab, transform.position, Quaternion.identity) as GameObject;
		ea.testObject = go;
		if (ea.testObject != null) {
			if (!go.GetComponent<EffectFollow> ()) {
				EffectFollow ef = go.AddComponent<EffectFollow> ();
				ef.hideFlags = HideFlags.HideInInspector;
				ef.parentGo = transform;
				ef.ea = ea;
			}
			if (isDefaultScale) {
				ea.scale = ea.testObject.transform.localScale;
			} else {
				ea.testObject.transform.localScale = ea.scale;
			}
			if (ea.follow != null) {
				ea.testObject.transform.position = ea.follow.position;
				//				ea.testObject.transform.parent = ea.follow;
			} else {
				ea.testObject.transform.position = transform.position + ea.position;
			}
			ea.testObject.transform.localEulerAngles = ea.rotation;
			ea.testObject.transform.localScale = ea.scale;
			testController = GameObject.Find ("_EffectController").GetComponent<TestEffectController> ();
			foreach (GameObject go0 in testController.testObjects) {
				DestroyImmediate (go0);
			}
			testController.testObjects.Clear ();
			testController.testObjects.Add (ea.testObject);
		}
		return ea.testObject;
	}
	#endif

	//	void OnDrawGizmos()
	//	{
	//		Gizmos.color = Color.blue;
	//		foreach(EffectAttr ea in attackEffectAttrList)
	//		{
	//			Vector3 pos = ea.follow == null ? ea.position + transform.position : ea.follow.position;
	//			Gizmos.DrawWireCube(pos,Vector3.one * gizmosSize);
	//		}
	//		Gizmos.color = Color.green;
	//		foreach(EffectAttr ea in standbyEffectAttrList)
	//		{
	//			Vector3 pos = ea.follow == null ? ea.position + transform.position : ea.follow.position;
	//			Gizmos.DrawWireCube(pos,Vector3.one * gizmosSize);
	//		}
	//	}
}
