using UnityEngine;
using System.Collections;

public class AnimationPlayer: MonoBehaviour {

	public bool Loop;
	public Animation anim;
	
	void OnGUI()
	{
		if(anim!=null)
		{
			int offsetY = 10;
			foreach(AnimationState state in anim)
			{
				Loop = GUI.Toggle(new Rect(130,offsetY,100,30),Loop,"Loop");
				if(GUI.Button(new Rect(10,offsetY,100,30),state.name))
				{
					if(Loop)
						anim.wrapMode = WrapMode.Loop;
					else
						anim.wrapMode = WrapMode.Once;
					anim.Play(state.name);
				}
				offsetY += 40;
			}
		}
	}

}
