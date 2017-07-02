using UnityEngine;
using System.Collections;

public class BezierController : MonoBehaviour {

	public Bezier bezier;
	public Transform warehouse;
	private float offsetx;
	private float offsety;
	public float orientation;
	private int i;
	private Vector3 smal;

	// Use this for initialization
	void Start () {

		i = 1;
		float x = transform.position.x;
		float y = transform.position.y;
		//float x2 = transform.position.x;
		smal = new Vector3 (0.2f,0.2f,0.2f);

		if (x < 0)
			x = x + 2/3f;
		else
			x = x - 1/3f;

		if (y < 0)
			y = y + 4/3f;
		else
			y = y + 1/3f;

		//x = x + offsetx;
		//y = y + offsety;

		bezier = new Bezier(transform.position,new Vector3(x,y,0),
		                    new Vector3(0,0,0),new Vector3(transform.position.x + orientation,transform.position.y,0));

	}
	
	// Update is called once per frame
	void Update () {
		if (i < 50)
		{
			transform.position = bezier.GetPointAtTime(i * 0.02f);
			i += 1;
		}

		i += 1;

		if (i >= 50)
		{
			//Debug.Log(warehouse.position);
			transform.position = Vector3.Lerp(transform.position,warehouse.position,Time.deltaTime * 2);
			transform.localScale = Vector3.Lerp(transform.localScale,smal,Time.deltaTime * 2);
		}

		if (i > 200)
		{
			this.transform.gameObject.SetActive(false);
		}
	}
}
