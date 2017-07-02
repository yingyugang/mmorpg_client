using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCenter
{
    public enum MSG_TYPE
    {
        _MSG_CLIENT_ERROR = 0,
        //客户端与服务器通讯消息：登录流程消息
        _MSG_CLIENT_BEGIN = 1000, //客户端与服务器通讯消息开始标志
        _MSG_CLIENT_LOGIN_BEGIN = 1000, //客户端与服务器通讯消息：登录流程消息开始标志
        _MSG_CLIENT_LOGIN_2_LS = _MSG_CLIENT_LOGIN_BEGIN + 1, //客户端向登录服务器请求登录消息(CLIENT->LS)
        _MSG_CLIENT_LOGIN_2_LS_ERROR = _MSG_CLIENT_LOGIN_BEGIN + 2, //登录服务器返回登录失败消息(LS->CLIENT)
        _MSG_CLIENT_LOGIN_RESPONSE = _MSG_CLIENT_LOGIN_BEGIN + 3, //登录回复(GS->CLIENT),(LS->CLIENT)
        _MSG_CLIENT_LOGIN_LS_VALIDATE = _MSG_CLIENT_LOGIN_BEGIN + 4, //登录服务器通知客户端认证生效消息(LS->CLIENT)
        _MSG_CLIENT_LOGIN_2_GS = _MSG_CLIENT_LOGIN_BEGIN + 5, //客户端向游戏服务器请求登录消息(CLIENT->GS)
        _MSG_CLIENT_USERINFO_REQUEST = _MSG_CLIENT_LOGIN_BEGIN + 6, //客户端请求玩家信息(CLIENT->GS)
        _MSG_CLIENT_USERINFO_STAR = _MSG_CLIENT_LOGIN_BEGIN + 7, //游戏服务器通知客户端发送玩家信息开始(GS->CLIENT)
        _MSG_CLIENT_USERINFO_FINISH = _MSG_CLIENT_LOGIN_BEGIN + 8, //游戏服务器通知客户端发送玩家信息完成(GS->CLIENT)
        _MSG_CLIENT_CLIENTINITOVER_REQUEST = _MSG_CLIENT_LOGIN_BEGIN + 9, //客户端通知游戏服务器客户端初始化完成(CLIENT->GS)
        _MSG_CLIENT_SERVER_TIME_EVENT = _MSG_CLIENT_LOGIN_BEGIN +10,  //服务器时间

        _MSG_CLIENT_LOGIN_END = 1049, //客户端与服务器通讯消息：登录流程消息结束标志

        //客户端与服务器通讯消息：通用消息
        _MSG_CLIENT_COMMON_BEGIN = 1050, //客户端与服务器通讯消息：通用消息开始标志
        _MSG_CLIENT_ACTIVE_REQUEST = _MSG_CLIENT_COMMON_BEGIN + 1, //客户端激活包请求(CLIENT->GS) 
        _MSG_CLIENT_ACTIVE_RESPONSE = _MSG_CLIENT_COMMON_BEGIN + 2, //客户端激活包应答(GS->CLIENT) 
        _MSG_CLIENT_ERROR_RETURN = _MSG_CLIENT_COMMON_BEGIN + 3, //服务器返回错误码给客户端(GS->CLIENT)
        _MSG_CLIENT_VERSION_VALID_REQUEST = _MSG_CLIENT_COMMON_BEGIN + 4, //客户端请求游戏版本号(CLIENT->GS)
        _MSG_CLIENT_VERSION_VALID_RESPONSE = _MSG_CLIENT_COMMON_BEGIN + 5, //服务器回复游戏版本验证(GS->CLIENT)

        _MSG_CLIENT_COMMON_END = 1099, //客户端与服务器通讯消息：通用消息结束标志

        //客户端与服务器通讯消息：充值流程消息
        _MSG_CLIENT_CHARGE_BEGIN = 1100, //客户端与服务器通讯消息：充值流程开始标志
        _MSG_CLIENT_CHARGE_SUCCEED_RESPONSE = _MSG_CLIENT_CHARGE_BEGIN + 1, //DBS向玩家发送充值成功信息(DBS->CLIENT)

        _MSG_CLIENT_CHARGE_END = 1129, //客户端与服务器通讯消息：充值流程结束标志
        
        //客户端与服务器通讯消息：其他流程消息(预留)
        _MSG_CLIENT_OTHER_BEGIN = 1130, //客户端与服务器通讯消息：其他流程消息(预留)开始标志
        
        _MSG_CLIENT_OTHER_END = 1199, //客户端与服务器通讯消息：其他流程消息(预留)结束标志
                
        //客户端与服务器通讯消息：业务逻辑(玩家相关)
        _MSG_CLIENT_USER_BEGIN = 1200, //客户端与服务器通讯消息：业务逻辑(玩家相关)开始标志
        _MSG_CLIENT_NEW_USER_REQUEST = _MSG_CLIENT_USER_BEGIN + 1, //新建玩家请求(CLIENT->GS),(GS->US)
        _MSG_CLIENT_NEW_USER_RESPONSE = _MSG_CLIENT_USER_BEGIN + 2, //新建玩家应答(CLIENT->GS),(GS->US)
        _MSG_CLIENT_SEL_MAIN_HERO = _MSG_CLIENT_USER_BEGIN + 3, //选择主角英雄
        _MSG_CLIENT_USER_INFO_EVENT = _MSG_CLIENT_USER_BEGIN + 5, //玩家信息
        _MSG_CLIENT_USER_ATTR_EVENT = _MSG_CLIENT_USER_BEGIN + 6, //玩家属性信息
        _MSG_CLIENT_BUY_HERO_SIZE = _MSG_CLIENT_USER_BEGIN + 7, //购买英雄数量上限
        _MSG_CLIENT_BUY_POWER = _MSG_CLIENT_USER_BEGIN + 8, //购买体力
        _MSG_CLIENT_GET_KEY = _MSG_CLIENT_USER_BEGIN +9,    //领取钥匙

        _MSG_CLIENT_USER_END = 1299, //客户端与服务器通讯消息：业务逻辑(玩家相关)结束标志

        //客户端与服务器通讯消息：业务逻辑(英雄相关)
        _MSG_CLIENT_HERO_BEGIN = 1300, //客户端与服务器通讯消息：业务逻辑(英雄相关)开始标志
        _MSG_CLIENT_HERO_LST_EVENT = _MSG_CLIENT_HERO_BEGIN + 1, //英雄列表信息
        _MSG_CLIENT_DIAMOND_ACQUIRE_HERO = _MSG_CLIENT_HERO_BEGIN + 2, //钻石抽卡
        _MSG_CLIENT_FRIEND_ACQUIRE_HERO = _MSG_CLIENT_HERO_BEGIN + 3, //友情抽卡
        _MSG_CLIENT_HERO_CHANGE_EVENT = _MSG_CLIENT_HERO_BEGIN + 4, //英雄增减消息
        _MSG_CLIENT_HERO_SELL = _MSG_CLIENT_HERO_BEGIN + 5, //出售英雄
        _MSG_CLIENT_HERO_EVOLVE = _MSG_CLIENT_HERO_BEGIN + 6, //英雄进化
        _MSG_CLIENT_HERO_ENHANCE = _MSG_CLIENT_HERO_BEGIN + 7, //英雄强化
        _MSG_CLIENT_HERO_COLLECT = _MSG_CLIENT_HERO_BEGIN + 8, //英雄收藏
        _MSG_CLIENT_HERO_ATTR_EVENT = _MSG_CLIENT_HERO_BEGIN + 9, //英雄属性信息
        _MSG_CLIENT_HERO_FORMATION = _MSG_CLIENT_HERO_BEGIN + 10, //英雄出战设置
        _MSG_CLIENT_HERO_FORMATION_EVENT = _MSG_CLIENT_HERO_BEGIN + 11, //登录下发英雄阵型信息
        _MSG_CLIENT_ACQUIRE_EXP_FAT = _MSG_CLIENT_HERO_BEGIN + 12, //付费抽取经验胖子
        _MSG_CLIENT_PM_TEST_ACQUIRE_HERO = _MSG_CLIENT_HERO_BEGIN + 13, //PM测试抽卡(切记：测试阶段用，上线要关闭)
        _MSG_CLIENT_HERO_FORMATION_TRIAL = _MSG_CLIENT_HERO_BEGIN + 14, //试炼场阵型设置
        _MSG_CLIENT_HERO_FORMATION_ARENA = _MSG_CLIENT_HERO_BEGIN + 15, //竞技场阵型设置

        _MSG_CLIENT_HERO_END = 1399, //客户端与服务器通讯消息：业务逻辑(英雄相关)结束标志
        
        //客户端与服务器通讯消息：业务逻辑(建筑相关)
        _MSG_CLIENT_BUILDING_BEGIN = 1400, //客户端与服务器通讯消息：业务逻辑(建筑相关)开始标志
        _MSG_CLIENT_BUILDING_LIST = _MSG_CLIENT_BUILDING_BEGIN + 1, //建筑列表
        _MSG_CLIENT_BUILDING_UPDATE = _MSG_CLIENT_BUILDING_BEGIN + 2, //建筑更新
        _MSG_CLIENT_BUILDING_UPLEV = _MSG_CLIENT_BUILDING_BEGIN + 3, //建筑升级
        _MSG_CLIENT_BUILDING_LEVY = _MSG_CLIENT_BUILDING_BEGIN + 4, //建筑采集
         _MSG_CLIENT_BUILDING_END                        = 1449, //客户端与服务器通讯消息：业务逻辑(建筑相关)结束标志

        //客户端与服务器通讯消息：业务逻辑(好友相关)
         _MSG_CLIENT_FRIEND_BEGIN = 1450, //客户端与服务器通讯消息：业务逻辑(好友相关)开始标志
         _MSG_CLIENT_FRIEND_LIST = _MSG_CLIENT_FRIEND_BEGIN + 1, //好友列表
         _MSG_CLIENT_FRIEND_UPDATE = _MSG_CLIENT_FRIEND_BEGIN + 2, //好友更新
         _MSG_CLIENT_FRIEND_APPLY = _MSG_CLIENT_FRIEND_BEGIN + 3, //好友申请
         _MSG_CLIENT_FRIEND_REFUSE = _MSG_CLIENT_FRIEND_BEGIN + 4, //拒绝好友申请
         _MSG_CLIENT_FRIEND_ACCEPT = _MSG_CLIENT_FRIEND_BEGIN + 5, //接受好友申请
         _MSG_CLIENT_FRIEND_DEL = _MSG_CLIENT_FRIEND_BEGIN + 6, //删除好友
         _MSG_CLIENT_SEARCH_USER = _MSG_CLIENT_FRIEND_BEGIN + 7, //查找玩家
         _MSG_CLIENT_FRIEND_GIVE_GIFT = _MSG_CLIENT_FRIEND_BEGIN + 8, //赠送礼物
         _MSG_CLIENT_FRIEND_AWARD_GIFT = _MSG_CLIENT_FRIEND_BEGIN + 9, //领取礼物
         _MSG_CLIENT_FRIEND_GIFT_LIST = _MSG_CLIENT_FRIEND_BEGIN + 10,//好友礼物列表
         _MSG_CLIENT_FRIEND_GIFT_DEL = _MSG_CLIENT_FRIEND_BEGIN + 11,//礼物信息删除
         _MSG_CLIENT_FRIEND_GIFT_SET = _MSG_CLIENT_FRIEND_BEGIN + 12,//设置领取礼物类型ID
         _MSG_CLIENT_FRIEND_INFO_DEL = _MSG_CLIENT_FRIEND_BEGIN+13,//好友信息删除
         _MSG_CLIENT_FRIEND_COLLECT = _MSG_CLIENT_FRIEND_BEGIN + 14,//收藏好友
         _MSG_CLIENT_FRIEND_CANCEL_APPLY = _MSG_CLIENT_FRIEND_BEGIN + 15,//取消好友申请
         _MSG_CLIENT_FRIEND_END                          = 1499, //客户端与服务器通讯消息：业务逻辑(好友相关)结束标志
        
        //客户端与服务器通讯消息：业务逻辑(物品相关)
        _MSG_CLIENT_ITEM_BEGIN = 1500, //客户端与服务器通讯消息：业务逻辑(物品相关)开始标志
        _MSG_CLIENT_ITEM_LIST_EVENT = _MSG_CLIENT_ITEM_BEGIN + 1, //物品列表
        _MSG_CLIENT_ITEM_UPDATE_EVENT = _MSG_CLIENT_ITEM_BEGIN + 2, //物品更新
        _MSG_CLIENT_ITEM_DELETE_EVENT = _MSG_CLIENT_ITEM_BEGIN + 3, //物品删除
        _MSG_CLEINT_ITEM_SELL = _MSG_CLIENT_ITEM_BEGIN + 4, //物品出售
        _MSG_CLEINT_SET_BATTLE_IEM = _MSG_CLIENT_ITEM_BEGIN + 5, //设置战斗物品
        _MSG_CLEINT_BUY_ITEM_SIZE = _MSG_CLIENT_ITEM_BEGIN + 6, //购买物品空间
        _MSG_CLEINT_EQUIP_ITEM = _MSG_CLIENT_ITEM_BEGIN + 7, //英雄装备宝玉
        _MSG_CLEINT_UNEQUIP_ITEM = _MSG_CLIENT_ITEM_BEGIN + 8, //英雄卸下宝玉
        _MSG_CLEINT_MAKE_ITEM_BY_FORMULA = _MSG_CLIENT_ITEM_BEGIN + 9, //制作配方物品
        _MSG_CLEINT_USE_BATTLE_IEM = _MSG_CLIENT_ITEM_BEGIN + 10, //使用战斗物品

        _MSG_CLIENT_ITEM_END = 1599, //客户端与服务器通讯消息：业务逻辑(物品相关)结束标志
        
        //客户端与服务器通讯消息：业务逻辑(战斗相关)
        _MSG_CLIENT_BATTLE_BEGIN = 1600, //客户端与服务器通讯消息：业务逻辑(战斗相关)开始标志
        _MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE = _MSG_CLIENT_BATTLE_BEGIN + 1, //PVE普通关卡通关信息
        _MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE = _MSG_CLIENT_BATTLE_BEGIN + 2, //PVE活动关卡通关信息
        _MSG_CLIENT_BATTLE_PVE_START = _MSG_CLIENT_BATTLE_BEGIN + 3, //PVE战斗开始
        _MSG_CLIENT_BATTLE_PVE_FIELD_ENEMY = _MSG_CLIENT_BATTLE_BEGIN + 4, //PVE战斗关卡怪物队伍
        _MSG_CLIENT_BATTLE_PVE_FIELD_MONSTER_CATCH = _MSG_CLIENT_BATTLE_BEGIN + 5, //PVE战斗关卡怪物捕获
        _MSG_CLIENT_BATTLE_PVE_FIELD_DROP_CHESTS = _MSG_CLIENT_BATTLE_BEGIN + 6, //PVE战斗关卡宝箱掉落
        _MSG_CLIENT_BATTLE_PVE_FIELD_DROP_ITEM = _MSG_CLIENT_BATTLE_BEGIN + 7, //PVE战斗关卡素材掉落
        _MSG_CLIENT_BATTLE_PVE_END = _MSG_CLIENT_BATTLE_BEGIN + 8, //PVE战斗结束
        _MSG_CLIENT_BATTLE_PVE_REWARD = _MSG_CLIENT_BATTLE_BEGIN + 9, //PVE战斗奖励
        _MSG_CLIENT_BATTLE_PVE_USER_HELP = _MSG_CLIENT_BATTLE_BEGIN + 10, //PVE战斗玩家帮忙
        
        _MSG_CLIENT_BATTLE_END = 1699, //客户端与服务器通讯消息：业务逻辑(战斗相关)结束标志
        
        //客户端与服务器通讯消息：业务逻辑(竞技场相关)
        _MSG_CLIENT_ARENA_BEGIN = 1700, //客户端与服务器通讯消息：业务逻辑(战斗相关)开始标志
        _MSG_CLIENT_ARENA_BRIEF_INFO_EVENT = _MSG_CLIENT_ARENA_BEGIN + 1, //玩家竞技场简单信息
        _MSG_CLIENT_ARENA_TARGET = _MSG_CLIENT_ARENA_BEGIN + 2, //竞技场目标玩家信息
        _MSG_CLIENT_ARENA_PVP_START = _MSG_CLIENT_ARENA_BEGIN + 3, //请求竞技场战斗开始
        _MSG_CLIENT_ARENA_TARGET_HERO_INFO = _MSG_CLIENT_ARENA_BEGIN + 4, //竞技场目标玩家英雄信息
        _MSG_CLIENT_ARENA_PVP_END = _MSG_CLIENT_ARENA_BEGIN + 5, //请求竞技场战斗结束
        _MSG_CLIENT_ARENA_PVP_REWARD = _MSG_CLIENT_ARENA_BEGIN + 6, //竞技场战斗奖励
        _MSG_CLIENT_ARENA_FRIEND_RANKING = _MSG_CLIENT_ARENA_BEGIN + 7, //竞技场好友排行
        _MSG_CLIENT_ARENA_TOTAL_RANKING = _MSG_CLIENT_ARENA_BEGIN + 8, //竞技场综合排行

        _MSG_CLIENT_ARENA_END = 1799, //客户端与服务器通讯消息：业务逻辑(战斗相关)结束标志

        _MSG_CLIENT_SUMMON_BEGIN = 1800,
        _MSG_CLIENT_SUMMON_LST = _MSG_CLIENT_SUMMON_BEGIN,             // 召唤兽列表
        _MSG_CLIENT_SUMMON_EVOLVE = _MSG_CLIENT_SUMMON_BEGIN + 1,         // 召唤兽进化
        _MSG_CLIENT_SUMMON_CHANGE_EVENT = _MSG_CLIENT_SUMMON_BEGIN + 2,         // 召唤兽增减
        _MSG_CLIENT_SUMMON_UPLVL = _MSG_CLIENT_SUMMON_BEGIN + 3,         // 召唤兽升级
        _MSG_CLIENT_SUMMON_QUALITY = _MSG_CLIENT_SUMMON_BEGIN + 4,         // 召唤兽提升品质
        _MSG_CLIENT_SUMMON_SKILL_UPLVL = _MSG_CLIENT_SUMMON_BEGIN + 5,      //技能升级
        _MSG_CLIENT_SUMMON_UNLOCK = _MSG_CLIENT_SUMMON_BEGIN + 6,           //解锁

        // 客户端与服务器通讯消息：业务逻辑(礼品相关)
        _MSG_CLIENT_PRESENT_BEGIN = 1830,
        _MSG_CLIENT_PRESENT_LST = _MSG_CLIENT_PRESENT_BEGIN,            // 礼品列表
        _MSG_CLIENT_PRESENT_CHANGE_EVENT = _MSG_CLIENT_PRESENT_BEGIN + 1,        // 礼品增减事件
        _MSG_CLIENT_GET_PRESENT = _MSG_CLIENT_PRESENT_BEGIN + 2,        // 领取礼品
        _MSG_CLIENT_PRESENT_END = 1859,

        // 客户端与服务器通讯消息：业务逻辑(聊天相关)
        _MSG_CLIENT_CHAT_BEGIN = 1860,
        _MSG_CLIENT_CHAT_REQUEST = _MSG_CLIENT_CHAT_BEGIN + 1, //聊天请求
        _MSG_CLIENT_CHAT_INFO = _MSG_CLIENT_CHAT_BEGIN + 2, //聊天信息
        _MSG_CLIENT_GET_WORLDCHAT = _MSG_CLIENT_CHAT_BEGIN + 3, //世界聊天内容请求
        _MSG_CLIENT_CHAT_END = 1899,

        //////////////////////////////////////////////////////////////////////////
        // 客户端与服务器通讯消息：业务逻辑(剧情相关)
        _MSG_CLIENT_STORY_BEGIN = 1900,
        _MSG_CLIENT_STORY_LIST = _MSG_CLIENT_STORY_BEGIN + 1, //剧情信息
        _MSG_CLIENT_STORY_UPDAE = _MSG_CLIENT_STORY_BEGIN + 2, //剧情信息更新
        _MSG_CLIENT_STORY_ACTIVATE = _MSG_CLIENT_STORY_BEGIN + 3, //激活新剧情

        _MSG_CLIENT_STORY_END = 1909,

        _MSG_CLIENT_END = 1999, //客户端与服务器通讯消息: 客户端与服务器通讯消息结束标志
    }
}
