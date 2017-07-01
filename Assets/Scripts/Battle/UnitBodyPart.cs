using UnityEngine;
using System.Collections;

public class UnitBodyPart : MonoBehaviour {

	public BodyPart bodyPart;
	Collider mBodyCollider;

	void Awake()
	{
		mBodyCollider = GetComponent<Collider>();
	}

}
