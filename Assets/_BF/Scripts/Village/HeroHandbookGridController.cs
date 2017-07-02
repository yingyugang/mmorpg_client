using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
public class HeroHandbookGridController : MonoBehaviour {
	
	//public GameObject stuffitem;
	//public GameObject stuffSale;
	private GameObject heroDetail;
	private UIGrid grid;
	private List<GameObject> stuffList;
	
	void Start () {
		if (grid == null)
		{
			grid = transform.GetComponent<UIGrid>();
			grid.transform.position= new Vector3(-290,30,0);
		}
		heroDetail = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelHandbook/PanelHeroHandbook/PanelHeroDetail");
		iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));
	}
	
	void OnEnable()
	{
		if (grid == null)
			grid = transform.GetComponent<UIGrid>();
		if(heroDetail !=null)
			heroDetail.SetActive (false);
		addStoneItem();
	}

	void addStoneItem()
	{	
		clearStoneItem ();
		stuffList = new List<GameObject>();
		
		
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
		if (table == null )
			return;
		foreach(ConfigRow row in table.rows)
		{
			int lid = row.getIntValue(DICT_HERO.LIBRARY);
			int typeid = row.getIntValue(DICT_HERO.HERO_TYPEID);
			int seris = row.getIntValue(DICT_HERO.SERIES);
			int star = row.getIntValue(DICT_HERO.STAR);
			GameObject stuffItem = (GameObject) Instantiate(Resources.Load("Village/stuffitem"));
			stuffItem.SetActive(true);
			HeroItem itemData = stuffItem.AddComponent<HeroItem>();
			itemData.typeid = typeid;
			itemData.library = lid;
			itemData.Sname= BaseLib.LanguageMgr.getString(row.getIntValue(DICT_HERO.NAME_ID));
			itemData.desc = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_HERO.DESC_ID));

			stuffItem.transform.Find("name").GetComponent<UILabel>().text = "?";
			stuffItem.transform.Find("amount").GetComponent<UILabel>().text= "";

			stuffItem.transform.Find("icon").GetComponent<UISprite>().spriteName = typeid.ToString();
			//	stuffItem.transform.Find("Background").GetComponent<UISprite>().alpha=0.5f;
			stuffItem.transform.Find("bg").GetComponent<UIWidget>().depth = 2;
			stuffItem.transform.parent = transform;
			stuffItem.transform.localScale = Vector3.one;
			UIEventListener.Get(stuffItem).onClick = ItemClick;

			UserInfo info = DataManager.getModule<DataUser> (DATA_MODULE.Data_User).mainUser;
			if(info.heroIsOpen((uint)lid))
			{
				stuffItem.transform.Find("name").GetComponent<UILabel>().text = "NO."+ lid;
				stuffItem.transform.Find("bg").GetComponent<UIWidget>().depth = 1;
			}
			stuffList.Add(stuffItem);
		}
		grid.repositionNow = true;
	}
	void clearStoneItem()
	{
		if(stuffList!=null)
			stuffList.Clear ();
		for (int i = transform.childCount -1; i >=0; i--) 
		{
			Destroy(transform.GetChild(i).gameObject);
		}			
	}
	void ItemClick(GameObject click)
	{
		AudioManager.me.PlayBtnActionClip();
		HeroItem data = click.GetComponent<HeroItem> ();
		heroHandbookDetail madeObj = heroDetail.GetComponent<heroHandbookDetail> ();
		if (madeObj != null)
			madeObj.curItem = data;
		heroDetail.SetActive(true);
		Debug.Log ("onclick");
	}
}
