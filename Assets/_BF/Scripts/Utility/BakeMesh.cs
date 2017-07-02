using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BakeMesh : MonoBehaviour {

	Animation m_animation;
	public string AnimClip = "Attack";
	public SpriteRenderer[] SpriteRs;

	void Start()
	{
		SpriteRs = GetComponentsInChildren<SpriteRenderer>();
		if(m_animation==null)
			m_animation = GetComponent<Animation>();
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,170,30),"Attack"))
		{
			StartCoroutine(_Move());
		}
	}
	
	IEnumerator _Move()
	{
		float timeDur = m_animation[AnimClip].length;
		Debug.Log("timeDur:" + timeDur);
		float framePer = timeDur / 5;
		m_animation.Play(AnimClip);
		while(m_animation.IsPlaying(AnimClip))
		{
			SampleSpriteRenderer(timeDur);
			yield return new WaitForSeconds(framePer);;
		}
	}

	void SampleSpriteRenderer(float timeDur)
	{
//		string frameName = string.Format("BakedFrame{0}", frameIndex1);
//		Mesh frameMesh = new Mesh();
//		frameMesh.name = frameName;
//		m_skinnedMeshRenderer.BakeMesh(frameMesh);
		for(int i = 0;i < SpriteRs.Length ; i ++)
		{
			GameObject frameGO = new GameObject();
			frameGO.hideFlags = HideFlags.HideAndDontSave;
			frameGO.transform.position = SpriteRs[i].transform.position;
			frameGO.transform.eulerAngles = SpriteRs[i].transform.eulerAngles;
			SpriteRenderer rs = frameGO.AddComponent<SpriteRenderer>();
			rs.sprite = SpriteRs[i].sprite;
			StartCoroutine(_Hide(rs));
		}
//		MeshFilter meshFilter = frameGO.AddComponent<MeshFilter>();
//		meshFilter.mesh = frameMesh;
//		MeshRenderer meshRenderer = frameGO.AddComponent<MeshRenderer>();
//		meshRenderer.sharedMaterials = m_skinnedMeshRenderer.sharedMaterials;
//		frameGO.transform.eulerAngles = new Vector3(-90,0,0);
	}

	IEnumerator _Hide(SpriteRenderer sr)
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
