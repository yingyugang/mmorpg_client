using CSV;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	[System.Serializable]
	public class MSkill:BaseCSVStructure
	{
		//技能名称
		[CsvColumn (CanBeNull = true)]
		public string name{ get; set; }
		//技能描述
		[CsvColumn (CanBeNull = true)]
		public string description{ get; set; }
		//技能Icon
		[CsvColumn (CanBeNull = true)]
		public string skill_icon{ get; set; }
		//技能产生的buff Icon
		public string buff_icon {
			get {
				if (mSkillEffectBaseCSVStructure != null)
					return mSkillEffectBaseCSVStructure.icon;
				else
					return "";
			}
		}
		//技能产生的buff 名称
		public string buff_name {
			get {
				if (mSkillEffectBaseCSVStructure != null)
					return mSkillEffectBaseCSVStructure.name;
				else
					return "";
			}
		}
		//最大施放回合数；
		[CsvColumn (CanBeNull = true)]
		public int max_cast_round{ get; set; }
		//効果続くターン
		[CsvColumn (CanBeNull = true)]
		public int effect_continue_round{ get; set; }
		//效果触发几率
		[CsvColumn (CanBeNull = true)]
		public int effect_possibility{ get; set; }
		//施放的对象
		[CsvColumn (CanBeNull = true)]
		public int cast_target_type{ get; set; }
		//施放的元素对象
		[CsvColumn (CanBeNull = true)]
		public int cast_target_element_type{ get; set; }
		//技能效果类型（HP = 0,MaxHP = 1,Crit = 2,Attack = 3,Defence = 4）
		[CsvColumn (CanBeNull = true)]
		public int effect_type {
			get { 
				if (mSkillEffectBaseCSVStructure != null)
					return mSkillEffectBaseCSVStructure.effect_action_type;
				else
					return -1;
			}
			set {
				CSVManager.Instance.skillEffectBaseDic.TryGetValue (value, out mSkillEffectBaseCSVStructure);
				if (mSkillEffectBaseCSVStructure == null)
					Debug.LogError ("effect type:" + value + " is not exiting!");
			} 
		}

		public int effect_base_type_id{
			get{ 
				if (mSkillEffectBaseCSVStructure != null)
					return mSkillEffectBaseCSVStructure.id;
				return -1;
			}
		}

		public int effect_type_group {
			get { 
				if (mSkillEffectBaseCSVStructure != null)
					return mSkillEffectBaseCSVStructure.effect_action_group;
				else
					return -1;
			}
		}

		//技能element类型（0 = 斩,1 = 打,2 = 突,3 = 魔）
		[CsvColumn (CanBeNull = true)]
		public int element_type{ get; set; }
		//　技能值
		[CsvColumn (CanBeNull = true)]
		public int effect_value_min{ get; set; }
		//技能值
		[CsvColumn (CanBeNull = true)]
		public int effect_value_max{ get; set; }
		//攻击方式(Scope = 0,Single = 1) 必ず目標を命中する。
		[CsvColumn (CanBeNull = true)]
		public int impact_type { get; set; }
		//skill base.
		MSkillEffectBaseCSVStructure mSkillEffectBaseCSVStructure;
		public MSkillEffectBaseCSVStructure GetMSkillEffectBaseCSVStructure(){
			return mSkillEffectBaseCSVStructure;
		}
		//受影响的单位数
		[CsvColumn (CanBeNull = true)]
		public int impact_count{ get; set; }
		//受影响检测时的角度
		[CsvColumn (CanBeNull = true)]
		public int impact_check_radiu{ get; set; }
		//攻击距离
		[CsvColumn (CanBeNull = true)]
		public float range{ get; set; }

		//命中にチェック種類(0:丸で　1:ラインで)
		[CsvColumn (CanBeNull = true)]
		public int hit_check_type{ get; set;}
		//0-360;
		[CsvColumn (CanBeNull = true)]
		public int hit_check_radiu{ get; set;}
		//击中是影响单位个数
		[CsvColumn (CanBeNull = true)]
		public int hit_impact_count{ get; set;}

		//skillの持続時間,skill 終わり時点はskill起こした時点プラスduration.
		[CsvColumn (CanBeNull = true)]
		public int duration{ get; set;}
		//skillが役割する時点
		[CsvColumn (CanBeNull = true)]
		public int active{ get; set;}
		//skillの冷却持続時間
		[CsvColumn (CanBeNull = true)]
		public int cooldown{ get; set;}

		//击中的检测范围
		[CsvColumn (CanBeNull = true)]
		public float hit_range{ get; set; }

		string m_sub_skills;
		//与技能同时释放的子技能(0.8,1|1,2 “｜”分隔多个，“，”分隔时间和子技能id)
		[CsvColumn (CanBeNull = true)]
		public string sub_skills { 
			set {
				m_sub_skills = value;
				subSkills = new List<SubSkill> ();
				string v = value.Replace ("\"", "");
				string[] parts = v.Split ('|');
				for (int i = 0; i < parts.Length; i++) {
					string part = parts [i];
					SubSkill subSkill = new SubSkill ();
					int.TryParse (part, out subSkill.subSkillId);
					subSkills.Add (subSkill);
				}
			}
			get { 
				return m_sub_skills;
			}
		}

		public List<SubSkill> additionEffects;
		string m_addition_effects;
		//附加在技能本身的效果，与子技能不同之处在于只会计算技能表里面少数几个参数。
		[CsvColumn (CanBeNull = true)]
		public string addition_effects { 
			set {
				m_addition_effects = value;
				additionEffects = new List<SubSkill> ();
				string v = value.Replace ("\"", "");
				string[] parts = v.Split ('|');
				for (int i = 0; i < parts.Length; i++) {
					string part = parts [i];
					SubSkill subSkill = new SubSkill ();
					int.TryParse (part, out subSkill.subSkillId);
					additionEffects.Add (subSkill);
				}
			}
			get { 
				return m_addition_effects;
			}
		}

		//技能en值消耗 TODO需要在技能表中添加列来进行配置
		[CsvColumn (CanBeNull = true)]
		public int en_cost{ get; set; }

		[CsvColumn (CanBeNull = true)]
		public int probability{ get; set; }

		[CsvColumn (CanBeNull = true)]
		public int priority{ get; set; }

		public List<SubSkill> subSkills;
		//just use for debug mode.
		public bool isSelected{ get; set; }

		public bool IsSingle(){
			return impact_count == 1;
		}
		//shoot id > 0 means remote attack;
		public int shoot_id { get; set;}
		//skill shoot
		MSkillShoot mSkillShoot;
		public MSkillShoot skillShoot{
			get{
				if(shoot_id <= 0){
					return null;
				}
				if (mSkillShoot == null) {
					mSkillShoot = CSVManager.Instance.GetSkillShoot (shoot_id);
				}
				return mSkillShoot;
			}
		}
	}

	[System.Serializable]
	public class SubSkill
	{
		public float delay;
		public int subSkillId;
	}

}