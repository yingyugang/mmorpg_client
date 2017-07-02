using UnityEngine;
using System.Collections;

public class VillageEffect : MonoBehaviour {

	private UITexture texture;
	float y;
	// Use this for initialization
	void Start () {
		texture = GetComponent<UITexture>();
		y = 0;
	}

	// Update is called once per frame
	void Update () {
		
		y -= Time.deltaTime * 0.5f;
		if (y < -1)
			y = 0;
		Rect rect = new Rect(0,y,1,1);
		texture.uvRect = rect;
	}
}
