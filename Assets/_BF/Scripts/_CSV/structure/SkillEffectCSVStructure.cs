using System.Collections;
using CSV;

[System.Serializable]
public class SkillEffectCSVStructure : BaseCSVStructure {

	//镜像效果id
	[CsvColumn (CanBeNull = true)]
	public int mirror_id{ get;set; }
	//效果名称
	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }
	//效果描述
	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }
	//效果Icon
	[CsvColumn (CanBeNull = true)]
	public string icon{ get; set; }
	//基础效果id
	[CsvColumn (CanBeNull = true)]
	public int skill_effect_base_id{ get; set; }
	//效果出现几率
	[CsvColumn (CanBeNull = true)]
	public int effect_possibility{ get; set; }
	//效果类型
	[CsvColumn (CanBeNull = true)]
	public int effect_type{ get; set; }
	//效果开始回合
	[CsvColumn (CanBeNull = true)]
	public int effect_start_round{ get; set; }
	//最大持续回合
	[CsvColumn (CanBeNull = true)]
	public int effect_max_round{ get; set; }
	//最小绝对值
	[CsvColumn (CanBeNull = true)]
	public int effect_value_min{ get; set; }
	//最大绝对值
	[CsvColumn (CanBeNull = true)]
	public int effect_value_max{ get; set; }
	//最小百分比值
	[CsvColumn (CanBeNull = true)]
	public int effect_percent_value_min{ get; set; }
	//最大百分比值
	[CsvColumn (CanBeNull = true)]
	public int effect_percent_value_max{ get; set; }
	//基于目标最小百分比值
	[CsvColumn (CanBeNull = true)]
	public int effect_percent_target_value_min{ get; set; }
	//基于目标最大百分比值
	[CsvColumn (CanBeNull = true)]
	public int effect_percent_target_value_max{ get; set; }

	//不支持递归,效率和死循环问题。TODO
	public void CopyFromMirror (SkillEffectCSVStructure mirror)
	{
		if (this.effect_possibility == 0)
			this.effect_possibility = mirror.effect_possibility;
		if (this.effect_type == 0)
			this.effect_type = mirror.effect_type;
		if (this.effect_start_round == 0)
			this.effect_start_round = mirror.effect_start_round;
		if (this.effect_max_round == 0)
			this.effect_max_round = mirror.effect_max_round;
		if (this.effect_value_min == 0)
			this.effect_value_min = mirror.effect_value_min;
		if (this.effect_value_max == 0)
			this.effect_value_max = mirror.effect_value_max;
		if (this.effect_percent_value_min == 0)
			this.effect_percent_value_min = mirror.effect_percent_value_min;
		if (this.effect_percent_value_max == 0)
			this.effect_percent_value_max = mirror.effect_percent_value_max;
		if (this.effect_percent_target_value_min == 0)
			this.effect_percent_target_value_min = mirror.effect_percent_target_value_min;
		if (this.effect_percent_target_value_max == 0)
			this.effect_percent_target_value_max = mirror.effect_percent_target_value_max;
	}

}
