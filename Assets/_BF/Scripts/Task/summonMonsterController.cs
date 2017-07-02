using UnityEngine;
using System.Collections;

public class summonMonsterController : MonoBehaviour {


	private GameObject btnDismissionSummon;
	private GameObject summonBar;
	private HeroBtn heroBtn;
	private Hero summonMonster;

	// Use this for initialization
	void Start () {
	
		/*UI.PanelStack.me.root = this.gameObject;
		//UI.PanelStack.me.root = GameObject.Find("Camera");
		btnDismissionSummon = UI.PanelStack.me.FindPanel("dismissionSummon");
		UIEventListener.Get(btnDismissionSummon).onClick = onDismissionSummon; 

		UI.PanelStack.me.root = GameObject.Find("Camera");
		summonBar = UI.PanelStack.me.FindPanel("BattleUI/SummonBar");
		
		heroBtn = this.gameObject.GetComponent<HeroBtn>();
		heroBtn.ActiveButton();
		*/

	}

	void onDismissionSummon(GameObject go)
	{
		this.gameObject.SetActive(false);
		summonBar.gameObject.SetActive(true);
	}


	void OnEnable()
	{
		for (int i = 0; i < BattleController.SingleTon().RightHeroes.Count; i ++)
		{
			if (BattleController.SingleTon().RightHeroes[i].isSummonMonster == true)
			{
				summonMonster = BattleController.SingleTon().RightHeroes[i];
				break;
			}
		}
		
		if (summonMonster != null)
			summonMonster.SetControllBtn(heroBtn);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
