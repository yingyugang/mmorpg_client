using CSV;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class SkillCSVStructure : BaseCSVStructure
{
	//镜像技能id
	[CsvColumn (CanBeNull = true)]
	public int mirror_id{ get; set; }
	//技能名称
	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }
	//技能描述
	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }
	//技能Icon
	[CsvColumn (CanBeNull = true)]
	public string icon{ get; set; }
	//技能类型（0＝自身属性，1＝主动技能，2=装备增强）
	[CsvColumn (CanBeNull = true)]
	public int type{ get; set; }
	//最大施放回合数；
	[CsvColumn (CanBeNull = true)]
	public int max_cast_round{ get; set; }
	//施放优先级，优先级越高越先施放
	[CsvColumn (CanBeNull = true)]
	public int cast_priority{ get; set; }
	//施放几率
	[CsvColumn (CanBeNull = true)]
	public int cast_possibility{ get; set; }
	//施放的对象(0=Self,1=Enemy,2=All)
	[CsvColumn (CanBeNull = true)]
	public int cast_target_type{ get; set; }
	//技能效果类型（HP = 0,MaxHP = 1,Crit = 2,Attack = 3,Defence = 4）
	[CsvColumn (CanBeNull = true)]
	public int effect_type{ get; set; }
	//技能element类型（0 = 斩,1 = 打,2 = 突,3 = 魔）
	[CsvColumn (CanBeNull = true)]
	public int element_type{ get; set; }
	//技能值
	[CsvColumn (CanBeNull = true)]
	public int effect_value_min{ get; set; }
	//技能值
	[CsvColumn (CanBeNull = true)]
	public int effect_value_max{ get; set; }
	//技能值(百分比)
	[CsvColumn (CanBeNull = true)]
	public int effect_percent_value_min{ get; set; }
	//技能值(百分比)
	[CsvColumn (CanBeNull = true)]
	public int effect_percent_value_max{ get; set; }
	//技能值(目标的百分比)
	[CsvColumn (CanBeNull = true)]
	public int effect_target_percent_value_min{ get; set; }
	//技能值(目标的百分比)
	[CsvColumn (CanBeNull = true)]
	public int effect_target_percent_value_max{ get; set; }
	//攻击方式(Scope = 0,Single = 1)
	[CsvColumn (CanBeNull = true)]
	public int impact_type{ get; set; }
	//受影响的单位数
	[CsvColumn (CanBeNull = true)]
	public int impact_count{ get; set; }
	//技能覆盖方式(AddUp = 0,Cover = 1)
	[CsvColumn (CanBeNull = true)]
	public int cover_type{ get; set; }
	//演出type
	[CsvColumn (CanBeNull = true)]
	public int perform_type{ get; set; }
	//攻击距离(近战为0)
	[CsvColumn (CanBeNull = true)]
	public int range{ get; set; }
	//技能次数和时间间隔(0.8,0.3,5|1,0.5,5,2 “｜”分隔多个，“，”分隔时间/技能数值百分比/伤害的次数)
	[CsvColumn (CanBeNull = true)]
	public string effect_partments {
		set {
			partments = new List<SkillEffectPartment> ();
			string v = value.Replace ("\"", "");
			string[] parts = v.Split ('|');
			float power = 0;
			for (int i = 0; i < parts.Length; i++) {
				string part = parts [i];
				string[] subs = part.Split (',');
				SkillEffectPartment attackEffectPartment = new SkillEffectPartment ();
				if (subs.Length > 0) {
					float.TryParse (subs [0], out attackEffectPartment.delay);
				}
				if (subs.Length > 1) {
					float.TryParse (subs [1], out attackEffectPartment.effectValue);
					float realPower = Mathf.Min (1 - power, attackEffectPartment.effectValue);//总power不超过1.0f
					attackEffectPartment.effectValue = realPower;
					power += realPower;
				}
				if (subs.Length > 2) {
					int.TryParse (subs [2], out attackEffectPartment.partCount);
					attackEffectPartment.partCount = Mathf.Max (1,attackEffectPartment.partCount);
				}
				partments.Add (attackEffectPartment);
			}
		}
	}
	//与技能同时释放的子技能(0.8,1|1,2 “｜”分隔多个，“，”分隔时间和子技能id)
	[CsvColumn (CanBeNull = true)]
	public string sub_skills { 
		set {
			subSkills = new List<SubSkill> ();
			string v = value.Replace ("\"", "");
			string[] parts = v.Split ('|');
			for (int i = 0; i < parts.Length; i++) {
				string part = parts [i];
//				string[] subs = part.Split (',');
				SubSkill subSkill = new SubSkill ();
//				if (subs.Length > 0)
				int.TryParse (part, out subSkill.subSkillId);
//				if (subs.Length > 1)
//					int.TryParse (subs [1], out subSkill.subSkillId);
				subSkills.Add (subSkill);
			}
		}
	}

	public List<SkillEffectPartment> partments;

	public List<SubSkill> subSkills;

	//不支持递归,效率和死循环问题。TODO
	public void CopyFromMirror (SkillCSVStructure mirror)
	{
		if (this.type == 0)
			this.type = mirror.type;
		if (this.max_cast_round == 0)
			this.max_cast_round = mirror.max_cast_round;
		if (this.cast_priority == 0)
			this.cast_priority = mirror.cast_priority;
		if (this.cast_possibility == 0)
			this.cast_possibility = mirror.cast_possibility;
		if (this.cast_target_type == 0)
			this.cast_target_type = mirror.cast_target_type;
		if (this.effect_type == 0)
			this.effect_type = mirror.effect_type;
		if (this.element_type == 0)
			this.element_type = mirror.element_type;
		if (this.effect_value_min == 0)
			this.effect_value_min = mirror.effect_value_min;
		if (this.effect_value_max == 0)
			this.effect_value_max = mirror.effect_value_max;
		if (this.effect_percent_value_min == 0)
			this.effect_percent_value_min = mirror.effect_percent_value_min;
		if (this.effect_percent_value_max == 0)
			this.effect_percent_value_max = mirror.effect_percent_value_max;
		if (this.impact_type == 0)
			this.impact_type = mirror.impact_type;
		if (this.impact_count == 0)
			this.impact_count = mirror.impact_count;
		if (this.cover_type == 0)
			this.cover_type = mirror.cover_type;
		if (this.perform_type == 0)
			this.perform_type = mirror.perform_type;
		if (this.range == 0)
			this.range = mirror.range;
		if (this.partments == null || this.partments.Count == 0)
			this.partments = mirror.partments;
		if (this.subSkills == null || this.subSkills.Count == 0)
			this.subSkills = mirror.subSkills;
	}

}

[System.Serializable]
public class SkillEffectPartment
{
	public int partCount = 5;
	public float delay;
	public float effectValue;
	public List<Unit> partmentTargets;
	public List<Hashtable> partmentParams;
}

[System.Serializable]
public class SubSkill
{
	public float delay;
	public int subSkillId;
}
