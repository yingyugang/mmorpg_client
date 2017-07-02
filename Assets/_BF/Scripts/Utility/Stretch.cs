using UnityEngine;
using System.Collections;

public class Stretch : MonoBehaviour {

	Vector3 defaultPos;
	Vector3 defaultScale;

	void Start () {
		defaultPos = transform.position;
		defaultScale = transform.localScale;
		transform.position = new Vector3(defaultPos.x * ScreenScale.RadiusX,defaultPos.y * ScreenScale.RadiusY,0);
		transform.localScale = new Vector3(defaultScale.x * ScreenScale.RadiusX,defaultScale.y * ScreenScale.RadiusY,0);
	}
}
