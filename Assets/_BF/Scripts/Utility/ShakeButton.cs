using UnityEngine;
using System.Collections;

public class ShakeButton : MonoBehaviour {

	public void ShakeScale()
	{
		iTween.ShakeScale(gameObject,new Vector3(0.5f,0.5f,0),0.5f);
	}

}
