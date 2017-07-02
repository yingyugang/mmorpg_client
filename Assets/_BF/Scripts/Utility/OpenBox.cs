using UnityEngine;
using System.Collections;

public class OpenBox : MonoBehaviour {

	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void OnEnable()
	{
		if(anim!=null)
			anim.enabled = true;
	}

}
