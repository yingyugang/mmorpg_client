using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class RemoveParent : MonoBehaviour {

	public bool IsRemove = false;

	void Update()
	{
		if(IsRemove)
		{
			Remove(transform);
			IsRemove = false;
		}
	}

	void Remove(Transform trans)
	{
		int count = trans.childCount;
		for(int i = 0;i < count; i++)
		{
			Transform t = trans.GetChild(0);
			t.parent = null;
			Remove(t);
		}
	}



}
