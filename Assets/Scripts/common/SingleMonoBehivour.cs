using UnityEngine;
using System.Collections;

public class SingleMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour{

	private static T t;

	public static T GetInstance(){
		if (t == null) {
			t = GameObject.FindObjectOfType(typeof(T)) as T;
			if (t == null) {
				GameObject go = new GameObject (typeof(T).Name);
				t = go.AddComponent<T> ();
			}
		}
		return t;
	}

	protected virtual void Awake(){
		if(t==null){
			t = gameObject.GetComponent<T> ();
		}
	}

	protected bool isInited;


}
