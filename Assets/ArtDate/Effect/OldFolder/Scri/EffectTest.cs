using UnityEngine;
using System.Collections;

public class EffectTest : MonoBehaviour {

	
	public Animation anim;
	
	void OnGUI()
	{
		if(anim!=null)
		{
			int offsetY = 10;
			foreach(AnimationState state in anim)
			{
				if(GUI.Button(new Rect(10,offsetY,100,30),state.name))
				{
					anim.Play(state.name);
				}
				offsetY += 40;
			}
		}
	}

}
