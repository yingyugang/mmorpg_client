using UnityEngine;
using System.Collections;

public class SpriteTween : MonoBehaviour {

	UISprite mSprite;
	public string[] SpriteNames;
//	public SpriteRenderer mRenderer;
//	public Sprite[] sprites;
	
	void Awake () {
		mSprite = GetComponent<UISprite>();
	}

	void OnEnable ()
	{
		StartCoroutine(_Flash());
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}

	int index = 0;
	IEnumerator _Flash()
	{
		while(true)
		{
			mSprite.spriteName = SpriteNames[index];
			index ++;
			if(index==SpriteNames.Length)
			{
				index = 0;
			}
			yield return new WaitForSeconds(0.2f);
		}
	}
}
