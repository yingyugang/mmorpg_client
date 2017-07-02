using UnityEngine;
using System.Collections;

public class RecordController : MonoBehaviour {

	private GameObject collecthouse;
	private GameObject normal,battle,arena;
	private int index;

	void Start () {
		index = 2;
		UI.PanelTools.setBtnFunc(transform, "Bg/left", onLeftClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/right", onRightClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/title/back", onBackClick);

		collecthouse = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/Bg");
		normal = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelRecord/Bg/NormalRecord");
		battle = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelRecord/Bg/BattleRecord");
		arena = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelRecord/Bg/ArenaRecord");
	}

	void OnEnable()
	{
		index = 2;
	}

	void onLeftClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		index --;
		if(index == 0)
			index = 3;
	}

	void onRightClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		index ++;
		if(index == 4)
			index = 1;
	}

	void onBackClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		this.gameObject.SetActive(false);
		collecthouse.SetActive(true);
	}

	void Update()
	{
		if(index ==2)
		{
			normal.SetActive(true);
			battle.SetActive(false);
			arena.SetActive(false);
		}
		else if(index == 1)
		{
			normal.SetActive(false);
			battle.SetActive(true);
			arena.SetActive(false);
		}
		else
		{
			normal.SetActive(false);
			battle.SetActive(false);
			arena.SetActive(true);
		}
	}
}
