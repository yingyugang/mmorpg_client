using System.Collections.Generic;
using UnityEngine;

public class GameConstant
{
	public static string userName;

	public static Sprite headerSprite;
	public static Sprite headerFrame;
	public static Sprite headerRate;
    public static int currentHeadImageID;
	//public static GameDetail gameDetail;
	public const int GACHA_RECYCLE_NUMBER_OF_MODE = 5;
	public const int GACHA_RECYCLE_START_ID = 6;
	public static bool hasLogin;
	public static bool hasDownloaded;
	public static bool isPlayingGame;
	public static bool isClearGacha;
	public static string HasCompletedTutorial = "HasCompletedTutorial";
	public static int numOfCard;
    public static int lastNumOfCard;
	//public static EventTypeEnum eventTypeEnum;
	public static int currentEventGameID;
	public static int GameRate;

	public const string UpdateNoticeManager = "UpdateNoticeManager";
	public const string ClearNoticeManager = "ClearNoticeManager";
	public const string UpdateBadgeManager = "UpdateBadgeManager";
	public const string UpdateApRecoveryTime = "UpdateApRecoveryTime";
	public const string EVENT_MAIL_ADDRESS = "oso_party@d777.jp";
	public const string UNKNOWN_IMAGE_NAME = "unknown";
}
