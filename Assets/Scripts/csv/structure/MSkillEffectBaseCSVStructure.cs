using System.Collections;
using CSV;

namespace MMO
{

	[System.Serializable]
	public class MSkillEffectBaseCSVStructure : BaseCSVStructure
	{

		//效果名称，跟技能名称不同之处在于，这里是在人物buff上显示的名字。
		[CsvColumn (CanBeNull = true)]
		public string name{ get; set; }
		//效果描述，同上
		[CsvColumn (CanBeNull = true)]
		public string description{ get; set; }
		//效果Icon，同上
		[CsvColumn (CanBeNull = true)]
		public string icon{ get; set; }
		//效果类别
		public int effect_action_type{ get; set; }
		//效果类别分组
		public int effect_action_group{ get; set; }

	}

}
