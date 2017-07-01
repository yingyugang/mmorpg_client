using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BodyPart{Head,Body,LeftLeg,RightLeg};
public class UnitResources : MonoBehaviour {

	public Enemy[] bodyParts;
	public Transform headTrans;
	public Transform centerTrans;
	public Transform rightHandTrans;

	void Awake()
	{
		bodyParts = GetComponentsInChildren<Enemy>();
		if (rightHandTrans == null)
			rightHandTrans = transform;
		if (headTrans == null)
			headTrans = transform;
		if (centerTrans == null)
			centerTrans = transform;
	}

}
