using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class PathConstant
{
	private const string DEVICE_ID = "deviceid.txt";

	internal const string SERVER_CSV = "server.csv";
	internal const string SERVER_RESOURCE_CSV = "server_resource.csv";
	internal const string CLIENT_CSV = "client.csv";
	internal const string CLIENT_RESOURCE_CSV = "client_resource.csv";
	public const string CSV = "csv";
	public const string CSV_PATH = "CSV/";

	public const string ASSETBUNDLES = "AssetBundles";
	private const string AB_PATH = "Assetbundles/";
	private const string ASSETBUNDLES_PATH = "AssetBundles/";
	private const string IMAGE_PATH = "DownloadImages/";
	public const string INFORMATION_IMAGE_PATH = "Information/";
	private const string SOUNDS_PATH = "DownloadSounds/";

	private const string RESOURCES_PATH = "Resources/";
	private const string VERSION_PATH = "";
	private const string ID_PATH = "ID/";

	public const string AB_VARIANT = "assetbundle";
	public const string HERO_AB_PATH = "/Prefabs/Heros/";
	public const string HERO_AB_FRONT = "hero_";
	public const string SOLDIER_AB_PATH = "/Prefabs/Soldiers/";
	public const string SOLDIER_AB_FRONT = "soldier_";
	public const string BUILDING_AB_PATH = "/Prefabs/Buildings/";
	public const string BUILDING_AB_FRONT = "building_";
	public const string ALTAS_AB_PATH = "/Altas/";
	public const string ALTAS_AB_FRONT = "altas_";

	public static string SERVER_PATH =
//		#if DEVELOP
//		"http://54.64.2.40/";
//		#elif TEST
//		"http://183.182.46.212/";
//		#elif PRODUCT
//		"http://183.182.46.212/";
//		#else
		#if UNITY_EDITOR
		"http://127.0.0.1/kingofhero/";
		#else
		"http://192.168.10.101/kingofhero/";
		#endif
//		"http://192.168.102.158/kingofhero/";
//		#endif

	public static string SERVER_DOWNLOAD_PATH =
//		#if DEVELOP
//		"http://54.64.2.40/";
//		#elif TEST
//		"http://183.182.46.212/";
//		#elif PRODUCT
//		"http://183.182.46.212/";
//		#else
		#if UNITY_EDITOR
		"http://127.0.0.1/kingofhero/";
		#else
		"http://192.168.10.101/kingofhero/";
		#endif
//		"http://192.168.102.158/kingofhero/";
//		#endif


	public static string CLIENT_PATH {
		get {
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
			return Application.dataPath;
			#else
			return Application.persistentDataPath;
			#endif
		}
	}

	public static string CLIENT_AB_PATH {
		get {
			return Path.Combine (CLIENT_PATH, AB_PATH);
		}
	}

	public static string CLIENT_RESOURCES_PATH {
		get {
			return Path.Combine (CLIENT_PATH, RESOURCES_PATH);
		}
	}

	public static string CLIENT_STREAMING_ASSETS_PATH {
		get {
			return Application.streamingAssetsPath;
		}
	}

	public static string CLIENT_CSV_PATH {
		get {
			return Path.Combine (CLIENT_RESOURCES_PATH, CSV_PATH);
		}
	}

	public static string CLIENT_ASSETBUNDLES_PATH {
		get {
			return Path.Combine (CLIENT_PATH,Path.Combine ( AB_PATH, SystemConstant.GetPlatformName ()));
		}
	}

	public static string CLIENT_IMAGES_PATH {
		get {
		return Path.Combine (CLIENT_PATH, IMAGE_PATH);
		}
	}

	public static string CLIENT_SOUNDS_PATH {
		get {
		return Path.Combine (CLIENT_PATH, SOUNDS_PATH);
		}
	}

	public static string CLIENT_VERSION_PATH {
		get {
			return Path.Combine (CLIENT_PATH, Path.Combine (VERSION_PATH, SystemConstant.GetPlatformName ()));
		}
	}

	public static string SERVER_AB_PATH {
		get {
			return Path.Combine (SERVER_DOWNLOAD_PATH, AB_PATH);
		}
	}

	public static string SERVER_VERSION_PATH {
		get {
			return Path.Combine (Path.Combine (SERVER_AB_PATH, SystemConstant.GetPlatformName ()), VERSION_PATH);
		}
	}

	public static string SERVER_ASSETBUNDLES_PATH {
		get {
			return Path.Combine (SERVER_AB_PATH, SystemConstant.GetPlatformName ());
		}
	}

	public static string SERVER_IMAGES_PATH {
		get {
		return Path.Combine (SERVER_DOWNLOAD_PATH, IMAGE_PATH);
		}
	}

	public static string SERVER_SOUNDS_PATH {
		get {
		return Path.Combine (SERVER_DOWNLOAD_PATH, SOUNDS_PATH);
		}
	}

	public static string SERVER_VERSION_CSV {
		get {
			return Path.Combine (SERVER_VERSION_PATH, SERVER_CSV);
		}
	}

	public static string CLIENT_SERVER_VERSION_CSV {
		get {
			return Path.Combine (CLIENT_VERSION_PATH, SERVER_CSV);
		}
	}

	public static string CLIENT_SERVER_RESOURCE_VERSION_CSV {
		get {
			return Path.Combine (CLIENT_VERSION_PATH, SERVER_RESOURCE_CSV);
		}
	}

	public static string CLIENT_CLIENT_VERSION_CSV {
		get {
			return Path.Combine (CLIENT_VERSION_PATH, CLIENT_CSV);
		}
	}

	public static string CLIENT_CLIENT_RESOURCE_VERSION_CSV {
		get {
			return Path.Combine (CLIENT_VERSION_PATH, CLIENT_RESOURCE_CSV);
		}
	}

	public static string DEVICEID {
		get {
			return Path.Combine (Path.Combine (CLIENT_RESOURCES_PATH, ID_PATH), DEVICE_ID);
		}
	}

	public static bool CheckIfExistingVersionCSV ()
	{
		return FileManager.Exists (CLIENT_CLIENT_VERSION_CSV);
	}
	
	public static string CLIENT_DEVICEID_DIRECTORY
	{
		get 
		{
			return Path.Combine (CLIENT_RESOURCES_PATH, ID_PATH);
		}
	}
}
