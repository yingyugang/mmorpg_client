using System.Collections;
using CSV;

public enum EffectSuperType{
	Default=0,
	Buff=1,//增益，可驱散
	Debuff=2,//减益，可净化
	Status=3,//增益，可驱散
	DeStatus=4//减益，可净化
};

public enum EffectActionType{
	//直接伤害，毒，剧毒，立即死亡（目标总血量的100%，这种情况下，免疫死亡的效果不会起效)
	HP = 101,
	//立即死亡（目标减血100%）,但是可能会有一个免疫立即死亡的效果，所以跟HP分开。
	DEATH = 102,
	//复活
	REVIVE = 103,
	//攻击力
	ATK = 201,
	//防御力
	DEF = 202,
	//影响暴击
	LUK = 203,
	//法力回复
	MAT = 204,
	//
	MDF = 205,
	//命中
	HIT = 206,
	//在大地图的移动速度
	SPD = 207,
	//
	TEC = 208,
	//部署队伍的消耗
	COST = 209,
	//反击
	THORNS = 300,
	//取消，麻痹
	NOACTION = 301,
	//不能使用技能
	NOSKILL = 302,
	//不能普通攻击
	NODEFAULT = 303,
	//不能攻击
	NOATTACK = 304,
	//不能回复HP
	NOHEALTH = 305,
	//幻化（变成某一个人物，继承它的属性和技能）
	TURNED = 306,
	//赶走,将对方赶出页面外，直到战斗结束。
	GETOUT = 307

};

[System.Serializable]
public class SkillEffectBaseCSVStructure : BaseCSVStructure {

	//效果名称
	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }
	//效果描述
	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }
	//效果Icon
	[CsvColumn (CanBeNull = true)]
	public string icon{ get; set; }

	//效果大类别
	public EffectSuperType effectSuperType;

	public int effect_super_type{
		set{ 
			effectSuperType = (EffectSuperType) value;
		}
	}

	//效果类别
	public EffectActionType effectActionType;

	public int effect_action_type{
		set{ 
			effectActionType = (EffectActionType) value;
		}
	}

}
