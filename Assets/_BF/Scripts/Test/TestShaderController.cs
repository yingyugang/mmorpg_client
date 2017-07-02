using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TestShaderController : MonoBehaviour {

	public GameObject testObj;
	public Color color;

	Shader mShader;
	List<Material> mats;
	bool mStarted;
	VortexEffect mVortexEffect;
	Vector3 defaultScale ;
	float t;

	public float speed  = 2000;
	public AnimationCurve ac;

	void Awake()
	{
		mShader = Shader.Find("Custom/Edges");
//		mVortexEffect = GetComponent<VortexEffect>();
		defaultScale = testObj.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
//		if(mStarted)
//		{
//			t += Time.deltaTime;
//			mVortexEffect.angle += Time.deltaTime * speed;
//			testObj.transform.localScale = Vector3.Lerp(defaultScale,Vector3.zero,ac.Evaluate(t)) ;
////			if(ac!=null)
////				ac.Evaluate(t);
//		}
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,100,30),"Test"))
		{
			if(testObj!=null)
			{
//				mats = new List<Material>();
//				SkinnedMeshRenderer[] skinnedMeshRenderers = testObj.GetComponentsInChildren<SkinnedMeshRenderer>();
//				foreach(SkinnedMeshRenderer sr in skinnedMeshRenderers)
//				{
//					Texture tex = sr.material.mainTexture;
//					sr.material = new Material(mShader);
//					sr.material.SetTexture("_MainTex",tex);
//					mats.Add(sr.material);
//				}
//
//				SpriteRenderer[] spriteRenderers = testObj.GetComponentsInChildren<SpriteRenderer>();
//				foreach(SpriteRenderer sr in spriteRenderers)
//				{
//					sr.material = new Material(mShader);
//					mats.Add(sr.material);
//				}
//				ChangeColor(testObj,Color.red);
				AddEdges(testObj,color);


			}
		}

		if(GUI.Button(new Rect(10,50,100,30),"Begin"))
		{
			mStarted = true;
		}
	}

	public void AddEdges(GameObject obj,Color c)
	{
		SkinnedMeshRenderer[] mrs = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach(SkinnedMeshRenderer sr in mrs)
		{
			if(sr.name!="Character_Shadow" && sr.tag != "shadow")
			{
//				mMeshRenders.Add(sr,sr.material);
				Texture tex = sr.material.mainTexture;
				sr.material = new Material(mShader);
				sr.material.mainTexture = tex;
				sr.material.SetColor("_Color",c);
				sr.material.SetVector("_TexSize",new Vector4(tex.width/2,tex.height/2,0,0));
//				sr.sortingLayerName = "Default";
			}
		}
//		mSpriteRenders = new Dictionary<SpriteRenderer, Material>();
		SpriteRenderer[] srs = obj.GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer sr in srs)
		{
			if(sr.name!="Character_Shadow" && sr.tag != "shadow")
			{
//				mSpriteRenders.Add(sr,sr.material);
				Texture tex = sr.sprite.texture;
				sr.material = new Material(mShader);
				sr.material.SetColor("_Color",c);
				sr.material.SetVector("_TexSize",new Vector4(tex.width/2,tex.height/2,0,0));
//				sr.sortingLayerName = "Default";

			}
		}
	}


	Dictionary<SkinnedMeshRenderer,Material> mMeshRenders;
	Dictionary<SpriteRenderer,Material> mSpriteRenders;

	bool mIsColorChanged;
	public void ChangeColor(GameObject obj,Color c)
	{
		if(!mIsColorChanged)
		{
			mMeshRenders = new Dictionary<SkinnedMeshRenderer,Material>();
			SkinnedMeshRenderer[] mrs = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach(SkinnedMeshRenderer sr in mrs)
			{
				if(sr.name!="Character_Shadow" && sr.tag != "shadow")
				{
					mMeshRenders.Add(sr,sr.material);
					Texture tex = sr.material.mainTexture;
					sr.material = new Material(mShader);
					sr.material.mainTexture = tex;
					sr.material.SetColor("_Color",c);
					sr.sortingLayerName = "Default";
				}
			}
			mSpriteRenders = new Dictionary<SpriteRenderer, Material>();
			SpriteRenderer[] srs = obj.GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer sr in srs)
			{
				if(sr.name!="Character_Shadow" && sr.tag != "shadow")
				{
					mSpriteRenders.Add(sr,sr.material);
					sr.material = new Material(mShader);
					sr.material.SetColor("_Color",c);
					sr.sortingLayerName = "Default";
				}
			}
			mIsColorChanged = true;
		}
	}
	
	public void RevertColor()
	{
		if(mIsColorChanged)
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
		}
	}



}
