using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCenter
{
    public enum EVENT_GROUP
    {
        globalEvent =0,
        mainUI,
        packet,
    }

    public enum EVENT_GLOBAL:int
    {
        //切换场景
        sys_quit,
        sys_chgScene,
        sys_loginSucc,
        sys_loginFailed,

        net_sendData,
        net_connSucc,
        net_connfailed,
        net_disconnt
    }

    public enum EVENT_MAINUI
    {
        showMainUI,
        showHeader,
        showBody,
        showFooter,
        showHero,
        showVillage,
        showTask,
        //
        itemMake,
		itemUpdate,
		itemDelete,
		itemSetBattle,

        userUpdate,
        userLevelUpdate,
        //
        battleStart,
        battleReward,
        battleTask,
        battleActivityTask,
        battleFriend,

		buildUpdate,
        buildLevy,

        //
        friendGetList,
        friendUpdate,
        friendApply,
        friendCollect,
        friendCancelApply,
        friendRefuse,
        friendAccept,
        friendDelete,
        frinedSearch,
        friendSendGift,
        friendAwardGift,
        friendGetGift,
        friendDelGift,
        friendSetGift,

        chatRecvMsg,
        chatRecvWorld,

        summonRecvList,
        summonEvolve,
        summonChange,
        summonUpLv,
        summonQuality,

        presentGetSucc,
        presentCantGet,
        presentUpdate,
        presentRecvList,

        storyUpdate,
        storyActivaty,

        arenaRecvTargetList,
        arenaRecvHeroList,
        arenaStart,
        arenaReward,
        arenaFriendRank,
        arenaTotalRank,
    }
}
