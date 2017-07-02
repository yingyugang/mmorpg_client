using UnityEngine;
using System.Collections;

public class WaterEffect : MonoBehaviour {

	private UITexture texture;
	float x;
	// Use this for initialization
	void Start () {
		texture = GetComponent<UITexture>();
		x = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		x += Time.deltaTime * 0.1f;
		if (x > 1)
			x = 0;
		Rect rect = new Rect(x,0,1,1);
		texture.uvRect = rect;
	}
}
