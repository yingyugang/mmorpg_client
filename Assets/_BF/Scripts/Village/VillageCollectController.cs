using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillageCollectController : MonoBehaviour {

	public List<GameObject> items;
	public Transform parent;

	public GameObject tree;
	public GameObject rock;
	public GameObject river;
	public GameObject field;

	//public Camera camera;

	//private Random ra;
	private GameObject collectItem;

	// Use this for initialization
	void Start () {
	
		UIEventListener.Get(tree).onClick = onCollectClick;
		UIEventListener.Get(rock).onClick = onCollectClick;
		UIEventListener.Get(river).onClick = onCollectClick;
		UIEventListener.Get(field).onClick = onCollectClick;

		//ra = new Random();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onCollectClick(GameObject go)
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		int index = Random.Range(0,3);
		//int index = 0;
		GameObject prefab = items[index];
		Debug.Log(pos);
		collectItem = Instantiate(prefab) as GameObject;
		//UI2DSprite sprite = collectItem.GetComponent<UI2DSprite>();
		//collectItem.AddComponent<BezierController>();
		collectItem.transform.localPosition = pos;
		collectItem.SetActive(true);
		collectItem.transform.parent = parent;
		collectItem.transform.localScale = Vector3.one;

		//Debug.Log(pos);
		//Debug.Log(Input.mousePosition);
	}

}
