using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LoadPotionBtn : MonoBehaviour {

	public BattleController Controller;
	public GameObject PotionBtnPrefabs;
	public Vector3 BtnPosition;
	public float Interval;
	public bool Load;

	void Start () {
	
	}

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
		foreach(Potion potion in Controller.Potions)
		{
			DestroyImmediate(potion.gameObject);
		}
		Controller.Potions = new Potion[5];
		for(int i = 0 ; i < 5 ; i ++)
		{
			GameObject go = Instantiate(PotionBtnPrefabs) as GameObject;
			go.transform.parent = transform;
			go.transform.localPosition = BtnPosition + new Vector3(Interval * i,0,0);
			go.transform.localScale = PotionBtnPrefabs.transform.localScale;
			go.name = PotionBtnPrefabs.name + i;
			Potion potion = go.GetComponent<Potion>();
			Controller.Potions[i] = potion;
		}
	}


}
