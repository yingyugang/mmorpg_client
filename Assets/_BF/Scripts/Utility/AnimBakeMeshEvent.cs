using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class AnimBakeMeshEvent : MonoBehaviour {
 
	public SkinnedMeshRenderer[] skinnedRs;
	public SpriteRenderer[] spriteRs;


	void Start () {
//		if(skinnedRs==null)
			skinnedRs=GetComponentsInChildren<SkinnedMeshRenderer>();
//		if(spriteRs==null)
			spriteRs=GetComponentsInChildren<SpriteRenderer>();
	}

	public void BakeRenders()
	{
		SampleRenderer(skinnedRs,spriteRs);
	}

	public void SampleRenderer(SkinnedMeshRenderer[] skinRs,SpriteRenderer[] spriteRs)
	{
		for(int i = 0;i < skinRs.Length;i++)
		{
//			Material mat = skinRs[i].material;
			GameObject frameGO = new GameObject();
//			frameGO.hideFlags = HideFlags.HideAndDontSave;
			Mesh frameMesh = new Mesh();
			skinRs[i].BakeMesh(frameMesh);
			frameMesh.name = skinRs[i].gameObject.name;
			skinRs[i].material = new Material(skinRs[i].material);
//			frameGO.hideFlags = HideFlags.HideAndDontSave;
			frameGO.transform.position = skinRs[i].transform.position;
			frameGO.transform.eulerAngles = skinRs[i].transform.eulerAngles;
			MeshFilter meshFilter = frameGO.AddComponent<MeshFilter>();
			meshFilter.mesh = frameMesh;
			MeshRenderer sr = frameGO.AddComponent<MeshRenderer>();
			sr.sortingLayerName = skinRs[i].sortingLayerName;
			sr.sortingOrder = skinRs[i].sortingOrder;
			sr.material = new Material(skinRs[i].material);
			StartCoroutine(_HideSkinned(sr));
		}
		for(int i = 0;i < spriteRs.Length ; i ++)
		{
			GameObject frameGO = new GameObject();
//			frameGO.hideFlags = HideFlags.HideAndDontSave;
			frameGO.transform.position = spriteRs[i].transform.position;
			frameGO.transform.eulerAngles = spriteRs[i].transform.eulerAngles;
			SpriteRenderer rs = frameGO.AddComponent<SpriteRenderer>();
			rs.sprite = spriteRs[i].sprite;
			rs.sortingLayerName = spriteRs[i].sortingLayerName;
			rs.sortingOrder = spriteRs[i].sortingOrder;
			StartCoroutine(_HideSprite(rs));
		}

	}
	
	IEnumerator _HideSkinned(MeshRenderer sr)
	{
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime * 2;
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
			t += Time.deltaTime * 2;
			sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,1-t);;
			yield return null;
		}
		Destroy(sr.gameObject);
	}

	public class AnimClipEventsMaps
	{
		public string animClipName;
		public List<string> funcNames;
		public List<float> funcTimes;

	}
}
