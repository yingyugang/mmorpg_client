using CSV;
using System.Collections.Generic;


[System.Serializable]
public class UnitSkillCSVStructure: BaseCSVStructure
{
	//unit id
	[CsvColumn (CanBeNull = true)]
	public int unit_id{ get; set; }
	//skill id
	[CsvColumn (CanBeNull = true)]
	public int skill_id{ get; set; }
	//技能名称
	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }
	//技能描述
	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }
	//最大施放回合数；
	[CsvColumn (CanBeNull = true)]
	public int max_cast_round{ get; set; }
	//施放优先级，优先级越高越先施放
	[CsvColumn (CanBeNull = true)]
	public int cast_priority{ get; set; }
	//施放几率
	[CsvColumn (CanBeNull = true)]
	public int cast_possibility{ get; set; }
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
	//受影响的单位数
	[CsvColumn (CanBeNull = true)]
	public int impact_count{ get; set; }
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
			for (int i = 0; i < parts.Length; i++) {
				string part = parts [i];
				string[] subs = part.Split (',');
				SkillEffectPartment attackEffectPartment = new SkillEffectPartment ();
				if (subs.Length > 0)
					float.TryParse (subs [0], out attackEffectPartment.delay);
				if (subs.Length > 1)
					float.TryParse (subs [1], out attackEffectPartment.effectValue);
				if (subs.Length > 2)
					int.TryParse (subs [2], out attackEffectPartment.partCount);
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
				SubSkill subSkill = new SubSkill ();
				float.TryParse (part, out subSkill.delay);
				subSkills.Add (subSkill);
			}
		}
	}

	public List<SkillEffectPartment> partments;

	public List<SubSkill> subSkills;
}
