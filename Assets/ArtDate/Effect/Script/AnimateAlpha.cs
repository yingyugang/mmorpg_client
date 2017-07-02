using UnityEngine;
using System.Collections;

public class AnimateAlpha : MonoBehaviour {
	public float speed=1.0f;
	public AnimationCurve curve= new AnimationCurve();
	public float valueScale=1.0f;
	private Material _mat;
	private Color _color;
	// Use this for initialization
	void Start () {
		_mat = gameObject.GetComponent<Renderer>().material;
		_color = _mat.GetColor ("_TintColor");
		curve.preWrapMode = WrapMode.PingPong;
		curve.postWrapMode = WrapMode.PingPong;
	}
	
	// Update is called once per frame
	void Update () {
		_mat.SetColor ("_TintColor",new Color(_color.r,_color.g,_color.b,valueScale*curve.Evaluate(Time.time*speed)));
	}


}
