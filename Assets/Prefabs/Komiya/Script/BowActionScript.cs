using UnityEngine;
using System.Collections;


public class BowActionScript :MonoBehaviour
{
	public void setFloat (float f){  // f 0,-1.0 //Mathf.Abs(CrossPlatformInputManager.GetAxisRaw ("Vertical")
		float v = 100.0f-93.0f* f;
		Renderer r = GetComponent<Renderer> ();
		r.material.SetFloat ("_curlR", v);
	}
}


