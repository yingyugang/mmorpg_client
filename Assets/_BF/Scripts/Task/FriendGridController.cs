using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendGridController : MonoBehaviour {

	public struct friendInfo
	{
		public string name;
		public string friendcount;
		public string friendicon;
		public string stoneicon;
		public string desc;
		public string level;
	}
	
	public GameObject panelDrugList;
	public GameObject frienditemPrefab;
	public List<friendInfo> friendInfoList;
	public GameObject madeDrugPrefab;
	
	private GameObject madeDrug;
	
	private UIGrid grid;
	
	// Use this for initialization
	void Awake()
	{
	}
	
	void Start () {	
		friendInfoList = new List<friendInfo>();
		grid = transform.GetComponent<UIGrid>();
		getFriendList();
		iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));		
	
	}
	
	void getFriendList()
	{
		friendInfo strdungeon1 = new friendInfo();
		strdungeon1.name = "列亚力克";
		strdungeon1.friendcount = "友情点 +10";
		strdungeon1.friendicon = "10131BattleEffectFireWorks1Add_64x64";
		strdungeon1.stoneicon = "10133BattleEffectFireWorks3Add_64x64";
		strdungeon1.desc = "名剑-勇气之锋";
		
		friendInfoList.Add(strdungeon1);
		
		friendInfo strdungeon2 = new friendInfo();
		strdungeon2.name = "恋之埋火";
		strdungeon2.friendcount = "友情点 +10";
		strdungeon2.friendicon = "10131BattleEffectFireWorks1Add_64x64";
		strdungeon2.stoneicon = "10133BattleEffectFireWorks3Add_64x64";
		strdungeon2.desc = "机剑-防卫者";
		
		friendInfoList.Add(strdungeon2);
		
		friendInfo strdungeon3 = new friendInfo();
		strdungeon3.name = "Jack";
		strdungeon3.friendcount = "友情点 +10";
		strdungeon3.friendicon = "10132BattleEffectFireWorks2Add_64x64";
		strdungeon3.stoneicon = "10133BattleEffectFireWorks3Add_64x64";
		strdungeon3.desc = "食铠-阿巴顿之墙";
		
		friendInfoList.Add(strdungeon3);
		
		friendInfo strdungeon4 = new friendInfo();
		strdungeon4.name = "Tom";
		strdungeon4.friendcount = "友情点 +10";
		strdungeon4.friendicon = "10133BattleEffectFireWorks3Add_64x64";
		strdungeon4.stoneicon = "10133BattleEffectFireWorks3Add_64x64";
		strdungeon4.desc = "机剑-防卫者";
		
		friendInfoList.Add(strdungeon4);
		
		friendInfo strdungeon5 = new friendInfo();
		strdungeon5.name = "古雷休波特";
		strdungeon5.friendcount = "友情点 +10";
		strdungeon5.friendicon = "10133BattleEffectFireWorks3Add_64x64";
		strdungeon5.stoneicon = "10133BattleEffectFireWorks3Add_64x64";
		strdungeon5.desc = "食铠-阿巴顿之墙";
		
		friendInfoList.Add(strdungeon5);
		
		friendInfo strdungeon6 = new friendInfo();
		strdungeon6.name = "埃蒙沫儿";
		strdungeon6.friendcount = "友情点 +10";
		strdungeon6.friendicon = "10131BattleEffectFireWorks1Add_64x64";
		strdungeon6.stoneicon = "10133BattleEffectFireWorks3Add_64x64";
		strdungeon6.desc = "食铠-阿巴顿之墙";
		
		friendInfoList.Add(strdungeon6);
		
		addFriendItem();
	}
	
	void addFriendItem()
	{	
		for (int i = 0; i < friendInfoList.Count; i ++)
		{
			friendInfo Info = friendInfoList[i];
			GameObject friendItem = Instantiate(frienditemPrefab) as GameObject;
			
			
			friendItem.transform.Find("friendPnl/friendname").GetComponent<UILabel>().text = Info.name;			
			friendItem.transform.Find("friendPnl/friendcount").GetComponent<UILabel>().text = Info.friendcount;
			friendItem.transform.Find("friendPnl/stonedesc").GetComponent<UILabel>().text = Info.desc;
			friendItem.transform.Find("friendPnl/bgicon/friendicon").GetComponent<UISprite>().spriteName = Info.friendicon;
			friendItem.transform.Find("friendPnl/stoneicon/icon").GetComponent<UISprite>().spriteName = Info.stoneicon;
						
			friendItem.transform.parent = transform;
			grid.Reposition();
			friendItem.transform.localScale = Vector3.one;			
			UIEventListener.Get(friendItem).onClick = ItemClick;
		}
	}
	
	void ItemClick(GameObject click)
	{	
		if (madeDrugPrefab)
		{
			panelDrugList.SetActive(false);
			madeDrugPrefab.SetActive(true);
		}
	}

}
