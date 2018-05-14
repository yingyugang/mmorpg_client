public class APIConstant
{
	public const string PREFIX = "api/";
	public const string SIGNIN = "signin";
	public const string SIGNUP = "signup";
	public const string PLAYER_MIGRATION = "player/migration";//引继
	public const string PLAYER_ISSUE_MIGRATION = "player/issue_migration";//生成code
	public const string PLAYER_UPDATE_NAME = "player/updatename";
	public const string PLAYER_UPDATE_MIGRATION = "player/update_migration";
	public const string PLAYER_UPDATE_HEAD = "player/updatehead";
	public const string PLAYER_RECOVER = "player/recover";
	public const string PLAYER_INFO = "player/info";
	public const string PLAYER_CHANGENAME = "player/updatename";
	public const string PLAYER_FRIEND_LIST = "friend/list";//请求好友列表
	public const string PLAYER_FRIEND_FOLLOW = "friend/follow";//添加或解除follow
	public const string GAME_OPEN = "game/open";
	public const string GAME_START = "game/start";
	public const string GAME_OVER = "game/over";
	public const string GACHA_LIST = "gacha/list";
	public const string GACHA_DO = "gacha/do";
	public const string CHARGE_UPDATE_COIN = "charge/update_coin";
	public const string CHARGE_LIMIT = "charge/limit";
	public const string CHARGE_EXCHANGE = "charge/exchange";
	public const string CHARGE_AD = "charge/rm_ad";
	public const string MISSION_RECEIVE = "mission/receive";
	public const string PRESENT_RECEIVE = "present/receive";
	public const string SHOP_LIMIT = "shop/limit";
//	public const string SHOP_ITEM_EXCHANGE = "shop/item_exchange";
	public const string SHOP_ITEM_LIMIT = "shop/item_list";
	public const string RANKING_LIST = "ranking/list";
	public const string SYSTEM_INFO = "player/systeminfo";
	public const string RECOVER_AD = "player/recover_ad";
	public const string PLAYER_SHARE = "player/share";
	public const string INFORMATION_LIST = "information/list";
	public const string PRESENT_LIST = "present/list";
	public const string RANKING_EVENT = "ranking/event";
	public const string MISSION_EVENT_RECEIVE = "event/receive";

	/**
	2001 maintenance
	2002 app version
	2003 resoure version
	2004 review
	2005 check_last_login 日付变更 重启 
	2006 apitoken
	**/

	public const int ERROR_CODE_MAINTENANCE = 2001;
	public const int ERROR_CODE_APP_VERSION = 2002;
	public const int ERROR_CODE_RESOURCE_VERSION = 2003;
	public const int ERROR_CODE_REVIEW = 2004;
	public const int ERROR_CODE_CHECK_LAST_LOGIN = 2005;//日付变更
	public const int ERROR_CODE_APITOKEN = 2006;
	public const int ERROR_DEVICEID_NOT_EXIST = 1001;//用户deviceid不存在
	public const int ERROR_CODE_SERVER_RESTART = 3000;
	public const int ERROR_CODE_SERVER_MSG = 3001;


//	public const string ERROR_CODE_1001 = "1001";
//	public const string ERROR_CODE_1002 = "1002";
//	public const string ERROR_CODE_1004 = "1004";
//	public const string ERROR_CODE_1111 = "1111";
//	public const string ERROR_CODE_2001 = "2001";
//	public const string ERROR_CODE_2002 = "2002";
//	public const string ERROR_CODE_2004 = "2004";
//	public const string ERROR_CODE_2005 = "2005";
//	public const string ERROR_CODE_2006 = "2006";
//	public const string ERROR_CODE_2007 = "2007";
//	public const string ERROR_CODE_2008 = "2008";
//	public const string ERROR_CODE_2009 = "2009";
//	public const string ERROR_CODE_3000 = "3000";

}
