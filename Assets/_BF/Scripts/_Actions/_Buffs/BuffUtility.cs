using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class BuffUtility
{

	public static void AddBuff (Hero hero, BaseSkill buff)
	{
		if (hero.buffs == null) {
			hero.buffs = new List<BaseSkill> ();
		}
		if (hero.mainBuffs == null) {
			hero.mainBuffs = new List<BaseSkill> ();
		}
	}

}
