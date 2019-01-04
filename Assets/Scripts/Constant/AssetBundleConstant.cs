using UnityEngine;

public static class AssetBundleConstant
{
    public const string ASSETBUNDLE_BUILD_WINDOW_MENUITEM = "Tools/BlueNoah/AssetBundle/AB Build Manager";
    public const string ASSETBUNDLE_SETTING_WINDOW_MENUITEM = "Tools/BlueNoah/AssetBundle/AB Settings";
    public const string ASSETBUNDLE_ROOT = "";
    public static string ASSETBUNDLE_PATH
    {
        get
        {
            return ASSETBUNDLE_ROOT + "/AssetBundleBuilds/";
        }
    }
    public static string ASSETBUNDLE_RESOURCES_PATH
    {
        get
        {
            return ASSETBUNDLE_ROOT + "/AssetBundleResources/";
        }
    }

    public const string ASSETBUNDLE_UPLOAD_PATH = "/Applications/XAMPP/xamppfiles/htdocs/DownloadSample/";

    public const string CONFIG_FILE = "assetbundle_config.json";

    public static string ASSETBUNDLE_ROOT_PATH
    {
        get
        {
            return Application.dataPath + ASSETBUNDLE_PATH;
        }
    }

    public static string STREAMINGASSET_PLATFORM_PATH{
        get{
            return Application.streamingAssetsPath + "/"  + CommonConstant.PLATFORM + "/";
        }
    }

    public static string ASSETBUNDLE_PLATFORM_PATH
    {
        get
        {
            return ASSETBUNDLE_ROOT_PATH+ CommonConstant.PLATFORM + "/";
        }
    }

    public static string ASSETDATABASE_PLATFORM_PATH
    {
        get
        {
            return "Assets" + ASSETBUNDLE_PATH + CommonConstant.PLATFORM + "/";
        }
    }

    public static string ASSETBUNDLE_PLATFORM_CONFIG_FILE
    {
        get
        {
            return ASSETBUNDLE_PLATFORM_PATH + CONFIG_FILE;
        }
    }

    public static string SYSTEM_ASSETBUNDLE_RESOURCES_PATH
    {
        get
        {
            return Application.dataPath + ASSETBUNDLE_RESOURCES_PATH;
        }
    }

    public static string ASSETDATABASE_ASSETBUNDLE_RESOURCES_PATH
    {
        get
        {
            return "Assets" + ASSETBUNDLE_RESOURCES_PATH;
        }
    }

}
