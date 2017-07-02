using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class MeshToSprite : MonoBehaviour {

	public Transform Body;
	public string path = "Assets/";
	public bool Match;
	
	// Update is called once per frame
	void Update () {
		if(Match)
		{
			Replace(transform);
			Match = false;
		}
	}

	void Replace(Transform trans)
	{
		#if UNITY_EDITOR
		List<Transform> transList = new List<Transform>();
		foreach(Transform t0 in trans)
		{
			transList.Add(t0);
		}
		Transform tmp;
		for(int i = 0 ; i < transList.Count;i++)
		{
			tmp = transList[i];
			for(int j = 0 ; j < transList.Count;j++)
			{
				if(tmp.position.z > transList[j].position.z)
				{
					int j0 = transList.IndexOf(tmp);
					transList[j0] = transList[j];
					transList[j] = tmp;
					tmp = transList[j0];
				}
			}
		}
		for(int i = 0 ; i < transList.Count ;i++)
		{
			Transform child = transList[i];
			if(child.GetComponent<MeshRenderer>()!=null)
			{
				MeshRenderer mr = child.GetComponent<MeshRenderer>();
				string path = AssetDatabase.GetAssetPath(mr.sharedMaterial.mainTexture);
				DestroyImmediate(child.GetComponent<Uni2DSprite>());
				DestroyImmediate(child.GetComponent<MeshRenderer>());
				DestroyImmediate(child.GetComponent<MeshFilter>());
				SpriteRenderer sr = child.gameObject.AddComponent<SpriteRenderer>();
				sr.sortingOrder = i;
				Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
				sr.sprite = sprite;
				child.localPosition = new Vector3(child.localPosition.x,child.localPosition.y,0);
			}
		}
		#endif
	}
}
