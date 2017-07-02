using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BoneToBone : MonoBehaviour {

	public Transform Bone0;
	public Transform Bone1;

	public Dictionary<string,Transform> BonePoints = new Dictionary<string,Transform>();

	public List<Transform> Meshs = new List<Transform>();
	public Animation anim;

	public bool IsMatch;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
	
	}
	
	// Update is called once per frame
	void Update () {

		if(IsMatch)
		{
			BonePoints = new Dictionary<string,Transform>(); 
			Meshs = new List<Transform>();
			GetBoneChilds(Bone0);
			GetMeshChilds(Bone1);
			Match();
			IsMatch = false;
			anim.Sample();
		}
	}

	void GetBoneChilds(Transform trans)
	{
		int childCount = trans.childCount;

		for(int i = 0 ; i < childCount ; i ++)
		{
			Transform t = trans.GetChild(i);
			if(!BonePoints.ContainsKey(t.name))
			{
				BonePoints.Add(t.name,t);
			}
			else
			{
				Debug.Log(t.name);
			}

			GetBoneChilds(t);
		}
	}

	void GetMeshChilds(Transform trans)
	{
		int childCount = trans.childCount;

		for(int i = 0 ; i < childCount ; i ++)
		{
			Transform t = trans.GetChild(i);
			Debug.Log(t.name);
			if(t.GetComponent<SpriteRenderer>()!=null)
			{
				Meshs.Add(t);
			}
			GetMeshChilds(t);
		}
	}

	void Match()
	{
		foreach(Transform trans in Meshs)
		{
			string name = trans.name;
			name = name.Substring(2);
			if(BonePoints.ContainsKey(name))
			{
				trans.parent = BonePoints[name];
			}
		}
	}

}
