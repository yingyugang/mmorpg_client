using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
public class BoneMatching : MonoBehaviour {

	public Transform Bone;
	public Transform Body;
	List<Transform> BonePoints = new List<Transform>();
	Dictionary<string,Transform> BodyParts = new Dictionary<string, Transform>();
	public bool Match;
	
	// Update is called once per frame
	void Update () {
		if(Match)
		{
			BonePoints = new List<Transform>();
			BodyParts = new Dictionary<string, Transform>();
			GetBoneChilds(Bone);
			GetBodyChilds(Body);
			RemoveMeshTrans();
			_Match();
			Match = false;
		}
	}

	void RemoveMeshTrans()
	{
		for(int i = 0 ; i < BonePoints.Count ;)
		{
			if(BonePoints[i].GetComponent<MeshFilter>()!=null)
			{
				BonePoints[i].parent = null;
				BonePoints.Remove(BonePoints[i]);

			}else{
				i ++;
			}
		}
	}

	void GetBoneChilds(Transform trans)
	{
		int childCount = trans.childCount;
		for(int i = 0 ; i < childCount ; i ++)
		{
			BonePoints.Add(trans.GetChild(i));
			GetBoneChilds(trans.GetChild(i));
		}
	}

	void GetBodyChilds(Transform trans)
	{
		int childCount = trans.childCount;
		for(int i = 0 ; i < childCount ; i ++)
		{
            Transform child = trans.GetChild(i);
            if (child.gameObject.GetComponentsInChildren<PolygonCollider2D>().Length > 0)
                continue;
            string name = child.name.Substring(3);
            BodyParts[name] = child;
            GetBodyChilds(child);
		}
	}

	void _Match()
	{
		foreach(Transform trans in BonePoints)
		{
			if(BodyParts.ContainsKey(trans.name))
			{
				BodyParts[trans.name].parent = trans;
			}
		}
	}


}
