using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public static class PotionUtility {

	public static PotionAttr[] BattlePotAttrs = new PotionAttr[5];

	public static PotionAttr InitHealth(ItemInfo itemInfo)
	{
		PotionAttr attr = new PotionAttr();
		attr.Count = (int)itemInfo.amount;
		attr.Power = 500;//TODO
		attr.PosName = itemInfo.name;
		//attr.Icon = itemInfo.icon;
		attr.Icon = itemInfo.type.ToString();
		attr.Type = _PotionType.Health;
		return attr;
	}

	public static PotionAttr InitStrongHealth(ItemInfo itemInfo)
	{
		PotionAttr attr = new PotionAttr();
		attr.Count = (int)itemInfo.amount;
		attr.Power = 2000;//TODO
		attr.PosName = itemInfo.name;
		attr.Icon = itemInfo.icon;
		attr.Type = _PotionType.Health;
		return attr;
	}

	public static PotionAttr InitGlobalHealth(ItemInfo itemInfo)
	{
		PotionAttr attr = new PotionAttr();
		attr.Count = (int)itemInfo.amount;
		attr.ImpactHeroType = 0;
		attr.ImpactNum = 6;
		attr.Power = 500;
		attr.PosName = itemInfo.name;
		attr.Icon = itemInfo.icon;
		attr.Type = _PotionType.Health;
		return attr;
	}

	public static PotionAttr InitElementAttackEnhanceStone(ItemInfo itemInfo,_ElementType eleType)
	{
		PotionAttr attr = new PotionAttr();
		attr.Count = (int)itemInfo.amount;
		attr.ImpactHeroType = 0;
		attr.ImpactNum = 6;
		attr.Power = 0;
		attr.PosName = itemInfo.name;
		attr.Icon = itemInfo.icon;
		attr.Type = _PotionType.Enhance;
		attr.SubType = _SubPotionType.Attack;
		attr.ElementType = eleType;
		return attr;
	}

	public static PotionAttr InitRebirth(ItemInfo itemInfo)
	{
		PotionAttr attr = new PotionAttr();
		attr.Count = (int)itemInfo.amount;
		attr.ImpactHeroType = 0;
		attr.ImpactNum = 1;
		attr.Power = 100;
		attr.PosName = itemInfo.name;
		attr.Icon = itemInfo.icon;
		attr.Type = _PotionType.Rebirth;
		return attr;
	}


//	public static PotionAttr


	public static void InitBattlePotions(Potion[] battlePotions)
	{
		ItemInfo[] itemInfo = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getBattleItemList();
		for(int i = 0;i < itemInfo.Length;i ++)
		{
			PotionAttr attr = new PotionAttr();
			switch(itemInfo[i].type)
			{
				case 42001:attr = InitHealth(itemInfo[i]);break;
				case 42002:attr = InitStrongHealth(itemInfo[i]);break;
				case 42003:attr = InitGlobalHealth(itemInfo[i]);break;
				case 42004:attr = InitHealth(itemInfo[i]);break;
				case 42005:attr = InitHealth(itemInfo[i]);break;
			}
			BattlePotAttrs[i] = attr;
			Debug.Log("itemInfo[i].type:" + itemInfo[i].type);
			Debug.Log("itemInfo[i].id:" + itemInfo[i].id);
		}


		for(int i = 0;i < BattlePotAttrs.Length;i ++)
		{
			battlePotions[i].SetPotionAttr(BattlePotAttrs[i]);
		}

//		PotionAttr attr = new PotionAttr();
//		attr.Count = 10;
//		attr.ImpactHeroType = 0;
//		attr.ImpactNum = 1;
//		attr.Power = 500;
//		attr.PosName = "回复药";
//		attr.Icon = "Med6";
//		attr.Type = _PotionType.Health;
//		BattlePotAttrs[0] = attr;
//
//		attr = new PotionAttr();
//		attr.Count = 3;
//		attr.ImpactHeroType = 0;
//		attr.ImpactNum = 6;
//		attr.Power = 500;
//		attr.PosName = "祝福之光";
//		attr.Icon = "Med3";
//		attr.Type = _PotionType.Health;
//		BattlePotAttrs[1] = attr;
//
//		attr = new PotionAttr();
//		attr.Count = 7;
//		attr.ImpactHeroType = 0;
//		attr.ImpactNum = 6;
//		attr.Power = 0;
//		attr.PosName = "炎击石";
//		attr.BuffSpriteName = "battle_buff_icon_atk_fire";
//		attr.Icon = "Med1";
//		attr.Type = _PotionType.Enhance;
//		attr.SubType = _SubPotionType.Attack;
//		attr.ElementType = _ElementType.Fire;
//		BattlePotAttrs[2] = attr;
//
//		attr = new PotionAttr();
//		attr.Count = 1;
//		attr.ImpactHeroType = 0;
//		attr.ImpactNum = 1;
//		attr.Power = 100;
//		attr.PosName = "复活药";
//		attr.Icon = "Med2";
//		attr.Type = _PotionType.Rebirth;
//		BattlePotAttrs[3] = attr;
//
//		attr = new PotionAttr();
//		attr.Count = 10;
//		attr.ImpactHeroType = 0;
//		attr.ImpactNum = 6;
//		attr.Power = 0;
//		attr.PosName = "水击石";
//		attr.BuffSpriteName = "battle_buff_icon_atk_water";
//		attr.Icon = "Med4";
//		attr.Type = _PotionType.Enhance;
//		attr.SubType = _SubPotionType.Attack;
//		attr.ElementType = _ElementType.Water;
//		BattlePotAttrs[4] = attr;
//		for(int i = 0;i < BattlePotAttrs.Length;i ++)
//		{
//			battlePotions[i].SetPotionAttr(BattlePotAttrs[i]);
//		}


	}


}
