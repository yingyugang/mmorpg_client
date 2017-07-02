using UnityEngine;
using System.Collections;

public class SubPanelPosition : MonoBehaviour {
	public ScreenDirection screenDirection;
	//horizontal表示水平滑动；vertical表示垂直滑动。
	public enum ScreenDirection 
	{
		horizontal,
		vertical
	}
	public float size;
	private Transform parent;
	private Transform child;
	private float ScaleSize;
	private float rateX;
	private float rateY;
	UIPanel PanelScript;
	void Start()
	{
		//Invoke("SetPanel",0.5f);
		SetPanel ();
	}
	void SetPanel()
	{
		parent = transform.parent;
		child = transform.GetChild(0);
		PanelScript = transform.GetComponent<UIPanel>();
		
		transform.parent = null;
		child.parent = null;
		
		
		//if(screenDirection == ScreenDirection.vertical)
		//{
			rateX =  Screen.width/size;
			rateY = 1;
			ScaleSize = transform.localScale.y;
		//}
//		else if(screenDirection == ScreenDirection.horizontal)
//		{
//			rateX = 1;
//			rateY = Screen.height/size;
//			ScaleSize = transform.localScale.x;
//		}
		
		transform.localScale = new Vector4(ScaleSize,ScaleSize,ScaleSize,ScaleSize);	
		transform.parent = parent;
		child.parent = transform;
		PanelScript.clipRange = new Vector4(PanelScript.clipRange.x,PanelScript.clipRange.y,PanelScript.clipRange.z * rateX,PanelScript.clipRange.w * rateY); 
	}
	
}