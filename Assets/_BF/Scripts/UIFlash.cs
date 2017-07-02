using UnityEngine;
using System.Collections;

public class UIFlash : MonoBehaviour {

	public UISprite uiSprite;
	public string[] Frames;

	void Awake()
	{
		uiSprite = GetComponent<UISprite>();
	}

	void Start () {
		StartCoroutine(_Flash());
	}

	int index = 0;
	IEnumerator _Flash()
	{
		while(true)
		{
			uiSprite.spriteName = Frames[index];
			index ++;
			if(index==Frames.Length)
			{
				index = 0;
			}
			yield return new WaitForSeconds(0.5f);
		}
	}


}
