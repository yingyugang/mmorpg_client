using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestHeroResController : MonoBehaviour {

	public HeroRes heroRes;
	public SpriteRenderer[] SpriteRs;
	public SkinnedMeshRenderer[] SkinRs;

	void Start()
	{
		SpriteRs = heroRes.GetComponentsInChildren<SpriteRenderer>(true);
		SkinRs =  heroRes.GetComponentsInChildren<SkinnedMeshRenderer>(true);

	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,100,30),"Test"))
		{
			SampleRenderer();
		}
		int index = 0;
		foreach(string animName in heroRes.bodyAnimMapping.Keys)
		{
			if(GUI.Button(new Rect(10,Screen.height - 40 - 30 * index,100,30),heroRes.bodyAnimMapping[animName].clipName))
			{
//				heroRes.Play(heroRes.bodyAnimMapping[animName].clipName);
				StartCoroutine(_Play(heroRes.bodyAnimMapping[animName].clipName));
			}
			index ++;
		}
	}

	IEnumerator _Play(string clipName)
	{
//		while(heroRes.IsPlaying(clipName))
//		{
			SampleRenderer();
			yield return null;
//		}
	}

	void SampleRenderer()
	{
		for(int i = 0;i < SkinRs.Length;i++)
		{
//			Material mat = SkinRs[i].material;
			GameObject frameGO = new GameObject();
			frameGO.hideFlags = HideFlags.HideAndDontSave;
			Mesh frameMesh = new Mesh();
			SkinRs[i].BakeMesh(frameMesh);
			frameMesh.name = SkinRs[i].gameObject.name;
			SkinRs[i].material = new Material(SkinRs[i].material);
			frameGO.hideFlags = HideFlags.HideAndDontSave;
			frameGO.transform.position = SkinRs[i].transform.position;
			frameGO.transform.eulerAngles = SkinRs[i].transform.eulerAngles;
			MeshFilter meshFilter = frameGO.AddComponent<MeshFilter>();
			meshFilter.mesh = frameMesh;
			MeshRenderer sr = frameGO.AddComponent<MeshRenderer>();
			sr.material = new Material(SkinRs[i].material);
			StartCoroutine(_HideSkinned(sr));
		}

		for(int i = 0;i < SpriteRs.Length ; i ++)
		{
			GameObject frameGO = new GameObject();
			frameGO.hideFlags = HideFlags.HideAndDontSave;
			frameGO.transform.position = SpriteRs[i].transform.position;
			frameGO.transform.eulerAngles = SpriteRs[i].transform.eulerAngles;
			SpriteRenderer rs = frameGO.AddComponent<SpriteRenderer>();
			rs.sprite = SpriteRs[i].sprite;
			StartCoroutine(_HideSprite(rs));
		}
	}

	IEnumerator _HideSkinned(MeshRenderer sr)
	{
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime;
			sr.material.SetFloat("_Alpha",1-t);
			yield return null;
		}
		Destroy(sr.gameObject);
	}

	IEnumerator _HideSprite(SpriteRenderer sr)
	{
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime;
			sr.color = new Color(0,0,0,1-t);;
			yield return null;
		}
		Destroy(sr.gameObject);
	}


}
