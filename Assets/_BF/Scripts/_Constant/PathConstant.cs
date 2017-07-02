using UnityEngine;
using System.Collections;

public class PathConstant : MonoBehaviour
{
	private static string SERVER_PATH = 
	#if DEVELOP
//		"http://10.10.10.252:3000/";
		"http://183.182.46.212/";
	//"http://52.193.224.73/";
	#elif TEST
		"http://183.182.46.212/";
	





#elif PRODUCT
		"http://183.182.46.212/";
	





#else
		"http://183.182.46.212/";
	#endif

	private static string SERVER_DOWNLOAD_PATH = 
	#if DEVELOP
		"http://183.182.46.212/";
	//"http://52.193.224.73/";
	#elif TEST
		"http://183.182.46.212/";
	





#elif PRODUCT
		"http://183.182.46.212/";
	





#else
		"http://183.182.46.212/";
	#endif
	public static bool isReview = false;
	public static string REVIEW_SERVER_PATH;
	
	public const string CLIENT_COROUTINE_PATH = "frame/prefabs/common/Coroutine";
	public const string CLIENT_SCENES_PATH = "frame/prefabs/scenes/";
	public const string SERVER_CSV = "server.csv";
	public const string SERVER_RESOURCE_CSV = "server_resource.csv";
	public const string CLIENT_CSV = "client.csv";
	public const string CLIENT_RESOURCE_CSV = "client_resource.csv";


	private const string resources_path = "/DownloadResources/";
	private const string assets_path = "assets/";
	private const string version_path = "version/";
	private const string ios_path = "ios/";
	private const string android_path = "android/";
	private const string id_path = "id/";
	private const string uuid = "uuid.txt";
	private const string deviceid = "deviceid.txt";
	public const string csv_path = "csv/";
	public const string json_path = "json/";

	public static string Get_SERVER_PATH {
		get {
			if (!isReview) {
				return SERVER_PATH;
			} else {
				return REVIEW_SERVER_PATH;
			}
		}
	}

	public static string CLIENT_RESOURCES_PATH {
		get {
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
				return Application.persistentDataPath + resources_path;
			} else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) {
				return Application.dataPath + resources_path;
			} else {
				return string.Empty;
			}
		}
	}

	public static string CLIENT_STREAMING_ASSETS_PATH {
		get {
			return Application.streamingAssetsPath + "/" + GetPlatformPath ();
		}
	}

	public static string CLIENT_JSON_PATH {
		get {
			#if READ_LOCAL_CSV
		return Application.streamingAssetsPath + "/" + json_path;
			#endif
			return CLIENT_RESOURCES_PATH + json_path;
		}
	}

	public static string CLIENT_CSV_PATH {
		get {
			#if READ_LOCAL_CSV
			return Application.streamingAssetsPath + "/" + csv_path;
			#endif
			return CLIENT_RESOURCES_PATH + csv_path;
		}
	}

	public static string CLIENT_ASSETS_PATH {
		get {
			return CLIENT_RESOURCES_PATH + assets_path;
		}
	}

	public static string CLIENT_VERSION_PATH {
		get {
			return CLIENT_RESOURCES_PATH + version_path;
		}
	}

	public static string SERVER_VERSION_PATH {
		get {
			if (!isReview) {
				return SERVER_DOWNLOAD_PATH + GetPlatformPath () + version_path;
			} else {
				return REVIEW_SERVER_PATH + GetPlatformPath () + version_path;
			}
		}
	}

	public static string SERVER_ASSETS_PATH {
		get {
			if (!isReview) {
				return SERVER_DOWNLOAD_PATH + GetPlatformPath () + assets_path;
			} else {
				return REVIEW_SERVER_PATH + GetPlatformPath () + assets_path;
			}
		}
	}

	public static string CLIENT_SERVER_VERSION_CSV {
		get {
			return CLIENT_VERSION_PATH + SERVER_CSV;
		}
	}

	public static string CLIENT_SERVER_RESOURCE_VERSION_CSV {
		get {
			return CLIENT_VERSION_PATH + SERVER_RESOURCE_CSV;
		}
	}

	public static string CLIENT_CLIENT_VERSION_CSV {
		get {
			return CLIENT_VERSION_PATH + CLIENT_CSV;
		}
	}

	public static string CLIENT_CLIENT_RESOURCE_VERSION_CSV {
		get {
			return CLIENT_VERSION_PATH + CLIENT_RESOURCE_CSV;
		}
	}

	public static string UUID {
		get {
			return CLIENT_RESOURCES_PATH + id_path + uuid;
		}
	}

	public static string DEVICEID {
		get {
			return CLIENT_RESOURCES_PATH + id_path + deviceid;
		}
	}

	public static bool CheckIfExistingVersionCSV ()
	{
		if (FileManager.Exists (PathConstant.CLIENT_CLIENT_VERSION_CSV)) {
			return true;
		} else {
			return false;
		}
	}

	private static string GetPlatformPath ()
	{
		string platform_path = ios_path;
		#if UNITY_ANDROID
		platform_path = android_path;
		#endif
		return platform_path;
	}

//	public static string GetPathFromDownloadingFileType (DownloadingFileTypeEnum downloadingFileTypeEnum, bool isServer)
//	{
//		if (isServer) {
//			if (downloadingFileTypeEnum == DownloadingFileTypeEnum.CSV) {
//				return SERVER_VERSION_PATH;
//			} else if (downloadingFileTypeEnum == DownloadingFileTypeEnum.Assets) {
//				return SERVER_ASSETS_PATH;
//			}
//		} else {
//			if (downloadingFileTypeEnum == DownloadingFileTypeEnum.CSV) {
//				return CLIENT_VERSION_PATH;
//			} else if (downloadingFileTypeEnum == DownloadingFileTypeEnum.Assets) {
//				return CLIENT_ASSETS_PATH;
//			}
//		}
//		return null;
//	}
}
