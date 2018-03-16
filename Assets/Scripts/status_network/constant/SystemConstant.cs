using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.iOS;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SystemConstant
{
    private static string A_DEVICE_ID = "a_device_id";
    public const string CLIENT_VERSION = "0.6.0";

	#if UNITY_EDITOR
    public const string STORE_URL = "https://www.google.co.jp";
	#elif UNITY_IPHONE
	public const string STORE_URL = "itms-apps://itunes.apple.com/app/id";
	#elif UNITY_ANDROID
	public const string STORE_URL = "market://details?id=jp.co.d2cr.koekatsu";
	#endif

    private static string hashCode;
    private static string deviceid;
    private static string userAgent;

    public const int DisplayWidth = 1080;
    public const int DisplayHeight = 1920;

	public const string REPRO_SDK_TOKEN = "21ee7f25-c266-40b4-8a1b-8195db986581";
	public const string ANDROID_SENDER_ID = "74748458072";


    public static string HashCodeOfVersionFile
    {
        get
        {
            if (FileManager.Exists(PathConstant.CLIENT_CLIENT_VERSION_CSV))
            {
                hashCode = FileManager.GetFileHash(PathConstant.CLIENT_CLIENT_VERSION_CSV);
                return hashCode;
            }
            return null;
        }
    }

    public static string HashCodeOfResourceVersionFile
    {
        get
        {
            if (FileManager.Exists(PathConstant.CLIENT_CLIENT_RESOURCE_VERSION_CSV))
            {
                hashCode = FileManager.GetFileHash(PathConstant.CLIENT_CLIENT_RESOURCE_VERSION_CSV);
                return hashCode;
            }
            return null;
        }
    }

    public static string P_CODE
    {
        get
        {
            return PlayerPrefs.GetString("P_CODE");
        }
        set
        {
            if(!String.IsNullOrEmpty(P_CODE))
                return;
            PlayerPrefs.SetString("P_CODE", value);
            PlayerPrefs.Save();
        }
    }

    public static string DeviceID
    {
        get
        {
            if (FileManager.Exists(PathConstant.DEVICEID))
            {
                deviceid = FileManager.ReadString(PathConstant.DEVICEID);
	/*
                if (string.IsNullOrEmpty(deviceid))
                {
                    //deviceid = Alternative_DeviceID;
                    SetDeviceID(deviceid);
                }
                else
                {
                    //Alternative_DeviceID = deviceid;
                }
	*/
            }
            else
            {
                //deviceid = Alternative_DeviceID;
                //SetDeviceID(deviceid);
            }
            return deviceid;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                //Alternative_DeviceID = value;
                SetDeviceID(value);
            }
        }
    }

    private static void SetDeviceID(string deviceid)
    {
        FileManager.WriteString(PathConstant.DEVICEID, deviceid);
    }

    public static void ClearDeviceID()
    {
		if (FileManager.DirectoryExists (PathConstant.CLIENT_DEVICEID_DIRECTORY)) 
		{	
			try
			{
				FileManager.DeleteFile (PathConstant.DEVICEID);
				FileManager.DeleteFile (PathConstant.DEVICEID + ".meta");
                		ClearPCode();
			}
			catch (Exception e)
			{
				Debug.LogError("ファイル削除失敗\n");
				Debug.LogError("File IO error: " + e.Message);
			}
		}
        PlayerPrefs.SetString(A_DEVICE_ID, string.Empty);
        FileManager.WriteString(PathConstant.DEVICEID, string.Empty);
    }

    private static void ClearPCode()
    {
        PlayerPrefs.DeleteKey("P_CODE");
        PlayerPrefs.Save();
    }
/*
    public static string Alternative_DeviceID
    {
        get
        {
            return "";
            return PlayerPrefs.GetString(A_DEVICE_ID, string.Empty);
        }
        private set
        {
            PlayerPrefs.SetString(A_DEVICE_ID, value);
        }
    }
*/
    public static string UserAgent
    {
        get
        {
            if (userAgent != null)
            {
                return userAgent;
            }
            Dictionary<string, string> systemInformation = SystemInformation;
            foreach (string key in systemInformation.Keys)
            {
                userAgent += key + ":" + systemInformation[key] + "||";
            }
            return userAgent;
        }
    }

    private static Dictionary<string, string> SystemInformation
    {
        get
        {
            Dictionary<string, string> systemInformation = new Dictionary<string, string>();
            systemInformation.Add("operatingSystem", SystemInfo.operatingSystem);
            systemInformation.Add("deviceModel", SystemInfo.deviceModel);
            systemInformation.Add("graphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
            systemInformation.Add("graphicsMemorySize", SystemInfo.graphicsMemorySize.ToString());
            systemInformation.Add("systemMemorySize", SystemInfo.systemMemorySize.ToString());

#if UNITY_IOS
            systemInformation.Add("iPhone.generation", Device.generation.ToString());
#endif

            return systemInformation;
        }
    }

	public static string GetPlatformName ()
	{
		#if UNITY_EDITOR
		return GetPlatformForAssetBundles (EditorUserBuildSettings.activeBuildTarget);
		#else
		return GetPlatformForAssetBundles (Application.platform);
		#endif
	}

	#if UNITY_EDITOR
	private static string GetPlatformForAssetBundles (BuildTarget target)
	{
		switch (target) {
		case BuildTarget.Android:
			return "android";

		case BuildTarget.iOS:
			return "ios";

		case BuildTarget.WebGL:
			return "webgl";

		default:
			return null;
		}
	}
	#endif

	private static string GetPlatformForAssetBundles (RuntimePlatform platform)
	{
		switch (platform) {
		case RuntimePlatform.Android:
		return "android";

		case RuntimePlatform.IPhonePlayer:
		return "ios";

		case RuntimePlatform.WebGLPlayer:
		return "webgl";

		default:
			return null;
		}
	}
}
