using UnityEngine;
using System.Collections;

public class SliderCurve : MonoBehaviour {

	public UISlider slider;
	public Transform tmpt;

	public Vector3 startPos;
	public Vector3 endPos;
	public Vector3 controllPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float y = Curve.Bezier2(startPos,controllPos,endPos,slider.value).y;
		tmpt.localPosition = new Vector3(tmpt.localPosition.x,y,tmpt.localPosition.z);
	}
}
