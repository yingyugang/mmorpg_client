using UnityEngine;
using System.Collections;

public enum _PotionType{Health=1,Enhance=2,Remove=4,Rebirth=8}
public enum _SubPotionType{None=0,Attack=1,Defence=2,Curse=3,Poison=4}
[System.Serializable]
public class PotionAttr {

	public _PotionType Type = _PotionType.Health;
	public _SubPotionType SubType = _SubPotionType.None;
	public _ElementType ElementType;
	public string BuffSpriteName;
	public string PosName;
	public string Icon;
	public int ImpactNum = 0;
	public int ImpactHeroType = 0;
	public float Power = 0;
	public int Count = 0;

}
