using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
	public class DataSkill: DataModule
	{
		public override bool init ()
		{
			return base.init ();
		}

		public override void release()
		{

		}

		public ConfigRow[] getSkillBaseData()
		{
			ConfigRow[] rows = {};
			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SKILL_BASE);		
			if (table != null)
			{
				rows = table.rows;
			}
			return rows;
		}

		public SkillBase getSkillBaseDataBySkillID(int skillID)
		{
			SkillBase skillbase = null;
            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SKILL_BASE);		
			if (table != null)
			{
				for (int i = 0; i < table.rows.Length; i ++)
				{
                    if (table.rows[i].getIntValue(DICT_SKILL_BASE.ID) == skillID)
					{
						skillbase = new SkillBase();
						ConfigRow row = table.rows[i];
                        skillbase.sid = row.getIntValue(DICT_SKILL_BASE.ID);
                        skillbase.name = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_SKILL_BASE.NAME_ID));
                        skillbase.desc = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_SKILL_BASE.DESC_ID));
                        skillbase.icon = row.getStringValue(DICT_SKILL_BASE.ICON_FILE);
                        skillbase.actionDisplay = row.getStringValue(DICT_SKILL_BASE.ACTION_DISPLAY);
                        skillbase.skillType = row.getIntValue(DICT_SKILL_BASE.TYPE_ID);
                        skillbase.powerReq = row.getIntValue(DICT_SKILL_BASE.POWER_REQ);
						skillbase.skillResultID = new List<int>();
                        skillbase.skillResultID.Add(row.getIntValue(DICT_SKILL_BASE.SKILL_RESULT1));
                        skillbase.skillResultID.Add(row.getIntValue(DICT_SKILL_BASE.SKILL_RESULT2));

						skillbase.skillAdvance = new List<int>();
                        skillbase.skillAdvance.Add(row.getIntValue(DICT_SKILL_BASE.SKILL_ADVANCE1));
                        skillbase.skillAdvance.Add(row.getIntValue(DICT_SKILL_BASE.SKILL_ADVANCE2));
						break;
					}
				}
			}

			 
			return skillbase;
		}


		public ConfigRow[] getSkillResultData()
		{
			ConfigRow[] rows = {};
			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SKILL_RESULT);		
			if (table != null)
			{
				rows = table.rows;
			}
			return rows;
		}

		public SkillResult getSkillResultDataBySkillID(int skillResultID)
		{
			SkillResult skillResult = null;
			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SKILL_RESULT);		
			if (table != null)
			{
				for (int i = 0; i < table.rows.Length; i ++)
				{
                    if (table.rows[i].getIntValue(DICT_SKILL_RESULT.ID) == skillResultID)
					{
						ConfigRow row = table.rows[i];
						skillResult = new SkillResult();
                        skillResult.id = row.getIntValue(DICT_SKILL_RESULT.ID);
                        skillResult.effecttype = (AttackType)row.getIntValue(DICT_SKILL_RESULT.EFFECT_TYPE);
                        skillResult.specialcondition = row.getIntValue(DICT_SKILL_RESULT.SPECIAL_CONDITION);
                        skillResult.scdata1 = row.getIntValue(DICT_SKILL_RESULT.SCDATA1);
                        skillResult.target = row.getIntValue(DICT_SKILL_RESULT.TARGET);
                        skillResult.tdata1 = row.getIntValue(DICT_SKILL_RESULT.TDATA1);
                        skillResult.targetnum = row.getIntValue(DICT_SKILL_RESULT.TARGET_NUM);
                        skillResult.delaytime = row.getIntValue(DICT_SKILL_RESULT.DELAY_TIME);
                        skillResult.bufficon = row.getStringValue(DICT_SKILL_RESULT.BUFF_ICON);
                        skillResult.effectobject = row.getIntValue(DICT_SKILL_RESULT.EFFECT_OBJECT);
                        skillResult.effectvaluetype = row.getIntValue(DICT_SKILL_RESULT.EFFECT_VALUE_TYPE);
                        skillResult.effectvalue1 = row.getIntValue(DICT_SKILL_RESULT.EFFECT_VALUE1);
                        skillResult.effectvalue2 = row.getIntValue(DICT_SKILL_RESULT.EFFECT_VALUE2);
						break;
					}
				}
			}
			return skillResult;
		}


	}
}