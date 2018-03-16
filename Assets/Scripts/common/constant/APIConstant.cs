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
	public const string PLAYER_RECOVER = "player/recover";//恢复营养 require :planter id
	public const string PLAYER_INFO = "player/info";
	public const string PLAYER_CHANGENAME = "player/updatename";//改名字 name string
	public const string PLAYER_FRIEND_LIST = "friend/list";//请求好友列表
	public const string PLAYER_FRIEND_FOLLOW = "friend/follow";//添加或解除follow

	public const string SEEDBED_PLANT = "seedbed/plant";//种植 requires :id, type: Integer requires :item, type: String 返回 seedbed_list player_items
	public const string SEEDBED_EVOLUTION = "seedbed/evolution";//点击种球 requires :id, type: Integer 返回 seedbed_list
	public const string SEEDBED_TRANSPLANT = "seedbed/transplant";//移植到花盆 requires :id, type: Integer
	public const string SEEDBED_USEITEM = "seedbed/item";//使用item,requires :id, type: Integer requires :mode, type: Integer 1:使用item（单个）2:全部（广告）返回 seedbed_list
	public const string SEEDBED_SALE = "seedbed/sale";//requires :id, type: Integer  
	public const string PLANTER_REAR = "planter/rear";//requires :id, type: Integer requires :mode, type: Integer requires :value, type: Integer 
	public const string PLANTER_SALE = "planter/sale";//卖花 requires :ids, type: string
	public const string PLANTER_COLOR = "planter/colorchange";//改变花盆颜色 requires :id, type: Integer requires :colorid, type: Integer
	public const string PLANTER_RECOVERHEART = "planter/recoverheart";//恢复植物的心 require :planter id
	public const string PLANTER_ITEM = "planter/item";//对心使用item requires :id, type: Integer requires :mode, type: Integer
	public const string PLANTER_EVOLUTION = "planter/evolution";//进化 requires :id, type: Integer
	public const string PLANTER_COMMUNICATION = "planter/evolution";//communication 开始
	public const string PLANTER_COMMUNICATION_ITEM = "planter/communication/item";//communication item使用
	public const string PLANTER_TODEPOT = "planter/todepot";//移动到保管库 planterid 返回 planterlist和detoplist
	public const string DEPOT_TOPLANTER = "depot/toplanter";//移动到花盆 depotid 返回 planterlist和detoplist
	public const string DEPOT_SALE = "depot/sale";//保管库出售
	public const string DEPOT_COMBINE = "depot/combine";//保管库交配

	public const string PLANTER_INJECT = "planter/injection";//注射
	public const string PLANTER_SLEEP = "planter/sleep";//睡觉
	public const string SHOP_OPEN = "shop/open";//解锁苗床，解锁或者强化花盆 id 
	public const string SHOP_ITEM_EXCHANGE = "shop/item_exchange";//购买item  id
	public const string PLAYER_HELPFUL = "player/helpful";//参数 mode:1 虫1 mode2:虫2 num：钱的数量

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
