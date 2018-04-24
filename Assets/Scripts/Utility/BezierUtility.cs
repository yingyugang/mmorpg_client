using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierUtility {

	public static Vector3 BezierVector3(Vector3 P0,Vector3 P2,Vector3 P1,float t){
		t = Mathf.Clamp (t,0,1f);
		Vector3 target = (1-t)*(1-t) *P0 + 2 * (1-t) * t *P1 + t*t*P2;
		return target;
	}

	public static Vector3 Bezier2(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;
		Vector3 p = uu * p0;
		p += 2 * u * t * p1;
		p +=  tt * p2;      
		return p;
	}

	public static Vector3 Bezier3(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;
		Vector3 p = uuu * p0;
		p += 3 * uu * t * p1;
		p += 3 * u * tt * p2;
		p += ttt * p3;
		return p;
	}

}
