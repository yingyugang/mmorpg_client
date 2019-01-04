using UnityEngine;

public static class CommonConstant
{
    public static string APP_STORE_VERSION = "0.1.7";

    public static string PLATFORM
    {
        get
        {
#if UNITY_EDITOR
            return EDITOR_PLATFORM;
#else
            return DEPLOY_PLATFORM;
#endif
        }

    }

#if UNITY_EDITOR
    static string EDITOR_PLATFORM
    {
        get
        {
            switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget)
            {
                case UnityEditor.BuildTarget.Android:
                    return "Android";
                case UnityEditor.BuildTarget.iOS:
                    return "iOS";
                case UnityEditor.BuildTarget.WebGL:
                    return "WebGL";
#if UNITY_2018_1_OR_NEWER
                case UnityEditor.BuildTarget.StandaloneOSX:
#else
                case UnityEditor.BuildTarget.StandaloneOSXUniversal:
#endif
                    return "StandardOSX";
                case UnityEditor.BuildTarget.StandaloneWindows:
                    return "StandardWindows";
                default:
                    return "StandardOSX";
            }
        }
    }
#endif

    static string DEPLOY_PLATFORM
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.OSXPlayer:
                    return "StandardOSX";
                case RuntimePlatform.WindowsPlayer:
                    return "StandardWindows";
                default:
                    return "StandardOSX";
            }
        }
    }
}
