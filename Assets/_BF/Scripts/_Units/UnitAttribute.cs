using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UnitAttribute  {

	public Dictionary<EffectType,int> attributes;

	public UnitAttribute(){
		attributes = new Dictionary<EffectType, int> ();
		attributes.Add (EffectType.HP,5000);//TODO
		attributes.Add (EffectType.MaxHP,5000);//TODO
		attributes.Add (EffectType.DefenceMuilt,0);
		attributes.Add (EffectType.DamageMuilt,0);
		attributes.Add (EffectType.CritOdds,0);
		attributes.Add (EffectType.DodgeOdds, 0);
		attributes.Add (EffectType.SkillOdds, 0);
		attributes.Add (EffectType.DefenceOdds, 0);
		attributes.Add (EffectType.ElementMuilt, 0);
		attributes.Add (EffectType.DamageReduceMuilt, 0);
		attributes.Add (EffectType.SuckBlood, 0);
		attributes.Add (EffectType.Speed,0);
		attributes.Add (EffectType.CommonCD,0);
		attributes.Add (EffectType.UsedCD,0);
		attributes.Add (EffectType.Element,0);
		attributes.Add (EffectType.BaseDamage, 100);
		attributes.Add (EffectType.BaseDefence, 100);
	}

}

