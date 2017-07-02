using UnityEngine;
using System.Collections;

public class TestAnim01Controller : MonoBehaviour {

	public HeroAnimation heroAnimation;
	HeroRes heroRes;
	void Awake()
	{
		 heroRes = FindObjectOfType<HeroRes>();
		if(heroRes!=null)
		{
			if(heroRes.GetComponent<HeroAnimation>()!=null)
			{
				heroAnimation = heroRes.GetComponent<HeroAnimation>();
			}
			else
			{
				heroAnimation = heroRes.gameObject.AddComponent<HeroAnimation>();
			}
			
		}
	}
	
	void OnGUI()
	{
		int index = 0;
		foreach(AnimMapping am in heroRes.bodyAnims)
		{
			if(GUI.Button(new Rect(10,10 + 40 * index,100,30),am.clipName))
			{
				heroAnimation.PlayByFrags(am.clipName);
			}
			index ++;
		}

	}


}
