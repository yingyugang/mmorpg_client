using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReloadFX : MonoBehaviour {

	public GameObject[] SkillObj0;
	public GameObject[] SkillObj1;

	public NcCurveAnimation[] curves;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetNCAnim0()
	{
		//if(curves==null || curves.Length == 0)
		//{
		foreach(GameObject go in SkillObj0)
		{
			curves = go.GetComponents<NcCurveAnimation>();
			foreach(NcCurveAnimation nc in curves)
			{
				if(nc.IsEndAnimation())nc.ResetAnimation();
			}
			curves = go.GetComponentsInChildren<NcCurveAnimation>(true);
			foreach(NcCurveAnimation nc in curves)
			{
				
				if(nc.IsEndAnimation())nc.ResetAnimation();
			}
		}
		//}
	
	}

	public void ResetNcAnim1()
	{
		foreach(GameObject go in SkillObj1)
		{
			curves = go.GetComponents<NcCurveAnimation>();
			foreach(NcCurveAnimation nc in curves)
			{
				if(nc.IsEndAnimation())nc.ResetAnimation();
			}
			curves = go.GetComponentsInChildren<NcCurveAnimation>(true);
			foreach(NcCurveAnimation nc in curves)
			{
				
				if(nc.IsEndAnimation())nc.ResetAnimation();
			}
		}
	}

}
