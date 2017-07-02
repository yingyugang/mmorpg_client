using UnityEngine;
using System.Collections;

public class Curve : MonoBehaviour {

	public static Vector2 Bezier2(Vector2 start,Vector2  control,Vector2  end,float  t)
	{
		return (((1-t)*(1-t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
	}
	
	public static Vector3 Bezier2(Vector3 start,Vector3 control,Vector3  end,float t)
	{
		return (((1-t)*(1-t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
	}

	public static Vector2 Bezier3(Vector2 s,Vector2 st,Vector2 et,Vector2  e,float t)
	{
		return (((-s + 3*(st-et) + e)* t + (3*(s+et) - 6*st))* t + 3*(st-s))* t + s;
	}
	
	public static Vector3 Bezier3(Vector3 s,Vector3 st,Vector3 et,Vector3 e,float t)
	{
		return (((-s + 3*(st-et) + e)* t + (3*(s+et) - 6*st))* t + 3*(st-s))* t + s;
	}
}
