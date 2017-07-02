using UnityEngine;
using System.Collections;

public class AutoFullFillScreen : MonoBehaviour {

	void Start () {
		ResizeSpriteToScreen();
	}

	void LateUpdate()
	{
		ResizeSpriteToScreen();
	}

	void ResizeSpriteToScreen() {
		SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();
		if (sr == null) return;
		transform.localScale = Vector3.one;
		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;
		transform.localScale = new Vector3(ScreenScale.CurrentWorldsizeX / width,ScreenScale.CurrentWorldsizeY / height,0);
	}



}
