using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

	public Sprite[] FrameSprites;
	SpriteRenderer m_spriteRender;

	void Awake()
	{
		m_spriteRender = GetComponent<SpriteRenderer>();
	}

	void Start () {
		StartCoroutine(_Flash());
	}

	int index = 0;
	IEnumerator _Flash()
	{
		while(true)
		{
			m_spriteRender.sprite = FrameSprites[index];
			index ++;
			if(index==FrameSprites.Length)
			{
				index = 0;
//				yield return new WaitForSeconds(Random.Range(1.5f,3.0f));
			}
			else
			{
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

}
