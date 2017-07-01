using UnityEngine;
using System.Collections;

namespace DataCenter
{
    public enum HERO_SERIES
    {
        HERO_SERIES_WATER = 1,
        HERO_SERIES_FIRE = 2,
        HERO_SERIES_WOOD = 3,
        HERO_SERIES_WIND = 4,
        HERO_SERIES_BRIGHT = 5,
        HERO_SERIES_DARK = 6,
    }

    public enum HERO_GROWUP
    {
        HERO_GROWUP_INVALID = 0,    // 无效类型
        HERO_GROWUP_ATK = 1,        // 攻击
        HERO_GROWUP_DEF = 2,        // 防御
        HERO_GROWUP_HP = 3,         // 体力
        HERO_GROWUP_RECOVER = 4,    // 回复
        HERO_GROWUP_BALANCE = 5,    // 全能   概率万分之五百
    };

    public enum DiamondNewCard
    {
        ONCE, //一次，
        TENS, //十次
    }

    public enum HERO_ATTR
    {
        HERO_ATTR_ID            = 0,            // 英雄ID
        HERO_ATTR_TYPE_ID       = 1,            // 类型ID
        HERO_ATTR_GROWUP        = 2,            // 成长类型
        HERO_ATTR_LEVEL         = 3,            // 英雄等级
        HERO_ATTR_SKILL_LEVEL   = 4,            // 必杀技能等级
        HERO_ATTR_EXP           = 5,            // 英雄经验
        HERO_ATTR_EQUIP_ID      = 6,            // 英雄装备
        HERO_ATTR_COLLECTED     = 7,            // 收藏标志
    }
}