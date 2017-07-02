using UnityEngine;
using System.Collections;

public class ScreenScale : MonoBehaviour {

	public float default_worldsize_x = 13.34278f;
	public float default_worldsize_y = 10;

	public float current_worldsize_x;
	public float current_worldsize_y;

	float radius_x;
	float radius_y;

	 bool IsCalculateEveryFrame = true;

	static ScreenScale instance;
	public static ScreenScale SingleTon()
	{
		return instance;
	}

	static public float CurrentWorldsizeX
	{
		get
		{
			if (instance == null) Spawn();
			return instance.current_worldsize_x;
		}
	}

	static public float CurrentWorldsizeY
	{
		get
		{
			if (instance == null) Spawn();
			return instance.current_worldsize_y;
		}
	}

	static public float RadiusX
	{
		get
		{
			if (instance == null) Spawn();
			return instance.radius_x;
		}
	}

	static public float RadiusY
	{
		get
		{
			if (instance == null) Spawn();
			return instance.radius_y;
		}
	}

	static void Spawn ()
	{
		GameObject go = new GameObject("_ScreenScale");
		DontDestroyOnLoad(go);
		instance = go.AddComponent<ScreenScale>();
		instance.CalculateScaleRadius();
	}

	void Start()
	{
		CalculateScaleRadius();
	}

	void Update () {
		if(IsCalculateEveryFrame)CalculateScaleRadius();
	}

	public void CalculateScaleRadius()
	{
		float x0 = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
		float x1 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,0)).x;
		
		float y0 = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
		float y1 = Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height*0.5f,0)).y ;
		
		float x = Mathf.Abs(x1-x0);
		float y = Mathf.Abs(y1-y0);

		current_worldsize_x = x;
		current_worldsize_y = y;

		radius_x = current_worldsize_x / default_worldsize_x;
		radius_y = current_worldsize_y / default_worldsize_y;
	}





}
