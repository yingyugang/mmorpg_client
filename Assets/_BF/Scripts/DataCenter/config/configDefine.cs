using System;

namespace DataCenter
{
	public enum ART_RESOURCE
	{
		ID,
		RESOURCE,
	}

	public enum ATTACKPART
	{
		HERO_TYPEID,
		PART1,
		PART2,
		PART3,
		PART4,
		PART5,
		PART6,
		PART7,
		PART8,
		PART9,
		PART10,
		PART11,
		PART12,
		PART13,
		PART14,
		PART15,
		PART16,
		PART17,
		PART18,
		PART19,
		PART20,
	}

	public enum DICT_ACHIEVEMENT
	{
        //int;成就类型ID（具体的枚举详见成就系统注释.txt）
		TYPEID,
        //tinyint;成就分类(具体的枚举详见成就系统注释.txt)
		CLASSIFY,
        //tinyint;成就的最大阶段（如成就：等级达到5,10,15，那么等级达到x级这系列成就的max_step=3）
		MAX_STEP,
        //int;CLIENT 成就描述,多语言，支持ID查询
		NAME_ID,
        //int;CLIENT 成就描述,多语言，支持ID查询
		DESC_ID,
        //tinyint;成就阶段（如成就：等级达到5,10,15..,等级达到5级即为该成就的第1阶段，step值为1）
		STEP,
        //int;成就参数1（根据不同的成就类型有不同的意义）
		PARAM1,
        //int;成就参数2（根据不同的成就类型有不同的意义）
		PARAM2,
        //int;成就参数3（根据不同的成就类型有不同的意义）
		PARAM3,
        //int;成就参数4（根据不同的成就类型有不同的意义）
		PARAM4,
        //int;成就参数5（根据不同的成就类型有不同的意义）
		PARAM5,
        //int;成就参数6（根据不同的成就类型有不同的意义）
		PARAM6,
        //int;成就累积值（如成就：等级达到5级，这里记录5）
		NEED_ACCUMULATE_VALUE,
        //int;奖励金币
		REWARD_COIN,
        //int;奖励魂
		REWARD_SOUL,
        //smallint;奖励钻石
		REWARD_DIAMOND,
        //int;奖励物品类型ID
		REWARD_ITEM_TYPEID,
        //tinyint;奖励物品数量
		REWARD_ITEM_AMOUNT,
        //int;奖励英雄类型ID
		REWARD_HERO_TYPEID,
        //tinyint;奖励英雄数量
		REWARD_HERO_AMOUNT,
	}

    //PVE地区字典
	public enum DICT_AREA
	{
        //int;地区ID
		AREA_ID,
        //int;客户端字符串ID
		NAME_ID,
        //int;客户端字符串ID
		DESC_ID,
        //int;前置关卡ID
		PRE_FIELD_ID,
	}

    //竞技场段位与奖励
	public enum DICT_ARENA_RANK
	{
        //int;
		ID,
        //int;所需竞技点数
		ARENA_POINT,
        //int;客户端用 段位名称
		NAME_ID,
        //smallint;钻石奖励
		DIAMOND,
        //int;物品奖励
		ITEMTYPE_ID,
	}

    //PVE战役字典
	public enum DICT_BATTLE
	{
        //int;
		BATTLE_ID,
        //int;客户端字符串ID
		NAME_ID,
        //int;客户端字符串ID
		DESC_ID,
        //smallint;
		STYLE,
        //smallint;0:无条件开启 1:需要钥匙 2:需要道具 3:周X开启 4:月(每月几号) 5:某个时间段YYMMDDHHMM
		COND,
        //int;
		COND_PARAM1,
        //int;
		COND_PARAM2,
        //int;
		PRE_FIELD_ID,
        //int;
		MAP_ID,
        //decimal;
		POSX,
        //decimal;
		POSY,
        //int;
		REWARD_RULE,
        //varchar;场景资源名(客户端用)
		SCENE,
        //int;场景类型1单一场景，2滚屏场景(客户端用）
		SCENETYPE,
	}

    //战役用物品ID
	public enum DICT_BATTLE_ITEM
	{
        //int;
		BATTLE_ID,
        //int;
		GROUP_ID,
        //int;
		USE_ITEM1_TYPEID,
        //smallint;
		USE_AMOUNT1,
        //int;
		USE_ITEM2_TYPEID,
        //smallint;
		USE_AMOUNT2,
        //int;
		USE_ITEM3_TYPEID,
        //smallint;
		USE_AMOUNT3,
        //int;
		USE_ITEM4_TYPEID,
        //smallint;
		USE_AMOUNT4,
        //int;
		USE_ITEM5_TYPEID,
        //smallint;
		USE_AMOUNT5,
	}

    //怪物BOSS类型字典表
	public enum DICT_BOSS
	{
        //int;
		BOSS_TYPEID,
        //int;客户端字符串ID
		NAME_ID,
        //int;客户端字符串ID
		DESC_ID,
        //varchar;客户端，相对于ASSET的路径名
		ICON_FILE,
        //varchar;客户端，ICON_FILE中的文件名
		ICON_SPRITE_NAME,
        //varchar;客户端，相对于ASSET的路径名
		FBX_FILE,
        //varchar;客户端，相对于ASSET的路径名
		PORTARAIT,
	}

	public enum DICT_BUILDING
	{
        //int;15001:村落；15002：宝玉屋；15003：调和屋；15004：道具仓库；15005：田地；15006：山川；15007：河流；15008：大树；15009：音乐屋。
		BUILDING_TYPEID,
        //int;
		NAME_ID,
        //int;
		DESC_ID,
        //int;
		MAX_LEVEL,
        //int;
		OPEN_USER_LEVEL,
	}

	public enum DICT_BUILDING_LEVEL
	{
        //int;
		BUILDING_TYPEID,
        //int;
		LEVEL,
        //int;
		COST_SOUL,
        //int;
		EFFECT1,
        //int;
		EFFECT2,
        //int;
		EFFECT3,
	}

    //建筑产出
	public enum DICT_BUILDING_OUTPUT
	{
        //int;
		OUTPUT_TYPEID,
        //tinyint;1:金币，2：魂，3：物品
		CLASSIFY,
        //int;
		ITEM_TYPEID,
        //smallint;
		AMOUNT,
        //smallint;
		RATE,
	}

    //PVE宝箱字典
	public enum DICT_CHESTS
	{
        //int;
		CHESTS_ID,
        //int;
		COIN_DROP_RATE,
        //int;
		COIN_DROP_MIN,
        //int;
		COIN_DROP_MAX,
        //int;
		SOUL_DROP_RATE,
        //int;
		SOUL_DROP_MIN,
        //int;
		SOUL_DROP_MAX,
        //int;
		ITEM_DROP_RATE,
        //int;
		MONSTER_STAR_THREE,
        //int;
		MONSTER_STAR_FOUR,
        //int;
		MONSTER_STAR_FIVE,
	}

    //每日登录奖励
	public enum DICT_DAILY_PRESENT
	{
        //int;顺序领取的次数
		ID,
        //int;礼品类型 0:物品 1:英雄 2:友情点 3:金币 4:魂 5:钻石
		TAG,
        //int;
		PARA1,
        //int;表示物品数量  英雄数量
		PARA2,
	}

    //效果类型字典
	public enum DICT_EFFECT
	{
        //int;
		EFFECT_ID,
        //smallint;
		EFFECT_TYPE,
        //smallint;
		AFFECT_SUBTYPE,
        //tinyint;
		AFFECT_SIDE,
        //tinyint;
		AFFECT_OBJECT,
        //smallint;
		AFFECT_COUNT,
        //smallint;
		AFFECT_DURATION,
        //smallint;
		COND,
        //tinyint;
		COND_SIDE,
        //tinyint;
		COND_OBJECT,
        //decimal;
		COND_VALUE,
        //tinyint;
		VALUE_TYPE,
        //decimal;
		VALUE,
	}

    //PVE怪物队伍字典
	public enum DICT_ENEMY
	{
        //int;
		ENEMY_ID,
        //int;
		MONSTER_TYPEID,
        //smallint;
		MONSTER_LOCATION,
	}

    //PVE乱入怪队伍字典
	public enum DICT_ENEMY_MESS
	{
        //int;
		ENEMY_ID,
        //int;概率
		RATE,
        //tinyint;玩家等级限制
		USER_LEVEL,
	}

    //活动字典
	public enum DICT_EVENT
	{
        //int;活动ID
		ID,
        //varchar;描述
		DESCRIPTION,
        //int;0: 活动无条件关闭 1: 活动无条件开启 2: 起始终止时间，其中PARA1-起始时间 PARA2-终止时间  3: 每周固定星期X活动 PARA1 星期1-7(按位方式) PARA2 开始小时0-24 PARA3结束小时0-24
		RULE,
        //int;
		PARA1,
        //int;
		PARA2,
        //int;
		PARA3,
	}

    //PVE关卡字典
	public enum DICT_FIELD
	{
        //int;
		FIELD_ID,
        //int;
		BATTLE_ID,
        //int;客户端字符串ID
		NAME_ID,
        //int;客户端字符串ID
		DESC_ID,
        //int;前置关卡ID(依赖条件)
		PRE_FIELD_ID,
        //int;下一关卡ID(客户端排序用)
		NEXT_FIELD_ID,
        //int;
		EXP,
        //smallint;
		COST_ENARGY,
        //smallint;
		MAX_STEP,
        //int;
		ENEMY1_ID,
        //int;
		ENEMY1_RATE,
        //int;
		ENEMY2_ID,
        //int;
		ENEMY2_RATE,
        //int;
		ENEMY3_ID,
        //int;
		ENEMY3_RATE,
        //int;
		ENEMY4_ID,
        //int;
		ENEMY4_RATE,
        //int;
		ENEMY5_ID,
        //int;
		ENEMY5_RATE,
        //int;
		ENEMY6_ID,
        //int;
		ENEMY6_RATE,
        //int;
		MESS_STEP_SUM,
        //int;
		FINAL_BOSS_ENEMY_ID,
        //int;
		CHESTS_ID,
        //int;
		CHESTS_RATE,
        //int;
		MONSTER_DROP_COIN_MAX,
        //int;
		MONSTER_DROP_SOUL_MAX,
	}

    //PVE关卡素材掉落字典
	public enum DICT_FIELD_ITEM_DROP
	{
        //int;
		FIELD_ID,
        //int;
		ITEM_TYPEID,
        //int;
		DROP_RATE,
        //int;
		DROP_MIN,
        //int;
		DROP_MAX,
	}

    //PVE关卡小BOSS波次字典
	public enum DICT_FIELD_SMALLBOSS
	{
        //int;
		FIELD_ID,
        //int;小BOSS波次
		STEP,
        //int;小BOSS波次队伍
		ENEMY_ID,
	}

    //好友礼物字典?
	public enum DICT_FRIEND_GIFT
	{
        //int;
		GIFT_TYPEID,
        //tinyint;
		CLASSIFY,
        //int;
		ITEM_TYPEID,
        //smallint;
		AMOUNT,
	}

    //英雄类型字典表
	public enum DICT_HERO
	{
        //int;
		HERO_TYPEID,
        //int;客户端字符串ID
		NAME_ID,
        //int;客户端字符串ID
		DESC_ID,
        //int;队长技能字符串ID
		CAPTAIN_SKILL_NAME,
        //int;队长技能字符串ID
		CAPTAIN_SKILL_DESC,
        //int;必杀技能字符串ID
		BASE_SKILL_NAME,
        //int;必杀技能字符串ID
		BASE_SKILL_DESC,
        //varchar;客户端，相对于ASSET的路径名
		ICON_FILE,
        //varchar;客户端，ICON_FILE中的文件名
		ICON_SPRITE_NAME,
        //varchar;客户端，相对于ASSET的路径名
		FBX_FILE,
        //varchar;客户端，相对于ASSET的路径名
		PORTARAIT,
        //tinyint;客户端，攻击方式:  0:近战攻击  1:远程攻击
		ATK_METHOD,
        //float;客户端，移动速度
		MOVE_SPEED,
        //tinyint;升级方式，0: 弱势英雄(1.0)，1: 普通英雄(1.33), 2: 强势英雄(2.0), 3: 主角(1.0)，4：经验宝宝，5：金钱宝宝，6：进化材料，7：爆炸青蛙，8：经验+金钱宝宝
		LVLUP_METHOD,
        //tinyint;水火木风 光明 暗影  秘法 暂定只属于一个系
		SERIES,
        //tinyint;星级 1-9
		STAR,
        //tinyint;
		LEADER,
        //smallint;
		LIBRARY,
        //int;
		SKILL_CAPTAIN,
        //int;
		SKILL_BASE,
        //int;作为强化材料获得的英雄 特殊修正经验 + 英雄星级修正
		EXP,
        //int;出售时获得的金币
		COIN,
        //int;
		INIT_HP,
        //int;
		INIT_ATK,
        //int;
		INIT_DEF,
        //int;
		INIT_RECOVER,
        //smallint;概率值 万分比
		INIT_VIOLENCE,
        //tinyint;英雄升级加点数量
		BASE_HP,
        //tinyint;英雄升级加点数量
		BASE_ATK,
        //tinyint;英雄升级加点数量
		BASE_DEF,
        //tinyint;英雄升级加点数量
		BASE_RECOVER,
        //smallint;概率值 万分比
		BASE_VIOLENCE,
        //int;攻击类型，1近战，2远程（客户端用）
		MOVABLE,
		//float:普通技能延迟
		DEFAULT_SKILL_EFFECT_DELAY,
		//float:特殊技能延迟
		SKILL_EFFECT_DELAY,

	}

    //召唤概率字典表
	public enum DICT_HERO_CALLRATE
	{
        //int;1:钻石抽卡 2:友情抽卡 3:抽经验胖子
		METHOD,
        //int;
		HERO_TYPEID,
        //int;万分比
		NORMAL_RATE,
        //int;活动期间的概率 万分比
		EVENT_RATE,
        //int;活动ID 对应dict_event字典
		EVENT_ID,
	}

    //英雄进化配方字典
	public enum DICT_HERO_EVOLVE
	{
        //int;
		HERO_TYPEID,
        //int;进化后的英雄类型
		EVOLVE_TYPEID,
        //int;进化所需金币
		COIN,
        //int;
		COST_HERO_1,
        //int;
		COST_HERO_2,
        //int;
		COST_HERO_3,
        //int;
		COST_HERO_4,
        //int;
		COST_HERO_5,
	}

    //英雄等级字典 不同星级升级所需经验不同
	public enum DICT_HERO_LEVEL
	{
        //int;
		LEVEL,
        //int;
		EXP,
	}

    //用户初始化数据表
	public enum DICT_INIT_DATA
	{
        //varchar;
		DATA_TYPE,
        //int;
		DATA_ORDER,
        //int;
		DATA_TYPEID,
        //int;
		DATA_AMOUNT,
	}

    //物品字典
	public enum DICT_ITEM
	{
        //int;
		ITEM_TYPEID,
        //int;1素材，2道具（药品），3宝玉，4召唤兽碎片
		SORT,
        //int;客户端字符串ID
		NAME_ID,
        //int;客户端字符串ID
		DESC_ID,
        //int;客户端字符串ID
		DESC_EX_ID,
        //varchar;客户端，相对于ASSET的路径名
		ICON_FILE,
        //varchar;客户端，ICON_FILE中的文件名
		ICON_SPRITE_NAME,
        //int;客户端，增加的属性值类型，按生命、攻击、防御、回复这样的顺序，以2进制的方式记录数据
		VALUE_TYPE,
        //int;客户端，基础值的百分比值，基数10000.
		VALUE,
        //smallint;是否可堆叠
		STACKABLE,
        //int;
		PRICE,
        //int;
		MAX_USED,
        //int;
		EFFECT_ID,
        //smallint;
		LIBRARY,
	}

    //装备和物品配方字典
	public enum DICT_ITEM_FORMULA
	{
        //int;
		ITEM_TYPEID,
        //smallint;
		BUILDING_LEVEL,
        //int;
		COST_SOUL,
        //int;
		COST_ITEM1_TYPEID,
        //smallint;
		COST_AMOUNT1,
        //int;
		COST_ITEM2_TYPEID,
        //smallint;
		COST_AMOUNT2,
        //int;
		COST_ITEM3_TYPEID,
        //smallint;
		COST_AMOUNT3,
        //int;
		COST_ITEM4_TYPEID,
        //smallint;
		COST_AMOUNT4,
        //int;
		COST_ITEM5_TYPEID,
        //smallint;
		COST_AMOUNT5,
	}

    //PVE地图字典表
	public enum DICT_MAP
	{
        //int;地图ID
		MAP_ID,
        //int;地区ID
		AREA_ID,
        //int;客户端字符串ID
		NAME_ID,
        //int;客户端字符串ID
		DESC_ID,
        //int;前置关卡ID
		PRE_FIELD_ID,
	}

    //怪物字典
	public enum DICT_MONSTER
	{
        //int;
		MONSTER_TYPEID,
        //varchar;
		NAME,
        //tinyint;
		CLASS,
        //int;
		HP,
        //int;
		ATK,
        //int;
		DEF,
        //int;
		RECOVER,
        //int;
		VIOLENCE,
        //tinyint;
		STATUS_POISON,
        //tinyint;
		STATUS_WEAK,
        //tinyint;
		STATUS_SICK,
        //tinyint;
		STATUS_HURT,
        //tinyint;
		STATUS_PARALYSIS,
        //tinyint;
		STATUS_CURSE,
        //int;
		SPARK_COIN,
        //int;
		SPARK_SOUL,
        //int;
		SKILL_BASE,
        //tinyint;
		ACTION_CNT_MIN,
        //tinyint;
		ACTION_CNT,
        //int;预留
		AI,
        //int;
		CATCH_ID,
        //smallint;
		CATCH_RATE,
        //tinyint;捕获等级下限
		CATCH_LEVEL_MIN,
        //tinyint;捕获等级上限
		CATCH_LEVEL_MAX,
        //int;是否是BOSS(客户端用)
		IS_BOSS,
	}

    //技能基础结构表
	public enum DICT_SKILL_BASE
	{
        //int;值范围：【45000-49999】
		ID,
        //int;名称
		NAME_ID,
        //int;描述
		DESC_ID,
        //varchar;技能图标存放路径
		ICON_FILE,
        //varchar;技能动作表现动画存放路径
		ACTION_DISPLAY,
        //int;技能类型ID 值范围：【0-9】0-队长技能（都为被动技能）1-BB技能（都为主动技能），2-装备效果，3-药品效果
		TYPE_ID,
        //int;技能的能量消耗【0-999】
		POWER_REQ,
        //int;对应【skill_result】表中ID字段，表示技能第一个效果的id。范围：【0,999999】
		SKILL_RESULT1,
        //int;
		SKILL_RESULT2,
        //int;技能成长系数(客户端用)
		SKILL_ADVANCE1,
        //int;技能成长系数(客户端用)
		SKILL_ADVANCE2,
	}

    //技能效果表
	public enum DICT_SKILL_RESULT
	{
        //int;技能效果ID（唯一标识一种魔法效果）值范围：【0,999999】 格式：sid+n，比如rid=450001，表示是技能id为45000的第一个效果
		ID,
        //int;技能效果类别 值范围：【0,99】1：直接伤害类 2：直接治疗类 3：增益类（包括持续治疗） 4：减益类（包括持续伤害） 5：其他
		EFFECT_TYPE,
        //int; 值范围：【0,99】技能效果触发条件（这里的条件，都需要特别的定义）0：不受条件影响 1：战斗开始后SCData1回合内 2：每个回合开始时 3：每个回合结束时 4：合击时 5：暴击时 6：队伍中拥有SCData1种不同元素的队员 7：对每个目标攻击SCData1%几率触发  
		SPECIAL_CONDITION,
        //int;技能效果触发条件参数1 值范围：【0,999999】
		SCDATA1,
        //int;可作用目标类型，也可以认为是搜索目标类 值范围：【0,99】 1：敌方单位 2：TData1特指属性的友方单位
		TARGET,
        //int;技能效果目标参数 值范围：【0,999999】如果TData1用于特指元素属性，那么：0：无属性要求 1：水，2：火，3：木，4：风，5：光，6：暗
		TDATA1,
        //int;效果作用目标最大数量 值范围：【0,99】 注：0表示没有限制
		TARGET_NUM,
        //int;技能效果持续回合 范围【0,99】；单位：回合 99：表示无限，即持续整场战斗，表现为被动技能  0：该技能效果是即时生效的，比如一次性直接的伤害，没有持续时间  回合数由释放技能的当前回合生效，并计数  非即时生效的技能，会以buff形式，表现在对应人物身上
		DELAY_TIME,
        //varchar;技能效果图标存放路径 注：如果delayTime值不等于0，并且是BB能量技能，那么对应英雄头上会显示有技能效果图标，该字段值为空时，默认没有
		BUFF_ICON,
        //int;值范围【0,99】1：最大HP 2：当前HP 3：攻击 4：防御 5：回复 6：BB能量 7：暴击几率 8：暴击伤害加成 9：合击伤害加成 10：受到伤害 11：受到异常状态几率 12：触发异常状态几率 13：BB掉落几率 14：攻击吸血 15：反弹伤害 16：心的回复量 17：BB能量的回复量 18：中毒 19：不能行动 20：不能使用技能，同时BB能量不会增加 21：攻击附带水属性 22：攻击附带火属性 23：攻击附带木属性 24：攻击附带风属性   25：攻击附带光属性 26：攻击附带暗属性
		EFFECT_OBJECT,
        //int;技能效果数值算法 值范围【0,99】 0：空值（空值情况下，无需用到EffectValue) 1：通常伤害公式算法：effectValue*（攻-防/3） 2：通常治疗公式算法：effectValue*（“施法者回复”+“被施法者回复”+ effectValue1） 1：固定为EffectValue值 2：提高EffectValue% （如果EffectObject是基础属性，则为） 3：提高 EffectValue 4：提高EffectValue%*当前回合数 注：直接伤害类技能的段数攻击形式，读取相应的段数表
		EFFECT_VALUE_TYPE,
        //int;技能效果数值 值范围【-99999,99999】
		EFFECT_VALUE1,
        //int;技能效果数值 值范围【-99999,99999】
		EFFECT_VALUE2,
	}

    //剧情字典表
	public enum DICT_STORY
	{
        //int;剧情ID
		STORY_TYPEID,
        //int;剧情名字ID
		NAME_ID,
        //int;描述ID
		DESCRIPTION_ID,
        //tinyint;1:主线，2:支线
		SORT,
        //tinyint;章节
		CHAPTER,
        //int;剧情触发战役
		BATTLE_ID,
        //int;剧情图片ID
		ICON_ID,
        //int;背景音乐ID
		BG_MUSIC_ID,
	}

    //剧情对话字典表
	public enum DICT_STORY_DIALOG
	{
        //int;剧情ID
		STORY_TYPEID,
        //tinyint;顺序
		ORDER,
        //int;索引人物形象图片
		CHARACTER_ICON_ID,
        //int;索引人物名字
		CHARACTER_NAME_ID,
        //int;对话内容
		CONTENT_STR_ID,
        //int;对话背景图片ID
		CONTENT_ICON_ID,
        //int;对话延迟秒
		DELAY,
        //tinyint;对话开始是否震动0=否1是
		SHAKE,
	}

    //召唤兽类型字典表
	public enum DICT_SUMMON
	{
        //int;
		SUMMON_TYPEID,
        //int;客户端字符串ID
		NAME_ID,
        //int;客户端字符串ID
		DESC_ID,
        //varchar;客户端，相对于ASSET的路径名
		ICON_FILE,
        //varchar;客户端，ICON_FILE中的文件名
		ICON_SPRITE_NAME,
        //varchar;客户端，相对于ASSET的路径名
		FBX_FILE,
        //varchar;客户端，相对于ASSET的路径名
		PORTARAIT,
        //tinyint;星级/阶数
		STAR,
        //tinyint;水火木风 光明 暗影  秘法 暂定只属于一个系
		SERIES,
        //int;出场技能
		SKILL_IN,
        //int;出场技能
		SKILL_IN_NAME,
        //int;出场技能
		SKILL_IN_DESC,
        //int;离场技能
		SKILL_OUT,
        //int;离场技能
		SKILL_OUT_NAME,
        //int;离场技能
		SKILL_OUT_DESC,
        //int;战斗技能
		SKILL_BASE,
        //int;战斗技能
		SKILL_BASE_NAME,
        //int;战斗技能
		SKILL_BASE_DESC,
        //int;被动技能
		SKILL_PASSIVE,
        //int;被动技能
		SKILL_PASSIVE_NAME,
        //int;被动技能
		SKILL_PASSIVE_DESC,
        //int;普通攻击
		SKILL,
        //int;普通攻击
		SKILL_NAME,
        //int;普通攻击
		SKILL_DESC,
        //int;召唤需消耗的能量
		ENERGY,
        //int;解锁所需魂
		SOUL,
        //int;初始生命
		INIT_HP,
        //int;初始攻击
		INIT_ATK,
        //int;初始防御
		INIT_DEF,
        //int;初始回复
		INIT_RECOVER,
        //int;初始暴击
		INIT_VIOLENCE,
        //int;召唤兽继承属性百分比 50代表50%
		INHERIT,
        //int;攻击段数
		FREQUENCY,
        //int;技能消耗AP
		SKILL_AP,
        //int;攻击消耗AP
		ATK_AP,
        //int;防御消耗AP
		DEF_AP,
        //int;升级加点数量
		BASE_HP,
        //int;升级加点数量
		BASE_ATK,
        //int;升级加点数量
		BASE_DEF,
        //int;升级加点数量
		BASE_RECOVER,
        //int;概率值 万分比
		BASE_VIOLENCE,
	}

	public enum DICT_SUMMON_CFG
	{
        //int;不区分阶数的召唤兽类型
		SUMMON_TYPE,
        //int;解锁所需碎片
		WHITE,
        //int;升级到绿色品质需要碎片
		GREEN,
        //int;升级到蓝色品质需要碎片
		BLUE,
        //int;升级到紫色品质需要碎片
		PURPLE,
        //int;HP继承 百分比
		WHITE_HP,
        //int;攻击继承 百分比
		WHITE_ATK,
        //int;防守继承 百分比
		WHITE_DEF,
        //int;回复继承 百分比
		WHITE_RECOVER,
        //int;
		GREEN_HP,
        //int;
		GREEN_ATK,
        //int;
		GREEN_DEF,
        //int;
		GREEN_RECOVER,
        //int;
		BLUE_HP,
        //int;
		BLUE_ATK,
        //int;
		BLUE_DEF,
        //int;
		BLUE_RECOVER,
        //int;
		PURPLE_HP,
        //int;
		PURPLE_ATK,
        //int;
		PURPLE_DEF,
        //int;
		PURPLE_RECOVER,
	}

    //召唤兽进化表
	public enum DICT_SUMMON_EVOLVE
	{
        //int;
		SUMMON_TYPEID,
        //int;
		EVOLVE_TYPEID,
        //int;所需魂数量
		SOUL,
        //int;
		COST_HERO_1,
        //int;0: 不限制等级  999:最高等级
		LVL_1,
        //int;
		COST_HERO_2,
        //int;
		LVL_2,
        //int;
		COST_HERO_3,
        //int;
		LVL_3,
        //int;
		COST_HERO_4,
        //int;
		LVL_4,
        //int;
		COST_HERO_5,
        //int;
		LVL_5,
	}

    //召唤兽升级字典
	public enum DICT_SUMMON_LEVEL
	{
        //int;
		LEVEL,
        //int;
		SOUL,
	}

    //用户升级字典表
	public enum DICT_USER_LEVEL
	{
        //int;
		LEVEL,
        //int;
		EXP,
        //smallint;
		MAX_POWER,
        //smallint;
		MAX_LEADER,
        //smallint;
		MAX_FRIEND,
	}

}
