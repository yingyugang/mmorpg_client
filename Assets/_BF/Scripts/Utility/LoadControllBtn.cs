using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LoadControllBtn : MonoBehaviour {

	public BattleController Controller;
	public GameObject ControllBtnPrefabs;
	public Vector3 BtnPosition;
	public float Interval;

	public bool Load;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
		if(Load && !Application.isPlaying)
		{
			InitBtns();
			Load = false;
		}
#endif
	}

	void InitBtns()
	{
		foreach(HeroBtn btn in Controller.PlayerBattleButtons)
		{
			if(btn!=null)
			{
				DestroyImmediate(btn.gameObject);
			}
		}
		Controller.PlayerBattleButtons.Clear();
		for(int i = 0 ; i < 6 ; i ++)
		{
			GameObject go = Instantiate(ControllBtnPrefabs) as GameObject;
			go.transform.parent = transform;
			go.transform.localPosition = BtnPosition + new Vector3(Interval * i,0,0);
			go.transform.localScale = ControllBtnPrefabs.transform.localScale;
			go.name = ControllBtnPrefabs.name + i;
			HeroBtn btn = go.GetComponent<HeroBtn>();
			Controller.PlayerBattleButtons.Add(btn);
		}
	}
}
