using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EffectFollow : MonoBehaviour {

	public Transform parentGo;
	public EffectAttr ea;

	
	// Update is called once per frame
	void Update () {
		if(ea!=null && parentGo!=null)
		{
			if(ea.follow==null)
			{
				ea.position = transform.position - parentGo.position;
			}
			ea.rotation = transform.localEulerAngles;
			ea.scale = transform.localScale;
		}

	}
}
