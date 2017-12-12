using UnityEngine;
using System.Collections;

public static class MonoExtendUtility{

	public static T GetOrAddComponent<T>(this GameObject go) where T : MonoBehaviour
	{
		T t = go.GetComponent<T>();
		if(t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}


}
