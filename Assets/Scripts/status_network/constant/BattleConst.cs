using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleConst
{
	public enum ElementType
	{
		red = 1,
		green = 2,
		blue = 3,
		white = 4,
		black = 5,
		none = 0,
		//斬
		cut = 6,
		//壊
		destroy = 7,
		//突
		bulge = 8}
	;

	public enum CalculationResultType
	{
		IsCrit,
		IsDefence,
		IsMiss,
		IsRelation,
		IsDefIgnore,
		IsDead,
		//奖励攻击点数
		IsActionPoint,
		RealDamage,
		RealHealth,
		Attribute,
		AttributeValue
	}

	public const int FRAME_RATE = 60;

	public struct BattleSkillEffectTypeConst
	{
		//血量
		public const int HP = 1001;
		//治疗
		public const int Heal = 1002;
		//最大血量
		public const int MaxHP = 1;
		//EN
		public const int EN = 17;
		//Max EN
		public const int MaxEN = 18;
		//暴击率
		public const int CritOdds = 2;
		//伤害减免百分比
		public const int DamageReduceMuilt = 9;
		//元素相克加成百分比
		public const int ElementMuilt = 5;
		//技能释放几率
		public const int SkillOdds = 6;
		//闪避几率(1-100)
		public const int DODGE = 7;
		//防御几率
		public const int DefenceOdds = 8;
		//吸血
		public const int SuckBlood = 2009;
		public const int Element = 14;
		public const int ATKMulti = 2001;
		public const int DEFMulti = 2002;
		public const int MATMulti = 2003;
		public const int MDFMulti = 2004;
		public const int HITMulti = 2005;
//				public const int LUKMulti = 32;
		public const int TECMulti = 2006;
		public const int SPDMulti = 2007;
		//忽略物理防御
		public const int DEFIgnore = 2010;
		//忽略魔法防御
		public const int MDFIgnore = 2011;
		//反击机率
		public const int CounterMulti = 2008;

		public const int ElementAdd = 2015;
		//基础伤害
		public const int ATK = 15;
		//基础防御
		public const int DEF = 16;
		public const int MAT = 19;
		public const int MDF = 20;
		public const int HIT = 21;
		public const int LUK = 22;
		public const int TEC = 24;
		//影响攻击顺序
		public const int SPD = 11;
		//攻击次数
		public const int ActionPoint = 2013;
		//奖励攻击几率
		public const int ActionPointAdd = 2014;
		//无法使用伤害技能(can't use damage skill)
		public const int UnAttack = 3001;
		//只能进行普通攻击(can't use skill)
		public const int UnSkill = 3002;
		//跳过回合(can't do anything)
		public const int UnTurn = 3003;
		//麻痹
		public const int UnTurnMulti = 3004;
		//净化
		public const int StatusClear = 3005;
		//清除debuff
		public const int DebuffClear = 3006;
		//清除buff
		public const int BuffClear = 3007;
		//立即死亡
		public const int Dead = 2012;
	}

	public struct BattleSkillEffectTypeGroupConst
	{
		public const int Heal = 1;
		public const int DebuffClear = 2;
		public const int Buff = 3;
		public const int Debuff = 4;
		public const int Status = 5;
		public const int BuffClear = 6;
		public const int Damage = 7;
		public const int StatusClear = 8;
	}

	public struct BattleSkillPriorityConst
	{
		//每个可技能目标数量奖励值
		public const int ImpactCountBonus = 100;
		//目标没有buff时的奖励值
		public const int UnBuffBonus = 500;
		//目标没有debuff时的奖励值
		public const int UnDebuffBonus = 250;
		//目标没有status时的奖励值
		public const int UnStatusBonus = 250;
		//目标有status时的奖励值（TODO，策划案里面没有写）
		public const int StatusBonus = 1000;
		//目标有buff时的奖励值
		public const int BuffBonus = 500;
		//目标有debuff时的奖励值
		public const int DebuffBonus = 500;
	}

	public struct BattleUnitDisposition
	{
		//好战型
		public const int WarLike = 1;
		//厌战型
		public const int WarDislike = 2;
	}

	//enum.ToString的效率低GC高，所以改成string型的常量。
	public struct BattleMachineStatus
	{
//		//戦闘始める
//		public const string	Begin = "Begin";
//		//待て
//		public const string	Waiting = "Waiting";
//		//战斗开始前前演出
//		public const string	Perform = "Perform";
//		//己方攻击
//		public const string	PlayerAttack = "PlayerAttack";
//		//切换到地方攻击
//		public const string	ToggleToEnemy = "ToggleToEnemy";
//		//切换到己方攻击
//		public const string	ToggleToPlayer = "ToggleToPlayer";
//		//敌方攻击
//		public const string	EnemyAttack = "EnemyAttack";
//		//准备攻击
//		public const string	PreAttack = "PreAttack";
//		//攻击
//		public const string	Attack = "Attack";
//		//攻击结束，进入下一回合之前
//		public const string	AttackDone = "AttackDone";
//		public const string	Failure = "Failure";
//		public const string	Win = "Win";
//		public const string	AllTurnEnd = "AllTurnEnd";

		public const string CAST = "cast";
		public const string MOVE = "walk";
		public const string STANDBY = "idle";
		public const string DEATH = "death";
		public const string CHEER = "cheer";
		public const string HIT = "hit";
	}

	public struct UnitMachineStatus
	{
		public const int STANDBY = 1;//idle
		public const int MOVE = 2;//walk
		public const int CAST = 3;//cast
		public const int DEATH = 4;//death
		public const int MOVE_PATROL = 5;//patrol
		public const int RESPAWN = 6;//respawn
		//now are for shooter.
		public const int SQUAT = 7;
		public const int LYING = 8;
		public const int FIRE = 9;
		public const int UNSQUAT = 10;
		public const int UNLYING = 11;
		public const int UNFIRE = 12;
		public const int JUMP = 13;
		public const int RELOAD = 14;
		public const int THROWN = 15;
		public const int MELEE = 16;
	}

	public struct BattleLayers
	{
		public const string GROUND = "Ground";
		public const string UNIT_SHADOW_LAYER = "Shadow";
		public const string BACK_RAY = "Back_Ray";
		public const string UNIT_LAYER = "Character";
		public const string PARTICLE = "Particle";
		public const string SEQUENCE = "Sequence";
		public const string FLASH = "Flash";
		public const string RANG = "Rang";
		public const string SUPER_LAYER = "SuperLayer";
	}

	public struct UnitArtActionType{
		//待機
		public const string cmn_0001 = "cmn_0001";
		//行使
		public const string cmn_0002 = "cmn_0002";
		//前ステップ
		public const string cmn_0003 = "cmn_0003";
		//後ステップ
		public const string cmn_0004 = "cmn_0004";
		//被弾
		public const string cmn_0006 = "cmn_0006";
		//通常攻撃
		public const string atk_0001 = "atk_0001";
		//スキル1
		public const string atk_0101 = "atk_0101";
		//スキル2
		public const string atk_0102 = "atk_0102";
		//スキル3
		public const string atk_0103 = "atk_0103";
		//High level スキル1
		public const string atk_0201 = "atk_0201";
		//High level スキル2
		public const string atk_0202 = "atk_0202";
		//High level スキル3
		public const string atk_0203 = "atk_0203";
	}

	public struct UnitScanSize{
		public static float NORMAL_MONSTAR = 20;
	}

	public struct UnitSpeed{
		public static float NORMAL_PATROL = 2.5f;//MOVE_PATROL
		public static float NORMAL_SPEED = 5;
	}

	public const float BACK_TO_DEFAULT_DELAY = 10;

	public const int CRIT_RATE = 10000;

	public const float CRIT_DEFAULT = 15f;

	public const int BASE_DAMAGE = 50;

	public const float MONSTER_RESPAWN_DELAY = 5;

	public const int DEFAULT_MELLE_SKILL_ID = 18;

	public const int DEFAULT_REMOTE_SKILL_ID = 21;

	public const float MIN_SKILL_RANGE = 2;

	public const float MOBILE_BUTTON_TOGGLE_DURATION = 0.6f;

	public const float DEFAULT_UNSPAWN_DELAY = 3f;

	public const int DEFAULT_EFFECT_ID = 1;

	public const int RESPAWN_EFFECT_ID = 55;

	public const int DEFAULT_BULLET_COUNT = 30;

	public const int DEFAULT_GUN_SHOOT_ID = -1;

	public struct BattleSounds{
		public const string SHOOT = "Sounds/MiniGun";
		public const string RELOAD = "Sounds/Reload";
		public const string EMPTY = "Sounds/Empty";
	}

}
