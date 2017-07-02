using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace DataCenter
{
	public enum CONFIG_MODULE
	{
		[Description("dict/art_resource")] ART_RESOURCE,
		[Description("dict/AttackPart")] ATTACKPART,
		[Description("dict/dict_achievement")] DICT_ACHIEVEMENT,
        //PVE地区字典
		[Description("dict/dict_area")] DICT_AREA,
        //竞技场段位与奖励
		[Description("dict/dict_arena_rank")] DICT_ARENA_RANK,
        //PVE战役字典
		[Description("dict/dict_battle")] DICT_BATTLE,
        //战役用物品ID
		[Description("dict/dict_battle_item")] DICT_BATTLE_ITEM,
        //怪物BOSS类型字典表
		[Description("dict/dict_boss")] DICT_BOSS,
		[Description("dict/dict_building")] DICT_BUILDING,
		[Description("dict/dict_building_level")] DICT_BUILDING_LEVEL,
        //建筑产出
		[Description("dict/dict_building_output")] DICT_BUILDING_OUTPUT,
        //PVE宝箱字典
		[Description("dict/dict_chests")] DICT_CHESTS,
        //每日登录奖励
		[Description("dict/dict_daily_present")] DICT_DAILY_PRESENT,
        //效果类型字典
		[Description("dict/dict_effect")] DICT_EFFECT,
        //PVE怪物队伍字典
		[Description("dict/dict_enemy")] DICT_ENEMY,
        //PVE乱入怪队伍字典
		[Description("dict/dict_enemy_mess")] DICT_ENEMY_MESS,
        //活动字典
		[Description("dict/dict_event")] DICT_EVENT,
        //PVE关卡字典
		[Description("dict/dict_field")] DICT_FIELD,
        //PVE关卡素材掉落字典
		[Description("dict/dict_field_item_drop")] DICT_FIELD_ITEM_DROP,
        //PVE关卡小BOSS波次字典
		[Description("dict/dict_field_smallboss")] DICT_FIELD_SMALLBOSS,
        //好友礼物字典?
		[Description("dict/dict_friend_gift")] DICT_FRIEND_GIFT,
        //英雄类型字典表
		[Description("dict/dict_hero")] DICT_HERO,
        //召唤概率字典表
		[Description("dict/dict_hero_callrate")] DICT_HERO_CALLRATE,
        //英雄进化配方字典
		[Description("dict/dict_hero_evolve")] DICT_HERO_EVOLVE,
        //英雄等级字典 不同星级升级所需经验不同
		[Description("dict/dict_hero_level")] DICT_HERO_LEVEL,
        //用户初始化数据表
		[Description("dict/dict_init_data")] DICT_INIT_DATA,
        //物品字典
		[Description("dict/dict_item")] DICT_ITEM,
        //装备和物品配方字典
		[Description("dict/dict_item_formula")] DICT_ITEM_FORMULA,
        //PVE地图字典表
		[Description("dict/dict_map")] DICT_MAP,
        //怪物字典
		[Description("dict/dict_monster")] DICT_MONSTER,
        //技能基础结构表
		[Description("dict/dict_skill_base")] DICT_SKILL_BASE,
        //技能效果表
		[Description("dict/dict_skill_result")] DICT_SKILL_RESULT,
        //剧情字典表
		[Description("dict/dict_story")] DICT_STORY,
        //剧情对话字典表
		[Description("dict/dict_story_dialog")] DICT_STORY_DIALOG,
        //召唤兽类型字典表
		[Description("dict/dict_summon")] DICT_SUMMON,
		[Description("dict/dict_summon_cfg")] DICT_SUMMON_CFG,
        //召唤兽进化表
		[Description("dict/dict_summon_evolve")] DICT_SUMMON_EVOLVE,
        //召唤兽升级字典
		[Description("dict/dict_summon_level")] DICT_SUMMON_LEVEL,
        //用户升级字典表
		[Description("dict/dict_user_level")] DICT_USER_LEVEL,


		[Description("dict/m_skill")] M_SKILL,
	}
}
