using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(SpriteRenderer))]
public class Buff : MonoBehaviour {

	public List<Sprite> sprites = new List<Sprite>();
	public Dictionary<Sprite,int> TurnMap = new Dictionary<Sprite, int>();
	public static Dictionary<string,Sprite> BuffSpriteMaps ;
	SpriteRenderer mRenderer;

	void Start () {
		mRenderer = GetComponent<SpriteRenderer>();
		mRenderer.sortingLayerName = "SuperLayer";
		if(BuffSpriteMaps==null)
		{
			LoadBuffSprites();
		}
		StartCoroutine(FrameAnim());
	}

	public static void LoadBuffSprites()
	{
		BuffSpriteMaps = new Dictionary<string, Sprite>();
		Object[] BuffSprites = Resources.LoadAll("Buff",typeof(Sprite));
		Sprite s;
		foreach(Object obj in BuffSprites)
		{
			s = obj as Sprite;
			BuffSpriteMaps.Add(s.name,s);
		}
	}

//	void OnGUI()
//	{
//		if(GUI.Button(new Rect(10,10,100,30),"Test"))
//		{
//			LoadBuffSprites();
//			AddSprite("battle_buff_icon_0");
//			AddSprite("battle_buff_icon_1");
//			AddSprite("battle_buff_icon_2");
//			AddSprite("battle_buff_icon_3");
//			AddSprite("battle_buff_icon_4");
//		}
//	}

	public void AddSprite(string spriteName)
	{
		if(BuffSpriteMaps.ContainsKey(spriteName))
		{
			Sprite s = BuffSpriteMaps[spriteName];
			mRenderer.sprite = s;
			sprites.Add(s);
			if(TurnMap.ContainsKey(s))
			{
				TurnMap[s] = 3;
			}
			else
			{
				TurnMap.Add(s,3);
			}
		}
	}

	public void OnTurnFinish()
	{
		sprites.Clear();
		mRenderer.sprite = null;
		/*foreach(Sprite s in TurnMap.Keys)
		{
			TurnMap[s] -= 1;
			if(TurnMap[s] <= 0)
			{
				TurnMap.Remove(s);
				sprites.Remove(s);
			}
		}*/
	}

	IEnumerator FrameAnim()
	{
		int index = 0;
		while(true)
		{
			if(sprites.Count > 0)
			{
				mRenderer.sprite = sprites[index];
				index ++;
				if(index>=sprites.Count)index = 0;
			}
			yield return new WaitForSeconds(2);
		}
	}
}
